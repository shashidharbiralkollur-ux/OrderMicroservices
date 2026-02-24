using OrderService.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddHttpClient();

var app = builder.Build();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();