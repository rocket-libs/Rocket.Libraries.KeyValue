using Newtonsoft.Json;
using Rocket.Apps.KeyValue.Models;
using Rocket.Libraries.ServiceProviders.Services;
using Rocket.Libraries.ServiceProviders.Services.Instantiatable;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Rocket.Apps.KeyValue.Services
{
    public class PersistantValuesLoader : ISingletonService
    {
        private bool _valuesAlreadyLoaded = false;
        public IService Parent { get; set; }
        public RocketServiceProvider RocketServiceProvider { get; set; }

        private static readonly object locker = new object();

        public void LoadPermanentValuesIfNeeded()
        {
            lock (locker)
            {
                if (_valuesAlreadyLoaded)
                {
                    return;
                }
                else
                {
                    var files = GetFiles();
                    InjectValuesToCache(files);
                    _valuesAlreadyLoaded = true;
                }
            }
        }

        private List<string> GetFiles()
        {
            const string persistantValuesDirectory = "persistant-values";
            var permanentValuesDirectoryMissing = Directory.Exists(persistantValuesDirectory) == false;
            if (permanentValuesDirectoryMissing)
            {
                return null;
            }
            else
            {
                var files = Directory.GetFiles(persistantValuesDirectory, "*.json");
                return files.ToList();
            }
        }

        private void InjectValuesToCache(List<string> files)
        {
            var nothingToInject = files == null || files.Count == 0;
            if (nothingToInject)
            {
                return;
            }
            else
            {
                var repository = RocketServiceProvider.GetService<Repository>();
                foreach (var jsonFile in files)
                {
                    var jsonString = GetFileContents(jsonFile);
                    var container = JsonConvert.DeserializeObject<KeyValueContainer>(jsonString);
                    repository.Insert(container);
                }
            }
        }

        private string GetFileContents(string jsonFile)
        {
            using (var fileStream = new FileStream(jsonFile, FileMode.Open))
            {
                using (var streamReader = new StreamReader(fileStream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
    }
}