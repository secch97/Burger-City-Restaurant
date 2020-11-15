using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestauranteWeb.Models
{
    public class CarritoCompras
    {
        public static List<modeloCarritoObjetos> objetos { get; set; }

        public void insertarObjeto(string idObjeto, string nombre, int cantidad, decimal precio, string imagen)
        {
            //inicializando lista
            if(objetos == null)
            {
                objetos = new List<modeloCarritoObjetos>();
            }
            modeloCarritoObjetos objeto = new modeloCarritoObjetos();
            objeto.idObjeto = idObjeto;
            objeto.nombre = nombre;
            objeto.cantidad = cantidad;
            objeto.precio = precio;
            objeto.imagen = imagen;
            objeto.monto = cantidad * precio;
            objetos.Add(objeto);

        }

        public bool existeObjeto(string idObjeto)
        {
            if (objetos == null)
                return false;
            else if (objetos.Find(x => x.idObjeto == idObjeto) != null)
                return true;
            else
                return false;

        }


        public void modificarObjeto(string idObjeto, int cantidad)
        {
            int indice = 0;

            indice = objetos.FindIndex(x => x.idObjeto == idObjeto);

            //Actualizando cantidad
            objetos[indice].cantidad += cantidad;
            //Actualizando monto
            objetos[indice].monto = objetos[indice].cantidad * objetos[indice].precio;

        }

        public decimal cambiarCantidad(string idObjeto, int cantidad)
        {
            int indice = 0;

            indice = objetos.FindIndex(x => x.idObjeto == idObjeto);
            if (indice != -1)
            {
                //Actualizando cantidad
                objetos[indice].cantidad = cantidad;
                //Actualizando monto
                objetos[indice].monto = objetos[indice].cantidad * objetos[indice].precio;

                return objetos[indice].monto;
            }
            else
            {
                return 0;
            }
            
        }

        public int countObjetos()
        {
            if (objetos != null)
                return objetos.Count;
            else
                return 0;
        }

        public decimal obtenerMonto()
        {
            if (objetos != null)
                return objetos.Sum((x => x.precio * x.cantidad));
            else
                return 0;
                           
        }

        public void eliminarProducto(string idObjeto)
        {
            if (objetos != null)
                objetos.RemoveAt(objetos.FindIndex(x=>x.idObjeto == idObjeto));

        }

 
    }

    public class modeloCarritoObjetos
    {
        public string idObjeto { get; set; }
        public string nombre { get; set; }
        public int cantidad { get; set; }
        public decimal precio { get; set; }
        public string imagen { get; set; }
        public decimal monto { get; set; }

    }
}