using System;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using FoodOrderApp.Model;

namespace FoodOrderApp.Services
{
    public class UserService
    {
        FirebaseClient client;

        public UserService()
        {
            client = new FirebaseClient("https://foodorderingapp-2d3aa.firebaseio.com/");
        }

        public async Task<bool> IsUserExists(string uname)
        {
            var user = (await client.Child("Users")
                .OnceAsync<User>()).Where(u => u.Object.Username == uname).FirstOrDefault();

            return (user != null);
        }

        public async Task<bool> RegisterUser(string uname, string password)
        {
            if(await IsUserExists(uname) == false)
            {
                await client.Child("Users")
                    .PostAsync(new User() {
                        Username = uname,
                        Password = password
                    });

                return true;
            }
            else
            {
                return false;
            }
        }

        public async  Task<bool> LoginUser(string uname, string password)
        {
            var user = (await client.Child("Users")
                .OnceAsync<User>())
                .Where(u => u.Object.Username == uname)
                .Where(u => u.Object.Password == password)
                .FirstOrDefault();

            return (user != null);

        }
    }
}
