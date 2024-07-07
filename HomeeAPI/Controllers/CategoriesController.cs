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
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Categories
        [HttpGet]
        public ActionResult<ApiResponse<IEnumerable<CategoryResponse>>> GetCategories()
        {
            var response = new ApiResponse<IEnumerable<CategoryResponse>>();

            try
            {
                var categories = _unitOfWork.CategoryRepository.Get()
                                    .Select(category => new CategoryResponse
                                    {
                                        CategoryId = category.Id,
                                        CategoryName = category.Name
                                    }).ToList();

                response.Ok(categories);
            }
            catch (Exception ex)
            {
                response.Error($"An error occurred while retrieving categories: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            return Ok(response);
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<CategoryResponse>>> GetCategory(int id)
        {
            var category = _unitOfWork.CategoryRepository.GetByID(id);

            if (category == null)
            {
                var errorResponse = new ApiResponse<CategoryResponse>();
                errorResponse.Error("Category not found");
                return NotFound(errorResponse);
            }

            var categoryResponse = new CategoryResponse
            {
                CategoryId = category.Id,
                CategoryName = category.Name
            };

            var response = new ApiResponse<CategoryResponse>();
            response.Ok(categoryResponse);
            return Ok(response);
        }

        // PUT: api/Categories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, CategoryResponse categoryResponse)
        {
            //if (id != categoryResponse.CategoryId)
            //{
            //    var errorResponse = new ApiResponse<CategoryResponse>();
            //    errorResponse.Error("Category ID mismatch");
            //    return BadRequest(errorResponse);
            //}

            var category = new Category
            {
                Id = id,
                Name = categoryResponse.CategoryName
            };

            _unitOfWork.CategoryRepository.Update(category);

            try
            {
                _unitOfWork.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CategoryExists(id))
                {
                    var errorResponse = new ApiResponse<CategoryResponse>();
                    errorResponse.Error("Category not found");
                    return NotFound(errorResponse);
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Categories
        [HttpPost]
        public async Task<ActionResult<ApiResponse<CategoryResponse>>> PostCategory(CategoryResponse categoryResponse)
        {
            var category = new Category
            {
                Name = categoryResponse.CategoryName
            };

            _unitOfWork.CategoryRepository.Insert(category);

            try
            {
                _unitOfWork.Save();
            }
            catch (DbUpdateException)
            {
                if (await CategoryExists(category.Id))
                {
                    var errorResponse = new ApiResponse<CategoryResponse>();
                    errorResponse.Error("Category conflict");
                    return Conflict(errorResponse);
                }
                else
                {
                    throw;
                }
            }

            categoryResponse.CategoryId = category.Id;

            var response = new ApiResponse<CategoryResponse>();
            response.Ok(categoryResponse);
            return CreatedAtAction("GetCategory", new { id = category.Id }, response);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = _unitOfWork.CategoryRepository.GetByID(id);
            if (category == null)
            {
                var errorResponse = new ApiResponse<CategoryResponse>();
                errorResponse.Error("Category not found");
                return NotFound(errorResponse);
            }

            _unitOfWork.CategoryRepository.Delete(id);
            _unitOfWork.Save();

            return NoContent();
        }

        private async Task<bool> CategoryExists(int id)
        {
            var category = _unitOfWork.CategoryRepository.GetByID(id);
            return category != null;
        }
    }
}