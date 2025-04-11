using Figgle;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using MotoLocadora.Application.Extensions;
using MotoLocadora.BuildingBlocks.Entities;
using MotoLocadora.BuildingBlocks.Options;
using MotoLocadora.Infraestructure.Ioc;
using MotoLocadora.Infrastructure.Context;
using MotoLocadora.Infrastructure.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Exibir banner ascii no startup
var asciiArt = FiggleFonts.Standard.Render("MOTOLOCADORA");
Console.WriteLine(asciiArt);

// Configuração das opções de connection string e jwt
var connectionStringOptions = new ConnectionStringOptions();
builder.Configuration.GetSection(ConnectionStringOptions.SectionName).Bind(connectionStringOptions);
var jwtOptions = new JwtOptions();
builder.Configuration.GetSection(JwtOptions.SectionName).Bind(jwtOptions);

// ? Centralizamos a injeção no método AddInfrastructure
builder.Services.AddInfraestructure(builder.Configuration);
builder.Services.AddApplicationServices();

// Configuração do Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
    .AddDefaultTokenProviders();

// ? Registrar Email Sender para o Identity API
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, EmailSender>();

// Configuração do JWT
var key = Encoding.UTF8.GetBytes(jwtOptions.Key);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidAudience = jwtOptions.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();

builder.Services.AddControllers(options =>
{
    var policy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()
    .Build();

    options.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.CustomSchemaIds(type => type.FullName);
});

var app = builder.Build();

// Configuração do pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Mapear os endpoints automáticos do Identity API
app.MapIdentityApi<ApplicationUser>();

app.MapControllers();

app.Run();