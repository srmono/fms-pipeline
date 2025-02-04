namespace fleetmanagement.dtos
{
    public class ValidationFailure
    {
        public string? Message { get; set; }
        public string? Details { get; set; }

        public ValidationFailure(string message, string? details = null)
        {
            Message = message;
            Details = details;
        }
    }
}
