using APICatalogo.Services;
using DiarioObras.Data.Context;
using DiarioObras.Data.Interfaces;
using DiarioObras.DTOs.LoginDTOs;
using DiarioObras.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DiarioObras.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;
        private readonly IUnitOfWork _uof;
        private readonly AppDbContext _context;


        public AuthController(ITokenService tokenService,
                              UserManager<ApplicationUser> userManager,
                              RoleManager<IdentityRole> roleManager,
                              IConfiguration configuration,
                              ILogger<AuthController> logger,
                              IUnitOfWork uof,
                              AppDbContext context)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
            _uof = uof;
            _context = context;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModelDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.UserName!);

            if (user is not null && await _userManager.CheckPasswordAsync(user, model.Password!))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim("empresaId", user.EmpresaId.ToString()),
                new Claim("id", user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = _tokenService.GenerateAccessToken(authClaims,
                                                             _configuration);

                var refreshToken = _tokenService.GenerateRefreshToken();

                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"],
                                   out int refreshTokenValidityInMinutes);

                user.RefreshTokenExpiryTime =
                                DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);

                user.RefreshToken = refreshToken;

                await _userManager.UpdateAsync(user);

                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    Expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterModelDTO model,
            [FromHeader(Name = "Authorization")] string authHeader)
        {
            try
            {
                // Validação do token
                var token = authHeader?.Replace("Bearer ", "");
                var principal = _tokenService.GetPrincipalFromExpiredToken(token, _configuration);

                var empresaIdClaim = principal?.Claims.FirstOrDefault(c => c.Type == "empresaId")?.Value;
                var purposeClaim = principal?.Claims.FirstOrDefault(c => c.Type == "purpose")?.Value;

                if (purposeClaim != "user_registration" || !int.TryParse(empresaIdClaim, out var empresaId))
                    return Unauthorized(new { Message = "Token de registro inválido ou expirado" });

                // Restante do código permanece igual
                var userExists = await _userManager.FindByEmailAsync(model.Email);
                if (userExists != null)
                    return BadRequest(new { Message = "E-mail já está em uso" });

                var user = new ApplicationUser
                {
                    Email = model.Email,
                    UserName = model.Email,
                    Nome = model.UserName,
                    EmpresaId = empresaId, // Agora usando int
                    PhoneNumber = model.PhoneNumber,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                    return BadRequest(new
                    {
                        Message = "Falha ao criar usuário",
                        Errors = result.Errors.Select(e => e.Description)
                    });

                return Ok(new { Message = "Usuário criado com sucesso" });
            }
            catch (SecurityTokenException ex)
            {
                return Unauthorized(new { Message = $"Token inválido: {ex.Message}" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = $"Erro durante o registro: {ex.Message}" });
            }
        }


        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModelDTO tokenModel)
        {

            if (tokenModel is null)
            {
                return BadRequest("Invalid client request");
            }

            string? accessToken = tokenModel.AccessToken
                                  ?? throw new ArgumentNullException(nameof(tokenModel));

            string? refreshToken = tokenModel.RefreshToken
                                   ?? throw new ArgumentException(nameof(tokenModel));

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken!, _configuration);

            if (principal == null)
            {
                return BadRequest("Invalid access token/refresh token");
            }

            string username = principal.Identity.Name;

            var user = await _userManager.FindByNameAsync(username!);

            if (user == null || user.RefreshToken != refreshToken
                             || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid access token/refresh token");
            }

            var newAccessToken = _tokenService.GenerateAccessToken(
                                               principal.Claims.ToList(), _configuration);

            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;

            await _userManager.UpdateAsync(user);

            return new ObjectResult(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refreshToken = newRefreshToken
            });
        }

        [Authorize]
        [HttpPost]
        [Route("revoke/{username}")]
        public async Task<IActionResult> Revoke(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null) return BadRequest("Invalid user name");

            user.RefreshToken = null;

            await _userManager.UpdateAsync(user);

            return NoContent();
        }


        [HttpPost]
        [Route("CreateRole")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            var roleExist = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExist)
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));

                if (roleResult.Succeeded)
                {
                    _logger.LogInformation(1, "Roles Added");
                    return StatusCode(StatusCodes.Status200OK,
                            new ResponseLoginDTO
                            {
                                Status = "Success",
                                Message =
                            $"Role {roleName} added successfully"
                            });
                }
                else
                {
                    _logger.LogInformation(2, "Error");
                    return StatusCode(StatusCodes.Status400BadRequest,
                       new ResponseLoginDTO
                       {
                           Status = "Error",
                           Message =
                           $"Issue adding the new {roleName} role"
                       });
                }
            }
            return StatusCode(StatusCodes.Status400BadRequest,
              new ResponseLoginDTO { Status = "Error", Message = "Role already exist." });
        }

        [HttpPost]
        [Route("AddUserToRole")]
        public async Task<IActionResult> AddUserToRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var result = await _userManager.AddToRoleAsync(user, roleName);
                if (result.Succeeded)
                {
                    _logger.LogInformation(1, $"User {user.Email} added to the {roleName} role");
                    return StatusCode(StatusCodes.Status200OK,
                           new ResponseLoginDTO
                           {
                               Status = "Success",
                               Message =
                           $"User {user.Email} added to the {roleName} role"
                           });
                }
                else
                {
                    _logger.LogInformation(1, $"Error: Unable to add user {user.Email} to the {roleName} role");
                    return StatusCode(StatusCodes.Status400BadRequest, new ResponseLoginDTO
                    {
                        Status = "Error",
                        Message = $"Error: Unable to add user {user.Email} to the {roleName} role"
                    });
                }
            }
            return BadRequest(new { error = "Unable to find user" });
        }

    }
}
