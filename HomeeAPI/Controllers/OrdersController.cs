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

namespace HomeeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Orders
        [HttpGet]
        public ActionResult<ApiResponse<IEnumerable<OrderResponse>>> GetOrders(int pageIndex = 1, int pageSize = 10)
        {
            var response = new ApiResponse<IEnumerable<OrderResponse>>();

            try
            {
                var orders = _unitOfWork.OrderRepository.Get(
                    pageIndex: pageIndex,
                    pageSize: pageSize
                ).Select(order => new OrderResponse
                {
                    Id = order.Id,
                    ChefId = order.ChefId,
                    DeliveryAddress = order.DeliveryAddress,
                    OrderPrice = order.OrderPrice,
                    Quantity = order.Quantity,
                    UserId = order.UserId,
                    Status = order.Status,
                    OrderDate = order.OrderDate
                }).ToList();

                response.Ok(orders);
            }
            catch (Exception ex)
            {
                response.Error($"An error occurred while retrieving orders: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            return Ok(response);
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<OrderResponse>>> GetOrder(int id)
        {
            var order = _unitOfWork.OrderRepository.GetByID(id);

            if (order == null)
            {
                var errorResponse = new ApiResponse<OrderResponse>();
                errorResponse.Error("Order not found");
                return NotFound(errorResponse);
            }

            var orderResponse = new OrderResponse
            {
                Id = order.Id,
                ChefId = order.ChefId,
                DeliveryAddress = order.DeliveryAddress,
                OrderPrice = order.OrderPrice,
                Quantity = order.Quantity,
                UserId = order.UserId,
                Status = order.Status,
                OrderDate = order.OrderDate
            };

            var response = new ApiResponse<OrderResponse>();
            response.Ok(orderResponse);
            return Ok(response);
        }

        // PUT: api/Orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, OrderResponse orderResponse)
        {
            var existingOrder = _unitOfWork.OrderRepository.GetByID(id);
            if (existingOrder == null)
            {
                var errorResponse = new ApiResponse<OrderResponse>();
                errorResponse.Error("Order not found");
                return NotFound(errorResponse);
            }

            if (orderResponse.ChefId.HasValue)
            {
                existingOrder.ChefId = orderResponse.ChefId.Value;
            }

            if (orderResponse.DeliveryAddress != null)
            {
                existingOrder.DeliveryAddress = orderResponse.DeliveryAddress;
            }

            existingOrder.OrderPrice = orderResponse.OrderPrice;
            existingOrder.Quantity = orderResponse.Quantity;

            if (orderResponse.UserId.HasValue)
            {
                existingOrder.UserId = orderResponse.UserId.Value;
            }

            if (orderResponse.Status != null)
            {
                existingOrder.Status = orderResponse.Status;
            }

            existingOrder.OrderDate = orderResponse.OrderDate;

            _unitOfWork.OrderRepository.Update(existingOrder);

            try
            {
                _unitOfWork.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await OrderExists(id))
                {
                    var errorResponse = new ApiResponse<OrderResponse>();
                    errorResponse.Error("Order not found");
                    return NotFound(errorResponse);
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<ApiResponse<OrderResponse>>> PostOrder(OrderResponse orderResponse)
        {
            var chef = _unitOfWork.ChefRepository.GetByID(orderResponse.ChefId);
            if (chef == null)
            {
                var errorResponse = new ApiResponse<OrderResponse>();
                errorResponse.Error("Chef not found");
                return NotFound(errorResponse);
            }

            if (chef.Money < 3000000)
            {
                var todayOrdersCount = _unitOfWork.OrderRepository.Get()
                    .Count(order => order.ChefId == orderResponse.ChefId && order.OrderDate.Date == DateTime.Now.Date);

                if (todayOrdersCount > 3)
                {
                    var errorResponse = new ApiResponse<OrderResponse>();
                    errorResponse.Error("Cannot buy from free chef account, please choose another chef");
                    return BadRequest(errorResponse);
                }
            }

            var order = new Order
            {
                ChefId = orderResponse.ChefId,
                DeliveryAddress = orderResponse.DeliveryAddress,
                OrderPrice = orderResponse.OrderPrice,
                Quantity = orderResponse.Quantity,
                UserId = orderResponse.UserId,
                Status = orderResponse.Status,
                OrderDate = orderResponse.OrderDate,
 

            };

            _unitOfWork.OrderRepository.Insert(order);

            try
            {
                _unitOfWork.Save();
            }
            catch (DbUpdateException)
            {
                if (await OrderExists(order.Id))
                {
                    var errorResponse = new ApiResponse<OrderResponse>();
                    errorResponse.Error("Order conflict");
                    return Conflict(errorResponse);
                }
                else
                {
                    throw;
                }
            }

            orderResponse.Id = order.Id;

            var response = new ApiResponse<OrderResponse>();
            response.Ok(orderResponse);
            return CreatedAtAction("GetOrder", new { id = order.Id }, response);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = _unitOfWork.OrderRepository.GetByID(id);
            if (order == null)
            {
                var errorResponse = new ApiResponse<OrderResponse>();
                errorResponse.Error("Order not found");
                return NotFound(errorResponse);
            }

            _unitOfWork.OrderRepository.Delete(id); 
            _unitOfWork.Save();

            return NoContent();
        }
        [HttpGet("by-user")]
        public ActionResult<ApiResponse<IEnumerable<OrderResponse>>> GetOrderByUser(int userId)
        {
            var response = new ApiResponse<IEnumerable<OrderResponse>>();

            try
            {
                var orders = _unitOfWork.OrderRepository.GetAll()
                                                        .Where(order => order.UserId == userId)
                                                        .Select(order => new OrderResponse
                                                        {
                                                            Id = order.Id,
                                                            ChefId = order.ChefId,
                                                            DeliveryAddress = order.DeliveryAddress,
                                                            OrderPrice = order.OrderPrice,
                                                            Quantity = order.Quantity,
                                                            UserId = order.UserId,
                                                            Status = order.Status,
                                                            OrderDate = order.OrderDate
                                                        }).ToList();

                response.Ok(orders);
            }
            catch (Exception ex)
            {
                response.Error($"An error occurred while retrieving order: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            return Ok(response);
        }

        [HttpGet("by-chef")]
        public ActionResult<ApiResponse<IEnumerable<OrderResponse>>> GetOrderByChef(int chefId)
        {
            var response = new ApiResponse<IEnumerable<OrderResponse>>();

            try
            {
                var orders = _unitOfWork.OrderRepository.GetAll()
                                                        .Where(order => order.ChefId == chefId)
                                                        .Select(order => new OrderResponse
                                                        {
                                                            Id = order.Id,
                                                            ChefId = order.ChefId,
                                                            DeliveryAddress = order.DeliveryAddress,
                                                            OrderPrice = order.OrderPrice,
                                                            Quantity = order.Quantity,
                                                            UserId = order.UserId,
                                                            Status = order.Status,
                                                            OrderDate = order.OrderDate
                                                        }).ToList();

                response.Ok(orders);
            }
            catch (Exception ex)
            {
                response.Error($"An error occurred while retrieving order: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            return Ok(response);
        }
        private async Task<bool> OrderExists(int id)
        {
            var order = _unitOfWork.OrderRepository.GetByID(id);
            return order != null;
        }
    }
}
