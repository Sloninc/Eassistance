using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Eassistance.Infrastructure;
using Telegram.Bot;
using Eassistance.Services;

namespace Eassistance
{
    public class Program
    {
        public static void Main()
        {
            var builder = WebApplication.CreateBuilder();

            // Add services to the container.
            builder.Services.AddControllers().AddNewtonsoftJson();
            builder.Services.AddSingleton<TelegramBot>();
            builder.Services.AddDbContext<DataContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DataBase")));
            //builder.Services.AddSingleton<BaseCommand, StartCommand>();
            //builder.Services.AddAuthentication("Bearer").AddJwtBearer();
            //builder.Services.AddAuthorization();
            //builder.Services.AddServices();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.Services.GetRequiredService<TelegramBot>().GetBot().Wait();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}