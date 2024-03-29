using System;

namespace Insight.TelegramBot.Hosting.Options
{
    public sealed class WebHookOptions
    {
        public bool UseWebHook { get; set; }

        public string WebHookBaseUrl { get; set; }

        public string WebHookPath { get; set; }
        
        public bool DropPendingUpdatesOnDeleteWebhook { get; set; }

        public string WebHookUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(WebHookBaseUrl))
                    throw new ArgumentNullException(nameof(WebHookBaseUrl));

                if (string.IsNullOrWhiteSpace(WebHookPath))
                    throw new ArgumentNullException(nameof(WebHookPath));

                return new UriBuilder(WebHookBaseUrl) {Path = WebHookPath}.ToString();
            }
        }
    }
}