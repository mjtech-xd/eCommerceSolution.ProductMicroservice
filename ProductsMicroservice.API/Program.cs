using BusinessLogicLayer;
using DataAccessLayer;
using DataAccessLayer.Context;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using ProductsMicroservice.API.APIEndpoints;
using ProductsMicroservice.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

//Add DAL and BLL services
builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddDataBusinessLogicLayer();
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();

//Swagger 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => 
    {
        builder.WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
var app = builder.Build();

// Apply database migrations on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}
//Swagger
app.UseSwagger();
app.UseSwaggerUI();
app.UseExceptionHandlingMiddleware();
app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapProductAPIEndpoints();
app.Run();