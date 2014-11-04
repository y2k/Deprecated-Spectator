using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using Moq;
using NUnit.Framework;
using Spectator.Core.Model;
using Spectator.Core.Model.Database;
using Spectator.Core.Model.Inject;
using Spectator.Core.Model.Web;
using Spectator.Core.Model.Web.Proto;
using Spectator.Core.Tests.Common;

namespace Spectator.Core.Tests
{
	[TestFixture]
	public class SnapshotCollectionModelTest
	{
		TestModule injectModule;

		[SetUp]
		public void SetUp ()
		{
			injectModule = new TestModule ();
			ServiceLocator.SetLocatorProvider (() => new SpectatorServiceLocator (injectModule));
		}

		[Test]
		public void TestGetFeed ()
		{
			var web = injectModule.Set<IApiClient> (Mock.Of<IApiClient> ());
			var repo = injectModule.Set<IRepository> (Mock.Of<IRepository> ());
			web.Setup (s => s.Get (0)).Returns (new SnapshotsResponse { Snapshots = Generate (100) });

			var model = new SnapshotCollectionModel (0);

			// загрузка первой страницы
			repo.Setup (s => s.GetAll ()).Returns (GenerateShapshot (100));
			var actual = model.Next ().Result;
			Assert.AreEqual (100, actual.Count ());
			web.Verify (s => s.Get (0));
			repo.Verify (s => s.ReplaceAll (0, It.Is<IEnumerable<Snapshot>> (a => a.Count () == 100)));

			// загрузка второй страницы
			repo.Setup (s => s.GetAll ()).Returns (GenerateShapshot (200));
			actual = model.Next ().Result;
			Assert.AreEqual (200, actual.Count ());
			web.Verify (s => s.Get (100));
			repo.Verify (s => s.ReplaceAll (0, It.Is<IEnumerable<Snapshot>> (a => a.Count () == 100)));

			// перезагрузка первой страницы
			model.Reset ().Wait ();
			actual = model.Next ().Result;
			Assert.AreEqual (0, actual.Count ());

			actual = model.Next ().Result;
			Assert.AreEqual (100, actual.Count ());
			web.Verify (s => s.Get (0));
			repo.Verify (s => s.ReplaceAll (0, It.Is<IEnumerable<Snapshot>> (a => a.Count () == 100)));
		}

		List<Snapshot> GenerateShapshot (int count)
		{
			var items = new List<Snapshot> ();
			for (int i = 0; i < count; i++)
				items.Add (new Snapshot ());
			items.Last ().Id = count;
			return items;
		}

		List<SnapshotsResponse.ProtoSnapshot> Generate (int count)
		{
			var items = new List<SnapshotsResponse.ProtoSnapshot> ();
			for (int i = 0; i < count; i++)
				items.Add (new SnapshotsResponse.ProtoSnapshot ());
			return items;
		}
	}
}