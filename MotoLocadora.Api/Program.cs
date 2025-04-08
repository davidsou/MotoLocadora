using Figgle;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
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

// Configura��o das op��es de connection string e jwt
var connectionStringOptions = new ConnectionStringOptions();
builder.Configuration.GetSection(ConnectionStringOptions.SectionName).Bind(connectionStringOptions);
var jwtOptions = new JwtOptions();
builder.Configuration.GetSection(JwtOptions.SectionName).Bind(jwtOptions);

// ? Centralizamos a inje��o no m�todo AddInfrastructure
builder.Services.AddInfraestructure(builder.Configuration);
builder.Services.AddApplicationServices();

// Configura��o do Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
    .AddDefaultTokenProviders();

// ? Registrar Email Sender para o Identity API
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, EmailSender>();

// Configura��o do JWT
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

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configura��o do pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Mapear os endpoints autom�ticos do Identity API
app.MapIdentityApi<ApplicationUser>();

app.MapControllers();

app.Run();