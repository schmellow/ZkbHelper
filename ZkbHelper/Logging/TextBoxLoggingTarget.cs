using System;
using System.Windows.Controls;

namespace ZkbHelper.Logging
{
    public class TextBoxLoggingTarget : ILoggingTarget
    {
        private TextBox _textBox;

        public TextBoxLoggingTarget(TextBox textBox)
        {
            _textBox = textBox;
        }

        public void Write(string message)
        {
            _textBox.AppendText(message + Environment.NewLine);
        }
    }
}
