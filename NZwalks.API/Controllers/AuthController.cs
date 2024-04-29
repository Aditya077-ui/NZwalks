using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZwalks.API.Models.DTO;
using NZwalks.API.Repositories;

namespace NZwalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;
        public AuthController(UserManager<IdentityUser> userManager,ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }


        //POST: /api/auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDTO.UserName,
                Email = registerRequestDTO.UserName
            };

            var identityResult = await _userManager.CreateAsync(identityUser,registerRequestDTO.Password);

            if (identityResult.Succeeded)
            {
                //add roles to this user
                if (registerRequestDTO.Roles != null && registerRequestDTO.Roles.Any())
                {
                    identityResult = await _userManager.AddToRolesAsync(identityUser,registerRequestDTO.Roles);

                    if(identityResult.Succeeded)
                    {
                        return Ok("User was Registered!Please Login");
                    }
                }
            }
            return BadRequest("Something went wrong!!");
        }


        //POST : /api/auth/login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginRequestDTO.UserName);
            if(user != null)
            {
              var checkpasswordresult =  await _userManager.CheckPasswordAsync(user,loginRequestDTO.Password);
                if (checkpasswordresult)
                {
                    //get roles for this user
                    var roles =  await _userManager.GetRolesAsync(user);
                    if(roles != null)
                    {
                        //create token
                        var JWTToken = _tokenRepository.CreateJWTToken(user, roles.ToList());
                        var Response = new LoginResponseDTO
                        {
                            JWTToken = JWTToken,
                        };
                        return Ok(Response);
                    }

                   
                }
            }

            return BadRequest("Username or password is incorrect");
        }
    }
}
