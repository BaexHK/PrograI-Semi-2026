using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace miPrimeaAplicacion
{
    public partial class frm_login : Form
    {
        Conexion conexion = new Conexion();
        private TextBox txtUsuario, txtClave;
        private Button btnIngresar;

        /* ========================================================
         * CREDENCIALES REGISTRADAS EN LA BD PARA PRUEBA DEL DOCENTE:
         * 1. Usuario: jfuentes   | Clave: Admin123*
         * 2. Usuario: vmerino    | Clave: Clave2026#
         * 3. Usuario: user_test  | Clave: Prueba987$
         * ======================================================== */

        public frm_login()
        {
            InitializeComponentCustom();
        }

        private void frm_login_Load(object sender, EventArgs e)
        {
            // Método requerido por el Diseñador
        }

        private void InitializeComponentCustom()
        {
            this.Text = "Iniciar Sesión - UGB";
            this.Size = new Size(420, 340);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(248, 250, 252);

            Panel panelHeader = new Panel() { Dock = DockStyle.Top, Height = 70, BackColor = Color.FromArgb(26, 54, 93) };
            Label lblTitulo = new Label() { Text = "Acceso al Sistema", Font = new Font("Segoe UI", 15, FontStyle.Bold), ForeColor = Color.White, Location = new Point(20, 15), AutoSize = true };
            Label lblSub = new Label() { Text = "Universidad Gerardo Barrios", Font = new Font("Segoe UI", 9f), ForeColor = Color.FromArgb(203, 213, 225), Location = new Point(23, 42), AutoSize = true };

            panelHeader.Controls.Add(lblTitulo);
            panelHeader.Controls.Add(lblSub);
            this.Controls.Add(panelHeader);

            Label lblUsuario = new Label() { Text = "Usuario:", Location = new Point(45, 95), Font = new Font("Segoe UI", 9.5f, FontStyle.Bold), AutoSize = true };
            txtUsuario = new TextBox() { Location = new Point(45, 120), Size = new Size(310, 26), Font = new Font("Segoe UI", 10f) };
            this.Controls.Add(lblUsuario);
            this.Controls.Add(txtUsuario);

            Label lblClave = new Label() { Text = "Contraseña:", Location = new Point(45, 160), Font = new Font("Segoe UI", 9.5f, FontStyle.Bold), AutoSize = true };
            txtClave = new TextBox() { Location = new Point(45, 185), Size = new Size(310, 26), Font = new Font("Segoe UI", 10f), PasswordChar = '*' };
            this.Controls.Add(lblClave);
            this.Controls.Add(txtClave);

            btnIngresar = new Button() { Text = "Ingresar", Location = new Point(45, 235), Size = new Size(310, 38), BackColor = Color.FromArgb(40, 167, 69), ForeColor = Color.White, Font = new Font("Segoe UI", 10f, FontStyle.Bold), FlatStyle = FlatStyle.Flat };
            btnIngresar.FlatAppearance.BorderSize = 0;
            btnIngresar.Click += new EventHandler(this.btnIngresar_Click);
            this.Controls.Add(btnIngresar);
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text.Trim();
            string clave = txtClave.Text;

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(clave))
            {
                MessageBox.Show("Por favor complete todos los campos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string query = "SELECT COUNT(*) FROM usuarios_usss087526 WHERE usuario = @usuario AND clave = @clave";
                SqlCommand cmd = new SqlCommand(query, conexion.objConexion);
                cmd.Parameters.AddWithValue("@usuario", usuario);
                cmd.Parameters.AddWithValue("@clave", clave);

                int resultado = Convert.ToInt32(cmd.ExecuteScalar());

                if (resultado > 0)
                {
                    MessageBox.Show("¡Bienvenido al sistema, " + usuario + "!", "Acceso Concedido", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    frm_usuario formUsuario = new frm_usuario();
                    formUsuario.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos.", "Error de Autenticación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con la base de datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}