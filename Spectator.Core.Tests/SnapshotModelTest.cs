using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using Spectator.Core.Model;
using Spectator.Core.Model.Inject;
using Spectator.Core.Tests.Common;
using System;
using Spectator.Core.Model.Database;
using Spectator.Core.Model.Web;
using Moq;
using Spectator.Core.Model.Web.Proto;
using System.Collections.Generic;

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
		public void TestReload ()
		{
			var api = injectModule.Set (Mock.Of<IApiClient> ());
			var storage = injectModule.Set (Mock.Of<IRepository> ());
			storage.Setup (s => s.GetSnapshot (TestId)).Returns (new Snapshot { ServerId = 1000 });
			api.Setup (s => s.GetSnapshot (1000)).Returns (new SnapshotsResponse.ProtoSnapshot {
				Images = new List<string> (){ "http://google.com/image1", "http://google.com/image2" },
				Thumbnail = 999,
				ThumbnailWidth = 1024,
				ThumbnailHeight = 768,
			});

			var model = new SnapshotModel (TestId);

			model.Reload ().Wait ();
			api.Verify (s => s.GetSnapshot (1000));
			storage.Verify (s => s.Update (new Snapshot {
				//
			}));
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