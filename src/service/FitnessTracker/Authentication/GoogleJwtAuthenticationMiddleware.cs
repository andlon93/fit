using FitnessTracker.Users;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace FitnessTracker.Authentication
{
    public class GoogleJwtAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly HttpClient _httpClient;
        private const string _googleApiTokenInfoUrl = "https://www.googleapis.com/oauth2/v3/tokeninfo?id_token={0}";
        private readonly UserQueryService _userQueryService;

        public GoogleJwtAuthenticationMiddleware(RequestDelegate next, IHttpClientFactory factory, UserQueryService userQueryService)
        {
            _next = next;
            _httpClient = factory.CreateClient(AuthorizationConstants.GoogleAuthHttpClientName);
            _userQueryService = userQueryService;
        }

        public async Task Invoke(HttpContext context)
        {
            var googleToken = await ValidateJwtAmdGetUserDetailsAsync(context.Request.Headers[AuthorizationConstants.AuthorizationHeaderName].FirstOrDefault()?.Split(" ").Last());
            context.Items.Add(AuthorizationConstants.GoogleIdContextTitle, googleToken?.sub);

            var authorId = googleToken?.sub != null ? (await GetAuthorIdFromJwtAsync(googleToken.sub))?.ToString() : null;
            context.Items.Add(AuthorizationConstants.AuthorIdContextTitle, authorId);

            await _next(context);
        }

        private async Task<GoogleApiTokenInfo?> ValidateJwtAmdGetUserDetailsAsync(string? jwt)
        {
            if (string.IsNullOrEmpty(jwt)) { return null; }

            try
            {
                // TODO: should one cache valid tokens or something to avoid calling googles API for auth validation all the time, or is that insecure to do?
                var request = new HttpRequestMessage(HttpMethod.Get, new Uri(string.Format(_googleApiTokenInfoUrl, jwt)));
                var httpResponseMessage = await _httpClient.SendAsync(request);

                if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
                {
                    Log.Warning("Http request for google authorization not suucessfull. " + httpResponseMessage.ReasonPhrase);
                    return null;
                }

                return JsonSerializer.Deserialize<GoogleApiTokenInfo>(httpResponseMessage.Content.ReadAsStringAsync().Result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Google authentication failed");
                return null;
            }
        }

        private async Task<Guid?> GetAuthorIdFromJwtAsync(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                Log.Warning("Found no valid user token in the http context.");
                return null;
            }

            var user = await _userQueryService.GetUserByGoogleId(key);
            if (user == null)
            {
                Log.Warning($"Found no user with google id {key} in the database.");
                return null;
            }

            return user.Id;
        }
    }
}
