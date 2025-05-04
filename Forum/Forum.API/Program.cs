using Forum.API;
using Forum.Infrastructure;
using Forum.Infrastructure.DataInitilalizer;
using Forum.Infrastructure.MongoDB;
using Forum.Infrastructure.Services.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJWTTokenAuthentication(builder.Configuration);
builder.Services.ConfigureControllers();
builder.Services.ConfigureCORS();
builder.Services.ConfigureValidation();
builder.Services.AddInfrastructure();
builder.Services.AddServices();
builder.Logging.AddLogger(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    //var scope = app.Services.CreateScope();
    //var context = scope.ServiceProvider.GetRequiredService<MongoDbContext>();
    //var logger = scope.ServiceProvider.GetRequiredService<ILogger<PasswordHasher>>();
    //await DbInitializer.Initialize(context, logger);
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureCustomExceptionMiddleware();

app.UseRouting();

app.UseCors("allowMyOrigin");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
