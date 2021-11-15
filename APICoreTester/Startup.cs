using System;
using APICore;
using APICore.Security;
using APICoreTester.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static APICore.Configurations;

namespace APICoreTester
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private IConfiguration Configuration { get; }

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

            // Login authentication
            app.Use( (context, next) =>
            {
                try
                {
                    if (context.Request.Path.ToString().Contains("login")) return next();                
                    var token = context.Request.HttpContext.Request.Headers["token"];
                
                    if (string.IsNullOrEmpty(token))
                    {
                        return context.Response.WriteAsJsonAsync(new Response<dynamic> { Status = false, Message = GetMessage(MessageTypes.Login) });                    
                    }

                    if (!JwtToken.Verify<User>(token).Item1)
                    {
                        context.Response.WriteAsJsonAsync(new Response<dynamic> { Status = false, Message = GetMessage(MessageTypes.Login) });   
                    }
                    
                    return next();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return context.Response.WriteAsJsonAsync(new Response<dynamic> { Status = false, Message = GetMessage(MessageTypes.Login) });
                }                
            });

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}