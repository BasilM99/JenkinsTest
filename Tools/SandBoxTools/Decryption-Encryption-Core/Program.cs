using System;

namespace Decryption_Encryption_Core
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            services.AddDbContext<MyKeysContext>(options =>
             options.UseMySQL(
                JsonConfigurationManager.ConnectionStrings["MyKeysConnection"], b => b.MigrationsAssembly("Noqoush.AdFalcon.Services")));

            // using Microsoft.AspNetCore.DataProtection;
            services.AddDataProtection()
                .SetApplicationName("AdFalcon").PersistKeysToDbContext<MyKeysContext>();


        }
    }
}
