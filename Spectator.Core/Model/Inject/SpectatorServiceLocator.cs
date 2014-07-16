using System;
using Microsoft.Practices.ServiceLocation;
using System.Collections.Generic;
using Autofac;
using Spectator.Core.Model.Web;
using Spectator.Core.Model.Image;
using Spectator.Core.Model.Image.Impl;

namespace Spectator.Core.Model.Inject
{
	public class SpectatorServiceLocator : ServiceLocatorImplBase
	{
		private IContainer locator;

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

		private class DefaultModule : Module
		{
			protected override void Load (ContainerBuilder b)
			{
				b.RegisterType<WebConnect> ().As<IWebConnect> ().SingleInstance ();

				b.RegisterType<SubscriptionModel> ().As<ISubscriptionModel> ().SingleInstance ();
				b.RegisterType<SnapshotCollectionModel> ().As<ISnapshotCollectionModel> ().SingleInstance ();
				b.RegisterType<ProfileModel> ().As<IProfileModel> ().SingleInstance ();

				b.RegisterType<DefaultDiskCache> ().As<IDiskCache> ().SingleInstance ();
				b.RegisterType<DefaultMemoryCache> ().As<IMemoryCache> ().SingleInstance ();
//				b.RegisterType<StubMemoryCache> ().As<IMemoryCache> ().SingleInstance ();
				b.RegisterType<ImageModel> ().As<IImageModel> ().SingleInstance ();
			}
		}

		#endregion
	}
}