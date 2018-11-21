using Fusillade;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpevoCore.Services.API_Service
{
    public interface IApiService<T>
    {
        T GetApi(Priority priority);
    }
}
