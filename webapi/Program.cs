using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using webapi.SharedServices.Authentication;
using webapi.SharedServices.Db;
using webapi.StudentFeatures;
using webapi.Swashbuckle;
using static webapi.SharedServices.Authentication.AuthenticationService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        connectionString: builder.Configuration.GetConnectionString("DefaultLocal")!, 
        sqlServerOptionsAction: (cnfg) => cnfg.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName)
        );
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "StudentsAPI",
        Version = "v1"
    });

    c.AddUIButtonBearerTokenAuthToSwagger();
});

builder.Services.AddAuthentication("OAuth").AddJwtBearer("OAuth", (conf) =>
{
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppAuthConstants.Secret));

    conf.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = AppAuthConstants.Issuer,
        ValidAudience = AppAuthConstants.Audience,
        IssuerSigningKey = key
    };
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();    
app.UseAuthorization();

app.MapControllers();

var scopeToInitAppDb = app.Services.CreateScope();
var services = scopeToInitAppDb.ServiceProvider;

DbInitializer.Initialize(services.GetRequiredService<AppDbContext>());
StudentsSeedProvider.Seed(services.GetRequiredService<AppDbContext>());

scopeToInitAppDb.Dispose();

app.Run();
