using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dominio;

namespace Negocio
{
    //Esta clase me permite centralizar la conexión a la Base de Datos
    public class AccesoDatos
    {
        //Campos necesarios para conectarme a la DB y hacer realizar operaciones

        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;

        //Constructor de la clase, este me permite que al instanciar un objeto se le asigne los valores en campos conexion y comando.
        public AccesoDatos()
        {
            conexion = new SqlConnection("server = .\\SQLEXPRESS; database = DISCOS_DB; integrated security = true");
            comando = new SqlCommand();
        }

        //Propiedad para leer el lector desde el exterior. El lector es donde se guardan los datos proveniente de la DB
        public SqlDataReader Lector 
        {
            get { return lector; }
        }

        //Metodo para configurar la consulta u operacion que necesito realizar contra la DB, recibe como parámetro
        //una cadena de caracteres (Select, Insert, Delete, Update)
        public void ConfigurarConsulta(string consulta)
        {
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;
        }

        //Este metodo me permite hacer una lectura en la Base de Datos, es decir, obtener un conjunto de datos correspondientes
        //a una tabla de la DB.
        public void EjectutarLectura()
        {
            comando.Connection = conexion;

            try
            {
                //Abro conexion con la DB y guardo los datos obtenidos en el campo lector
                conexion.Open();
                lector = comando.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Este metodo me permite conectarme a la DB, pero lo voy a utilizar cuando realiza una operacion en la DB
        //conocida como NonQuery (Insert, Delete, Update)
        public void EjecutarAccion()
        {
            comando.Connection = conexion;

            try
            {
                conexion.Open();            //Los comandos del tipo NonQuery se utilizan cuando se necesita ejecutar una operación que
                comando.ExecuteNonQuery();  //que no devuelven un conjunto de resultados, sino que realizan cambio en la DB (Inser, Deleter, Update)
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Este metodo me permite configurar ciertos parámetros cuando necesito realizar una operación contra la DB
        //es útil cuando hay datos que estan relacionados y pertenecen a diferentes tablas de la DB.
        //Los parámetros que recibe dependen del metodo AddWithValue.
        public void ConfigurarParametros (string parametro, object variable)
        {
            comando.Parameters.AddWithValue(parametro, variable);
        }

        //Metodo para cerrar la conexion y el lector, siempre y cuando el lector contenga datos.
        public void CerrarConexion()
        {
            lector?.Close(); //Forma corta de comprobar si el lector es null o no, si no lo es, cierra la conexion.
            conexion.Close();
        }
    }
}
