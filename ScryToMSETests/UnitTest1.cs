using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScryToMSE;
using System;
using System.Text;
using System.Net;
using System.IO;
using System.IO.Compression;


namespace ScryToMSETests
{
    [TestClass]
    public class UnitTest1
    {
        Card JaseMirrorMageRu = Program.GetCard("ZNR", 63, "ru");
        //Card JaseMirrorMageEn = Program.GetCard("ZNR", 63);

        [TestMethod]
        public void TestMethod1()
        {
            var a = new Card();
            a.Num(1,2);
            Assert.AreEqual(a.collector_number, "001/2");
            a.Num(10, 395);
            Assert.AreEqual(a.collector_number, "010/395");
            Assert.AreEqual(Program.GetNumberOfSet(1, 2), "001/2");
        }

        [TestMethod]
        public void TestGetNameSting()
        {
            Assert.AreEqual(Program.GetNameSting(JaseMirrorMageRu), "\tname: <b>Джейс, Маг Зеркал</b>" + Environment.NewLine);
            //Assert.AreEqual(Program.GetNameSting(JaseMirrorMageEn), "\tname: <b>Jace, Mirror Mage</b>" + Environment.NewLine);
        }
    }
}
