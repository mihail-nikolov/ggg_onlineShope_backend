namespace GGG_OnlineShop.Web.Api.App_Start
{
    using GGG_OnlineShop.InternalApiDB.Data;
    using Data.Common;
    using Common;
    using Ninject;
    using Ninject.Web.Common;
    using System;
    using System.Web;
    using Ninject.Extensions.Conventions;
    using GGG_OnlineShop.Common.Services.Contracts;
    using GGG_OnlineShop.Common.Services;

    public class NinjectConfig
    {
        public static object ObjectFactory { get; private set; }

        public static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IInternalApiDbContext>().To<InternalApiDbContext>().InRequestScope();
            kernel.Bind(typeof(IDbRepository<>)).To(typeof(DbRepository<>)).InRequestScope();
            kernel.Bind<ILogger>().To<FileLogger>();
            kernel.Bind<IReader>().To<FileReader>();
            //kernel.Bind(typeof(IUserStore<User>)).To(typeof(UserStore<User>)); TODO remove if not needed

            kernel.Bind(b => b
                .From(Assemblies.DataServices)
                .SelectAllClasses()
                .BindDefaultInterface());

            kernel.Bind(b => b
                .From(Assemblies.CommonServices)
                .SelectAllClasses()
                .BindDefaultInterface());
        }
    }
}