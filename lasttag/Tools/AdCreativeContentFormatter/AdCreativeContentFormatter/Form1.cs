using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AdCreativeContentFormatter.AdFalcon;

namespace AdCreativeContentFormatter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            var adcreativeunit = new AdCreativeUnit()
                {
                    Content = txtContent.Text
                };

            var adCreativeContentFormatter = AdCreativeContentFormatterBase.GetAdCreativeContentFormatter(adcreativeunit);
            adCreativeContentFormatter.FormatContent();
            txtResult.Text = adcreativeunit.Content;
        }

        private void cbxSampleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtResult.Text = "";
            switch (cbxSampleType.SelectedIndex)
            {
                case 0:
                    {
                        txtContent.Text = "";
                        break;
                    }
                case 1:
                    {
                        txtContent.Text = string.Format(@"<script src={0}mraid.js{0}></script>
<script>
(function() {{
    var c = '';
    var u = 'http://api.celtra.com/v1/creatives/6415/compiled/ExpandableBanner/mraid-inapp.js?channel=ROS&c='+encodeURIComponent(c)+'&rnd='+(Math.random()+'').slice(2);
    document.write('<script src={0}'+u+'{0}></scr'+'ipt>');
}})();
</script>", "\"");
                        break;
                    }
                case 3:
                    {
                        txtContent.Text = string.Format(@"<div class={0}celtra-ad-v2{0}>
    <script>
        (function() {{
            var c = {0}{0};
            var sticky = {0}{0};

            var scripts = document.getElementsByTagName({0}script{0});
            var me = scripts[scripts.length-1];

            var req = document.createElement({0}script{0});
            req.id = {0}celtra-{0}+(Math.random()+{0}{0}).slice(2);
            req.src = {0}http://api.celtra.com/v1/creatives/6415/compiled/ExpandableBanner/banner.js?channel=ROS&c={0}+encodeURIComponent(c)+{0}&scriptId={0}+req.id+{0}&sticky={0}+sticky;
            me.parentNode.insertBefore(req, me.nextSibling);
        }})();
    </script>
</div>", "\"");
                        break;
                    }
                case 4:
                    {
                        txtContent.Text = string.Format(@"<script src={0}mraid.js{0}></script>
<div class={0}celtra-ad-v3{0}>
    <img src={0}data:image/png,celtra{0} style={0}display: none{0} onerror={0}
        (function(img) {{
            var params = {{'channelId':'cb3cba87','clickUrl':'{{CLICK_URL}}','clickUrlNeedsDest':'1','externalAdServer':'Custom'}};
            var req = document.createElement('script');
            req.id = params.scriptId = 'celtra-script-' + (window.celtraScriptIndex = (window.celtraScriptIndex||0)+1);
            params.clientTimestamp = new Date/1000;
            req.src = (window.location.protocol == 'https:' ? 'https' : 'http') + '://ads.celtra.com/6d8ef8ed/mraid-ad.js?';
            for (var k in params) {{
                req.src += '&amp;' + encodeURIComponent(k) + '=' + encodeURIComponent(params[k]);
            }}
            img.parentNode.insertBefore(req, img.nextSibling);
        }})(this);
    {0}/>
</div>", "\"");
                        break;
                    }
                case 5:
                    {
                        txtContent.Text = string.Format(@"<div class={0}celtra-ad-v3{0}>
    <img src={0}data:image/png,celtra{0} style={0}display: none{0} onerror={0}
        (function(img) {{
            var params = {{'channelId':'cb3cba87','clickUrl':'{{CLICK_URL}}','clickUrlNeedsDest':'1','externalAdServer':'Custom'}};
            var req = document.createElement('script');
            req.id = params.scriptId = 'celtra-script-' + (window.celtraScriptIndex = (window.celtraScriptIndex||0)+1);
            params.clientTimestamp = new Date/1000;
            req.src = (window.location.protocol == 'https:' ? 'https' : 'http') + '://ads.celtra.com/6d8ef8ed/web.js?';
            for (var k in params) {{
                req.src += '&amp;' + encodeURIComponent(k) + '=' + encodeURIComponent(params[k]);
            }}
            img.parentNode.insertBefore(req, img.nextSibling);
        }})(this);
    {0}/>
</div>", "\"");
                        break;
                    }
                case 6:
                    {
                        txtContent.Text = string.Format(@"<script type={0}text/javascript{0}>
var ord = Number(ord) || Math.floor(Math.random()*10e12);
document.write('<a href={0}http://ad.doubleclick.net/Nnetwork_code/jump/first_level_ad_unit/second_level_ad_unit;pos=top;tile=tile_number;sz=widthxheight;ord=' + ord + '?{0} target={0}_blank{0}><img src={0}http://ad.doubleclick.net/Nnetwork_code/ad/first_level_ad_unit/second_level_ad_unit;pos=top;tile=tile_number;sz=widthxheight;ord=' + ord + '?{0} /></a>');
</script>
<noscript>
<a href={0}http://ad.doubleclick.net/Nnetwork_code/jump/first_level_ad_unit/second_level_ad_unit;pos=top;tile=tile_number;sz=widthxheight;ord=1234567890?{0} target={0}_blank{0}>
<img src={0}http://ad.doubleclick.net/Nnetwork_code/ad/first_level_ad_unit/second_level_ad_unit;pos=top;tile=tile_number;sz=widthxheight;ord=1234567890?{0} />
</a>
</noscript>", "\"");
                        break;
                    }
                case 7:
                    {
                        txtContent.Text = string.Format(@"<a href={0}http://ad.doubleclick.net/Nnetwork_code/jump/first_level_ad_unit/second_level_ad_unit;pos=top;tile=tile_number;sz=widthxheight;ord=1234567890?{0} target={0}_blank{0}>
<img src={0}http://ad.doubleclick.net/Nnetwork_code/ad/first_level_ad_unit/second_level_ad_unit;pos=top;tile=tile_number;sz=widthxheight;ord=1234567890?{0} />
</a>", "\"");
                        break;
                    }
                case 8:
                    {
                        txtContent.Text = string.Format(@"<SCRIPT LANGUAGE={0}JavaScript{0} SRC={0}http://ad.doubleclick.net/adj/N2222.moceanmobile/B123123.1;sz=300x50;pc=[TPAS_ID];click=;ord=?{0}>
</SCRIPT>
<NOSCRIPT>
<A HREF={0}http://ad.doubleclick.net/jump/N2222.moceanmobile/B123123.1;sz=300x50;pc=[TPAS_ID];ord=?{0} TARGET={0}_blank{0}>
<IMG SRC={0}http://ad.doubleclick.net/ad/N2222.moceanmobile/B123123.1;sz=300x50;pc=[TPAS_ID];ord=?{0} WIDTH={0}300{0} HEIGHT={0}50{0} BORDER={0}0{0} ALT={0}ad{0}>
</A>
</NOSCRIPT>", "\"");
                        break;
                    }
                case 9:
                    {
                        txtContent.Text = string.Format(@"<div id='crispZone3962'></div><script type='text/javascript'>(function(){{try{{var v=(inDapIF===true)?self.parent:self;v.name;}}catch(e){{v=window;}}v.__CrispApiUrl='http://api.crispadvertising.com/';v._cevu='%VIEWURL%';v._cecu='%CLICKURL%';v._ceau='%ACTURL%';v._cexp='%PARAM%';var s='http://cdn2.crispadvertising.com/afw/2.2/framework/client/adrequest.js',r=Math.random()*9999,p={{partnerkey:'afa1a1efc4977cc8bc83a8fe6a952a39',ver:'2.2',cb:Math.floor(r),zid:'3962',publisherid:'647'}},d=document,m=d.getElementById('crispZone3962'),f=function(j){{j.onloadDone=true;v.crisploader.loader(p,j)}},j=d.createElement('script');j.setAttribute('type','text/javascript');j.setAttribute('src',s);j.setAttribute('id','loader'+r);j.setAttribute('defer','defer');j.onload=function(){{if(!j.onloadDone){{f(j)}}}};j.onreadystatechange=function(){{if('loaded'===j.readyState){{if(!j.onloadDone){{f(j)}}}}}};var iFF=null;try{{if(self.parent&&inDapIF===true){{var tw=self.parent;var iframes=tw.document.getElementsByTagName('iframe');for(var k=0;k<iframes.length;k++){{var iFe=iframes[k];if(iFe.contentWindow==self){{iFF=iFe}}}}}}if(iFF){{if(iFF.parentNode){{iFF.parentNode.insertBefore(j,iFF);iFF.width=0;iFF.height=0;iFF.style.width='0px';iFF.style.height='0px';iFF.style.visibility='hidden'}}else{{throw e;}}}}else{{throw e;}}}}catch(e){{if(m.parentNode)m.parentNode.appendChild(j)}}}}());</script><noscript><img src='http://api.crispadvertising.com/adRequest/control/noscript.gif?partnerkey=afa1a1efc4977cc8bc83a8fe6a952a39&amp;zid=3962&amp;publisherid=647' style='display:none;' width='5' height='5'/></noscript><img src='http://api.crispadvertising.com/adRequest/control/ad.gif?partnerkey=afa1a1efc4977cc8bc83a8fe6a952a39&amp;zid=3962&amp;publisherid=647' style='display:none;' width='5' height='5'/>", "\"");
                        break;
                    }
            }
        }
    }
}
