using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApp.Data.DbContexts;
using WebApp.Data.Entities.Classes;
using WebApp.Data.Repositories.Classes;
using WebApp.Data.Repositories.Interfaces;
using WebApp.Infrastructure.AutoMapper;
using WebApp.Infrastructure.Classes;
using WebApp.Infrastructure.Interfaces;
using WebApp.Shared.Token;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DatabaseContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("WebApp")));
builder.Services.Configure<AuthOptions>(builder.Configuration.GetSection("AuthOptions"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;

                        var authOptions = new AuthOptions();
                        builder.Configuration.GetSection("AuthOptions").Bind(authOptions);
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = authOptions.Issuer,

                            ValidateAudience = true,
                            ValidAudience = authOptions.Audience,
                            ValidateLifetime = true,

                            IssuerSigningKey = authOptions.GetSymmetricSecurityKey(),
                            ValidateIssuerSigningKey = true,
                        };
                    });
builder.Services.AddAutoMapper(typeof(AppMappingProfile));

builder.Services.AddTransient<IGenericRepository<User>, GenericRepository<User>>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.MapControllers();

app.Run();
