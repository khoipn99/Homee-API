using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeeRepositories.Models;
using HomeeRepositories.Interface;
using HomeeAPI.DTO;
using HomeeAPI.DTO.ResponseObject;
using HomeeAPI.DTO.RequestObject;

namespace HomeeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public FoodsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Foods
        [HttpGet]
        public ActionResult<ApiResponse<IEnumerable<FoodResponse>>> GetFoods(int pageIndex = 1, int pageSize = 10)
        {
            var response = new ApiResponse<IEnumerable<FoodResponse>>();

            try
            {
                var foods = _unitOfWork.FoodRepository.Get(
                                    pageIndex: pageIndex,
                                    pageSize: pageSize)
                                    .Select(food => new FoodResponse
                                    {
                                        Id = food.Id,
                                        Name = food.Name,
                                        Image = food.Image,
                                        FoodType = food.FoodType,
                                        Price = food.Price,
                                        SellPrice = food.SellPrice,
                                        CategoryId = food.CategoryId, 
                                        ChefId = food.ChefId,         
                                        SellCount = food.SellCount,
                                        Status = food.Status
                                    }).ToList();

                response.Ok(foods);
            }
            catch (Exception ex)
            {
                response.Error($"An error occurred while retrieving foods: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            return Ok(response);
        }

        // GET: api/Foods/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<FoodResponse>>> GetFood(int id)
        {
            var food = _unitOfWork.FoodRepository.GetByID(id);

            if (food == null)
            {
                var errorResponse = new ApiResponse<FoodResponse>();
                errorResponse.Error("Food not found");
                return NotFound(errorResponse);
            }

            var foodResponse = new FoodResponse
            {
                Id = food.Id,
                Name = food.Name,
                Image = food.Image,
                FoodType = food.FoodType,
                Price = food.Price,
                SellPrice = food.SellPrice,
                SellCount = food.SellCount,
                Status = food.Status,
                CategoryId = food.CategoryId,
                ChefId = food.ChefId
            };

            var response = new ApiResponse<FoodResponse>();
            response.Ok(foodResponse);
            return Ok(response);
        }

        // POST: api/Foods
        //[HttpPost]
        //public async Task<ActionResult<ApiResponse<FoodResponse>>> PostFood(FoodResponse foodRequest)
        //{
        //    var food = new Food
        //    {
        //        Name = foodRequest.Name,
        //        Image = foodRequest.Image,
        //        FoodType = foodRequest.FoodType,
        //        Price = foodRequest.Price,
        //        SellPrice = foodRequest.SellPrice,
        //        CategoryId = foodRequest.CategoryId,
        //        ChefId = foodRequest.ChefId,
        //        SellCount = foodRequest.SellCount,
        //        Status = foodRequest.Status
        //    };

        //    _unitOfWork.FoodRepository.Insert(food);

        //    try
        //    {
        //        _unitOfWork.Save();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (await FoodExists(food.Id))
        //        {
        //            var errorResponse = new ApiResponse<FoodResponse>();
        //            errorResponse.Error("Food conflict");
        //            return Conflict(errorResponse);
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    var foodResponse = new FoodResponse
        //    {
        //        Id = food.Id,
        //        Name = food.Name,
        //        Image = food.Image,
        //        FoodType = food.FoodType,
        //        Price = food.Price,
        //        SellPrice = food.SellPrice,
        //        SellCount = food.SellCount,
        //        Status = food.Status,
        //        CategoryId = food.CategoryId,
        //        ChefId = food.ChefId
        //    };

        //    var response = new ApiResponse<FoodResponse>();
        //    response.Ok(foodResponse);
        //    return CreatedAtAction("GetFood", new { id = food.Id }, response);
        //}
        [HttpPost]
        public async Task<ActionResult<ApiResponse<FoodResponse>>> PostFood([FromForm] FoodRequest foodRequest, IFormFile file)
        {
            // Upload image if provided
            string imagePath = null;
            if (file != null && file.Length > 0)
            {
                var uploadResponse = await UploadFoodImage(file);
                if (uploadResponse is OkObjectResult okResult)
                {
                    var responseObject = okResult.Value as dynamic;
                    imagePath = responseObject.FilePath;
                }
                else
                {
                    // Handle upload error if needed
                    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to upload food image.");
                }
            }

            var food = new Food
            {
                Name = foodRequest.Name,
                Image = imagePath, // Set the image path here
                FoodType = foodRequest.FoodType,
                Price = foodRequest.Price,
                SellPrice = foodRequest.SellPrice,
                CategoryId = foodRequest.CategoryId,
                ChefId = foodRequest.ChefId,
                SellCount = foodRequest.SellCount,
                Status = foodRequest.Status
            };

            _unitOfWork.FoodRepository.Insert(food);

            try
            {
                _unitOfWork.Save();
            }
            catch (DbUpdateException)
            {
                if (await FoodExists(food.Id))
                {
                    var errorResponse = new ApiResponse<FoodResponse>();
                    errorResponse.Error("Food conflict");
                    return Conflict(errorResponse);
                }
                else
                {
                    throw;
                }
            }

            var foodResponse = new FoodResponse
            {
                Id = food.Id,
                Name = food.Name,
                Image = food.Image,
                FoodType = food.FoodType,
                Price = food.Price,
                SellPrice = food.SellPrice,
                SellCount = food.SellCount,
                Status = food.Status,
                CategoryId = food.CategoryId,
                ChefId = food.ChefId
            };

            var response = new ApiResponse<FoodResponse>();
            response.Ok(foodResponse);
            return CreatedAtAction("GetFood", new { id = food.Id }, response);
        }
        private async Task<IActionResult> UploadFoodImage(IFormFile file)
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


        // PUT: api/Foods
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFood(int id, FoodResponse foodResponse)
        {
            var existingFood = _unitOfWork.FoodRepository.GetByID(id);

            if (existingFood == null)
            {
                var errorResponse = new ApiResponse<FoodResponse>();
                errorResponse.Error("Food not found");
                return NotFound(errorResponse);
            }

            // Update only the fields that are not null in foodResponse
            if (foodResponse.Name != null)
            {
                existingFood.Name = foodResponse.Name;
            }
            if (foodResponse.Image != null)
            {
                existingFood.Image = foodResponse.Image;
            }
            if (foodResponse.FoodType != null)
            {
                existingFood.FoodType = foodResponse.FoodType;
            }
            if (foodResponse.Price != default)
            {
                existingFood.Price = foodResponse.Price;
            }
            if (foodResponse.SellPrice != default)
            {
                existingFood.SellPrice = foodResponse.SellPrice;
            }
            if (foodResponse.CategoryId != null)
            {
                existingFood.CategoryId = foodResponse.CategoryId;
            }
            if (foodResponse.ChefId != null)
            {
                existingFood.ChefId = foodResponse.ChefId;
            }
            if (foodResponse.SellCount != null)
            {
                existingFood.SellCount = foodResponse.SellCount;
            }
            if (foodResponse.Status != null)
            {
                existingFood.Status = foodResponse.Status;
            }

            _unitOfWork.FoodRepository.Update(existingFood);

            try
            {
                _unitOfWork.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await FoodExists(id))
                {
                    var errorResponse = new ApiResponse<FoodResponse>();
                    errorResponse.Error("Food not found");
                    return NotFound(errorResponse);
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        // DELETE: api/Foods/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFood(int id)
        {
            var food = _unitOfWork.FoodRepository.GetByID(id);
            if (food == null)
            {
                var errorResponse = new ApiResponse<FoodResponse>();
                errorResponse.Error("Food not found");
                return NotFound(errorResponse);
            }

            _unitOfWork.FoodRepository.Delete(id);
            _unitOfWork.Save();

            return NoContent();
        }

        private async Task<bool> FoodExists(int id)
        {
            var food = _unitOfWork.FoodRepository.GetByID(id);
            return food != null;
        }
    }
}
