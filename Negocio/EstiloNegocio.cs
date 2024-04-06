using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class EstiloNegocio
    {
        public List<Estilo> Listar()
        {
            //Creo una lista del tipo estilos donde se van a guardar los objetos, con los datos obtenidos de la DB
            List<Estilo> listaEstilos = new List<Estilo>();

            //Instancio un objeto de la clase AccesoDatos para poder conectarme con la DB y obtener los datos.
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.ConfigurarConsulta("Select Id, Descripcion From ESTILOS");
                datos.EjectutarLectura();

                //Obtengo los datos del reader leyendolo y asignado los mismo a las propiedades del objeto aux de la clase Estilos
                while (datos.Lector.Read())
                {
                    Estilo aux = new Estilo()
                    {
                        Id = (int)datos.Lector["Id"],
                        Descripcion = (string)datos.Lector["Descripcion"]
                    };

                    listaEstilos.Add(aux);
                }

                return listaEstilos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
    }
}
