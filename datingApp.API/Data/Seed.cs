using System.Collections.Generic;
using System.Linq;
using datingApp.API.Models;
using Newtonsoft.Json;

namespace datingApp.API.Data
{
    public class Seed
    {
        public static void SeedUser(DataContext context)
        {
            if (!context.users.Any())
            {
                var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
                var usres = JsonConvert.DeserializeObject<List<User>>(userData);
                foreach (var user in usres)
                {
                    byte[] passwordhash, passwordSalt;
                    CreatePasswordHash("password", out passwordhash, out passwordSalt);
                    user.PasswordHash=passwordhash;
                    user.PasswordSalt=passwordSalt;
                    user.UserName=user.UserName.ToLower();
                    context.users.Add(user);                    
                }
                context.SaveChanges();
            }
        }
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            };
        }
    }
}