using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SL.Domain.Common.Constants;
using SL.Domain.Common.Ultilities;
using SL.Domain.IServices;
using SL.Domain.IServices.ISysServices;
using SL.Infrastructures.EntityFramework;
using SL.Infrastructures.EntityFramework.Entities.SysEntities;
using SL.Services.Services;
using SL.Services.Services.SysServices;
using SL.WebApi.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Config appsettings
builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

// Add services to the container.
builder.Services.AddControllers();

//Register DBContext
builder.Services.AddDbContext<SnapLifeContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("SnapLifeDatabase")));
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(setup =>
{
    setup.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SnapLife - APIs",
        Version = "v1"
    });
    setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Sample for login: 'Bearer + {your_token}'"
    });
    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
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
    setup.MapType<DateOnly>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "date"
    });
});

//Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: Strings.AllowOrigins.PolicyWithOrigins, policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

//JWT LOGIN
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "4335d179-a729-489c-82ec-b5ccd05a10f5"))
    };
});

//MVC
builder.Services.AddControllersWithViews();

// IDENTITY & DI
builder.Services.AddIdentityCore<AspNetUsers>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.User.RequireUniqueEmail = true;
})
.AddRoles<AspNetRoles>()
.AddEntityFrameworkStores<SnapLifeContext>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<Globals>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRelationshipService, RelationshipService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IReactionService, ReactionService>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();

// Config for memory cache
builder.Services.AddMemoryCache();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

string urlSwagger = builder.Configuration["Url:WebApi"] + "/swagger/index.html";
app.MapGet("/", async context =>
{
    await context.Response.WriteAsync("<h1>Welcome to SnapLife WebAPI!!!</h1>" +
        "<h4>You can access to the <a href='" + urlSwagger + "'>" + urlSwagger + "</a> and see all the APIs</h4>");
});

app.UseHttpsRedirection();

app.UseCors(Strings.AllowOrigins.PolicyWithOrigins);

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseSession();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
