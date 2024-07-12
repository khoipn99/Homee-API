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
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using NuGet.Common;
using HomeeAPI.DTO.RequestObject;

namespace HomeeAPI.Controllers
{   
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public UsersController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration  = configuration;
        }

        // GET: api/Users
        [Authorize]
        [HttpGet]
        public ActionResult<ApiResponse<IEnumerable<UserResponse>>> GetUsers(int pageIndex = 1, int pageSize = 10)
        {
            var response = new ApiResponse<IEnumerable<UserResponse>>();

            try
            {
                var users = _unitOfWork.UserRepository.Get(
                    pageIndex: pageIndex,
                    pageSize: pageSize
                ).Select(user => new UserResponse
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Password = user.Password,
                    Phone = user.Phone,
                    Address = user.Address,
                    Dob = user.Dob,
                    Gender = user.Gender,
                    Avatar = user.Avatar,
                    RoleId = user.RoleId,
                    Status = user.Status,
                    Money = user.Money,
                    Discount = user.Discount
                }).ToList();

                response.Ok(users);
            }
            catch (Exception ex)
            {
                response.Error($"An error occurred while retrieving users: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            return Ok(response);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<UserResponse>>> GetUser(int id)
        {
            var user = _unitOfWork.UserRepository.GetByID(id);

            if (user == null)
            {
                var errorResponse = new ApiResponse<UserResponse>();
                errorResponse.Error("User not found");
                return NotFound(errorResponse);
            }

            var userResponse = new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.Password,
                Phone = user.Phone,
                Address = user.Address,
                Dob = user.Dob,
                Gender = user.Gender,
                Avatar = user.Avatar,
                RoleId = user.RoleId,
                Status = user.Status,
                Money = user.Money,
                Discount = user.Discount
            };

            var response = new ApiResponse<UserResponse>();
            response.Ok(userResponse);
            return Ok(response);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserResponse userResponse)
        {
            var existingUser = _unitOfWork.UserRepository.GetByID(id);
            if (existingUser == null)
            {
                var errorResponse = new ApiResponse<UserResponse>();
                errorResponse.Error("User not found");
                return NotFound(errorResponse);
            }

            if (userResponse.Email != null)
            {
                existingUser.Email = userResponse.Email;
            }

            if (userResponse.FirstName != null)
            {
                existingUser.FirstName = userResponse.FirstName;
            }

            if (userResponse.LastName != null)
            {
                existingUser.LastName = userResponse.LastName;
            }

            if (userResponse.Password != null)
            {
                existingUser.Password = userResponse.Password;
            }

            if (userResponse.Phone != null)
            {
                existingUser.Phone = userResponse.Phone;
            }

            if (userResponse.Address != null)
            {
                existingUser.Address = userResponse.Address;
            }

            if (userResponse.Dob.HasValue)
            {
                existingUser.Dob = userResponse.Dob.Value;
            }

            if (userResponse.Gender != null)
            {
                existingUser.Gender = userResponse.Gender;
            }

            if (userResponse.Avatar != null)
            {
                existingUser.Avatar = userResponse.Avatar;
            }

            if (userResponse.RoleId.HasValue)
            {
                existingUser.RoleId = userResponse.RoleId.Value;
            }

            if (userResponse.Status != null)
            {
                existingUser.Status = userResponse.Status;
            }

            if (userResponse.Money.HasValue)
            {
                existingUser.Money = userResponse.Money.Value;
            }

            if (userResponse.Discount.HasValue)
            {
                existingUser.Discount = userResponse.Discount.Value;
            }

            _unitOfWork.UserRepository.Update(existingUser);

            try
            {
                _unitOfWork.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UserExists(id))
                {
                    var errorResponse = new ApiResponse<UserResponse>();
                    errorResponse.Error("User not found");
                    return NotFound(errorResponse);
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<ApiResponse<UserResponse>>> PostUser(UserResponse userResponse)
        {
            var user = new User
            {
                Email = userResponse.Email,
                FirstName = userResponse.FirstName,
                LastName = userResponse.LastName,
                Password = userResponse.Password,
                Phone = userResponse.Phone,
                Address = userResponse.Address,
                Dob = userResponse.Dob,
                Gender = userResponse.Gender,
                Avatar = userResponse.Avatar,
                RoleId = userResponse.RoleId,
                Status = userResponse.Status,
                Money = userResponse.Money,
                Discount = userResponse.Discount
            };

            _unitOfWork.UserRepository.Insert(user);

            try
            {
                _unitOfWork.Save();
            }
            catch (DbUpdateException)
            {
                if (await UserExists(user.Id))
                {
                    var errorResponse = new ApiResponse<UserResponse>();
                    errorResponse.Error("User conflict");
                    return Conflict(errorResponse);
                }
                else
                {
                    throw;
                }
            }

            userResponse.Id = user.Id;

            var response = new ApiResponse<UserResponse>();
            response.Ok(userResponse);
            return CreatedAtAction("GetUser", new { id = user.Id }, response);
        }

        [HttpPost("UserImg")]
        public async Task<ActionResult<ApiResponse<UserResponse>>> PostUser([FromForm] UserRequest userRequest, IFormFile file)
        {
            string avatarPath = null;
            if (file != null && file.Length > 0)
            {
                var uploadResponse = await UploadAvatar(file);
                if (uploadResponse is OkObjectResult okResult)
                {
                    var responseObject = okResult.Value as dynamic;
                    avatarPath = responseObject.FilePath;
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to upload avatar.");
                }
            }

            var user = new User
            {
                Email = userRequest.Email,
                FirstName = userRequest.FirstName,
                LastName = userRequest.LastName,
                Password = userRequest.Password,
                Phone = userRequest.Phone,
                Address = userRequest.Address,
                Dob = userRequest.Dob,
                Gender = userRequest.Gender,
                Avatar = avatarPath, // Set the avatar path here
                RoleId = userRequest.RoleId,
                Status = userRequest.Status,
                Money = userRequest.Money,
                Discount = userRequest.Discount
            };

            _unitOfWork.UserRepository.Insert(user);

            try
            {
                _unitOfWork.Save();
            }
            catch (DbUpdateException)
            {
                if (await UserExists(user.Id))
                {
                    var errorResponse = new ApiResponse<UserResponse>();
                    errorResponse.Error("User conflict");
                    return Conflict(errorResponse);
                }
                else
                {
                    throw;
                }
            }

            var userResponse = new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Phone,
                Address = user.Address,
                Dob = user.Dob,
                Gender = user.Gender,
                Avatar = user.Avatar,
                RoleId = user.RoleId,
                Status = user.Status,
                Money = user.Money,
                Discount = user.Discount
            };

            var response = new ApiResponse<UserResponse>();
            response.Ok(userResponse);
            return CreatedAtAction("GetUser", new { id = user.Id }, response);
        }

        private async Task<IActionResult> UploadAvatar(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var extension = Path.GetExtension(file.FileName).ToUpperInvariant();
            var validExtensions = new List<string> { ".JPEG", ".JPG", ".PNG", ".GIF" };
            if (!validExtensions.Contains(extension))
            {
                return BadRequest("Invalid file extension. Allowed extensions are: .JPEG, .JPG, .PNG, .GIF.");
            }

            var fileSizeLimit = 5 * 1024 * 1024; // 5MB
            if (file.Length > fileSizeLimit)
            {
                return BadRequest("File size exceeds 5MB.");
            }

            var fileName = Guid.NewGuid().ToString() + extension;

            var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            if (!Directory.Exists(uploadsDir))
            {
                Directory.CreateDirectory(uploadsDir);
            }

            var filePath = Path.Combine(uploadsDir, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var response = new { FilePath = $"uploads/{fileName}" };
            return Ok(response);
        }


        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = _unitOfWork.UserRepository.GetByID(id);
            if (user == null)
            {
                var errorResponse = new ApiResponse<UserResponse>();
                errorResponse.Error("User not found");
                return NotFound(errorResponse);
            }

            _unitOfWork.UserRepository.Delete(id);
            _unitOfWork.Save();

            return NoContent();
        }

        private async Task<bool> UserExists(int id)
        {
            var user = _unitOfWork.UserRepository.GetByID(id);
            return user != null;
        }

        // POST: api/Users/Login
        [HttpPost("Login")]
        public async Task<ActionResult<UserResponse>> Login(UserResponse loginRequest)
        {
            var existingUser =  _unitOfWork.UserRepository.Get().FirstOrDefault(u => u.Email == loginRequest.Email);

            if (existingUser == null || existingUser.Password != loginRequest.Password)
            {
                var errorResponse = new ApiResponse<UserResponse>();
                errorResponse.Error("Invalid email or password");
                return Unauthorized(errorResponse);
            }

            var user = new User
            {
                Id = existingUser.Id,
                Email = loginRequest.Email,
                FirstName = loginRequest.FirstName,
                LastName = loginRequest.LastName,
                Password = loginRequest.Password,
                Phone = loginRequest.Phone,
                Address = loginRequest.Address,
                Dob = loginRequest.Dob,
                Gender = loginRequest.Gender,
                Avatar = loginRequest.Avatar,
                RoleId = loginRequest.RoleId,
                Status = loginRequest.Status,
                Money = loginRequest.Money,
                Discount = loginRequest.Discount
            };
            var userResponse = new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.Password,
                Phone = user.Phone,
                Address = user.Address,
                Dob = user.Dob,
                Gender = user.Gender,
                Avatar = user.Avatar,
                RoleId = user.RoleId,
                Status = user.Status,
                Money = user.Money,
                Discount = user.Discount
            };
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserId",existingUser.Id.ToString()),
                new Claim("Email",existingUser.Email.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: signIn
                );
            string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new { Token= tokenValue, UserResponse = userResponse});
        }

        [HttpPost("Register")]
        public async Task<ActionResult<ApiResponse<UserResponse>>> Register(UserResponse registerRequest)
        {
            var existingUser =  _unitOfWork.UserRepository.Get().ToList().Any(u => u.Email == registerRequest.Email);

            if (existingUser != null)
            {
                var errorResponse = new ApiResponse<UserResponse>();
                errorResponse.Error("Email is already registered");
                return Conflict(errorResponse);
            }

            var user = new User
            {
                Email = registerRequest.Email,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                Password = registerRequest.Password,
                Phone = registerRequest.Phone,
                Address = registerRequest.Address,
                Dob = registerRequest.Dob,
                Gender = registerRequest.Gender,
                Avatar = registerRequest.Avatar,
                RoleId = registerRequest.RoleId,
                Status = registerRequest.Status,
                Money = registerRequest.Money,
                Discount = registerRequest.Discount
            };

            _unitOfWork.UserRepository.Insert(user);

            try
            {
                _unitOfWork.Save();
            }
            catch (DbUpdateException)
            {
                var errorResponse = new ApiResponse<UserResponse>();
                errorResponse.Error("Failed to register user");
                return Conflict(errorResponse);
            }

            var userResponse = new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.Password,
                Phone = user.Phone,
                Address = user.Address,
                Dob = user.Dob,
                Gender = user.Gender,
                Avatar = user.Avatar,
                RoleId = user.RoleId,
                Status = user.Status,
                Money = user.Money,
                Discount = user.Discount
            };

            var response = new ApiResponse<UserResponse>();
            response.Ok(userResponse);
            return CreatedAtAction("GetUser", new { id = user.Id }, response);
        }
        
    }
}
