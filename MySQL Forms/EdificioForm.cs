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


namespace MySQL_Forms
{
    public partial class EdificioForm : Form
    {
        private string connectionString = "Server=localhost;Database=Enti;Uid=root;Pwd=DAY20;";
        public EdificioForm()
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
            // Recoger datos de los TextBoxes
            int numeroEdificio = Convert.ToInt32(textBox1.Text);
            int salones = Convert.ToInt32(textBox2.Text);
            bool status = true;  // Valor por defecto

            // Consulta SQL para insertar los datos en la tabla Edificio
            string query = "INSERT INTO Edificio (numeroEdificio, salones, status) " +
                           "VALUES (@numeroEdificio, @salones, @status)";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    // Agregar parámetros para evitar inyecciones SQL
                    cmd.Parameters.AddWithValue("@numeroEdificio", numeroEdificio);
                    cmd.Parameters.AddWithValue("@salones", salones);
                    cmd.Parameters.AddWithValue("@status", status);

                    // Abrir la conexión a la base de datos
                    conn.Open();

                    // Ejecutar la consulta INSERT
                    cmd.ExecuteNonQuery();

                    // Confirmación de que se ha agregado el registro
                    MessageBox.Show("Edificio agregado correctamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar edificio: {ex.Message}");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    int numeroEdificio = Convert.ToInt32(row.Cells["numeroEdificio"].Value);
                    int salones = Convert.ToInt32(row.Cells["salones"].Value);
                    bool status = Convert.ToBoolean(row.Cells["status"].Value);

                    string query = "INSERT INTO Edificio (numeroEdificio, salones, status) " +
                                   "VALUES (@numeroEdificio, @salones, @status)";

                    try
                    {
                        using (MySqlConnection conn = new MySqlConnection(connectionString))
                        {
                            MySqlCommand cmd = new MySqlCommand(query, conn);

                            cmd.Parameters.AddWithValue("@numeroEdificio", numeroEdificio);
                            cmd.Parameters.AddWithValue("@salones", salones);
                            cmd.Parameters.AddWithValue("@status", status);

                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al guardar desde el DataGridView: {ex.Message}");
                    }
                }
            }
            MessageBox.Show("Edificios guardados correctamente.");
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            //Limpiar TextBox y DataGridView

            textBox1.Clear();
            textBox2.Clear();

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
