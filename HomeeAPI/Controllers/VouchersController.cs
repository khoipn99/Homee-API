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

namespace HomeeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VouchersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public VouchersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Vouchers
        [HttpGet]
        public ActionResult<ApiResponse<IEnumerable<VoucherResponse>>> GetVouchers()
        {
            var response = new ApiResponse<IEnumerable<VoucherResponse>>();

            try
            {
                var vouchers = _unitOfWork.VoucherRepository.Get()
                                    .Select(voucher => new VoucherResponse
                                    {
                                        Id = voucher.Id,
                                        Name = voucher.Name,
                                        Discount = voucher.Discount,
                                        Quantity = voucher.Quantity
                                    }).ToList();

                response.Ok(vouchers);
            }
            catch (Exception ex)
            {
                response.Error($"An error occurred while retrieving vouchers: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            return Ok(response);
        }

        // GET: api/Vouchers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<VoucherResponse>>> GetVoucher(int id)
        {
            var voucher = _unitOfWork.VoucherRepository.GetByID(id);

            if (voucher == null)
            {
                var errorResponse = new ApiResponse<VoucherResponse>();
                errorResponse.Error("Voucher not found");
                return NotFound(errorResponse);
            }

            var voucherResponse = new VoucherResponse
            {
                Id = voucher.Id,
                Name = voucher.Name,
                Discount = voucher.Discount,
                Quantity = voucher.Quantity
            };

            var response = new ApiResponse<VoucherResponse>();
            response.Ok(voucherResponse);
            return Ok(response);
        }

        // POST: api/Vouchers
        [HttpPost]
        public async Task<ActionResult<ApiResponse<VoucherResponse>>> PostVoucher(Voucher voucher)
        {
            _unitOfWork.VoucherRepository.Insert(voucher);

            try
            {
                _unitOfWork.Save();
            }
            catch (DbUpdateException)
            {
                if (await VoucherExists(voucher.Id))
                {
                    var errorResponse = new ApiResponse<VoucherResponse>();
                    errorResponse.Error("Voucher conflict");
                    return Conflict(errorResponse);
                }
                else
                {
                    throw;
                }
            }

            var voucherResponse = new VoucherResponse
            {
                Id = voucher.Id,
                Name = voucher.Name,
                Discount = voucher.Discount,
                Quantity = voucher.Quantity
            };

            var response = new ApiResponse<VoucherResponse>();
            response.Ok(voucherResponse);
            return CreatedAtAction("GetVoucher", new { id = voucher.Id }, response);
        }

        // DELETE: api/Vouchers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVoucher(int id)
        {
            var voucher = _unitOfWork.VoucherRepository.GetByID(id);
            if (voucher == null)
            {
                var errorResponse = new ApiResponse<VoucherResponse>();
                errorResponse.Error("Voucher not found");
                return NotFound(errorResponse);
            }

            _unitOfWork.VoucherRepository.Delete(id);
            _unitOfWork.Save();

            return NoContent();
        }

        private async Task<bool> VoucherExists(int id)
        {
            var voucher = _unitOfWork.VoucherRepository.GetByID(id);
            return voucher != null;
        }
    }
}
