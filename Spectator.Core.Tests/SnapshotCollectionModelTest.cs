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
using System;
using System.Threading.Tasks;

namespace Spectator.Core.Tests
{
	[TestFixture]
	public class SnapshotCollectionModelTest
	{
		TestModule injectModule;
		Random rand;

		[SetUp]
		public void SetUp ()
		{
			rand = new Random ();
			injectModule = new TestModule ();
			ServiceLocator.SetLocatorProvider (() => new SpectatorServiceLocator (injectModule));
		}

		[Test]
		public async void TestGetFeed ()
		{
			var web = injectModule.Set<ISpectatorApi> (Mock.Of<ISpectatorApi> ());
//			var repo = injectModule.Set<IRepository> (Mock.Of<IRepository> ());
			var responsePage1 = new SnapshotsResponse { Snapshots = Generate (100) };
            web.Setup(s => s.GetSnapshots(0)).Returns(Task.FromResult(responsePage1));
			var responsePage2 = new SnapshotsResponse { Snapshots = Generate (100) };
			web.Setup (s => s.GetSnapshots (responsePage1.Snapshots.Last ().Id)).Returns (Task.FromResult(responsePage2));

			var model = SnapshotCollectionModel.CreateForFeed ();

			// загрузка первой страницы
			await model.Next ();
			var actual = await model.Get ();
			Assert.AreEqual (100, actual.Count ());
			web.Verify (s => s.GetSnapshots (0));
//			repo.Verify (s => s.ReplaceAll (0, It.Is<IEnumerable<Snapshot>> (a => a.Count () == 100)));

			// загрузка второй страницы
//			repo.Setup (s => s.GetSnapshots (0)).Returns (GenerateShapshot (200));
			await model.Next ();
			actual = await model.Get ();
			Assert.AreEqual (200, actual.Count ());
			web.Verify (s => s.GetSnapshots (responsePage1.Snapshots.Last ().Id));
//			repo.Verify (s => s.ReplaceAll (0, It.Is<IEnumerable<Snapshot>> (a => a.Count () == 100)));

			// перезагрузка первой страницы
			await model.Reset ();
			actual = await model.Get ();
			Assert.AreEqual (0, actual.Count ());
			await model.Next ();
			actual = await model.Get ();
			Assert.AreEqual (100, actual.Count ());
			web.Verify (s => s.GetSnapshots (0));
//			repo.Verify (s => s.ReplaceAll (0, It.Is<IEnumerable<Snapshot>> (a => a.Count () == 100)));
		}

		List<Snapshot> GenerateShapshot (int count)
		{
			var items = new List<Snapshot> ();
			for (int i = 0; i < count; i++)
				items.Add (new Snapshot { ServerId = i });
			items.Last ().Id = count;
			return items;
		}

		List<SnapshotsResponse.ProtoSnapshot> Generate (int count)
		{
			var items = new List<SnapshotsResponse.ProtoSnapshot> ();
			for (int i = 0; i < count; i++)
				items.Add (new SnapshotsResponse.ProtoSnapshot {
					Id = rand.Next (),
				});
			return items;
		}
	}
}