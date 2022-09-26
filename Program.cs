using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using MovieApi.Common;
using MovieApi.OpenApiSecurity;
using MovieApi.Repository;
using MovieApi.Service;

namespace MovieApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddTransient<ILogger, MovieLogger>();
            builder.Services.AddTransient<IMovieService, MovieService>();
            builder.Services.AddTransient<IMovieRepository, MovieRepository>();

            // OAuth - Add Authentication Services
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}/";
                options.Audience = builder.Configuration["Auth0:Audience"];
            });

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                //OAuth
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyProject", Version = "v1.0.0" });

                string securityDefinitionName = builder.Configuration["SwaggerUISecurityMode"] ?? "Bearer";
                OpenApiSecurityScheme securityScheme = new OpenApiBearerSecurityScheme();
                OpenApiSecurityRequirement securityRequirement = new OpenApiBearerSecurityRequirement(securityScheme);

                if (securityDefinitionName.ToLower() == "oauth2")
                {
                    securityScheme = new OpenApiOAuthSecurityScheme(builder.Configuration["Auth0:Domain"], builder.Configuration["Auth0:Audience"]);
                    securityRequirement = new OpenApiOAuthSecurityRequirement();
                }

                c.AddSecurityDefinition(securityDefinitionName, securityScheme);

                c.AddSecurityRequirement(securityRequirement);
            });

            //OAuth
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("ReadAccess", policy =>
                                  policy.RequireClaim("permissions", "read:movies"));
                options.AddPolicy("AddAccess", policy =>
                                  policy.RequireClaim("permissions", "add:movies"));
                options.AddPolicy("EditAccess", policy =>
                                  policy.RequireClaim("permissions", "edit:movies"));
                options.AddPolicy("DeleteAccess", policy =>
                                  policy.RequireClaim("permissions", "delete:term"));
            });

           
            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(
                    //OAuth
                    c =>
                    {
                        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Movie Api v1");

                        if (builder.Configuration["SwaggerUISecurityMode"]?.ToLower() == "oauth2")
                        {
                            c.OAuthClientId(builder.Configuration["Auth0:ClientId"]);
                            c.OAuthClientSecret(builder.Configuration["Auth0:ClientSecret"]);
                            c.OAuthAppName("Movie Client");
                            c.OAuthAdditionalQueryStringParams(new Dictionary<string, string> { { "audience", builder.Configuration["Auth0:Audience"] } });
                            c.OAuthUsePkce();
                        }
                    });
            }

            app.UseHttpsRedirection();

            // OAuth - Enable authentication middleware
            app.UseAuthentication();
            app.UseAuthorization();

            app.ConfigureExceptionHandler(app.Environment, new MovieLogger());

            app.MapControllers();

            app.Run();
        }
    }

}