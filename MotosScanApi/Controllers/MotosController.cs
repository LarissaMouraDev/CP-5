using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MotosScan.Application.DTOs;
using MotosScan.Application.Services;

namespace MotosScan.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class MotosController : ControllerBase
{
    private readonly MotoService _service;

    public MotosController(MotoService service)
    {
        _service = service;
    }

    /// 
    /// Lista todas as motos
    /// 
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable>> ObterTodas()
    {
        var motos = await _service.ObterTodasAsync();
        return Ok(motos);
    }

    /// 
    /// Busca uma moto por ID
    /// 
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(MotoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ObterPorId(string id)
    {
        var moto = await _service.ObterPorIdAsync(id);

        if (moto == null)
            return NotFound(new { mensagem = "Moto não encontrada" });

        return Ok(moto);
    }

    /// 
    /// Busca uma moto por placa
    /// 
    [HttpGet("placa/{placa}")]
    [ProducesResponseType(typeof(MotoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ObterPorPlaca(string placa)
    {
        var moto = await _service.ObterPorPlacaAsync(placa);

        if (moto == null)
            return NotFound(new { mensagem = "Moto não encontrada" });

        return Ok(moto);
    }

    /// 
    /// Cria uma nova moto
    /// 
    [HttpPost]
    [ProducesResponseType(typeof(MotoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Criar([FromBody] CriarMotoDto dto)
    {
        try
        {
            var moto = await _service.CriarAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = moto.Id }, moto);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    /// 
    /// Atualiza uma moto existente
    /// 
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task Atualizar(string id, [FromBody] AtualizarMotoDto dto)
    {
        try
        {
            var sucesso = await _service.AtualizarAsync(id, dto);

            if (!sucesso)
                return NotFound(new { mensagem = "Moto não encontrada" });

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    /// 
    /// Remove uma moto
    /// 
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task Deletar(string id)
    {
        var sucesso = await _service.DeletarAsync(id);

        if (!sucesso)
            return NotFound(new { mensagem = "Moto não encontrada" });

        return NoContent();
    }
}