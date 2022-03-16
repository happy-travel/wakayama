using HappyTravel.ErrorHandling.Extensions;
using HappyTravel.StdOutLogger.Extensions;
using HappyTravel.Wakayama.Api.Infrastructure.Extensions;
using HappyTravel.Wakayama.Common.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureApp("wakayama");
builder.ConfigureLogging();
builder.ConfigureSentry();
builder.ConfigureServiceProvider();
builder.ConfigureServices();

var app = builder.Build();

app.UseProblemDetailsErrorHandler(app.Environment, app.Logger);
app.UseHttpContextLogging(options => options.IgnoredPaths = new HashSet<string> {"/health"});
            
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HappyTravel.Wakayama.Api v1"));
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHealthChecks("/health").AllowAnonymous();
    endpoints.MapControllers();
});

app.Run();