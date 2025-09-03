using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
public class ApiResponseFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context) { }

  public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult objectResult)
            {
                bool isSuccess = objectResult.StatusCode >= 200 && objectResult.StatusCode < 300;

                var apiResponse = new ApiResponseDto<object>(
                    isSuccess,
                    isSuccess ? "Success" : "Failure",
                    null 
                );

                if (objectResult.Value is List<string> errorList && !isSuccess)
                {
                apiResponse.Errors = errorList;
                }
                else
                {
                    apiResponse.Data = objectResult.Value;
                }

                context.Result = new ObjectResult(apiResponse)
                {
                    StatusCode = objectResult.StatusCode
                };
            }
        }
    }

