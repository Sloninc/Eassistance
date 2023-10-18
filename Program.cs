using Microsoft.EntityFrameworkCore;
using Eassistance.Infrastructure;
using Eassistance.Services;
using Eassistance.BuisnessLogic.FSM;
using Eassistance.Services.Abstract;

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
            builder.Services.AddDbContextFactory<DataContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DataBase")));
            builder.Services.AddTransient<IOperationService, OperationService>();
            builder.Services.AddTransient<IUnitService, UnitService>();
            builder.Services.AddTransient<IStepService, StepService>();
            builder.Services.AddTransient<IEquipmentService, EquipmentService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<BaseState, StartState>();
            builder.Services.AddScoped<FSMContext>();

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