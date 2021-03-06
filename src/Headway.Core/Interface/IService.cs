﻿using System.Net.Http;
using System.Threading.Tasks;

namespace Headway.Core.Interface
{
    public interface IService
    {
        Task<IServiceResult<T>> GetServiceResult<T>(HttpResponseMessage httpResponseMessage);
    }
}
