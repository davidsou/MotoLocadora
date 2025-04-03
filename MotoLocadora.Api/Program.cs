using Figgle;
using MotoLocadora.BuildingBlocks.Options;
using MotoLocadora.Infraestructure.Ioc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var asciiArt = FiggleFonts.Standard.Render("FT Folha");
Console.WriteLine(asciiArt);


var connectionStringOptions = new ConnectionStringOptions();
builder.Configuration.GetSection(ConnectionStringOptions.SectionName).Bind(connectionStringOptions);

builder.Services.AddInfraestructure(builder.Configuration);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
