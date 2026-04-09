using Microsoft.EntityFrameworkCore;
using UrlShortenerBackend.Data;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. Cấu hình DbContext
builder.Services.AddDbContext<UrlDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. MỚI: Đăng ký dịch vụ Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
// 3. MỚI: Kích hoạt Swagger (Bỏ if IsDevelopment để luôn hiện cho dễ test)
app.UseSwagger();
app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "URL Shortener API V1");
    c.RoutePrefix = "swagger"; // Truy cập qua đường dẫn /swagger
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

// 4. MỚI: Cấu hình Route cho Redirect (Để domain.com/abc nhảy vào RedirectController)
app.MapControllerRoute(
    name: "shortener",
    pattern: "{code}",
    defaults: new { controller = "Redirect", action = "Index" });

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();