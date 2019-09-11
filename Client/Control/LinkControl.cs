using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Client.Control
{
    public partial class LinkControl : UserControl
    {
        public LinkControl()
        {
            InitializeComponent();

            initLink();
        }


        private void initLink()
        {
            Task.Run(() =>
            {
                if (File.Exists("link.dat"))
                {
                    var urls = File.ReadAllLines("link.dat");
                    for (int i = 0; i < urls.Length; i++)
                    {
                        var item = new ListViewItem(new[] { i.ToString(), urls[i] });
                        lv_link.Items.Add(item);
                    }
                }
            });
        }
    }
}
