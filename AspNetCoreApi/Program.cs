using TestApi.Controllers;
var builder = WebApplication.CreateBuilder(args);

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(
            name: "Origins",
            policy =>
            {
                policy.WithOrigins("http://localhost:4200", "https://localhost:4200");
            }
        );
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Origins");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
