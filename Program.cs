using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Eassistance.Infrastructure;

namespace Eassistance
{
    public class Program
    {
        public static void Main()
        {
            var builder = WebApplication.CreateBuilder();

     
            string connection = builder.Configuration.GetConnectionString("DataBase");
            // Add services to the container.

            builder.Services.AddControllers().AddNewtonsoftJson();
            builder.Services.AddDbContext<DataContext>(opt => opt.UseSqlServer(connection), ServiceLifetime.Singleton);
            //builder.Services.AddServices();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}