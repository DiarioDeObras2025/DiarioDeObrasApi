﻿using APICatalogo.Services;
using AutoMapper;
using DiarioObras.Data.Interfaces;
using DiarioObras.DTOs.EmpresaDTOs;
using DiarioObras.Models;
using Microsoft.AspNetCore.Mvc;
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

        public EmpresaController(IUnitOfWork uof, IMapper mapper, ITokenService tokenService, IConfiguration configuration)
        {
            _uof = uof;
            _mapper = mapper;
            _tokenService = tokenService;
            _configuration = configuration;
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
