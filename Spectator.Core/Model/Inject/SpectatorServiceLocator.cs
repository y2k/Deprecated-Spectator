using System;
using System.Collections.Generic;
using Autofac;
using Microsoft.Practices.ServiceLocation;
using Spectator.Core.Model.Account;
using Spectator.Core.Model.Database;
using Spectator.Core.Model.Image;
using Spectator.Core.Model.Image.Impl;
using Spectator.Core.Model.Web;

namespace Spectator.Core.Model.Inject
{
	public class SpectatorServiceLocator : ServiceLocatorImplBase
	{
		IContainer locator;

		public SpectatorServiceLocator (Module platformModule)
		{
			var b = new ContainerBuilder ();
			b.RegisterModule (new DefaultModule ());
			if (platformModule != null)
				b.RegisterModule (platformModule);
			locator = b.Build ();
		}

		#region implemented abstract members of ServiceLocatorImplBase

		protected override object DoGetInstance (Type serviceType, string key)
		{
			return locator.Resolve (serviceType);
		}

		protected override IEnumerable<object> DoGetAllInstances (Type serviceType)
		{
			throw new NotImplementedException ();
		}

		#endregion

		#region Inner classes

		class DefaultModule : Module
		{
			protected override void Load (ContainerBuilder b)
			{
				b.RegisterType<HttpApiClient> ().As<ISpectatorApi> ().SingleInstance ();
				b.RegisterType<SqliteRepository> ().As<IRepository> ();

				b.RegisterType<DefaultDiskCache> ().As<IDiskCache> ().SingleInstance ();
				b.RegisterType<DefaultMemoryCache> ().As<IMemoryCache> ().SingleInstance ();
				b.RegisterType<ImageModel> ().AsSelf ().SingleInstance ();

				b.RegisterType<RepositoryAuthProvider> ().As<IAuthProvider> ();
			}
		}

		#endregion
	}
}