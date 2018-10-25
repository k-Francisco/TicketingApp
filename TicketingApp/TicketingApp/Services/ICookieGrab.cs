using System;
using System.Collections.Generic;
using System.Text;

namespace TicketingApp.Services
{
    public interface ICookieGrab
    {
        string GrabCookies(string url);
    }
}
