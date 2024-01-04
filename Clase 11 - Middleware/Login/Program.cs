using Data;
using Logic.User;
using Logic.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Login.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

/* Clase 9: El siguiente fragmento (invocación) es una modificación al método 
 * builder.Services.AddSwaggerGen() para agregar el Sagger el botón de 
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

/* Clase 9: El siguiente fragmento (invocación) de datos configura el 
 * comportamiento de la autentificación basada en JWT.
 * Los datos para configurar el compotamiento se toman desde el appsettings,
 * por lo tanto es necesario agregar al appsettings la siguiente sección
 * "Jwt": {
 *   "Key": "Contraseña secreta para generar la clave simétrica",
 *   "Issuer": "Emisor: URL del que provee el token, por ejemplo: http://localhost:5063",
 *   "Audience": "Usuario: URL del que provee el token, por ejemplo: http://localhost:5063",
 *   "Subject":  "Tema del token"
 * }
 * 
 * Recordatorio: para usar esta característica hay que usar la dependencia: Microsoft.AspNetCore.Authentication.JwtBearer
 */
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Clase 9: Se indica que hay que verificar la firma a partir de la clave configurada.
            ValidateIssuerSigningKey = true,
            // Clase 9: Se crea la clave de cifrado simétrico a partir de la contraseña
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

builder.Services.AddScoped<IUserLogic, UserLogic>();
builder.Services.AddScoped<ISessionLogic, SessionLogic>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

/* Clase 11: Los middleware se cargan usando metodos que geenralmente comienzan con Use...
 * Así mismo pueden cargarse usando él método UseMiddleware o usando una extensión ad hoc
 */
//app.UseMiddleware<LoggingHandlerMiddleware>();
app.UseLoggingHandlerMiddleware();

/* Clase 9: Los siguientes dos comando configuran el sistema para usar la 
 * authentificación y la autorización.
 * Es necesario que se escriban antes del comando app.MapControllers();
 */
app.UseAuthentication();
app.UseAuthorization();

/* Clase 11: Los middleware se cargan usando metodos que geenralmente comienzan con Use...
 * Así mismo pueden cargarse usando él método UseMiddleware o usando una extensión ad hoc
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

/* Clase 11: El Run puede ser redefinico, pero en este caso se pierde la funcionalidad automática
 * de .NET Core de inyección automática de dependencias y carga automática de controladores y toda
 * esa tarea debería hacerse a mano.
 */
/*app.Run(async context =>
{
    await context.Response.WriteAsJsonAsync(new { message = "Hola Mundo!" });

});*/

app.Run();
