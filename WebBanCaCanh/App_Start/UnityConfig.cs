using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Unity.Mvc5;
using WebBanCaCanh.Models;
using WebBanCaCanh.Service;

namespace WebBanCaCanh
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            // Đăng ký các dịch vụ cho Identity
            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>();
            container.RegisterType<ApplicationUserManager>();
            container.RegisterType<IAuthenticationManager>(
                new InjectionFactory(c => HttpContext.Current.GetOwinContext().Authentication));

            container.RegisterType<DbContext, ApplicationDbContext>(new HierarchicalLifetimeManager());


            container.RegisterType<ICategoryService, CategoryService>();
            container.RegisterType<IProductService, ProductService>();
            container.RegisterType<IBannerService, BannerService>();
            container.RegisterType<INewsService, NewsService>();
            container.RegisterType<IOrderService, OrderService>();
            container.RegisterType<IOrderDetailService, OrderDetailService>();
            container.RegisterType<IProductService, ProductService>();
            container.RegisterType<IPromotionService, PromotionService>();
            container.RegisterType<IProductPromotionService, ProductPromotionService>();
        

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}