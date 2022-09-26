using Microsoft.OpenApi.Models;

namespace MovieApi.OpenApiSecurity
{
    public class OpenApiBearerSecurityRequirement: OpenApiSecurityRequirement
  {
    public OpenApiBearerSecurityRequirement(OpenApiSecurityScheme securityScheme)
    {
      this.Add(securityScheme, new[] { "Bearer" });
    }
  }
}
