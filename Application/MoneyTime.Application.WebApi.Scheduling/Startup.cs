using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoneyTime.Application.WebApi.Settings;
using Swashbuckle.AspNetCore.Swagger;

namespace MoneyTime.Application.WebApi.Scheduling
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<SchedulingSettings>(Configuration);
            var settings = Configuration.Get<SchedulingSettings>();
            services.AddSingleton(settings);
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddOpenIdConnect(options =>
                {
                    options.Authority = settings.IdentityUrl;
                    options.RequireHttpsMetadata = false;
                    options.ClientId = settings.IdentityClientId;
                    options.ClientSecret = settings.IdentityClientSecret;
                    options.ResponseType = settings.IdentityResponseType;
                });

            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Info
                {
                    Title = "Scheduling API",
                    Version = "v1",
                    Description = "Scheduling API service",
                });

                //options.AddSecurityDefinition("oauth2", new OAuth2Scheme
                //{
                //    Type = "oauth2",
                //    Flow = "application",
                //    AuthorizationUrl = $"{settings.IdentityUrlExternal}/connect/authorize",
                //    TokenUrl = $"{settings.IdentityUrlExternal}/connect/token",
                //    Scopes = new Dictionary<string, string>
                //    {
                //        {settings.IdentityAudience, "Profile API"}
                //    }
                //});

                //options.OperationFilter<AuthorizeCheckOperationFilter>();
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Scheduling.WebApi V1");
                    //c.OAuthClientId("root");
                    //c.OAuthClientSecret("secret");
                    c.OAuthAppName("Profile Swagger UI");
                });

            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
        }
    }
}
