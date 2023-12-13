using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ElabSupport.Controllers
{
    public class ExotelController : Controller
    {
        private const string Username = "f60730bf842e84322beaa8e29343ba4261dcb3cff518e0af";
        private const string Password = "6dd8ad80b30219045b204def703bb68602aa9bac0cf9ce09";
        public ActionResult Index()
        {
            return View();
        }

        public async Task<string> GetExotelData()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Set up Basic Authentication
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{Username}:{Password}"));
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

                    // Make the request
                    HttpResponseMessage response = await client.GetAsync("https://ccm-api.exotel.com/v2/accounts/bluepearlhealthtech2/users?fields=devices,active_call");

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read and return the response content
                        string responseData = await response.Content.ReadAsStringAsync();
                        return responseData;
                    }
                    else
                    {
                        // Return an error status code along with the reason
                        return response.StatusCode.ToString()+ $"Error: {response.ReasonPhrase}";
                    }
                }
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error with the exception message
                return ex.Message;
            }
        }
        public async Task<string> UpdateDeviceAvailability(string userId, bool available)
        {
            try
            {
                string deviceId = "490038";
                string ExotelApiUrlBase = "https://ccm-api.exotel.in/v2/accounts/bluepearlhealthtech2/users/";
                string apiUrl = $"{ExotelApiUrlBase}{userId}/devices/{deviceId}";
                DeviceAvailabilityRequest request = new DeviceAvailabilityRequest
                {
                    available = false
                };
                using (HttpClient client = new HttpClient())
                {
                    // Set up Basic Authentication
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{Username}:{Password}"));
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

                    // Convert the request object to JSON
                    string jsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(request);
                    StringContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync(apiUrl, content);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                        return responseData;
                    }
                    else
                    {
                        // Return an error status code along with the reason
                        return response.StatusCode.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error with the exception message
                return ex.Message;
            }

        }
        public class DeviceAvailabilityRequest
        {
            public bool available { get; set; }
        }

        public async Task<string> GetUserData(string userId)
        {
            try
            {
                string ExotelApiUrlBase = "https://ccm-api.exotel.in/v2/accounts/bluepearlhealthtech2/users/";
                string apiUrl = $"{ExotelApiUrlBase}{userId}";

                using (HttpClient client = new HttpClient())
                {
                    // Set up Basic Authentication
                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{Username}:{Password}"));
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);

                    // Make the reques3t
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                        return responseData;
                    }
                    else
                    {
                        // Return an error status code along with the reason
                        return response.StatusCode.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error with the exception message
                return ex.Message;
            }
        }

    }
}