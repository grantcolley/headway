﻿using System.Net.Http;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IApiRequest
    {
        Task<IResponse<T>> GetResponseAsync<T>(HttpResponseMessage httpResponseMessage);
    }
}
