using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dominio;

namespace Negocio
{
    public class DiscoNegocio
    {
		//Este metodo me permite conectarme a la DB y obtener un conjunto de datos, correspondiente en este caso, a tablas relacionadas de la DB
		//que guardo en primer lugar en un reader, luego leo este reader para obtener los datos, almacenarlos en un objeto de la clase correspondiente
		//y por último agrego a una lista.
        public List<Disco> Listar()
        {
			//Creo una lista donde se van a guardar los datos que contenga el lector
			List<Disco> listaDiscos = new List<Disco>();
			
			//Instancio un objeto de la clase AccesoDatos para poder establecer la conexcion con la DB y
			//obtener los datos del lector.
			AccesoDatos datos = new AccesoDatos();

			try
			{
				datos.ConfigurarConsulta("Select D.Id, Titulo, FechaLanzamiento, CantidadCanciones, UrlImagenTapa, E.Id, E.Descripcion Genero, T.Id, T.Descripcion Formato from DISCOS D, ESTILOS E, TIPOSEDICION T where D.IdEstilo = E.Id and D.IdEstilo = T.Id");
				datos.EjectutarLectura();
				
				//Realizo la lectura de los datos que contiene el lector y los asigno a las propiedades de un objeto de la clase Disco
				while (datos.Lector.Read())
				{
					Disco aux = new Disco()
					{
						Id = (int)datos.Lector["Id"],
						Titulo = (string)datos.Lector["Titulo"],
						CantidadCanciones = (int)datos.Lector["CantidadCanciones"],
						FechaLanzamiento = (DateTime)datos.Lector["FechaLanzamiento"],
						UrlImagen = (string)datos.Lector["UrlImagenTapa"],
						Genero = new Estilo()
						{
							Id = (int)datos.Lector["Id"],
							Descripcion = (string)datos.Lector["Genero"]
						},
						Formato = new TipoEdicion()
						{							
							Id = (int)datos.Lector["Id"],
							Descripcion = (string)datos.Lector["Formato"]
						}
					};

					listaDiscos.Add(aux);
				}

				return listaDiscos;
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
		
		//Este metodo me permite conectarme a la DB y agregar un nuevo registro
		public void AgregarDisco(Disco newDisco)
		{
			//Instancio un objeto de la clase AccesoDatos para poder utilizar sus metodos, es la clase que me permite conectarme a la DB y realizar consultas u operaciones
			AccesoDatos datos = new AccesoDatos();

			try
			{
				datos.ConfigurarConsulta($"Insert into DISCOS (Titulo, FechaLanzamiento, CantidadCanciones, UrlImagenTapa, IdEstilo, IdTipoEdicion) values ('{newDisco.Titulo}', @FechaLanzamiento, {newDisco.CantidadCanciones}, '{newDisco.UrlImagen}', @IdEstilo, @IdTipoEdicion)");
				datos.ConfigurarParametros("@IdEstilo", newDisco.Genero.Id);
				datos.ConfigurarParametros("@IdTipoEdicion", newDisco.Formato.Id);
				datos.ConfigurarParametros("@FechaLanzamiento", newDisco.FechaLanzamiento);
                datos.EjecutarAccion(); //Es este caso utilizo el metodo EjecutarAccion porque quiero agregar un
                                        //registro en la DB, corresponde a una operacion NonQuery
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
		public void ModificarDisco(Disco discoEdit)
		{
			AccesoDatos datos = new AccesoDatos();

			try
			{
				datos.ConfigurarConsulta($"Update DISCOS set Titulo = '{discoEdit.Titulo}', FechaLanzamiento = @FechaLanzamiento, CantidadCanciones = {discoEdit.CantidadCanciones},UrlImagenTapa = '{discoEdit.UrlImagen}', IdEstilo = {discoEdit.Genero.Id}, IdTipoEdicion = {discoEdit.Formato.Id} where Id = {discoEdit.Id}");
				datos.ConfigurarParametros("@FechaLanzamiento", discoEdit.FechaLanzamiento);
				datos.EjecutarAccion();

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
		//Metodo para eliminar de forma fisica el objeto seleccioando de la lista, recibe como parámetro el Id del objeto según la DB
		public void EliminarDisco(int id)
		{
			AccesoDatos datos = new AccesoDatos();

			try
			{
				datos.ConfigurarConsulta($"Delete from DISCOS where id = {id}");
				datos.EjecutarAccion();
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
