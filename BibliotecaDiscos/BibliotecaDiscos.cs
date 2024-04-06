using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;

namespace WinForms
{
    public partial class frmBiblioteca : Form
    {
        private List<Disco> listaDiscos = new List<Disco>();

        public frmBiblioteca()
        {
            InitializeComponent();
        }
        private void frmBiblioteca_Load(object sender, EventArgs e)
        {
            ActualizarLista();

            cboCriterio.Items.Add("Título");
            cboCriterio.Items.Add("Género");
            cboCriterio.Items.Add("Formato");
            cboCriterio.Items.Add("Cant.Canciones");

        }
        private void dgvDatos_SelectionChanged(object sender, EventArgs e)
        {
            if(dgvDatos.CurrentRow != null)
            {
                Disco discoSelect = (Disco)dgvDatos.CurrentRow.DataBoundItem;
                CargarImagen(discoSelect.UrlImagen);
            }
        }
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            frmNuevoDisco newFormulario = new frmNuevoDisco();
            newFormulario.ShowDialog();
            ActualizarLista(); //Este metodo me permite ver los cambios en la DB cuando se cierra el formulario
                               //donde cargo los datos del nuevo registro.
        }
        private void btnModificar_Click(object sender, EventArgs e)
        {
            Disco seleccionado;
            seleccionado = (Disco)dgvDatos.CurrentRow.DataBoundItem;

            frmNuevoDisco frmModificarDisco = new frmNuevoDisco(seleccionado);
            frmModificarDisco.ShowDialog();

            ActualizarLista();
        }
        private void btnEliminarFisico_Click(object sender, EventArgs e)
        {
            Eliminar();
        }
        private void ActualizarLista()
        {
            //Instancio un objetos de la clases de la capa Negocios para poder utilizar el metodo listar y obtener los datos de la DB.
            DiscoNegocio discoNegocio = new DiscoNegocio();

            try
            {
                listaDiscos = discoNegocio.Listar();
                //Agregar los datos al DataGridView
                dgvDatos.DataSource = listaDiscos;
                dgvDatos.Columns["UrlImagen"].Visible = false; //Ocultar columna
                dgvDatos.Columns["Id"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            imgDisco.Load();
        }
        private void CargarImagen(string imagen)
        {
            try
            {
                imgDisco.Load(imagen);
            }
            catch (Exception )
            {
               imgDisco.Load("https://cdn.domestika.org/c_fit,dpr_auto,f_auto,q_80,t_base_params,w_820/v1646399360/content-items/010/751/047/01-original.jpg?1646399360");
            }

        }
        private void Eliminar(bool logico = false)
        {
            //Clase que me permite acceder a la Db
            DiscoNegocio negocio = new DiscoNegocio();

            //Variable para datos de la fila seleccionada
            Disco seleccionado;

            try
            {
                seleccionado = (Disco)dgvDatos.CurrentRow.DataBoundItem;

                //Obtengo la respuesta del MessageBox para luego utilizar como un control
                DialogResult respuesta = MessageBox.Show("Estas seguro que quieres elminiar el disco?", "Eliminar Disco", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);

                //Si la respueste es Si, es decir, el usuario presiona el boton Si, el disco se borrara de forma definitiva de la DB. 
                //
                //Eliminacion Fisica
                //
                if (respuesta == DialogResult.Yes)
                {                    
                    negocio.EliminarDisco(seleccionado.Id);
                    MessageBox.Show("El disco se elimino correctamente.");
                    ActualizarLista();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void BuscarDisco()
        {
            List<Disco> listaFiltrada;

            try
            {
                if (txtFiltro.Text != "")
                {
                    if (cboCriterio.SelectedItem.ToString() == "Título")
                    {
                        listaFiltrada = listaDiscos.FindAll(x => x.Titulo.ToUpper().Contains(txtFiltro.Text.ToUpper()));
                    }
                    else if (cboCriterio.SelectedItem.ToString() == "Género")
                    {
                        listaFiltrada = listaDiscos.FindAll(x => x.Genero.Descripcion.ToUpper().Contains(txtFiltro.Text.ToUpper()));
                    }
                    else if (cboCriterio.SelectedItem.ToString() == "Formato")
                    {
                        listaFiltrada = listaDiscos.FindAll(x => x.Formato.Descripcion.ToUpper().Contains(txtFiltro.Text.ToUpper()));
                    }
                    else
                    {
                        listaFiltrada = listaDiscos.FindAll(x => x.CantidadCanciones <= int.Parse(txtFiltro.Text));
                    }
                }
                else
                {
                    listaFiltrada = listaDiscos;
                }

                dgvDatos.DataSource = null;
                dgvDatos.DataSource = listaFiltrada;
                dgvDatos.Columns["UrlImagen"].Visible = false; //Ocultar columna
                dgvDatos.Columns["Id"].Visible = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            BuscarDisco();
        }
        private void txtFiltro_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cboCriterio.SelectedItem.ToString() == "Cant.Canciones")
            {
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar); 
            }
        }
        private void cboCriterio_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFiltro.Text = "";
        }

        private void lblFiltro_Click(object sender, EventArgs e)
        {

        }
    }
}
