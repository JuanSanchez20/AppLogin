using AppLogin.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

//Llama a la coneccion de la base de datos
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DBContext>(
    options => { options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSQL"));}
    );
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Access/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });

//Inicia la aplicacion
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Access}/{action=Login}/{id?}");

app.Run();
