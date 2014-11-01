using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using Spectator.Core.Model;
using Spectator.Core.Model.Inject;
using Spectator.Core.Tests.Common;

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
		public void Test ()
		{
			var model = new SnapshotModel (0);

			model.Reload ().Wait ();

			var actual = model.Get ().Result;
			var attachments = model.GetAttachments ();
		}
	}
}