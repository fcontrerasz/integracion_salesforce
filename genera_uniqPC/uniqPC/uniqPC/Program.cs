using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uniqPC
{
    class Program
    {
        static void Main(string[] args)
        {
            FingerPrint maquina = new FingerPrint();
            string makina = maquina.unico;
            Console.Write(makina);
            Console.ReadLine();
        }
    }
}
