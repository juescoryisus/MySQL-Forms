using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace MySQL_Forms
{
    public partial class EscuelaForm : Form
    {
        private string connectionString = "Server=localhost;Database=Enti;Uid=root;Pwd=DAY20;";
        public EscuelaForm()
        {
            InitializeComponent();
            CargarClientes();
        }
        private void CargarClientes()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM ", conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridView1.DataSource = dt; // Mostrar datos en el DataGridView
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar datos: {ex.Message}");
                }
            }
        }
        private void bibliotecaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainMenu mainMenu = new mainMenu();
            mainMenu.ShowDialog();
        }
        private void btnCargar_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                string nombreTabla = comboBox1.SelectedItem.ToString();

                CargarDatosDeTabla(nombreTabla);
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una tabla primero.");
            }
        }

        private void CargarDatosDeTabla(string nombreTabla)
        {
            string query = $"SELECT * FROM {nombreTabla}";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos: {ex.Message}");
            }
        }
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            string guardias = textBox1.Text;
            string maestros = textBox2.Text;
            string directivos = textBox3.Text;
            string clases = textBox4.Text;
            int idDireccionGeneral = Convert.ToInt32(textBox5.Text);
            bool status = true;

            string query = "INSERT INTO Escuela (Guardias, Maestros, Directivos, Clases, idDireccionGeneral, status) " +
                           "VALUES (@Guardias, @Maestros, @Directivos, @Clases, @idDireccionGeneral, @status)";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@Guardias", guardias);
                    cmd.Parameters.AddWithValue("@Maestros", maestros);
                    cmd.Parameters.AddWithValue("@Directivos", directivos);
                    cmd.Parameters.AddWithValue("@Clases", clases);
                    cmd.Parameters.AddWithValue("@idDireccionGeneral", idDireccionGeneral);
                    cmd.Parameters.AddWithValue("@status", status);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Escuela agregada correctamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar escuela: {ex.Message}");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow) // Verificar que no sea la fila vacía del final
                {
                    string guardias = row.Cells["Guardias"].Value.ToString();
                    string maestros = row.Cells["Maestros"].Value.ToString();
                    string directivos = row.Cells["Directivos"].Value.ToString();
                    string clases = row.Cells["Clases"].Value.ToString();
                    int idDireccionGeneral = Convert.ToInt32(row.Cells["idDireccionGeneral"].Value);
                    bool status = Convert.ToBoolean(row.Cells["status"].Value);

                    string query = "INSERT INTO Escuela (Guardias, Maestros, Directivos, Clases, idDireccionGeneral, status) " +
                                   "VALUES (@Guardias, @Maestros, @Directivos, @Clases, @idDireccionGeneral, @status)";

                    try
                    {
                        using (MySqlConnection conn = new MySqlConnection(connectionString))
                        {
                            MySqlCommand cmd = new MySqlCommand(query, conn);

                            // Agregar parámetros
                            cmd.Parameters.AddWithValue("@Guardias", guardias);
                            cmd.Parameters.AddWithValue("@Maestros", maestros);
                            cmd.Parameters.AddWithValue("@Directivos", directivos);
                            cmd.Parameters.AddWithValue("@Clases", clases);
                            cmd.Parameters.AddWithValue("@idDireccionGeneral", idDireccionGeneral);
                            cmd.Parameters.AddWithValue("@status", status);

                            // Abrir la conexión y ejecutar la consulta
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al agregar desde el DataGridView: {ex.Message}");
                    }
                }
            }

            // Confirmación de que las filas fueron agregadas
            MessageBox.Show("Registros agregados correctamente.");
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            //Limpiar TextBox y DataGridView

            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();

            dataGridView1.DataSource = null;
        }

        private void btnDeleate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);
            }
        }
    }
}
