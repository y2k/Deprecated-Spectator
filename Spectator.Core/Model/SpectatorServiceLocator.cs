using Autofac;
using Microsoft.Practices.ServiceLocation;
using Spectator.Core.Model.Account;
using Spectator.Core.Model.Database;
using Spectator.Core.Model.Web;
using System;
using System.Collections.Generic;

namespace Spectator.Core.Model
{
    public class SpectatorServiceLocator : ServiceLocatorImplBase
    {
        IContainer locator;

        public SpectatorServiceLocator(Module platformModule)
        {
            var b = new ContainerBuilder();
            b.RegisterModule(new DefaultModule());
            if (platformModule != null)
                b.RegisterModule(platformModule);
            locator = b.Build();
        }

        #region implemented abstract members of ServiceLocatorImplBase

        protected override object DoGetInstance(Type serviceType, string key)
        {
            return locator.Resolve(serviceType);
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Inner classes

        class DefaultModule : Module
        {
            protected override void Load(ContainerBuilder b)
            {
                b.RegisterType<HttpApiClient>().As<ISpectatorApi>().SingleInstance();
                b.RegisterInstance(new MemoryRepository()).As<IRepository>();
                b.Register(_ => ImageModel.Instance).AsSelf();

                b.RegisterType<PreferenceCookieStorage>().As<IAuthProvider>();
                b.RegisterType<PreferenceCookieStorage>().As<Account.Account.IStorage>();
            }
        }

        #endregion
    }
}