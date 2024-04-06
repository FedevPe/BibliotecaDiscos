using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Dominio
{
    public class Disco
    {
        //[DisplayName("")] ------> Annotations, sirve para configurar como se muestra el nombre de las columnas del dgv.
        public int Id { get; set; }

        [DisplayName("Título")]
        public string Titulo { get; set; }

        [DisplayName("Cant. Canciones")]
        public int CantidadCanciones{ get; set; }

        [DisplayName("Fecha de Lanzamiento")]
        public DateTime FechaLanzamiento { get; set; }

        [DisplayName("Url Imagen")]
        public string UrlImagen { get; set; }

        [DisplayName("Género")]
        public Estilo Genero { get; set; } //Esta propiedad es del tipo Estilo, por lo tanto, a su vez tiene Id y Descripción

        public TipoEdicion Formato { get; set; } //Esta propiedad es del tipo TipoEdición, por lo tano, a su vez tiene Id y Descripción

    }
}
