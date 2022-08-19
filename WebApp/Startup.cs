using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Repositories;
using WebApp.Services;

namespace WebApp
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddCors();
            //Cross-Origin Requests

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                // Prevent Cross-Site Request Forgery (XSRF/CSRF)
            });

            services.AddRazorPages().AddRazorRuntimeCompilation();

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(_config.GetConnectionString("eCommerce")));
            services.AddDbContext<eCommerceContext>(options => options.UseSqlServer(_config.GetConnectionString("eCommerce")));
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IUploadRepository, UploadRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IGeneralServices, GeneralServices>();
            services.AddTransient<ICartRepository, CartRepository>();
            services.AddTransient<IPanelRepository, PanelRepository>();
            services.AddTransient<IEmailSender, EmailSender>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;
            });

            services.ConfigureApplicationCookie(options => { options.AccessDeniedPath = "/Home/AccessDenied"; });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Expiration = TimeSpan.FromDays(365);
                    options.ExpireTimeSpan = TimeSpan.FromHours(8);
                });

            services.AddIdentity<ApplicationUser, IdentityRole>(setupAction =>
                {
                    setupAction.Password.RequireDigit = true;
                    setupAction.Password.RequireNonAlphanumeric = false;
                    setupAction.Password.RequireLowercase = true;
                    setupAction.Password.RequireUppercase = true;
                    setupAction.Password.RequiredLength = 6;
                    setupAction.Password.RequiredUniqueChars = 0;
                    setupAction.SignIn.RequireConfirmedEmail = true;
                    setupAction.SignIn.RequireConfirmedPhoneNumber = false;
                    setupAction.User.RequireUniqueEmail = true;
                }).AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseRewriter(new RewriteOptions().AddRedirectToWww());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}