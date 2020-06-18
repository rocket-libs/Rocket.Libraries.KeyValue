using Microsoft.AspNetCore.Mvc;
using Rocket.Services.KeyValue.Models;
using System;
using Rocket.Services.KeyValue.Shared.Routing;

namespace Rocket.Services.KeyValue.Features.Repository
{
    public class RepositoryController : RocketController
    {
        private readonly IRepositoryWriter repositoryWriter;
        private readonly IRepositoryReader repositoryReader;

        public RepositoryController(
            IRepositoryWriter repositoryWriter,
            IRepositoryReader repositoryReader)
        {
            this.repositoryWriter = repositoryWriter;
            this.repositoryReader = repositoryReader;
        }

        [Route("insert")]
        [HttpPost]
        public ResponseObject<KeyValueContainer> Insert([FromBody] KeyValueContainer container)
        {
            try
            {
                return GetSuccessResponse(repositoryWriter.Insert(container));
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
                return GetSuccessResponse(repositoryReader.Get(key));
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
                return GetSuccessResponse(repositoryWriter.Delete(key));
            }
            catch (Exception e)
            {
                return GetErrorResponse<KeyValueContainer>(e);
            }
        }
    }
}