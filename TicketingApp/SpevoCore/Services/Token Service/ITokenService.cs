using System;
using System.Collections.Generic;
using System.Text;

namespace SpevoCore.Services.Token_Service
{
    public interface ITokenService
    {
        bool SaveCookies(string cookie, string url);
        string ExtractRtFa();
        string ExtractFedAuth();
        string ExtractDomain();
        bool IsAlreadyLoggedIn();
        void Clear();
    }
}
