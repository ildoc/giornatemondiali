using GiornateMondiali.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

//app.UseHttpsRedirection();


app.MapGet("/giornatemondiali", () =>
{
    var today = DateTime.Today;
    var specialDays = GmCore.GetSpecialDays(today.Year).Where(x => x.Date.Date == DateTime.Today).ToList();
    return specialDays;
})
.WithName("GiornateMondiali")
.WithOpenApi();

app.Run();
