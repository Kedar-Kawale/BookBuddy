
using BookBuddy.API.Data;
using BookBuddy.API.Repositories.Implementation;
using BookBuddy.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookBuddy.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // registering DbContext into Program.cs 
           string connectionString = builder.Configuration.GetConnectionString("BookBuddyConnectionString");
            builder.Services.AddDbContext<BookBuddyDbContext>(options => options.UseSqlServer(connectionString));

            // registering/injecting the services into program.cs 
            builder.Services.AddScoped<IBookRepository, BookRepository>();





            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();



            var app = builder.Build();




            // Configure the HTTP request pipeline. | Middleware Pipeline
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
