using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Project.Helpers;
using System;

namespace Project.Api
{
   public class Startup
   {
      public Startup(IConfiguration configuration)
      {
         Configuration = configuration;
      }
      public IConfiguration Configuration { get; }
      public void ConfigureServices(IServiceCollection services)
      {
         services.AddMemoryCache();
         services.AddControllers();
         services.AddCors(options =>
         {
            options.AddPolicy("AllowOrigin", builder => builder.WithOrigins(MyStatic.ApiUrlTest));
         });
         services.AddSwaggerGen(setup =>
         {
            setup.SwaggerDoc("v1", new OpenApiInfo
            {
               Version = "v1",
               Title = "Rent a Car API",
               Description = "It is the api service of a rent a car system.",
               License = new OpenApiLicense
               {
                  Name = "by Ali KARAKOÇ",
               }
            });
            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
               Scheme = "bearer",
               BearerFormat = "JWT",
               Name = "JWT Authentication",
               In = ParameterLocation.Header,
               Type = SecuritySchemeType.Http,
               Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

               Reference = new OpenApiReference
               {
                  Id = JwtBearerDefaults.AuthenticationScheme,
                  Type = ReferenceType.SecurityScheme
               }
            };
            setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
            setup.AddSecurityRequirement(new OpenApiSecurityRequirement
             {
                 { jwtSecurityScheme, Array.Empty<string>() }
             });
         });
      }
      public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
      {
         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
         }
         app.UseSwagger();
         app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rent a Car BackEnd Services"));
         app.UseCors(builder => builder.WithOrigins(MyStatic.ApiUrlTest).AllowAnyHeader().AllowAnyMethod());
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
