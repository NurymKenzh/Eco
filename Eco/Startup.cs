using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Eco.Data;
using Eco.Models;
using Eco.Services;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Http.Features;

namespace Eco
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 5;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddErrorDescriber<CustomIdentityErrorDescriber>();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.Configure<FormOptions>(x => x.ValueCountLimit = int.MaxValue);

            services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });
            services.AddMvc(options =>
                {
                    var F = services.BuildServiceProvider().GetService<IStringLocalizerFactory>();
                    var L = F.Create("ModelBindingMessages", "Eco");
                    options.ModelBindingMessageProvider.SetValueIsInvalidAccessor(
                        (x) => L["The value '{0}' is invalid."]);
                    options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(
                        (x) => L["The field {0} must be a number."]);
                    options.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor(
                        (x) => L["A value for the '{0}' property was not provided.", x]);
                    options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor(
                        (x, y) => L["The value '{0}' is not valid for {1}.", x, y]);
                    options.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(
                        () => L["A value is required."]);
                    options.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor(
                        (x) => L["The supplied value is invalid for {0}.", x]);
                    options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
                        (x) => L["Null value is invalid.", x]);
                })
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix,
                    opts => { opts.ResourcesPath = "Resources"; })
                .AddDataAnnotationsLocalization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            var supportedCultures = new[]
            {
                new CultureInfo("kk"),
                new CultureInfo("ru"),
            };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("ru"),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            CreateRoles(serviceProvider).Wait();
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] roleNames = { "Administrator", "Moderator", "Analyst" };
            IdentityResult roleResult;
            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            ApplicationUser user = userManager.Users.FirstOrDefault(u => u.Email == "n.a.k@bk.ru");
            await userManager.AddToRoleAsync(user, "Administrator");
        }
    }
}
