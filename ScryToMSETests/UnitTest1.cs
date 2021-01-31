using Microsoft.VisualStudio.TestTools.UnitTesting;
using static ScryToMSE.Program;
using System;

namespace ScryToMSETests
{
    [TestClass]
    public class Class1_Style
    {
        [TestMethod, TestCategory("Style")]
        public void TestGetStyleCardString()
        {
            var res = "\tstylesheet: m15-altered-ru\u000A" +
                "\tstylesheet_version: 2020-09-04\u000a" +
                "\thas_styling: true\u000A" +
                "\tstyling_data:\u000A" +
                "\t\tchop_top: 7\u000A" +
                "\t\tchop_bottom: 7\u000A" +
                "\t\ttext_box_mana_symbols: magic-mana-small.mse-symbol-font\u000A" +
                "\t\tlevel_mana_symbols: magic-mana-large.mse-symbol-font\u000A" +
                "\t\tinverted_common_symbol: no\u000A" +
                "\t\toverlay: \u000A" +
                "\tnotes: Создано автоматически\u000A";
            Assert.AreEqual(GetStyleCardString(GetCard("ZNR", 1, "ru")).Remove(309, 71), res);
        }
    }

    [TestClass]
    public class Class2_Color
    {
        [TestMethod]
        public void OneColor()
        {
            var res = "\tcard_color: white, \u000A";
            Assert.AreEqual(GetColorCardString(GetCard("ZNR", 3, "ru")), res);
        }
    }

    [TestClass]
    public class Class3_Name
    {
        [TestMethod]
        public void Test1_GetNameStringEn()
        {
            Assert.AreEqual(GetNameSting(GetCard("ZNR", 63)), "\tname: <b>Jace, Mirror Mage</b>\u000A");
        }

        [TestMethod, TestCategory("Name")]
        public void Test2_GetNameStingRu()
        {
            Assert.AreEqual(GetNameSting(GetCard("ZNR", 63, "ru")), "\tname: <b>Джейс, Маг Зеркал</b>\u000A");
        }
    }

    [TestClass]
    public class Class4_CastingCost
    {
        [TestMethod]
        public void Test1()
        {
            var res = "\tcasting_cost: 2W\u000A";
            Assert.AreEqual(GetCastingCost(GetCard("ZNR", 3, "ru")), res);
        }
    }

    [TestClass]
    public class Class5_Type
    {
        [TestMethod]
        public void Test1()
        {
            var res = "\tsuper_type: <word-list-type><b>Существо</b></word-list-type>\u000A" +
                "\tsub_type: <word-list-race><b>Архонт</b></word-list-race>\u000A";
            Assert.AreEqual(GetTypesString(GetCard("ZNR", 4, "ru")), res);
        }
    }

    [TestClass]
    public class Class7_RuleText
    {
        [TestMethod]
        public void TestGetRuleTextOneString()
        {
            var res = "\trule_text: Не более двух целевых существ получают по +X/+X до конца хода, где Х — количество существ в вашем отряде. " +
                "<i-auto>(В вашем отряде может быть не более одного Бродяги, Воина, Священника и Чародея.)</i-auto>\u000A";
            Assert.AreEqual(GetRuleTextString(GetCard("ZNR", 1, "ru")), res);
        }

        [TestMethod]
        public void TestGetRuleTextSomeString()
        {
            var res = "\trule_text:\u000A" +
                "\t\tПолет, Двойной удар\u000A" +
                "\t\tКаждый раз, когда существо под вашим контролем наносит боевые повреждения игроку, вы и тот игрок получаете по такому же количеству жизней.\u000A" +
                "\t\tВ начале вашего заключительного шага, если количество ваших жизней превышает ваше начальное количество жизней как минимум на 15, то каждый игрок, " +
                "которого Ангел Судьбы атаковал в этом ходу, проигрывает партию.\u000A";
            Assert.AreEqual(GetRuleTextString(GetCard("ZNR", 2, "ru")), res);
        }

        //[TestMethod]
        public void TestGetRuleText()
        {
            Assert.AreEqual(GetRuleTextString(GetCard("ZNR", 63, "ru")), "\tspecial_text:\u000A" +
                "\t\tУсилитель <sym>2</sym>\u000A" +
                "\t\tКогда Джейс, Маг Зеркал выходит на поле битвы, если он получил Усилитель, создайте одну фишку, " +
                "являющуюся копией Джейса, Мага Зеркал, но при этом она не является легендарной и ее начальная верность равна 1.\u000A" +
                "\t\t+1: предскажите 2.\u000A" +
                "\t\t0: возьмите карту и покажите ее. Удалите с Джейса, Мага Зеркал количество жетонов верности, равное конвертированной мана-стоимости той карты.\u000A" +
                "\tlevel_1_text: \u000A" +
                "\t\tУсилитель <sym>2</sym>\u000A" +
                "\t\tКогда Джейс, Маг Зеркал выходит на поле битвы, если он получил Усилитель, создайте одну фишку, " +
                "являющуюся копией Джейса, Мага Зеркал, но при этом она не является легендарной и ее начальная верность равна 1.\u000A" +
                "\tloyalty_cost_2: +1\u000A" +
                "\tlevel_2_text: \u000A" +
                "\t\tпредскажите 2.\u000A" +
                "\tloyalty_cost_3: 0\u000A" +
                "\tlevel_3_text: \u000A" +
                "\t\tвозьмите карту и покажите ее. Удалите с Джейса, Мага Зеркал количество жетонов верности, равное конвертированной мана-стоимости той карты.\u000A");
        }
    }

    [TestClass]
    public class Class8_FlavorText
    {
        [TestMethod]
        public void TestGetFlavorTextNoneString()
        {
            var res = "\tflavor_text: <i-flavor></i-flavor>\u000A";
            Assert.AreEqual(GetFlavorTextString(GetCard("ZNR", 2, "ru")), res);
            Assert.AreEqual(GetFlavorTextString(GetCard("ZNR", 5, "ru")), res);
        }

        [TestMethod]
        public void TestGetFlavorTextSomeString()
        {
            var res = "\tflavor_text:\u000a" +
                "\t\t<i-flavor>«Архонты вершат правосудие согласно своим древним — и ошибочным — убеждениям».<soft-line>\u000a" +
                "\t\t</soft-line>— Нисса Ревейн</i-flavor>\u000A";
            Assert.AreEqual(GetFlavorTextString(GetCard("ZNR", 4, "ru")), res);
        }

        [TestMethod]
        public void TestGetFlavorTextOneString()
        {
            var res = "\tflavor_text: <i-flavor>Общий враг положит конец старой вражде.</i-flavor>\u000A";
            Assert.AreEqual(GetFlavorTextString(GetCard("ZNR", 1, "ru")), res);
        }
    }

    [TestClass]
    public class Class9_CustomCardNumber
    {
        [TestMethod]
        public void TestGetNumberOfSet()
        {
            Assert.AreEqual(GetNumberOfSet(1, 20), "01/20");
            Assert.AreEqual(GetNumberOfSet(1, 200), "001/200");
            Assert.AreEqual(GetNumberOfSet(199, 200), "199/200");
            Assert.AreEqual(GetNumberOfSet(201, 200), "201              ");
        }

    }
}
