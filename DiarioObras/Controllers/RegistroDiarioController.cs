using AutoMapper;
using DiarioObras.Configurations;
using DiarioObras.Data.Interfaces;
using DiarioObras.DTOs.ObraDTOs;
using DiarioObras.DTOs.RegistroDiarioDTOs;
using DiarioObras.Infra;
using DiarioObras.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;

namespace DiarioObras.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class RegistroDiarioController : ControllerBase
{

    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    public RegistroDiarioController(IUnitOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }

    [HttpGet("get-relatorio-from-obra/{obraId:int}")]
    public ActionResult<IEnumerable<RegistroDiarioDTO>> GetAllFromObra(int obraId)
    {
        var empresaId = User.GetEmpresaId();
        var registroDiarios = _uof.RegistroDiarioRepository.GetAll(r => r.Obra!.EmpresaId == empresaId && r.ObraId == obraId).OrderByDescending(p => p.Data);
        if (registroDiarios is null)
            return NotFound("Obras não encontrada!");

        var registroDiarioDTOs = _mapper.Map<IEnumerable<RegistroDiarioDTO>>(registroDiarios);
        return Ok(registroDiarioDTOs);
    }


    [HttpGet("{id:int}", Name = "ObterRelatorio")]
    public ActionResult<RegistroDiarioDTO> GetById(int id)
    {
        var empresaId = User.GetEmpresaId();
        var registroDiario = _uof.RegistroDiarioRepository.GetById(r => r.Obra!.EmpresaId == empresaId && r.Id == id);
        if (registroDiario is null)
            return NotFound("Obra não encontrada!");

        var registroDiarioDto = _mapper.Map<RegistroDiarioDTO>(registroDiario);
        return Ok(registroDiarioDto);
    }


    [HttpPost]
    public ActionResult<RegistroDiarioDTO> Post(RegistroDiarioDTO registroDiarioDTO)
    {
        if (registroDiarioDTO is null)
            return BadRequest();

        var empresaId = User.GetEmpresaId();
        var registroDiario = _mapper.Map<RegistroDiario>(registroDiarioDTO);

        var novoRegistroDiario = _uof.RegistroDiarioRepository.Create(registroDiario);
        _uof.Commit();

        var novoregistroDiarioDTO = _mapper.Map<RegistroDiarioDTO>(novoRegistroDiario);

        return new CreatedAtRouteResult("ObterRelatorio",
            new { id = novoregistroDiarioDTO.Id }, novoregistroDiarioDTO);
    }

    [HttpPut("{id:int}")]
    public ActionResult<RegistroDiarioDTO> Put(int id, RegistroDiarioDTO registroDiarioDTO)
    {
        if (id != registroDiarioDTO.Id)
            return BadRequest();

        var empresaId = User.GetEmpresaId();
        var registroExistente = _uof.RegistroDiarioRepository.GetById(r => r.Obra!.EmpresaId == empresaId && r.Id == id);
        if (registroExistente is null)
            return NotFound();

        var registroDiario = _mapper.Map<RegistroDiario>(registroDiarioDTO);

        var registroAtualizado = _uof.RegistroDiarioRepository.Update(registroDiario);
        _uof.Commit();

        var registroDtoAtualizada = _mapper.Map<RegistroDiarioDTO>(registroAtualizado);
        return Ok(registroDtoAtualizada);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<RegistroDiarioDTO> Delete(int id)
    {
        var empresaId = User.GetEmpresaId();
        var registroDiario = _uof.RegistroDiarioRepository.GetById(r => r.Obra!.EmpresaId == empresaId && r.Id == id);
        if (registroDiario is null)
        {
            return NotFound("Obra não encontrada");
        }

        var registroDiarioExcluido = _uof.RegistroDiarioRepository.Delete(registroDiario);
        _uof.Commit();

        return Ok(registroDiarioExcluido);
    }

    [HttpGet("relatorios/{idObra}/{idRegistroDiario}/pdf")]
    public IActionResult GerarRelatorioPdf(int idObra, int idRegistroDiario)
    {
        // Buscar o registro diário no banco
        var relatorio = _uof.RegistroDiarioRepository.getRelatorioByObraID(idObra, idRegistroDiario); // ou await se for async

        if (relatorio == null)
            return NotFound();

        // Gerar o documento PDF
        var document = new RegistroDiarioDertaisReport(relatorio);
        var pdf = document.GeneratePdf();

        return File(pdf, "application/pdf", $"relatorio-diario-{idRegistroDiario}.pdf");
    }
    [HttpGet("total")]
    public IActionResult getTotalRelatorio()
    {
        var empresaId = User.GetEmpresaId();
        // Buscar o registro diário no banco
        var relatorio = _uof.RegistroDiarioRepository.getTotalRelatorio(r => r.Obra!.EmpresaId == empresaId); // ou await se for async

        if (relatorio == null)
            return NotFound();


        return Ok(relatorio);
    }




}
