﻿using System;
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
        private readonly string API_URL = "";
        private readonly RestClient client = new RestClient();
        private ApiUser user = new ApiUser();

        public bool LoggedIn { get { return !string.IsNullOrWhiteSpace(user.Token); } }

        public ApiService(string api_url)
        {
            API_URL = api_url;
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
            RestRequest request = new RestRequest(API_URL + "users");
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

        public decimal? GetBalance()
        {
            Account userAccount = new Account();
            RestRequest request = new RestRequest(API_URL + "account/balance");
            IRestResponse<Account> response = client.Get<Account>(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                ProcessErrorResponse(response);
            }
            else
            {
                return response.Data.Balance;
            }
            return null;
        }

        public void TransferMoney(int userId, decimal xferAmount)
        {
            Transfer newTransfer = new Transfer(UserService.GetUserId(),userId,xferAmount);
            RestRequest request = new RestRequest(API_URL + "account/transfer");
            request.AddJsonBody(newTransfer);
            IRestResponse<Transfer> response = client.Post<Transfer>(request);

            if (response.ResponseStatus != ResponseStatus.Completed || !response.IsSuccessful)
            {
                ProcessErrorResponse(response);
            }
            //else
            //{
            //    return response.Data;
            //}
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
