using AutoMapper;
using EmployeeManagementNextrek.Models;
using EmployeeManagementNextrek.Models.Dtos;
using EmployeeManagementNextrek.Repositories;
using EmployeeManagementNextrek.Repositories.IRepository;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EmployeeManagementNextrek.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        private readonly ILogger<EmployeeController> _logger;
        private readonly IEmployeeRepository _userRepository;
        private readonly IEmployeeAddressRepository _addressRepository;
        private readonly IEmployeeDocumentRepository _documentRepository;
        private readonly IMapper _mapper;
        protected APIResponse _response;
      

        public EmployeeController(ILogger<EmployeeController> logger,IEmployeeRepository userRepository, IEmployeeDocumentRepository documentRepository, IEmployeeAddressRepository addressRepository, IMapper mapper)
        {
            _logger = logger;
            _addressRepository = addressRepository;
            _documentRepository = documentRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAllEmployees()
        {
            try
            {
                _logger.LogInformation("Obtener los empleados");
                IEnumerable<Employee> employees = await _userRepository.GetAll();
                _response.Result = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.ErrorMessage = new List<string> { ex.Message };
            }
            return _response;
        }

        [HttpGet("{id:int}", Name = "GetEmployes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetIdEmployes(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al traer empleado con Id  " + id);
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;

                    return BadRequest(_response);
                }
                
                var employe = await _userRepository.GetById(v => v.EmployeeID == id);
                if (employe == null)
                {
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<EmployeeDto>(employe);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsSuccessful = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateEmploye([FromBody] EmployeCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (await _userRepository.GetById(v => v.FirstName.ToLower() == createDto.FirstName.ToLower()) != null)
                {
                    ModelState.AddModelError("NombreExiste", "ese empleado ya existe");
                    return BadRequest(ModelState);

                }
                if (createDto == null)
                {
                    return BadRequest(createDto);
                }

                Employee modelo = _mapper.Map<Employee>(createDto);

                //modelo.Fechacreacion = DateTime.Now;
                //modelo.FechaActualizacion = DateTime.Now;
                await _userRepository.Create(modelo);
                _response.Result = modelo;
                _response.StatusCode = HttpStatusCode.Created;


                return CreatedAtRoute("GetEmployes", new { id = modelo.EmployeeID}, _response);
            }
            catch (Exception ex)
            {

                _response.IsSuccessful = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpDelete("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteEmploye(int id)
        {
            try
            {
                if (id == 0)
                {

                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var employe = await _userRepository.GetById(v => v.EmployeeID == id);
                if (employe == null)
                {
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound();
                }
                await _userRepository.Delete(employe);

                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsSuccessful = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };

            }
            return BadRequest(_response);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> updateEmploye(int id, [FromBody] EmployeeUpdateDto updateDto)
        {
            try
            {
                if (updateDto == null || id != updateDto.EmployeeID)
                {
                    _response.IsSuccessful = !false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                Employee modelo = _mapper.Map<Employee>(updateDto);

                await _userRepository.UpDate(modelo);
                _response.StatusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };

            }
            return BadRequest(_response);
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> updateEmployee(int id, JsonPatchDocument<EmployeeUpdateDto> patchDto)
        {
            try
            {
                if (patchDto == null || id == 0)
                {
                    return BadRequest();
                }


                var employe = await _userRepository.GetById(v => v.EmployeeID == id, Tracked: false);

                EmployeeUpdateDto employeDto = _mapper.Map<EmployeeUpdateDto>(employe);

                if (employe == null) return BadRequest();

                patchDto.ApplyTo(employeDto, ModelState);

                if (!ModelState.IsValid)
                {

                    return BadRequest(ModelState);
                }
                Employee modelo = _mapper.Map<Employee>(employeDto);

                await _userRepository.UpDate(modelo);
                _response.StatusCode = HttpStatusCode.NoContent;


                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return BadRequest(_response);
        }





        // Manage employee addresses



        [HttpGet("{id:int}/addresses", Name = "GetAddress")]
        public async Task<ActionResult<APIResponse>> GetIdEmployeeAddresses(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al traer departamento con Id  " + id);
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;

                    return BadRequest(_response);
                }

                var address = await _addressRepository.GetById(v => v.AddressID == id);
                if (address == null)
                {
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<EmployeeAddressDto>(address);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsSuccessful = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost("{id:int}/addresses")]
        public async Task<ActionResult<APIResponse>> CreateEmployeeAddress(int id, [FromBody] EmployeeAddressCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
               
                if (createDto == null)
                {
                    return BadRequest(createDto);
                }

                EmployeeAddress modelo = _mapper.Map<EmployeeAddress>(createDto);


                await _addressRepository.Create(modelo);
                _response.Result = modelo;
                _response.StatusCode = HttpStatusCode.Created;


                return CreatedAtRoute("GetAddress", new { id = modelo.AddressID }, _response);
            }
            catch (Exception ex)
            {

                _response.IsSuccessful = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response;
        }


        [HttpPut("{id:int}/addresses/{addressId:int}")]
        public async Task<IActionResult> UpdateEmployeeAddress(int id, int addressId, [FromBody] EmployeeAddressUpDateDto UpDateDto)
        {
            try
            {
                if (UpDateDto == null || addressId != UpDateDto.AddressID)
                {
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                EmployeeAddress address = _mapper.Map<EmployeeAddress>(UpDateDto);
                await _addressRepository.UpDate(address);
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.ErrorMessage = new List<string> { ex.Message };
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }

        [HttpDelete("{id:int}/addresses/{addressId:int}")]
        public async Task<IActionResult> DeleteEmployeeAddress(int id, int addressId)
        {
            try
            {
                var address = await _addressRepository.GetById(a => a.AddressID == addressId && a.EmployeeID == id);
                if (address == null)
                {
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _addressRepository.Delete(address);
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.ErrorMessage = new List<string> { ex.Message };
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }

        


             // Manage employee documents



        [HttpGet("{id:int}/documents", Name = "GetDocuments")]
        public async Task<ActionResult<APIResponse>> GetEmployeeDocuments(int id)
        {
            try
            {
                var documents = await _documentRepository.GetAll(d => d.EmployeeID == id);
                _response.Result = _mapper.Map<IEnumerable<EmployeeDocumentDto>>(documents);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.ErrorMessage = new List<string> { ex.Message };
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }

        [HttpPost("{id:int}/documents")]
        public async Task<ActionResult<APIResponse>> CreateEmployeeDocument(int id, [FromBody] EmployeeDocumentCreateDto documentDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (documentDto == null)
                {
                    return BadRequest(documentDto);
                }
                EmployeeDocument document = _mapper.Map<EmployeeDocument>(documentDto);
                
                await _documentRepository.Create(document);
                _response.Result = document;
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetDocuments", new { id = document.DocumentID }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.ErrorMessage = new List<string> { ex.Message };
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }

        [HttpPut("{id:int}/documents/{documentId:int}")]
        public async Task<IActionResult> UpdateEmployeeDocument(int id, int documentId, [FromBody] EmployeeDocumentUpdateDto documentDto)
        {
            try
            {
                if (documentDto == null || documentId != documentDto.DocumentID)
                {
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                EmployeeDocument document = _mapper.Map<EmployeeDocument>(documentDto);
                await _documentRepository.UpDate(document);
                _response.StatusCode = HttpStatusCode.NoContent;
                return NoContent();
            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.ErrorMessage = new List<string> { ex.Message };
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }

        [HttpDelete("{id:int}/documents/{documentId:int}")]
        public async Task<IActionResult> DeleteEmployeeDocument(int id, int documentId)
        {
            try
            {
                var document = await _documentRepository.GetById(d => d.DocumentID == documentId && d.EmployeeID == id);
                if (document == null)
                {
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _documentRepository.Delete(document);
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.ErrorMessage = new List<string> { ex.Message };
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }
    }
}

    
