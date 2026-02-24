using InventoryService.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add controllers
builder.Services.AddControllers();

// Register Inventory repository
builder.Services.AddScoped<InventoryRepository>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();

// Map controllers
app.MapControllers();

app.Run();
