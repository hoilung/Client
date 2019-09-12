using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.Common
{
    public class ConsoleHelpler : TextWriter
    {
        public override Encoding Encoding => Encoding.UTF8;

        private System.Windows.Forms.TextBox _textBox { set; get; }
        public ConsoleHelpler(System.Windows.Forms.TextBox textBox)
        {
            this._textBox = textBox;
            Console.SetOut(this);
        }

        public override void WriteLine(string value)
        {
            try
            {
                if (_textBox.IsHandleCreated)
                    _textBox.BeginInvoke(new MethodInvoker(() => _textBox.AppendText(value + "\r\n")));
                //base.WriteLine(value);
            }
            catch (Exception)
            {

                //throw;
            }
        }
        public override void Write(string value)
        {
            try
            {
                if (_textBox.IsHandleCreated)
                    _textBox.BeginInvoke(new MethodInvoker(() => _textBox.AppendText(value + " ")));
            }
            catch (Exception)
            {
            }
        }
    }
}
