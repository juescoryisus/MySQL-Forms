﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace MySQL_Forms
{
    public partial class AulaForm : Form
    {
        private string connectionString = "Server=localhost;Database=Enti;Uid=root;Pwd=DAY20;";
        public AulaForm()
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
            int numero = Convert.ToInt32(textBox1.Text);
            int alumnos = Convert.ToInt32(textBox2.Text);
            int maestros = Convert.ToInt32(textBox3.Text);
            string grupo = textBox4.Text;
            bool status = true;  // Valor por defecto

            // Consulta SQL para insertar los datos en la tabla Aula
            string query = "INSERT INTO Aula (numero, alumnos, maestros, grupo, status) " +
                           "VALUES (@numero, @alumnos, @maestros, @grupo, @status)";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    MySqlCommand cmd = new MySqlCommand(query, conn);


                    // Agregar parámetros para evitar inyecciones SQL
                    cmd.Parameters.AddWithValue("@numero", numero);
                    cmd.Parameters.AddWithValue("@alumnos", alumnos);
                    cmd.Parameters.AddWithValue("@maestros", maestros);
                    cmd.Parameters.AddWithValue("@grupo", grupo);
                    cmd.Parameters.AddWithValue("@status", status);

                    // Abrir la conexión a la base de datos
                    conn.Open();

                    // Ejecutar la consulta INSERT
                    cmd.ExecuteNonQuery();

                    // Confirmación de que se ha agregado el registro
                    MessageBox.Show("Aula agregada correctamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar aula: {ex.Message}");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    int numero = Convert.ToInt32(row.Cells["numero"].Value);
                    int alumnos = Convert.ToInt32(row.Cells["alumnos"].Value);
                    int maestros = Convert.ToInt32(row.Cells["maestros"].Value);
                    string grupo = row.Cells["grupo"].Value.ToString();
                    bool status = Convert.ToBoolean(row.Cells["status"].Value);

                    string query = "INSERT INTO Aula (numero, alumnos, maestros, grupo, status) " +
                                   "VALUES (@numero, @alumnos, @maestros, @grupo, @status)";

                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            SqlCommand cmd = new SqlCommand(query, conn);

                            cmd.Parameters.AddWithValue("@numero", numero);
                            cmd.Parameters.AddWithValue("@alumnos", alumnos);
                            cmd.Parameters.AddWithValue("@maestros", maestros);
                            cmd.Parameters.AddWithValue("@grupo", grupo);
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
            MessageBox.Show("Aulas guardadas correctamente.");
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            //Limpiar TextBox y DataGridView

            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();

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
