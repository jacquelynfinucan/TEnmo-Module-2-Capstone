using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;
using RestSharp.Authenticators;
using TenmoClient.Models;
using TenmoClient.Exceptions;

namespace TenmoClient
{
    public class ApiService
    {
        private readonly string API_URL = "https://localhost:44315/";
        private readonly IRestClient client = new RestClient();
        private ApiUser user = new ApiUser();

        public bool LoggedIn { get { return !string.IsNullOrWhiteSpace(user.Token); } }

        public ApiService(string api_url,IRestClient client)
        {
            API_URL = api_url;
            this.client = client;
        }

        public ApiService()
        {

        }

        public Account GetAccount()
        {
            Account userAccount = new Account();
            RestRequest request = new RestRequest(API_URL + "account");
            IRestResponse<Account> response = client.Get<Account>(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                ProcessErrorResponse(response);
            }
            else
            {
                return response.Data;
            }
            return null;
        }

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            RestRequest request = new RestRequest(API_URL + "account/users");
            IRestResponse<List<User>> response = client.Get<List<User>>(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                ProcessErrorResponse(response);
            }
            else
            {
                return response.Data;
            }
            return null;
        }

        public List<Transfer> GetPastTransfers()
        {
            List<Transfer> transfers = new List<Transfer>();
            RestRequest request = new RestRequest(API_URL + "account/transfers/?userId=" + UserService.GetUserId());
            IRestResponse<List<Transfer>> response = client.Get<List<Transfer>>(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                ProcessErrorResponse(response);
            }
            else
            {
                return response.Data;
            }
            return null;
        }

        public decimal? GetBalance()
        {
            RestRequest request = new RestRequest(API_URL + "account/" + UserService.GetUserId()); 
            IRestResponse<decimal> response = client.Get<decimal>(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                ProcessErrorResponse(response);
            }
            else
            {
                return response.Data;
            }
            return null;
        }

        public void TransferMoney(int userId, decimal xferAmount)
        {

            RestRequest request = new RestRequest(API_URL + "account/transfers/" + UserService.GetUserId() + "?receivingUserId=" + userId + "&amount=" + xferAmount);
            IRestResponse response = client.Post(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                ProcessErrorResponse(response);
            }
            else
            {
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("Transfer Successful!");
                Console.WriteLine("--------------------------------------------");
            }
        }

        public Transfer GetTransferDetailsById(int transferId)
        {
            Transfer transfer = new Transfer();
            RestRequest request = new RestRequest(API_URL + "account/transfers/" + transferId);
            IRestResponse<Transfer> response = client.Get<Transfer>(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                ProcessErrorResponse(response);
            }
            else
            {
                return response.Data;
            }
            return null;
        }

        public string GetUsername(int accountId)
        {
            RestRequest request = new RestRequest(API_URL + "users/" + accountId);
            IRestResponse<string> response = client.Get<string>(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                ProcessErrorResponse(response);
            }
            else
            {
                return response.Data;
            }
            return "";
        }
        private void ProcessErrorResponse(IRestResponse response)
        {
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new NoResponseException("Error occurred - unable to reach server.", response.ErrorException);
            }
            else if (!response.IsSuccessful)
            {

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedException("Authorization is required for this endpoint. Please Log In.");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    throw new ForbiddenException("You do not have permission to perform the requested action.");
                }
                else
                {
                    throw new NonSuccessException((int)response.StatusCode);
                }
            }
        }

    }
}
