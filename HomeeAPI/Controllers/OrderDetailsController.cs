using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeeRepositories.Models;
using HomeeRepositories.Interface;
using HomeeAPI.DTO.ResponseObject;
using HomeeAPI.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HomeeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderDetailsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/OrderDetails
        [HttpGet]
        public ActionResult<ApiResponse<IEnumerable<OrderDetailResponse>>> GetOrderDetails(int pageIndex = 1, int pageSize = 10)
        {
            var response = new ApiResponse<IEnumerable<OrderDetailResponse>>();

            try
            {
                var orderDetails = _unitOfWork.OrderDetailRepository.Get(
                    pageIndex: pageIndex,
                    pageSize: pageSize
                ).Select(od => new OrderDetailResponse
                {
                    Id = od.Id,
                    FoodId = od.FoodId,
                    Price = od.Price,
                    Quantity = od.Quantity,
                    OrderId = od.OrderId,
                    Status = od.Status
                }).ToList();

                response.Ok(orderDetails);
            }
            catch (Exception ex)
            {
                response.Error($"An error occurred while retrieving order details: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            return Ok(response);
        }

        // GET: api/OrderDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<OrderDetailResponse>>> GetOrderDetail(int id)
        {
            var orderDetail = _unitOfWork.OrderDetailRepository.GetByID(id);

            if (orderDetail == null)
            {
                var errorResponse = new ApiResponse<OrderDetailResponse>();
                errorResponse.Error("Order detail not found");
                return NotFound(errorResponse);
            }

            var orderDetailResponse = new OrderDetailResponse
            {
                Id = orderDetail.Id,
                FoodId = orderDetail.FoodId,
                Price = orderDetail.Price,
                Quantity = orderDetail.Quantity,
                OrderId = orderDetail.OrderId,
                Status = orderDetail.Status
            };

            var response = new ApiResponse<OrderDetailResponse>();
            response.Ok(orderDetailResponse);
            return Ok(response);
        }

        // PUT: api/OrderDetails/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderDetail(int id, OrderDetailResponse orderDetailResponse)
        {
            var existingOrderDetail = _unitOfWork.OrderDetailRepository.GetByID(id);
            if (existingOrderDetail == null)
            {
                var errorResponse = new ApiResponse<OrderDetailResponse>();
                errorResponse.Error("Order detail not found");
                return NotFound(errorResponse);
            }

            if (orderDetailResponse.FoodId.HasValue)
            {
                existingOrderDetail.FoodId = orderDetailResponse.FoodId.Value;
            }

            if (orderDetailResponse.Price != default(decimal))
            {
                existingOrderDetail.Price = orderDetailResponse.Price;
            }

            if (orderDetailResponse.Quantity != default(int))
            {
                existingOrderDetail.Quantity = orderDetailResponse.Quantity;
            }

            if (orderDetailResponse.Status != null)
            {
                existingOrderDetail.Status = orderDetailResponse.Status;
            }

            _unitOfWork.OrderDetailRepository.Update(existingOrderDetail);

            try
            {
                _unitOfWork.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await OrderDetailExists(id))
                {
                    var errorResponse = new ApiResponse<OrderDetailResponse>();
                    errorResponse.Error("Order detail not found");
                    return NotFound(errorResponse);
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/OrderDetails
        [HttpPost]
        public async Task<ActionResult<ApiResponse<OrderDetailResponse>>> PostOrderDetail(OrderDetailResponse orderDetailResponse)
        {
            var orderDetail = new OrderDetail
            {
                FoodId = orderDetailResponse.FoodId,
                Price = orderDetailResponse.Price,
                Quantity = orderDetailResponse.Quantity,
                Status = orderDetailResponse.Status,
                               OrderId = orderDetailResponse.OrderId
            };

            _unitOfWork.OrderDetailRepository.Insert(orderDetail);

            try
            {
                _unitOfWork.Save();
            }
            catch (DbUpdateException)
            {
                if (await OrderDetailExists(orderDetail.Id))
                {
                    var errorResponse = new ApiResponse<OrderDetailResponse>();
                    errorResponse.Error("Order detail conflict");
                    return Conflict(errorResponse);
                }
                else
                {
                    throw;
                }
            }

            var order = _unitOfWork.OrderRepository.GetByID(orderDetailResponse.OrderId.GetValueOrDefault());
            if (order != null)
            {
                order.OrderPrice += orderDetail.Price * orderDetail.Quantity;
                _unitOfWork.OrderRepository.Update(order);
                _unitOfWork.Save();
            }

            orderDetailResponse.Id = orderDetail.Id;

            var response = new ApiResponse<OrderDetailResponse>();
            response.Ok(orderDetailResponse);
            return CreatedAtAction("GetOrderDetail", new { id = orderDetail.Id }, response);
        }


        // DELETE: api/OrderDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderDetail(int id)
        {
            var orderDetail = _unitOfWork.OrderDetailRepository.GetByID(id);
            if (orderDetail == null)
            {
                var errorResponse = new ApiResponse<OrderDetailResponse>();
                errorResponse.Error("Order detail not found");
                return NotFound(errorResponse);
            }

            var order = _unitOfWork.OrderRepository.GetByID(orderDetail.OrderId.GetValueOrDefault());
            if (order != null)
            {
                order.OrderPrice -= orderDetail.Price * orderDetail.Quantity;
                _unitOfWork.OrderRepository.Update(order);
                _unitOfWork.OrderDetailRepository.Delete(id);
                _unitOfWork.Save();
            }
            else
            {
                _unitOfWork.OrderDetailRepository.Delete(id);
                _unitOfWork.Save();
            }

            return NoContent();
        }


        private async Task<bool> OrderDetailExists(int id)
        {
            var orderDetail = _unitOfWork.OrderDetailRepository.GetByID(id);
            return orderDetail != null;
        }
    }
}
