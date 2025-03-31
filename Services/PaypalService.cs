using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace MassageManagementSystem.Services
{
    public class PayPalService : IPaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public PayPalService(IConfiguration configuration)
        {
            _configuration = configuration;
            // Create HttpClient instance (in production, use IHttpClientFactory)
            _httpClient = new HttpClient();
        }

        public async Task<string> ProcessPayment(decimal amount)
        {
            Console.WriteLine("Received payment amount: " + amount); // For debug
            if (amount <= 0)
            {
                throw new Exception("Invalid amount: Amount must be greater than zero.");
            }

            // Get your PayPal API credentials from configuration
            var clientId = _configuration["PayPal:ClientId"];
            var secret = _configuration["PayPal:Secret"];

            // 1. Obtain an access token from PayPal
            var token = await GetAccessToken(clientId, secret);

            // Set the Authorization header with the retrieved token.
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            // 2. Create a payment request
            var paymentRequest = new
            {
                intent = "sale",
                payer = new { payment_method = "paypal" },
                transactions = new[]
                {
                    new {
                        amount = new {
                            total = amount.ToString("F2"),
                            currency = "USD"
                        },
                        description = "Massage booking payment"
                    }
                },
                redirect_urls = new
                {
                    return_url = "https://localhost:7210/payment/success",
                    cancel_url = "https://localhost:7210/payment/cancel"
                }
            };

            var jsonRequest = JsonConvert.SerializeObject(paymentRequest);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api.sandbox.paypal.com/v1/payments/payment", content);
            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                throw new Exception($"Payment processing failed: {response.ReasonPhrase}. Details: {errorBody}");
            }

            var json = await response.Content.ReadAsStringAsync();
            dynamic paymentResult = JsonConvert.DeserializeObject(json);

            // Find the PayPal approval URL
            string approvalUrl = null;
            foreach (var link in paymentResult.links)
            {
                if (link.rel == "approval_url")
                {
                    approvalUrl = link.href;
                    break;
                }
            }

            if (approvalUrl == null)
            {
                throw new Exception("PayPal approval URL not found in response.");
            }

            return approvalUrl;

        }

        private async Task<string> GetAccessToken(string clientId, string secret)
        {
            // PayPal OAuth2 token URL (Sandbox)
            var tokenUrl = "https://api.sandbox.paypal.com/v1/oauth2/token";

            // Prepare basic auth header using your credentials
            var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{clientId}:{secret}"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

            // Request body for token retrieval
            var content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await _httpClient.PostAsync(tokenUrl, content);
            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                throw new Exception($"Could not retrieve PayPal access token. Status: {response.StatusCode}, Response: {errorBody}");
            }

            var json = await response.Content.ReadAsStringAsync();
            dynamic tokenResponse = JsonConvert.DeserializeObject(json);
            return tokenResponse.access_token;
        }
    }
}
