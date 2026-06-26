using SalesInventory.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// In this learning environment, console logging is enough and avoids Windows Event Log permission issues.
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.

// Registers controller support so ASP.NET Core can discover classes that inherit from ControllerBase.
builder.Services.AddControllers();

// Registers our application service in the DI container.
// Controllers ask for ICategoryService, and ASP.NET Core provides CategoryService.
builder.Services.AddSingleton<ICategoryService, CategoryService>();
builder.Services.AddSingleton<IProductService, ProductService>();

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
