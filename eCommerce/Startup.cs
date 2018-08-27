using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using eCommerce.Data;
using eCommerce.Services;
using eCommerce.Repositories;

namespace eCommerce
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("eCommerce")));
            services.AddDbContext<eCommerceContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("eCommerce")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IUploadRepository, UploadRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IGeneralServices, GeneralServices>();
            services.AddTransient<ICartRepository, CartRepository>();
            services.AddTransient<IPanelRepository, PanelRepository>();

            services.AddMvc()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeFolder("/Account/Manage");
                    options.Conventions.AuthorizePage("/Account/Logout");
                    options.Conventions.AddPageRoute("/Profile/Index", "Profile/{userId}");
                    options.Conventions.AddPageRoute("/Profile", "Profile/{userId}");
                    options.Conventions.AddPageRoute("/Cart/Index", "Cart/{userId?}");
                    options.Conventions.AddPageRoute("/Cart/View", "Cart/View/{id}");
                    options.Conventions.AddPageRoute("/Profile/Edit", "Profile/Edit/{userId}");
                });

            // Register no-op EmailSender used by account confirmation and password reset during development
            // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=532713
            services.AddSingleton<IEmailSender, EmailSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            //RoleInitializer.Initialize(roleManager).Wait();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            //await RoleInitializer.Initialize(roleManager);
        }
    }
}
