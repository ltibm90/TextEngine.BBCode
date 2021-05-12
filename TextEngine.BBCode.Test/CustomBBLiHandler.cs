using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEngine.BBCode.Test
{
    class CustomBBLiHandler : BBCodeDefaultHandler
    {
        private string endTag = "";
        public override void TagStart()
        {
            var attrib = this.Element.TagAttrib;
            if (string.IsNullOrEmpty(attrib))
            {
                this.Result.Text = "<ul>";
                this.endTag = "</ul>";
            }
            else
            {
                this.Result.Text = $"<ol start={Utils.ToInt32(attrib)}>";
                this.endTag = "</ol>";
            }

            //base.TagStart();
        }


        public override void TagFinish(string textContent)
        {
            this.Result.Text = textContent + endTag;
            //base.TagFinish(textContent);
        }
        public override void OnDataLoaded(dynamic data)
        {
            //base.OnDataLoaded(data);
        }
    }
}
