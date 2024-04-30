namespace Claims.Extensions;

public static class MiddlewareRegistrationsExtensions
{
    public static IApplicationBuilder UseClaimsSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger(u =>
        {
            u.RouteTemplate = "swagger/{documentName}/swagger.json";
        });
    
        app.UseSwaggerUI(c =>
        {
            c.EnableFilter();
            c.RoutePrefix = "swagger";
            c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "Claims API v1");
        });

        return app;
    }
    
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(err =>  err.UseCustomErrors());

        return app;
    }
}