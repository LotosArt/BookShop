using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopEFConsoleApp.Data
{
    public class ApplicationContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
    {
        private static IConfigurationRoot config;
        static ApplicationContextFactory()
        {
            // получаем конфигурацию из файла appsettings.json
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            config = builder.Build();
        }
        public ApplicationContext CreateDbContext(string[]? args = null)
        {
            // получаем строку подключения из файла appsettings.json
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            return new ApplicationContext(optionsBuilder.Options);
        }
    }

}
