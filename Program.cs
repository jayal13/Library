using System.Text;
using Library.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddCors((options) =>
{
    options.AddPolicy("DevCors", (corsBuilder) =>
    {
        //Allowing React
        corsBuilder.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
    });
});
builder.Services.AddScoped<ILibraryRepository, LibraryRepository>();

string? tokenkeyString = builder.Configuration.GetSection("AppSettings:TokenKey").Value;

SymmetricSecurityKey tokenKey = new(
    Encoding.UTF8.GetBytes(tokenkeyString ?? ""));

TokenValidationParameters tokenValidationParameters = new()
{
    IssuerSigningKey = tokenKey,
    ValidateIssuer = false,
    ValidateIssuerSigningKey = false,
    ValidateAudience = false
};

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = tokenValidationParameters;
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors("DevCors");
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseAuthorization();

// Configure the HTTP request pipeline.
app.MapControllers();

app.Run();