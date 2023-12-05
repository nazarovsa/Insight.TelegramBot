using System;
using Insight.TelegramBot.Abstractions.Configurations;
using Xunit;

namespace Insight.TelegramBot.Tests
{
    public class WebHookConfigurationTests
    {
        [Theory]
        [InlineData("http://site.com/", "update", "http://site.com:80/update")]
        [InlineData("https://site.com/", "update", "https://site.com:443/update")]
        [InlineData("https://site.com/", "/update", "https://site.com:443/update")]
        [InlineData("https://site.com", "/update/token", "https://site.com:443/update/token")]
        [InlineData("https://site.com/", "/update/token", "https://site.com:443/update/token")]
        public void Should_return_correct_uri(string baseUrl, string path, string url)
        {
            var configuration = new WebHookConfiguration
            {
                UseWebHook = true,
                WebHookBaseUrl = baseUrl,
                WebHookPath = path
            };
            
            Assert.Equal(url, configuration.WebHookUrl, StringComparer.InvariantCultureIgnoreCase);
        }

        [Fact]
        public void Should_throw_ANE_when_baseUrl_isEmpty()
        {
            var configuration = new WebHookConfiguration
            {
                UseWebHook = true,
                WebHookBaseUrl = null,
                WebHookPath = "path"
            };

            Assert.Throws<ArgumentNullException>(() => configuration.WebHookUrl);
        }
        
        [Fact]
        public void Should_throw_ANE_when_path_isEmpty()
        {
            var configuration = new WebHookConfiguration
            {
                UseWebHook = true,
                WebHookBaseUrl = "host",
                WebHookPath = null
            };

            Assert.Throws<ArgumentNullException>(() => configuration.WebHookUrl);
        }
    }
}