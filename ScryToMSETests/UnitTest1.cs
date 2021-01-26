using Microsoft.VisualStudio.TestTools.UnitTesting;
using static ScryToMSE.Program;
using System;

namespace ScryToMSETests
{
    [TestClass]
    public class UnitTest1
    {
        Card JaseMirrorMageRu = GetCard("ZNR", 63, "ru");
        Card JaseMirrorMageEn = GetCard("ZNR", 63);

        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual(GetNumberOfSet(1, 20), "01/20");
            Assert.AreEqual(GetNumberOfSet(1, 200), "001/200");
            Assert.AreEqual(GetNumberOfSet(199, 200), "199/200");
            Assert.AreEqual(GetNumberOfSet(201, 200), "201              ");
        }

        [TestMethod]
        public void TestGetNameSting()
        {
            Assert.AreEqual(GetNameSting(JaseMirrorMageRu), "\tname: <b>Джейс, Маг Зеркал</b>" + Environment.NewLine);
            Assert.AreEqual(GetNameSting(JaseMirrorMageEn), "\tname: <b>Jace, Mirror Mage</b>" + Environment.NewLine);
        }
    }
}
