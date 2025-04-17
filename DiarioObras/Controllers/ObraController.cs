using AutoMapper;
using DiarioObras.Configurations;
using DiarioObras.Data.Interfaces;
using DiarioObras.DTOs.ObraDTOs;
using DiarioObras.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace DiarioObras.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class ObraController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;

    public ObraController(IUnitOfWork uof, IMapper mapper)
    {
        _uof = uof;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ObraDTO>>> GetAll()
    {
        var empresaId = User.GetEmpresaId();
        var obras = await _uof.ObraRepository.GetAllAsync(
            o => o.EmpresaId == empresaId,
            o => o.RegistrosDiarios);

        if (obras is null)
            return NotFound("Obras não encontradas!");

        var obrasDto = _mapper.Map<IEnumerable<ObraDTO>>(obras);
        return Ok(obrasDto);
    }

    [HttpGet("{id:int}", Name = "ObterObra")]
    public async Task<ActionResult<ObraDTO>> GetById(int id)
    {
        var empresaId = User.GetEmpresaId();
        var obra = await _uof.ObraRepository.GetByIdAsync(o => o.Id == id && o.EmpresaId == empresaId);
        if (obra is null)
            return NotFound("Obra não encontrada!");

        var obraDto = _mapper.Map<ObraDTO>(obra);
        return Ok(obraDto);
    }

    [HttpPost]
    public async Task<ActionResult<ObraDTO>> Post(ObraDTO obraDto)
    {
        if (obraDto is null)
            return BadRequest();

        var empresaId = User.GetEmpresaId();
        var obra = _mapper.Map<Obra>(obraDto);
        obra.EmpresaId = empresaId;

        var novaObra = await _uof.ObraRepository.CreateAsync(obra);
        await _uof.CommitAsync();

        var novaObraDto = _mapper.Map<ObraDTO>(novaObra);

        return new CreatedAtRouteResult("ObterObra", new { id = novaObraDto.Id }, novaObraDto);
    }

    [HttpPatch("{id}/UpdatePartial")]
    public async Task<ActionResult<ObraDTONome>> Patch(int id, JsonPatchDocument<ObraDTONome> patchObraDTO)
    {
        if (patchObraDTO is null || id <= 0)
            return BadRequest();

        var empresaId = User.GetEmpresaId();
        var obra = await _uof.ObraRepository.GetByIdAsync(o => o.Id == id && o.EmpresaId == empresaId);

        if (obra is null)
            return NotFound();

        var obraDtoNome = _mapper.Map<ObraDTONome>(obra);
        patchObraDTO.ApplyTo(obraDtoNome, ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _mapper.Map(obraDtoNome, obra);

        _uof.ObraRepository.Update(obra);
        await _uof.CommitAsync();

        return Ok(_mapper.Map<ObraDTONome>(obra));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ObraDTO>> Put(int id, ObraDTO obraDto)
    {
        if (id != obraDto.Id)
            return BadRequest();

        var empresaId = User.GetEmpresaId();
        var obraExistente = await _uof.ObraRepository.GetByIdAsync(o => o.Id == id && o.EmpresaId == empresaId);
        if (obraExistente is null)
            return NotFound();

        _mapper.Map(obraDto, obraExistente);

        _uof.ObraRepository.Update(obraExistente);
        await _uof.CommitAsync();

        var obraDtoAtualizada = _mapper.Map<ObraDTO>(obraExistente);
        return Ok(obraDtoAtualizada);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ObraDTO>> Delete(int id)
    {
        var empresaId = User.GetEmpresaId();
        var obra = await _uof.ObraRepository.GetByIdAsync(o => o.Id == id && o.EmpresaId == empresaId);
        if (obra is null)
        {
            return NotFound("Obra não encontrada");
        }

        var obraExcluida = _uof.ObraRepository.Delete(obra);
        await _uof.CommitAsync();

        return Ok(obraExcluida);
    }
}


//[Route("[controller]")]
//[ApiController]
//[Authorize]
//public class ObraController : ControllerBase
//{
//    private readonly IUnitOfWork _uof;
//    private readonly IMapper _mapper;

//    public ObraController(IUnitOfWork uof, IMapper mapper)
//    {
//        _uof = uof;
//        _mapper = mapper;
//    }

//    [HttpGet]
//    public ActionResult<IEnumerable<ObraDTO>> GetAll()
//    {
//        var empresaId = User.GetEmpresaId();
//        var obras = _uof.ObraRepository.GetAll(
//            o => o.EmpresaId == empresaId,
//            o => o.RegistrosDiarios);

//        if (obras is null)
//            return NotFound("Obras não encontradas!");

//        var obrasDto = _mapper.Map<IEnumerable<ObraDTO>>(obras);
//        return Ok(obrasDto);
//    }

//    [HttpGet("{id:int}", Name = "ObterObra")]
//    public ActionResult<ObraDTO> GetById(int id)
//    {
//        var empresaId = User.GetEmpresaId();
//        var obra = _uof.ObraRepository.GetById(o => o.Id == id && o.EmpresaId == empresaId);
//        if (obra is null)
//            return NotFound("Obra não encontrada!");

//        var obraDto = _mapper.Map<ObraDTO>(obra);
//        return Ok(obraDto);
//    }

//    [HttpPost]
//    public ActionResult<ObraDTO> Post(ObraDTO obraDto)
//    {
//        if (obraDto is null)
//            return BadRequest();

//        var empresaId = User.GetEmpresaId();
//        var obra = _mapper.Map<Obra>(obraDto);
//        obra.EmpresaId = empresaId;

//        var novaObra = _uof.ObraRepository.Create(obra);
//        _uof.Commit();

//        var novaObraDto = _mapper.Map<ObraDTO>(novaObra);

//        return new CreatedAtRouteResult("ObterObra",
//            new { id = novaObraDto.Id }, novaObraDto);
//    }

//    [HttpPatch("{id}/UpdatePartial")]
//    public ActionResult<ObraDTONome> Patch(int id, JsonPatchDocument<ObraDTONome> patchObraDTO)
//    {
//        if (patchObraDTO is null || id <= 0)
//            return BadRequest();

//        var empresaId = User.GetEmpresaId();
//        var obra = _uof.ObraRepository.GetById(o => o.Id == id && o.EmpresaId == empresaId);

//        if (obra is null)
//            return NotFound();

//        var obraDtoNome = _mapper.Map<ObraDTONome>(obra);
//        patchObraDTO.ApplyTo(obraDtoNome, ModelState);

//        if (!ModelState.IsValid)
//            return BadRequest(ModelState);

//        _mapper.Map(obraDtoNome, obra);

//        _uof.ObraRepository.Update(obra);
//        _uof.Commit();

//        return Ok(_mapper.Map<ObraDTONome>(obra));
//    }

//    [HttpPut("{id:int}")]
//    public ActionResult<ObraDTO> Put(int id, ObraDTO obraDto)
//    {
//        if (id != obraDto.Id)
//            return BadRequest();

//        var empresaId = User.GetEmpresaId();
//        var obraExistente = _uof.ObraRepository.GetById(o => o.Id == id && o.EmpresaId == empresaId);
//        if (obraExistente is null)
//            return NotFound();

//        _mapper.Map(obraDto, obraExistente);

//        var obraAtualizada = _uof.ObraRepository.Update(obraExistente);
//        _uof.Commit();

//        var obraDtoAtualizada = _mapper.Map<ObraDTO>(obraAtualizada);
//        return Ok(obraDtoAtualizada);
//    }


//    [HttpDelete("{id:int}")]
//    public ActionResult<ObraDTO> Delete(int id)
//    {
//        var empresaId = User.GetEmpresaId();
//        var obra = _uof.ObraRepository.GetById(o => o.Id == id && o.EmpresaId == empresaId);
//        if (obra is null)
//        {
//            return NotFound("Obra não encontrada");
//        }

//        var obraExcluida = _uof.ObraRepository.Delete(obra);
//        _uof.Commit();

//        return Ok(obraExcluida);
//    }
//}
