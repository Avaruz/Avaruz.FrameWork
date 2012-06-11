using System;
using System.Web.UI.WebControls;

namespace Avaruz.FrameWork.Controls.Web
{
    public partial class DoAfterPostback : WebControl
    {
        public string DoAfterPostbackJavaScript { get; set; }

        private HiddenField DoAfterPostbackJavaScriptHiddenField = new HiddenField();

        protected override void OnInit(EventArgs e)
        {
            DoAfterPostbackJavaScriptHiddenField.ID = "DoAfterPostbackJavaScriptHiddenField";
            this.Controls.Add(DoAfterPostbackJavaScriptHiddenField);
            Page.ClientScript.RegisterStartupScript(
            this.GetType(),
            "DoAfterPostbackJavaScriptEngine",
            @"
var prm = Sys.WebForms.PageRequestManager.getInstance();
prm.add_endRequest(function(s, e) {
$('[id$=_DoAfterPostbackJavaScriptHiddenField]').each(doPostbackJS);
}); 

function doPostbackJS(i){
eval(this.value);
}

", true);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            DoAfterPostbackJavaScriptHiddenField.Value = DoAfterPostbackJavaScript;
        }
    }
}