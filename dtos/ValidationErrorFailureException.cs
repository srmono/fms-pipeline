using System;

//namespace fleetmanagement.dtos;

// public class ValidationErrorFaulureException : Exception
// {
//     public IEnumerable<ValidationErrorDetail> Failures { get; }

//     public ValidationErrorFaulureException(IEnumerable<ValidationErrorDetail> failures)
//     {
//         Failures = failures;
//     }
// }



namespace fleetmanagement.dtos
{
    public class ValidationErrorFaulureException : Exception
    {
        public List<ValidationErrorDetail> Errors { get; }

        public ValidationErrorFaulureException(IEnumerable<ValidationErrorDetail> errors)
            : base("Validation failed")
        {
            Errors = errors.ToList();
        }
    }

    // public class ValidationErrorDetail
    // {
    //     public string Field { get; }
    //     public string Message { get; }

    //     public ValidationErrorDetail(string field, string message)
    //     {
    //         Field = field;
    //         Message = message;
    //     }
    // }
}
