
using SwimmingSchool.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;
builder.Services.AddMvc(option => option.EnableEndpointRouting = false);
builder.Services.AddDbContext<AppDbContext>(options => {
    var connectionString = configuration.GetConnectionString("AppDbContext");
    options.UseSqlServer(connectionString);
});


builder.Services.AddRazorPages();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(1800); //30 minute session
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var db = builder.Services.AddDbContext<AppDbContext>();
var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();


if (configuration.GetValue<bool>("Enviroment:DisableDeveloperExceptions"))
{
    app.UseExceptionHandler("/error.html");
    app.UseHsts();
}


app.MapGet("/invalid", () =>
{
    throw new Exception("ERROR!");

});

app.UseMvc(routes => {
    routes.MapRoute("Default",
        "{controller=Home}/{action=Index}/{id?}"
    );
});

app.UseFileServer();
app.Run();