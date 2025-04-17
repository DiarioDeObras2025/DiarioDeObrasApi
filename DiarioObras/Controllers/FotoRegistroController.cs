using DiarioObras.Data.Interfaces;
using DiarioObras.DTOs.FotoRegistroDTOs;
using DiarioObras.Models;
using DiarioObras.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DiarioObras.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FotoRegistroController : ControllerBase
    {
        private readonly IUnitOfWork _uof;

        public FotoRegistroController(IUnitOfWork uof)
        {
            _uof = uof;
        }

        [HttpPost("{registroDiarioId}")]
        public async Task<IActionResult> UploadFotos(
            int registroDiarioId,
            [FromForm] List<IFormFile> arquivos,
            [FromForm] string metadadosJson,
            [FromServices] S3Service s3Service)
        {
            if (arquivos == null || arquivos.Count == 0)
                return BadRequest("Nenhum arquivo enviado.");

            // Mudança para o método assíncrono GetByIdAsync
            var registroExiste = await _uof.RegistroDiarioRepository.GetByIdAsync(r => r.Id == registroDiarioId);
            if (registroExiste is null)
                return NotFound("Registro diário não encontrado.");

            List<FotoRegistroMetadataDto> metadados;
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                metadados = JsonSerializer.Deserialize<List<FotoRegistroMetadataDto>>(metadadosJson, options);
            }
            catch (JsonException)
            {
                return BadRequest("Metadados em formato inválido.");
            }

            var fotos = new List<FotoRegistro>();

            foreach (var arquivo in arquivos)
            {
                if (arquivo.Length == 0)
                    continue;

                var meta = metadados.FirstOrDefault(m => m.NomeArquivo == arquivo.FileName);
                if (meta == null)
                    return BadRequest($"Metadados não encontrados para o arquivo {arquivo.FileName}.");

                using var stream = arquivo.OpenReadStream();
                var extensao = Path.GetExtension(arquivo.FileName);
                var nomeArquivo = $"fotos/{Guid.NewGuid()}{extensao}";
                var url = await s3Service.UploadArquivoAsync(stream, nomeArquivo, arquivo.ContentType);

                var foto = new FotoRegistro
                {
                    Descricao = meta.Descricao,
                    Categoria = meta.Categoria,
                    CaminhoArquivo = url,
                    RegistroDiarioId = registroDiarioId
                };

                // Usando CreateAsync ao invés de Create
                foto = await _uof.FotoRegistroRepository.CreateAsync(foto);
                fotos.Add(foto);
            }

            // Commit assíncrono para garantir que as mudanças sejam persistidas no banco
            await _uof.CommitAsync();

            return Ok(fotos.Select(f => new
            {
                f.Id,
                f.Descricao,
                f.CaminhoArquivo,
                f.Categoria,
                f.Localizacao
            }));
        }
    }
}
