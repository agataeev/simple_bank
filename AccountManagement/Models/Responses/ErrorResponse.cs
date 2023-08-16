using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace AccountManagement.Models.Responses;

public static class ErrorResponse
{
    public static ObjectResult BadRequest(string massage)
    {
        return CommonError(HttpStatusCode.BadRequest, massage);
    }
    
    public static ObjectResult InternalServerError(string massage)
    {
        return CommonError(HttpStatusCode.InternalServerError, massage);
    }

    public static ObjectResult Conflict(string massage)
    {
        return CommonError(HttpStatusCode.Conflict, massage);
    }
    
    public static ObjectResult CommonError(HttpStatusCode status, string massage)
    {
        var problemDetails = new ProblemDetails
        {
            Detail = massage,
            Status = (int)status,
            Title = status.ToString()
        };
        
        return new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };
    }
}