using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Windows.Forms;

namespace miPrimeaAplicacion
{
    public partial class frm_usuario : Form
    {
        Conexion conexion = new Conexion();
        int posicion = 0;

        private TextBox txtId, txtUsuario, txtClave, txtConfirmarClave, txtNombre, txtDireccion, txtTelefono;
        private DataGridView dataGridView1;
        private Button btnAgregar, btnModificar, btnEliminar, btnAnterior, btnSiguiente, btnMenuPrincipal;

        public frm_usuario()
        {
            InitializeComponentCustom();
            CargarDatos();
            BloquearCampos();
        }

        private void frm_usuario_Load(object sender, EventArgs e)
        {
            // Método requerido por el Diseñador
        }

        private void InitializeComponentCustom()
        {
            this.Text = "Mantenimiento de Usuarios - UGB";
            this.Size = new Size(920, 480);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.BackColor = Color.FromArgb(248, 250, 252);

            Panel panelHeader = new Panel() { Dock = DockStyle.Top, Height = 65, BackColor = Color.FromArgb(26, 54, 93) };
            Label lblTitulo = new Label() { Text = "Gestión de Usuarios", Font = new Font("Segoe UI", 16, FontStyle.Bold), ForeColor = Color.White, Location = new Point(20, 10), AutoSize = true };
            Label lblSubtitulo = new Label() { Text = "Universidad Gerardo Barrios", Font = new Font("Segoe UI", 9.5f), ForeColor = Color.FromArgb(203, 213, 225), Location = new Point(23, 38), AutoSize = true };

            btnMenuPrincipal = new Button() { Text = "Menú Principal", Location = new Point(750, 15), Size = new Size(130, 32), BackColor = Color.FromArgb(71, 85, 105), ForeColor = Color.White, Font = new Font("Segoe UI", 9f, FontStyle.Bold), FlatStyle = FlatStyle.Flat };
            btnMenuPrincipal.FlatAppearance.BorderSize = 0;
            btnMenuPrincipal.Click += (s, e) => {
                frmPrincipal principal = new frmPrincipal();
                principal.Show();
                this.Hide();
            };

            panelHeader.Controls.Add(lblTitulo);
            panelHeader.Controls.Add(lblSubtitulo);
            panelHeader.Controls.Add(btnMenuPrincipal);
            this.Controls.Add(panelHeader);

            int x = 30, y = 80;

            txtId = CrearCampo("ID Usuario:", ref y, x, true);
            txtId.BackColor = Color.FromArgb(241, 245, 249);
            txtUsuario = CrearCampo("Usuario (6-16):", ref y, x);
            txtClave = CrearCampo("Contraseña:", ref y, x, false, true);
            txtConfirmarClave = CrearCampo("Confirmar Clave:", ref y, x, false, true);
            txtNombre = CrearCampo("Nombre:", ref y, x);
            txtDireccion = CrearCampo("Dirección:", ref y, x);
            txtTelefono = CrearCampo("Teléfono:", ref y, x);

            int navY = y + 5;
            Label lblNav = new Label() { Text = "Navegación:", Location = new Point(x, navY + 5), Font = new Font("Segoe UI", 9f, FontStyle.Bold), AutoSize = true };
            this.Controls.Add(lblNav);

            btnAnterior = new Button() { Text = "<", Location = new Point(x + 100, navY), Size = new Size(45, 30), Font = new Font("Segoe UI", 9.5f, FontStyle.Bold) };
            btnAnterior.Click += (s, e) => { if (posicion > 0) { posicion--; MostrarRegistro(posicion); } };

            btnSiguiente = new Button() { Text = ">", Location = new Point(x + 155, navY), Size = new Size(45, 30), Font = new Font("Segoe UI", 9.5f, FontStyle.Bold) };
            btnSiguiente.Click += (s, e) => { if (posicion < dataGridView1.Rows.Count - 1) { posicion++; MostrarRegistro(posicion); } };

            this.Controls.Add(btnAnterior);
            this.Controls.Add(btnSiguiente);

            int actY = navY + 45;
            btnAgregar = CrearBotonAccion("Agregar", x, actY, Color.FromArgb(40, 167, 69), btnAgregar_Click);
            btnModificar = CrearBotonAccion("Modificar", x + 102, actY, Color.FromArgb(235, 137, 37), btnModificar_Click);
            btnEliminar = CrearBotonAccion("Eliminar", x + 204, actY, Color.FromArgb(220, 53, 69), btnEliminar_Click);

            dataGridView1 = new DataGridView() { Location = new Point(370, 80), Size = new Size(510, 345) };
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.CellClick += (s, e) => {
                if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
                {
                    posicion = e.RowIndex;
                    MostrarRegistro(posicion);
                }
            };
            this.Controls.Add(dataGridView1);
        }

        private TextBox CrearCampo(string etiqueta, ref int yPos, int xPos, bool readOnly = false, bool isPassword = false)
        {
            Label lbl = new Label() { Text = etiqueta, Location = new Point(xPos, yPos), Font = new Font("Segoe UI", 9f, FontStyle.Bold), AutoSize = true };
            TextBox txt = new TextBox() { Location = new Point(xPos + 130, yPos - 2), Size = new Size(190, 24), Font = new Font("Segoe UI", 9.5f), ReadOnly = readOnly };
            if (isPassword) txt.PasswordChar = '*';
            this.Controls.Add(lbl);
            this.Controls.Add(txt);
            yPos += 35;
            return txt;
        }

        private Button CrearBotonAccion(string texto, int x, int y, Color color, EventHandler evento)
        {
            Button btn = new Button() { Text = texto, Location = new Point(x, y), Size = new Size(96, 35), BackColor = color, ForeColor = Color.White, Font = new Font("Segoe UI", 9f, FontStyle.Bold), FlatStyle = FlatStyle.Flat };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += evento;
            this.Controls.Add(btn);
            return btn;
        }

        private void BloquearCampos()
        {
            txtUsuario.ReadOnly = true;
            txtClave.ReadOnly = true;
            txtConfirmarClave.ReadOnly = true;
            txtNombre.ReadOnly = true;
            txtDireccion.ReadOnly = true;
            txtTelefono.ReadOnly = true;

            Color grisClaro = Color.FromArgb(241, 245, 249);
            txtUsuario.BackColor = grisClaro;
            txtClave.BackColor = grisClaro;
            txtConfirmarClave.BackColor = grisClaro;
            txtNombre.BackColor = grisClaro;
            txtDireccion.BackColor = grisClaro;
            txtTelefono.BackColor = grisClaro;
        }

        private void DesbloquearCampos(bool limpiar)
        {
            txtUsuario.ReadOnly = false;
            txtClave.ReadOnly = false;
            txtConfirmarClave.ReadOnly = false;
            txtNombre.ReadOnly = false;
            txtDireccion.ReadOnly = false;
            txtTelefono.ReadOnly = false;

            Color blanco = Color.White;
            txtUsuario.BackColor = blanco;
            txtClave.BackColor = blanco;
            txtConfirmarClave.BackColor = blanco;
            txtNombre.BackColor = blanco;
            txtDireccion.BackColor = blanco;
            txtTelefono.BackColor = blanco;

            if (limpiar)
            {
                txtId.Clear();
                txtUsuario.Clear();
                txtClave.Clear();
                txtConfirmarClave.Clear();
                txtNombre.Clear();
                txtDireccion.Clear();
                txtTelefono.Clear();
                txtUsuario.Focus();
            }
        }

        private void CargarDatos()
        {
            try
            {
                DataSet ds = conexion.obtenerDatos();
                dataGridView1.DataSource = ds.Tables["usuarios_usss087526"];
                if (dataGridView1.Rows.Count > 0) MostrarRegistro(posicion);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message);
            }
        }

        private void MostrarRegistro(int index)
        {
            if (index >= 0 && index < dataGridView1.Rows.Count)
            {
                DataGridViewRow row = dataGridView1.Rows[index];
                if (!row.IsNewRow)
                {
                    txtId.Text = row.Cells["idUsuario"].Value?.ToString();
                    txtUsuario.Text = row.Cells["usuario"].Value?.ToString();
                    txtClave.Text = row.Cells["clave"].Value?.ToString();
                    txtConfirmarClave.Text = row.Cells["clave"].Value?.ToString();
                    txtNombre.Text = row.Cells["nombre"].Value?.ToString();
                    txtDireccion.Text = row.Cells["dirección"].Value?.ToString();
                    txtTelefono.Text = row.Cells["teléfono"].Value?.ToString();
                    BloquearCampos();
                }
            }
        }

        private bool ValidarSeguridadClave(string clave)
        {
            return Regex.IsMatch(clave, "[a-zA-Z]") && Regex.IsMatch(clave, "[0-9]");
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (txtUsuario.ReadOnly)
            {
                DesbloquearCampos(true);
                MessageBox.Show("Campos habilitados. Ingrese los datos y presione 'Agregar' nuevamente para guardar.", "Nuevo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string usuario = txtUsuario.Text.Trim();
            string clave = txtClave.Text;

            if (usuario.Length < 6 || usuario.Length > 16)
            {
                MessageBox.Show("El usuario debe tener entre 6 y 16 caracteres.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (clave != txtConfirmarClave.Text)
            {
                MessageBox.Show("Las contraseñas no coinciden.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidarSeguridadClave(clave))
            {
                MessageBox.Show("La contraseña debe llevar letras y números.", "Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string[] datos = { "0", usuario, clave, txtNombre.Text, txtDireccion.Text, txtTelefono.Text };
            string res = conexion.administrarDatosUsuarios(datos, "nuevo");

            if (res == "1")
            {
                MessageBox.Show("Usuario registrado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarDatos();
                BloquearCampos();
            }
            else
            {
                MessageBox.Show("Error al guardar: " + res);
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Text)) return;

            if (txtUsuario.ReadOnly)
            {
                DesbloquearCampos(false);
                MessageBox.Show("Campos habilitados para edición. Modifique y presione 'Modificar' nuevamente para guardar.", "Editar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string usuario = txtUsuario.Text.Trim();
            string clave = txtClave.Text;

            if (usuario.Length < 6 || usuario.Length > 16 || clave != txtConfirmarClave.Text || !ValidarSeguridadClave(clave))
            {
                MessageBox.Show("Verifique las reglas de validación.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string[] datos = { txtId.Text, usuario, clave, txtNombre.Text, txtDireccion.Text, txtTelefono.Text };
            string res = conexion.administrarDatosUsuarios(datos, "modificar");

            if (res == "1")
            {
                MessageBox.Show("Usuario modificado con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarDatos();
                BloquearCampos();
            }
            else
            {
                MessageBox.Show("Error al modificar: " + res);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Text)) return;

            if (MessageBox.Show("¿Está seguro de eliminar este usuario?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string[] datos = { txtId.Text, "", "", "", "", "" };
                string res = conexion.administrarDatosUsuarios(datos, "eliminar");

                if (res == "1")
                {
                    MessageBox.Show("Usuario eliminado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CargarDatos();
                    BloquearCampos();
                }
                else
                {
                    MessageBox.Show("Error al eliminar: " + res);
                }
            }
        }
    }
}