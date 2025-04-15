using Amazon.S3;
using Figgle;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using MotoLocadora.Application.Extensions;
using MotoLocadora.BuildingBlocks.Entities;
using MotoLocadora.BuildingBlocks.Interfaces;
using MotoLocadora.BuildingBlocks.Options;
using MotoLocadora.Infraestructure.Ioc;
using MotoLocadora.Infrastructure.Context;
using MotoLocadora.Infrastructure.Seeders;
using MotoLocadora.Infrastructure.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Exibir banner ascii no startup
var asciiArt = FiggleFonts.Standard.Render("MOTOLOCADORA");
Console.WriteLine(asciiArt);

// Configuração das options : connectionstring , jwt, seeder, eventbus, S3
var connectionStringOptions = new ConnectionStringOptions();
builder.Configuration.GetSection(ConnectionStringOptions.SectionName).Bind(connectionStringOptions);
var jwtOptions = new JwtOptions();
builder.Configuration.GetSection(JwtOptions.SectionName).Bind(jwtOptions);
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));
builder.Services.Configure<AdminSeedOptions>(builder.Configuration.GetSection(AdminSeedOptions.SectionName));
builder.Services.Configure<EventBusOptions>(builder.Configuration.GetSection(EventBusOptions.SectionName));
builder.Services.Configure<S3Options>(builder.Configuration.GetSection(S3Options.SectionName));


// Registra o seeder
builder.Services.AddScoped<ApplicationSeeder>();

// Centralizamos a injeção no método AddInfrastructure
builder.Services.AddInfraestructure(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddHostedService<RabbitMqConsumer>();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddScoped<IStorageService, LocalStorageService>();
}
else
{
    builder.Services.AddAWSService<IAmazonS3>();
    builder.Services.AddScoped<IStorageService, S3StorageService>();
}


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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "MotoLocadora API",
        Version = "v1"
    });

    c.CustomSchemaIds(type => type.FullName);

    var securityScheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Insira o token JWT no formato: Bearer {token}",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new Microsoft.OpenApi.Models.OpenApiReference
        {
            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });

    c.SupportNonNullableReferenceTypes();
    c.UseInlineDefinitionsForEnums();
});



var app = builder.Build();

// Seeder generico.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppSqlContext>();
    dbContext.Database.Migrate();

    var seeder = scope.ServiceProvider.GetRequiredService<ApplicationSeeder>();
    await seeder.SeedAsync();
}

// Configuração do pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MotoLocadora API v1");
        c.InjectJavascript("/swagger-custom.js");
    });
}
// Pra formatar a URL dos arquivos em ambiente de desenvolvimento
var storagePath = Path.Combine(app.Environment.ContentRootPath, "Storage");

if (!Directory.Exists(storagePath))
    Directory.CreateDirectory(storagePath);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(storagePath),
    RequestPath = "/storage"
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();