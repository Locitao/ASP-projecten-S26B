using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MaterialRenting;

namespace UnitTestMaterialRenting
{
    [TestClass]
    public class UnitTestMaterial
    {
        List<DateTime[]> dates = new List<DateTime[]>();
        DateTime[] date1 = new DateTime[2];
        private DateTime myDate;
        private DateTime myDate2;

        /// <summary>
        /// mades the static information that every test method in this test class uses
        /// </summary>
        public UnitTestMaterial()
        {
            date1[0] = myDate;
            date1[1] = myDate2;
            dates.Add(date1);
            DateTime[] date2 = new DateTime[2];
            myDate = DateTime.ParseExact("01-06-2015", "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            myDate2 = DateTime.ParseExact("05-06-2015", "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            date2[0] = myDate;
            date2[1] = myDate2;
            dates.Add(date2);
        }

        [TestMethod]
        public void TestMaterialStatus1()
        {
            // same dates as the reserved dates
            Material mat = new Material(1, "Samsung" ,"Galaxy S5", 25, "0001.001", dates);
            myDate = DateTime.ParseExact("01-06-2015", "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            myDate2 = DateTime.ParseExact("05-06-2015", "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            Status result = mat.CheckStatus(myDate, myDate2);
            Assert.AreEqual(result, Status.Reserved);
        }

        [TestMethod]
        public void TestMaterialStatus2()
        {
            // dates are one month earlier than the reserved dates
            Material mat = new Material(1, "Samsung", "Galaxy S5", 25, "0001.001", dates);
            myDate = DateTime.ParseExact("01-05-2015", "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            myDate2 = DateTime.ParseExact("05-05-2015", "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            Status result = mat.CheckStatus(myDate, myDate2);
            Assert.AreEqual(result, Status.Free);
        }

        [TestMethod]
        public void TestMaterialStatus3()
        {
            // the first date is between the reserved dates
            Material mat = new Material(1, "Samsung", "Galaxy S5", 25, "0001.001", dates);
            myDate = DateTime.ParseExact("03-06-2015", "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            myDate2 = DateTime.ParseExact("15-06-2015", "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            Status result = mat.CheckStatus(myDate, myDate2);
            Assert.AreEqual(result, Status.Reserved);
        }

        [TestMethod]
        public void TestMaterialStatus4()
        {
            // the first date is before, and the second date is after the reserved dates
            Material mat = new Material(1, "Samsung", "Galaxy S5", 25, "0001.001", dates);
            myDate = DateTime.ParseExact("25-05-2015", "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            myDate2 = DateTime.ParseExact("15-06-2015", "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            Status result = mat.CheckStatus(myDate, myDate2);
            Assert.AreEqual(result, Status.Reserved);
        }
    }
}
