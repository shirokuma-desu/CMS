using CMS_API.JWTService;
using CMS_API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var config = builder.Configuration;

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<CMSContext>();

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // Remember to set to true on production
                    ValidateIssuerSigningKey = false,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
                        (config["JwtToken:NotTokenKeyForSureSourceTrustMeDude"])),
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = config["JwtToken:Issuer"],
                    ValidAudience = config["JwtToken:Audience"]
                };
            });

builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("Admin", policy => policy.RequireClaim("Admin"));
    o.AddPolicy("Teacher", policy => policy.RequireClaim("Teacher"));
    o.AddPolicy("Student", policy => policy.RequireClaim("Student"));
});

builder.Services.AddSwaggerGen(
            c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo { Title = "Test Api", Version = "v1" });
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.OperationFilter<SecurityRequirementsOperationFilter>();
                //Enabla api summary
                c.EnableAnnotations();
            }
        );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
