using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Appv3.sForce;
using System.ServiceModel;
using System.Net;
using System.Xml;
using System.Data.SqlClient;

namespace Appv3
{
    public partial class Salesforce : ServiceBase
    {
        System.Timers.Timer timeDelay;
        int count;
        public static SoapClient client;
        private static SoapClient loginClient;
        private static SessionHeader sessionHeader;
        private static EndpointAddress endpoint;
        private static string urlServer;
        private static string idUsuario;
        private static string idSession;
        private static SqlConnection conexionBD;
        public static string rutalog = @"C:\Salesforce\Log\";     

        public Salesforce()
        {
            InitializeComponent();
            timeDelay = new Timer(5 * 60 * 1000);
            timeDelay.Elapsed += new System.Timers.ElapsedEventHandler(WorkProcess);
            timeDelay.Start();
        }

        public void WorkProcess(object sender, System.Timers.ElapsedEventArgs e)
        {
            string process = "NUEVO PROCESO " + count + ", FECHA: " + fecha();
            createDump(process);
            FingerPrint maquina = new FingerPrint();
            string makina = maquina.unico;
            if (makina != "D0E7-3B3F-242B-2723-4D44-98B2-0A42-4A51")
            {
                string s_msg = "ERROR, EXISTEN CAMBIOS FISICOS O DE S.O EN LA MAQUINA PRINCIPAL, QUE IMPIDEN PROCESAR LA INTEGRACION. COD:" + makina + Environment.NewLine;
                createDump(s_msg);
                Environment.Exit(1);
            }

            Console.WriteLine("" + Environment.NewLine);

            SqlDataReader rdr = null;
            string mistringconx = "Data Source=localhost\\SQLEXPRESS;MultipleActiveResultSets=true;Initial Catalog=ws_salesforce;Persist Security Info=True;User ID=sa;Password=Ws%inex18";
            conexionBD = new SqlConnection(mistringconx);
            int contador = 0;

            try
            {
                createDump("***** CONECTANDOSE A SALESFORCE **********");
                if (Login())
                {
                    createDump("USUARIO: " + idUsuario);
                    createDump("URL: " + urlServer);
                    createDump("SESSION: " + idSession);

                    createDump("***** CONECTANDOSE A LA BASE DE DATOS **********");
                    conexionBD.Open();
                    createDump(" [OK] ");

                    SqlCommand cmd = new SqlCommand();
                    string querym = "SELECT * FROM [ws_salesforce].[dbo].[listar_cuentas_q]";
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = querym;
                    cmd.Connection = conexionBD;
                    rdr = cmd.ExecuteReader();
                    contador = 0;

                    createDump("***** PROCESANDO CUENTAS **********" );


                    while (rdr.Read())
                    {
                        contador++;
                        string i_cta = rdr["ID_CUENTA"].ToString();
                        string r_cta = rdr["RUT"].ToString();
                        string s_cta = rdr["RAZSOC"].ToString();
                        string d_cta = rdr["DIR"].ToString();
                        string g_cta = rdr["GIRO"].ToString();
                        string co_cta = rdr["COMUNA"].ToString();
                        string ci_cta = rdr["CIUDAD"].ToString();
                        string for_cta = rdr["FORMAP"].ToString();
                        int dias_cta = string.IsNullOrEmpty(rdr["DIASPAGO"].ToString()) ? 0 : Convert.ToInt32(rdr["DIASPAGO"].ToString());
                        double cd_cta = string.IsNullOrEmpty(rdr["CRED_APR"].ToString()) ? 0 : Convert.ToDouble(rdr["CRED_APR"].ToString());
                        string f_cta = rdr["FONO"].ToString();
                        string c1_cta = rdr["CLASE1"].ToString();
                        string c2_cta = rdr["CLASE2"].ToString();
                        string c3_cta = rdr["CLASE3"].ToString();
                        string c4_cta = rdr["CLASE4"].ToString();
                        double cdu_cta = string.IsNullOrEmpty(rdr["CRED_UTI"].ToString()) ? 0 : Convert.ToDouble(rdr["CRED_UTI"].ToString());
                        int n_cta = string.IsNullOrEmpty(rdr["MOROSID"].ToString()) ? 0 : Convert.ToInt32(rdr["MOROSID"].ToString());
                        string proc = crearCuenta(i_cta, s_cta, r_cta, f_cta);
                        createDump("ID_CUENTAS:" + i_cta + " | " + "RESULTADO: " + proc );
                    }
                    createDump("TOTAL PROCESADOS: " + contador.ToString());
                    rdr.Close();
                    conexionBD.Close();
                    createDump("***** PROCESANDO CONTACTOS **********" );
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
                        string proc = crearContacto(i_cont, n_cont, m_cont, f_cont);
                        createDump("ID_CONTACTO:" + i_cont + " | " + "RESULTADO: " + proc );
                    }
                    createDump("TOTAL PROCESADOS: " + contador.ToString());
                    rdr.Close();
                    conexionBD.Close();
                    createDump("***** PROCESANDO PRODUCTOS **********");
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
                        double p_prod = string.IsNullOrEmpty(rdr["PRECVTA"].ToString()) ? 0 : Convert.ToDouble(rdr["PRECVTA"].ToString());
                        double co_prod = string.IsNullOrEmpty(rdr["COSTOREP"].ToString()) ? 0 : Convert.ToDouble(rdr["COSTOREP"].ToString());
                        double fa_prod = string.IsNullOrEmpty(rdr["FACEQUI"].ToString()) ? 0 : Convert.ToDouble(rdr["FACEQUI"].ToString());
                        double q_prod = string.IsNullOrEmpty(rdr["STOCK"].ToString()) ? 0 : Convert.ToDouble(rdr["STOCK"].ToString());

                        string proc = crearProductos(i_prod, n_prod, c_prod, q_prod, p_prod);
                        createDump("ID_PRODUCTO:" + i_prod + " | " + "RESULTADO: " + proc);
                    }
                    createDump("TOTAL PROCESADOS: " + contador.ToString());
                    rdr.Close();
                    conexionBD.Close();
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

                }
                else
                {
                    string s_msg = "NO SE LOGRO INGRESAR A SALESFORCE, " + fecha();
                    createDump(s_msg);
                    Console.WriteLine("" + Environment.NewLine);
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

                if (ex is SqlException)
                {
                    string s_msg = "ERROR SQL, " + fecha() + " \n\r" + Environment.NewLine + errorMessage;
                    createDump(s_msg);
                }
                else if (ex is WebException)
                {
                    string a_msg = "ERROR WEB, " + fecha() + " \n\r" + Environment.NewLine + errorMessage;
                    createDump(a_msg);
                }
                else if (ex is EndpointNotFoundException)
                {
                    string a_msg = "ERROR DESDE RESPUESTA SOAP SALESFORCE, " + fecha() + " \n\r" + Environment.NewLine + errorMessage;
                    createDump(a_msg);
                }
                else
                {
                    string a_msg = "ERROR INTERNO, " + fecha() + " \n\r" + Environment.NewLine + ex.GetType().ToString() + ", TIPO: " + errorMessage;
                    createDump(a_msg);
                    throw;
                }

                System.Console.BackgroundColor = ConsoleColor.Black;

            }

            string fechadt = Convert.ToString(DateTime.Now.Day) + "-" + Convert.ToString(DateTime.Now.Month) + "-" + Convert.ToString(DateTime.Now.Year);
            createDump("PROCESO TERMINADO, con fecha: "+ fechadt);
            
            //Console.WriteLine("\n" + enviarEmail("corellana@gildemeister.cl, mguzman@mets.cl, nespinoza@mets.cl, cpalomera@mets.cl, ccontreras@mets.cl", "fcontreras@mets.cl, pmoncada@mets.cl, sgodas@mets.cl", "", "AUTONOMOHYUNDAI - PROCESO DE ENVIO " + fechadt, "EL DIA " + fechadt + " SE LEYERON " + contador.ToString() + " REGISTRO(S) PARA SER PROCESADOS. EN RESUMEN: \r\n\r\n - SE ENVIARON " + totalinternos.ToString() + " MAILS INTERNOS.  \r\n - SE ENVIARON " + totalexternos.ToString() + " MAILS EXTERNOS. \r\n\r\n ERRORES \r\n\r\n - NO SE PUDIERON PROCESAR " + totalsinbuenformato.ToString() + " MAIL(S) POR ERROR DEL FORMATO DEL CORREO DESTINO. \r\n - NO SE PUDO ACTUALIZAR EL ID EN LA TABLA 'DEMONIO_COLA_CONTACTOSWEB' UN TOTAL DE " + totalerrorsql.ToString() + " REGISTROS. \r\n\r\n MAIL ENVIADO EN FORMA AUTOMATICA, FAVOR DE NO CONTESTAR. \r\n\r\n METS S.A"));
            count++;
        }
        public static string fecha() {
            return Convert.ToString(DateTime.Now.Day)+ "/" + Convert.ToString(DateTime.Now.Month) + "/" + Convert.ToString(DateTime.Now.Year) + " " + DateTime.Now.ToShortTimeString();
        }
        protected override void OnStart(string[] args)
        {
            createDump("Servicio fue iniciado "+ fecha());
            timeDelay.Enabled = true;
        }
        protected override void OnStop()
        {
            createDump("Servicio fue detenido" + fecha());
            timeDelay.Enabled = false;
        }
        /*private void LogService(string content)
        {
            FileStream fs = new FileStream(@"d:\\TestServiceLog.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.BaseStream.Seek(0, SeekOrigin.End);
            sw.WriteLine(content);
            sw.Flush();
            sw.Close();
        }*/

        private void timer1_Tick(object sender, EventArgs e)
        {

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
                string sqlinsert = "UPDATE " + tabla + "S SET OUT_ESTADO = " + estado + ", OUT_ID = '" + nuevo + "', OUT_MSG = '" + desc + "', OUT_FECHA = GETDATE() WHERE ID_" + tabla + " = " + idx;
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
                createDump("ERROR SQL TABLA: " + tabla + " ID:" + idx + Environment.NewLine + " DESCRIPCION: " + Environment.NewLine + errorMessage + ", FECHA: " + fecha() + Environment.NewLine);
                createDump("---> ERROR: " + errorMessage);
            }

        }

        public static void createDump(string logdata)
        {
            string strLogText = logdata;
            StreamWriter log;
            string carpeta = Convert.ToString(DateTime.Now.Day) + Convert.ToString(DateTime.Now.Month) + Convert.ToString(DateTime.Now.Year);
            string archivo = rutalog + "log_" + carpeta + ".txt";
            Directory.CreateDirectory(Path.GetDirectoryName(archivo));
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

        private static void Logout()
        {
            client.logout(sessionHeader);
        }

        private static string crearContacto(string id, string n, string c, string t)
        {

            Contact acct = new Contact();
            acct.Name = n; //nombre de la cuenta
            acct.Email = c; //correo
            acct.Phone = t; //teléfono

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

        private static string crearCuenta(string id, string n, string s, string r)
        {

            //string nombre = RandomString(8);
            Account acct = new Account();
            acct.Name = n;
            acct.Razon_Social__c = s;
            acct.Rut__c = r;
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
                terminar("CUENTA", id, idm, "1", "Correcto");
                return "1|" + id.ToString();
            }
            else
            {
                string result = createResults[0].errors[0].message;
                terminar("CUENTA", id, "", "2", result);
                return "-1|" + result;
            }

        }
    }
}
