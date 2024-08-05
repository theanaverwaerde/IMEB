using System.Security.Claims;

namespace IMEB.Common;

public static class ClaimExtensions
{
    public static string GetOrganizationName(this ClaimsPrincipal principal)
    {
        string value = principal.Claims.FirstOrDefault(claim => claim.Type == "organization")?.Value ?? throw new NullReferenceException();
        
        int startIndex = value.IndexOf('\"') + 1;
        int endIndex = value.IndexOf('\"', startIndex);
        return value.Substring(startIndex, endIndex - startIndex);
    }
}