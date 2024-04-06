using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class TipoEdicionNegocio
    {
        public List<TipoEdicion> Listar()
        {
            List<TipoEdicion> listaTiposEdicion = new List<TipoEdicion>();

            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.ConfigurarConsulta("Select Id, Descripcion From TIPOSEDICION");
                datos.EjectutarLectura();

                while (datos.Lector.Read())
                {
                    TipoEdicion aux = new TipoEdicion()
                    {
                        Id = (int)datos.Lector["Id"],
                        Descripcion = (string)datos.Lector["Descripcion"]
                    };

                    listaTiposEdicion.Add(aux);
                }

                return listaTiposEdicion;
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
