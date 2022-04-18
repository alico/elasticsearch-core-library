using ElasticSearch7x.Core.Interfaces;
using Nest;
using System;

namespace ElasticSearch7x.Core
{
    public class ElasticClientFactory : IElasticClientFactory
    {
        private string _serverUrl;
        private string _userName;
        private string _password;
        private string _defaultIndexName;

        private ElasticClientFactory()
        {
        }

        public static IElasticClientFactory Init()
        {
            return new ElasticClientFactory();
        }

        public IElasticClient Create()
        {
            var settings = new ConnectionSettings(new Uri(_serverUrl));
            settings.DefaultIndex(_defaultIndexName);

            if (!string.IsNullOrEmpty(_userName) && !string.IsNullOrEmpty(_password))
            {
                settings.BasicAuthentication(_userName, _password);
            }

            return new ElasticClient(settings);
        }

        public IElasticClientFactory ServerUrl(string url)
        {
            _serverUrl = url;
            return this;
        }

        public IElasticClientFactory UserName(string userName)
        {
            _userName = userName;
            return this;
        }

        public IElasticClientFactory Password(string password)
        {
            _password = password;
            return this;
        }

        public IElasticClientFactory DefaultIndex(string indexName)
        {
            _defaultIndexName = indexName;
            return this;
        }
    }
}