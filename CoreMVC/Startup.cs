using CoreMVC.Domain;
using CoreMVC.Domain.Repositories.Abstract;
using CoreMVC.Domain.Repositories.EntityFramework;
using CoreMVC.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy; 
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreMVC
{
	public class Startup
	{
		public IConfiguration Configuration { get; }
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			Configuration.Bind("Project", new Config());

			//подключаем нужный функционал приложения в качестве сервисов
			services.AddTransient<ITextFieldsRepository, EFTextFieldsRepository>();
			services.AddTransient<IServiceItemsRepository, EFServiceItemsRepository>();
			services.AddTransient<DataManager>();

			//подключаем контекст к БД
			services.AddDbContext<AppDbContext>(x => x.UseSqlServer(Config.ConnectionString));

			//настраиваем identity систему
			services.AddIdentity<IdentityUser, IdentityRole>(opts =>
			{
				opts.User.RequireUniqueEmail = true;
				opts.Password.RequiredLength = 6;
				opts.Password.RequireNonAlphanumeric = false;
				opts.Password.RequireLowercase = false;
				opts.Password.RequireUppercase = false;
				opts.Password.RequireDigit = false;
			}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

			//настраиваем authentication cookie
			services.ConfigureApplicationCookie(options =>
			{
				options.Cookie.Name = "CoreMVCAuth";
				options.Cookie.HttpOnly = true;
				options.LoginPath = "/account/login";
				options.AccessDeniedPath = "/account/accessdenied";
				options.SlidingExpiration = true;
			});

			//настраиваем политику авторизации для Admin area
			services.AddAuthorization(x =>
			{
				x.AddPolicy("AdminArea", policy => { policy.RequireRole("admin"); });
			});

			//добавляем сервисы для контроллеров и представлений (MVC)
			services.AddControllersWithViews(x =>
			{
				x.Conventions.Add(new AdminAreaAuthorization("Admin", "AdminArea"));
			})
				//выставляем совместимость с asp.net core 3.0
				.SetCompatibilityVersion(CompatibilityVersion.Version_3_0).AddSessionStateTempDataProvider();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{

			app.UseHttpsRedirection();
			app.UseStaticFiles();


			//в процессе разработки нам важно видеть какие именно ошибки
			if (env.IsDevelopment())
				app.UseDeveloperExceptionPage();

			//подключаем поддержку статичных файлов в приложении (css, js и т.д.)
			app.UseStaticFiles();

			//подключаем систему маршрутизации
			app.UseRouting();

			//подключаем аутентификацию и авторизацию
			app.UseCookiePolicy();
			app.UseAuthentication();
			app.UseAuthorization();

			//регистриуруем нужные нам маршруты (ендпоинты)
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute("admin", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
				endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
			});

		}
	}
}
