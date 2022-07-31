using System;
using Xunit;

namespace Insight.TelegramBot.Tests
{
    public sealed class BotTests
    {
        [Fact]
        public void BotCtor_ClientIsNull_ThrowsANE()
        {
            Assert.Throws<ArgumentNullException>(() => new Bot(null));
        }
    }
}
