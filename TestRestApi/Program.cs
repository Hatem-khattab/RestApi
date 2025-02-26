using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TestRestApi.DATA;
using TestRestApi.DATA.Models;
using TestRestApi.extintion;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<APPDbContext>(option => 
option.UseSqlServer(builder.Configuration.GetConnectionString("MyCon"))
);

//builder.Services.AddIdentity<UserData, IdentityRole>()
//    .AddEntityFrameworkStores<APPDbContext>()
//    .AddDefaultTokenProviders();

builder.Services.AddIdentity<UserData, IdentityRole>(options =>
{
    options.Password.RequireDigit = false; // No need for numbers
    options.Password.RequiredLength = 4;  // Minimum length 4
    options.Password.RequireNonAlphanumeric = false; // No need for symbols
    options.Password.RequireUppercase = false; // No need for uppercase letters
    options.Password.RequireLowercase = false; // No need for lowercase letters
})
.AddEntityFrameworkStores<APPDbContext>()
.AddDefaultTokenProviders();


builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.addCustomeJWTAuth(builder.Configuration);



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
