﻿using Microsoft.Practices.ServiceLocation;
using Moq;
using NUnit.Framework;
using Spectator.Core.Model;
using Spectator.Core.Model.Account;
using Spectator.Core.Model.Database;
using Spectator.Core.Model.Web;
using Spectator.Core.Tests.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spectator.Core.Tests
{
    [TestFixture]
    public class AuthTests
    {
        TestModule injectModule;

        [SetUp]
        public void SetUp()
        {
            injectModule = new TestModule();
            ServiceLocator.SetLocatorProvider(() => new SpectatorServiceLocator(injectModule));
        }

        [Test]
        public async void TestLoginAndLogout()
        {
            var web = injectModule.Set<ISpectatorApi>();
            var module = new Account();

            var testUserState = (IDictionary<string, string>)new Dictionary<string, string> { { "a", "b" } };
            web.Setup(s => s.LoginByCode(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(testUserState));
            await module.LoginByCode("test-token");
            web.Verify(s => s.LoginByCode("test-token", GoogleUrlParser.RedirectUri), Times.Once);

            var actual = await new PreferenceCookieStorage().Load();
            Assert.AreEqual(testUserState, actual);

            await module.Logout();
            actual = await new PreferenceCookieStorage().Load();
            Assert.AreEqual(0, actual.Count);
        }
    }
}