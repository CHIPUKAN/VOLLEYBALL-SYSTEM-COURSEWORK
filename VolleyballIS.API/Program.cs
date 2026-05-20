using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using VolleyballIS.Application.Repositories;
using VolleyballIS.Application.Services;
using VolleyballIS.Infrastructure.Data;
using VolleyballIS.Infrastructure.Repositories;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// строка подключения
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Строка подключения 'DefaultConnection' не задана в конфигурации");

// CORS
string[] allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? ["http://localhost:5173"];

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactFrontend", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Entity Framework Core + Npgsql
builder.Services.AddDbContext<VolleyballDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

// JWT аутентификация
string jwtSecret = builder.Configuration["Jwt:Secret"]
    ?? throw new InvalidOperationException("JWT Secret не задан в конфигурации");

byte[] keyBytes = Encoding.UTF8.GetBytes(jwtSecret);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization(options =>
{
    // по умолчанию все эндпоинты требуют аутентификации
    options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

// репозитории
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<IVenueRepository, VenueRepository>();
builder.Services.AddScoped<ICoachRepository, CoachRepository>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<ISeasonRepository, SeasonRepository>();
builder.Services.AddScoped<IOrganizerRepository, OrganizerRepository>();
builder.Services.AddScoped<IRefereeRepository, RefereeRepository>();
builder.Services.AddScoped<ITournamentRepository, TournamentRepository>();
builder.Services.AddScoped<IMatchRepository, MatchRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
builder.Services.AddScoped<IRefereeAssignmentRepository, RefereeAssignmentRepository>();
builder.Services.AddScoped<IProtocolRepository, ProtocolRepository>();
builder.Services.AddScoped<ISetRepository, SetRepository>();
builder.Services.AddScoped<IPlayerStatsRepository, PlayerStatsRepository>();
builder.Services.AddScoped<IStartingLineupRepository, StartingLineupRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<ISanctionRepository, SanctionRepository>();
builder.Services.AddScoped<IAwardRepository, AwardRepository>();
builder.Services.AddScoped<IMatchCaptainRepository, MatchCaptainRepository>();
builder.Services.AddScoped<IDelegationRepository, DelegationRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProtocolHistoryRepository, ProtocolHistoryRepository>();

// сервисы
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IVenueService, VenueService>();
builder.Services.AddScoped<ICoachService, CoachService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<ISeasonService, SeasonService>();
builder.Services.AddScoped<IOrganizerService, OrganizerService>();
builder.Services.AddScoped<IRefereeService, RefereeService>();
builder.Services.AddScoped<ITournamentService, TournamentService>();
builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();
builder.Services.AddScoped<IRefereeAssignmentService, RefereeAssignmentService>();
builder.Services.AddScoped<IProtocolService, ProtocolService>();
builder.Services.AddScoped<ISetService, SetService>();
builder.Services.AddScoped<IPlayerStatsService, PlayerStatsService>();
builder.Services.AddScoped<IStartingLineupService, StartingLineupService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<ISanctionService, SanctionService>();
builder.Services.AddScoped<IAwardService, AwardService>();
builder.Services.AddScoped<IMatchCaptainService, MatchCaptainService>();
builder.Services.AddScoped<IDelegationService, DelegationService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProtocolHistoryService, ProtocolHistoryService>();
builder.Services.AddScoped<IStandingService, StandingService>();

// контроллеры
builder.Services.AddControllers();

// Swagger/OpenAPI с поддержкой JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Volleyball IS API",
        Version = "v1",
        Description = "Информационная система ведения учёта волейбольных турниров"
    });

    OpenApiSecurityScheme securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Введите Bearer {token}",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    };
    options.AddSecurityDefinition("Bearer", securityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

WebApplication app = builder.Build();

// синхронизация истории миграций и заполнение справочников при старте
using (IServiceScope scope = app.Services.CreateScope())
{
    VolleyballDbContext dbContext = scope.ServiceProvider.GetRequiredService<VolleyballDbContext>();
    try
    {
        // если таблицы созданы вручную через SQL, помечаем миграцию как выполненную
        await dbContext.Database.ExecuteSqlRawAsync(@"
            INSERT INTO ""__EFMigrationsHistory"" (""MigrationId"", ""ProductVersion"")
            SELECT '20260517194633_InitialCreate', '9.0.4'
            WHERE NOT EXISTS (
                SELECT 1 FROM ""__EFMigrationsHistory""
                WHERE ""MigrationId"" = '20260517194633_InitialCreate'
            )
            AND EXISTS (
                SELECT 1 FROM information_schema.tables
                WHERE table_name = 's1_amplua'
            )
        ");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[MigrationSync] Предупреждение: {ex.Message}");
    }

    try
    {
        await DataSeeder.SeedAsync(dbContext);
    }
    catch (Exception ex)
    {
        // не роняем приложение при проблемах с подключением к БД на старте
        Console.WriteLine($"[DataSeeder] Предупреждение: не удалось выполнить сид данных — {ex.Message}");
    }
}

// HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Volleyball IS API v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseCors("ReactFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
