using GoFish.Library.Model;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GoFish.Library.Services
{
    public class UserService
    {
        private readonly string _apiKey;

        public UserService(string apiKey)
        {
            _apiKey = apiKey;
        }

        public List<User> GetUsers()
        {
            var url = $"{Constants.BaseUrl}{"users"}";

            var responseText = Internet.Execute(url, _apiKey);

            return JsonConvert.DeserializeObject<List<User>>(responseText);
        }
    }
}
