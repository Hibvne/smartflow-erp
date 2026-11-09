
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SmartFlow.Application.Interfaces;
using SmartFlow.Application.Services;
using SmartFlow.Infrastructure.Data;
using SmartFlow.Infrastructure.Repositories;
using System.Text;
using SmartFlow.API.Middlewares; 


namespace SmartFlow.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Entrez uniquement votre token JWT (sans le mot 'Bearer')"
                });

                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
            }); 
            
            builder.Services.AddDbContext<SmartFlowDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IUtilisateurRepository, UtilisateurRepository>();
            builder.Services.AddScoped<PasswordService>();
            builder.Services.AddScoped<TokenService>();
            builder.Services.AddScoped<AuthService>();

            builder.Services.AddScoped<IClientRepository, ClientRepository>();
            builder.Services.AddScoped<ClientService>();

            builder.Services.AddScoped<ICategorieRepository, CategorieRepository>();
            builder.Services.AddScoped<CategorieService>();

            builder.Services.AddScoped<IProduitRepository, ProduitRepository>();
            builder.Services.AddScoped<ProduitService>();

            builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();


            builder.Services.AddScoped<IVenteRepository, VenteRepository>();
            builder.Services.AddScoped<IFactureRepository, FactureRepository>();
            builder.Services.AddScoped<VenteService>();
            builder.Services.AddScoped<FactureService>();

            builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
            builder.Services.AddScoped<DashboardService>();

            builder.Services.AddScoped<IPaiementRepository, PaiementRepository>();


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
             {
            options.TokenValidationParameters = new TokenValidationParameters
            {
             ValidateIssuer = true,
             ValidateAudience = true,
             ValidateLifetime = true,
             ValidateIssuerSigningKey = true,
             ValidIssuer = builder.Configuration["Jwt:Issuer"],
             ValidAudience = builder.Configuration["Jwt:Audience"],
             IssuerSigningKey = new SymmetricSecurityKey(
             Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
            });

            builder.Services.AddAuthorization();


            // Autorise le frontend React (localhost:5173) à communiquer avec cette API
            // Sans ça, le navigateur bloque les requêtes par sécurité (protection cross-origin)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp", policy =>
                {
                    policy.WithOrigins("http://localhost:5173")
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });


            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowReactApp");

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
