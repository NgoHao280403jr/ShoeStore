using Microsoft.EntityFrameworkCore;
using QLBanGiay.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Cấu hình DbContext
builder.Services.AddDbContext<QlShopBanGiayContext>(options =>
	options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Cấu hình Session
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian hết hạn của Session
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

// Thêm Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();  // Kích hoạt Swagger trong môi trường Development
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "QL Shop Ban Giay API v1");
		c.RoutePrefix = "swagger"; 
	});
}
else
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
