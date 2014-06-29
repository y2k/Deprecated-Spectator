using System;
using Microsoft.Practices.ServiceLocation;
using System.Collections.Generic;
using Autofac;

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
				b.RegisterType<SubscriptionModel>().As<ISubscriptionModel>();
				b.RegisterType<SnapshotCollectionModel> ().As<ISnapshotCollectionModel> ();
				b.RegisterType<ProfileModel> ().As<IProfileModel> ();

//				b.RegisterType<WebDownloader>().As<IWebDownloader>();
//				b.RegisterType<ReactorParser>().As<ISiteParser>();
//				b.RegisterType<DefaultDiskCache>().As<IDiskCache>();
//				b.RegisterType<MemoryCache>().As<IMemoryCache>();
//				b.RegisterType<PostCollectionModel>().As<IPostCollectionModel>();
//				b.RegisterType<ImageModel>().As<IImageModel>();
//				b.RegisterType<TagCollectionModel>().As<ITagCollectionModel>();
//				b.RegisterType<ProfileModel>().As<IProfileModel>();
//				b.RegisterType<PostModel>().As<IPostModel>().SingleInstance();
			}
		}

		#endregion
	}
}