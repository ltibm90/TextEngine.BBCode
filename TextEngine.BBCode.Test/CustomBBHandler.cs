using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEngine.Evulator;
using TextEngine.Text;

namespace TextEngine.BBCode.Test
{
    public class CustomBBHandler : BBCodeDefaultHandler
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
            //base.OnDataLoaded(data);
        }
    }
}