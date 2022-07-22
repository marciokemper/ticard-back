using Control.Facilites.AppServices.Interfaces;
using Control.Facilites.Domain.Entities;
using Control.Facilites.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Control.Facilites.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    public class AuthController : Controller
    {

        private readonly IUsuarioAppService appService;
        private readonly UsuarioValidator validator;

        public AuthController(IUsuarioAppService appService, UsuarioValidator validator)
        {
            this.appService = appService;
            this.validator = validator;
        }

       
        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody] UsuarioLogin model)
        {
            try
            {


                var usuario = appService.Login(model.Login, model.Senha);
                Console.WriteLine($"USUARIO LOGADO: = {usuario}");
                if (usuario == null)
                    return Unauthorized();

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.NameId, usuario.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.Sub, usuario.Login),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                        new Claim("userId",  usuario.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.Acr, usuario.EmpresaFornecedor==null?"": usuario.EmpresaFornecedor.Id.ToString()),

                    };

                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: "http://localhost:5000",
                    audience: "http://localhost:5000",
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(2),
                    signingCredentials: signinCredentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);


                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    Expiration = jwtSecurityToken.ValidTo,
                    Claims = jwtSecurityToken.Claims,
                });
            }
            catch (Exception ex)
            {
                    return StatusCode((int)HttpStatusCode.InternalServerError, $"Ops!!! Erro enquanto criava o token: {ex.Message} =>> {ex.ToString()}");
            }

        }

        

    }
}
