using AutoMapper;
using EmployeeManagementNextrek.Models.Dtos;
using EmployeeManagementNextrek.Models;
using EmployeeManagementNextrek.Repositories.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.JsonPatch;

namespace EmployeeManagementNextrek.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkManagementController : ControllerBase
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly  IWorkScheduleRepository _workScheduleRepository;
        private readonly IWorkShiftRepository _workShiftRepository;
        private readonly IScheduleExceptionRepository _scheduleExceptionRepository;
        private readonly IMapper _mapper;
        protected APIResponse _response;


        public WorkManagementController(ILogger<EmployeeController> logger, IScheduleExceptionRepository scheduleExceptionRepository, IWorkShiftRepository workShiftRepository ,IWorkScheduleRepository workScheduleRepository, IMapper mapper)
        {
            _logger = logger;
            _workScheduleRepository = workScheduleRepository;
            _workShiftRepository = workShiftRepository;
            _scheduleExceptionRepository = scheduleExceptionRepository;
            _mapper = mapper;
            _response = new();
        }

        #region WorkSchedule
        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetAllWorkSchedules()
        {
            try
            {
                _logger.LogInformation("Obtener los Agenda");
                IEnumerable<WorkSchedule> schedules = await _workScheduleRepository.GetAll();
                _response.Result = _mapper.Map<IEnumerable<WorkScheduleDto>>(schedules);
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

        [HttpGet("{id:int}", Name = "GetWorkSchedules")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetIdWorkSchedules(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al traer agenda con Id  " + id);
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;

                    return BadRequest(_response);
                }

                var schedule = await _workScheduleRepository.GetById(v => v.ScheduleId == id);
                if (schedule == null)
                {
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<WorkScheduleDto>(schedule);
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
        public async Task<ActionResult<APIResponse>> CreateWorkSchedules([FromBody] WorkScheduleCreateDto createDto)
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

                WorkSchedule modelo = _mapper.Map<WorkSchedule>(createDto);

                //modelo.Fechacreacion = DateTime.Now;
                //modelo.FechaActualizacion = DateTime.Now;
                await _workScheduleRepository.Create(modelo);
                _response.Result = modelo;
                _response.StatusCode = HttpStatusCode.Created;


                return CreatedAtRoute("GetWorkSchedules", new { id = modelo.ScheduleId }, _response);
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
        public async Task<IActionResult> DeleteWorkSchedules(int id)
        {
            try
            {
                if (id == 0)
                {

                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var schedule = await _workScheduleRepository.GetById(v => v.ScheduleId == id);
                if (schedule == null)
                {
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound();
                }
                await _workScheduleRepository.Delete(schedule);

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
        public async Task<IActionResult> updateWorkSchedules(int id, [FromBody] WorkScheduleUpdateDto updateDto)
        {
            try
            {
                if (updateDto == null || id != updateDto.ScheduleId)
                {
                    _response.IsSuccessful = !false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                WorkSchedule modelo = _mapper.Map<WorkSchedule>(updateDto);

                await _workScheduleRepository.UpDate(modelo);
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
        public async Task<IActionResult> updateWorkSchedules(int id, JsonPatchDocument<WorkScheduleUpdateDto> patchDto)
        {
            try
            {
                if (patchDto == null || id == 0)
                {
                    return BadRequest();
                }


                var schedule = await _workScheduleRepository.GetById(v => v.ScheduleId == id, Tracked: false);

                WorkScheduleUpdateDto scheduleDto = _mapper.Map<WorkScheduleUpdateDto>(schedule);

                if (schedule == null) return BadRequest();

                patchDto.ApplyTo(scheduleDto, ModelState);

                if (!ModelState.IsValid)
                {

                    return BadRequest(ModelState);
                }
                WorkSchedule modelo = _mapper.Map<WorkSchedule>(scheduleDto);

                await _workScheduleRepository.UpDate(modelo);
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


        #endregion

        #region Workshift

        [HttpGet("Shifts")]
        public async Task<ActionResult<APIResponse>> GetAllWorkShifts()
        {
            try
            {
                _logger.LogInformation("Obtener los turno");
                IEnumerable<WorkShift> shifts = await _workShiftRepository.GetAll();
                _response.Result = _mapper.Map<IEnumerable<WorkShiftDto>>(shifts);
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

        [HttpGet("{id:int}/Shifts", Name = "GetShifts")]
        public async Task<ActionResult<APIResponse>> GetIdWorkShift(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al traer turno con Id  " + id);
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;

                    return BadRequest(_response);
                }

                var shift = await _workShiftRepository.GetById(v => v.ShiftId == id);
                if (shift == null)
                {
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<WorkShiftDto>(shift);
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

        [HttpPost("{id:int}/Shifts")]
        public async Task<ActionResult<APIResponse>> CreateWorkShift(int id, [FromBody] WorkShiftCreateDto createDto)
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

                WorkShift modelo = _mapper.Map<WorkShift>(createDto);


                await _workShiftRepository.Create(modelo);
                _response.Result = modelo;
                _response.StatusCode = HttpStatusCode.Created;


                return CreatedAtRoute("GetShifts", new { id = modelo.ShiftId }, _response);
            }
            catch (Exception ex)
            {

                _response.IsSuccessful = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPut("{id:int}/Shifts/{shiftId:int}")]
        public async Task<IActionResult> UpdateWorkShift(int id, int shiftId, [FromBody] WorkShiftUpdateDto UpDateDto)
        {
            try
            {
                if (UpDateDto == null || shiftId != UpDateDto.ShiftId)
                {
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                WorkShift shift = _mapper.Map<WorkShift>(UpDateDto);
                await _workShiftRepository.UpDate(shift);
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

        [HttpDelete("{id:int}/Shifts/{shiftId:int}")]
        public async Task<IActionResult> DeleteWorkShift(int id, int shiftId)
        {
            try
            {
                var shift = await _workShiftRepository.GetById(a => a.ShiftId== shiftId && a.ShiftId == id);
                if (shift == null)
                {
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _workShiftRepository.Delete(shift);
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

        [HttpPatch("{id:int}/Shifts/{shiftId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> updateWorkShift(int id, JsonPatchDocument<WorkShiftUpdateDto> patchDto)
        {
            try
            {
                if (patchDto == null || id == 0)
                {
                    return BadRequest();
                }


                var shift = await _workShiftRepository.GetById(v => v.ShiftId == id, Tracked: false);

                WorkShiftUpdateDto shiftDto = _mapper.Map<WorkShiftUpdateDto>(shift);

                if (shift == null) return BadRequest();

                patchDto.ApplyTo(shiftDto, ModelState);

                if (!ModelState.IsValid)
                {

                    return BadRequest(ModelState);
                }
                WorkShift modelo = _mapper.Map<WorkShift>(shiftDto);

                await _workShiftRepository.UpDate(modelo);
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

        #endregion
        #region  ScheduleExceptions

        [HttpGet("ScheduleExceptions")]
        public async Task<ActionResult<APIResponse>> GetAllScheduleExceptions()
        {
            try
            {
                _logger.LogInformation("Obtener exepciones");
                IEnumerable<ScheduleException>exceptions = await _scheduleExceptionRepository.GetAll();
                _response.Result = _mapper.Map<IEnumerable<ScheduleExceptionDto>>(exceptions);
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

        [HttpGet("{id:int}/ScheduleExceptions", Name = "GetScheduleExceptions")]
        public async Task<ActionResult<APIResponse>> GetIdScheduleExceptions(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al traer exepcion con Id  " + id);
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;

                    return BadRequest(_response);
                }

                var exepcion = await _scheduleExceptionRepository.GetById(v => v.ExceptionId == id);
                if (exepcion == null)
                {
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<ScheduleExceptionDto>(exepcion);
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

        [HttpPost("{id:int}/ScheduleExceptions")]
        public async Task<ActionResult<APIResponse>> CreateScheduleExceptions(int id, [FromBody] ScheduleExceptionCreateDto createDto)
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

                ScheduleException modelo = _mapper.Map<ScheduleException>(createDto);


                await _scheduleExceptionRepository.Create(modelo);
                _response.Result = modelo;
                _response.StatusCode = HttpStatusCode.Created;


                return CreatedAtRoute("GetScheduleExceptions", new { id = modelo.ExceptionId }, _response);
            }
            catch (Exception ex)
            {

                _response.IsSuccessful = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPut("{id:int}/ScheduleExceptions/{exceptionId:int}")]
        public async Task<IActionResult> UpdateScheduleExceptions(int id, int exceptionId, [FromBody] ScheduleExceptionUpdateDto UpDateDto)
        {
            try
            {
                if (UpDateDto == null || exceptionId != UpDateDto.ExceptionId)
                {
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                ScheduleException exception = _mapper.Map<ScheduleException>(UpDateDto);
                await _scheduleExceptionRepository.UpDate(exception);
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

        [HttpDelete("{id:int}/ScheduleExceptions/{exceptionId:int}")]
        public async Task<IActionResult> DeleteScheduleExceptions(int id, int exceptionId)
        {
            try
            {
                var exeption = await _scheduleExceptionRepository.GetById(a => a.ExceptionId == exceptionId && a.ExceptionId == id);
                if (exeption == null)
                {
                    _response.IsSuccessful = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                await _scheduleExceptionRepository.Delete(exeption);
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

        [HttpPatch("{id:int}/ScheduleExceptions/{exceptionId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> updateScheduleExceptions(int id, JsonPatchDocument<ScheduleExceptionUpdateDto> patchDto)
        {
            try
            {
                if (patchDto == null || id == 0)
                {
                    return BadRequest();
                }


                var exeption = await _scheduleExceptionRepository.GetById(v => v.ExceptionId == id, Tracked: false);

                ScheduleExceptionUpdateDto exeptionDto = _mapper.Map<ScheduleExceptionUpdateDto>(exeption);

                if (exeption == null) return BadRequest();

                patchDto.ApplyTo(exeptionDto, ModelState);

                if (!ModelState.IsValid)
                {

                    return BadRequest(ModelState);
                }
                ScheduleException modelo = _mapper.Map<ScheduleException>(exeptionDto);

                await _scheduleExceptionRepository.UpDate(modelo);
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

        #endregion
    }
}





