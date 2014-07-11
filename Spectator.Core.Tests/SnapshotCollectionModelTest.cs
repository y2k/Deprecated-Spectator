using NUnit.Framework;
using System;
using Spectator.Core.Model;
using Microsoft.Practices.ServiceLocation;
using Spectator.Core.Model.Inject;
using Spectator.Core.Tests.Common;

namespace Spectator.Core.Tests
{
	[TestFixture ()]
	public class SnapshotCollectionModelTest
	{
		[SetUp]
		public void SetUp ()
		{
			ServiceLocator.SetLocatorProvider (() => new SpectatorServiceLocator(new TestModule()));
		}

		[Test ()]
		public void TestCase ()
		{
			var model = ServiceLocator.Current.GetInstance<ISnapshotCollectionModel> ();

			var s = model.GetAllAsync (false, 0).Result;
			s.ToString ();

			s = model.GetAllAsync (true, 0).Result;
			s.ToString ();
		}
	}
}