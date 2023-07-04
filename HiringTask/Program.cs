using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using HiringTask.Models.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using HiringTask.Helpers;
using Microsoft.AspNetCore.Identity;
using HiringTask.Models;

namespace HiringTask
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<TaskContext>(options =>
                           options.UseSqlServer(builder.Configuration.GetConnectionString("TaskCon1")));
            builder.Services.Configure<Jwt>(builder.Configuration.GetSection("JWT"));
            builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<TaskContext>();
           
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}