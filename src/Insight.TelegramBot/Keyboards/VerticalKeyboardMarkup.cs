using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types.ReplyMarkups;

namespace Insight.TelegramBot.Keyboards
{
    public sealed class VerticalKeyboardMarkup
    {
        private readonly List<IEnumerable<IKeyboardButton>> _buttons;

        public InlineKeyboardMarkup InlineKeyboardMarkup =>
            new InlineKeyboardMarkup(
                _buttons.Select(x => x.Select(b => b as InlineKeyboardButton)));

        public ReplyKeyboardMarkup ReplyKeyboardMarkup =>
            new ReplyKeyboardMarkup(_buttons
                .Select(x => x.Select(b => b as KeyboardButton)));

        public VerticalKeyboardMarkup()
        {
            _buttons = new List<IEnumerable<IKeyboardButton>>();
        }

        public VerticalKeyboardMarkup(IEnumerable<IKeyboardButton> buttons) : this()
        {
            _buttons.AddRange(buttons.Select(x => new[] {x}));
        }

        public void Add(IKeyboardButton button)
        {
            if (button == null)
                throw new ArgumentNullException(nameof(button));

            _buttons.Add(new[] {button});
        }

        public void AddRange(IEnumerable<IKeyboardButton> buttons)
        {
            if (buttons == null)
                throw new ArgumentNullException(nameof(buttons));

            foreach (var button in buttons)
                Add(button);
        }

        public void AddRow(IEnumerable<IKeyboardButton> buttons)
        {
            if (buttons == null)
                throw new ArgumentNullException(nameof(buttons));

            var keyboardButtons = buttons as IKeyboardButton[] ?? buttons.ToArray();
            if (!keyboardButtons.Any())
                throw new ArgumentException($"Empty collection: {nameof(buttons)}");

            _buttons.Add(keyboardButtons);
        }
    }
}