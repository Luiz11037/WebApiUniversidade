using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using apiUniversidade.DTO;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace apiUniversidade.Controllers
{
    [ApiController]
    [Route("[controller]")]    
    public class AutorizaController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AutorizaController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        private UserToken GerarToken(UserDTO userInfo){
            var claims = new[]{
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.UniqueName, userInfo.Email),
                new Claim("IFRN", "TecInfo"),
                new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));

            var credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var expiracao = _configuration["TokenConfiguration:Expirehours"];
            var expiration = DateTime.UtcNow.AddHours(double.Parse(expiracao));

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["TokenConfiguration:Issuer"],
                audience: _configuration["TokenConfiguration:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            return new UserToken(){
                Authenticated = true,
                Expiration = expiration,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Message = "JWT Ok"
            };

        }

        [HttpGet]
            public ActionResult<string> Get()
            {
                return "AutorizaController :: Acessado em: " + DateTime.Now.ToLongDateString();
            }

        [HttpPost("registeer")]
            public async Task<ActionResult> RegisterUser([FromBody]UserDTO model)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if(!result.Succeeded)
                return BadRequest(result.Errors);

                await _signInManager.SignInAsync(user, false);
                    return Ok(GerarToken(model));
            }

        [HttpPost("login")]

        public async Task<ActionResult> Login ([FromBody] UserDTO userInfo)
        {
            var result = await _signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password, isPersistent: false, lockoutOnFailure: false);

            if(result.Succeeded)
            {
                return Ok(GerarToken(userInfo));                
            }
            else
            {
                ModelState.AddModelError(string.Empty, "=== Login inválido ===");
                return BadRequest(ModelState);
            }
        }
    }
}