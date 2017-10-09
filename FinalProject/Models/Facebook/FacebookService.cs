using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace FinalProject.Models.Facebook
{
    public static class FacebookSettings
    {
        public static string AccessToken = "EAAHuIBhqFLkBAPHa1Ao1Iq9QhTBjpXu2ZASqafghbasZC87rUur7NIvIm7jEqrry9rqE3eeemgGBZC7MiD9qWwItiNMgj9XQq56SKfIINKFi5l3D1YglYY4hgKDOG6kumyemEZB3WwuuUjYHOT7n3g7RcTSH4VHGatzMkgZCC27DNnIHMfC0NNOp6WwimCUkGHkZBoq54FPAZDZD";
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