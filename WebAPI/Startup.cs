using Business.Abstract;
using Business.Concrete;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Core.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Core.Utilities.Security.Encryption;
using Core.Utilities.IOC;
using Core.DependencyResolvers;

namespace WebAPI
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
            //services.AddSingleton<IProductService, ProductManager>();
            //services.AddSingleton<IProductDal, EfProductDal>();
            // IProductService istenen yere ProductManager i gönder diyoruz.ProductManager i newliyor bizim yerimize.

            //aþaðýdaki satýrý core a taþýdýk. core/dependencyresolvers/coremodule
            //her yapýlan istekle oluþan ve post un geri dönüþüne kadar kullanýlýr.
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin",
                    builder => builder.WithOrigins("http://localhost:4200"));
            }
               );


            var tokenOptions = Configuration.GetSection("TokenOptions").Get<TokenOptions>();// tokenoption dan getsection ile al.
                                                                                            // <TokenOptions> a ata.
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = tokenOptions.Issuer,
                        ValidAudience = tokenOptions.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
                    }; 
                });
            services.AddDependencyResolvers
                (
                new ICoreModule[] { new CoreModule() }
                );
            //sadece coremodule deðil ileride oluþturabileceðim diðer module larý da buraya ekleyebilmek istiyorum.
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder.WithOrigins("http://localhost:4200").AllowAnyOrigin());
            //yukarýdaki satýr any get, post bir istek gelirse 4200 deki porttan gelirse izin ver demek.

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
