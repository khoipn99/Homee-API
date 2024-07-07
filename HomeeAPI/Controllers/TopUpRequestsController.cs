using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HomeeRepositories.Models;
using HomeeRepositories.Interface;
using HomeeAPI.DTO.ResponseObject;
using HomeeAPI.DTO;
using Microsoft.EntityFrameworkCore;

namespace HomeeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopUpRequestController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public TopUpRequestController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // POST: api/TopUpRequest
        [HttpPost]
        public async Task<ActionResult<ApiResponse<TopUpRequestResponse>>> PostTopUpRequest(TopUpRequestResponse topUpRequestResponse)
        {
            var topUpRequest = new TopUpRequest
            {
                UserId = topUpRequestResponse.UserId,
                ChefId = topUpRequestResponse.ChefId,
                Amount = topUpRequestResponse.Amount,
                RequestDate = DateTime.UtcNow,
                IsApproved = false,
                ApprovalDate = null
            };

            _unitOfWork.TopUpRequestRepository.Insert(topUpRequest);

            try
            {
                _unitOfWork.Save();
            }
            catch (DbUpdateException ex)
            {
                var errorResponse = new ApiResponse<TopUpRequestResponse>();
                errorResponse.Error($"An error occurred while creating the top-up request: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }

            topUpRequestResponse.Id = topUpRequest.Id;

            var response = new ApiResponse<TopUpRequestResponse>();
            response.Ok(topUpRequestResponse);
            return CreatedAtAction("GetTopUpRequest", new { id = topUpRequest.Id }, response);
        }

        // GET: api/TopUpRequest/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<TopUpRequestResponse>>> GetTopUpRequest(int id)
        {
            var topUpRequest = _unitOfWork.TopUpRequestRepository.GetByID(id);

            if (topUpRequest == null)
            {
                var errorResponse = new ApiResponse<TopUpRequestResponse>();
                errorResponse.Error("Top-up request not found");
                return NotFound(errorResponse);
            }

            var topUpRequestResponse = new TopUpRequestResponse
            {
                Id = topUpRequest.Id,
                UserId = topUpRequest.UserId,
                ChefId = topUpRequest.ChefId,
                Amount = topUpRequest.Amount,
                RequestDate = topUpRequest.RequestDate,
                IsApproved = topUpRequest.IsApproved,
                ApprovalDate = topUpRequest.ApprovalDate
            };

            var response = new ApiResponse<TopUpRequestResponse>();
            response.Ok(topUpRequestResponse);
            return Ok(response);
        }

        // PUT: api/TopUpRequest/approve/5
        [HttpPut("approve/{id}")]
        public async Task<IActionResult> ApproveTopUpRequest(int id)
        {
            var topUpRequest = _unitOfWork.TopUpRequestRepository.GetByID(id);
            if (topUpRequest == null)
            {
                var errorResponse = new ApiResponse<TopUpRequestResponse>();
                errorResponse.Error("Top-up request not found");
                return NotFound(errorResponse);
            }

            topUpRequest.IsApproved = true;
            topUpRequest.ApprovalDate = DateTime.UtcNow;

            if (topUpRequest.UserId.HasValue)
            {
                var user = _unitOfWork.UserRepository.GetByID(topUpRequest.UserId.Value);
                if (user == null)
                {
                    var errorResponse = new ApiResponse<TopUpRequestResponse>();
                    errorResponse.Error("User not found");
                    return NotFound(errorResponse);
                }
                user.Money += topUpRequest.Amount;
                _unitOfWork.UserRepository.Update(user);
            }
            else if (topUpRequest.ChefId.HasValue)
            {
                var chef = _unitOfWork.ChefRepository.GetByID(topUpRequest.ChefId.Value);
                if (chef == null)
                {
                    var errorResponse = new ApiResponse<TopUpRequestResponse>();
                    errorResponse.Error("Chef not found");
                    return NotFound(errorResponse);
                }
                chef.Money += topUpRequest.Amount;
                _unitOfWork.ChefRepository.Update(chef);
            }

            _unitOfWork.TopUpRequestRepository.Update(topUpRequest);

            try
            {
                _unitOfWork.Save();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!await TopUpRequestExists(id))
                {
                    var errorResponse = new ApiResponse<TopUpRequestResponse>();
                    errorResponse.Error("Top-up request not found");
                    return NotFound(errorResponse);
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private async Task<bool> TopUpRequestExists(int id)
        {
            var topUpRequest = _unitOfWork.TopUpRequestRepository.GetByID(id);
            return topUpRequest != null;
        }
    }
}
