using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MotosScan.Application.DTOs;
using MotosScan.Application.Services;

namespace MotosScan.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class MotoristasController : ControllerBase
{
    private readonly MotoristaService _service;

    public MotoristasController(MotoristaService service)
    {
        _service = service;
    }

    /// <summary>
    /// Lista todos os motoristas
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MotoristaDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<MotoristaDto>>> ObterTodos()
    {
        var motoristas = await _service.ObterTodosAsync();
        return Ok(motoristas);
    }

    /// <summary>
    /// Busca um motorista por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(MotoristaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MotoristaDto>> ObterPorId(string id)
    {
        var motorista = await _service.ObterPorIdAsync(id);

        if (motorista == null)
            return NotFound(new { mensagem = "Motorista não encontrado" });

        return Ok(motorista);
    }

    /// <summary>
    /// Busca um motorista por CPF
    /// </summary>
    [HttpGet("cpf/{cpf}")]
    [ProducesResponseType(typeof(MotoristaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MotoristaDto>> ObterPorCpf(string cpf)
    {
        var motorista = await _service.ObterPorCpfAsync(cpf);

        if (motorista == null)
            return NotFound(new { mensagem = "Motorista não encontrado" });

        return Ok(motorista);
    }

    /// <summary>
    /// Busca um motorista por CNH
    /// </summary>
    [HttpGet("cnh/{cnh}")]
    [ProducesResponseType(typeof(MotoristaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MotoristaDto>> ObterPorCnh(string cnh)
    {
        var motorista = await _service.ObterPorCnhAsync(cnh);

        if (motorista == null)
            return NotFound(new { mensagem = "Motorista não encontrado" });

        return Ok(motorista);
    }

    /// <summary>
    /// Cria um novo motorista
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(MotoristaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MotoristaDto>> Criar([FromBody] CriarMotoristaDto dto)
    {
        try
        {
            var motorista = await _service.CriarAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = motorista.Id }, motorista);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    /// <summary>
    /// Atualiza um motorista existente
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(string id, [FromBody] AtualizarMotoristaDto dto)
    {
        try
        {
            var sucesso = await _service.AtualizarAsync(id, dto);

            if (!sucesso)
                return NotFound(new { mensagem = "Motorista não encontrado" });

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    /// <summary>
    /// Remove um motorista
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deletar(string id)
    {
        var sucesso = await _service.DeletarAsync(id);

        if (!sucesso)
            return NotFound(new { mensagem = "Motorista não encontrado" });

        return NoContent();
    }

    /// <summary>
    /// Adiciona foto da CNH
    /// </summary>
    [HttpPost("{id}/foto-cnh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AdicionarFotoCnh(string id, [FromForm] IFormFile arquivo)
    {
        if (arquivo == null || arquivo.Length == 0)
            return BadRequest(new { mensagem = "Arquivo não fornecido" });

        // Aqui você implementaria o upload real do arquivo
        // Por enquanto, vamos simular com um caminho
        var caminhoArquivo = $"/uploads/cnh/{id}_{Path.GetFileName(arquivo.FileName)}";

        var sucesso = await _service.AdicionarFotoCnhAsync(id, caminhoArquivo);

        if (!sucesso)
            return NotFound(new { mensagem = "Motorista não encontrado" });

        return Ok(new { mensagem = "Foto da CNH adicionada com sucesso", url = caminhoArquivo });
    }

    /// <summary>
    /// Remove foto da CNH
    /// </summary>
    [HttpDelete("{id}/foto-cnh")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoverFotoCnh(string id)
    {
        var sucesso = await _service.RemoverFotoCnhAsync(id);

        if (!sucesso)
            return NotFound(new { mensagem = "Motorista não encontrado" });

        return NoContent();
    }
}
