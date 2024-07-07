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
    public class PaymentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Payments
        [HttpGet]
        public ActionResult<ApiResponse<IEnumerable<PaymentResponse>>> GetPayments(int pageIndex = 1, int pageSize = 10)
        {
            var response = new ApiResponse<IEnumerable<PaymentResponse>>();

            try
            {
                var payments = _unitOfWork.PaymentRepository.Get(
                    pageIndex: pageIndex,
                    pageSize: pageSize
                ).Select(payment => new PaymentResponse
                {
                    Id = payment.Id,
                    OrderId = payment.OrderId,
                    PaymentDate = payment.PaymentDate,
                    TotalPrice = payment.TotalPrice,
                    PaymentType = payment.PaymentType,
                    Discount = payment.Discount,
                    UserId = payment.UserId,
                    Status = payment.Status
                }).ToList();

                response.Ok(payments);
            }
            catch (Exception ex)
            {
                response.Error($"An error occurred while retrieving payments: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            return Ok(response);
        }

        // GET: api/Payments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<PaymentResponse>>> GetPayment(int id)
        {
            var payment = _unitOfWork.PaymentRepository.GetByID(id);

            if (payment == null)
            {
                var errorResponse = new ApiResponse<PaymentResponse>();
                errorResponse.Error("Payment not found");
                return NotFound(errorResponse);
            }

            var paymentResponse = new PaymentResponse
            {
                Id = payment.Id,
                OrderId = payment.OrderId,
                PaymentDate = payment.PaymentDate,
                TotalPrice = payment.TotalPrice,
                PaymentType = payment.PaymentType,
                Discount = payment.Discount,
                UserId = payment.UserId,
                Status = payment.Status
            };

            var response = new ApiResponse<PaymentResponse>();
            response.Ok(paymentResponse);
            return Ok(response);
        }

        // PUT: api/Payments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPayment(int id, PaymentResponse paymentResponse)
        {
            var existingPayment = _unitOfWork.PaymentRepository.GetByID(id);
            if (existingPayment == null)
            {
                var errorResponse = new ApiResponse<PaymentResponse>();
                errorResponse.Error("Payment not found");
                return NotFound(errorResponse);
            }

            if (paymentResponse.OrderId.HasValue)
            {
                existingPayment.OrderId = paymentResponse.OrderId.Value;
            }

            existingPayment.PaymentDate = paymentResponse.PaymentDate;

            existingPayment.TotalPrice = paymentResponse.TotalPrice;

            if (paymentResponse.PaymentType != null)
            {
                existingPayment.PaymentType = paymentResponse.PaymentType;
            }

            if (paymentResponse.Discount.HasValue)
            {
                existingPayment.Discount = paymentResponse.Discount.Value;
            }

            if (paymentResponse.UserId.HasValue)
            {
                existingPayment.UserId = paymentResponse.UserId.Value;
            }

            if (paymentResponse.Status != null)
            {
                existingPayment.Status = paymentResponse.Status;
            }

            _unitOfWork.PaymentRepository.Update(existingPayment);

            try
            {
                _unitOfWork.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await PaymentExists(id))
                {
                    var errorResponse = new ApiResponse<PaymentResponse>();
                    errorResponse.Error("Payment not found");
                    return NotFound(errorResponse);
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<PaymentResponse>>> PostPayment(PaymentResponse paymentResponse)
        {
            var response = new ApiResponse<PaymentResponse>();

            try
            {
                var user = _unitOfWork.UserRepository.GetByID(paymentResponse.UserId);

                if (user == null)
                {
                    response.Error("User not found");
                    return NotFound(response);
                }

                var order = _unitOfWork.OrderRepository.GetByID(paymentResponse.OrderId);
                if (order == null)
                {
                    response.Error("Order not found");
                    return NotFound(response);
                }

                var chef = _unitOfWork.ChefRepository.GetByID(order.ChefId);
                if (chef == null)
                {
                    response.Error("Chef not found");
                    return NotFound(response);
                }

                if (user.Money > 300000)
                {
                    paymentResponse.Discount = (paymentResponse.TotalPrice * 5) / 100;
                    paymentResponse.TotalPrice -= paymentResponse.Discount.Value;
                }

                var chefMoneyDeduction = (paymentResponse.TotalPrice * 8) / 100;

                chef.Money -= chefMoneyDeduction;
                _unitOfWork.ChefRepository.Update(chef);

                var payment = new Payment
                {
                    OrderId = paymentResponse.OrderId,
                    PaymentDate = paymentResponse.PaymentDate,
                    TotalPrice = paymentResponse.TotalPrice,
                    PaymentType = paymentResponse.PaymentType,
                    Discount = paymentResponse.Discount,
                    UserId = paymentResponse.UserId,
                    Status = paymentResponse.Status
                };

                _unitOfWork.PaymentRepository.Insert(payment);
                _unitOfWork.Save();

                paymentResponse.Id = payment.Id;
                response.Ok(paymentResponse);
                return CreatedAtAction("GetPayment", new { id = payment.Id }, response);
            }
            catch (DbUpdateException ex)
            {
                if (await PaymentExists(paymentResponse.Id))
                {
                    response.Error("Payment conflict");
                    return Conflict(response);
                }
                else
                {
                    response.Error($"An error occurred while creating the payment: {ex.Message}");
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                }
            }
        }


        // DELETE: api/Payments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var payment = _unitOfWork.PaymentRepository.GetByID(id);
            if (payment == null)
            {
                var errorResponse = new ApiResponse<PaymentResponse>();
                errorResponse.Error("Payment not found");
                return NotFound(errorResponse);
            }

            _unitOfWork.PaymentRepository.Delete(id);
            _unitOfWork.Save();

            return NoContent();
        }

        private async Task<bool> PaymentExists(int id)
        {
            var payment = _unitOfWork.PaymentRepository.GetByID(id);
            return payment != null;
        }


    }
}
