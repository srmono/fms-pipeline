namespace fleetmanagement.dtos;

public class ValidationErrorDetail
{
    public string FieldName { get; set; }
    public string ErrorMessage { get; set; }

    public ValidationErrorDetail(string fieldName, string errorMessage)
    {
        FieldName = fieldName;
        ErrorMessage = errorMessage;
    }
}
