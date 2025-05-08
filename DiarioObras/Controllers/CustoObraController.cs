using DiarioObras.Models;
using DiarioObras.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DiarioObras.DTOs.Financeiro;
using DiarioObras.Infra;
using QuestPDF.Fluent;

namespace DiarioObras.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class CustoObraController : ControllerBase
{
    private readonly IUnitOfWork _uof;

    public CustoObraController(IUnitOfWork uof)
    {
        _uof = uof;
    }

    [HttpPost]
    public async Task<IActionResult> CriarCusto([FromBody] CustoObraDTO dto)
    {
        var custo = new CustoObra
        {
            ObraId = dto.ObraId,
            Valor = dto.Valor,
            Descricao = dto.Descricao,
            Categoria = dto.Categoria,
            Data = dto.Data
        };

        await _uof.CustoObraRepository.CreateAsync(custo); // método do Repository<T>
        await _uof.CommitAsync();

        return Ok(new { Message = "Custo adicionado com sucesso!" });
    }

    [HttpGet("obra/{obraId}")]
    public async Task<IActionResult> ListarCustosPorObra(int obraId)
    {
        var custos = await _uof.CustoObraRepository.ListarPorObraAsync(obraId);
        return Ok(custos);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoverCusto(int id)
    {
        var custo = await _uof.CustoObraRepository.GetByIdAsync(c => c.Id == id);
        if (custo == null)
            return NotFound(new { Message = "Custo não encontrado." });

        _uof.CustoObraRepository.Delete(custo);
        await _uof.CommitAsync();

        return Ok(new { Message = "Custo removido com sucesso!" });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarCusto(int id, [FromBody] CustoObraDTO dto)
    {
        var custo = await _uof.CustoObraRepository.GetByIdAsync(c => c.Id == id);
        if (custo == null)
            return NotFound(new { Message = "Custo não encontrado." });

        custo.Valor = dto.Valor;
        custo.Descricao = dto.Descricao;
        custo.Categoria = dto.Categoria;
        custo.Data = dto.Data;

        _uof.CustoObraRepository.Update(custo);
        await _uof.CommitAsync();

        return Ok(new { Message = "Custo atualizado com sucesso!" });
    }

    [HttpGet("financeiro/obra/{idObra}/pdf")]
    public IActionResult GerarRelatorioPdf(int idObra)
    {
        var relatorio = _uof.CustoObraRepository.getRelatorioByObraID(idObra);

        if (relatorio == null)
            return NotFound();

        var document = new RelatorioFinanceiroObraReport(relatorio);
        var pdf = document.GeneratePdf();

        return File(pdf, "application/pdf", $"relatorio-diario-{idObra}.pdf");
    }

}
