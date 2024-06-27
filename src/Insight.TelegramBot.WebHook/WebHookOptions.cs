using System;

namespace Insight.TelegramBot.WebHook
{
    public sealed class WebHookOptions
    {
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