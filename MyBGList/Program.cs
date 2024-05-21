
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBGList.Models;

namespace MyBGList
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddCors(options =>
                options.AddDefaultPolicy(cfg =>
                {
                    cfg.AllowAnyHeader();
                    cfg.AllowAnyMethod();
                    cfg.AllowAnyOrigin();
                }));

            var app = builder.Build();

            //Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }
            //if (app.Configuration.GetValue<bool>("UseDeveloperExceptionPage"))
            //    app.UseDeveloperExceptionPage();
            //else
            //    app.UseExceptionHandler("/error");

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors();
            app.MapControllers();


            app.MapGet("/error",
            [EnableCors("AnyOrigin")]
            [ResponseCache(NoStore = true)] () => 
               Results.Problem());


            app.Map("/error/test", () => { throw new Exception("test"); });
            app.Run();
        }
    }
}
