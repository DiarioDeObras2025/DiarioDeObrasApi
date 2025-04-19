using AutoMapper;
using DiarioObras.Configurations;
using DiarioObras.Data.Interfaces;
using DiarioObras.Data.Repositories;
using DiarioObras.DTOs.ObraDTOs;
using DiarioObras.DTOs.RegistroDiarioDTOs;
using DiarioObras.Infra;
using DiarioObras.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;

namespace DiarioObras.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
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
    public async Task<ActionResult<IEnumerable<RegistroDiarioDTO>>> GetAllFromObra(int obraId)
    {
        var empresaId = User.GetEmpresaId();

        var registros = await _uof.RegistroDiarioRepository
            .GetAllAsync(r => r.Obra!.EmpresaId == empresaId && r.ObraId == obraId);

        var ordenados = registros.OrderByDescending(p => p.Data);

        var dto = _mapper.Map<IEnumerable<RegistroDiarioDTO>>(ordenados);
        return Ok(dto);
    }

    [HttpGet("get-relatorio-from-empresa")]
    public async Task<ActionResult<IEnumerable<RegistroDiarioResumoDTO>>> GetAllWithObraByEmpresa()
    {
        var empresaId = User.GetEmpresaId();

        var registros = await _uof.RegistroDiarioRepository.GetAllWithObraByEmpresaAsync(empresaId); // Supondo que você ajustou esse método pra versão async

        if (registros == null)
            return NotFound("Obras não encontradas!");

        var dto = _mapper.Map<IEnumerable<RegistroDiarioResumoDTO>>(registros);
        return Ok(dto);
    }

    [HttpGet("{id:int}", Name = "ObterRelatorio")]
    public async Task<ActionResult<RegistroDiarioDTO>> GetById(int id)
    {
        var empresaId = User.GetEmpresaId();
        var registro = await _uof.RegistroDiarioRepository.GetByIdAsync(r => r.Obra!.EmpresaId == empresaId && r.Id == id);

        if (registro == null)
            return NotFound("Obra não encontrada!");

        var dto = _mapper.Map<RegistroDiarioDTO>(registro);
        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<RegistroDiarioDTO>> Post(RegistroDiarioDTO registroDiarioDTO)
    {
        if (registroDiarioDTO == null)
            return BadRequest();

        var registro = _mapper.Map<RegistroDiario>(registroDiarioDTO);

        var novo = await _uof.RegistroDiarioRepository.CreateAsync(registro);
        await _uof.CommitAsync();

        var dto = _mapper.Map<RegistroDiarioDTO>(novo);

        return CreatedAtRoute("ObterRelatorio", new { id = dto.Id }, dto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<RegistroDiarioDTO>> Put(int id, RegistroDiarioDTO registroDiarioDTO)
    {
        if (id != registroDiarioDTO.Id)
            return BadRequest();

        var empresaId = User.GetEmpresaId();
        var existente = await _uof.RegistroDiarioRepository.GetByIdAsync(r => r.Obra!.EmpresaId == empresaId && r.Id == id);

        if (existente == null)
            return NotFound();

        _mapper.Map(registroDiarioDTO, existente);

        var atualizado = _uof.RegistroDiarioRepository.Update(existente);
        await _uof.CommitAsync();

        var dto = _mapper.Map<RegistroDiarioDTO>(atualizado);
        return Ok(dto);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<RegistroDiarioDTO>> Delete(int id)
    {
        var empresaId = User.GetEmpresaId();
        var registro = await _uof.RegistroDiarioRepository.GetByIdAsync(r => r.Obra!.EmpresaId == empresaId && r.Id == id);

        if (registro == null)
            return NotFound("Obra não encontrada");

        var deletado = _uof.RegistroDiarioRepository.Delete(registro);
        await _uof.CommitAsync();

        return Ok(deletado);
    }

    [HttpGet("relatorios/{idObra}/{idRegistroDiario}/pdf")]
    public IActionResult GerarRelatorioPdf(int idObra, int idRegistroDiario)
    {
        var relatorio = _uof.RegistroDiarioRepository.getRelatorioByObraID(idObra, idRegistroDiario);

        if (relatorio == null)
            return NotFound();

        var document = new RegistroDiarioDertaisReport(relatorio);
        var pdf = document.GeneratePdf();

        return File(pdf, "application/pdf", $"relatorio-diario-{idRegistroDiario}.pdf");
    }

    [HttpGet("total")]
    public async Task<IActionResult> getTotalRelatorio()
    {
        var empresaId = User.GetEmpresaId();
        var total = await _uof.RegistroDiarioRepository.getTotalRelatorioAsync(r => r.Obra!.EmpresaId == empresaId);

        if (total == null)
            return NotFound();

        return Ok(total);
    }


    [HttpPost("obra/{idObra}/registro/{idRegistro}/duplicar")]
    public async Task<IActionResult> DuplicarRelatorio(int idObra, int idRegistro)
    {
        var registroOriginal = _uof.RegistroDiarioRepository.getRelatorioByObraID(idObra, idRegistro);

        if (registroOriginal is null)
            return NotFound("Registro não encontrado.");

        var registroCopia = new RegistroDiario
        {
            Data = DateTime.UtcNow,
            Titulo = registroOriginal.Titulo,
            ObraId = registroOriginal.ObraId,
            Resumo = registroOriginal.Resumo,
            CondicoesClimaticas = registroOriginal.CondicoesClimaticas,
            HorasTrabalhadas = registroOriginal.HorasTrabalhadas,
            Equipamentos = registroOriginal.Equipamentos,
            ConsumoCimento = registroOriginal.ConsumoCimento,
            Etapa = registroOriginal.Etapa,
            PercentualConcluido = registroOriginal.PercentualConcluido,
            AreaExecutada = registroOriginal.AreaExecutada,
            Ocorrencias = registroOriginal.Ocorrencias,
            Temperatura = registroOriginal.Temperatura,
            Precipitacao = registroOriginal.Precipitacao,
            AssinaturaResponsavel = null,
            DataAssinatura = null,
            DataCriacao = DateTime.UtcNow,
            Equipe = new List<MembroEquipe>(),
            Materiais = new List<MaterialUtilizado>()
        };

        // Duplicar Equipe
        foreach (var membro in registroOriginal.Equipe)
        {
            registroCopia.Equipe.Add(new MembroEquipe
            {
                Nome = membro.Nome,
                Cargo = membro.Cargo,
                Observacao = membro.Observacao,
                Terceirizado = membro.Terceirizado,
            });
        }

        // Duplicar Materiais
        foreach (var mat in registroOriginal.Materiais)
        {
            registroCopia.Materiais.Add(new MaterialUtilizado
            {
                Nome = mat.Nome,
                Quantidade = mat.Quantidade,
                Unidade = mat.Unidade
            });
        }

        await _uof.RegistroDiarioRepository.CreateAsync(registroCopia);
        await _uof.CommitAsync();

        var dto = new RegistroDiarioDTO
        {
            Id = registroCopia.Id,
            Titulo = registroCopia.Titulo,
            Resumo = registroCopia.Resumo,
            Data = registroCopia.Data,
            ObraId = registroCopia.ObraId,
            HorasTrabalhadas = registroCopia.HorasTrabalhadas,
            Equipamentos = registroCopia.Equipamentos,
            ConsumoCimento = registroCopia.ConsumoCimento,
            Etapa = registroCopia.Etapa,
            PercentualConcluido = registroCopia.PercentualConcluido,
            AreaExecutada = registroCopia.AreaExecutada,
            Ocorrencias = registroCopia.Ocorrencias,
            Temperatura = registroCopia.Temperatura,
            Precipitacao = registroCopia.Precipitacao,
            Equipe = registroCopia.Equipe.Select(e => new MembroEquipeDTO
            {
                Nome = e.Nome,
                Cargo = e.Cargo,
                Observacao = e.Observacao,
                Terceirizado = e.Terceirizado
            }).ToList(),
            Materiais = registroCopia.Materiais.Select(m => new MaterialUtilizadoDTO
            {
                Nome = m.Nome,
                Quantidade = m.Quantidade,
                Unidade = m.Unidade
            }).ToList()
        };

        return Ok(dto);
    }


}
