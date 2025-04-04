using Microsoft.AspNetCore.HttpOverrides;
using UserManagement.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddDataBase()
    .AddCorsPolicy()
    .ConfigureJWT()
	.AddApplicationServices()
    .AddSwaggerConfig();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();
app.UseForwardedHeaders(new ForwardedHeadersOptions {
	ForwardedHeaders = ForwardedHeaders.All
});
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("index.html");
app.Run();
