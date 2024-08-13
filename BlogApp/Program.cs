using BlogApp.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateAudience = true, // Token'ýn hedef kitlesi (audience) doðrulanacak mý?
        ValidateIssuer = true, // Token'ý oluþturan (issuer) doðrulanacak mý?
        ValidateLifetime = true, // Token'ýn geçerlilik süresi doðrulanacak mý?
        ValidateIssuerSigningKey = true, // Token'ýn imza anahtarý doðrulanacak mý?
        ValidIssuer = builder.Configuration["Token:Issuer"], // Geçerli issuer (oluþturan) deðeri
        ValidAudience = builder.Configuration["Token:Audience"], // Geçerli audience (hedef kitle) deðeri
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
        (builder.Configuration["Token:SecurityKey"])), // Token'ý imzalamak için kullanýlan simetrik güvenlik anahtarý
        ClockSkew = TimeSpan.Zero, // Token'ýn süresi dolduktan sonra verilen zaman toleransý
    };

});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
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

app.Run();
