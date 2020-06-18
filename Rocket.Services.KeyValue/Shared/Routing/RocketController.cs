using Microsoft.AspNetCore.Mvc;
using Rocket.Services.KeyValue.Exceptions;
using Rocket.Services.KeyValue.Models;
using System;

namespace Rocket.Services.KeyValue.Shared.Routing
{
    [Route("api/v1/[controller]")]
    public abstract class RocketController : ControllerBase
    {
        

        public RocketController()
        {
            
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