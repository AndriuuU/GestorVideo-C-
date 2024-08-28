using VideoManager.Components;
using VideoMatrix.Data; // Importa la clase DatabaseInitializer
using VideoMatrix.Services;

var builder = WebApplication.CreateBuilder(args);

// Agregar la cadena de conexión a los servicios
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registro de DatabaseInitializer
builder.Services.AddSingleton(new DatabaseInitializer(connectionString));

// Registro de DeviceService
builder.Services.AddScoped<DeviceService>(provider =>
{
    return new DeviceService(connectionString); // Usa la misma cadena de conexión
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Ejecutar la inicialización de la base de datos al inicio de la aplicación
using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
    await initializer.InitializeAsync();  // Inicializar la base de datos y datos de prueba
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
