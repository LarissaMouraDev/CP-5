using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MotosScan.Application.DTOs;
using MotosScan.Application.Services;

namespace MotosScan.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class ManutencoesController : ControllerBase
{
    private readonly ManutencaoService _service;

    public ManutencoesController(ManutencaoService service)
    {
        _service = service;
    }

    /// <summary>
    /// Lista todas as manutenções
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ManutencaoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ManutencaoDto>>> ObterTodas()
    {
        var manutencoes = await _service.ObterTodasAsync();
        return Ok(manutencoes);
    }

    /// <summary>
    /// Busca uma manutenção por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ManutencaoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ManutencaoDto>> ObterPorId(string id)
    {
        var manutencao = await _service.ObterPorIdAsync(id);

        if (manutencao == null)
            return NotFound(new { mensagem = "Manutenção não encontrada" });

        return Ok(manutencao);
    }

    /// <summary>
    /// Lista manutenções de uma moto específica
    /// </summary>
    [HttpGet("moto/{motoId}")]
    [ProducesResponseType(typeof(IEnumerable<ManutencaoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ManutencaoDto>>> ObterPorMoto(string motoId)
    {
        var manutencoes = await _service.ObterPorMotoAsync(motoId);
        return Ok(manutencoes);
    }

    /// <summary>
    /// Lista manutenções de um motorista específico
    /// </summary>
    [HttpGet("motorista/{motoristaId}")]
    [ProducesResponseType(typeof(IEnumerable<ManutencaoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ManutencaoDto>>> ObterPorMotorista(string motoristaId)
    {
        var manutencoes = await _service.ObterPorMotoristaAsync(motoristaId);
        return Ok(manutencoes);
    }

    /// <summary>
    /// Lista manutenções pendentes
    /// </summary>
    [HttpGet("pendentes")]
    [ProducesResponseType(typeof(IEnumerable<ManutencaoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ManutencaoDto>>> ObterPendentes()
    {
        var manutencoes = await _service.ObterPendentesAsync();
        return Ok(manutencoes);
    }

    /// <summary>
    /// Cria uma nova manutenção
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ManutencaoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ManutencaoDto>> Criar([FromBody] CriarManutencaoDto dto)
    {
        try
        {
            var manutencao = await _service.CriarAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = manutencao.Id }, manutencao);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    /// <summary>
    /// Inicia uma manutenção
    /// </summary>
    [HttpPost("{id}/iniciar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Iniciar(string id)
    {
        try
        {
            var sucesso = await _service.IniciarAsync(id);

            if (!sucesso)
                return NotFound(new { mensagem = "Manutenção não encontrada" });

            return Ok(new { mensagem = "Manutenção iniciada com sucesso" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    /// <summary>
    /// Conclui uma manutenção
    /// </summary>
    [HttpPost("{id}/concluir")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Concluir(string id)
    {
        try
        {
            var sucesso = await _service.ConcluirAsync(id);

            if (!sucesso)
                return NotFound(new { mensagem = "Manutenção não encontrada" });

            return Ok(new { mensagem = "Manutenção concluída com sucesso" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    /// <summary>
    /// Cancela uma manutenção
    /// </summary>
    [HttpPost("{id}/cancelar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancelar(string id)
    {
        try
        {
            var sucesso = await _service.CancelarAsync(id);

            if (!sucesso)
                return NotFound(new { mensagem = "Manutenção não encontrada" });

            return Ok(new { mensagem = "Manutenção cancelada com sucesso" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    /// <summary>
    /// Remove uma manutenção
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deletar(string id)
    {
        var sucesso = await _service.DeletarAsync(id);

        if (!sucesso)
            return NotFound(new { mensagem = "Manutenção não encontrada" });

        return NoContent();
    }
}