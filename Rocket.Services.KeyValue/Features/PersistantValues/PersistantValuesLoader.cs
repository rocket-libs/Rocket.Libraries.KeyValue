using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Rocket.Services.KeyValue.Features.Repository;
using Rocket.Services.KeyValue.Models;

namespace Rocket.Services.KeyValue.Features.PersistantValues
{
    public class PersistantValuesLoader : IHostedService
    {
        private bool _valuesAlreadyLoaded = false;

        private static readonly object locker = new object ();
        private readonly IRepositoryWriter repositoryWriter;

        public PersistantValuesLoader (IRepositoryWriter repositoryWriter)
        {
            this.repositoryWriter = repositoryWriter;
        }

        public async Task StartAsync (CancellationToken cancellationToken)
        {
            await Task.Run (() => LoadPermanentValuesIfNeeded ());
        }

        public async Task StopAsync (CancellationToken cancellationToken)
        {
            await Task.Run (() => { });
        }

        private void LoadPermanentValuesIfNeeded ()
        {
            lock (locker)
            {
                if (_valuesAlreadyLoaded)
                {
                    return;
                }
                else
                {
                    var files = GetFiles ();
                    InjectValuesToCache (files);
                    _valuesAlreadyLoaded = true;
                }
            }
        }

        private List<string> GetFiles ()
        {
            var persistantValuesDirectory = $"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}/persistant-values";
            var permanentValuesDirectoryMissing = Directory.Exists (persistantValuesDirectory) == false;
            if (permanentValuesDirectoryMissing)
            {
                return null;
            }
            else
            {
                var files = Directory.GetFiles (persistantValuesDirectory, "*.json");
                return files.ToList ();
            }
        }

        private void InjectValuesToCache (List<string> files)
        {
            var nothingToInject = files == null || files.Count == 0;
            if (nothingToInject)
            {
                return;
            }
            else
            {
                foreach (var jsonFile in files)
                {
                    var jsonString = GetFileContents (jsonFile);
                    var container = JsonConvert.DeserializeObject<KeyValueContainer> (jsonString);
                    repositoryWriter.Insert (container);
                }
            }
        }

        private string GetFileContents (string jsonFile)
        {
            using (var fileStream = new FileStream (jsonFile, FileMode.Open))
            {
                using (var streamReader = new StreamReader (fileStream))
                {
                    return streamReader.ReadToEnd ();
                }
            }
        }

        /*private int GetRetryPolicy()
        {
            return Policy
                .Handle<Excption>()
                .WaitAndRetry(5, retryAttempt => 
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) 
                );
        }*/
    }
}