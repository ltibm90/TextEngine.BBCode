# TextEngine.BBCode
BBCode converter extensions for TextEngine


## Nuget 

**Requirements**
[Text Engine](https://www.nuget.org/packages/TextEngine)

or

[Text Engine](https://www.nuget.org/packages/TextEngine.x86)

[Microsoft.CSharp](https://www.nuget.org/packages/Microsoft.CSharp/)


**Installation**

Install-Package TextEngine.BBCode

For **x86**

Install-Package TextEngine.BBCode.x86

[Text Engine.BBCode](https://www.nuget.org/packages/TextEngine.BBCode)

[Text Engine.BBCode x86](https://www.nuget.org/packages/TextEngine.BBCode.x86)



# Usage

## Simple Usage
```csharp
          BBCodeEvulator evulator = new BBCodeEvulator();
          //Default tag output format.
          evulator.SetTag("*", "[{%TagName}]{%Text}[/{%TagName%}]");
          
          //B tag format
          evulator.SetTag("b", "<b>{%Text}</b>");
          
          //Multiple tag format.
          evulator.SetMultipleTag(new string[] { "i", "u", "s" }, "<{%TagName}>{%Text}</{%TagName}>");
          
          evulator.SetTag("test", "{%GetAttribute('name')} {%Text}");
          
          //\n to <br />\r\n
          evulator.SetMap('\n', "<br />\r\n");
          
          //Output: <b><i>Bold</i></b>
          string converted = evulator.EvulateBBCode("[B][I]Bold[I][/B]");
          
          //Output: name Content text
          string converted2 = evulator.EvulateBBCode("[test name=name]Content text[/test]");
          
```


## With Validator
```csharp
          BBCodeEvulator evulator = new BBCodeEvulator();
          
          evulator.SetTag("url", new BBCodeInfo("<a href=\"{%TagAttrib}\">{%Text}</a>").SetValidator(
                (validator, tag) =>
                {
                    string attr = validator.TagData.TagAttrib?.ToString();
                    if (attr == "@cw")
                    {
                        //Change attribute value.
                        validator.TagData.TagAttrib = "https://www.cyber-warrior.org";
                    }
                    else if(attr == "@site")
                    {
                        //Change attribute value.
                        validator.TagData.TagAttrib = "https://www.site.com";
                    }
                }
                )
            );
          
          //Output: <a href="https://www.cyber-warrior.org">Cyber-Warrior.Org</a>
          string converted = evulator.EvulateBBCode("[url=@cw]Cyber-Warrior.Org[/url]");
```

## With Custom Handler

**CenterBBHandler** class
```csharp
    public class CenterBBHandler : BBCodeDefaultHandler
    {
        public override void TagStart()
        {
            this.Result.Text = "<div style='text-align: center'>";
            //base.TagStart();
        }
        public override void TagFinish(string textContent)
        {
            this.Result.Text = textContent + "</div>";
            //base.TagFinish(textContent);
        }
        public override void OnDataLoaded(dynamic data)
        {
            //Prefent to call base data loaded.
            //data.TagAttrib = "Attrib";
            //base.OnDataLoaded(data);
        }
    }
```


```csharp
          
    BBCodeEvulator evulator = new BBCodeEvulator();     
     evulator.SetTag("center", new BBCodeInfo().SetCustomHandler(typeof(CenterBBHandler)));
    //Output: <div style='text-align: center'>Centered</div>
    string converted = evulator.EvulateBBCode("[center]Centered[/center]");
```
