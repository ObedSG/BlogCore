using BlogCore.AccesoDatos.Data;
using BlogCore.AccesoDatos.Data.Repository;
using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("ConexionSQL") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false) // por ahora lo cambiamos a false
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI();


builder.Services.AddControllersWithViews();

//Agregar contenedor de trabajo al contenedor IoC de inyeccion de dependencias
builder.Services.AddScoped<IContenedorTrabajo, ContenedorTrabajo>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=cliente}/{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// Documentación de enrutamiento:
// - Sistema de áreas: Organiza controladores en módulos
// - Estructura URL: /Área/Controlador/Acción/ID
// - Parámetros opcionales: {id?} (el '?' indica opcional)
// - Integración con: Razor Pages para vistas dinámicas

app.Run();