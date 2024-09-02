using AutoMapper;
using EmployeeManagementNextrek.Models.Dtos;
using EmployeeManagementNextrek.Models;
using EmployeeManagementNextrek.Repositories.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EmployeeManagementNextrek.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly ILogger<RolesController> _logger;
        private readonly IRoleRepository _rollRepository;
        private readonly IMapper _mapper;
        protected APIResponse _response;


        public RolesController(ILogger<RolesController> logger, IRoleRepository rollRepository, IMapper mapper)
        {
            _logger = logger;
            _rollRepository = rollRepository;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAllRoles()
        {
            try
            {
                _logger.LogInformation("Obtener los roles");
                IEnumerable<Role> roles = await _rollRepository.GetAll();
                _response.Result = _mapper.Map<IEnumerable<RoleDto>>(roles);
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

        [HttpGet("{id:int}", Name = "GetRole")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetIdRole(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al traer rol con Id  " + id);
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;

                    return BadRequest(_response);
                }

                var rol = await _rollRepository.GetById(v => v.RoleID == id);
                if (rol == null)
                {
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<RoleDto>(rol);
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
        public async Task<ActionResult<APIResponse>> CreateRole([FromBody] RoleCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (await _rollRepository.GetById(v => v.RoleName.ToLower() == createDto.RoleName.ToLower()) != null)
                {
                    ModelState.AddModelError("NombreExiste", "ese rol ya existe");
                    return BadRequest(ModelState);

                }
                if (createDto == null)
                {
                    return BadRequest(createDto);
                }

                Role modelo = _mapper.Map<Role>(createDto);

                
                await _rollRepository.Create(modelo);
                _response.Result = modelo;
                _response.StatusCode = HttpStatusCode.Created;


                return CreatedAtRoute("GetRole", new { id = modelo.RoleID }, _response);
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
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                if (id == 0)
                {

                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var rol = await _rollRepository.GetById(v => v.RoleID == id);
                if (rol == null)
                {
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound();
                }
                await _rollRepository.Delete(rol);

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
        public async Task<IActionResult> updateRole(int id, [FromBody] RoleUpDateDto updateDto)
        {
            try
            {
                if (updateDto == null || id != updateDto.RoleID)
                {
                    _response.IsSuccessful = !false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                Role modelo = _mapper.Map<Role>(updateDto);

                await _rollRepository.UpDate(modelo);
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
