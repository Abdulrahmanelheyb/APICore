using APICore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace APICoreTester
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            
            app.Use( (context, next) =>
            {
                if (context.Request.Path.ToString() == "api/user/login") return next();
                
                var token = context.Request.HttpContext.Request.Headers["token"];
                return string.IsNullOrEmpty(token) ? context.Response.WriteAsync(Message.GetMessage(MessageTypes.Login)) : next();

            });

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}