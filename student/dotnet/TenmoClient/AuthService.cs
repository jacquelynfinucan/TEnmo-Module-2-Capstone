using RestSharp;
using RestSharp.Authenticators;
using System;
using TenmoClient.Models;

namespace TenmoClient
{
    public class AuthService
    {
        private readonly static string API_BASE_URL = "https://localhost:44315/";
        private readonly IRestClient client = new RestClient();
        private ApiUser user = new ApiUser();

        public IRestClient getClient => client;


        //login endpoints
        public bool Register(LoginUser registerUser)
        {
            RestRequest request = new RestRequest(API_BASE_URL + "login/register");
            request.AddJsonBody(registerUser);
            IRestResponse<ApiUser> response = client.Post<ApiUser>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Program.dyn.Add("An error occurred communicating with the server.");
                return false;
            }
            else if (!response.IsSuccessful)
            {
                if (!string.IsNullOrWhiteSpace(response.Data.Message))
                {
                    Program.dyn.Add("An error message was received: " + response.Data.Message);
                }
                else
                {
                    Program.dyn.Add("An error response was received from the server. The status code is " + (int)response.StatusCode);
                }
                return false;
            }
            else
            {
                return true;
            }
        }

        public ApiUser Login(LoginUser loginUser)
        {
            RestRequest request = new RestRequest(API_BASE_URL + "login");
            request.AddJsonBody(loginUser);
            IRestResponse<ApiUser> response = client.Post<ApiUser>(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                Program.dyn.Add("An error occurred communicating with the server.");
                return null;
            }
            else if (!response.IsSuccessful)
            {
                if (!string.IsNullOrWhiteSpace(response.Data.Message))
                {
                    Program.dyn.Add("An error message was received: " + response.Data.Message);
                }
                else
                {
                    Program.dyn.Add("An error response was received from the server. The status code is " + (int)response.StatusCode);
                }
                return null;
            }
            else
            {
                user.Token = response.Data.Token;
                client.Authenticator = new JwtAuthenticator(response.Data.Token);
                return response.Data;
            }
        }
    }
}
