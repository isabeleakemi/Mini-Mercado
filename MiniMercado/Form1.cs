using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using System.Drawing.Imaging;

namespace MiniMercado
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        MySqlConnection con = new MySqlConnection("datasource=localhost;port=3306;username=root;password=H1u2n3e4s");
        private string strMySQL;

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private byte[] ConverterParaBitArray()
        {
            MemoryStream stream = new MemoryStream();
            byte[] bArray;

            if(pictureBox1.Image == null)
            {
                stream = null;
                bArray = new byte[stream.Length];
            }
            else if(pictureBox1.Image != null)
            {
                pictureBox1.Image.Save(stream, ImageFormat.Png);
                stream.Seek(0, SeekOrigin.Begin);
            }

            bArray = new byte[stream.Length];
            stream.Read(bArray, 0, Convert.ToInt32(stream.Length));
            return bArray;
        }

        private void CadastrarProduto()
        {
            strMySQL = "insert into mercado.produtos(produtoID, nome, preco, quantidade, foto) values(@produtoID, @nome, @preco, @quantidade, @foto)";
            MySqlCommand comando = new MySqlCommand(strMySQL, con);

            comando.Parameters.AddWithValue("@produtoID", txtCod.Text);
            comando.Parameters.AddWithValue("@nome", txtNome.Text);
            comando.Parameters.Add("@preco", MySqlDbType.Float).Value = txtValor.Text;
            comando.Parameters.AddWithValue("@quantidade", txtQuant.Text);
            comando.Parameters.AddWithValue("@foto", ConverterParaBitArray());

            try
            {
                con.Open();
                comando.ExecuteNonQuery();
                MessageBox.Show("PRODUTO CADASTRADO COM SUCESSO!", "MENSAGEM", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog abrir = new OpenFileDialog();
            abrir.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            abrir.Filter = "Image Files (*.bmp, *.jpg, *.png, *.jpeg)| *.bmp; *.jpg; *.png; *.jpeg";
            abrir.Multiselect = false;

            if(abrir.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.Image = new Bitmap(abrir.FileName);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CadastrarProduto();
            txtCod.Clear();
            txtNome.Clear();
            txtQuant.Clear();
            txtValor.Clear();
            pictureBox1.Image = null;
            txtCod.Focus();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Form2 f2 = new Form2();
            f2.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txtCod_Validating(object sender, CancelEventArgs e)
        {
            strMySQL = "select * from mercado.produtos";
            MySqlCommand comando = new MySqlCommand(strMySQL, con);

            try
            {
                con.Open();
                MySqlDataReader dr = comando.ExecuteReader();

                while (dr.Read())
                {
                    if((dr.HasRows) && (dr["produtoID"].ToString() == txtCod.Text))
                    {
                        MessageBox.Show("CÓDIGO DE BARRAS JÁ CADASTRADO! TENTE NOVAMENTE", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txtCod.Clear();
                        txtCod.Focus();
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            FormCaixa fc = new FormCaixa();
            fc.ShowDialog();
        }
    }
}
