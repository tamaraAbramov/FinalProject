using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace FinalProject.Models.Facebook
{
    public static class FacebookSettings
    {
        public static string AccessToken = "EAAHuIBhqFLkBABf6MGfk1ZAPRq5H1KPK1k4fIimdXaSFTyDWvdjsM0Ryc3ouZCpd1uiVrJ7TfbLFkNWW3hfxjcQQ6CuFVUAZB1HsX0BtDZCMRowRW36DpXDdjfzrDJDEPGPFiCfKYa2C1oMeeXIJSRCfBKg6hIrd52cORWQnpogt4GvBcGTpyRIWQIDOnfAiEuZCx6ui7MwZDZD";
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