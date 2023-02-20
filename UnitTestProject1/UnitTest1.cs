using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text.RegularExpressions;
using VIN_LIB;
namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethodCounty() // Страна производителя совпадает
        {
            string vin = "JHMCM56557C404453";
            string vout = "J[A-T]";
            var test = new Class1();
            string country = test.GetVINCountry(vin);
            Assert.AreEqual(vout, country); 
        }

        [TestMethod]
        public void TestMethodCheckVINTrue() // VIN совпадает
        {
            string vin = "JHMCM56557C404453";
            var test = new Class1();
            bool country = test.CheckVIN(vin);
            Assert.AreEqual(true, country);
        }

        [TestMethod]
        public void TestMethodCheckVINFalse() // Когда в VIN больше символов
        {
            string vin = "JHMCM56557C4044535";
            var test = new Class1();
            bool country = test.CheckVIN(vin);
            Assert.AreEqual(false, country);
        }


        [TestMethod]
        public void TestMethodCheckVINFalseCount() // Когда в VIN больше меньше
        {
            string vin = "JHMCM56557C4044";
            var test = new Class1();
            bool country = test.CheckVIN(vin);
            Assert.AreEqual(false, country);
        }

        [TestMethod]
        public void TestMethodCheckVINFalseControl() // Контрольная сумма
        {
            string vin = "JHMCM56557C404453";
            var test = new Class1();
            bool country = test.CheckControlCount(vin);
            Assert.AreEqual(true, country);
        }
    }
}
