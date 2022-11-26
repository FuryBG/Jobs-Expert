using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Data.Common;
using System.Text;
using WebApplication1.Hubs;
using WebApplication1.Models.AuthModels;
using WebApplication1.Models.DatabaseModels;
using WebApplication1.Services;
//using WebApplication1.Services;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<DbDataContext>(
                x => x.UseNpgsql(builder.Configuration.GetConnectionString("JobsExpertDb"))
                );
            builder.Services.AddControllersWithViews();
            builder.Services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "jobs-expert/dist";
            });
            builder.Services.AddScoped<AuthService, AuthService>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value,
                        ValidAudience = builder.Configuration.GetSection("Jwt:Audience").Value,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value)),
                    };
                });

            builder.Services.AddSignalR();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();


            app.Use(async (context, next) =>
            {
                var JWToken = context.Request.Cookies["Authorization"];
                if (!string.IsNullOrEmpty(JWToken))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + JWToken);
                }
                await next();
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chat");
            });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapWhen(config =>
            {
                if (config.Request.Path.StartsWithSegments("/app") && config.User.Identity.IsAuthenticated)
                {
                    if (!builder.Environment.IsDevelopment())
                    {
                        config.Request.Path = config.Request.Path.Value.Replace("/app", "");
                    }
                    return true;
                }
                return false;
            },
                appBuilder =>
                {
                    appBuilder.UseSpaStaticFiles();
                    appBuilder.UseSpa(spa =>
                    {
                        spa.Options.SourcePath = "jobs-expert";

                        if (builder.Environment.IsDevelopment())
                        {
                            spa.UseAngularCliServer(npmScript: "start");
                        }
                    });
                });


            app.Run();
        }
    }
}