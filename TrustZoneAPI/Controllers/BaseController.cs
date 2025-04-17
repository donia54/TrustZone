using Microsoft.AspNetCore.Mvc;
using TrustZoneAPI.Services;

namespace TrustZoneAPI.Controllers
{
    public class BaseController : ControllerBase
    {
        // This method handles the response based on the status code returned.
        // It checks the status code and returns the appropriate HTTP response.

        public string CurrentUserId => HttpContext.Items["UserId"] as string ?? string.Empty;


        protected ActionResult MapResponseToActionResult<T>(ResponseResult<T> response)
        {
            try
            {
                // Switch case based on status code of the response
                switch (response.StatusCode)
                {
                    case StatusCodes.Status200OK: // OK: Success
                        return Ok(response.Data);  // Returns 200 OK with the data

                    case StatusCodes.Status201Created: // Created: Resource was created
                        return Created(string.Empty, response.Data);  // Returns 201 Created with the data

                    case StatusCodes.Status204NoContent: // No Content: Successful but no content to return
                        return NoContent();  // Returns 204 No Content

                    case StatusCodes.Status400BadRequest: // Bad Request: Invalid request from the client
                        return BadRequest(response.ErrorMessage);  // Returns 400 with error message

                    case StatusCodes.Status401Unauthorized: // Unauthorized: Authentication is required
                        return Unauthorized(response.ErrorMessage);  // Returns 401 Unauthorized with error message

                    case StatusCodes.Status403Forbidden: // Forbidden: Server understands the request, but refuses to authorize it
                        return StatusCode(StatusCodes.Status403Forbidden, response.ErrorMessage);// Returns 403 Forbidden

                    case StatusCodes.Status404NotFound: // Not Found: Requested resource could not be found
                        return NotFound(response.ErrorMessage);  // Returns 404 Not Found with error message

                    case StatusCodes.Status405MethodNotAllowed: // Method Not Allowed: The HTTP method used is not allowed for the resource
                        return StatusCode(StatusCodes.Status405MethodNotAllowed, response.ErrorMessage);  // Returns 405 Method Not Allowed with error message

                    case StatusCodes.Status409Conflict: // Conflict: There is a conflict with the current state of the resource
                        return Conflict(response.ErrorMessage);  // Returns 409 Conflict with error message

                    case StatusCodes.Status422UnprocessableEntity: // Unprocessable Entity: The request was well-formed, but the server couldn't process it
                        return UnprocessableEntity(response.ErrorMessage);  // Returns 422 Unprocessable Entity with error message

                    case StatusCodes.Status500InternalServerError: // Internal Server Error: A generic error occurred on the server
                        return StatusCode(StatusCodes.Status500InternalServerError, response.ErrorMessage);  // Returns 500 Internal Server Error with error message

                    case StatusCodes.Status502BadGateway: // Bad Gateway: Invalid response from the upstream server
                        return StatusCode(StatusCodes.Status502BadGateway, response.ErrorMessage);  // Returns 502 Bad Gateway with error message

                    case StatusCodes.Status503ServiceUnavailable: // Service Unavailable: Server is temporarily unavailable (e.g., overloaded or down for maintenance)
                        return StatusCode(StatusCodes.Status503ServiceUnavailable, response.ErrorMessage);  // Returns 503 Service Unavailable with error message

                    case StatusCodes.Status504GatewayTimeout: // Gateway Timeout: The upstream server failed to send a response in time
                        return StatusCode(StatusCodes.Status504GatewayTimeout, response.ErrorMessage);  // Returns 504 Gateway Timeout with error message

                    default:
                        // If the status code does not match any predefined status, return a generic error
                        // This acts as a fallback for unexpected status codes
                        return StatusCode(StatusCodes.Status500InternalServerError,
                                           $"Unexpected error: {response.ErrorMessage ?? "An unexpected error occurred."}");
                }
            }
            catch (Exception ex)
            {
                //   return StatusCode(StatusCodes.Status500InternalServerError,
                //                    $"An error occurred while processing the request: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request");
            }
        }
        protected ActionResult MapResponseToActionResult(ResponseResult response)
        {
            try
            {
                // Switch case based on status code of the response
                switch (response.StatusCode)
                {
                    case StatusCodes.Status200OK: // OK: Success
                        return Ok();  // Returns 200 OK with the data

                    case StatusCodes.Status201Created: // Created: Resource was created
                        return Created();  // Returns 201 Created with the data

                    case StatusCodes.Status204NoContent: // No Content: Successful but no content to return
                        return NoContent();  // Returns 204 No Content

                    case StatusCodes.Status400BadRequest: // Bad Request: Invalid request from the client
                        return BadRequest(response.ErrorMessage);  // Returns 400 with error message

                    case StatusCodes.Status401Unauthorized: // Unauthorized: Authentication is required
                        return Unauthorized(response.ErrorMessage);  // Returns 401 Unauthorized with error message

                    case StatusCodes.Status403Forbidden: // Forbidden: Server understands the request, but refuses to authorize it
                        return Forbid();  // Returns 403 Forbidden

                    case StatusCodes.Status404NotFound: // Not Found: Requested resource could not be found
                        return NotFound(response.ErrorMessage);  // Returns 404 Not Found with error message

                    case StatusCodes.Status405MethodNotAllowed: // Method Not Allowed: The HTTP method used is not allowed for the resource
                        return StatusCode(StatusCodes.Status405MethodNotAllowed, response.ErrorMessage);  // Returns 405 Method Not Allowed with error message

                    case StatusCodes.Status409Conflict: // Conflict: There is a conflict with the current state of the resource
                        return Conflict(response.ErrorMessage);  // Returns 409 Conflict with error message

                    case StatusCodes.Status422UnprocessableEntity: // Unprocessable Entity: The request was well-formed, but the server couldn't process it
                        return UnprocessableEntity(response.ErrorMessage);  // Returns 422 Unprocessable Entity with error message

                    case StatusCodes.Status500InternalServerError: // Internal Server Error: A generic error occurred on the server
                        return StatusCode(StatusCodes.Status500InternalServerError, response.ErrorMessage);  // Returns 500 Internal Server Error with error message

                    case StatusCodes.Status502BadGateway: // Bad Gateway: Invalid response from the upstream server
                        return StatusCode(StatusCodes.Status502BadGateway, response.ErrorMessage);  // Returns 502 Bad Gateway with error message

                    case StatusCodes.Status503ServiceUnavailable: // Service Unavailable: Server is temporarily unavailable (e.g., overloaded or down for maintenance)
                        return StatusCode(StatusCodes.Status503ServiceUnavailable, response.ErrorMessage);  // Returns 503 Service Unavailable with error message

                    case StatusCodes.Status504GatewayTimeout: // Gateway Timeout: The upstream server failed to send a response in time
                        return StatusCode(StatusCodes.Status504GatewayTimeout, response.ErrorMessage);  // Returns 504 Gateway Timeout with error message

                    default:
                        // If the status code does not match any predefined status, return a generic error
                        // This acts as a fallback for unexpected status codes
                        return StatusCode(StatusCodes.Status500InternalServerError,
                                           $"Unexpected error: {response.ErrorMessage ?? "An unexpected error occurred."}");
                }
            }
            catch (Exception ex)
            {
                //   return StatusCode(StatusCodes.Status500InternalServerError,
                //                    $"An error occurred while processing the request: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request");
            }
        }
    }
}