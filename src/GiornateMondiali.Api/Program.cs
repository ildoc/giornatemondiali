using GiornateMondiali.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IGmService,GmService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.MapGet("/giornatemondiali", (IGmService gmService) =>
{
    var today = DateTime.Today;
    return gmService.GetSpecialDays(today);
})
.WithName("GiornateMondiali")
.WithOpenApi();

app.Run();
