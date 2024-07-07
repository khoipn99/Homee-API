namespace HomeeAPI.DTO
{
    public class ApiResponse<T>
    {
        public StatusEnum Status { get; set; }
        public T Payload { get; set; }
        public object Message { get; set; }
        public Dictionary<string, object> Metadata { get; set; }

        public void Ok()
        {
            this.Status = StatusEnum.SUCCESS;
        }

        public void Ok(T data)
        {
            this.Status = StatusEnum.SUCCESS;
            this.Payload = data;
        }

        public void OkV2(object message)
        {
            this.Status = StatusEnum.SUCCESS;
            this.Message = message;
        }

        public void Ok(T data, Dictionary<string, object> metadata)
        {
            this.Status = StatusEnum.SUCCESS;
            this.Payload = data;
            this.Metadata = metadata;
        }

        public void Error(object message)
        {
            this.Status = StatusEnum.ERROR;
            this.Message = message;
        }
    }
}
