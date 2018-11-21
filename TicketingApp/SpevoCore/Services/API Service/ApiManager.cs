using Fusillade;
using Polly;
using Refit;
using SpevoCore.Models.FormDigest;
using SpevoCore.Models.User;
using SpevoCore.Services.Sharepoint_API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SpevoCore.Services.API_Service
{
    public class ApiManager : IApiManager
    {
        private static ApiManager instance;
        public bool IsConnected { get; set; }
        public bool IsReachable { get; set; }
        private Dictionary<int, CancellationTokenSource> runningTasks = new Dictionary<int, CancellationTokenSource>();
        private Dictionary<string, Task<HttpResponseMessage>> taskContainer = new Dictionary<string, Task<HttpResponseMessage>>();

        private ApiManager()
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                IsConnected = true;

            Connectivity.ConnectivityChanged += OnConnectivityChanged;
        }

        public static ApiManager GetInstance
        {
            get
            {
                if (instance == null)
                    instance = new ApiManager();

                return instance;
            }
        }

        private void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                IsConnected = true;
            else
                IsConnected = false;

            if (!IsConnected)
            {
                var items = runningTasks.ToList();
                foreach (var item in items)
                {
                    item.Value.Cancel();
                    runningTasks.Remove(item.Key);
                }
            }
        }

        protected async Task<TData> RemoteRequestAsync<TData>(Task<TData> task)
            where TData : HttpResponseMessage,
            new()
        {
            TData data = new TData();

            data = await Policy
                .Handle<WebException>()
                .Or<ApiException>()
                .Or<TaskCanceledException>()
                .WaitAndRetryAsync(
                    retryCount: 1,
                    sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                )
                .ExecuteAsync(async () =>
                {
                    var result = await task;

                    if (result.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        //tokens expire so maybe you log out the user?
                    }

                    return result;
                });

            return data;
        }

        public async Task<HttpResponseMessage> AddTask(Task<HttpResponseMessage> task)
        {
            var cts = new CancellationTokenSource();
            var addedTask = RemoteRequestAsync<HttpResponseMessage>(task);
            runningTasks.Add(addedTask.Id, cts);

            return await addedTask;
        }
    }

    public interface IApiManager
    {
        Task<HttpResponseMessage> AddTask(Task<HttpResponseMessage> task);
    }
}