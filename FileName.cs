using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoSelectCommond
{
    public partial class FileName : Form
    {
        Form1 _form;
        public FileName(Form1 form)
        {
            this._form = form;
            InitializeComponent();
        }

        private void FileName_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {

            if (textBox1.Text=="")
            {
                MessageBox.Show("請輸入");
            }
            else
            {
                _form.FileName = textBox1.Text;
                this.Close();
            }
            
        }
    }
}
