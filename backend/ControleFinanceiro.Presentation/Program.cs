using ControleFinanceiro.Application.Interfaces.Services;
using ControleFinanceiro.Application.Services;
using ControleFinanceiro.Application.Models;
using ControleFinanceiro.Domain.Interfaces.Repositories;
using ControleFinanceiro.Infrastructure.Data;
using ControleFinanceiro.Infrastructure.Data.Repositories;
using ControleFinanceiro.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ControleFinanceiro
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Configurar CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });
            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "Controle Financeiro API", 
                    Version = "v1",
                    Description = "API para controle financeiro pessoal com Domain-Driven Design",
                    Contact = new OpenApiContact
                    {
                        Name = "Controle Financeiro",
                        Email = "contato@controlefinanceiro.com"
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License"
                    }
                });
                
                // Configurar comentários XML
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }

                // Configurar esquemas para enums
                c.UseInlineDefinitionsForEnums();

                // Configurar autenticação JWT no Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header usando o esquema Bearer. Exemplo: \"Authorization: Bearer {token}\"",
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
                        Array.Empty<string>()
                    }
                });
            });

            // Configurar Entity Framework
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Configurar Email Settings
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

            // Configurar AutoMapper
            builder.Services.AddAutoMapper(
                typeof(Program),
                typeof(ControleFinanceiro.Application.Mappings.CategoriaProfile),
                typeof(ControleFinanceiro.Application.Mappings.LancamentoProfile),
                typeof(ControleFinanceiro.Application.Mappings.UsuarioProfile),
                typeof(ControleFinanceiro.Application.Mappings.ContaProfile));

            // Configurar Autenticação JWT
            var jwtSettings = builder.Configuration.GetSection("JWT");
            var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey não configurada");
            var key = Encoding.ASCII.GetBytes(secretKey);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidateAudience = true,
                    ValidAudience = jwtSettings["Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            builder.Services.AddAuthorization();

            // Registrar repositórios
            builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            builder.Services.AddScoped<ILancamentoRepository, LancamentoRepository>();
            builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            builder.Services.AddScoped<IContaRepository, ContaRepository>();
            builder.Services.AddScoped<IUsuarioContaRepository, UsuarioContaRepository>();
            builder.Services.AddScoped<ISolicitacaoAcessoContaRepository, SolicitacaoAcessoContaRepository>();

            // Registrar serviços
            builder.Services.AddScoped<ICategoriaService, CategoriaService>();
            builder.Services.AddScoped<ILancamentoService, LancamentoService>();
            builder.Services.AddScoped<IUsuarioService, UsuarioService>();
            builder.Services.AddScoped<IContaService, ContaService>();
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IEmailTemplateService, EmailTemplateService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Controle Financeiro API V1");
                c.RoutePrefix = string.Empty; // Para acessar o Swagger na raiz
                c.DocumentTitle = "Controle Financeiro API";
                c.DefaultModelsExpandDepth(-1); // Não expandir modelos por padrão
                c.DisplayRequestDuration();
            });

            app.UseHttpsRedirection();

            // Habilitar CORS em modo Development
            if (app.Environment.IsDevelopment())
            {
                app.UseCors("AllowAll");
            }

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
