using System;
using System.Collections.Generic;
namespace TextEngine.BBCode
{
    public class BBCodeDefaultHandler
    {
        public BBCodeInfo BBCodeInfo { get; internal set; }
        public TextEngine.Text.TextElement Element { get; internal set; }
        public object TagVars { get; internal set; }
        public BBCodeHandlerResult Result { get; protected set; }
        public BBCodeDefaultHandler()
        {
            this.Result = new BBCodeHandlerResult();
        }
        public virtual void TagStart()
        {
               
            if(this.BBCodeInfo.Flags.HasFlag(BBCodeInfoFlags.InnerTextOnly))
            {
                this.Result.Text = this.Element.InnerText();
                this.Result.Handled = true;
            }
        }
        public virtual void TagFinish(string textContent)
        {
            this.Result.Text = textContent;
        }
        public virtual void OnDataLoaded(dynamic data)
        {
            data.TagAttrib = this.Element.TagAttrib;
            data.TagName = this.Element.ElemName.ToLowerInvariant();
            data.GetAttribute = (Func<string, object>)delegate (string name)
            {
                return this.Element.ElemAttr.GetAttribute(name);
            };
        }
    }
}
