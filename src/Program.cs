using api.Data;
using api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// TODO test this ---- vvvvvv
builder.Services.AddScoped<AdminService>();
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(
        builder.Configuration.GetConnectionString("LegendsConnection")
    )); // -------------------------------------- ^^^^^^^^^^^^^^^^^^ this is the database connection, check appsettings.json
// TODO ---------- ^^^^^^ ----------------------- 

var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.MapControllers().WithParameterValidation();
app.Run();