using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Rotativa.AspNetCore;
using StoreData;
using StoreData.Models;
using StoreIdentity;
using StoreServices;
using Stripe;
using WebApplication6.Extentions;

namespace WebApplication6
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
            services.AddMvc();
            services.AddSingleton(Configuration);
            services.AddScoped<IProductService, StoreProductService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IExternalUserLoginsStoreService, ExternalUserLoginsStoreService>();
            services.AddDbContext<StoreContext>(options => options.UseSqlServer(Configuration.GetConnectionString("StoreConnection")));
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddIdentity<Users, UserRole>(
                options =>
                {
                    options.SignIn.RequireConfirmedEmail = true;
                    options.Lockout.AllowedForNewUsers = true;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
                    options.Lockout.MaxFailedAccessAttempts = 2;
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = false;
                }


                )
                    .AddDefaultTokenProviders();
            /*services.Replace( ServiceDescriptor.Scoped(
             typeof(IUserClaimsPrincipalFactory<Users>), typeof(MyClaimsPrincipalFactory)));*/
            /*services.AddScoped<IUserClaimsPrincipalFactory<Users>, MyClaimsPrincipalFactory>();*/
            services.AddTransient<IUserStore<Users>, UserStore>();
            services.AddTransient<IRoleStore<UserRole>, RoleStore>();
            /*services.AddTransient <IUserLoginStore<ExternalUserLogins>, ExternalUserLoginsStore>();*/
            services.AddTransient<IEmailSender, EmailSender>();
            /*services.AddSingleton<IAuthorizationHandler, IsUserHandler>();*/
            services.ConfigureApplicationCookie(options =>
            {
               /* options.Events = new Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents
                {
                    OnRedirectToLogin = ctx =>
                    {
                        var requestPath = ctx.Request.Path;
                        if (requestPath.Value == "/Order/ShopingCart")
                        {
                            ctx.Response.Redirect("/Account/Login");
                        }
                        else if (requestPath.Value == "/Home/Contact")
                        {
                            ctx.Response.Redirect("/Home/AdminLogin");
                        }

                        return Task.CompletedTask;
                    }
                };*/
                options.Cookie.HttpOnly = true;
                options.LoginPath = new PathString ("/Account/Login");
                options.LogoutPath = "/Logout";
                options.Cookie.Expiration = TimeSpan.FromDays(7);
                /*options.AccessDeniedPath = "/www.google.com";*/
                
            });
            services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = "141391246433-mkqbpqsnje06o7vj5ip3qahf048mn9cv.apps.googleusercontent.com";
                googleOptions.ClientSecret = "qE0ea_cf1LmcecJSdZoe_MfH";
                googleOptions.CallbackPath = new PathString ("/Home/Index");
                
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("IsUser", policy =>
                          policy.RequireClaim("Role", "User"));

                 options.AddPolicy("IsAdmin", policy =>
                     policy.RequireClaim("Role","Admin"));

                options.AddPolicy("IsManager", policy =>
                     policy.RequireClaim("Role", "Manager"));
            });
            StripeConfiguration.SetApiKey(Configuration.GetSection("Stripe")["SecretKey"]);
            services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));
         
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, StoreContext db, SignInManager<Users> s,ILoggerFactory loggerFactory)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                /*app.UseExceptionHandlingMiddleware();
                app.UseStatusCodePages();*/
            }

             else
             {
                app.UseExceptionHandlingMiddleware();
                app.UseStatusCodePages();
                /*app.UseExceptionHandler("/Home/Error");*/
            }

            app.UseAuthentication();

            RotativaConfiguration.Setup(env, "..\\Rotativa\\");

            app.UseStaticFiles();

                app.UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");
                });
            }
        }
    }

