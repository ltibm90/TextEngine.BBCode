using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine.Text;

namespace TextEngine.BBCode
{
    public class BBCodeEvulator
    {
        private Dictionary<string, BBCodeInfo> BBCodes { get; set; }
        public Dictionary<string, object> PreDefinitions { get; private set; }
        private readonly TextEvulator evulator;

        public BBCodeInfo SetMultipleTag(string[] bbcodes, string content)
        {
            BBCodeInfo info = new BBCodeInfo(content);
            this.SetMultipleTag(bbcodes, info);
            return info;
        }

        /// <summary>
        /// Set multiple tags definition
        /// </summary>
        /// <param name="bbcodes">BBCodes</param>
        /// <param name="info">BBCode Info</param>
        public void SetMultipleTag(string[] bbcodes, BBCodeInfo info)
        {
            for (int i = 0; i < bbcodes.Length; i++)
            {
                this.SetTag(bbcodes[i], info);
            }
        }
        /// <summary>
        /// Set single tag definition, with string
        /// </summary>
        /// <param name="bbcode">BBCode</param>
        /// <param name="info">BBCode Info</param>
        public BBCodeInfo SetTag(string bbcode, string content, BBCodeInfoFlags flags = 0)
        {
            if (string.IsNullOrEmpty(bbcode)) return null;
            BBCodeInfo info = new BBCodeInfo(content, flags);
            this.SetTag(bbcode, info);
            return info;
        }

        /// <summary>
        /// Set single tag definition
        /// </summary>
        /// <param name="bbcode">BBCode</param>
        /// <param name="info">BBCode Info</param>
        public void SetTag(string bbcode, BBCodeInfo info)
        {
            this.BBCodes[bbcode] = info;
        }
        public BBCodeEvulator()
        {
            //Case insensivite
            this.BBCodes = new Dictionary<string, BBCodeInfo>(StringComparer.OrdinalIgnoreCase);
            evulator = new TextEvulator();
            //Clear default evulator options
            evulator.ClearAllInfos();
            //Set evulator custom data.
            evulator.CustomDataSingle = this;

            //Disable lastslash character(e.g [TEST /])
            evulator.TagInfos["*"].Flags = TextElementFlags.TEF_DisableLastSlash;
            evulator.EvulatorTypes.GeneralType = typeof(BBCodeGeneralEvulator);
            evulator.LeftTag = '[';
            evulator.RightTag = ']';
            evulator.SurpressError = true;
            evulator.AllowCharMap = true;
            this.PreDefinitions = new Dictionary<string, object>();
     

        }
        /// <summary>
        /// Convert bbcode to definitons.
        /// </summary>
        /// <param name="bbcodetext">BBCode to convert.</param>
        /// <returns></returns>
        public string EvulateBBCode(string bbcodetext)
        {
            evulator.Text = bbcodetext;
            evulator.ClearElements();
            evulator.Parse();
            var result = evulator.Elements.EvulateValue();
            return result?.TextContent;
        }
        /// <summary>
        /// Set tag closing alias.
        /// </summary>
        /// <param name="name">Source</param>
        /// <param name="target">Target</param>
        public void SetAlias(string name, string target)
        {
            evulator.Aliasses.Add(name, target);
        }
        public BBCodeInfo GetTag(string bbcode)
        {
            if (this.BBCodes.TryGetValue(bbcode, out BBCodeInfo info)) return info;
            //Find default tag...
            this.BBCodes.TryGetValue("*", out info);
            return info;

        }

        public void SetAutoClosed(string tagname)
        {
            this.evulator.TagInfos[tagname].Flags |= TextElementFlags.TEF_AutoClosedTag;
        }
        public void SetMap(char cur, string target)
        {
            this.evulator.CharMap[cur] = target;
        }
    }
}
