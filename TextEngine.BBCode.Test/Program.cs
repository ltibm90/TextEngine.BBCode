using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine.BBCode.Test
{
    class Program
    {
       static readonly BBCodeEvulator evulator = new BBCodeEvulator();
        static void Main(string[] args)
        {
            //Default BBCode definition
            evulator.PreDefinitions["link"] = "https://www.site.com.tr/link/";
            evulator.SetTag("*", "[{%TagName}]{%Text}[/{%TagName%}]");

            //Set multiple definition
            evulator.SetMultipleTag(new string[] { "b", "i", "u", "s" }, "<{%TagName}>{%Text}</{%TagName}>");
            

            //Set bbcode definition with using Validator(Required TextEngine)
            evulator.SetTag("url", new BBCodeInfo("<a href=\"{%TagAttrib}\">{%Text}</a>").SetValidator(
                (validator, tag) =>
                {
                    
                    string attr = validator.TagData.TagAttrib?.ToString();
                    if (attr == "@cw")
                    {
                        validator.TagData.TagAttrib = "https://www.cyber-warrior.org";
                    }
                    else if(attr == "@site")
                    {
                        validator.TagData.TagAttrib = "https://www.site.com";
                    }
                    
                }
                )
            );
            //set innertext only.
            evulator.SetTag("img", "<img src=\"{%Text}\">", BBCodeInfoFlags.InnerTextOnly);
            evulator.SetTag("size", "<font size=\"{%TagAttrib}\">{%Text}</font>");
            evulator.SetTag("color", "<font color=\"{%TagAttrib}\">{%Text}</font>");
            evulator.SetTag("font", "<font face=\"{%TagAttrib}\">{%Text}</font>");
            evulator.SetTag("center", new BBCodeInfo().SetCustomHandler(typeof(CustomBBHandler)));
            evulator.SetTag("link", "<a href=\"{%link + GetAttribute('id')}\">{%Text}</a>");
            evulator.SetTag("li", new BBCodeInfo().SetCustomHandler(typeof(CustomBBLiHandler)));
            evulator.SetTag("*", "<li>{%Text}</li>");
            //Prevent auto creation for not closed tag.
            evulator.PreventAutoCreation("*");


            //\n to <br />\r\n
            evulator.SetMap('\n', "<br />\r\n");
            //hr tags will closed automaticly.
            evulator.SetAutoClosed("hr");

            evulator.SetTag("hr", "<hr />");
            //Color can be closed with Font
            evulator.SetAlias("color", "font");
            //Size can be closed with Font
            evulator.SetAlias("size", "font");




            ConvertShow("[b]Bold[/b]");
            ConvertShow("[B]Bold [I]Italic[/I] [U]Underline[/U][/B]");
            ConvertShow("[LINK id=12345]Link Text[/LINK]");
            ConvertShow("[SIZE=5]H5[/FONT]");
            ConvertShow("go to [url=http://site.com][B]Site[/B][/url] ");
            ConvertShow("[CENTER]Custom BB Evulator[/CENTER]");
            ConvertShow("[IMG]https://www.site.com/image.jpg[/IMG]");
            ConvertShow("[URL=@site]Site[/URL]");
            ConvertShow("[LI][*]item1[*]item2[*]item3[/LI]");
            ConvertShow("[LI=1][*]item1[*]item2[*]item3[/LI]");
            Console.ReadKey();


        }
        static void ConvertShow(string bbcode)
        {
            Console.WriteLine("BBText: " + bbcode);
            Console.WriteLine("Converted: " + evulator.EvulateBBCode(bbcode));
            Console.WriteLine("-------------------\r\n");
        }
    }
}
