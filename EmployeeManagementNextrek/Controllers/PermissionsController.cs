using AutoMapper;
using EmployeeManagementNextrek.Models.Dtos;
using EmployeeManagementNextrek.Models;
using EmployeeManagementNextrek.Repositories.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using EmployeeManagementNextrek.Repositories;
using Microsoft.AspNetCore.JsonPatch;

namespace EmployeeManagementNextrek.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly ILogger<PermissionsController> _logger;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;
        protected APIResponse _response;


        public PermissionsController(ILogger<PermissionsController> logger, IPermissionRepository permissionRepository, IMapper mapper)
        {
            _logger = logger;
            _permissionRepository = permissionRepository;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAllPermission()
        {
            try
            {
                _logger.LogInformation("Obtener los permisos");
                IEnumerable<Permission> permission = await _permissionRepository.GetAll();
                _response.Result = _mapper.Map<IEnumerable<PermissionDto>>(permission);
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

        [HttpGet("{id:int}", Name = "GetPermission")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetIdPermission(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al traer permiso con Id  " + id);
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;

                    return BadRequest(_response);
                }

                var permission = await _permissionRepository.GetById(v => v.PermissionID == id);
                if (permission == null)
                {
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<PermissionDto>(permission);
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
        public async Task<ActionResult<APIResponse>> CreatePermission([FromBody] PermissionCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (await _permissionRepository.GetById(v => v.PermissionName.ToLower() == createDto.PermissionName.ToLower()) != null)
                {
                    ModelState.AddModelError("NombreExiste", "ese permiso ya existe");
                    return BadRequest(ModelState);

                }
                if (createDto == null)
                {
                    return BadRequest(createDto);
                }

                Permission modelo = _mapper.Map<Permission>(createDto);

                //modelo.Fechacreacion = DateTime.Now;
                //modelo.FechaActualizacion = DateTime.Now;
                await _permissionRepository.Create(modelo);
                _response.Result = modelo;
                _response.StatusCode = HttpStatusCode.Created;


                return CreatedAtRoute("GetPermission", new { id = modelo.PermissionID }, _response);
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
        public async Task<IActionResult> DeletePermission(int id)
        {
            try
            {
                if (id == 0)
                {

                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var permission = await _permissionRepository.GetById(v => v.PermissionID == id);
                if (permission == null)
                {
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound();
                }
                await _permissionRepository.Delete(permission);

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
        public async Task<IActionResult> updatePermission(int id, [FromBody] PermissionUpDateDto updateDto)
        {
            try
            {
                if (updateDto == null || id != updateDto.PermissionID)
                {
                    _response.IsSuccessful = !false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
               Permission modelo = _mapper.Map<Permission>(updateDto);

                await _permissionRepository.UpDate(modelo);
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
        public async Task<IActionResult> updatePermission(int id, JsonPatchDocument<PermissionUpDateDto> patchDto)
        {
            try
            {
                if (patchDto == null || id == 0)
                {
                    return BadRequest();
                }


                var permission = await _permissionRepository.GetById(v => v.PermissionID == id, Tracked: false);

                PermissionUpDateDto permissionDto = _mapper.Map<PermissionUpDateDto>(permission);

                if (permission == null) return BadRequest();

                patchDto.ApplyTo(permissionDto, ModelState);

                if (!ModelState.IsValid)
                {

                    return BadRequest(ModelState);
                }
                Permission modelo = _mapper.Map<Permission>(permissionDto);

                await _permissionRepository.UpDate(modelo);
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
