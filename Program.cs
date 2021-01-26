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

    public void Num(int number, int maxSetCard)
    {

        if (number <= maxSetCard)
        {
            if (number <= 9)
                this.collector_number = "00" + number + "/" + maxSetCard;
            else if (number <= 99)
                this.collector_number = "0" + number + "/" + maxSetCard;
            else
                this.collector_number = number + "/" + maxSetCard;
        }
        else
        {
            if (number <= 9)
                this.collector_number = "00" + number + "              ";
            else if (number <= 99)
                this.collector_number = "0" + number + "              ";
            else
                this.collector_number = number + "              ";
        }
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
            //Encoding utf8 = Encoding.GetEncoding("UTF-8");
            //Encoding win1251 = Encoding.GetEncoding(1251);
            //byte[] utf8Bytes;
            //byte[] win1251Bytes;
            Card res = JsonConvert.DeserializeObject<Card>(client.DownloadString("https://api.scryfall.com/cards/" + setCode.ToLower() + "/" + num + "/" + lang));

            try
            {
             //   utf8Bytes = win1251.GetBytes(client.DownloadString("https://api.scryfall.com/cards/" + setCode.ToLower() + "/" + num + "/" + lang));
                //win1251Bytes = Encoding.Convert(utf8, win1251, utf8Bytes);
                //res = JsonConvert.DeserializeObject<Card>(win1251.GetString(win1251Bytes));
                res = JsonConvert.DeserializeObject<Card>(client.DownloadString("https://api.scryfall.com/cards/" + setCode.ToLower() + "/" + num + "/" + lang));
            }
            catch (WebException)
            {
               // utf8Bytes = win1251.GetBytes(client.DownloadString("https://api.scryfall.com/cards/" + setCode.ToLower() + "/" + num));
               // win1251Bytes = Encoding.Convert(utf8, win1251, utf8Bytes);
                //res = JsonConvert.DeserializeObject<Card>(win1251.GetString(win1251Bytes));
                
                try
                {
                  //  utf8Bytes = win1251.GetBytes(client.DownloadString("https://api.scryfall.com/cards/search?q=" + res.name.Replace(" ", "%20") + "%20lang%3Arussian"));
                   // win1251Bytes = Encoding.Convert(utf8, win1251, utf8Bytes);

                    //foreach (Card c in JsonConvert.DeserializeObject<List>(win1251.GetString(win1251Bytes)).data)
                      //  if (c.oracle_id == res.oracle_id)
                        //{
                         //   res.printed_name = c.printed_name;
                          //  res.printed_text = c.printed_text;
                          //  res.flavor_text = c.flavor_text;
                          //  res.printed_type_line = c.printed_type_line;
                          //  res.card_faces = c.card_faces;
                          //  res.lang = c.lang;
                       // }
                }
                catch (WebException)
                {
                    Console.WriteLine("Будет английская");
                    //Console.WriteLine("https://api.scryfall.com/cards/search?q=" + res.name.Replace(" ", "%20") + "%20lang%3Arussian");
                }
            }
            /*
            switch (res.layout)
            {
                case "modal_dfc":
                    //utf8Bytes = win1251.GetBytes(client.DownloadString("https://api.scryfall.com/cards/" + setCode.ToLower() + "/" + num + "/en"));
                    //win1251Bytes = Encoding.Convert(utf8, win1251, utf8Bytes);
                    //res.card_faces[0].download_url = JsonConvert.DeserializeObject<Card>(win1251.GetString(win1251Bytes)).card_faces[0].image_uris.art_crop;
                    res.card_faces[0].image_name = res.collector_number + "a";
                    //res.card_faces[1].download_url = JsonConvert.DeserializeObject<Card>(win1251.GetString(win1251Bytes)).card_faces[1].image_uris.art_crop;
                    res.card_faces[1].image_name = res.collector_number + "b";
                    break;
                default:
                    if (lang != "en")
                    {
                      //  utf8Bytes = win1251.GetBytes(client.DownloadString("https://api.scryfall.com/cards/" + setCode.ToLower() + "/" + num + "/en"));
                        //win1251Bytes = Encoding.Convert(utf8, win1251, utf8Bytes);
                        //res.download_url = JsonConvert.DeserializeObject<Card>(win1251.GetString(win1251Bytes)).image_uris.art_crop;
                    }
                    else
                        res.download_url = res.image_uris.art_crop;
                    break;
            }*/
            //Console.WriteLine(client.DownloadString("https://api.scryfall.com/cards/" + setCode.ToLower() + "/" + num + "/" + lang));
            //Console.WriteLine(res.name);
            return res;
        }

        static String GetText(string str, string oracle_str = null)
        {
            String res = "";
            if (str != null)
            {
                String[] text = str.Split('\n');
                var numStr = (oracle_str != null) ? oracle_str.Split('\n').Length : str.Split('\n').Length;
                if (text != null)
                    for (var i = 0; i < numStr; i++)
                        res += "\t\t" + text[i].Replace("{", "<sym>").Replace("}", "</sym>").Trim() + Environment.NewLine;
            }
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

        static String GetStyleCardString(Card card)
        {
            String res = "\tstylesheet: ";
            switch (card.layout)
            {
                case "modal_dfc":
                    res += card.lang == "ru" ? "m15-mainframe-dfc-ru" : "m15-mainframe-dfc";
                    res += Environment.NewLine;
                    res += "\thas_styling: true" + Environment.NewLine;
                    res += "\tstyling_data:" + Environment.NewLine;
                    res += "\t\tother_options: use hovering pt, use holofoil stamps, unindent nonloyalty abilities, auto nyx crowns" + Environment.NewLine;
                    res += "\t\ttext_box_mana_symbols: magic-mana-small.mse-symbol-font" + Environment.NewLine;
                    res += "\t\tlevel_mana_symbols: magic-mana-large.mse-symbol-font" + Environment.NewLine;
                    res += "\t\toverlay:" + Environment.NewLine;
                    res += "\textra_data:" + Environment.NewLine;
                    res += "\t\tmagic-m15-mainframe-dfc-ru:" + Environment.NewLine;
                    res += "\t\t\tcorner: " + card.card_faces[0].type_line.ToLower().Split('—')[0].Trim() + Environment.NewLine;
                    res += "\t\t\tcorner_2: " + card.card_faces[1].type_line.ToLower().Split('—')[0].Trim() + Environment.NewLine;
                    res += "\tnotes: Создано автоматически" + Environment.NewLine;
                    res += "\ttime_created: " + DateTime.Now.ToString("u").Remove(DateTime.Today.ToString("u").Length - 1) + Environment.NewLine;
                    res += "\ttime_modified: " + DateTime.Now.ToString("u").Remove(DateTime.Today.ToString("u").Length - 1) + Environment.NewLine;
                    break;
                case "adventure":
                    res += "m15-adventure-ru" + Environment.NewLine;
                    res += "\thas styling: true" + Environment.NewLine;
                    res += "\tstyling data:" + Environment.NewLine;
                    res += "\t\tchop main: " + Environment.NewLine;
                    res += "\t\tshrink name text: " + Environment.NewLine;
                    if (card.frame_effects != null)
                        foreach (var str in card.frame_effects)
                            if (str == "showcase") res += "\t\tframes: Spotlight" + Environment.NewLine;
                            else res += "\t\tframes: " + Environment.NewLine;
                    res += "\t\tauto frames: " + Environment.NewLine;
                    res += "\t\tother options: " + Environment.NewLine;
                    res += "\t\ttext box mana symbols: magic-mana-small.mse-symbol-font" + Environment.NewLine;
                    res += "\t\tpromo: no" + Environment.NewLine;
                    res += "\t\toverlay: " + Environment.NewLine;
                    break;
                default:
                    switch (card.type_line.Split('—')[0].Trim())
                    {
                        case "Legendary Planeswalker":
                            res += card.lang == "ru" ? "m15-mainframe-planeswalker-ru" : "m15-mainframe-planeswalker-ru";
                            res += Environment.NewLine;
                            res += "\tnotes: Создано автоматически" + Environment.NewLine;
                            res += "\ttime_created: " + DateTime.Now.ToString("u").Remove(DateTime.Today.ToString("u").Length - 1) + Environment.NewLine;
                            res += "\ttime_modified: " + DateTime.Now.ToString("u").Remove(DateTime.Today.ToString("u").Length - 1) + Environment.NewLine;
                            res += "\thas_styling: true" + Environment.NewLine;
                            res += "\tstyling data:" + Environment.NewLine;
                            res += "\t\tКоличество_полей_способностей: Три" + Environment.NewLine;
                            break;
                        default:
                            res += "m15-altered-ru" + Environment.NewLine;
                            res += "\thas styling: true" + Environment.NewLine;
                            res += "\tstyling data:" + Environment.NewLine;
                            res += "\t\tchop top: 7" + Environment.NewLine;
                            res += "\t\tchop bottom: 7" + Environment.NewLine;

                            if (card.frame_effects != null)
                            {
                                res += "\t\tframes: ";
                                foreach (var str in card.frame_effects)
                                {
                                    if (str == "extendedart") res += "puma, ";
                                    if (str == "legendary") res += "legend, ";
                                    if (str == "inverted") res += "fnm promo, ";
                                }
                                res += "\t\tframes: " + Environment.NewLine;
                            }

                            res += "\t\ttext box mana symbols: magic-mana-small.mse-symbol-font" + Environment.NewLine;
                            res += "\t\tinverted common symbol: no" + Environment.NewLine;
                            res += "\t\toverlay:" + Environment.NewLine;
                            res += "\tnotes: Создано автоматически" + Environment.NewLine;
                            res += "\ttime created: " + DateTime.Now.ToString("u").Remove(DateTime.Today.ToString("u").Length - 1) + Environment.NewLine;
                            res += "\ttime modified: " + DateTime.Now.ToString("u").Remove(DateTime.Today.ToString("u").Length - 1) + Environment.NewLine;
                            break;
                    }                    
                    break;
            }
            /*
            if (card.type_line.IndexOf("Planeswalker") >= 0)
            {
                res += "m15-mainframe-planeswalker-ru" + Environment.NewLine;
                res += "\thas styling: true" + Environment.NewLine;
                res += "\tstyling data:" + Environment.NewLine;
                switch (card.oracle_text.Split('\n').Length)
                {
                    case 4:
                        res += "\t\tuse separate textboxes: four" + Environment.NewLine;
                        break;
                    default:
                        res += "\t\tuse separate textboxes: three" + Environment.NewLine;
                        break;
                }
                res += "\t\ttext box mana symbols: magic-mana-small.mse-symbol-font" + Environment.NewLine;
                res += "\t\toverlay:" + Environment.NewLine;
            }
            */
            return res;
        }

        static String GetColorCardString(Card card)
        {
            String res = "\tcard color: ";

            switch (card.layout)
            {
                case "modal_dfc":
                    res += GetColor(card.card_faces[0].type_line, card.card_faces[0].colors) + Environment.NewLine;
                    res += "\tcard_color_2: " + GetColor(card.card_faces[1].type_line, card.card_faces[1].colors, card.card_faces[1].oracle_text);
                    break;
                default:
                    res += GetColor(card.type_line, card.colors, card.oracle_text);
                    break;
            }
            return res + Environment.NewLine;
        }

        public static String GetNameSting(Card card)
        {
            String res = "\tname: <b>";
            switch (card.layout)
            {
                case "modal_dfc":
                    res += card.lang == "en" ? card.card_faces[0].name : card.card_faces[0].printed_name;
                    res += "</b>" + Environment.NewLine;
                    res += "\tname_2: <b>";
                    res += card.lang == "en" ? card.card_faces[1].name : card.card_faces[1].printed_name;
                    res += "</b>" + Environment.NewLine;
                    break;
                case "adventure":
                    res += card.lang == "en" ? card.card_faces[0].name : card.card_faces[0].printed_name;
                    res += "</b>" + Environment.NewLine;
                    break;
                default:
                    res += card.lang == "en" ? card.name : card.printed_name;
                    res += "</b>" + Environment.NewLine;
                    break;
            }
            return res;
        }

        static String GetCastingCost(Card card)
        {
            switch (card.layout)
            {
                case "modal_dfc":
                    return "\tcasting_cost: " + card.card_faces[0].mana_cost + Environment.NewLine +
                        "\tcasting_cost_2: " + card.card_faces[1].mana_cost + Environment.NewLine;
                default:
                    return "\tcasting_cost: " + card.mana_cost + Environment.NewLine;
            }
        }

        static String GetTypesString(Card card)
        {
            String res = "";
            switch (card.layout)
            {
                case "modal_dfc":
                    res += "\tsuper type: <b>";
                    res += card.lang == "en" ? card.card_faces[0].type_line.Split('—')[0].Trim() : card.card_faces[0].printed_type_line.Split('—')[0].Trim();
                    res += "</b>" + Environment.NewLine;
                    if (card.card_faces[0].type_line.Split('—').Length > 1)
                    {
                        res += "\tsub type: <b>";
                        res += card.lang == "en" ? card.card_faces[0].type_line.Split('—')[1].Trim() : card.card_faces[0].printed_type_line.Split('—')[1].Trim();
                        res += "</b>" + Environment.NewLine;
                    }
                    res += "\tsuper type 2: <b>";
                    res += card.lang == "en" ? card.card_faces[1].type_line.Split('—')[0].Trim() : card.card_faces[1].printed_type_line.Split('—')[0].Trim();
                    res += "</b>" + Environment.NewLine;
                    if (card.card_faces[1].type_line.Split('—').Length > 1)
                    {
                        res += "\tsub type 2: <b>";
                        res += card.lang == "en" ? card.card_faces[1].type_line.Split('—')[1].Trim() : card.card_faces[1].printed_type_line.Split('—')[1].Trim();
                        res += "</b>" + Environment.NewLine;
                    }
                    break;
                case "adventure":
                    res += "\tsuper type: <b>";
                    res += card.lang == "en" ? card.card_faces[0].type_line.Split('—')[0].Trim() : card.card_faces[0].printed_type_line.Split('—')[0].Trim();
                    res += "</b>" + Environment.NewLine;
                    if (card.card_faces[0].type_line.Split('—').Length > 1)
                    {
                        res += "\tsub type: <b>";
                        res += card.lang == "en" ? card.card_faces[0].type_line.Split('—')[1].Trim() : card.card_faces[0].printed_type_line.Split('—')[1].Trim();
                        res += "</b>" + Environment.NewLine;
                    }
                    break;
                default:
                    res += "\tsuper type: <b>";
                    res += card.lang == "en" ? card.type_line.Split('—')[0].Trim() : card.printed_type_line.Split('—')[0].Trim();
                    res += "</b>" + Environment.NewLine;
                    if (card.type_line.Split('—').Length > 1)
                    {
                        res += "\tsub type: <b>";
                        res += card.lang == "en" ? card.type_line.Split('—')[1].Trim() : card.printed_type_line.Split('—')[1].Trim();
                        res += "</b>" + Environment.NewLine;
                    }
                    break;
            }
            return res;
        }

        static String GetRarityString(String str)
        {
            if (str == "mythic")
                return "\trarity: mythic rare" + Environment.NewLine;
            else
                return "\trarity: " + str + Environment.NewLine;
        }

        static String GetRuleTextString(Card card)
        {
            String rule = "";
            Console.WriteLine(card.type_line);
            Console.WriteLine(card.printed_name);
            switch (card.layout)
            {
                case "modal_dfc":
                    rule += "\trule_text:" + Environment.NewLine;
                    rule += card.lang == "en" ? 
                        GetText(card.card_faces[0].oracle_text) : 
                            GetText(card.card_faces[0].printed_text, card.card_faces[0].oracle_text);
                    rule += "\trule_text_2:" + Environment.NewLine;
                    rule += card.lang == "en" ? GetText(card.card_faces[1].oracle_text) : GetText(card.card_faces[1].printed_text, card.card_faces[1].oracle_text);
                    break;
                default:
                    switch (card.type_line.Split('—')[0].Trim())
                    {
                        case "Legendary Planeswalker":
                            rule += "\tspecial_text:" + Environment.NewLine;
                            rule += card.lang == "en" ? GetText(card.oracle_text) : GetText(card.printed_text);
                            var level = 1;
                            var first = true;
                            foreach (var str in GetText(card.printed_text).Split('\n'))
                            {
                                string loyalty_cost = null;
                                string loyalty_text = null;
                                if (str.Split(':').Length > 1)
                                {
                                    loyalty_cost = str.Split(':')[0];
                                    for (var i = 1; i < str.Split(':').Length; i++) loyalty_text += str.Split(':')[i].Trim();
                                }
                                else
                                    loyalty_text = str;

                                if (first == true && loyalty_cost == null)
                                {
                                    first = false;
                                    rule += "\tlevel_" + level + "_text: " + Environment.NewLine;
                                    rule += "\t\t" + loyalty_text + Environment.NewLine;
                                }
                                else
                                {
                                    if (loyalty_cost != null)
                                    {
                                        level++;
                                        rule += "\tloyalty_cost_" + level + ": " + loyalty_cost + Environment.NewLine;
                                        rule += "\tlevel_" + level + "_text: " + Environment.NewLine;
                                        rule += "\t\t" + loyalty_text + Environment.NewLine;
                                    }
                                    else
                                    {
                                        rule += "\t\t" + loyalty_text + Environment.NewLine;
                                    }
                                }
                            }
                            Console.WriteLine(rule);
                            break;
                        default:
                            rule += "\trule_text:" + Environment.NewLine;
                            rule += card.lang == "en" ? GetText(card.oracle_text) : GetText(card.printed_text);
                            break;
                    }
                    break;
            }
            return rule;
        }

        static String GetFlavorTextString(Card card)
        {
            String res = "\tflavor text:" + Environment.NewLine;
            switch (card.layout)
            {
                case "modal_dfc":
                    if (card.card_faces[0].flavor_text != null)
                        foreach (var a in card.card_faces[0].flavor_text.Split('\n'))
                            res += "\t\t" + a.Replace("*", "</i>") + Environment.NewLine;
                    if (card.card_faces[1].flavor_text != null)
                    {
                        res += "\tflavor_text_2:" + Environment.NewLine;
                        foreach (var a in card.card_faces[1].flavor_text.Split('\n'))
                            res += "\t\t" + a.Replace("*", "</i>") + Environment.NewLine;
                    }
                    break;
                default:
                    if (card.flavor_text != null)
                        foreach (var a in card.flavor_text.Split('\n'))
                            res += "\t\t" + a.Replace("*", "</i>") + Environment.NewLine;
                    break;
            }
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
            if (res == "") return null; else return res + Environment.NewLine; 
        }
               
        static String GetCardText(Card card, int maxSetCard)
        {
            Console.WriteLine(GetNumberOfSet(Convert.ToInt32(card.collector_number), maxSetCard));
            String res = "card:" + Environment.NewLine;
            res += GetStyleCardString(card);
            res += GetColorCardString(card);
            res += GetNameSting(card);
            res += GetCastingCost(card);
            switch (card.layout)
            {
                case "modal_dfc":
                    res += "\timage: image" + card.card_faces[0].image_name + Environment.NewLine;
                    res += "\timage_2: image" + card.card_faces[1].image_name + Environment.NewLine;
                    break;
                default:
                    res += "\timage: image" + card.collector_number + Environment.NewLine;
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
                    if (card.card_faces[0].power != null) res += "\tpower: <b>" + card.card_faces[0].power + Environment.NewLine;
                    if (card.card_faces[0].toughness != null) res += "\ttoughness: <b>" + card.card_faces[0].toughness + Environment.NewLine;
                    if (card.card_faces[1].power != null) res += "\tpower_2: <b>" + card.card_faces[1].power + Environment.NewLine;
                    if (card.card_faces[1].toughness != null) res += "\ttoughness_2: <b>" + card.card_faces[1].toughness + Environment.NewLine;
                    break;
                default:
                    if (card.power != null) res += "\tpower: <b>" + card.power + "</b>" + Environment.NewLine;
                    if (card.toughness != null) res += "\ttoughness: " + card.toughness + Environment.NewLine;
                    break;
            }
            if (card.loyalty != null) res += "\tloyalty: " + card.loyalty + Environment.NewLine;
            res += "\tcustom card number: " + GetNumberOfSet(Convert.ToInt32(card.collector_number), maxSetCard) + Environment.NewLine;
            res += "\tillustrator: " + card.artist + Environment.NewLine;
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
                            "mse_version: 2.0.2" + Environment.NewLine +
                            "game: magic" + Environment.NewLine +
                            "stylesheet: m15-altered" + Environment.NewLine +
                            "stylesheet_version: 2020-09-04" + Environment.NewLine +
                            "set_info:" + Environment.NewLine +
                                "\ttitle: " + set.name + Environment.NewLine +
                                "\tcopyright: ™ & © Wizards of the Coast" + Environment.NewLine +
                                "\tset code: " + set.code.ToUpper() + Environment.NewLine +
                                "\tautomatic card numbers: no" + Environment.NewLine +
                                "\tcard_language: Russian" + Environment.NewLine +
                            "styling:" + Environment.NewLine +
                                "\tmagic-m15-altered:" + Environment.NewLine +
                                    "\t\tcolor indicator dot: no" + Environment.NewLine +
                                    "\t\tgrey hybrid name: yes" + Environment.NewLine +
                                    "\t\ttext box mana symbols: magic-mana-small.mse-symbol-font" + Environment.NewLine +
                                    "\t\toverlay:" + Environment.NewLine +
                            "version_control:" + Environment.NewLine +
                                "\ttype: none" + Environment.NewLine +
                            "apprentice_code:" + Environment.NewLine);
                }
                if (delSetFile && File.Exists(path + set.code.ToUpper() + ".mse-set")) File.Delete(path + set.code.ToUpper() + ".mse-set");                
            }
            else                        // не трогать имеющиеся файлы или создать новые
            {
                if (!Directory.Exists(path + set.code.ToUpper())) Directory.CreateDirectory(path + set.code.ToUpper());
                if (!File.Exists(path + set.code.ToUpper() + @"\set"))
                    File.AppendAllText(path + set.code.ToUpper() + @"\set",
                        "mse_version: 2.0.2" + Environment.NewLine +
                        "game: magic" + Environment.NewLine +
                        "stylesheet: m15-altered" + Environment.NewLine +
                        "stylesheet_version: 2020-09-04" + Environment.NewLine +
                        "set_info:" + Environment.NewLine +
                            "\ttitle: " + set.name + Environment.NewLine +
                            "\tcopyright: ™ & © Wizards of the Coast" + Environment.NewLine +
                            "\tset code: " + set.code.ToUpper() + Environment.NewLine +
                            "\tautomatic card numbers: no" + Environment.NewLine +
                            "\tcard_language: Russian" + Environment.NewLine +
                        "styling:" + Environment.NewLine +
                            "\tmagic-m15-altered:" + Environment.NewLine +
                                "\t\tcolor indicator dot: no" + Environment.NewLine +
                                "\t\tgrey hybrid name: yes" + Environment.NewLine +
                                "\t\ttext box mana symbols: magic-mana-small.mse-symbol-font" + Environment.NewLine +
                                "\t\toverlay:" + Environment.NewLine +
                        "version_control:" + Environment.NewLine +
                            "\ttype: none" + Environment.NewLine +
                        "apprentice_code:" + Environment.NewLine);
            }       
        }

        public static String GetNumberOfSet(int number, int maxSetCard)
        {
            if (number <= maxSetCard)
            {
                if (number <= 9)
                    return "00" + number + "/" + maxSetCard;
                else if (number <= 99)
                    return "0" + number + "/" + maxSetCard;
                else
                    return number + "/" + maxSetCard;
            }
            else
            {
                if (number <= 9)
                    return "00" + number + "              ";
                else if (number <= 99)
                    return "0" + number + "              ";
                else
                    return number + "              ";
            }
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
            if (File.Exists(path + set.code.ToUpper() + ".mse-set")) File.Delete(path + set.code.ToUpper() + ".mse-set");
            ZipFile.CreateFromDirectory(path + set.code.ToUpper(), path + set.code.ToUpper() + ".mse-set");
            if (delDirSet) Directory.Delete(path + set.code.ToUpper(), true);
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                String setCode = "ZNR";
                var set = GetSet(setCode.ToLower());
                set.unique_card = 280;

                Console.WriteLine(set.icon_svg_uri);
                PrepareFiles(set, "", true, false);

                var cards = new List<Card>();

                for (var i = 63; i <= 63; i++)
                    cards.Add(GetCard(setCode, i, "ru"));
                
                AddCardsToSet(cards, set.unique_card, "", true);
                CreateFileSet(set, "", false);
                Console.WriteLine("тык...");
            }
            Console.ReadLine();
        }
    }
}
