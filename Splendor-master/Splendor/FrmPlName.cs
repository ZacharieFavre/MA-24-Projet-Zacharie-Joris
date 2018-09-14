using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Splendor
{
    public partial class FrmPlName : Form
    {
        public FrmPlName()
        {
            InitializeComponent();
        }

        private void cmdValidateName_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
