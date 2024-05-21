using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Barınak___96
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            YeniEkleForm ekleForm = new YeniEkleForm();
            ekleForm.ShowDialog();
        }

        private void btnListele_Click(object sender, EventArgs e)
        {
            ListeleForm listeleForm = new ListeleForm();
            listeleForm.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string klasorYolu = @"fotograf";
            if (!Directory.Exists(klasorYolu))
            {
                Directory.CreateDirectory(klasorYolu);
            }
        }
    }
}
