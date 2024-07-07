namespace HomeeAPI.DTO.ResponseObject
{
    public class CommentResponse
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public string Content { get; set; }

        public DateTime SentDate { get; set; }

        public int? OrderId { get; set; }

        public int? Star { get; set; }

        public string? Status { get; set; }
    }
}
