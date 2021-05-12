using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine.Evulator;
using TextEngine.Text;
using Microsoft.CSharp.RuntimeBinder;
namespace TextEngine.BBCode
{
    public class BBCodeGeneralEvulator : BaseEvulator
    {
        private BBCodeDefaultHandler Handler { get; set; }
        private BBCodeEvulator BBCodeEvulator { get; set; }
        private dynamic GetData(TextElement tag)
        {
            IDictionary<string, object> tagVars = new ExpandoObject();
            foreach (var item in this.BBCodeEvulator.PreDefinitions)
            {
                tagVars[item.Key] = item.Value;
            }
            this.Handler.OnDataLoaded(tagVars);
            return tagVars;
        }
        public override TextEvulateResult Render(TextElement tag, object vars)
        {
            this.BBCodeEvulator = this.Evulator.CustomDataSingle as BBCodeEvulator;

            TextEvulateResult result = new TextEvulateResult()
            {
                Result = TextEvulateResultEnum.EVULATE_DEPTHSCAN
            };
            if (tag.AutoClosed && tag.SubElementsCount == 0) return result;
            var bbcodeInfo = this.BBCodeEvulator.GetTag(tag.ElemName);
            if(bbcodeInfo == null || !bbcodeInfo.Enabled)
            {
                return result;
            }

            Type defaultHandlertype = typeof(BBCodeDefaultHandler);
            if(bbcodeInfo.CustomHandler != null)
            {
               defaultHandlertype = bbcodeInfo.CustomHandler;
            }
            this.Handler = Activator.CreateInstance(defaultHandlertype) as BBCodeDefaultHandler;
            this.Handler.BBCodeInfo = bbcodeInfo;
            this.Handler.Element = tag;
            this.Handler.TagVars = vars;
            this.Handler.TagStart();
            if(this.Handler.Result.Handled)
            {
                var data = this.GetData(tag);
                data.Text = this.Handler.Result.Text;
                result.Result = TextEvulateResultEnum.EVULATE_TEXT;
                var validateResult = bbcodeInfo.Validate(data, tag);
                if(validateResult != null && validateResult.Cancel)
                {
                    result.TextContent = null;
                }
                else
                {
                    result.TextContent = bbcodeInfo.TagFormat.Apply(data);
                }
            }
            else
            {
                result.TextContent = this.Handler.Result.Text;
            }
            return result;
        }
        public override void RenderFinish(TextElement tag, object vars, TextEvulateResult latestResult)
        {
            if (this.Handler == null)
            {
                //default 
                if (tag.AutoClosed) 
                    latestResult.TextContent = "[" + tag.ElemName + "]";
                else
                    latestResult.TextContent = "[" + tag.ElemName + "]" + latestResult.TextContent + "[/" + tag.ElemName + "]";
                return;
            }
            if(this.Handler.Result.Handled)
            {
                base.RenderFinish(tag, vars, latestResult);
                return;
            }
            //Not continue if printed above.
            else
            {
                this.Handler.TagFinish(latestResult.TextContent);
                var data = this.GetData(tag);
                data.Text = this.Handler.Result.Text;
                var validateResult = this.Handler.BBCodeInfo.Validate(data, tag);
                if(validateResult != null && validateResult.Cancel)
                {
                    latestResult.TextContent = null;
                }
                else
                {
                    latestResult.TextContent = this.Handler.BBCodeInfo.TagFormat.Apply(data);
                }
            }
            base.RenderFinish(tag, vars, latestResult);
        }

    }
}
