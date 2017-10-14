using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace FinalProject.Models.Facebook
{
    public interface IFacebookService
    {
        Task PostOnWallAsync(string accessToken, string message);
    }

    public class FacebookService : IFacebookService
    {
        private readonly IFacebookClient _facebookClient;

        public FacebookService(IFacebookClient facebookClient)
        {
            _facebookClient = facebookClient;
        }

        public async Task PostOnWallAsync(string accessToken, string message)
            => await _facebookClient.PostAsync(accessToken, "me/feed", new { message });
    }
}