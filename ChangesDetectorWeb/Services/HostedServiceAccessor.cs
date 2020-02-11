﻿using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChangesDetectorWeb.Services
{
    public interface IHostedServiceAccessor<T> where T : IHostedService
    {
        T Service { get; }
    }

    public class HostedServiceAccessor<T> : IHostedServiceAccessor<T>
      where T : IHostedService
    {
        public HostedServiceAccessor(IEnumerable<IHostedService> hostedServices)
        {
            foreach (var service in hostedServices)
            {
                if (service is T match)
                {
                    Service = match;
                    break;
                }
            }
        }

        public T Service { get; }
    }
}
