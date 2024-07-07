﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeeRepositories.Models;
using HomeeRepositories.Interface;
using HomeeAPI.DTO;
using HomeeAPI.DTO.ResponseObject;

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
        [HttpPost]
        public async Task<ActionResult<ApiResponse<FoodResponse>>> PostFood(FoodResponse foodRequest)
        {
            var food = new Food
            {
                Name = foodRequest.Name,
                Image = foodRequest.Image,
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

        [HttpGet("by-chef")]
        public ActionResult<ApiResponse<IEnumerable<FoodResponse>>> GetFoodsByChef(int chefId)
        {
            var response = new ApiResponse<IEnumerable<FoodResponse>>();

            try
            {
                var foods = _unitOfWork.FoodRepository.GetAll()
                                                        .Where(food => food.ChefId == chefId)
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

        [HttpGet("by-category")]
        public ActionResult<ApiResponse<IEnumerable<FoodResponse>>> GetFoodsByCategory(int cateId)
        {
            var response = new ApiResponse<IEnumerable<FoodResponse>>();

            try
            {
                var foods = _unitOfWork.FoodRepository.GetAll()
                                                        .Where(food => food.CategoryId == cateId)
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

        private async Task<bool> FoodExists(int id)
        {
            var food = _unitOfWork.FoodRepository.GetByID(id);
            return food != null;
        }


    }
}