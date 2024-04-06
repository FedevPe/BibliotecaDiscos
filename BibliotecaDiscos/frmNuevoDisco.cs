using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms
{
    public partial class frmNuevoDisco : Form
    {
        //CAMPOS
        private Disco disco = null;

        public frmNuevoDisco()
        {
            InitializeComponent();
        }
        //
        //Sobrecarga del constructor
        //
        public frmNuevoDisco(Disco disco)
        {
            InitializeComponent();
            this.disco = disco;
            Text = "Modificar Disco";
            btnAceptar.Text = "Modificar";
        }
        private void frmNuevoDisco_Load(object sender, EventArgs e)
        {
            ObtenerDatos();
            CargarImagen(txtUrlImagen.Text);
        }
        //Metodo que configura el evento click del boton agregar, me permite asignar los valores de los controles
        //a los nuevos registros (Instancias de la clase Disco)
        private void bntAceptar_Click(object sender, EventArgs e)
        {
            //Objeto que me conecta con la DB y me permite agregar el nuevo registro utilizando el Metodo AgregarDisco
            DiscoNegocio negocio = new DiscoNegocio();

            try
            {
                //Si el disco llega hasta esta instruccion y sigue siendo null, es porque el usuario quiere agregar
                //un nuevo regisro, por lo tanto instancio un objeto.
                if (disco == null)
                {
                    disco = new Disco();
                }

                //Asigno valores de los controles a las propiedades del objeto
                disco.Titulo = txtTitulo.Text;
                disco.Genero = (Estilo)cbxGenero.SelectedItem;
                disco.Formato = (TipoEdicion)cbxFormato.SelectedItem;
                disco.FechaLanzamiento = dtpFechaLanzamiento.Value;
                disco.CantidadCanciones = int.Parse(txtCantCanciones.Text);
                disco.UrlImagen = txtUrlImagen.Text;

                //En caso se que se modifique un disco, es porque este ya existe en la DB, por lo tanto, tiene un Id
                //que se obtuvo al hacer la consulta con el metodo Listar().
                if (disco.Id != 0)
                {
                    negocio.ModificarDisco(disco);
                    MessageBox.Show("El disco se modifico correctamente. 👍");
                }
                else //En caso de que el usuario queire agregar un nuevo disco, este no tiene un Id en la DB
                {
                    negocio.AgregarDisco(disco);
                    MessageBox.Show("El disco se agrego correctamente. 😀");
                }

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void txtUrlImagen_TextChanged(object sender, EventArgs e)
        {
            CargarImagen(txtUrlImagen.Text);
        }
        private void ObtenerDatos()
        {
            //Instancio objetos de la capa Negocios para poder obtener los datos de la tabla Estilo y TipoEdicion
            //y agregarlos a los comboBox correspondientes.

            EstiloNegocio negocioEstilo = new EstiloNegocio();
            TipoEdicionNegocio negocioTipo = new TipoEdicionNegocio();

            try
            {
                //Utilizo el metodo Listar() para obtener los datos de la DB y agregarlos al comboBox que corresponda.
                //ValueMember ---> Indico cual es la clave;  DisplayMember ----> Indico cual es el valor
                cbxGenero.DataSource = negocioEstilo.Listar();
                cbxGenero.ValueMember = "Id";
                cbxGenero.DisplayMember = "Descripcion";

                cbxFormato.DataSource = negocioTipo.Listar();
                cbxFormato.ValueMember = "Id";
                cbxFormato.DisplayMember = "Descripcion";

                //Valido si disco sigue siendo una variable null, si no lo es, es porque el usuario desea modificar un disco
                //de la lista, por lo tanto, asigno sus valores en los controles.
                if(disco != null)
                {
                    txtTitulo.Text = disco.Titulo;
                    cbxGenero.SelectedItem = disco.Genero.Id;
                    cbxGenero.SelectedItem = disco.Formato.Id;
                    dtpFechaLanzamiento.Value = disco.FechaLanzamiento;
                    txtCantCanciones.Text = disco.CantidadCanciones.ToString();
                    txtUrlImagen.Text = disco.UrlImagen;
                    CargarImagen(txtUrlImagen.Text);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            if (txtUrlImagen.Text == "")
            {
                CargarImagen("https://cdn.domestika.org/c_fit,dpr_auto,f_auto,q_80,t_base_params,w_820/v1646399360/content-items/010/751/047/01-original.jpg?1646399360");
            }
            
        }
        public void CargarImagen(string imagen)
        {
            try
            {
                imgDisco.Load(imagen);
            }
            catch (Exception)
            {

                imgDisco.Load("https://cdn.domestika.org/c_fit,dpr_auto,f_auto,q_80,t_base_params,w_820/v1646399360/content-items/010/751/047/01-original.jpg?1646399360");
            }

        }
    }
}
