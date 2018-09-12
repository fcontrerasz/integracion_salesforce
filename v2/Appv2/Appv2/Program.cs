using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Appv2.sForce;
using System.ServiceModel;
using System.Net;
using System.Xml;
using System.IO;
using System.Data.SqlClient;
using System.Data;

namespace Appv2
{
    class Program
    {
        public static SoapClient client;
        private static SoapClient loginClient;
        private static SessionHeader sessionHeader;
        private static EndpointAddress endpoint;
        private static string urlServer;
        private static string idUsuario;
        private static string idSession;
        private static SqlConnection conexionBD;

        static void Main(string[] args)
        {
            Console.Write("**************************************************************" + Environment.NewLine);
            Console.Write("**                   PROCESO SALESFORCE                     **" + Environment.NewLine);
            Console.Write("**************************************************************" + Environment.NewLine);
            FingerPrint maquina = new FingerPrint();
            string makina = maquina.unico;
            Console.WriteLine("" + maquina.unico);
            //Console.ReadKey();
            if (makina != "BFD0-32E6-7C11-952D-7E62-1A49-16DE-D977")
            {
                string s_msg = "ERROR, EXISTEN CAMBIOS FISICOS O DE S.O EN LA MAQUINA PRINCIPAL, QUE IMPIDEN PROCESAR LA INTEGRACION. COD:" + makina + Environment.NewLine;               
                errorDump(s_msg);
                Console.WriteLine("EXITE UN ERROR");
               // Console.ReadKey();
                Environment.Exit(1);
            } 

            Console.WriteLine("" + Environment.NewLine);

            SqlDataReader rdr = null;
            string mistringconx = "Data Source=10.100.101.2,49675;MultipleActiveResultSets=true;Initial Catalog=ws_salesforce;Persist Security Info=True;User ID=entav;Password=*67914_pz8";
           // string mistringconx = "Data Source=localhost\\SQLEXPRESS;MultipleActiveResultSets=true;Initial Catalog=ws_salesforce;Persist Security Info=True;User ID=sa;Password=g4ng4sta";
           

            try
            {
                conexionBD = new SqlConnection(mistringconx);
                int contador = 0;

                Console.Write("***** CONECTANDOSE A SALESFORCE **********" + Environment.NewLine);
                Console.Write(Environment.NewLine);
                if (Login())
                {
                    Console.Write("USUARIO: " + idUsuario + Environment.NewLine);
                    Console.Write("URL: " + urlServer + Environment.NewLine);
                    Console.Write("SESSION: " + idSession + Environment.NewLine);
                    Console.Write(Environment.NewLine);
                    Console.Write("***** CONECTANDOSE A LA BASE DE DATOS **********" + Environment.NewLine);
                    conexionBD.Open();
                    System.Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.Write(" [OK] " + Environment.NewLine);
                    System.Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write(Environment.NewLine);
                    /*
                    System.Console.Write("***** PROCESANDO NUEVAS CUENTAS ******" + Environment.NewLine);
                    limpiarTabla("Todos_Cuentas");
                    listarCuentas();
                    System.Console.Write("***** PROCESANDO NUEVOS CONTACTOS ******" + Environment.NewLine);
                    limpiarTabla("Todos_Contactos");
                    listarContactos();
                    Console.ReadLine();
                    Environment.Exit(1);
                    */
                    using (SqlCommand cmd1 = new SqlCommand("sync_cuenta", conexionBD))
                    {
                        cmd1.CommandType = CommandType.StoredProcedure;

                        cmd1.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE",
                                        System.Data.SqlDbType.Int,
                                        4,
                                        System.Data.ParameterDirection.ReturnValue,
                                        false,
                                        ((System.Byte)(0)),
                                        ((System.Byte)(0)),
                                        "",
                                        System.Data.DataRowVersion.Current,
                                        null));

                        cmd1.ExecuteNonQuery();
                        string resultadoSQL = cmd1.Parameters["@RETURN_VALUE"].Value.ToString();
                        Console.Write("Procedimiento Sync_Cuenta: " + resultadoSQL + Environment.NewLine);
                    }

                    using (SqlCommand cmd2 = new SqlCommand("sync_contacto", conexionBD))
                    {
                        cmd2.CommandType = CommandType.StoredProcedure;

                        cmd2.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE",
                                        System.Data.SqlDbType.Int,
                                        4,
                                        System.Data.ParameterDirection.ReturnValue,
                                        false,
                                        ((System.Byte)(0)),
                                        ((System.Byte)(0)),
                                        "",
                                        System.Data.DataRowVersion.Current,
                                        null));

                        cmd2.ExecuteNonQuery();
                        string resultado2 = cmd2.Parameters["@RETURN_VALUE"].Value.ToString();
                        Console.Write("Procedimiento Sync_Contacto: " + resultado2 + Environment.NewLine);
                    }

                    using (SqlCommand cmd3 = new SqlCommand("sync_producto", conexionBD))
                    {
                        cmd3.CommandType = CommandType.StoredProcedure;

                        cmd3.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE",
                                        System.Data.SqlDbType.Int,
                                        4,
                                        System.Data.ParameterDirection.ReturnValue,
                                        false,
                                        ((System.Byte)(0)),
                                        ((System.Byte)(0)),
                                        "",
                                        System.Data.DataRowVersion.Current,
                                        null));

                        cmd3.ExecuteNonQuery();
                        string resultado3 = cmd3.Parameters["@RETURN_VALUE"].Value.ToString();
                        Console.Write("Procedimiento sync_producto: " + resultado3 + Environment.NewLine);
                    }



                    SqlCommand cmd = new SqlCommand();
                    string querym = "SELECT * FROM [ws_salesforce].[dbo].[listar_cuentas_q]";
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = querym;
                    cmd.Connection = conexionBD;
                    rdr = cmd.ExecuteReader();
                    contador = 0;

                    Console.Write("***** PROCESANDO CUENTAS **********" + Environment.NewLine);
                    

                    while (rdr.Read())
                    {
                        contador++;
                        string i_cta = rdr["ID_CUENTA"].ToString();
                        string r_cta = string.IsNullOrEmpty(rdr["RUT"].ToString()) ? "" : rdr["RUT"].ToString();
                        string s_cta = string.IsNullOrEmpty(rdr["RAZSOC"].ToString()) ? "" : rdr["RAZSOC"].ToString();
                        string d_cta = rdr["DIR"].ToString();
                        string g_cta = rdr["GIRO"].ToString();
                        string co_cta = rdr["COMUNA"].ToString();
                        string ci_cta = rdr["CIUDAD"].ToString();
                        string for_cta = rdr["FORMAP"].ToString();
                        int dias_cta = string.IsNullOrEmpty(rdr["DIASPAGO"].ToString()) ? 0 : Convert.ToInt32(rdr["DIASPAGO"].ToString());
                        double cd_cta = string.IsNullOrEmpty(rdr["CRED_APR"].ToString()) ? 0 : Convert.ToDouble(rdr["CRED_APR"].ToString());
                        string f_cta = string.IsNullOrEmpty(rdr["FONO"].ToString()) ? "" : rdr["FONO"].ToString();
                        string c1_cta = rdr["CLASE1"].ToString();
                        string c2_cta = rdr["CLASE2"].ToString();
                        string c3_cta = rdr["CLASE3"].ToString();
                        string c4_cta = rdr["CLASE4"].ToString();
                        double cdu_cta = string.IsNullOrEmpty(rdr["CRED_UTI"].ToString()) ? 0 : Convert.ToDouble(rdr["CRED_UTI"].ToString());
                        int n_cta = string.IsNullOrEmpty(rdr["MOROSID"].ToString()) ? 0 : Convert.ToInt32(rdr["MOROSID"].ToString());
                        Console.Write("ID_CUENTAS:" + i_cta + " | " + "RAZON: " + s_cta + " | " + "RUT: " + r_cta + " | " + "FONO: " + f_cta + Environment.NewLine);
                        // Console.ReadLine();
                        string proc = crearCuenta(i_cta, s_cta, r_cta, f_cta, false);
                        //string proc = "1";
                        Console.Write("ID_CUENTAS:" + i_cta + " | " + "RESULTADO: " + proc  + Environment.NewLine);
                    }
                    Console.Write("TOTAL PROCESADOS: ");
                    System.Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.Write("  "+ contador.ToString() + "  " + Environment.NewLine);
                    System.Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine("" + Environment.NewLine);
                    rdr.Close();
                    conexionBD.Close();
                    Console.Write("***** PROCESANDO CONTACTOS **********" + Environment.NewLine);
                    conexionBD.Open();
                    cmd = new SqlCommand();
                    querym = "SELECT * FROM [ws_salesforce].[dbo].[listar_contactos_q]";
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = querym;
                    cmd.Connection = conexionBD;
                    rdr = cmd.ExecuteReader();
                    contador = 0;
                    while (rdr.Read())
                    {
                        contador++;
                        string i_cont = rdr["ID_CONTACTO"].ToString();
                        string r_cont = rdr["RUT"].ToString();
                        string n_cont = rdr["NOMBRE"].ToString();
                        string a_cont = rdr["APELLIDO"].ToString();
                        string c_cont = rdr["CARGO"].ToString();
                        string f_cont = rdr["FONO"].ToString();
                        string m_cont = rdr["MAIL"].ToString();
                        string proc = crearContacto(i_cont, n_cont, m_cont, f_cont, false);
                        Console.Write("ID_CONTACTO:" + i_cont + " | " + "RESULTADO: " + proc + Environment.NewLine);
                    }
                    Console.Write("TOTAL PROCESADOS: ");
                    System.Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.Write("  " + contador.ToString() + "  " + Environment.NewLine);
                    System.Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine("" + Environment.NewLine);
                    rdr.Close();
                    conexionBD.Close();
                    Console.Write("***** PROCESANDO PRODUCTOS **********" + Environment.NewLine);
                    conexionBD.Open();
                    cmd = new SqlCommand();
                    querym = "SELECT * FROM [ws_salesforce].[dbo].[listar_productos_q]";
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = querym;
                    cmd.Connection = conexionBD;
                    rdr = cmd.ExecuteReader();
                    contador = 0;
                    while (rdr.Read())
                    {
                        contador++;
                        string i_prod = rdr["ID_PRODUCTO"].ToString();
                        string c_prod = rdr["CODIGO"].ToString();
                        string n_prod = rdr["NOMBRE"].ToString();
                        string c1_prod = rdr["CLASE1"].ToString();
                        string c2_prod = rdr["CLASE2"].ToString();
                        string c3_prod = rdr["CLASE3"].ToString();
                        string c4_prod = rdr["CLASE4"].ToString();
                        string m_prod = rdr["MONEVTA"].ToString();
                        double p_prod = string.IsNullOrEmpty(rdr["PRECVTA"].ToString())? 0 : Convert.ToDouble(rdr["PRECVTA"].ToString());
                        double co_prod = string.IsNullOrEmpty(rdr["COSTOREP"].ToString()) ? 0 : Convert.ToDouble(rdr["COSTOREP"].ToString());
                        double fa_prod = string.IsNullOrEmpty(rdr["FACEQUI"].ToString()) ? 0 : Convert.ToDouble(rdr["FACEQUI"].ToString());
                        double q_prod = string.IsNullOrEmpty(rdr["STOCK"].ToString()) ? 0 : Convert.ToDouble(rdr["STOCK"].ToString());

                        string proc = crearProductos(i_prod, n_prod, c_prod, q_prod, p_prod);
                        Console.Write("ID_PRODUCTO:" + i_prod + " | " + "RESULTADO: " + proc + Environment.NewLine);
                    }
                    Console.Write("TOTAL PROCESADOS: ");
                    System.Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.Write("  " + contador.ToString() + "  " + Environment.NewLine);
                    System.Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine("" + Environment.NewLine);
                    rdr.Close();
                    conexionBD.Close();
                    Console.Write("***** PROCESANDO NOTAS DE VENTA **********" + Environment.NewLine);
                    conexionBD.Open();
                    cmd = new SqlCommand();
                    querym = "SELECT * FROM [ws_salesforce].[dbo].[listar_notasventa_q]";
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = querym;
                    cmd.Connection = conexionBD;
                    rdr = cmd.ExecuteReader();
                    contador = 0;
                    while (rdr.Read())
                    {
                        contador++;
                        string i_cont = rdr["IdOportunidad"].ToString();
                        //int r_cont = string.IsNullOrEmpty(rdr["estado_sync"].ToString()) ? 0 : Convert.ToInt32(rdr["estado_sync"].ToString());
                        string r_cont = rdr["estado_sync"].ToString();
                        string n_cont = rdr["estado_desc"].ToString();
                        string proc = cambiaEstadoOport(i_cont, r_cont);
                        //string proc = actualizarNV(i_cont, r_cont, n_cont, false);
                        Console.Write("ID_CONTACTO:" + i_cont + " | " + "RESULTADO: " + proc + Environment.NewLine);
                    }
                    Console.Write("TOTAL PROCESADOS: ");
                    System.Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.Write("  " + contador.ToString() + "  " + Environment.NewLine);
                    System.Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine("" + Environment.NewLine);
                    rdr.Close();
                    //conexionBD.Close();

                    /*Console.Write("***** PROCESANDO NOTAS DE VENTA **********" + Environment.NewLine);
                    conexionBD.Open();
                    cmd = new SqlCommand();
                    querym = "SELECT * FROM [ws_salesforce].[dbo].[Productos]";
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = querym;
                    cmd.Connection = conexionBD;
                    rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        contador++;
                        i_prod = rdr["ID_PRODUCTO"].ToString();
                        n_prod = rdr["CR_NOMBRE"].ToString();
                        s_prod = rdr["CR_RAZON"].ToString();
                        r_prod = rdr["CR_RUT"].ToString();
                        string proc = crearCuenta(n_prod, s_prod, r_prod);
                        Console.Write("IDPROD:" + i_prod + " | " + "NOMBRE:" + n_prod + ", RESULTADO: " + proc + Environment.NewLine);
                    }
                    Console.Write("NOTAS DE VENTA PROCESADAS: " + contador.ToString() + Environment.NewLine);
                    rdr.Close();
                    conexionBD.Close();*/
                    System.Console.BackgroundColor = ConsoleColor.Black;
         //Console.WriteLine("\n" + enviarEmail("corellana@gildemeister.cl, mguzman@mets.cl, nespinoza@mets.cl, cpalomera@mets.cl, ccontreras@mets.cl", "fcontreras@mets.cl, pmoncada@mets.cl, sgodas@mets.cl", "", "AUTONOMOHYUNDAI - PROCESO DE ENVIO " + fechadt, "EL DIA " + fechadt + " SE LEYERON " + contador.ToString() + " REGISTRO(S) PARA SER PROCESADOS. EN RESUMEN: \r\n\r\n - SE ENVIARON " + totalinternos.ToString() + " MAILS INTERNOS.  \r\n - SE ENVIARON " + totalexternos.ToString() + " MAILS EXTERNOS. \r\n\r\n ERRORES \r\n\r\n - NO SE PUDIERON PROCESAR " + totalsinbuenformato.ToString() + " MAIL(S) POR ERROR DEL FORMATO DEL CORREO DESTINO. \r\n - NO SE PUDO ACTUALIZAR EL ID EN LA TABLA 'DEMONIO_COLA_CONTACTOSWEB' UN TOTAL DE " + totalerrorsql.ToString() + " REGISTROS. \r\n\r\n MAIL ENVIADO EN FORMA AUTOMATICA, FAVOR DE NO CONTESTAR. \r\n\r\n METS S.A"));
            //Console.ReadLine();
            System.Console.Write("***** PROCESANDO NUEVOS CONTACTOS ******" + Environment.NewLine);
            limpiarTabla("Todos_Contactos");
            listarContactos();
            System.Console.Write("***** PROCESANDO NUEVAS CUENTAS ******" + Environment.NewLine);
            limpiarTabla("Todos_Cuentas");
            listarCuentas();
            System.Console.Write("***** PROCESANDO NUEVOS PRODUCTOS ******" + Environment.NewLine);
            limpiarTabla("Todos_Productos");
            listarProductos();
            //System.Console.Write("***** PROCESANDO NOTAS DE VENTA ******" + Environment.NewLine);
            //listarNotasVenta();

                    //[Todos_NotasVenta

                    System.Console.Write("PROCESO TERMINADO" + Environment.NewLine);
            string fechadt = Convert.ToString(DateTime.Now.Day) + "-" + Convert.ToString(DateTime.Now.Month) + "-" + Convert.ToString(DateTime.Now.Year);

            conexionBD.Close();

                }
                else
                {
                    Console.Write("NO SE LOGRO INGRESAR A SALESFORCE" + Environment.NewLine);
                    string s_msg = "NO SE LOGRO INGRESAR A SALESFORCE, " + fecha() + Environment.NewLine;
                    errorDump(s_msg);
                    Console.WriteLine("" + Environment.NewLine);
                }


            }
            catch (Exception ex)
            {
                System.Console.BackgroundColor = ConsoleColor.Red;
                System.Console.ForegroundColor = ConsoleColor.White;
                Exception ex2 = ex;
                string errorMessage = string.Empty;
                while (ex2 != null)
                {
                    errorMessage += ex2.ToString();
                    ex2 = ex2.InnerException;
                }

                if (ex is SqlException)
                {
                    Console.Write("ERROR SQL: " + errorMessage);
                    string s_msg = "ERROR SQL, " + fecha() + " \n\r" + Environment.NewLine + errorMessage + Environment.NewLine;
                    errorDump(s_msg);
                }
                else if (ex is WebException)
                {
                    Console.Write("ERROR DESDE LA WEB: " + errorMessage);
                    string a_msg = "ERROR WEB, " + fecha() + " \n\r" + Environment.NewLine + errorMessage + Environment.NewLine;
                    errorDump(a_msg);
                }
                else if (ex is EndpointNotFoundException) {
                    Console.Write("ERROR EN PUNTO DE CONEXION: " + errorMessage);
                    string a_msg = "ERROR DESDE RESPUESTA SOAP SALESFORCE, " + fecha() + " \n\r" + Environment.NewLine + errorMessage + Environment.NewLine;
                    errorDump(a_msg);
                }
                else
                {
                    Console.Write("ERROR INTERNO: " + ex.GetType().ToString());
                    string a_msg = "ERROR INTERNO, " + fecha() + " \n\r" + Environment.NewLine + ex.GetType().ToString() + ", TIPO: " + errorMessage + Environment.NewLine;
                    errorDump(a_msg);
                    //Console.WriteLine(ex.GetType().FullName);
                    //Console.WriteLine(ex.GetBaseException().ToString());
                    //throw;
                }

                System.Console.BackgroundColor = ConsoleColor.Black;

            }

           
        }

        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private static void terminar(string tabla, string idx, string nuevo, string estado, string desc)
        {
            SqlDataReader rdr2 = null;
            //conexionBD.Open();
            try
            {
                string sqlinsert = "UPDATE "+tabla+ "S SET OUT_ESTADO = " + estado+ ", OUT_ID = '"+ nuevo + "', OUT_MSG = '" + desc+ "', OUT_FECHA = GETDATE() WHERE ID_" + tabla + " = " + idx;
                Console.WriteLine(sqlinsert);
               // Console.ReadLine();
                SqlCommand cmd = new SqlCommand(sqlinsert, conexionBD);
                rdr2 = cmd.ExecuteReader();
                rdr2.Close();
               // conexionBD.Close();
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
                errorDump("ERROR SQL TABLA: "+tabla+" ID:" + idx + Environment.NewLine + " DESCRIPCION: " + Environment.NewLine + errorMessage + ", FECHA: " + fecha() + Environment.NewLine);
                Console.Write("---> ERROR: " + errorMessage);
            }

        }
        private static void agregarProducto(string id, string nombre, double stock, string cod)
        {
            SqlDataReader rdr3 = null;
            try
            {
                string sqlinsert = "INSERT INTO [dbo].[Todos_Productos] ([IDSALESFORCE] ,[NPROD_NOMBRE] ,[NPROD_CODIGO] ,[NPROD_CLASE1] ,[NPROD_MONEVTA] ,[NPROD_PRECVTA]) VALUES ('" + id + "' ,'" + nombre.Replace("'", "`") + "' ,'"+ Convert.ToString(cod).Replace("'", "`") + "' ,'" + Convert.ToString(stock) + "' ,'' ,'')";
                Console.WriteLine(sqlinsert);
                SqlCommand cmd = new SqlCommand(sqlinsert, conexionBD);
                rdr3 = cmd.ExecuteReader();
                rdr3.Close();
            }
            catch (SqlException ex)
            {
                Exception ex2 = ex;
                string errorMessage = string.Empty;
                while (ex2 != null)
                {
                    errorMessage += ex2.ToString();
                    ex2 = ex2.InnerException;
                }
                if (ex.Number == 2601) {

                }
                else{
                    errorDump("ERROR SQL AL INTENTAR INSERTAR EN LA TABLA TODOS_PRODUCTOS ID:" + id + Environment.NewLine + " DESCRIPCION: " + Environment.NewLine + errorMessage + ", FECHA: " + fecha() + Environment.NewLine);
                    Console.Write("---> ERROR: " + errorMessage);
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
                errorDump("ERROR EXCPTI AL INTENTAR INSERTAR EN LA TABLA TODOS_PRODUCTOS ID:" + id + Environment.NewLine + " DESCRIPCION: " + Environment.NewLine + errorMessage + ", FECHA: " + fecha() + Environment.NewLine);
                Console.Write("---> ERROR: " + errorMessage);
                

            }

        }
        private static void limpiarTabla(string tabla)
        {
            SqlDataReader rdr4 = null;
            try
            {
                string sqlinsert = "DELETE FROM [dbo].["+ tabla + "]";
                Console.WriteLine(sqlinsert);
                SqlCommand cmd = new SqlCommand(sqlinsert, conexionBD);
                rdr4 = cmd.ExecuteReader();
                rdr4.Close();
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
                errorDump("ERROR SQL AL INTENTAR TRUNCAR :" + tabla + Environment.NewLine + " DESCRIPCION: " + Environment.NewLine + errorMessage + ", FECHA: " + fecha() + Environment.NewLine);
                Console.Write("---> ERROR: " + errorMessage);
            }
        }
        private static void agregarCuenta(string id, string fecham, bool matriz, string dire, string ciudad, string comuna, string nombre, string area, double apro, double uti, string razon, string rut, string giro, string tipo, string zona, string nac, string div, string fpago, double dpago, string dicom, double moro, bool impu, string desp_dire, string desp_ciudad, string desp_comuna)
        {
            // account.LastModifiedDate, esmatriz, account.BillingStreet, account.BillingCity, account.BillingState
            SqlDataReader rdr3 = null;
            string sqlinsert = "";
            try
            {
                sqlinsert = " INSERT INTO [dbo].[Todos_Cuentas] ([IDSALESFORCE] ,[NCUENT_NOMBRE] ,[NCUENT_AREA] ,[NCUENT_CAPROB] ,[NCUENT_CUTILIZ],[NCUENT_RAZON] ,[NCUENT_RUT] ,[NCUENT_GIRO] ,[NCUENT_TIPO] ,[NCUENT_ZONA] ,[NCUENT_NAC] ,[NCUENT_DIVISION] ,[NCUENT_FPAGO] ,[NCUENT_DPAGO] ,[NCUENT_DICOM] ,[NCUENT_MOROSIDAD] ,[NCUENT_IMPUTABLE],[NCUENT_ESMATRIZ],[NCUENT_DIRE],[NCUENT_COMUNA],[NCUENT_CIUDAD],[NCUENT_ULTIMAF],[NCUENT_DESP_DIRE],[NCUENT_DESP_CIUDAD],[NCUENT_DESP_COMUNA]) VALUES ('" + id + "' ,'" + nombre.Replace("'", "\"") + "' ,'" + area.Replace("'", "\"") + "' ,'" + Convert.ToString(apro) + "' ,'" + Convert.ToString(uti) + "','" + Convert.ToString(razon).Replace("'", "\"") + "','" + Convert.ToString(rut).Replace("'", "\"") + "','" + Convert.ToString(giro).Replace("'", "\"") + "','" + Convert.ToString(tipo).Replace("'", "\"") + "','" + Convert.ToString(zona).Replace("'", "\"") + "','" + Convert.ToString(nac).Replace("'", "\"") + "','" + Convert.ToString(div).Replace("'", "\"") + "','" + Convert.ToString(fpago).Replace("'", "\"") + "','" + Convert.ToString(dpago).Replace("'", "\"") + "','" + Convert.ToString(dicom).Replace("'", "\"") + "','" + Convert.ToString(moro).Replace("'", "\"") + "','" + Convert.ToString(impu) + "','" + matriz + "','" + dire.Replace("'", "\"") + "','" + comuna.Replace("'", "\"") + "','" + ciudad.Replace("'", "\"") + "','" + fecham + "','" + desp_dire.Replace("'", "\"") + "','" + desp_ciudad.Replace("'", "\"") + "','" + desp_comuna.Replace("'", "\"") + "')";
                Console.WriteLine(sqlinsert);
                SqlCommand cmd = new SqlCommand(sqlinsert, conexionBD);
                rdr3 = cmd.ExecuteReader();
                rdr3.Close();
            }
            catch (SqlException ex)
            {
                Exception ex2 = ex;
                string errorMessage = string.Empty;
                while (ex2 != null)
                {
                    errorMessage += ex2.ToString();
                    ex2 = ex2.InnerException;
                }
                if (ex.Number == 2601)
                {

                }
                else
                {
                    errorDump("ERROR SQL AL INTENTAR INSERTAR EN LA TABLA TODOS_cuentas ID:" + id + Environment.NewLine + " DESCRIPCION: " + Environment.NewLine + errorMessage + ", FECHA: " + fecha() + Environment.NewLine + sqlinsert + Environment.NewLine);
                    Console.Write("---> ERROR: " + errorMessage);
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
                errorDump("ERROR EXCEPCION AL INTENTAR INSERTAR EN LA TABLA TODOS_cuentas ID:" + id + Environment.NewLine + " DESCRIPCION: " + Environment.NewLine + errorMessage + ", FECHA: " + fecha() + Environment.NewLine);
                Console.Write("---> ERROR: " + errorMessage);


            }
           // Console.ReadLine();
        }
        private static void agregarContacto(string id, string acid, string fecham, string nombre, string first, string last, string mail, string fono, string cargo)
        {
            SqlDataReader rdr3 = null;
            try
            {
                string sqlinsert = "INSERT INTO [dbo].[Todos_Contactos] ([IDSALESFORCE],[IDCUENTA] ,[NCONT_CUENTA] ,[NCONT_EMAIL], [NCONT_NOMBRE], [NCONT_APELLIDO], [NCONT_FONO], [NCONT_CARGO], [NCONT_ULTIMAF]) VALUES ('" + id+ "' ,'" + acid + "' ,'" + nombre+ "' ,'" + mail + "','" + first + "','" + last + "','" + fono + "','" + cargo + "','" + fecham + "')";
                Console.WriteLine(sqlinsert);
                SqlCommand cmd = new SqlCommand(sqlinsert, conexionBD);
                rdr3 = cmd.ExecuteReader();
                rdr3.Close();
            }
            catch (SqlException ex)
            {
                Exception ex2 = ex;
                string errorMessage = string.Empty;
                while (ex2 != null)
                {
                    errorMessage += ex2.ToString();
                    ex2 = ex2.InnerException;
                }
                if (ex.Number == 2601)
                {

                }
                else
                {
                    errorDump("ERROR SQL AL INTENTAR INSERTAR EN LA TABLA TODOS_contactos ID:" + id + Environment.NewLine + " DESCRIPCION: " + Environment.NewLine + errorMessage + ", FECHA: " + fecha() + Environment.NewLine);
                    Console.Write("---> ERROR: " + errorMessage);
                }
                //Console.ReadKey();
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
                errorDump("ERROR EXCPTION AL INTENTAR INSERTAR EN LA TABLA TODOS_contacto ID:" + id + Environment.NewLine + " DESCRIPCION: " + Environment.NewLine + errorMessage + ", FECHA: " + fecha() + Environment.NewLine);
                Console.Write("---> ERROR: " + errorMessage);


            }

        }
        public static string fecha()
        {
            return Convert.ToString(DateTime.Now.Day) + "/" + Convert.ToString(DateTime.Now.Month) + "/" + Convert.ToString(DateTime.Now.Year) + " " + DateTime.Now.ToShortTimeString();
        }
        public static void createDump(string logdata)
        {
            string strLogText = logdata;
            StreamWriter log;
            string carpeta = Convert.ToString(DateTime.Now.Day) + Convert.ToString(DateTime.Now.Month) + Convert.ToString(DateTime.Now.Year);
            string archivo = "log_" + carpeta + ".txt";
            if (!File.Exists(archivo))
            {
                log = new StreamWriter(archivo);
            }
            else
            {
                log = File.AppendText(archivo);
            }
            log.WriteLine(strLogText);
            log.WriteLine();
            log.WriteLine();
            log.Close();
        }
        public static void errorDump(string logdata)
        {
            string strLogText = logdata;
            StreamWriter log;
            string carpeta = Convert.ToString(DateTime.Now.Day) + Convert.ToString(DateTime.Now.Month) + Convert.ToString(DateTime.Now.Year);
            string archivo = "error_" + carpeta + ".txt";
            if (!File.Exists(archivo))
            {
                log = new StreamWriter(archivo);
            }
            else
            {
                log = File.AppendText(archivo);
            }
            log.WriteLine(strLogText);
            log.WriteLine();
            log.WriteLine();
            log.Close();
        }
        public static bool Login()
        {
            try {

                string username = "consultor_force@nectia.inexsbx.com";
                string password = "nectia2020lT2HDj7BFGeAHkmSLqm2cRMu";

                loginClient = new SoapClient();

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                LoginResult lr = loginClient.login(null, username, password);

                urlServer = lr.serverUrl.ToString();
                idUsuario = lr.userId.ToString();
                idSession = lr.sessionId.ToString();

                endpoint = new EndpointAddress(lr.serverUrl);

                sessionHeader = new SessionHeader
                {
                    sessionId = lr.sessionId
                };

                client = new SoapClient("Soap", endpoint);

                return true;

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
                errorDump("ERROR SQL AL INTENTAR INSERTAR LOGEARSE A SALESFORCE:" + Environment.NewLine + " DESCRIPCION: " + Environment.NewLine + errorMessage + ", FECHA: " + fecha() + Environment.NewLine);
                Console.Write("---> ERROR: " + errorMessage);
                return false;


            }
        }

        private static void Logout()
        {
            client.logout(sessionHeader);
        }

        private static string actualizarNV(string id, int n, string c, bool actualiza)
        {

           /* Contact acct = new Contact();
            acct.Name = n; //nombre de la cuenta
            acct.Email = c; //correo
            acct.Phone = t; //teléfono

            LimitInfo[] limite = null;
            SaveResult[] createResults = null;

            if (actualiza)
            {

            }
            else
            {
                client.create(
                        sessionHeader, //sessionheader
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        new sObject[] { acct }, //objeto
                        out limite,
                        out createResults
                        );

            }


            if (createResults[0].success)
            {
                string idm = createResults[0].id;
                terminar("CONTACTO", id, idm, "1", "Correcto");
                return "1|" + id.ToString();
            }
            else
            {
                string result = createResults[0].errors[0].message;
                terminar("CONTACTO", id, "", "2", result);
                return "-1|" + result;
            }
            */
            return "1|" + id.ToString();
        }

        private static string crearContacto(string id, string n, string c, string t, bool actualiza)
        {

            Contact acct = new Contact();
            acct.Name = n; //nombre de la cuenta
            acct.Email = c; //correo
            acct.Phone = t; //teléfono

            LimitInfo[] limite = null;
            SaveResult[] createResults = null;

            if (actualiza) {
                
            }
            else
            {
                client.create(
                        sessionHeader, //sessionheader
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        new sObject[] { acct }, //objeto
                        out limite,
                        out createResults
                        );

            }
            

            if (createResults[0].success)
            {
                string idm = createResults[0].id;
                terminar("CONTACTO", id, idm, "1", "Correcto");
                return "1|" + id.ToString();
            }
            else
            {
                string result = createResults[0].errors[0].message;
                terminar("CONTACTO", id, "", "2", result);
                return "-1|" + result;
            }

        }

        private static string crearProductos(string id, string n, string c, double s, double v)
        {

            Product2 acct = new Product2();
            acct.Name = n;
            acct.ProductCode = c;
            acct.IsActive = true;
            acct.Stock__c = s;
            acct.Costo__c = v;
            acct.CurrencyIsoCode = "CLP";

            LimitInfo[] limite = null;
            SaveResult[] createResults = null;

            client.create(
            sessionHeader, //sessionheader
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            new sObject[] { acct }, //objeto
            out limite,
            out createResults
            );

            if (createResults[0].success)
            {
                string idm = createResults[0].id;
                terminar("PRODUCTO", id, idm, "1", "Correcto");
                return "1|" + id.ToString();
            }
            else
            {
                string result = createResults[0].errors[0].message;
                terminar("PRODUCTO", id, "", "2", result);
                return "-1|" + result;
            }

        }

        private static string crearCuenta(string id, string n, string s, string r, bool actualiza) {

            //string nombre = RandomString(8);
            Account acct = new Account();
            acct.Name = n;
            acct.Razon_Social__c = s;
            acct.Rut__c = r;
            acct.CurrencyIsoCode = "CLP";

            LimitInfo[] limite = null;
            SaveResult[] createResults = null;
            //UpsertResult[] upserResults = null;

            if (actualiza) {
                
            }
            else
            {
                client.create(
                sessionHeader, //sessionheader
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                new sObject[] { acct }, //objeto
                out limite,
                out createResults
                );

            }


            if (createResults[0].success)
            {
                string idm = createResults[0].id;
                terminar("CUENTA", id, idm, "1", "Correcto");
                return "1|"+id.ToString();
            }
            else
            {
                string result = createResults[0].errors[0].message;
                terminar("CUENTA", id, "", "2", result);
                return "-1|" + result;
            }

        }

        private static void listarContactos() {

            try
            {


                string query = "";
                query = "SELECT Id, AccountId, Name,FirstName,LastName,Email,Title,Phone,LastModifiedDate FROM Contact";
                sForce.QueryResult queryRes = null;

                client.query(
                sessionHeader, //sessionheader
                null, //queryoptions
                null, //mruheader
                null, //packageversionheader
                query, //SOQL query
                out queryRes
                );

                Console.Write("************* CONTACTOS **********************" + Environment.NewLine);

                if (queryRes.records != null)
                {
                    foreach (var record in queryRes.records)
                    {
                        var contact = (sForce.Contact)record;
                        Console.WriteLine(string.Format("Contact Id: {0}", contact.Id));
                        Console.WriteLine(string.Format("Contact Name: {0}", contact.Name));
                        string fecha = contact.LastModifiedDate.Value.ToString("yyyy-MM-dd HH:mm:ss") ?? "";
                        agregarContacto(contact.Id, contact.AccountId, fecha, contact.Name, contact.FirstName, contact.LastName, contact.Email, contact.Phone, contact.Title);
                    }
                }

            }
            catch (Exception ex)
            {
                System.Console.BackgroundColor = ConsoleColor.Red;
                System.Console.ForegroundColor = ConsoleColor.White;
                Exception ex2 = ex;
                string errorMessage = string.Empty;
                while (ex2 != null)
                {
                    errorMessage += ex2.ToString();
                    ex2 = ex2.InnerException;
                }


                Console.Write("ERROR SALESFORCE CONTACTOS: " + ex.GetType().ToString());
                string a_msg = "ERROR SALESFORCE CONTACTOS, " + fecha() + " \n\r" + Environment.NewLine + ex.GetType().ToString() + ", TIPO: " + errorMessage + Environment.NewLine;
                errorDump(a_msg);
                Console.WriteLine(ex.GetBaseException().ToString());


                System.Console.BackgroundColor = ConsoleColor.Black;

            }

        }

        private static void listarCuentas()
        {

            try
            {

                string query = "";
                query = "SELECT Id, Es_matriz__c, ShippingStreet, ShippingCity, ShippingState, BillingStreet, BillingCity, BillingState, LastModifiedDate, Name, Razon_Social__c,Rut__c,Giro__c,Tipo__c,Zona__c,Nac_o_inter__c,Division__c,Area_2__c,Forma_de_pago__c,Dias_de_pago__c,Dicom__c,Credito_aprobado__c,Morosidad__c,Credito_utilizado__c,Imputable__c FROM Account";
                sForce.QueryResult queryRes = null;

                client.query(
                sessionHeader, //sessionheader
                null, //queryoptions
                null, //mruheader
                null, //packageversionheader
                query, //SOQL query
                out queryRes
                );

                Console.Write("************* CUENTAS **********************" + Environment.NewLine);

                if (queryRes.records != null)
                {
                    foreach (var record in queryRes.records)
                    {
                        var account = (sForce.Account)record;

                        Console.WriteLine(string.Format("Account Id: {0}", account.Id));
                        Console.WriteLine(string.Format("Account Name: {0}", account.Name));
                        //Console.WriteLine(string.Format("Account Area: {0}", account.Area_2__c));
                        //Console.WriteLine(string.Format("Account Caprobado: {0}", account.Credito_aprobado__c));
                        //Console.WriteLine(string.Format("Account Cutilizado: {0}", account.Credito_utilizado__c));
                        double c_apro = 0;
                        if (account.Credito_aprobado__c.HasValue)
                        {
                            c_apro = (double)account.Credito_aprobado__c;
                        }

                        double c_uti = 0;
                        if (account.Credito_utilizado__c.HasValue)
                        {
                            c_uti = (double)account.Credito_utilizado__c;
                        }

                        double dpago = account.Dias_de_pago__c ?? 0;
                        double moro = account.Morosidad__c ?? 0;
                        bool imputa = account.Imputable__c ?? false;
                        bool esmatriz = account.Es_matriz__c ?? false;
                        string fecha = account.LastModifiedDate.Value.ToString("yyyy-MM-dd HH:mm:ss") ?? "";

                        agregarCuenta(account.Id, fecha, esmatriz, account.BillingStreet, account.BillingCity, account.BillingState, account.Name, account.Area_2__c, c_apro, c_uti, account.Razon_Social__c, account.Rut__c, account.Giro__c, account.Tipo__c, account.Zona__c, account.Nac_o_inter__c, account.Division__c, account.Forma_de_pago__c, dpago , account.Dicom__c, moro, imputa, account.ShippingStreet, account.ShippingCity, account.ShippingState);
                    }
                }

            }
            catch (Exception ex)
            {
                System.Console.BackgroundColor = ConsoleColor.Red;
                System.Console.ForegroundColor = ConsoleColor.White;
                Exception ex2 = ex;
                string errorMessage = string.Empty;
                while (ex2 != null)
                {
                    errorMessage += ex2.ToString();
                    ex2 = ex2.InnerException;
                }

                
                Console.Write("ERROR SALESFORCE CUENTAS: " + ex.GetType().ToString());
                string a_msg = "ERROR SALESFORCE CUENTAS, " + fecha() + " \n\r" + Environment.NewLine + ex.GetType().ToString() + ", TIPO: " + errorMessage + Environment.NewLine;
                errorDump(a_msg);
                Console.WriteLine(ex.GetBaseException().ToString());
             

                System.Console.BackgroundColor = ConsoleColor.Black;

            }

            //Console.ReadLine();

        }

        //            listarNotasVenta();

        //[Todos_NotasVenta
        private static void terminarNV(string IdOpt, string estado)
        {
            SqlDataReader rdr2 = null;
            //conexionBD.Open();
            try
            {
                string sqlinsert = "UPDATE Todos_NotasVenta SET estado_sync = "+ estado + " FROM Todos_NotasVenta INNER JOIN Todos_NV_Productos ON Todos_NotasVenta.IdCuenta = Todos_NV_Productos.IdCuenta WHERE(Todos_NV_Productos.IdOportunidad = '"+ IdOpt + "') ";
                Console.WriteLine(sqlinsert);
                SqlCommand cmd = new SqlCommand(sqlinsert, conexionBD);
                rdr2 = cmd.ExecuteReader();
                rdr2.Close();
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
                errorDump("ERROR SQL TABLA: NOTAS DE VENTA ID:" + IdOpt + Environment.NewLine + " DESCRIPCION: " + Environment.NewLine + errorMessage + ", FECHA: " + fecha() + Environment.NewLine);
                Console.Write("---> ERROR: " + errorMessage);
            }

        }

        private static string cambiaEstadoOport(String oppId, String Estado) {
            try
            {
                string mensj = "";
                string cod_t = "";
                if (Estado == "4")
                {
                    mensj = "Ups! Algo salió mal, debes hacer la Nota de Venta nuevamente.";
                    cod_t = "6";
                }
                if (Estado == "5")
                {
                    mensj = "Felicitaciones, tu Nota de venta ha sido facturada y coordinaremos la entrega de los productos.";
                    cod_t = "7";
                }
                Opportunity acOport = new Opportunity();
                acOport.Id = oppId;
                acOport.StageName = Estado;
                acOport.Estado_de_Errores__c = mensj;

                //SaveResult[] results = client.update(new sObject[] { acOport });

                LimitInfo[] limite = null;
                SaveResult[] updResults = null;

                client.update(
                sessionHeader, //sessionheader
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                new sObject[] { acOport },
                out limite,
                out updResults
                );
                terminarNV(oppId, cod_t);
                return "1|" + oppId;
            }
            catch (Exception e)
            {
                string error_m = "Error Actualizando la Nota de Venta: " + e.Message + "\n" + e.StackTrace;
                Console.WriteLine(error_m);
                errorDump(error_m);
                return "-1|" + e.Message;
            
            }

            }

            
        private static void listarProductos()
        {
            try
            {

                string query = "";
                query = "SELECT Id, Name, Stock__c, ProductCode FROM Product2";
                sForce.QueryResult queryRes = null;

                client.query(
                sessionHeader, //sessionheader
                null, //queryoptions
                null, //mruheader
                null, //packageversionheader
                query, //SOQL query
                out queryRes
                );

                Console.Write("************* PRODUCTOS **********************" + Environment.NewLine);

                if (queryRes.records != null)
                {
                    foreach (var record in queryRes.records)
                    {
                        var product = (sForce.Product2)record;

                        Console.WriteLine(string.Format("Producto Id: {0}", product.Id));
                        Console.WriteLine(string.Format("Producto Name: {0}", product.Name));
                        Console.WriteLine(string.Format("Producto Stock: {0}", product.Stock__c));
                        double st = product.Stock__c ?? 0;
                        agregarProducto(product.Id, product.Name, st, product.ProductCode);

                    }
                }

            }
            catch (Exception ex)
            {
                System.Console.BackgroundColor = ConsoleColor.Red;
                System.Console.ForegroundColor = ConsoleColor.White;
                Exception ex2 = ex;
                string errorMessage = string.Empty;
                while (ex2 != null)
                {
                    errorMessage += ex2.ToString();
                    ex2 = ex2.InnerException;
                }


                Console.Write("ERROR SALESFORCE PRODUCTOS: " + ex.GetType().ToString());
                string a_msg = "ERROR SALESFORCE PRODUCTOS, " + fecha() + " \n\r" + Environment.NewLine + ex.GetType().ToString() + ", TIPO: " + errorMessage + Environment.NewLine;
                errorDump(a_msg);
                Console.WriteLine(ex.GetBaseException().ToString());


                System.Console.BackgroundColor = ConsoleColor.Black;

            }

        }


    }
}
