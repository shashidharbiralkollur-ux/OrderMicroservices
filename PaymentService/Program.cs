using PaymentService.Repository;

var builder = WebApplication.CreateBuilder(args);

// Controllers only
builder.Services.AddControllers();

builder.Services.AddScoped<PaymentRepository>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();

// Map controllers
app.MapControllers();

app.Run();
