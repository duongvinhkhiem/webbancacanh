using Microsoft.Owin;
using Owin;
using System.Web.Services.Description;
using WebBanCaCanh.Service;

[assembly: OwinStartupAttribute(typeof(WebBanCaCanh.Startup))]
namespace WebBanCaCanh
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
         
            ConfigureAuth(app);
       
        }
    }
}
