using Eventos.Web.Data;
using Eventos.Web.Models;
using Eventos.Web.Repositorios;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<EventosContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EventosDb")));
builder.Services.AddScoped<EventoRepositorio>();
builder.Services.AddScoped<AsistenteRepositorio>();
builder.Services.AddScoped<InscripcionRepositorio>();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var context = scope.ServiceProvider.GetRequiredService<EventosContext>();
    await context.Database.MigrateAsync();
    if (!await context.Categorias.AnyAsync())
    {
        var categorias = new[]
        {
            new Categoria { Nombre = "Tecnología" },
            new Categoria { Nombre = "Negocios" },
            new Categoria { Nombre = "Cultura" }
        };
        context.Categorias.AddRange(categorias);
        await context.SaveChangesAsync();
        context.Eventos.AddRange(
            new Evento { Titulo = "Innovación Digital 2026", Fecha = DateTime.Today.AddDays(10).AddHours(18), Lugar = "Auditorio Central", Aforo = 120, Precio = 45, CategoriaId = categorias[0].Id, Modalidad = "Presencial" },
            new Evento { Titulo = "Datos para Decidir", Fecha = DateTime.Today.AddDays(16).AddHours(19), Lugar = "Sala Norte", Aforo = 80, Precio = 35, CategoriaId = categorias[0].Id, Modalidad = "Virtual" },
            new Evento { Titulo = "Liderazgo con Propósito", Fecha = DateTime.Today.AddDays(22).AddHours(17), Lugar = "Centro de Convenciones", Aforo = 150, Precio = 60, CategoriaId = categorias[1].Id, Modalidad = "Presencial" },
            new Evento { Titulo = "Encuentro de Lectores", Fecha = DateTime.Today.AddDays(28).AddHours(16), Lugar = "Biblioteca Municipal", Aforo = 70, Precio = 0, CategoriaId = categorias[2].Id, Modalidad = "Presencial" });
        context.Asistentes.AddRange(
            new Asistente { Nombres = "Ana Torres", Dni = "70123456", Correo = "ana.torres@correo.com", Telefono = "987654321" },
            new Asistente { Nombres = "Luis Quispe", Dni = "45879632", Correo = "luis.quispe@correo.com", Telefono = "986123456" },
            new Asistente { Nombres = "María Flores", Dni = "71234890", Correo = "maria.flores@correo.com" },
            new Asistente { Nombres = "Jorge Salazar", Dni = "08765432", Correo = "jorge.salazar@correo.com", Telefono = "999888777" },
            new Asistente { Nombres = "Rosa Huamán", Dni = "72345619", Correo = "rosa.huaman@correo.com" });
        await context.SaveChangesAsync();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
