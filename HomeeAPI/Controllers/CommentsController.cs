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
    public class CommentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommentsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Comments
        [HttpGet]
        public ActionResult<ApiResponse<IEnumerable<CommentResponse>>> GetComments(int pageIndex = 1, int pageSize = 10)
        {
            var response = new ApiResponse<IEnumerable<CommentResponse>>();

            try
            {
                var comments = _unitOfWork.CommentRepository.Get(
                    pageIndex: pageIndex,
                    pageSize: pageSize
                ).Select(comment => new CommentResponse
                {
                    Id = comment.Id,
                    UserId = comment.UserId,
                    Content = comment.Content,
                    SentDate = comment.SentDate,
                    OrderId = comment.OrderId,
                    Star = comment.Star,
                    Status = comment.Status
                }).ToList();

                response.Ok(comments);
            }
            catch (Exception ex)
            {
                response.Error($"An error occurred while retrieving comments: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            return Ok(response);
        }

        // GET: api/Comments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<CommentResponse>>> GetComment(int id)
        {
            var comment = _unitOfWork.CommentRepository.GetByID(id);

            if (comment == null)
            {
                var errorResponse = new ApiResponse<CommentResponse>();
                errorResponse.Error("Comment not found");
                return NotFound(errorResponse);
            }

            var commentResponse = new CommentResponse
            {
                Id = comment.Id,
                UserId = comment.UserId,
                Content = comment.Content,
                SentDate = comment.SentDate,
                OrderId = comment.OrderId,
                Star = comment.Star,
                Status = comment.Status
            };

            var response = new ApiResponse<CommentResponse>();
            response.Ok(commentResponse);
            return Ok(response);
        }

        // PUT: api/Comments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(int id, CommentResponse commentResponse)
        {
            var existingComment = _unitOfWork.CommentRepository.GetByID(id);
            if (existingComment == null)
            {
                var errorResponse = new ApiResponse<CommentResponse>();
                errorResponse.Error("Comment not found");
                return NotFound(errorResponse);
            }

            existingComment.UserId = commentResponse.UserId;
            existingComment.Content = commentResponse.Content;
            existingComment.SentDate = commentResponse.SentDate;
            existingComment.OrderId = commentResponse.OrderId;
            existingComment.Star = commentResponse.Star;
            existingComment.Status = commentResponse.Status;

            _unitOfWork.CommentRepository.Update(existingComment);

            try
            {
                _unitOfWork.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CommentExists(id))
                {
                    var errorResponse = new ApiResponse<CommentResponse>();
                    errorResponse.Error("Comment not found");
                    return NotFound(errorResponse);
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Comments
        [HttpPost]
        public async Task<ActionResult<ApiResponse<CommentResponse>>> PostComment(CommentResponse commentResponse)
        {
            var comment = new Comment
            {
                UserId = commentResponse.UserId,
                Content = commentResponse.Content,
                SentDate = commentResponse.SentDate,
                OrderId = commentResponse.OrderId,
                Star = commentResponse.Star,
                Status = commentResponse.Status
            };

            _unitOfWork.CommentRepository.Insert(comment);

            try
            {
                _unitOfWork.Save();
            }
            catch (DbUpdateException)
            {
                if (await CommentExists(comment.Id))
                {
                    var errorResponse = new ApiResponse<CommentResponse>();
                    errorResponse.Error("Comment conflict");
                    return Conflict(errorResponse);
                }
                else
                {
                    throw;
                }
            }

            commentResponse.Id = comment.Id;

            var response = new ApiResponse<CommentResponse>();
            response.Ok(commentResponse);
            return CreatedAtAction("GetComment", new { id = comment.Id }, response);
        }

        // DELETE: api/Comments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = _unitOfWork.CommentRepository.GetByID(id);
            if (comment == null)
            {
                var errorResponse = new ApiResponse<CommentResponse>();
                errorResponse.Error("Comment not found");
                return NotFound(errorResponse);
            }

            _unitOfWork.CommentRepository.Delete(id);
            _unitOfWork.Save();

            return NoContent();
        }

        private async Task<bool> CommentExists(int id)
        {
            var comment = _unitOfWork.CommentRepository.GetByID(id);
            return comment != null;
        }
        // GET: api/Comments/byChef/{chefId}
        [HttpGet("byChef/{chefId}")]
        public ActionResult<ApiResponse<IEnumerable<CommentResponse>>> GetCommentsByChefId(int chefId, int pageIndex = 1, int pageSize = 10)
        {
            var response = new ApiResponse<IEnumerable<CommentResponse>>();

            try
            {
                var comments = _unitOfWork.CommentRepository
                    .Get(
                        filter: c => c.Order.ChefId == chefId,
                        orderBy: q => q.OrderByDescending(c => c.SentDate),
                        pageIndex: pageIndex,
                        pageSize: pageSize
                    )
                    .Select(comment => new CommentResponse
                    {
                        Id = comment.Id,
                        UserId = comment.UserId,
                        Content = comment.Content,
                        SentDate = comment.SentDate,
                        OrderId = comment.OrderId,
                        Star = comment.Star,
                        Status = comment.Status
                    }).ToList();

                response.Ok(comments);
            }
            catch (Exception ex)
            {
                response.Error($"An error occurred while retrieving comments for chef with id {chefId}: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            return Ok(response);
        }
    }
}
