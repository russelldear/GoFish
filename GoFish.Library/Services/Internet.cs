using System;
using System.IO;
using System.Net;
using System.Text;

namespace GoFish.Library.Services
{
    public static class Internet
    {
        public static string Execute(string url, string apiToken)
        {
            try
            {
                var request = WebRequest.Create(url);

                request.ContentType = "application/json";

                var encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(apiToken + ":"));

                request.Headers.Add("Authorization", "Basic " + encoded);

                var response = (HttpWebResponse)request.GetResponse();

                var encoding = Encoding.ASCII;
                using (var reader = new StreamReader(response.GetResponseStream(), encoding))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message); // Keeping hold of ex while debugging
            }
        }
    }
}
