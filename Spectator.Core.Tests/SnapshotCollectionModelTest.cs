using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using Spectator.Core.Model;
using Spectator.Core.Model.Inject;
using Spectator.Core.Tests.Common;
using System.Threading.Tasks;

namespace Spectator.Core.Tests
{
    [TestFixture()]
    public class SnapshotCollectionModelTest
    {
        [Test]
        public void TestCase()
        {
            Task.Run(async () =>
            {
                ServiceLocator.SetLocatorProvider(() => new SpectatorServiceLocator(new TestModule()));
                var model = ServiceLocator.Current.GetInstance<ISnapshotCollectionModel>();

                await model.GetAllAsync(false, 0);
                await model.GetAllAsync(true, 0);
            }).Wait();
        }
    }
}