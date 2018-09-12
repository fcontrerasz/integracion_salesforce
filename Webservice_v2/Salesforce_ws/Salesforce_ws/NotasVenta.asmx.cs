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
            string OC_Referencia,
            DateTime Fecha_OC,
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
            Respuesta notax = agregarNota(IdOportunidad, IdCuenta, IdPresupuesto,  RutCliente,  nombre_vendedor,  Moneda,  Total_neto,  Totiva,  Forma_de_Pago,  Obs_NV,  Obs_Factura,  Obs_GD,  Obs_FAV, Descto, Fecha, OC_Referencia, Fecha_OC, productos);
            Retorno Nota = new Retorno();
            try
            {

                System.Data.SqlClient.SqlConnection cnBKTest = new System.Data.SqlClient.SqlConnection(mistringconx);
                System.Data.SqlClient.SqlCommand cmdTest = new System.Data.SqlClient.SqlCommand("Sync_NV", cnBKTest);
                cmdTest.CommandType = System.Data.CommandType.StoredProcedure;

                cmdTest.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE",
                                        System.Data.SqlDbType.Int,
                                        4,
                                        System.Data.ParameterDirection.ReturnValue,
                                        false,
                                        ((System.Byte)(0)),
                                        ((System.Byte)(0)),
                                        "",
                                        System.Data.DataRowVersion.Current,
                                        null));

                cmdTest.Parameters.Add(new System.Data.SqlClient.SqlParameter("@id_cuenta", System.Data.SqlDbType.Text, 50));
                cmdTest.Parameters["@id_cuenta"].Value = IdCuenta;

                cmdTest.Parameters.Add(new System.Data.SqlClient.SqlParameter("@rutcliente", System.Data.SqlDbType.Text, 13));
                cmdTest.Parameters["@rutcliente"].Value = RutCliente;

                cmdTest.Parameters.Add(new System.Data.SqlClient.SqlParameter("@nombre_vendedor", System.Data.SqlDbType.Text, 50));
                cmdTest.Parameters["@nombre_vendedor"].Value = nombre_vendedor;

                cmdTest.Parameters.Add(new System.Data.SqlClient.SqlParameter("@moneda", System.Data.SqlDbType.Text, 5));
                cmdTest.Parameters["@moneda"].Value = Moneda;

                cmdTest.Parameters.Add(new System.Data.SqlClient.SqlParameter("@total_neto", System.Data.SqlDbType.Float, 20));
                cmdTest.Parameters["@total_neto"].Value = Total_neto;

                cmdTest.Parameters.Add(new System.Data.SqlClient.SqlParameter("@totiva", System.Data.SqlDbType.Int, 50));
                cmdTest.Parameters["@totiva"].Value = Totiva;

                cmdTest.Parameters.Add(new System.Data.SqlClient.SqlParameter("@form_pago", System.Data.SqlDbType.Text, 50));
                cmdTest.Parameters["@form_pago"].Value = Forma_de_Pago;
                
                cmdTest.Parameters.Add(new System.Data.SqlClient.SqlParameter("@obs_nv", System.Data.SqlDbType.Text, 250));
                cmdTest.Parameters["@obs_nv"].Value = Obs_NV;

                cmdTest.Parameters.Add(new System.Data.SqlClient.SqlParameter("@obs_fact", System.Data.SqlDbType.Text, 250));
                cmdTest.Parameters["@obs_fact"].Value = Obs_Factura;

                cmdTest.Parameters.Add(new System.Data.SqlClient.SqlParameter("@obs_gd", System.Data.SqlDbType.Text, 250));
                cmdTest.Parameters["@obs_gd"].Value = Obs_GD;

                cmdTest.Parameters.Add(new System.Data.SqlClient.SqlParameter("@obs_fav", System.Data.SqlDbType.Text, 250));
                cmdTest.Parameters["@obs_fav"].Value = Obs_FAV;

                cmdTest.Parameters.Add(new System.Data.SqlClient.SqlParameter("@descuento", System.Data.SqlDbType.Int, 50));
                cmdTest.Parameters["@descuento"].Value = Descto;

                cmdTest.Parameters.Add(new System.Data.SqlClient.SqlParameter("@fecha", System.Data.SqlDbType.Text, 10));
                cmdTest.Parameters["@fecha"].Value = Fecha;

                cmdTest.Parameters.Add(new System.Data.SqlClient.SqlParameter("@estado", System.Data.SqlDbType.Int, 50));
                cmdTest.Parameters["@estado"].Value = 0;

                cmdTest.Parameters.Add(new System.Data.SqlClient.SqlParameter("@estado_desc", System.Data.SqlDbType.Text, 250));
                cmdTest.Parameters["@estado_desc"].Value = "ENVIO DESDE WS";

                cmdTest.Parameters.Add(new System.Data.SqlClient.SqlParameter("@numoc", System.Data.SqlDbType.Int, 50));
                cmdTest.Parameters["@numoc"].Value = Convert.ToInt32(OC_Referencia);

                cmdTest.Parameters.Add(new System.Data.SqlClient.SqlParameter("@fecha_oc", System.Data.SqlDbType.Text, 10));
                cmdTest.Parameters["@fecha_oc"].Value = Fecha_OC;

                cnBKTest.Open();
                cmdTest.ExecuteNonQuery();

                string resultadoSQL = cmdTest.Parameters["@RETURN_VALUE"].Value.ToString();

                cnBKTest.Close();

                int resx = Convert.ToInt32(resultadoSQL);
                if (resx == 0 || resx == 1 || resx == 2)
                {
                    Nota.StageName = resx;
                    Nota.IdCuenta = "FALLO";
                }
                else
                {
                    string xxx = agregarProductos(IdCuenta, IdOportunidad, productos);
                    if (xxx == "OK")
                    {
                        Nota.StageName = resx;
                        Nota.IdCuenta = "OK";
                    }
                    else {
                        Nota.StageName = 0;
                        Nota.IdCuenta = xxx;
                    }
                    
                    
                }

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

                Nota.StageName = 0;
                Nota.IdCuenta = errorMessage;
            }


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
        private static Respuesta agregarNota(string IdOportunidad, string IdCuenta, string IdPresupuesto, string RutCliente, string nombre_vendedor, string Moneda, double Total_neto, int Totiva, string Forma_de_Pago, string Obs_NV, string Obs_Factura, string Obs_GD, string Obs_FAV, double Descto, DateTime Fecha, string OC_ref, DateTime fecha_OC, Productos productos)
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

               // agregarProductos(IdCuenta, IdOportunidad, productos);
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
