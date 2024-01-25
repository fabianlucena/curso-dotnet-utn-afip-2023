using Data;
using Logic.User;
using Logic.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Login.Middleware;
using System.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Login.Policies;
using Microsoft.AspNetCore.Authorization;
using Entities.User;
using MemBD;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

/* Clase 9: El siguiente fragmento (invocaci�n) es una modificaci�n al m�todo 
 * builder.Services.AddSwaggerGen() para agregar el Sagger el bot�n de 
 * Authorize.
 */
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AFIP Login practice",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

/* Clase 9: El siguiente fragmento (invocaci�n) de datos configura el 
 * comportamiento de la autentificaci�n basada en JWT.
 * Los datos para configurar el compotamiento se toman desde el appsettings,
 * por lo tanto es necesario agregar al appsettings la siguiente secci�n
 * "Jwt": {
 *   "Key": "Contrase�a secreta para generar la clave sim�trica",
 *   "Issuer": "Emisor: URL del que provee el token, por ejemplo: http://localhost:5063",
 *   "Audience": "Usuario: URL del que provee el token, por ejemplo: http://localhost:5063",
 *   "Subject":  "Tema del token"
 * }
 * 
 * Recordatorio: para usar esta caracter�stica hay que usar la dependencia: Microsoft.AspNetCore.Authentication.JwtBearer
 */
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Clase 9: Se indica que hay que verificar la firma a partir de la clave configurada.
            ValidateIssuerSigningKey = true,
            // Clase 9: Se crea la clave de cifrado sim�trico a partir de la contrase�a
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "")),

            // Clase 9: Se indica que hay que verificar el Issuer
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            // Clase 9: Se indica que hay que verificar la Audience
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],

            // Clase 9: Se indica que hay que verificar el tiempo de validez
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
        };
    });


builder.Services.AddDbContext<DataContext>(
        options => options.UseOracle("name=ConnectionStrings:ServiceContext"));

/* Clase 13: La configuraci�n se carga en una clase para acceder en forma estructurada
 * Debido a que la configuraci�n se puede cambiar desde el controlador ConfigurationController
 * y esa alteraci�n debe afectar a toda la aplicaci�n, entonces esta dependencia es Singleton,
 * es decir es la misma instancia en toda la aplicaci�n. A diferencia de Scoped que crea una
 * instancia por cada conexi�n (request).
 * Una tercer forma de definir las dependencias es Transient (AddTransient) para dependencias
 * transitorias, cada vez que se necesitan se crea una instancia a�n dentro de la misma conexi�n.
 * 
 * Referencias:
 * https://learn.microsoft.com/es-mx/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-7.0
 * https://learn.microsoft.com/es-mx/dotnet/core/extensions/dependency-injection#service-registration-methods
 */
Login.Configuration configuration = builder.Configuration.GetSection("Configuration").Get<Login.Configuration>() ?? new Login.Configuration();
builder.Services.AddSingleton<Login.Configuration>(b => configuration);

builder.Services.AddScoped<IUserEntity, UserDB>();
//builder.Services.AddScoped<IUserEntity, UserMDB>();

builder.Services.AddScoped<IUserLogic, UserLogic>();
builder.Services.AddScoped<ISessionLogic, SessionLogic>();

// Clase 12: Se ha segregado la interface ILogin de ISession por lo tanto hay que indicar donde se encuentra esa funcionalidad.
builder.Services.AddScoped<ILoginLogic, SessionLogic>();



/* Clase 14: Authorizaci�n basada en pol�ticas, primero se inyecta como dependencia el manipulador de la pol�tica, 
 * este manipulador est�a relacionado con la clase RoleRequirement que se usa para definir las pol�ticas.
 */
builder.Services.AddScoped<IAuthorizationHandler, RoleHandler>();

// Clase 14: Authorizaci�n basada en pol�ticas, se define la pol�tica que requiere el rol admin. Pueden definirse otras para otros roles.
builder.Services.AddAuthorization(options => 
    options.AddPolicy("AdminRole", policy => policy.Requirements.Add(new RoleRequirement("admin"))));

builder.Services.AddAuthorization(options =>
    options.AddPolicy("OperatorRole", policy => policy.Requirements.Add(new RoleRequirement("operator"))));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

/* Clase 11: Los middleware se cargan usando metodos que geenralmente comienzan con Use...
 * As� mismo pueden cargarse usando �l m�todo UseMiddleware o usando una extensi�n ad hoc
 */
//app.UseMiddleware<LoggingHandlerMiddleware>();
app.UseLoggingHandlerMiddleware();

/* Clase 9: Los siguientes dos comando configuran el sistema para usar la 
 * authentificaci�n y la autorizaci�n.
 * Es necesario que se escriban antes del comando app.MapControllers();
 */
app.UseAuthentication();
app.UseAuthorization();

/* Clase 11: Los middleware se cargan usando metodos que geenralmente comienzan con Use...
 * As� mismo pueden cargarse usando �l m�todo UseMiddleware o usando una extensi�n ad hoc
 */
//app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseErrorHandlerMiddleware();

/* Clase 11: El Map es un tipo de middleware que permite redefinir el compoprtamiento para 
 * rutas o partes de rutas determinadas
 */
/*app.Map("/api/login", app =>
{
    app.Run(async context =>
    {
        await context.Response.WriteAsJsonAsync(new { message = "log in in maintenance" });

    });
});*/

app.MapControllers();

/* Clase 11: El Run puede ser redefinico, pero en este caso se pierde la funcionalidad autom�tica
 * de .NET Core de inyecci�n autom�tica de dependencias y carga autom�tica de controladores y toda
 * esa tarea deber�a hacerse a mano.
 */
/*app.Run(async context =>
{
    await context.Response.WriteAsJsonAsync(new { message = "Hola Mundo!" });

});*/

app.Run();
