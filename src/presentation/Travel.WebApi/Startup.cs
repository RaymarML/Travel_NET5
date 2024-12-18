using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Travel.Application;
using Travel.WebApi.Filters;
using Travel.Data;
using Travel.Shared;

namespace Travel.WebApi
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
      services.AddCoreApplication();
      services.AddInfrastructureData();
      services.AddInfrastructureShared(Configuration);
      
      services.AddHttpContextAccessor();
      services.AddControllers();
      services.AddControllersWithViews(options =>
          options.Filters.Add(new ApiExceptionFilter()));
      services.Configure<ApiBehaviorOptions>(options =>
          options.SuppressModelStateInvalidFilter = true);

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Travel.WebApi", Version = "v1" });
      });
    }


    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Travel.WebApi v1"));
      }
      app.UseHttpsRedirection();
      app.UseRouting();
      app.UseAuthorization();
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}