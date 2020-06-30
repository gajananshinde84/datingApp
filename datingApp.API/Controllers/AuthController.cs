using System;
using System.Collections.Generic;
using datingApp.API.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using datingApp.API.Models;
using datingApp.API.Dtos;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
namespace datingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _config = config;
            _repo = repo;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            // validate the request
            userForRegisterDto.Username = userForRegisterDto.Username.ToLower();

            if (await _repo.UserExists(userForRegisterDto.Username))
                return BadRequest("User Already Exists!");
            User user = new User
            {
                UserName = userForRegisterDto.Username
            };
            var userToCreate = user;
            var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);

            return StatusCode(201);

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {               
            var userFromRepo = await _repo.Login(userForLoginDto.UserName, userForLoginDto.Password);
            if (userFromRepo == null)
                return Unauthorized();
            var claim = new[]
            {
                 new Claim(ClaimTypes.NameIdentifier,userFromRepo.Id.ToString()),
                 new Claim(ClaimTypes.Name,userFromRepo.UserName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8
                        .GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claim),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}