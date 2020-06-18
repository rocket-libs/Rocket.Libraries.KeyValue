using Microsoft.AspNetCore.Mvc;
using Rocket.Apps.KeyValue.Models;
using Rocket.Apps.KeyValue.Services;
using Rocket.Libraries.ServiceProviders.Services;
using System;

namespace Rocket.Apps.KeyValue.Controllers
{
    public class RepositoryController : RocketController
    {
        public RepositoryController(IRocketServiceProvider rocketServiceProvider)
            : base(rocketServiceProvider)
        {
        }

        [Route("insert")]
        [HttpPost]
        public ResponseObject<KeyValueContainer> Insert([FromBody] KeyValueContainer container)
        {
            try
            {
                var repository = RocketServiceProvider.GetService<Repository>();
                return GetSuccessResponse(repository.Insert(container));
            }
            catch (Exception e)
            {
                return GetErrorResponse<KeyValueContainer>(e);
            }
        }

        [Route("get")]
        [HttpGet]
        public ResponseObject<KeyValueContainer> Get([FromQuery] string key)
        {
            try
            {
                var repository = RocketServiceProvider.GetService<Repository>();
                return GetSuccessResponse(repository.Get(key));
            }
            catch (Exception e)
            {
                return GetErrorResponse<KeyValueContainer>(e);
            }
        }

        [Route("delete")]
        [HttpDelete]
        public ResponseObject<KeyValueContainer> Delete([FromQuery] string key)
        {
            try
            {
                var repository = RocketServiceProvider.GetService<Repository>();
                return GetSuccessResponse(repository.Delete(key));
            }
            catch (Exception e)
            {
                return GetErrorResponse<KeyValueContainer>(e);
            }
        }
    }
}