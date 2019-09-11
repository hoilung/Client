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
    public partial class OuterLinkControl : UserControl
    {
        public OuterLinkControl()
        {
            InitializeComponent();
            if (!File.Exists("link.dat"))
            {
                var file = Client.Properties.Resources.link;
                File.WriteAllText("link.dat",file);

            }
        }
    }
}
