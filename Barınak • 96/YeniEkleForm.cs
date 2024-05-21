using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Barınak___96
{
    public partial class YeniEkleForm : Form
    {
        string baglanti = "Server=localhost;Database=barinak;Uid=root;Pwd='';";
        string yeniAd;
        public YeniEkleForm()
        {
            InitializeComponent();
        }
        private void YeniEkleForm_Load(object sender, EventArgs e)
        {
            CmbDoldur();
        }
        private void btnEkle_Click(object sender, EventArgs e)
        {
            using (MySqlConnection baglan = new MySqlConnection(baglanti))
            {
                baglan.Open();
                string sorgu = "INSERT INTO hayvanlar VALUES(NULL, @adi,@yas,@cins,@engel_durumu,@fotograf_adi);";
                MySqlCommand cmd = new MySqlCommand(sorgu, baglan);
                cmd.Parameters.AddWithValue("@adi", txtAd.Text);
                cmd.Parameters.AddWithValue("@yas", txtYas.Text);
                cmd.Parameters.AddWithValue("@cins", cmbCins.SelectedValue);
                cmd.Parameters.AddWithValue("@engel_durumu", cbEngel.Checked);
                cmd.Parameters.AddWithValue("@fotograf_adi", yeniAd);

                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Kayıt Eklendi");
                }
            }


        }

        private void pbResim_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;";
            DialogResult result = openFileDialog.ShowDialog(this);

            if (result != DialogResult.OK) return;

            string kaynakDosya = openFileDialog.FileName;
            yeniAd = Guid.NewGuid().ToString() + Path.GetExtension(kaynakDosya);
            string hedefDosya = Path.Combine(Environment.CurrentDirectory, "fotograf", yeniAd);

            File.Copy(kaynakDosya, hedefDosya);

            pbFotograf.Image = null;
            if (File.Exists(hedefDosya))
            {
                pbFotograf.Image = Image.FromFile(hedefDosya);
                pbFotograf.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        void CmbDoldur()
        {
            using (MySqlConnection baglan = new MySqlConnection(baglanti))
            {
                baglan.Open();
                string sorgu = "SELECT DISTINCT cins FROM hayvanlar;";

                MySqlCommand cmd = new MySqlCommand(sorgu, baglan);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);
                cmbCins.DataSource = dt;

                cmbCins.DisplayMember = "cins";
                cmbCins.ValueMember = "cins";

            }
        }


    }
}
