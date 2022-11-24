using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using Noqoush.AdFalcon.Common.UserInfo;
using Noqoush.AdFalcon.Services.Interfaces.Services;
using Noqoush.AdFalcon.Services.Mapping;
using Noqoush.Framework;
using Noqoush.Framework.Security;


namespace ConsoleApp
{
    class Program
    {
        private static SecurityManager _securityProxy;
        private static ISecurityService securityService;
        private static NoqoushPrincipal Principal;
        private static IAppSiteService _appSiteService;
        static SecurityManager securityProxy;
        private static void init()
        {
            securityService = Noqoush.Framework.IoC.Instance.Resolve<ISecurityService>();
            _appSiteService = Noqoush.Framework.IoC.Instance.Resolve<IAppSiteService>();
            securityProxy = new SecurityManager(IoC.Instance.Resolve<ISecurityService>());

            _securityProxy = new SecurityManager(securityService);
            MappingRegister.RegisterMapping();
        }
        private static void Login()
        {
            var isSuccess = false;
            while (!isSuccess)
            {
                System.Console.WriteLine("User Name");
                var Username = Console.ReadLine();
                System.Console.WriteLine("Password");
                var Password = Console.ReadLine();
                AuthenticateResponse response;
                response = _securityProxy.AuthenticateUser(Username, Password);
                if (response.Status == AuthenticateStatus.Success)
                {
                    Principal = response.Principal;
                    isSuccess = true;
                    System.Threading.Thread.CurrentPrincipal = response.Principal;
                    securityProxy.BuildSecurityContext(response.Principal.Token);
                    Console.WriteLine("login successfully");
                }
            }
        }

        static void Main(string[] args)
        {
            init();
            Login();

            //test call
            var cratiria = new Noqoush.AdFalcon.Domain.Repositories.AppSiteCriteria
            {
                Size = 10,
                AccountId =Noqoush.Framework.OperationContext.Current.UserInfo<AdFalconUserInfo>().AccountId.Value,
                Page = 1
            };

            var result = _appSiteService.QueryByCratiria(cratiria);
            foreach (var appSiteListDto in result.Items)
            {
                Console.WriteLine(appSiteListDto.Name);
            }

            Console.ReadLine();
        }


    }
}
