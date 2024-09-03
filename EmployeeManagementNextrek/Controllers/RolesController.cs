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
    public class RolesController : ControllerBase
    {
        private readonly ILogger<RolesController> _logger;
        private readonly IRoleRepository _rollRepository;
        private readonly IEmployeeRoleRepository _employeeRoleRepository;
        private readonly IRolePermissionRepository _roleRepository;
        private readonly IMapper _mapper;
        protected APIResponse _response;


        public RolesController(ILogger<RolesController> logger, IRoleRepository rollRepository, IRolePermissionRepository roleRepository, IEmployeeRoleRepository employeeRoleRepository, IMapper mapper)
        {
            _logger = logger;
            _rollRepository = rollRepository;
            _employeeRoleRepository = employeeRoleRepository;
            _roleRepository = roleRepository;
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

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> updateRole(int id, JsonPatchDocument<RoleUpDateDto> patchDto)
        {
            try
            {
                if (patchDto == null || id == 0)
                {
                    return BadRequest();
                }


                var roll = await _rollRepository.GetById(v => v.RoleID == id, Tracked: false);

                RoleUpDateDto rollDto = _mapper.Map<RoleUpDateDto>(roll);

                if (roll == null) return BadRequest();

                patchDto.ApplyTo(rollDto, ModelState);

                if (!ModelState.IsValid)
                {

                    return BadRequest(ModelState);
                }
                Role modelo = _mapper.Map<Role>(rollDto);

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


        //Asignar roles a empleados


        // POST: api/roles/{roleId}/employees
        [HttpPost("{roleId:int}/employees")]
        public async Task<IActionResult> AssignRoleToEmployee(int roleId, [FromBody] EmployeeRoleCreateDto employeeRoleCreateDto)
        {
            try { 
            var role = await _employeeRoleRepository.GetById(r => r.RoleID == roleId);
            if (role == null)
            {
                _response.IsSuccessful = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            var employeeRole = _mapper.Map<EmployeeRole>(employeeRoleCreateDto);
            employeeRole.RoleID = roleId;
            await _employeeRoleRepository.Create(employeeRole);

            _response.StatusCode = HttpStatusCode.Created;
            return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }

        // DELETE: api/roles/{roleId}/employees/{employeeId}
        [HttpDelete("{roleId:int}/employees/{employeeId:int}")]
        public async Task<IActionResult> DeleteRoleFromEmployee(int roleId, int employeeId)
        {
            try
            {

                var employeeRole = await _employeeRoleRepository.GetByIds(roleId, employeeId);
                if (employeeRole == null)
                {
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound();
                }

                await _employeeRoleRepository.Delete(employeeRole);
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccessful = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }
        }

         




        // POST: api/roles/{roleId}/permissions
        [HttpPost("{roleId:int}/permissions")]
        public async Task<IActionResult> AssignPermissionToRole(int roleId, [FromBody] RolePermissionCreateDto rolePermissionCreateDto)
        {
            try
            {
                var role = await _roleRepository.GetById(r => r.RoleID == roleId);
                if (role == null)
                {
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                var rolePermission = _mapper.Map<RolePermission>(rolePermissionCreateDto);
                rolePermission.RoleID = roleId;
                await _roleRepository.Create(rolePermission);

                _response.StatusCode = HttpStatusCode.Created;
                return Ok(_response);
            }
            catch (Exception ex) {
                _response.IsSuccessful = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
                return StatusCode((int)HttpStatusCode.InternalServerError, _response);
            }

        }

        // DELETE: api/roles/{roleId}/permissions/{permissionId}
        [HttpDelete("{roleId:int}/permissions/{permissionId:int}")]
        public async Task<IActionResult> RevokePermissionFromRole(int roleId, int permissionId)
        {
            var rolePermission = await _roleRepository.GetByIds(roleId, permissionId);
            if (rolePermission == null)
            {
                _response.IsSuccessful = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            await _roleRepository.Delete(rolePermission);
            _response.StatusCode = HttpStatusCode.NoContent;
            return NoContent();
        }


    }
}
