using AutoMapper;
using EmployeeManagementNextrek.Models.Dtos;
using EmployeeManagementNextrek.Models;
using EmployeeManagementNextrek.Repositories.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.JsonPatch;
using System.Reflection.Metadata;

namespace EmployeeManagementNextrek.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly ILogger<DepartmentsController> _logger;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;
        protected APIResponse _response;


        public DepartmentsController(ILogger<DepartmentsController> logger, IDepartmentRepository departmentRepository, IMapper mapper)
        {
            _logger = logger;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAllDepartments()
        {
            try
            {
                _logger.LogInformation("Obtener los departamentos");
                IEnumerable<Department> departments = await _departmentRepository.GetAll();
                _response.Result = _mapper.Map<IEnumerable<DepartmentDto>>(departments);
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

        [HttpGet("{id:int}", Name = "GetDepartment")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetIdDepartment(int id)
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

                var department = await _departmentRepository.GetById(v => v.DepartmentID == id);
                if (department == null)
                {
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<DepartmentDto>(department);
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
        public async Task<ActionResult<APIResponse>> CreateDepartment([FromBody] DepartmentCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (await  _departmentRepository.GetById(v => v.DepartmentName.ToLower() == createDto.DepartmentName.ToLower()) != null)
                {
                    ModelState.AddModelError("NombreExiste", "El departamento ya existe");
                    return BadRequest(ModelState);

                }
                if (createDto == null)
                {
                    return BadRequest(createDto);
                }

                Department modelo = _mapper.Map<Department>(createDto);

               
                await _departmentRepository.Create(modelo);
                _response.Result = modelo;
                _response.StatusCode = HttpStatusCode.Created;


                return CreatedAtRoute("GetDepartment", new { id = modelo.DepartmentID }, _response);
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
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                if (id == 0)
                {

                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var department = await _departmentRepository.GetById(v => v.DepartmentID == id);
                if (department == null)
                {
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound();
                }
                await _departmentRepository.Delete(department);

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
        public async Task<IActionResult> updateDepartment(int id, [FromBody] DepartmentUpDateDto updateDto)
        {
            try
            {
                if (updateDto == null || id != updateDto.DepartmentID)
                {
                    _response.IsSuccessful = !false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                Department modelo = _mapper.Map<Department>(updateDto);

                await  _departmentRepository.UpDate(modelo);
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
        public async Task<IActionResult> updateDepartment(int id, JsonPatchDocument<DepartmentUpDateDto> patchDto)
        {
            try
            {
                if (patchDto == null || id == 0)
                {
                    return BadRequest();
                }


                var department = await _departmentRepository.GetById(v => v.DepartmentID == id, Tracked: false);

                DepartmentUpDateDto departmentDto = _mapper.Map<DepartmentUpDateDto>(department);

                if (department == null) return BadRequest();

                patchDto.ApplyTo(departmentDto, ModelState);

                if (!ModelState.IsValid)
                {

                    return BadRequest(ModelState);
                }
                Department modelo = _mapper.Map<Department>(departmentDto);

                await _departmentRepository.UpDate(modelo);
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

    }
}
