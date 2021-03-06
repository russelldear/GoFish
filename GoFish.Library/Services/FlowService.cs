﻿using GoFish.Library.Model;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace GoFish.Library.Services
{
    public class FlowService
    {
        private readonly string _apiKey;

        public FlowService(string apiKey)
        {
            _apiKey = apiKey;
        }

        public List<Flow> GetFlows()
        {
            var url = $"{Constants.BaseUrl}{"flows"}";

            var responseText = Internet.Execute(url, _apiKey);

            return JsonConvert.DeserializeObject<List<Flow>>(responseText);
        }
    }
}
