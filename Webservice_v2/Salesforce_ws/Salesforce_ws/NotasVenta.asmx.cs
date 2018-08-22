using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;

namespace Salesforce_ws
{


    /// <summary>
    /// Summary description for NotasVenta
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class NotasVenta : System.Web.Services.WebService
    {
        private static SqlConnection conexionBD;
        private static SqlDataReader rdr = null;
        public static string mistringconx = "Data Source=10.100.101.2,49675;MultipleActiveResultSets=true;Initial Catalog=ws_salesforce;Persist Security Info=True;User ID=entav;Password=*67914_pz8";
        //public static string mistringconx = "Data Source=localhost\\SQLEXPRESS;MultipleActiveResultSets=true;Initial Catalog=ws_salesforce;Persist Security Info=True;User ID=sa;Password=g4ng4sta";


       /* [WebMethod]
        public Retorno Test()
        {
            
            Respuesta nota = agregarNota("1","");
            Retorno Nota = new Retorno();
            Nota.StageName = nota.codigo;
            Nota.IdCuenta = nota.mensaje;
            return Nota;
        }*/

        [WebMethod]
        public Retorno Sincronizar(
            string IdOportunidad, 
            string IdCuenta, 
            string IdPresupuesto,
            DateTime Fecha,
            string RutCliente,
            string nombre_vendedor,
            string CodVendedor, 
            string Moneda,
            double Descto,
            double Total_neto,
            string Obs_GD,
            string Obs_Factura,
            string Obs_FAV,
            string Obs_NV,
            string Forma_de_Pago,
            int Totiva,
            //string RutVendedor, 
            //DateTime FechaDoc, 
            //string apellido_vendedor, 
            //string glosa_de_pago, 
            //string OC, 
            //int Ncodart, 
            //string Descripcion, 
            //double Cantidad, 
            Productos productos)
        {
            // Respuesta nota = agregarNota(IdCuenta, RutCliente, FechaDoc, nombre_vendedor, apellido_vendedor, Moneda, descuento, Total_neto, glosa_de_pago, Totiva, Forma_de_Pago, Obs_NV, Obs_Factura, Obs_GD, Obs_FAV, OC, Ncodart, Descripcion, Cantidad, Descto, Fecha);
            Respuesta nota = agregarNota(IdOportunidad, IdCuenta, IdPresupuesto,  RutCliente,  nombre_vendedor,  Moneda,  Total_neto,  Totiva,  Forma_de_Pago,  Obs_NV,  Obs_Factura,  Obs_GD,  Obs_FAV, Descto, Fecha, productos);
            Retorno Nota = new Retorno();
            Nota.StageName = nota.codigo;
            Nota.IdCuenta = nota.mensaje;
            return Nota;
        }

        private static string agregarProductos(string IdCuenta, string IdOpor, Productos prox)
        {
            String rex = "";
            try
            {
                conexionBD = new SqlConnection(mistringconx);
                conexionBD.Open();
                rdr = null;
                foreach (Producto e in prox.ListadoCursos)
                {
                    string sqlinsert = "INSERT INTO [dbo].[Todos_NV_Productos] ([IdCuenta] ,[IdOportunidad] ,[Idproducto] ,[ncodart] ,[descripcion] ,[cantidad] ,[precio] ,[descuento] ,[item] ,[fecha_entrega]) VALUES ('" + IdCuenta + "' ,'" + IdOpor + "' ,'" + e.Idproducto + "' ,'" + e.ncodart + "' ,'" + e.descripcion + "' ,'" + e.cantidad + "' ,'" + e.precio + "' ,'" + e.descuento + "' ,'" + e.item + "' ,'" + e.fecha_entrega + "')";
                    SqlCommand cmd = new SqlCommand(sqlinsert, conexionBD);
                    rdr = cmd.ExecuteReader();
                }
                rdr.Close();
                rex = "OK";
            }
            catch (Exception ex)
            {

                Exception ex2 = ex;
                string errorMessage = string.Empty;
                while (ex2 != null)
                {
                    errorMessage += ex2.ToString();
                    ex2 = ex2.InnerException;
                }
                rex = errorMessage;
            }

            return rex;
        }
        //private static Respuesta agregarNota(string IdCuenta, string RutCliente, DateTime FechaDoc, string nombre_vendedor, string apellido_vendedor, string Moneda, double descuento, double Total_neto, string glosa_de_pago, int Totiva, string Forma_de_Pago, string Obs_NV, string Obs_Factura, string Obs_GD, string Obs_FAV, string OC, int Ncodart, string Descripcion, double Cantidad, double Descto, DateTime Fecha)
        private static Respuesta agregarNota(string IdOportunidad, string IdCuenta, string IdPresupuesto, string RutCliente, string nombre_vendedor, string Moneda, double Total_neto, int Totiva, string Forma_de_Pago, string Obs_NV, string Obs_Factura, string Obs_GD, string Obs_FAV, double Descto, DateTime Fecha, Productos productos)
        {
            Respuesta rex = new Respuesta();
            try
            {
                conexionBD = new SqlConnection(mistringconx);
                conexionBD.Open();
                rdr = null;
                string sqlinsert = "INSERT INTO [dbo].[Todos_NotasVenta] VALUES ('"+ IdCuenta + "','" + RutCliente + "','" + nombre_vendedor + "','" + Moneda + "'," + Total_neto + "," + Totiva + ",'" + Forma_de_Pago + "','" + Obs_NV + "','" + Obs_Factura + "','" + Obs_GD + "','" + Obs_FAV + "'," + Descto + ",'" + Fecha + "',0,'CREADO DESDE WS')";
                SqlCommand cmd = new SqlCommand(sqlinsert, conexionBD);
                rdr = cmd.ExecuteReader();
                rdr.Close();
                rex.codigo = 1;
                rex.mensaje = "OK";

                agregarProductos(IdCuenta, IdOportunidad, productos);
            }
            catch (Exception ex)
            {

                Exception ex2 = ex;
                string errorMessage = string.Empty;
                while (ex2 != null)
                {
                    errorMessage += ex2.ToString();
                    ex2 = ex2.InnerException;
                }
                rex.codigo = 0;
                rex.mensaje = errorMessage;
            }

            return rex;

        }

        public class Retorno
        {
            public int StageName { get; set; }
            public string IdCuenta { get; set; }
        }

        public class Productos
        {
            public List<Producto> ListadoCursos { get; set; }
        }

        public class Producto
        {
            public string Idproducto { get; set; }
            public string ncodart { get; set; }
            public string descripcion { get; set; }
            public int cantidad { get; set; }
            public int precio { get; set; }
            public float descuento { get; set; }
            public string item { get; set; }
            public DateTime fecha_entrega { get; set; }
        }

        public class Respuesta
        {
            public int codigo { get; set; }
            public string mensaje { get; set; }
        }



    }
}
