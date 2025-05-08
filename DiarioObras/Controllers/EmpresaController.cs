using APICatalogo.Services;
using AutoMapper;
using DiarioObras.Data.Interfaces;
using DiarioObras.DTOs.EmpreDTOs;
using DiarioObras.DTOs.EmpresaDTOs;
using DiarioObras.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DiarioObras.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmpresaController : ControllerBase
    {
        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmpresaController(IUnitOfWork uof, IMapper mapper, ITokenService tokenService, IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _uof = uof;
            _mapper = mapper;
            _tokenService = tokenService;
            _configuration = configuration;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost("registrar-completo")]
        public async Task<IActionResult> RegistrarComEmpresaEUsuario([FromBody] RegistroCompletoDto dto)
        {
            using var transaction = await _uof.Context.Database.BeginTransactionAsync();

            try
            {
                // 1. Cria a empresa
                var novaEmpresa = new Empresa
                {
                    Nome = dto.NomeEmpresa,
                    Segmento = dto.Segmento,
                    CriadoEm = DateTime.UtcNow,
                };

                novaEmpresa = await _uof.EmpresaRepository.CreateAsync(novaEmpresa);
                await _uof.CommitAsync();

                // 2. Cria o usuário associado
                var novoUsuario = new ApplicationUser
                {
                    Nome = dto.NomeUsuario,
                    Email = dto.Email,
                    UserName = dto.Email,
                    EmpresaId = novaEmpresa.Id,
                    PhoneNumber = dto.PhoneNumber
                };

                var result = await _userManager.CreateAsync(novoUsuario, dto.Password);
                if (!result.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(new { Message = "Erro ao criar usuário", Erros = result.Errors });
                }

                // 3. Gera token
                var claims = new List<Claim>
        {
            new Claim("empresaId", novaEmpresa.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, dto.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

                var token = _tokenService.GenerateAccessToken(claims, _configuration);

                await transaction.CommitAsync();

                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Empresa = new { novaEmpresa.Id, novaEmpresa.Nome }
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Erro ao registrar: {ex.Message}");
            }
        }


        [HttpPost]
        [Route("create-empresa")]
        public async Task<IActionResult> CreateEmpresa([FromBody] CreateEmpresaDTO empresaDto)
        {
            if (empresaDto == null || string.IsNullOrWhiteSpace(empresaDto.Nome))
                return BadRequest(new { Message = "Nome da empresa é obrigatório" });

            try
            {
                // Mapeia o DTO para a entidade Empresa
                var novaEmpresa = _mapper.Map<Empresa>(empresaDto);

                // Chama o repositório para criar a nova empresa de forma assíncrona
                novaEmpresa = await _uof.EmpresaRepository.CreateAsync(novaEmpresa);
                await _uof.CommitAsync();

                // Criação do token para a empresa registrada
                var authClaims = new List<Claim>
                {
                    new Claim("empresaId", novaEmpresa.Id.ToString()), // Id é int
                    new Claim("purpose", "user_registration"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var token = _tokenService.GenerateAccessToken(authClaims, _configuration);

                return Ok(new
                {
                    Empresa = new { Id = novaEmpresa.Id, Nome = novaEmpresa.Nome },
                    RegistrationToken = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo
                });
            }
            catch (Exception ex)
            {
                // Retorna erro caso algo dê errado
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = $"Erro ao criar empresa: {ex.Message}" });
            }
        }
    }
}
