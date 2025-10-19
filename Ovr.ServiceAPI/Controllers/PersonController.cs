using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ovr.Core.Infrastructures.Loggers.Interfaces;
using Ovr.DaoServices.Interfaces;
using Ovr.Domain.Models;
using Ovr.Domain.Responses;
using System.Net;

[Authorize]
[ApiController]
[Route("[controller]")]
public class PersonController : ControllerBase
{
    private readonly IEventLogger _eventLogger;
    private readonly IPersonService _personService;

    public PersonController(IPersonService personService, IEventLogger eventLogger)
    {
        _personService = personService ?? throw new ArgumentNullException(nameof(personService));
        _eventLogger = eventLogger ?? throw new ArgumentNullException(nameof(eventLogger));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var people = await _personService.GetAll();

            if (people == null || !people.Any())
            {
                _eventLogger.Log("No se encontró información de personas.", "PersonController.GetAll");
                return Ok(new ResponseBase<List<Person>>
                {
                    Code = (int)HttpStatusCode.OK,
                    Message = "No se encontró información de personas.",
                    Data = new List<Person>()
                });
            }

            _eventLogger.Log("Lista de personas obtenida correctamente.", "PersonController.GetAll");
            return Ok(new ResponseBase<List<Person>>
            {
                Code = 200,
                Message = "Personas obtenidas correctamente.",
                Data = people
            });
        }
        catch (Exception ex)
        {
            _eventLogger.LogException(ex, "PersonController.GetAll");
            return StatusCode(500, new ResponseBase<object>
            {
                Code = 500,
                Message = "Error interno al obtener personas.",
                Data = null
            });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        try
        {
            var person = await _personService.GetById(id);

            if (person == null)
            {
                _eventLogger.Log($"Persona con ID {id} no encontrada.", "PersonController.GetById");
                return NotFound(new ResponseBase<object>
                {
                    Code = 404,
                    Message = $"Persona con ID {id} no encontrada.",
                    Data = null
                });
            }

            return Ok(new ResponseBase<Person>
            {
                Code = 200,
                Message = "Persona obtenida correctamente.",
                Data = person
            });
        }
        catch (Exception ex)
        {
            _eventLogger.LogException(ex, "PersonController.GetById");
            return StatusCode(500, new ResponseBase<object>
            {
                Code = 500,
                Message = "Error interno al obtener persona.",
                Data = null
            });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Person request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _eventLogger.Log("Modelo inválido para la creación de persona.", "PersonController.Create");
                return BadRequest(new ResponseBase<object>
                {
                    Code = 400,
                    Message = "Datos inválidos.",
                    Data = ModelState
                });
            }

            var response = await _personService.Add(request);

            if (response.Code == 409)
            {
                _eventLogger.Log($"El nombre '{request.FirstName} {request.LastName}' ya existe.", "PersonController.Create");
                return Conflict(response);
            }

            if (response.Code == 500 || response.Data == null)
            {
                _eventLogger.Log("Error interno al crear la persona.", "PersonController.Create");
                return StatusCode(500, new ResponseBase<object>
                {
                    Code = 500,
                    Message = "Error interno al crear la persona.",
                    Data = null
                });
            }

            _eventLogger.Log($"Persona creada correctamente con ID {response.Data.PersonId}.", "PersonController.Create");

            return CreatedAtAction(nameof(GetById), new { id = response.Data.PersonId }, response);
        }
        catch (Exception ex)
        {
            _eventLogger.LogException(ex, "PersonController.Create");
            return StatusCode(500, new ResponseBase<object>
            {
                Code = 500,
                Message = "Excepción inesperada.",
                Data = null
            });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] Person request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _eventLogger.Log("Modelo inválido para la actualización.", "PersonController.Update");
                return BadRequest(new ResponseBase<object>
                {
                    Code = 400,
                    Message = "Datos inválidos.",
                    Data = ModelState
                });
            }

            if (id != request.PersonId)
            {
                return BadRequest(new ResponseBase<object>
                {
                    Code = 400,
                    Message = "El ID proporcionado no coincide con el ID del registro.",
                    Data = null
                });
            }

            var response = await _personService.Update(request);

            if (response.Code == 500 || response.Data == null)
            {
                return StatusCode(500, new ResponseBase<object>
                {
                    Code = 500,
                    Message = "Error interno al actualizar la persona.",
                    Data = null
                });
            }

            _eventLogger.Log($"Persona con ID {id} actualizada correctamente.", "PersonController.Update");
            return Ok(response);
        }
        catch (Exception ex)
        {
            _eventLogger.LogException(ex, "PersonController.Update");
            return StatusCode(500, new ResponseBase<object>
            {
                Code = 500,
                Message = "Excepción inesperada.",
                Data = null
            });
        }
    }

}
