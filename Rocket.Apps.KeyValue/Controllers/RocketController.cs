using Microsoft.AspNetCore.Mvc;
using Rocket.Apps.KeyValue.Exceptions;
using Rocket.Apps.KeyValue.Models;
using Rocket.Libraries.ServiceProviders.Services;
using System;

namespace Rocket.Apps.KeyValue.Controllers
{
    [Route("api/v1/[controller]")]
    public abstract class RocketController : ControllerBase
    {
        protected IRocketServiceProvider RocketServiceProvider { get; }

        public RocketController(IRocketServiceProvider rocketServiceProvider)
        {
            RocketServiceProvider = rocketServiceProvider;
        }

        protected ResponseObject<TResponse> GetSuccessResponse<TResponse>(TResponse response)
        {
            return new ResponseObject<TResponse>
            {
                Code = 1,
                Payload = response
            };
        }

        protected ResponseObject<TResponse> GetErrorResponse<TResponse>(Exception e)
        {
            return new ResponseObject<TResponse>
            {
                Code = GetErrorCode(e),
            };
        }

        private int GetErrorCode(Exception e)
        {
            var isUnknownKeyException = e.GetType() == typeof(UnknownKeyException);
            if (isUnknownKeyException)
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }
    }
}