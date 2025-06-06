namespace Production.DTO;

public class ServiceResponse
{
    public bool IsSuccess { get; set; }
    public string ErrorMessage { get; set; }

    public ServiceResponse()
    {
        
    }
    public ServiceResponse(bool isSuccess, string errorMessage)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }
}