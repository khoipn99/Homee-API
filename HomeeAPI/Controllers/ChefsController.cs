using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeeRepositories.Models;
using HomeeRepositories.Interface;
using HomeeAPI.DTO.ResponseObject;
using HomeeAPI.DTO;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HomeeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChefsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public ChefsController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        // GET: api/Chefs
        [HttpGet]
        public ActionResult<ApiResponse<IEnumerable<ChefResponse>>> GetChefs(int pageIndex = 1, int pageSize = 10)
        {
            var response = new ApiResponse<IEnumerable<ChefResponse>>();

            try
            {
                var chefs = _unitOfWork.ChefRepository.Get(
                    pageIndex: pageIndex,
                    pageSize: pageSize
                ).Select(chef => new ChefResponse
                {
                    Id = chef.Id,
                    Name = chef.Name,
                    Address = chef.Address,
                    CreatorId = chef.CreatorId,
                    ProfilePicture = chef.ProfilePicture,
                    Score = chef.Score,
                    Hours = chef.Hours,
                    Status = chef.Status,
                    Email = chef.Email,
                    Password = chef.Password,
                    Phone = chef.Phone,
                    Money = chef.Money,
                    Banking = chef.Banking
                }).ToList();

                response.Ok(chefs);
            }
            catch (Exception ex)
            {
                response.Error($"An error occurred while retrieving chefs: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            return Ok(response);
        }

        // GET: api/Chefs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ChefResponse>>> GetChef(int id)
        {
            var chef = _unitOfWork.ChefRepository.GetByID(id);

            if (chef == null)
            {
                var errorResponse = new ApiResponse<ChefResponse>();
                errorResponse.Error("Chef not found");
                return NotFound(errorResponse);
            }

            var chefResponse = new ChefResponse
            {
                Id = chef.Id,
                Name = chef.Name,
                Address = chef.Address,
                CreatorId = chef.CreatorId,
                ProfilePicture = chef.ProfilePicture,
                Score = chef.Score,
                Hours = chef.Hours,
                Status = chef.Status,
                Email = chef.Email,
                Password = chef.Password,
                Phone = chef.Phone,
                Money = chef.Money,
                Banking = chef.Banking
            };

            var response = new ApiResponse<ChefResponse>();
            response.Ok(chefResponse);
            return Ok(response);
        }

        // PUT: api/Chefs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChef(int id, ChefResponse chefResponse)
        {
            var existingChef = _unitOfWork.ChefRepository.GetByID(id);
            if (existingChef == null)
            {
                var errorResponse = new ApiResponse<ChefResponse>();
                errorResponse.Error("Chef not found");
                return NotFound(errorResponse);
            }

            if (chefResponse.Name != null)
            {
                existingChef.Name = chefResponse.Name;
            }

            if (chefResponse.Address != null)
            {
                existingChef.Address = chefResponse.Address;
            }

            if (chefResponse.CreatorId.HasValue)
            {
                existingChef.CreatorId = chefResponse.CreatorId.Value;
            }

            if (chefResponse.ProfilePicture != null)
            {
                existingChef.ProfilePicture = chefResponse.ProfilePicture;
            }

            if (chefResponse.Score.HasValue)
            {
                existingChef.Score = chefResponse.Score.Value;
            }

            if (chefResponse.Hours.HasValue)
            {
                existingChef.Hours = chefResponse.Hours.Value;
            }

            if (chefResponse.Status != null)
            {
                existingChef.Status = chefResponse.Status;
            }

            if (chefResponse.Email != null)
            {
                existingChef.Email = chefResponse.Email;
            }

            if (chefResponse.Password != null)
            {
                existingChef.Password = chefResponse.Password;
            }

            if (chefResponse.Phone != null)
            {
                existingChef.Phone = chefResponse.Phone;
            }
            if (chefResponse.Money != null)
            {
                existingChef.Money = chefResponse.Money;
            }
            if (chefResponse.Banking != null)
            {
                existingChef.Banking = chefResponse.Banking;
            }

            _unitOfWork.ChefRepository.Update(existingChef);

            try
            {
                _unitOfWork.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ChefExists(id))
                {
                    var errorResponse = new ApiResponse<ChefResponse>();
                    errorResponse.Error("Chef not found");
                    return NotFound(errorResponse);
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Chefs
        [HttpPost]
        public async Task<ActionResult<ApiResponse<ChefResponse>>> PostChef(ChefResponse chefResponse)
        {
            var chef = new Chef
            {
                Name = chefResponse.Name,
                Address = chefResponse.Address,
                CreatorId = chefResponse.CreatorId,
                ProfilePicture = chefResponse.ProfilePicture,
                Score = chefResponse.Score,
                Hours = chefResponse.Hours,
                Status = chefResponse.Status,
                Email = chefResponse.Email,
                Password = chefResponse.Password,
                Phone = chefResponse.Phone,
                Money = chefResponse.Money,
                Banking = chefResponse.Banking
            };

            _unitOfWork.ChefRepository.Insert(chef);

            try
            {
                _unitOfWork.Save();
            }
            catch (DbUpdateException)
            {
                if (await ChefExists(chef.Id))
                {
                    var errorResponse = new ApiResponse<ChefResponse>();
                    errorResponse.Error("Chef conflict");
                    return Conflict(errorResponse);
                }
                else
                {
                    throw;
                }
            }

            chefResponse.Id = chef.Id;

            var response = new ApiResponse<ChefResponse>();
            response.Ok(chefResponse);
            return CreatedAtAction("GetChef", new { id = chef.Id }, response);
        }

        // DELETE: api/Chefs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChef(int id)
        {
            var chef = _unitOfWork.ChefRepository.GetByID(id);
            if (chef == null)
            {
                var errorResponse = new ApiResponse<ChefResponse>();
                errorResponse.Error("Chef not found");
                return NotFound(errorResponse);
            }

            _unitOfWork.ChefRepository.Delete(id);
            _unitOfWork.Save();

            return NoContent();
        }

        private async Task<bool> ChefExists(int id)
        {
            var chef = _unitOfWork.ChefRepository.GetByID(id);
            return chef != null;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ChefResponse>> Login(ChefResponse loginRequest)
        {
            var existingChef = _unitOfWork.ChefRepository.Get().FirstOrDefault(c => c.Email == loginRequest.Email);

            if (existingChef == null || existingChef.Password != loginRequest.Password)
            {
                var errorResponse = new ApiResponse<ChefResponse>();
                errorResponse.Error("Invalid email or password");
                return Unauthorized(errorResponse);
            }

            var chefResponse = new ChefResponse
            {
                Id = existingChef.Id,
                Name = existingChef.Name,
                Address = existingChef.Address,
                CreatorId = existingChef.CreatorId,
                ProfilePicture = existingChef.ProfilePicture,
                Score = existingChef.Score,
                Hours = existingChef.Hours,
                Status = existingChef.Status,
                Email = existingChef.Email,
                Password = existingChef.Password,
                Phone = existingChef.Phone,
                Money = existingChef.Money,
                Banking = existingChef.Banking
            };

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("ChefId", existingChef.Id.ToString()),
        new Claim("Email", existingChef.Email.ToString()),
        new Claim(ClaimTypes.Role, "Chef")
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: signIn
            );

            string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new { Token = tokenValue, ChefResponse = chefResponse });
        }
        // regis 
        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<ChefResponse>>> Register(ChefResponse registerRequest)
        {
            var existingChef = _unitOfWork.ChefRepository.Get().FirstOrDefault(c => c.Email == registerRequest.Email);

            if (existingChef != null)
            {
                var errorResponse = new ApiResponse<ChefResponse>();
                errorResponse.Error("Email is already registered");
                return Conflict(errorResponse);
            }

            var chef = new Chef
            {
                Name = registerRequest.Name,
                Address = registerRequest.Address,
                CreatorId = registerRequest.CreatorId,
                ProfilePicture = registerRequest.ProfilePicture,
                Score = registerRequest.Score,
                Hours = registerRequest.Hours,
                Status = registerRequest.Status,
                Email = registerRequest.Email,
                Password = registerRequest.Password,
                Phone = registerRequest.Phone,
                Money = registerRequest.Money,
                Banking = registerRequest.Banking
            };

            _unitOfWork.ChefRepository.Insert(chef);

            try
            {
                _unitOfWork.Save();
            }
            catch (DbUpdateException)
            {
                var errorResponse = new ApiResponse<ChefResponse>();
                errorResponse.Error("Failed to register chef");
                return Conflict(errorResponse);
            }

            var chefResponse = new ChefResponse
            {
                Id = chef.Id,
                Name = chef.Name,
                Address = chef.Address,
                CreatorId = chef.CreatorId,
                ProfilePicture = chef.ProfilePicture,
                Score = chef.Score,
                Hours = chef.Hours,
                Status = chef.Status,
                Email = chef.Email,
                Password = chef.Password,
                Phone = chef.Phone,
                Money = chef.Money,
                Banking = chef.Banking
            };

            var response = new ApiResponse<ChefResponse>();
            response.Ok(chefResponse);
            return CreatedAtAction("GetChef", new { id = chef.Id }, response);
        }
    }
}
