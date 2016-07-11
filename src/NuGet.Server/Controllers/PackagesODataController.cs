﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NuGet.Server.Core.Infrastructure;

namespace NuGet.Server.DataServices
{
    public class PackagesODataController : NuGet.Server.V2.Controllers.NuGetODataController
    {
        private static IServerPackageRepository Repository
        {
            get
            {
                // It's bad to use the container directly but we aren't in the loop when this 
                // class is created
                return ServiceResolver.Resolve<IServerPackageRepository>();
            }
        }

        private static IPackageAuthenticationService AuthenticationService
        {
            get
            {
                // It's bad to use the container directly but we aren't in the loop when this 
                // class is created
                return ServiceResolver.Resolve<IPackageAuthenticationService>();
            }
        }

        public PackagesODataController()
            :base (Repository, AuthenticationService)
        {
            _maxPageSize = 100;
        }

        [HttpGet]
        // Exposed through ordinary Web API route. Bypasses OData pipeline.
        public HttpResponseMessage ClearCache()
        {
            if (this.RequestContext.IsLocal)
            {
                _serverRepository.ClearCache();
                return CreateStringResponse(HttpStatusCode.OK, "Server cache has been cleared.");
            }
            else
            {
                return CreateStringResponse(HttpStatusCode.Forbidden, "Clear cache is only supported for local requests.");
            }
        }

    }
}