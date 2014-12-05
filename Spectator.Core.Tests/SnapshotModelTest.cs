using System;
using System.Collections.Generic;
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
	public class SnapshotModelTest
	{
		const int TestId = 100;
		TestModule injectModule;

		[SetUp]
		public void SetUp ()
		{
			injectModule = new TestModule ();
			ServiceLocator.SetLocatorProvider (() => new SpectatorServiceLocator (injectModule));
		}

		[Test]
		public async void TestReload ()
		{
			var api = injectModule.Set<ISpectatorApi> ();
			api.Setup (s => s.GetSnapshot (1000)).Returns (
				new SnapshotsResponse.ProtoSnapshot {
					Id = 1000,
					Images = new List<string> { "http://google.com/image1", "http://google.com/image2" },
					Thumbnail = 999,
					ThumbnailWidth = 1024,
					ThumbnailHeight = 768,
					Title = "Test title",
				});
			var repo = ServiceLocator.Current.GetInstance<IRepository> ();
			var testSnapshot = new Snapshot { ServerId = 1000 };
			repo.Add (0, new []{ testSnapshot });

			var model = new SnapshotModel (testSnapshot.Id);

			await model.SyncWithWeb ();
			api.Verify (s => s.GetSnapshot (1000));

			var actual = await model.Get ();
			Assert.AreEqual (new Snapshot { 
				Id = testSnapshot.Id,
				ServerId = 1000,
				ThumbnailImageId = 999,
				ThumbnailWidth = 1024,
				ThumbnailHeight = 768,
				Title = "Test title",
			}, actual);
		}

		[Test]
		public void TestWebContent ()
		{
			var storage = injectModule.Set (Mock.Of<IRepository> ());
			storage.Setup (s => s.GetSnapshot (TestId)).Returns (new Snapshot { 
				HasWebContent = true,
				HasRevisions = true,
				ServerId = 1000,
			});

			var model = new SnapshotModel (TestId);

			var actual = model.Get ().Result;
			Assert.IsTrue (actual.HasWebContent);
			Assert.IsTrue (actual.HasRevisions);
			Assert.AreEqual (new Uri (Constants.BaseApi, "/Content/Index/1000"), model.WebContent);
			Assert.AreEqual (new Uri (Constants.BaseApi, "/Content/Diff/1000"), model.DiffContent);
		}

		[Test]
		public void TestSnapshotContent ()
		{
			var expectedCreated = new DateTime (2014, 1, 2, 3, 4, 5, 6);
			var storage = injectModule.Set (Mock.Of<IRepository> ());
			storage.Setup (s => s.GetSnapshot (TestId)).Returns (new Snapshot { 
				Title = "Test title",
				HasWebContent = true,
				HasRevisions = true,
				ServerId = 1000,
				Created = expectedCreated,
			});
			storage.Setup (s => s.GetAttachements (TestId)).Returns (new [] {
				new Attachment { Width = 10, Height = 10, Image = "http://google.com/image1.jpeg" },
				new Attachment { Width = 0, Height = 0, Image = "http://google.com/image2.jpeg" },
			});

			var model = new SnapshotModel (TestId);

			var actual = model.Get ().Result;
			Assert.AreEqual ("Test title", actual.Title);
			Assert.AreEqual (expectedCreated, actual.Created);
			CollectionAssert.AreEqual (new [] {
				new Attachment { Width = 10, Height = 10, Image = "http://google.com/image1.jpeg" },
				new Attachment { Width = 0, Height = 0, Image = "http://google.com/image2.jpeg" },
			}, model.GetAttachments ().Result);
		}
	}
}