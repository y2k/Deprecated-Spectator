using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using Spectator.Core.Model;
using Spectator.Core.Model.Inject;
using Spectator.Core.Tests.Common;
using System;
using Spectator.Core.Model.Database;

namespace Spectator.Core.Tests
{
	[TestFixture]
	public class SnapshotModelTest
	{
		readonly TestModule injectModule = new TestModule ();

		[SetUp]
		public void SetUp ()
		{
			ServiceLocator.SetLocatorProvider (() => new SpectatorServiceLocator (injectModule));
		}

		[Test]
		public void TestReload ()
		{
			var model = new SnapshotModel (0);
			model.Reload ().Wait ();
		}

		[Test]
		public void TestWebContent ()
		{
			var model = new SnapshotModel (0);
			model.Reload ().Wait ();

			var actual = model.Get ().Result;

			Assert.IsTrue (actual.HasWebContent);
			Assert.IsTrue (actual.HasRevisions);
			Assert.AreEqual (new Uri ("http://google.com/"), model.WebContent);
			Assert.AreEqual (new Uri ("http://api.example.com/"), model.DiffContent);
		}

		[Test]
		public void TestSnapshotContent ()
		{
			var model = new SnapshotModel (0);
			model.Reload ().Wait ();

			var actual = model.Get ().Result;

			Assert.AreEqual ("Test title", actual.Title);
			Assert.AreEqual (new DateTime (2014, 1, 2, 3, 4, 5, 6), actual.Created);

			CollectionAssert.AreEqual (new [] {
				new Attachment { Width = 10, Height = 10, Image = "http://google.com/image1.jpeg" },
				new Attachment { Width = 0, Height = 0, Image = "http://google.com/image2.jpeg" },
			}, model.GetAttachments ().Result);
		}
	}
}