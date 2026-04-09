
using Microsoft.EntityFrameworkCore;
using UrlShortener.Api.Data;
using UrlShortener.Api.Repositories;
using UrlShortener.Api.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddDbContext<UrlDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UrlDbConnection")));

builder.Services.AddScoped<IUrlRepository, UrlRepository>(); // Đăng ký Repository
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Port của React
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();




// Tự động Migration khi startup (giống Tutorial hướng dẫn)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<UrlDbContext>();
    context.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowReact");
app.UseAuthorization();
app.MapControllers();
app.Run();