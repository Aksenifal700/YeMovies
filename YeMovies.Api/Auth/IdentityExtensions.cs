namespace YeMoviesApi.Auth;

public static class IdentityExtensions
{
    public static Guid? GetUserId(this HttpContext httpContext)
    {
        var userid = httpContext.User.Claims.FirstOrDefault(c => c.Type == "userid");

        if (Guid.TryParse(userid?.Value, out var parseId))
        {
            return parseId;
        }
        
        return null;
    }
}