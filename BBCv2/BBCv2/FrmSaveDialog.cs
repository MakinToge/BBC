using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BBCv2
{
    public partial class FrmSaveDialog : Form
    {
        public String _identificationNumber { get; set; }
        public FrmSaveDialog()
        {
            InitializeComponent();
        }

        private void FrmSaveDialog_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            _identificationNumber = textBox1.Text;
        }
    }
}
