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
            Assert.AreEqual(GetNameSting(JaseMirrorMageRu), "\tname: <b>�����, ��� ������</b>" + Environment.NewLine);
            Assert.AreEqual(GetNameSting(JaseMirrorMageEn), "\tname: <b>Jace, Mirror Mage</b>" + Environment.NewLine);
        }

        [TestMethod]
        public void TestGetRuleText()
        {
            Assert.AreEqual(Environment.NewLine + GetRuleTextString(JaseMirrorMageRu), Environment.NewLine + "\tspecial_text:" + Environment.NewLine +
                "\t\t��������� <sym>2</sym>" + Environment.NewLine +
                "\t\t����� �����, ��� ������ ������� �� ���� �����, ���� �� ������� ���������, �������� ���� �����, " +
                "���������� ������ ������, ���� ������, �� ��� ���� ��� �� �������� ����������� � �� ��������� �������� ����� 1." + Environment.NewLine +
                "\t\t+1: ����������� 2." + Environment.NewLine +
                "\t\t0: �������� ����� � �������� ��. ������� � ������, ���� ������ ���������� ������� ��������, ������ ���������������� ����-��������� ��� �����." + Environment.NewLine +
                "\tlevel_1_text: " + Environment.NewLine +
                "\t\t��������� <sym>2</sym>" + Environment.NewLine +
                "\t\t����� �����, ��� ������ ������� �� ���� �����, ���� �� ������� ���������, �������� ���� �����, " +
                "���������� ������ ������, ���� ������, �� ��� ���� ��� �� �������� ����������� � �� ��������� �������� ����� 1." + Environment.NewLine +
                "\tloyalty_cost_2: +1" + Environment.NewLine +
                "\tlevel_2_text: " + Environment.NewLine +
                "\t\t����������� 2." + Environment.NewLine +
                "\tloyalty_cost_3: 0" + Environment.NewLine +
                "\tlevel_3_text: " + Environment.NewLine +
                "\t\t�������� ����� � �������� ��. ������� � ������, ���� ������ ���������� ������� ��������, ������ ���������������� ����-��������� ��� �����." + Environment.NewLine);
        }
    }
}
