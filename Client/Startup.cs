namespace Client;
public class Startup
{

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();

        IdentityModelEventSource.ShowPII = true;

        services.AddAuthentication(options =>
            {
                options.DefaultScheme = "cookie";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("cookie")
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = "https://localhost:5000";
                options.ClientId = "oidcClient";
                options.ClientSecret = "SuperSecretPassword";

                options.ResponseType = "code";
                options.UsePkce = true;
                options.ResponseMode = "query";

                // options.CallbackPath = "/signin-oidc"; // default redirect URI
                
                // options.Scope.Add("oidc"); // default scope
                // options.Scope.Add("profile"); // default scope
                options.Scope.Add("api1.read");
                options.SaveTokens = true;
            });
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseDeveloperExceptionPage();
        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
    }
}

