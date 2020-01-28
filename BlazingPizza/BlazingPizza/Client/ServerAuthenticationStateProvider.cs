using BlazingPizza.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazingPizza.Client
{
    public class ServerAuthenticationStateProvider : AuthenticationStateProvider
    {
        public ServerAuthenticationStateProvider(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public HttpClient HttpClient { get; }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var userInfo = await HttpClient.GetJsonAsync<UserInfo>("user");
            var identity = userInfo.IsAuthenticated
                ? new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, userInfo.Name) }, "serverauth")
                : new ClaimsIdentity();
            return new AuthenticationState(new ClaimsPrincipal(identity));




            //var claim = new Claim(ClaimTypes.Name, "Fake Account");
            //var identity = new ClaimsIdentity(new[] { claim }, "serverauth");
            //return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
        }
    }
}
