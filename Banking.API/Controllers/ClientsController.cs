using Banking.Service.Dtos;
using Banking.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
namespace Banking.API.Controllers;

[Authorize]
[ApiController]
[Route("/v1/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;
    private readonly ISearchParameterService _searchParameterService;

    public ClientsController(IClientService clientService, ISearchParameterService searchParameterService)
    {
        _clientService = clientService;
        _searchParameterService = searchParameterService;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [SwaggerOperation("CreateClient")]
    public async Task<IActionResult> CreateClientAsync(CreateClientDto newClient, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var result = await _clientService.AddClientAsync(newClient, cancellationToken);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok("Client added successfully");
        }
        catch
        {
            return StatusCode(500, "Internal server error.");
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    [SwaggerOperation("UpdateClient")]
    public async Task<IActionResult> UpdateClientAsync(int id, [FromBody] UpdateClientDto client, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

            var result = await _clientService.UpdateClientAsync(id, client, cancellationToken);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok("Client Updated successfully");
        }

        catch
        {
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpGet("{id}")]
    [SwaggerOperation("GetClientById")]
    public async Task<IActionResult> GetClientByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _clientService.GetClientByIdAsync(id, cancellationToken);
            if (!result.IsSuccess)
            {
                return NotFound(result.Error);
            }
            return Ok(result.Data);
        }
        catch
        {
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpGet]
    [SwaggerOperation("GetAllClients")]
    public async Task<IActionResult> GetAllClientsAsync(CancellationToken cancellationToken)
    {
        try
        {
            var result= await _clientService.GetAllClientAsync(cancellationToken);
            if (!result.IsSuccess)
            {
                return NotFound(result.Error);
            }
            return Ok(result.Data);
        }
        catch
        {
            return StatusCode(500, "Internal server error.");
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    [SwaggerOperation("DeleteClient")]
    public async Task<IActionResult> DeleteClientAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _clientService.DeleteClientAsync(id, cancellationToken);
            if (!result.IsSuccess)
            {
                return NotFound(result.Error);
            }
            return Ok("Client Deleted successfully");
        }
        catch
        {
            return StatusCode(500, "Internal server error.");
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("FilterClients")]
    [SwaggerOperation("FilterClients")]
    public async Task<IActionResult> GetFilteredClientsAsync([FromQuery] ClientQueryDto queryDto)
    {
        try
        {
            var result = await _clientService.GetFilteredClientsAsync(queryDto);

            if (!result.IsSuccess)
            {
                if (result.Error == "Requested page exceeds the available data.")
                {
                    return NotFound(result.Error);
                }
                if (result.Error == "No clients found.")
                {
                    return NotFound(result.Error);
                }
                return BadRequest(result.Error);
            }

            return Ok(result.Data);
        }
        catch
        {
            return StatusCode(500, "Internal server error.");
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("GetSearchSuggestions")]
    [SwaggerOperation("GetSearchSuggestions")]
    public async Task<IActionResult> GetSearchSuggestionsAsync(CancellationToken cancellationToken)
    {
        try
        {
            var result = await _searchParameterService.GetLastThreeParameters(cancellationToken);
            if (!result.IsSuccess)
            {
                return Ok(result.Error);
            }          
            return Ok(result.Data);
        }
        catch 
        {
            return StatusCode(500, "Internal server error.");
        }
    }
}
