using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace FinalProject.Models.Facebook
{
    public static class FacebookSettings
    {
        public static string AccessToken = "EAAHuIBhqFLkBAEaTZCUWtpNaxx7niq7qXMPj7bVxZBaKk0RUawa6ZBr7qjA76NAeAGsClZB3yvHNAv2w3sp3ZBUnkNtfZBihnkRNItPCK7gHNllA6xxgrZAeWAu3ZAKvojR7C3OiMT7nIQYcsoasltH7AdTlVFIHIG9MZCPe126ZC5XKj45G1ivVZCSdkZBCwZAgTmlMZD";
    }

    public interface IFacebookService
    {
        Task<FacebookAccount> GetAccountAsync(string accessToken);
        Task PostOnWallAsync(string accessToken, string message);
    }

    public class FacebookService : IFacebookService
    {
        private readonly IFacebookClient _facebookClient;

        public FacebookService(IFacebookClient facebookClient)
        {
            _facebookClient = facebookClient;
        }

        public async Task<FacebookAccount> GetAccountAsync(string accessToken)
        {
            var result = await _facebookClient.GetAsync<dynamic>(
                accessToken, "me", "fields=id,name,email,first_name,last_name,age_range,birthday,gender,locale");

            if (result == null)
            {
                return new FacebookAccount();
            }

            var account = new FacebookAccount
            {
                Id = result.id,
                Email = result.email,
                Name = result.name,
                UserName = result.username,
                FirstName = result.first_name,
                LastName = result.last_name,
                Locale = result.locale
            };

            return account;
        }

        public async Task PostOnWallAsync(string accessToken, string message)
            => await _facebookClient.PostAsync(accessToken, "me/feed", new { message });
    }
}