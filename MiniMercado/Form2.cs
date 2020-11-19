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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        MySqlConnection con = new MySqlConnection("datasource=localhost;port=3306;username=root;password=H1u2n3e4s");
        private string strMySQL;

        private void ConsultarProduto()
        {
            strMySQL = "select * from mercado.produtos where produtoID='"+txtConsulta.Text+"'";
            MySqlDataAdapter comando = new MySqlDataAdapter(strMySQL, con);

            try
            {
                con.Open();
                DataSet ds = new DataSet();
                comando.Fill(ds, "mercado.produtos");
                int cont = ds.Tables["mercado.produtos"].Rows.Count;

                if(cont == 0)
                {
                    MessageBox.Show("PRODUTO NÃO CADASTRADO! TENTE NOVAMENTE!", "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    txtConsulta.Clear();
                    txtConsulta.Focus();
                }
                else if(cont > 0)
                {
                    byte[] data = new Byte[0];
                    data = (Byte[])(ds.Tables["mercado.produtos"].Rows[cont - 1]["foto"]);
                    MemoryStream stream = new MemoryStream(data);
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox1.Image = Image.FromStream(stream);
                    txtCod.Text = Convert.ToString(ds.Tables["mercado.produtos"].Rows[cont - 1]["produtoID"]);
                    txtNome.Text = Convert.ToString(ds.Tables["mercado.produtos"].Rows[cont - 1]["nome"]);
                    txtQuant.Text = Convert.ToString(ds.Tables["mercado.produtos"].Rows[cont - 1]["quantidade"]);
                    txtValor.Text = Convert.ToString(ds.Tables["mercado.produtos"].Rows[cont - 1]["preco"]);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            txtConsulta.Focus();
        }

        private void AlterarProduto()
        {
            strMySQL = "update mercado.produtos set produtoID=@produtoID, nome=@nome, preco=@preco, quantidade=@quantidade, foto=@foto where produtoID='" + txtCod.Text + "'";
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
                MessageBox.Show("DADOS SALVOS COM SUCESSO!", "MENSAGEM", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        private void ExcluirProduto()
        {
            strMySQL = "delete from mercado.produtos where produtoID='" + txtCod.Text + "'";
            MySqlCommand comando = new MySqlCommand(strMySQL, con);

            try
            {
                con.Open();
                comando.ExecuteNonQuery();
                MessageBox.Show("ITEM EXCLUÍDO COM SUCESSO!", "MENSAGEM", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "ERRO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        private byte[] ConverterParaBitArray()
        {
            MemoryStream stream = new MemoryStream();
            byte[] bArray;

            if (pictureBox1.Image == null)
            {
                stream = null;
                bArray = new byte[stream.Length];
            }
            else if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Save(stream, ImageFormat.Png);
                stream.Seek(0, SeekOrigin.Begin);
            }

            bArray = new byte[stream.Length];
            stream.Read(bArray, 0, Convert.ToInt32(stream.Length));
            return bArray;
        }

        private void txtConsulta_KeyUp(object sender, KeyEventArgs e)
        {
            ConsultarProduto();
            txtConsulta.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog abrir = new OpenFileDialog();
            abrir.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            abrir.Filter = "Image Files (*.bmp, *.jpg, *.png, *.jpeg)| *.bmp; *.jpg; *.png; *.jpeg";
            abrir.Multiselect = false;

            if (abrir.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox1.Image = new Bitmap(abrir.FileName);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AlterarProduto();
            txtCod.Clear();
            txtNome.Clear();
            txtQuant.Clear();
            txtValor.Clear();
            pictureBox1.Image = null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ExcluirProduto();
            txtCod.Clear();
            txtNome.Clear();
            txtQuant.Clear();
            txtValor.Clear();
            pictureBox1.Image = null;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Close();
            Form1 f1 = new Form1();
            f1.Visible = true;
        }
    }
}
