using Meisy.API.BackgroundServices;
using Meisy.API.Filters;
using Meisy.API.Token;
using Meisy.Application;
using Meisy.Domain.Security.Token;
using Meisy.Infrastructure;
using Meisy.Infrastructure.Migrations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "I don't know",
        In = ParameterLocation.Header,
        Scheme = "Bearer",
        Type = SecuritySchemeType.ApiKey
    });

    config.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth12",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }

    });
});



// dependency injection
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddHostedService<DeliveryReminderBackgroundService>();

// exception filter
builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));

// token configs

builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = new TimeSpan(0),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Settings:Jwt:SigningKey")!))
    };
});

builder.Services.AddHttpContextAccessor();

// cors config

builder.Services.AddCors(options =>
{
    options.AddPolicy("client", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173", "https://meisy-client.pages.dev")     //
            .AllowAnyMethod()     // GET, POST, PUT...
            .AllowAnyHeader();    // permite qualquer header
    });
});


//

var app = builder.Build();

// use cors
app.UseCors("client");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await MigrateDatabase();

app.Run();

async Task MigrateDatabase()
{
    await using var scope = app.Services.CreateAsyncScope();
    await DatabaseMigration.MigrateDatabase(scope.ServiceProvider);
}