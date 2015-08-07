using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Runtime;
using Microsoft.AspNet.Hosting;
using GHUtils.Services;
using Microsoft.AspNet.Authentication.OAuth;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using Microsoft.AspNet.Http.Authentication;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.Framework.Configuration;

namespace GHUtils
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(IApplicationEnvironment appEnv)
        {
            var configBuilder = new ConfigurationBuilder(appEnv.ApplicationBasePath);
            configBuilder.AddUserSecrets();
            configBuilder.AddEnvironmentVariables();
            _configuration = configBuilder.Build();
        }
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication();
            services.Configure<ExternalAuthenticationOptions>(options =>
            {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });
            services.AddScoped(typeof(ScriptCollector));
            services.AddScoped(typeof(GitHubService));
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCookieAuthentication(options =>
            {
                options.AutomaticAuthentication = true;
                options.LoginPath = new PathString("/");
            });

            app.UseOAuthAuthentication("GitHub-AccessToken", options =>
            {
                options.ClientId = _configuration.Get("GitHub:ClientId");
                options.ClientSecret = _configuration.Get("GitHub:Secret");
                options.CallbackPath = new PathString("/signin-github");
                options.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
                options.TokenEndpoint = "https://github.com/login/oauth/access_token";
                options.SaveTokensAsClaims = true;
                options.Scope.Add("read:org");
                options.Scope.Add("repo");
            });

            app.Use(async (context, next) =>
            {
                if (!context.User.Identities.Any(identity => identity.IsAuthenticated))
                {
                    await context.Authentication.ChallengeAsync("GitHub-AccessToken", new AuthenticationProperties() { RedirectUri = context.Request.Path.ToString() });
                    return;
                }
                await next();
            });

            app.UseMvcWithDefaultRoute();
        }
    }
}
