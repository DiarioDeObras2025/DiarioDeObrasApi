using Moq;
using DiarioObras.Controllers;
using DiarioObras.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using DiarioObras.DTOs.EmpresaDTOs;
using DiarioObras.Data.Interfaces;
using APICatalogo.Services;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace DiarioObras.Tests.Controllers
{
    public class EmpresaControllerTests
    {
        private readonly Mock<IUnitOfWork> _mockUof;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly EmpresaController _controller;
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;

        public EmpresaControllerTests()
        {
            _mockUof = new Mock<IUnitOfWork>();
            _mockTokenService = new Mock<ITokenService>();
            _mockMapper = new Mock<IMapper>();
            _mockConfig = new Mock<IConfiguration>();

            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            _controller = new EmpresaController(
                _mockUof.Object,
                _mockMapper.Object,
                _mockTokenService.Object,
                _mockConfig.Object,
                _mockUserManager.Object
            );
        }

        [Fact]
        public async Task CreateEmpresa_DeveRetornarBadRequest_QuandoNomeForInvalido()
        {
            // Arrange
            var dto = new CreateEmpresaDTO { Nome = "" };

            // Act
            var result = await _controller.CreateEmpresa(dto);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Nome da empresa", badRequest.Value.ToString());
        }

        [Fact]
        public async Task CreateEmpresa_DeveRetornarOk_QuandoEmpresaForCriada()
        {
            // Arrange
            var dto = new CreateEmpresaDTO
            {
                Nome = "Minha Empresa",
                Telefone = "123456789",
                Responsavel = "João Silva"
            };

            var empresa = new Empresa { Id = 1, Nome = "Minha Empresa" };

            _mockMapper.Setup(m => m.Map<Empresa>(It.IsAny<CreateEmpresaDTO>())).Returns(empresa);
            _mockUof.Setup(u => u.EmpresaRepository.CreateAsync(It.IsAny<Empresa>())).ReturnsAsync(empresa);
            _mockUof.Setup(u => u.CommitAsync()).Returns(Task.CompletedTask);

            var signingKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes("UmaChaveSecretaMuitoLongaParaTestes1234567890")
            );
            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(
                signingKey,
                Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256
            );

            var tokenMock = new JwtSecurityToken(
                issuer: "testIssuer",
                audience: "testAudience",
                claims: null,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: signingCredentials
            );

            _mockTokenService.Setup(t => t.GenerateAccessToken(It.IsAny<List<Claim>>(), It.IsAny<IConfiguration>())).Returns(tokenMock);

            // Act
            var result = await _controller.CreateEmpresa(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var json = JsonSerializer.Serialize(okResult.Value);
            var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            Assert.Equal(1, root.GetProperty("Empresa").GetProperty("Id").GetInt32());
            Assert.Equal("Minha Empresa", root.GetProperty("Empresa").GetProperty("Nome").GetString());
        }


        [Fact]
        public async Task CreateEmpresa_DeveRetornarErro500_QuandoOcorreExcecao()
        {
            // Arrange
            var dto = new CreateEmpresaDTO { Nome = "Empresa X" };
            _mockMapper.Setup(m => m.Map<Empresa>(dto)).Throws(new Exception("Falha geral"));

            // Act
            var result = await _controller.CreateEmpresa(dto);

            // Assert
            var errorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, errorResult.StatusCode);
            Assert.Contains("Erro ao criar empresa", errorResult.Value.ToString());
        }
    }
}
