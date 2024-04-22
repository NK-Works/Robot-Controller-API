/* This code is made by Anneshu Nag, Student ID- 2210994760*/
using robot_controller_api.Persistence;
using robot_controller_api.Models;
using robot_controller_api.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options => 
{ 
    options.SwaggerDoc("v1", new OpenApiInfo
    { 
        Title = "Robot Controller API", 
        Description = "New backend service that provides resources for the Moon robot simulator.", 
        Contact = new OpenApiContact 
        { 
            Name = "Anneshu Nag", 
            Email = "anneshu4760.be22@chitkara.edu.in", 
        }, 
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"; 
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename)); 

    // options.DescribeAllParametersInCamelCase();
    // options.MapType<DateTime>(() => new OpenApiSchema { Type = "string", Format = "date" });
});

// Registering data access interfaces and implementations
// builder.Services.AddScoped<IRobotCommandDataAccess, RobotCommandADO>(); 
// builder.Services.AddScoped<IMapDataAccess, MapADO>();

// builder.Services.AddScoped<IRobotCommandDataAccess, RobotCommandRepository>(); 
// builder.Services.AddScoped<IMapDataAccess, MapRepository>();


builder.Services.AddScoped<IRobotCommandDataAccess, RobotCommandEF>();
builder.Services.AddScoped<IMapDataAccess, MapEF>();
builder.Services.AddDbContext<RobotContext>();
builder.Services.AddScoped<IUserDataAccess, UserServiceEF>();

// Uncomment when using the Microsoft's default Password Hasher and not BCrypt 
// builder.Services.AddScoped<IPasswordHasher<UserModel>, PasswordHasher<UserModel>>();

// Add authentication services.
builder.Services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", default);

// Add authorization policies.
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireClaim(ClaimTypes.Role, "Admin"));
        
    options.AddPolicy("UserOnly", policy =>
        policy.RequireClaim(ClaimTypes.Role, "Admin", "User", "Member"));

    // Adding two new policies
    options.AddPolicy("DevsOnly", policy =>
        policy.RequireClaim(ClaimTypes.Role, "Admin", "Tester"));   // Policy for testers/developers to test new endpoints/functionalities of the Moon Robot 
    
    options.AddPolicy("MemberOnly", policy =>
        policy.RequireClaim(ClaimTypes.Role, "Admin", "Member"));   // Policy for paid members for premium features
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    // Serve static files (wwwroot)
    app.UseStaticFiles();
    
    app.UseSwagger();
    app.UseSwaggerUI(setup => 
    {
        setup.InjectStylesheet("/styles/theme-custom-anneshu.css");   
        setup.SwaggerEndpoint("/swagger/v1/swagger.json", "Robot Controller API json-doc");
        /* Learnt how to generate yaml documentation from: https://stackoverflow.com/questions/45100923/generate-yaml-swagger-using-swashbuckle*/
        setup.SwaggerEndpoint("/swagger/v1/swagger.yaml", "Robot Controller API yaml-doc");
    });
}

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();
