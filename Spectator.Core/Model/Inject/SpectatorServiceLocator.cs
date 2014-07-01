using System;
using Microsoft.Practices.ServiceLocation;
using System.Collections.Generic;
using Autofac;
using Spectator.Core.Model.Web;

namespace Spectator.Core.Model.Inject
{
	public class SpectatorServiceLocator : ServiceLocatorImplBase
	{
		private IContainer locator;

		public SpectatorServiceLocator ()
		{
			var b = new ContainerBuilder();
			b.RegisterModule(new DefaultModule());
			locator = b.Build();
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
			protected override void Load(ContainerBuilder b)
			{
				b.RegisterType<WebConnect> ().As<IWebConnect>();

				b.RegisterType<SubscriptionModel>().As<ISubscriptionModel>();
				b.RegisterType<SnapshotCollectionModel> ().As<ISnapshotCollectionModel> ();
				b.RegisterType<ProfileModel> ().As<IProfileModel> ();
			}
		}

		#endregion
	}
}