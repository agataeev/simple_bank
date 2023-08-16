using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace AccountManagement.Models.Responses;

public class SuccessResponse
{
    public static ObjectResult CommonResponse(object? data)
    {
        var response = new ObjectResult(data);
        response.ContentTypes.Add("application/json");
        response.StatusCode = (int)HttpStatusCode.OK;
        return response;
    }
}