using Microsoft.OpenApi.Models;
using YachtCareAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// --- CORS ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "YachtCare Booking API", Version = "v1" });
});

// DI
builder.Services.AddSingleton<IBookingService, BookingService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// --- ВАЖНО: CORS должен быть ДО MapControllers() ---
app.UseCors("AllowAll");

app.MapControllers();
app.Run();
