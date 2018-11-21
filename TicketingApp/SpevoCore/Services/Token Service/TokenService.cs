using Newtonsoft.Json;
using SpevoCore.Services.Token_Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Essentials;

namespace SpevoCore.Services
{
    //this class is dependent on the plugin Xamarin.Essentials
    //It is required that you install the plugin on your project through nuget
    public class TokenService : ITokenService
    {

        private static TokenService instance;
        private const string COOKIE_KEY = "CookieString";

        private TokenService()
        {
        }

        public static TokenService GetInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TokenService();
                }

                return instance;
            }
        }

        public bool SaveCookies(string cookie, string url)
        {

            bool doneSavingCookies = false;

            if (cookie != null)
            {
                if (cookie.Contains("rtFa") && cookie.Contains("FedAuth"))
                {
                    Preferences.Set(COOKIE_KEY, JsonConvert.SerializeObject(cookie + ";domain=" + url));
                    doneSavingCookies = true;
                }
            }
            return doneSavingCookies;

        }

        public string ExtractRtFa()
        {
            string rtFa = string.Empty;
            try
            {
                string[] token = JsonConvert.DeserializeObject<string>(Preferences.Get(COOKIE_KEY, string.Empty)).Split(new char[] { ';' });

                for (int i = 0; i < token.Length; i++)
                {
                    if (token[i].Contains("rtFa"))
                    {
                        rtFa = token[i].Replace("rtFa=", "");
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("ExtractRtFa", e.Message);
            }

            return rtFa;
        }

        public string ExtractFedAuth()
        {
            string FedAuth = string.Empty;

            try
            {
                string[] token = JsonConvert.DeserializeObject<string>(Preferences.Get(COOKIE_KEY, string.Empty)).Split(new char[] { ';' });

                for (int i = 0; i < token.Length; i++)
                {
                    if (token[i].Contains("FedAuth"))
                    {
                        FedAuth = token[i].Replace("FedAuth=", "");
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("ExtractFedAuth", e.Message);
            }

            return FedAuth;
        }

        public string ExtractDomain()
        {
            string domain = string.Empty;

            try
            {
                string[] token = JsonConvert.DeserializeObject<string>(Preferences.Get(COOKIE_KEY, string.Empty)).Split(new char[] { ';' });

                for (int i = 0; i < token.Length; i++)
                {
                    if (token[i].Contains("domain"))
                    {
                        domain = token[i].Replace("domain=", "");
                    }
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine("ExtractDomain", e.Message);
            }

            return domain;
        }

        public bool IsAlreadyLoggedIn()
        {
            bool isLogged = false;

            if (!String.IsNullOrEmpty(Preferences.Get(COOKIE_KEY, string.Empty)))
                isLogged = true;

            return isLogged;
        }

        public void Clear()
        {
            Preferences.Clear();
        }
    }
}
