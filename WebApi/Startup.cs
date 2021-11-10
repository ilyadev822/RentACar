using DAL.DataFiles;
using DAL.DataFiles.BranchesRepository;
using DAL.DataFiles.CarModelsRepository;
using DAL.DataFiles.CarsRepository;
using DAL.DataFiles.OrdersRepository;
using DAL.DataFiles.UsersRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebApi
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
           
            services.AddTransient<IUsersRepository, UsersMockContext>();
            services.AddTransient<IBranchRepository, BranchMockContext>();
            services.AddTransient<IOrdersRepository, OrdersMockContext>();
            services.AddTransient<ICarsRepository, CarsMockContext>();
            services.AddTransient<ICarModelsRepository, CarModelsMockContext>();




            services.AddDbContext<DataContext>(op => op.UseSqlServer(
                    Configuration.GetConnectionString("str1")));
            services.AddControllers().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                opt.JsonSerializerOptions.MaxDepth = 2147483646;


            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(opt => {
                   opt.RequireHttpsMetadata = false;
                   opt.SaveToken = true;
                   opt.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ValidIssuer = Configuration["jwt:Issuer"],
                       ValidAudience = Configuration["jwt:Audience"],
                       IssuerSigningKey = new SymmetricSecurityKey(
                          Encoding.UTF8.GetBytes(Configuration["jwt:SecretKey"])),
                       ClockSkew = TimeSpan.Zero
                   };
               });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"StaticFiles")),
            //    RequestPath = new PathString("/StaticFiles")
            //});
            app.UseRouting();
          
            app.UseAuthentication();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthorization();
           
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
