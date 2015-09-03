using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.iOS;
using Xamarin.UITest.Queries;

namespace Spectator.iOS.UITests
{
    [TestFixture]
    public class Tests
    {
        iOSApp app;

        [SetUp]
        public void BeforeEachTest()
        {
            app = ConfigureApp.iOS.StartApp();
        }

        [Test]
        public void ViewIsDisplayed()
        {
            AppResult[] results = app.WaitForElement(c => c.Child("UIView"));

            Assert.IsTrue(results.Any());

            app.DragCoordinates(0, 150, 100, 150);
            app.Tap("Logout");
        }
    }
}