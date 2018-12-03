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
                    string querym = "";
                    /*
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
                        string s_cta2 = string.IsNullOrEmpty(rdr["RAZSOC2"].ToString()) ? "" : rdr["RAZSOC2"].ToString();
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
                        bool prin_cta = string.IsNullOrEmpty(rdr["PRINCIPAL"].ToString()) ? false : true;
                        string dir2_cta = rdr["DIR2"].ToString();
                        string comu2_cta = rdr["COMUNA2"].ToString();
                        string ciu2_cta = rdr["CIUDAD2"].ToString();
                        string valor_e = rdr["OUT_ESTADO"].ToString();
                        bool actual_ = false;
                        if (valor_e == "0") {
                            actual_ = false;
                        }
                        if (valor_e == "4")
                        {
                            actual_ = true;
                        }

                        Console.Write("ESTADO"+ valor_e + " | ID_CUENTAS:" + i_cta + " | " + "RAZON: " + s_cta + " | " + "RUT: " + r_cta + " | " + "FONO: " + f_cta + " | " + "MOROSIDAD: " + n_cta.ToString() + "MOROSIDAD (SQL) " + rdr["MOROSID"].ToString()  + " | " + "DIAS DE PAGO: " + dias_cta.ToString() + " | " + "CREDITO: " + cdu_cta.ToString() + " | " + "ES PRINCIPAL: " + prin_cta.ToString() + Environment.NewLine);
                        // Console.ReadLine();
                        string proc = crearCuenta(prin_cta, dir2_cta, comu2_cta, ciu2_cta, i_cta, s_cta, s_cta2, r_cta, f_cta, d_cta, g_cta, co_cta, ci_cta, for_cta, dias_cta, cd_cta, c1_cta, c2_cta, c3_cta, c4_cta, cdu_cta, n_cta, actual_);
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
                        string nnom = n_cont + " " + a_cont;
                        string c_cont = rdr["CARGO"].ToString();
                        string f_cont = rdr["FONO"].ToString();
                        string m_cont = rdr["MAIL"].ToString();
                        string ra_cont = rdr["RAZSOC"].ToString();
                        string valor_e = rdr["OUT_ESTADO"].ToString();
                        string id_cuen = rdr["IDCUENTA"].ToString();
                        bool actual_ = false;
                        if (valor_e == "0")
                        {
                            actual_ = false;
                        }
                        if (valor_e == "4")
                        {
                            actual_ = true;
                        }
                        Console.Write("ESTADO" + valor_e + " | ID_CONTACTO:" + i_cont + Environment.NewLine);
                        string proc = crearContacto(i_cont, nnom, m_cont, c_cont, f_cont, id_cuen, ra_cont, actual_);
                        Console.Write("ID_CONTACTO:" + i_cont + " | " + "RESULTADO: " + proc + Environment.NewLine);
                    }
                    Console.Write("TOTAL PROCESADOS: ");
                    System.Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.Write("  " + contador.ToString() + "  " + Environment.NewLine);
                    System.Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine("" + Environment.NewLine);
                    rdr.Close();
                    conexionBD.Close();*/
                    Console.Write("***** PROCESANDO PRODUCTOS **********" + Environment.NewLine);
                    //conexionBD.Open();
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
                        // string idprox = string.IsNullOrEmpty(rdr["IDPROD"].ToString()) ? "": rdr["IDPROD"].ToString();
                        string idprox = "";
                        string valor_e = rdr["OUT_ESTADO"].ToString();
                        bool actualpod_ = false;
                        if (valor_e == "0")
                        {
                            actualpod_ = false;
                        }
                        if (valor_e == "4")
                        {
                            actualpod_ = true;
                        }
                        string proc = crearProductos(i_prod, n_prod, c_prod, q_prod, p_prod, idprox, actualpod_);
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

                    System.Console.BackgroundColor = ConsoleColor.Black;

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

        //i_cta



        private static void terminar(string tabla, string idx, string nuevo, string estado, string desc)
        {
            SqlDataReader rdr2 = null;
            //conexionBD.Open();
            desc = desc.Replace("\"", string.Empty).Replace("'", string.Empty);
            string sqlinsert = "UPDATE " + tabla + "S SET OUT_ESTADO = " + estado + ", OUT_ID = '" + nuevo + "', OUT_MSG = '" + desc + "', OUT_FECHA = GETDATE() WHERE ID_" + tabla + " = " + idx;
            try
            {
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
                errorDump("ERROR SQL TABLA: " + tabla + " ID:" + idx + Environment.NewLine + " DESCRIPCION: " + Environment.NewLine + errorMessage + ", FECHA: " + fecha() + Environment.NewLine + "QUERY: " + sqlinsert + Environment.NewLine );
               // Console.ReadKey();
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

                //string username = "consultor_force@nectia.inexsbx.com";
                //string password = "nectia2020lT2HDj7BFGeAHkmSLqm2cRMu";
                string username = "consultor_force@nectia.com.inex";
                string password = "Nectia2019K2MU6nPNrsMAbzIQ0otbi93N";

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

        private static string crearContacto(string id, string n, string m, string c, string t, string ida, string r, bool actualiza)
        {

            Contact acct = new Contact();
            acct.Name = n; //nombre de la cuenta
            acct.Email = m; //correo
            acct.Title = c; //cargo
            acct.Phone = t; //teléfono
            acct.AccountId = ida;

            LimitInfo[] limite = null;
            SaveResult[] createResults = null;
            UpsertResult[] upserResults = null;

            if (actualiza) {
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
                        new sObject[] { acct }, //objeto
                        out limite,
                        out createResults
                        );
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

        private static string crearProductos(string id, string n, string c, double s, double v, string ida, bool actual)
        {

            Product2 acct = new Product2();
            acct.Name = n;
           // acct.Id = ida;
            acct.ProductCode = c;
            acct.IsActive = true;
            acct.Stock__c = s;
            acct.Costo__c = v;
            acct.CurrencyIsoCode = "CLP";
            acct.Codigo_de_Producto__c = c;

            LimitInfo[] limite = null;
            SaveResult[] createResults = null;
            UpsertResult[] upserResults = null;

            client.upsert(
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
             "Codigo_de_Producto__c",
             new sObject[] { acct }, //objeto
             out limite,
             out upserResults
             );
            /*
            if (actual)
            {
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
                        new sObject[] { acct }, //objeto
                        out limite,
                        out createResults
                        );
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

            }*/


            if (upserResults[0].success)
            {
                string idm = upserResults[0].id;
                terminar("PRODUCTO", id, idm, "1", "Correcto");
                return "1|" + id.ToString();
            }
            else
            {
                string result = upserResults[0].errors[0].message;
                terminar("PRODUCTO", id, "", "2", result);
                return "-1|" + result;
            }

        }

        private static string crearCuenta(bool prin_cta, string dir2_cta, string comu2_cta, string ciu2_cta, string i_cta, string s_cta, string s_cta2, string r_cta, string f_cta, string d_cta, string g_cta, string co_cta, string ci_cta, string for_cta, int dias_cta, double cd_cta, string c1_cta, string c2_cta, string c3_cta, string c4_cta, double cdu_cta, int n_cta, bool actualiza) {

            //string nombre = RandomString(8);
            //f_cta = fono
            //s_cta = razon social
            Account acct = new Account();
            acct.Name = s_cta2;
          //  acct.Razon_Social__c = s_cta;
            acct.Rut__c = r_cta;
            acct.BillingStreet = d_cta;
            acct.Giro__c = g_cta;
            acct.BillingState = co_cta;
            acct.BillingCity = ci_cta;
            acct.Forma_de_pago__c = for_cta;
            acct.Dias_de_pago__c = dias_cta;
            acct.Phone = f_cta;
            acct.Credito_aprobado__c = cd_cta;
            acct.Division__c = c1_cta;
            acct.Area_2__c = c2_cta;
            acct.Nac_o_inter__c = c3_cta;
            acct.Dicom__c = c4_cta;
            acct.Credito_utilizado__c = cdu_cta;
            acct.Morosidad__c = n_cta;
           // acct.Es_matriz__c = prin_cta;
            acct.ShippingStreet = dir2_cta;
            acct.ShippingState = comu2_cta;
            acct.ShippingCity = ciu2_cta;
            acct.CurrencyIsoCode = "CLP";


            LimitInfo[] limite = null;
            SaveResult[] createResults = null;
            UpsertResult[] upserResults = null;

            if (actualiza) {

                client.upsert(
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
                "Rut__c",
                new sObject[] { acct }, //objeto
                out limite,
                out upserResults
               );
                
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

            if (actualiza)
            {
                if (upserResults[0].success)
                {
                    string idm = upserResults[0].id;
                    terminar("CUENTA", i_cta, idm, "1", "Correcto");
                    return "1|" + i_cta.ToString();
                }
                else
                {
                    string result = upserResults[0].errors[0].message;
                    terminar("CUENTA", i_cta, "", "2", result);
                    return "-1|" + result;
                }
            }
            else
            {
                if (createResults[0].success)
                {
                    string idm = createResults[0].id;
                    terminar("CUENTA", i_cta, idm, "1", "Correcto");
                    return "1|" + i_cta.ToString();
                }
                else
                {
                    string result = createResults[0].errors[0].message;
                    terminar("CUENTA", i_cta, "", "2", result);
                    return "-1|" + result;
                }
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
                        // SE DEBE SOLUCIONAR
                        //bool esmatriz = account.Es_matriz__c ?? false;
                        bool esmatriz = false;
                        string fecha = account.LastModifiedDate.Value.ToString("yyyy-MM-dd HH:mm:ss") ?? "";
                        //ESTO ESTA MALO
                        //string r_sociax = account.Razon_Social__c;
                        string r_sociax = "";

                        agregarCuenta(account.Id, fecha, esmatriz, account.BillingStreet, account.BillingCity, account.BillingState, account.Name, account.Area_2__c, c_apro, c_uti, r_sociax, account.Rut__c, account.Giro__c, account.Tipo__c, account.Zona__c, account.Nac_o_inter__c, account.Division__c, account.Forma_de_pago__c, dpago , account.Dicom__c, moro, imputa, account.ShippingStreet, account.ShippingCity, account.ShippingState);
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
                string nuevo_estado = "";
                if (Estado == "4")
                {
                    mensj = "Felicitaciones, tu Nota de venta ha sido facturada y coordinaremos la entrega de los productos.";
                    cod_t = "6";
                    nuevo_estado = "Cerrada ganada";
                }
                if (Estado == "5")
                {
                    mensj = "Ups! Algo salió mal, debes hacer la Nota de Venta nuevamente.";
                    cod_t = "7";
                    nuevo_estado = "Cerrada perdida";
                }
                Opportunity acOport = new Opportunity();
                acOport.Id = oppId;
                acOport.StageName = nuevo_estado;
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
