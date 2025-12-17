namespace DTO.Client
{
    public class ClientErrorDto
    {
        public string? Message { get; set; }
        public string? StackTrace { get; set; }
        public string? Url { get; set; }
        public string? UserAgent { get; set; }
        public DateTime OccurredAt { get; set; }
    }
}
