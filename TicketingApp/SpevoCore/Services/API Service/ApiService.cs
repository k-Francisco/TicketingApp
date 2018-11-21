using Fusillade;
using Refit;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SpevoCore.Services.API_Service
{
    public class ApiService<T> : IApiService<T>
    {
        private Func<HttpMessageHandler, T> createClient;

        public ApiService(string baseAddress)
        {
            createClient = messageHandler =>
            {
                var handler = new HttpClientHandler();
                var domain = TokenService.GetInstance.ExtractDomain();
                handler.CookieContainer = new CookieContainer();
                handler.CookieContainer.Add(new Cookie("rtFa", TokenService.GetInstance.ExtractRtFa(), "/", domain));
                handler.CookieContainer.Add(new Cookie("FedAuth", TokenService.GetInstance.ExtractFedAuth(), "/", domain));

                var mediaType = new MediaTypeWithQualityHeaderValue("application/json");
                mediaType.Parameters.Add(new NameValueHeaderValue("odata", "verbose"));

                var client = new HttpClient(handler)
                {
                    BaseAddress = new Uri(baseAddress),
                };
                client.DefaultRequestHeaders.Accept.Add(mediaType);

                return RestService.For<T>(client);
            };
        }

        private T Background
        {
            get
            {
                return new Lazy<T>(() => createClient(
                    new RateLimitedHttpMessageHandler(new HttpClientHandler(), Priority.Background))).Value;
            }
        }

        private T UserInitiated
        {
            get
            {
                return new Lazy<T>(() => createClient(
                    new RateLimitedHttpMessageHandler(new HttpClientHandler(), Priority.UserInitiated))).Value;
            }
        }

        private T Speculative
        {
            get
            {
                return new Lazy<T>(() => createClient(
                    new RateLimitedHttpMessageHandler(new HttpClientHandler(), Priority.Speculative))).Value;
            }
        }

        public T GetApi(Priority priority)
        {
            switch (priority)
            {
                case Priority.Background:
                    return Background;

                case Priority.UserInitiated:
                    return UserInitiated;

                case Priority.Speculative:
                    return Speculative;

                default:
                    return UserInitiated;
            }
        }
    }
}