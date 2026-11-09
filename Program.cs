using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json.Serialization;
using CalidadAireApi.Configuration;
using CalidadAireApi.Data;
using CalidadAireApi.Middleware;
using CalidadAireApi.Models;
using CalidadAireApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var cadenaConexion = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Falta la cadena de conexión 'Default'.");

builder.Services.AddDbContext<AppDbContext>(opciones =>
    opciones.UseMySql(cadenaConexion, ServerVersion.AutoDetect(cadenaConexion)));

builder.Services.AddOptions<JwtSettings>()

    .Bind(builder.Configuration.GetSection(JwtSettings.Seccion))
    .ValidateDataAnnotations()
    .ValidateOnStart();

var jwt = builder.Configuration.GetSection(JwtSettings.Seccion).Get<JwtSettings>()
    ?? throw new InvalidOperationException("Falta la sección 'Jwt'.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opciones =>
    {
        opciones.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = jwt.Issuer,
            ValidAudience = jwt.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddOptions<ClimaSettings>()
    .Bind(builder.Configuration.GetSection(ClimaSettings.Seccion))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddHttpClient<IClimaService, ClimaService>((proveedor, cliente) =>
{
    var configuracion = proveedor.GetRequiredService<IOptions<ClimaSettings>>().Value;
    cliente.BaseAddress = new Uri(configuracion.BaseUrl);
    cliente.Timeout = TimeSpan.FromSeconds(configuracion.TimeoutSegundos);
});
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();
builder.Services.AddScoped<ISensorService, SensorService>();
builder.Services.AddScoped<ILecturaService, LecturaService>();
builder.Services.AddScoped<IAlertaConsultaService, AlertaConsultaService>();
builder.Services.AddSingleton<IAlertaService, AlertaService>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.ConfigureHttpJsonOptions(opciones =>
    opciones.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddControllers()
    .AddJsonOptions(opciones =>
        opciones.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddOpenApi(opciones => opciones.AddDocumentTransformer<BearerSecuritySchemeTransformer>());

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(opciones =>
    {
        opciones.SwaggerEndpoint("/openapi/v1.json", "Calidad Aire API v1");
        opciones.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();