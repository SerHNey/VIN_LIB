using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VIN_LIB;

namespace ConsoleApp1
{

    internal class Program
    {
        static void Main(string[] args)
        {
            string vin = "JHMCM56557C404453";
            var test = new Class1();
            var s = test.GetVINCountry(vin);
            Console.ReadLine();
        }
    }
}
