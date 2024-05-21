using MySql.Data.MySqlClient;
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
    public partial class ListeleForm : Form
    {
        string baglanti = "Server=localhost;Database=barinak;Uid=root;Pwd='';";
        string yeniAd;
        public ListeleForm()
        {
            InitializeComponent();
        }
        private void ListeleForm_Load(object sender, EventArgs e)
        {
            DgwDoldur();
            CmbDoldur();
        }
        private void dgwHayvanlar_SelectionChanged(object sender, EventArgs e)
        {
            if (dgwHayvanlar.SelectedCells.Count > 0)
            {
                txtAd.Text = dgwHayvanlar.SelectedRows[0].Cells["adi"].Value.ToString();
                txtYas.Text = dgwHayvanlar.SelectedRows[0].Cells["yas"].Value.ToString();
                cmbCins.SelectedValue = dgwHayvanlar.SelectedRows[0].Cells["cins"].Value.ToString();
                cbEngel.Checked = Convert.ToBoolean(dgwHayvanlar.SelectedRows[0].Cells["engel_durumu"].Value);

                string dosyaYolu = Path.Combine(Environment.CurrentDirectory, "fotograf", dgwHayvanlar.SelectedRows[0].Cells["fotograf_adi"].Value.ToString());

                pbFotograf.ImageLocation = null;
                if (File.Exists(dosyaYolu))
                {
                    pbFotograf.ImageLocation = dosyaYolu;
                    pbFotograf.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
        }

        void DgwDoldur()
        {
            using (MySqlConnection baglan = new MySqlConnection(baglanti))
            {
                baglan.Open();
                string sorgu = "SELECT * FROM hayvanlar;";

                MySqlCommand cmd = new MySqlCommand(sorgu, baglan);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);
                dgwHayvanlar.DataSource = dt;

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

        private void btnSil_Click(object sender, EventArgs e)
        {
            DataGridViewRow dr = dgwHayvanlar.SelectedRows[0];

            int id = Convert.ToInt32(dr.Cells[0].Value);

            string fotografYol = Path.Combine(Environment.CurrentDirectory, "fotograf", dgwHayvanlar.SelectedRows[0].Cells["fotograf_adi"].Value.ToString());


            DialogResult cevap = MessageBox.Show("Kayıtı silmek istediğinizden emin misiniz?",
                                                 "Kayıtı sil", MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Warning);


            if (cevap == DialogResult.Yes)
            {

                using (MySqlConnection baglan = new MySqlConnection(baglanti))
                {
                    int H_id = Convert.ToInt32(dgwHayvanlar.SelectedRows[0].Cells["id"].Value);
                    baglan.Open();
                    string sorgu = "DELETE FROM hayvanlar WHERE id=@id;";
                    MySqlCommand cmd = new MySqlCommand(sorgu, baglan);
                    cmd.Parameters.AddWithValue("@id", H_id);
                    cmd.ExecuteNonQuery();


                    if (File.Exists(fotografYol))
                    {

                        File.Delete(fotografYol);
                    }
                    DgwDoldur();

                }
            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            using (MySqlConnection baglan = new MySqlConnection(baglanti))
            {
                baglan.Open();
                string sorgu = "UPDATE hayvanlar SET adi=@adi,yas=@yas,cins=@cins,engel_durumu=@engel_durumu,fotograf_adi=@fotograf_adi WHERE id=@id";
                MySqlCommand cmd = new MySqlCommand(sorgu, baglan);
                cmd.Parameters.AddWithValue("@adi", txtAd.Text);
                cmd.Parameters.AddWithValue("@yas", txtYas.Text);
                cmd.Parameters.AddWithValue("@cins", cmbCins.SelectedValue);
                cmd.Parameters.AddWithValue("@engel_durumu", cbEngel.Checked);
                cmd.Parameters.AddWithValue("@fotograf_adi", yeniAd);
                int id = Convert.ToInt32(dgwHayvanlar.SelectedRows[0].Cells["id"].Value);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
                DgwDoldur();
            }
        }

        private void pbFotograf_Click(object sender, EventArgs e)
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
    }
}
