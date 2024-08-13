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
        ValidateAudience = true, // Token'�n hedef kitlesi (audience) do�rulanacak m�?
        ValidateIssuer = true, // Token'� olu�turan (issuer) do�rulanacak m�?
        ValidateLifetime = true, // Token'�n ge�erlilik s�resi do�rulanacak m�?
        ValidateIssuerSigningKey = true, // Token'�n imza anahtar� do�rulanacak m�?
        ValidIssuer = builder.Configuration["Token:Issuer"], // Ge�erli issuer (olu�turan) de�eri
        ValidAudience = builder.Configuration["Token:Audience"], // Ge�erli audience (hedef kitle) de�eri
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
        (builder.Configuration["Token:SecurityKey"])), // Token'� imzalamak i�in kullan�lan simetrik g�venlik anahtar�
        ClockSkew = TimeSpan.Zero, // Token'�n s�resi dolduktan sonra verilen zaman tolerans�
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
