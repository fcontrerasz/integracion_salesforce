using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace appv1
{
    class Program
    {
        static void Main(string[] args)
        {
            /*salesforce.Soap nuevo = new salesforce.Soap();
            LoginScopeHeader d_scp = new LoginScopeHeader();
            loginRequest d_log = new loginRequest(d_scp,"francisco","test");
            nuevo.login(d_log);*/

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            HttpWebRequest request = CreateWebRequest();
            XmlDocument soapEnvelopeXml = new XmlDocument();

            soapEnvelopeXml.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:urn=""urn:enterprise.soap.sforce.com"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
<soap:Body>
<urn:login>
<urn:username>consultor_force@nectia.inexsbx.com</urn:username>
<urn:password>nectia2020lT2HDj7BFGeAHkmSLqm2cRMu</urn:password>
</urn:login>
</soap:Body>
</soap:Envelope>");

            using (Stream stream = request.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
            using (
                WebResponse response = request.GetResponse())
            {
                using (StreamReader rd = new StreamReader(response.GetResponseStream()))
                {
                    string soapResult = rd.ReadToEnd();
                    Console.WriteLine(soapResult);
                    Console.Read();
                }
            }




        }
        public static HttpWebRequest CreateWebRequest()
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@"https://test.salesforce.com/services/Soap/c/43.0/0DFm00000008PV5");
            webRequest.Headers.Add(@"SOAP:Action");
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            return webRequest;
        }
    }
}
