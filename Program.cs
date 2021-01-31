using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.IO.Compression;

/// <summary>
/// Сет
/// </summary>
public class Set
{
    public String id;
    public String code;
    public String mtgo_code;
    public String arena_code;
    public Int16 tcgplayer_id;
    public String name;
    public String uri;
    public String scryfall_uri;
    public String search_uri;
    public String released_at;
    public String set_type;
    public Int16 card_count;
    public Int32 unique_card;
    public Boolean digital;
    public Boolean nonfoil_only;
    public Boolean foil_only;
    public String block_code;
    public String block;
    public String icon_svg_uri;
    public String lang;

    static void PrintNotNull(String str, String preStr) { if ((str != "") && !(str is null)) Console.WriteLine(preStr + ": " + str); }

    public void Print()
    {
        PrintNotNull(id, "Id");
        PrintNotNull(name, "Оригинальное имя");
        PrintNotNull(code, "Код");
    }
}

public class Image_urls
{
    public String small;
    public String normal;
    public String large;
    public String png;
    public String art_crop;
    public String border_crop;
}

public class Card_face
{
    public String name;
    public String printed_name;
    public String mana_cost;
    public String type_line;
    public String printed_type_line;
    public String oracle_text;
    public String printed_text;
    public String[] colors;
    public String power;
    public String toughness;
    public String flavor_text;
    public String artist;
    public Image_urls image_uris;
    public String download_url;
    public String collector_number;
    public String image_name;
}

/// <summary>
/// Карта сета
/// </summary>
public class Card
{
    public String id;
    public String oracle_id;
    public String[] multiverse_ids;
    public String mtgo_id;
    public String tcgplayer_id;
    public String name;
    public String printed_name;
    public String lang;
    public String released_at;
    public Image_urls image_uris;
    public String set;
    public String collector_number;
    public String image_name;
    public String type_line;
    public String printed_type_line;
    public String power;
    public String toughness;
    public String loyalty;
    public Int32 loyalty_count;
    public String mana_cost;
    public String rarity;
    public String oracle_text;
    public String printed_text;
    public String flavor_text;
    public String artist;
    public String[] color_identity;
    public String[] colors;
    public Card_face[] card_faces;
    public String download_url;
    public String[] frame_effects;
    public String layout;

    static void PrintNotNull(String str, String preStr) { if ((str != "") && !(str is null)) Console.WriteLine(preStr + ": " + str); }

    public void Print()
    {
        PrintNotNull(id, "Id");
        PrintNotNull(name, "Оригинальное имя");
        PrintNotNull(printed_name, "Имя");
        PrintNotNull(image_uris.png, "Картиночка");
        PrintNotNull(image_uris.art_crop, "Арт");
    }
}

public class List
{
    public Int32 total_cards;
    public Card[] data;
}

namespace ScryToMSE
{
    public class Program
    {
        static Set GetSet(String setCode)
        {
            WebClient client = new WebClient();
            Encoding utf8 = Encoding.GetEncoding("UTF-8");
            Encoding win1251 = Encoding.GetEncoding("Windows-1251");

            byte[] utf8Bytes = win1251.GetBytes(client.DownloadString("https://api.scryfall.com/sets/" + setCode));
            byte[] win1251Bytes = Encoding.Convert(utf8, win1251, utf8Bytes);

            return JsonConvert.DeserializeObject<Set>(win1251.GetString(win1251Bytes));
        }

        public static Card GetCard(String setCode, Int32 num, String lang = "en")
        {
            Console.WriteLine(num);
            WebClient client = new WebClient();
            Card res = new Card();

            try
            {
                res = JsonConvert.DeserializeObject<Card>(Encoding.UTF8.GetString(
                    Encoding.Default.GetBytes(
                        client.DownloadString("https://api.scryfall.com/cards/" + setCode.ToLower() + "/" + num + "/" + lang))));
            }
            catch (WebException)
            {
                res = JsonConvert.DeserializeObject<Card>(client.DownloadString("https://api.scryfall.com/cards/" + setCode.ToLower() + "/" + num));

                try
                {
                    var tmp = JsonConvert.DeserializeObject<List>(Encoding.UTF8.GetString(
                        Encoding.Default.GetBytes(
                            client.DownloadString("https://api.scryfall.com/cards/search?q=" + res.name.Replace(" ", "%20") + "%20lang%3Arussian")
                        )));
                    
                    foreach (Card c in tmp.data)
                        if (c.oracle_id == res.oracle_id)
                        {
                            res.printed_name = c.printed_name;
                            res.printed_text = c.printed_text;
                            res.flavor_text = c.flavor_text;
                            res.printed_type_line = c.printed_type_line;
                            res.card_faces = c.card_faces;
                            res.lang = c.lang;
                        }
                }
                catch (WebException)
                {
                    Console.WriteLine("Будет английская");                   
                }
            }

            var enRes = JsonConvert.DeserializeObject<Card>(client.DownloadString("https://api.scryfall.com/cards/" + setCode.ToLower() + "/" + num));

            switch (res.layout)
            {
                case "modal_dfc":                    
                    res.card_faces[0].download_url = enRes.card_faces[0].image_uris.art_crop;
                    res.card_faces[0].image_name = res.collector_number + "a";
                    res.card_faces[1].download_url = enRes.card_faces[1].image_uris.art_crop;
                    res.card_faces[1].image_name = res.collector_number + "b";
                    break;
                default:
                    res.download_url = enRes.image_uris.art_crop;
                    break;
            }
            return res;
        }

        static List<String> GetStrings(string str)
        {
            var res = new List<String>();
            if (str != null)
                foreach (var s in str.Split('\n'))
                    res.Add(s.Trim().Replace("(", "<i-auto>(").Replace(")", ")</i-auto>"));
            return res;       
        }

        static String GetColor(String type, String[] colors, String text = null)
        {
            String res = "";
            if (colors.Length > 2) res += "multicolor, ";
            if (type.ToLower().IndexOf("land") >= 0) res += "land, ";
            if (type.ToLower().IndexOf("artifact") >= 0) res += "artifact, ";
            foreach (var s in colors)
            {
                if (s == "W") res += "white, ";
                if (s == "U") res += "blue, ";
                if (s == "D") res += "black, ";
                if (s == "R") res += "red, ";
                if (s == "G") res += "green, ";
            }
            if (text != null && text != "")
                foreach (var a in text.Split('{'))
                {
                    if (a.Split('}')[0] == "W") res += "white, ";
                    if (a.Split('}')[0] == "U") res += "blue, ";
                    if (a.Split('}')[0] == "D") res += "black, ";
                    if (a.Split('}')[0] == "R") res += "red, ";
                    if (a.Split('}')[0] == "G") res += "green, ";
                }
            return res;
        }

        public static String GetStyleCardString(Card card)
        {
            String res = "\tstylesheet: ";
            switch (card.layout)
            {
                case "modal_dfc":
                    res += card.lang == "ru" ? "m15-mainframe-dfc-ru\u000A" : "m15-mainframe-dfc\u000A";
                    res += "\thas_styling: true" + "\u000A";
                    res += "\tstyling_data:" + "\u000A";
                    res += "\t\tother_options: use hovering pt, use holofoil stamps, unindent nonloyalty abilities, auto nyx crowns" + "\u000A";
                    res += "\t\ttext_box_mana_symbols: magic-mana-small.mse-symbol-font" + "\u000A";
                    res += "\t\tlevel_mana_symbols: magic-mana-large.mse-symbol-font" + "\u000A";
                    res += "\t\toverlay:" + "\u000A";
                    res += "\textra_data:" + "\u000A";
                    res += "\t\tmagic-m15-mainframe-dfc-ru:" + "\u000A";
                    res += "\t\t\tcorner: " + card.card_faces[0].type_line.ToLower().Split('—')[0].Trim() + "\u000A";
                    res += "\t\t\tcorner_2: " + card.card_faces[1].type_line.ToLower().Split('—')[0].Trim() + "\u000A";
                    res += "\tnotes: Создано автоматически" + "\u000A";
                    res += "\ttime_created: " + DateTime.Now.ToString("u").Remove(DateTime.Today.ToString("u").Length - 1) + "\u000A";
                    res += "\ttime_modified: " + DateTime.Now.ToString("u").Remove(DateTime.Today.ToString("u").Length - 1) + "\u000A";
                    break;
                case "adventure":
                    res += "m15-adventure-ru" + "\u000A";
                    res += "\thas styling: true" + "\u000A";
                    res += "\tstyling data:" + "\u000A";
                    res += "\t\tchop main: " + "\u000A";
                    res += "\t\tshrink name text: " + "\u000A";
                    if (card.frame_effects != null)
                        foreach (var str in card.frame_effects)
                            if (str == "showcase") res += "\t\tframes: Spotlight" + "\u000A";
                            else res += "\t\tframes: " + "\u000A";
                    res += "\t\tauto frames: " + "\u000A";
                    res += "\t\tother options: " + "\u000A";
                    res += "\t\ttext box mana symbols: magic-mana-small.mse-symbol-font" + "\u000A";
                    res += "\t\tpromo: no" + "\u000A";
                    res += "\t\toverlay: " + "\u000A";
                    break;
                default:
                    switch (card.type_line.Split('—')[0].Trim())
                    {
                        case "Legendary Planeswalker":
                            res += card.lang == "ru" ? "m15-mainframe-planeswalker-ru" : "m15-mainframe-planeswalker-ru";
                            res += "\u000A";
                            res += "\tnotes: Создано автоматически" + "\u000A";
                            res += "\ttime_created: " + DateTime.Now.ToString("u").Remove(DateTime.Today.ToString("u").Length - 1) + "\u000A";
                            res += "\ttime_modified: " + DateTime.Now.ToString("u").Remove(DateTime.Today.ToString("u").Length - 1) + "\u000A";
                            res += "\thas_styling: true" + "\u000A";
                            res += "\tstyling data:" + "\u000A";
                            res += "\t\tКоличество_полей_способностей: Три" + "\u000A";
                            break;
                        default:
                            res += card.lang == "ru" ? "m15-altered-ru\u000A" : "m15-altered\u000A";
                            res += "\tstylesheet_version: 2020-09-04\u000a";
                            res += "\thas_styling: true\u000A";
                            res += "\tstyling_data:\u000A";
                            res += "\t\tchop_top: 7\u000A";
                            res += "\t\tchop_bottom: 7\u000A";

                            if (card.frame_effects != null)
                            {
                                res += "\t\tframes: ";
                                foreach (var str in card.frame_effects)
                                {
                                    if (str == "extendedart") res += "puma, ";
                                    if (str == "legendary") res += "legend, ";
                                    if (str == "inverted") res += "fnm promo, ";
                                }
                                res += "\t\tframes: " + "\u000A";
                            }

                            res += "\t\ttext_box_mana_symbols: magic-mana-small.mse-symbol-font\u000A";
                            res += "\t\tlevel_mana_symbols: magic-mana-large.mse-symbol-font\u000A";
                            res += "\t\tinverted_common_symbol: no\u000A";
                            res += "\t\toverlay: \u000A";
                            res += "\tnotes: Создано автоматически\u000A";
                            res += "\ttime_created: " + DateTime.Now.ToString("u").Remove(DateTime.Today.ToString("u").Length - 1) + "\u000A";
                            res += "\ttime_modified: " + DateTime.Now.ToString("u").Remove(DateTime.Today.ToString("u").Length - 1) + "\u000A";
                            break;
                    }                    
                    break;
            }
            return res;
        }

        public static String GetColorCardString(Card card)
        {
            String res = "\tcard_color: ";

            switch (card.layout)
            {
                case "modal_dfc":
                    res += GetColor(card.card_faces[0].type_line, card.card_faces[0].colors) + "\u000A";
                    res += "\tcard_color_2: " + GetColor(card.card_faces[1].type_line, card.card_faces[1].colors, card.card_faces[1].oracle_text);
                    break;
                default:
                    res += GetColor(card.type_line, card.colors, card.oracle_text);
                    break;
            }
            return res + "\u000A";
        }

        public static String GetNameSting(Card card)
        {
            String res = "\tname: <b>";
            switch (card.layout)
            {
                case "modal_dfc":
                    res += card.lang == "en" ? card.card_faces[0].name : card.card_faces[0].printed_name;
                    res += "</b>" + "\u000A";
                    res += "\tname_2: <b>";
                    res += card.lang == "en" ? card.card_faces[1].name : card.card_faces[1].printed_name;
                    res += "</b>" + "\u000A";
                    break;
                case "adventure":
                    res += card.lang == "en" ? card.card_faces[0].name : card.card_faces[0].printed_name;
                    res += "</b>" + "\u000A";
                    break;
                default:
                    res += card.lang == "en" ? card.name : card.printed_name;
                    res += "</b>" + "\u000A";
                    break;
            }
            return res;
        }

        public static String GetCastingCost(Card card)
        {
            switch (card.layout)
            {
                case "modal_dfc":
                    return "\tcasting_cost: " + card.card_faces[0].mana_cost + "\u000A" +
                        "\tcasting_cost_2: " + card.card_faces[1].mana_cost + "\u000A";
                default:
                    return "\tcasting_cost: " + card.mana_cost.Trim().Replace("{", "").Replace("}","") + "\u000A";
            }
        }

        public static String GetTypesString(Card card)
        {
            String res = "";
            switch (card.layout)
            {
                case "modal_dfc":
                    res += "\tsuper type: <b>";
                    res += card.lang == "en" ? card.card_faces[0].type_line.Split('—')[0].Trim() : card.card_faces[0].printed_type_line.Split('—')[0].Trim();
                    res += "</b>" + "\u000A";
                    if (card.card_faces[0].type_line.Split('—').Length > 1)
                    {
                        res += "\tsub type: <b>";
                        res += card.lang == "en" ? card.card_faces[0].type_line.Split('—')[1].Trim() : card.card_faces[0].printed_type_line.Split('—')[1].Trim();
                        res += "</b>" + "\u000A";
                    }
                    res += "\tsuper type 2: <b>";
                    res += card.lang == "en" ? card.card_faces[1].type_line.Split('—')[0].Trim() : card.card_faces[1].printed_type_line.Split('—')[0].Trim();
                    res += "</b>" + "\u000A";
                    if (card.card_faces[1].type_line.Split('—').Length > 1)
                    {
                        res += "\tsub type 2: <b>";
                        res += card.lang == "en" ? card.card_faces[1].type_line.Split('—')[1].Trim() : card.card_faces[1].printed_type_line.Split('—')[1].Trim();
                        res += "</b>" + "\u000A";
                    }
                    break;
                case "adventure":
                    res += "\tsuper type: <b>";
                    res += card.lang == "en" ? card.card_faces[0].type_line.Split('—')[0].Trim() : card.card_faces[0].printed_type_line.Split('—')[0].Trim();
                    res += "</b>" + "\u000A";
                    if (card.card_faces[0].type_line.Split('—').Length > 1)
                    {
                        res += "\tsub type: <b>";
                        res += card.lang == "en" ? card.card_faces[0].type_line.Split('—')[1].Trim() : card.card_faces[0].printed_type_line.Split('—')[1].Trim();
                        res += "</b>" + "\u000A";
                    }
                    break;
                default:
                    res += "\tsuper_type: <word-list-type><b>";
                    res += card.lang == "en" ? card.type_line.Split('—')[0].Trim() : card.printed_type_line.Split('—')[0].Trim();
                    res += "</b></word-list-type>" + "\u000A";
                    if (card.type_line.Split('—').Length > 1)
                    {
                        res += "\tsub_type: <word-list-race><b>";
                        res += card.lang == "en" ? card.type_line.Split('—')[1].Trim() : card.printed_type_line.Split('—')[1].Trim();
                        res += "</b></word-list-race>" + "\u000A";
                    }
                    break;
            }
            return res;
        }

        static String GetRarityString(String str)
        {
            if (str == "mythic")
                return "\trarity: mythic rare" + "\u000A";
            else
                return "\trarity: " + str + "\u000A";
        }

        public static String GetRuleTextString(Card card)
        {
            String rule = "";
            switch (card.layout)
            {
                case "modal_dfc":
                    rule += "\trule_text:" + "\u000A";
                    rule += card.lang == "en" ?
                        GetStrings(card.card_faces[0].oracle_text) :
                            GetStrings(card.card_faces[0].printed_text);
                    rule += "\trule_text_2:" + "\u000A";
                    rule += card.lang == "en" ? GetStrings(card.card_faces[1].oracle_text) : GetStrings(card.card_faces[1].printed_text);
                    break;
                default:
                    switch (card.type_line.Split('—')[0].Trim())
                    {/*
                        case "Legendary Planeswalker":
                            rule += "\tspecial_text:" + "\u000A";
                            rule += card.lang == "en" ? GetStrings(card.oracle_text) : GetStrings(card.printed_text);
                            var levels = new List<string>();

                            Console.WriteLine(GetStrings(card.printed_text));

                            foreach (var str in GetStrings(card.printed_text))
                                if (str.Split(':').Length > 1 || levels.Count == 0)
                                    levels.Add(str + "\u000A");
                                else
                                    levels[levels.Count - 1] =  "\t\t" + str + "\u000A";
                            
                            foreach (var l in levels)
                            {
                                Console.WriteLine(l);
                                Console.ReadKey();
                            }

                            for (var i = 0; i < levels.Count; i++)
                            {
                                if (levels[i].Substring(0, 4).IndexOf(':') >= 1)
                                    rule += "\tloyalty_cost_" + (i + 1) + ": " + levels[i].Split(':')[0].Trim() + "\u000A";
                                rule += "\tlevel_" + (i + 1) + "_text: " + "\u000A";

                                if (levels[i].Split(':').Length > 2)
                                    for (var j = 1; j < levels[i].Split(':').Length; i++)
                                        rule += "\t\t" + levels[i].Split(':')[j].Trim() + "\u000A";
                                else if (levels[i].Split(':').Length == 2)
                                    rule += "\t\t" + levels[i].Split(':')[1].Trim() + "\u000A";
                                else
                                    rule += "\t\t" + levels[i].Split(':')[0].Trim() + "\u000A";
                            }

                            rule = rule.Remove(rule.Length - 4);
                            break;*/
                        default:
                            rule += "\trule_text:\u000A";
                            switch (card.lang)
                            {
                                case "ru":
                                    if (GetStrings(card.printed_text).Count == 1) rule = rule.Remove(11) + " " + GetStrings(card.printed_text)[0] + "\u000A";
                                    else foreach (var s in GetStrings(card.printed_text)) rule += "\t\t" + s + "\u000A"; break;
                                default:
                                    foreach (var s in GetStrings(card.oracle_text)) rule += "\t\t" + s + "\u000A"; break;
                            }
                            break;
                    }
                    break;
            }
            return rule;
        }

        public static String GetFlavorTextString(Card card)
        {
            String res = "\tflavor_text:";
            switch (card.layout)
            {
                case "modal_dfc":
                    if (card.card_faces[0].flavor_text != null)
                        foreach (var a in card.card_faces[0].flavor_text.Split('\n'))
                            res += "\t\t" + a.Replace("*", "</i>") + "\u000A";
                    if (card.card_faces[1].flavor_text != null)
                    {
                        res += "\tflavor_text_2:" + "\u000A";
                        foreach (var a in card.card_faces[1].flavor_text.Split('\n'))
                            res += "\t\t" + a.Replace("*", "</i>") + "\u000A";
                    }
                    break;
                default:
                    if (card.flavor_text != null)
                    {
                        if (GetStrings(card.flavor_text).Count == 1) res += " <i-flavor>" + GetStrings(card.flavor_text)[0];
                        else
                        {
                            for (var i = 0; i < GetStrings(card.flavor_text).Count - 1; i++)
                                switch (i)
                                {
                                    case 0: res += "\u000A\t\t<i-flavor>" + GetStrings(card.flavor_text)[i] + "<soft-line>\u000A"; break;
                                    default: res += "\t\t</soft-line>" + GetStrings(card.flavor_text)[i] + "<soft-line>\u000A"; break;
                                }
                            res += "\t\t</soft-line>" + GetStrings(card.flavor_text)[GetStrings(card.flavor_text).Count - 1];
                        }
                    } else
                        res += " <i-flavor>";
                    break;
            }
            res += "</i-flavor>\u000A";

            Console.WriteLine(res);
            return res;
        }

        static String GetWatermark(Card card)
        {
            var res = "";
            if (card.type_line.Split('—')[0].Trim() == "Basic Land")
                switch (card.type_line.Split('—')[1].Trim())
                {
                    case "Plains":
                        res += "\twatermark: mana symbol white";
                        break;
                    case "Island":
                        res += "\twatermark: mana symbol blue";
                        break;
                    case "Swamp":
                        res += "\twatermark: mana symbol black";
                        break;
                    case "Mountain":
                        res += "\twatermark: mana symbol red";
                        break;
                    case "Forest":
                        res += "\twatermark: mana symbol green";
                        break;
                }
            if (res == "") return null; else return res + "\u000A"; 
        }
               
        static String GetCardText(Card card, int maxSetCard)
        {
            Console.WriteLine(GetNumberOfSet(Convert.ToInt32(card.collector_number), maxSetCard));
            String res = "card:" + "\u000A";
            res += GetStyleCardString(card);
            res += GetColorCardString(card);
            res += GetNameSting(card);
            res += GetCastingCost(card);
            switch (card.layout)
            {
                case "modal_dfc":
                    res += "\timage: image" + card.card_faces[0].image_name + "\u000A";
                    res += "\timage_2: image" + card.card_faces[1].image_name + "\u000A";
                    break;
                default:
                    res += "\timage: image" + card.collector_number + "\u000A";
                    break;
            }            
            res += GetTypesString(card);
            res += GetRarityString(card.rarity);
            res += GetWatermark(card);
            res += GetRuleTextString(card);
            res += GetFlavorTextString(card);
            switch (card.layout)
            {
                case "modal_dfc":
                    if (card.card_faces[0].power != null) res += "\tpower: <b>" + card.card_faces[0].power + "\u000A";
                    if (card.card_faces[0].toughness != null) res += "\ttoughness: <b>" + card.card_faces[0].toughness + "\u000A";
                    if (card.card_faces[1].power != null) res += "\tpower_2: <b>" + card.card_faces[1].power + "\u000A";
                    if (card.card_faces[1].toughness != null) res += "\ttoughness_2: <b>" + card.card_faces[1].toughness + "\u000A";
                    break;
                default:
                    if (card.power != null) res += "\tpower: <b>" + card.power + "</b>" + "\u000A";
                    if (card.toughness != null) res += "\ttoughness: " + card.toughness + "\u000A";
                    break;
            }
            if (card.loyalty != null) res += "\tloyalty: " + card.loyalty + "\u000A";
            res += "\tcustom card number: " + GetNumberOfSet(Convert.ToInt32(card.collector_number), maxSetCard) + "\u000A";
            res += "\tillustrator: " + card.artist + "\u000A";
            return res;
        }

        /// <summary>
        /// Подготовить файлы сета для работы 
        /// </summary>
        /// <param name="set">Обрабатываемый сет</param>
        /// <param name="path">Путь для хранения папки с файлами сета</param>
        /// <param name="rewriteSetFiles">Маркер перезаписи имеющихся в папке сета файлов. По-умолчанию true - переписывать файлы из архива сета</param>
        /// <param name="delSetFile">Маркер удаления файла архива сета. По-умолчанию true - удалять файл после распаковки</param>
        static void PrepareFiles(Set set, String path = "", Boolean rewriteSetFiles = true, Boolean delSetFile = true)
        {
            set.Print();

            if (rewriteSetFiles)        // перезаписать распакованные файлы
            {
                if (Directory.Exists(path + set.code.ToUpper())) Directory.Delete(path + set.code.ToUpper(), true);
                if (File.Exists(path + set.code.ToUpper() + ".mse-set"))
                    ZipFile.ExtractToDirectory(path + set.code.ToUpper() + ".mse-set", path + set.code.ToUpper());
                else
                {
                    if (!Directory.Exists(path + set.code.ToUpper())) Directory.CreateDirectory(path + set.code.ToUpper());
                    if (!File.Exists(path + set.code.ToUpper() + @"\set"))
                        File.AppendAllText(path + set.code.ToUpper() + @"\set",
                            "mse_version: 2.0.2\u000A" +
                            "game: magic" + "\u000A" +
                            "stylesheet: m15-altered" + "\u000A" +
                            "stylesheet_version: 2020-09-04" + "\u000A" +
                            "set_info:" + "\u000A" +
                                "\ttitle: " + set.name + "\u000A" +
                                "\tcopyright: ™ & © Wizards of the Coast" + "\u000A" +
                                "\tset code: " + set.code.ToUpper() + "\u000A" +
                                "\tautomatic card numbers: no" + "\u000A" +
                                "\tcard_language: Russian" + "\u000A" +
                            "styling:" + "\u000A" +
                                "\tmagic-m15-altered:" + "\u000A" +
                                    "\t\tcolor_indicator_dot: no" + "\u000A" +
                                    "\t\ttext_box_mana_symbols: magic-mana-small.mse-symbol-font" + "\u000A" +
                                    "\t\tlevel_mana_symbols: magic-mana-large.mse-symbol-font" + "\u000A" +
                                    "\t\toverlay:" + "\u000A" +
                            "version_control:" + "\u000A" +
                                "\ttype: none" + "\u000A" +
                            "apprentice_code:" + "\u000A");
                }
                if (delSetFile && File.Exists(path + set.code.ToUpper() + ".mse-set")) File.Delete(path + set.code.ToUpper() + ".mse-set");                
            }
            else                        // не трогать имеющиеся файлы или создать новые
            {
                if (!Directory.Exists(path + set.code.ToUpper())) Directory.CreateDirectory(path + set.code.ToUpper());
                if (!File.Exists(path + set.code.ToUpper() + @"\set"))
                    File.AppendAllText(path + set.code.ToUpper() + @"\set",
                        "mse_version: 2.0.2\u000A" +
                        "game: magic" + "\u000A" +
                        "stylesheet: m15-altered" + "\u000A" +
                        "stylesheet_version: 2020-09-04" + "\u000A" +
                        "set_info:" + "\u000A" +
                            "\ttitle: " + set.name + "\u000A" +
                            "\tcopyright: ™ & © Wizards of the Coast" + "\u000A" +
                            "\tset code: " + set.code.ToUpper() + "\u000A" +
                            "\tautomatic card numbers: no" + "\u000A" +
                            "\tcard_language: Russian" + "\u000A" +
                        "styling:" + "\u000A" +
                            "\tmagic-m15-altered:" + "\u000A" +
                                "\t\tcolor_indicator_dot: no" + "\u000A" +
                                "\t\ttext_box_mana_symbols: magic-mana-small.mse-symbol-font" + "\u000A" +
                                "\t\tlevel_mana_symbols: magic-mana-large.mse-symbol-font" + "\u000A" +
                                "\t\toverlay:" + "\u000A" +
                        "version_control:" + "\u000A" +
                            "\ttype: none" + "\u000A" +
                        "apprentice_code:" + "\u000A");
            }       
        }

        public static String GetNumberOfSet(int number, int maxSetCard)
        {
            if (number <= maxSetCard)
                return maxSetCard > 99 ? number.ToString("000") + "/" + maxSetCard.ToString("000") : number.ToString("00") + "/" + maxSetCard.ToString("00");            
            else
                return maxSetCard > 99 ? number.ToString("000") + "              " : number.ToString("00") + "              ";
        }

        static String DelCardFromTextSet(String text, Card card, int maxSetCard)
        {
            var startCard = 0;
            var endCard = 0;
            var res = "";

            while (endCard != text.Length)
            {
                startCard = endCard;
                endCard = (text.IndexOf("card:", startCard + 1)) < 0 ? text.Length : text.IndexOf("card:", startCard + 1);
                
                if (text.Substring(startCard, endCard - startCard).IndexOf(GetNumberOfSet(Convert.ToInt32(card.collector_number), maxSetCard)) < 0)
                    res += text.Substring(startCard, endCard - startCard);
            }           

            return res;
        }

        static void AddCardsToSet(List<Card> cards, int maxSetCard, String path = "", Boolean owerwriteCards = false, Boolean reloadImage = false)
        {
            var file = File.ReadAllText(path + cards[0].set.ToUpper() + @"\set");
            var topFile = "";
            var bottomFile = "";
            var cardsFile = "";
            
            if (file.IndexOf("card:", 0) > 0)
            {
                topFile = file.Substring(0, file.IndexOf("card:", 0));
                cardsFile = file.Substring(file.IndexOf("card:", 0), file.IndexOf("version_control:", 0) - file.IndexOf("card:", 0));
                bottomFile = file.Substring(file.IndexOf("version_control:", 0));
            }
            else
            {
                topFile = file.Substring(0, file.IndexOf("version_control:", 0));
                bottomFile = file.Substring(file.IndexOf("version_control:", 0));
            }
            
            if (owerwriteCards && cardsFile.Length > 0)
                foreach (var card in cards)
                    cardsFile = DelCardFromTextSet(cardsFile, card, maxSetCard);

            foreach (var card in cards)
            {
                cardsFile += GetCardText(card, maxSetCard);
                if (reloadImage)
                    if (File.Exists(path + card.set.ToUpper() + @"\image" + card.collector_number))
                        File.Delete(path + card.set.ToUpper() + @"\image" + card.collector_number);
                if (!File.Exists(path + card.set.ToUpper() + @"\image" + card.collector_number))
                {
                    WebClient client = new WebClient();
                    switch (card.layout)
                    {
                        case "modal_dfc":
                            client.DownloadFile(card.card_faces[0].download_url, path + card.set.ToUpper() + @"\image" + card.card_faces[0].image_name);
                            client.DownloadFile(card.card_faces[1].download_url, path + card.set.ToUpper() + @"\image" + card.card_faces[1].image_name);
                            break;
                        default:
                            client.DownloadFile(card.download_url, path + card.set.ToUpper() + @"\image" + card.collector_number);
                            break;
                    }                    
                } 
            }
                        
            File.Delete(path + cards[0].set.ToUpper() + @"\set");
            File.WriteAllText(path + cards[0].set.ToUpper() + @"\set", topFile + cardsFile + bottomFile);
        }

        /// <summary>
        /// Создать файл сета (zip-архив) из папки с файлами сета
        /// </summary>
        /// <param name="set">Обрабатываемый сет</param>
        /// <param name="path">Путь до файлов сета. По-умолчанию используется папка в папке с исполняющим файлом</param>
        /// <param name="delDirSet">Маркер удаления файлов сета. По-умолчанию false - не удалять файлы сета</param>
        static void CreateFileSet(Set set, String path = "", Boolean delDirSet = false)
        {
            try
            {
                if (File.Exists(path + set.code.ToUpper() + ".mse-set")) File.Delete(path + set.code.ToUpper() + ".mse-set");
                ZipFile.CreateFromDirectory(path + set.code.ToUpper(), path + set.code.ToUpper() + ".mse-set");
                if (delDirSet) Directory.Delete(path + set.code.ToUpper(), true);
            }
            catch
            {
                Console.WriteLine("Файл то закрой...");
                Console.ReadKey();
                if (File.Exists(path + set.code.ToUpper() + ".mse-set")) File.Delete(path + set.code.ToUpper() + ".mse-set");
                ZipFile.CreateFromDirectory(path + set.code.ToUpper(), path + set.code.ToUpper() + ".mse-set");
                if (delDirSet) Directory.Delete(path + set.code.ToUpper(), true);
            }
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            if (args.Length == 0)
            {
                String setCode = "ZNR";
                var set = GetSet(setCode.ToLower());
                set.unique_card = 280;

                Console.WriteLine(set.icon_svg_uri);
                PrepareFiles(set, "", true, false);

                var cards = new List<Card>();

                for (var i = 1; i <= 63; i++)
                    cards.Add(GetCard(setCode, i, "ru"));

                AddCardsToSet(cards, set.unique_card, "", true);
                CreateFileSet(set, "", false);
                Console.WriteLine("тык...");
            }
            Console.ReadLine();
        }
    }
}
