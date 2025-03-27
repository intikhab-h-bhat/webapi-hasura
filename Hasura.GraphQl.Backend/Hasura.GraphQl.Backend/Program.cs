using Hasura.GraphQl.Backend.services;// Ensure correct namespace

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Register HasuraService
builder.Services.AddHttpClient();
builder.Services.AddScoped<HasuraService>();
builder.Services.AddScoped<PostService>();


// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Replace with your React app URL
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

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
app.UseCors("AllowReactApp");  // Apply CORS Policy

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
