﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moonglade.Core;
using Moonglade.Data.Entities;
using Moonglade.Data.Infrastructure;
using Moonglade.Model.Settings;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Moonglade.Tests
{
    [TestFixture]
    public class FriendLinkServiceTests
    {
        private Mock<ILogger<FriendLinkService>> _loggerMock;
        private Mock<IOptions<AppSettings>> _appSettingsMock;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<FriendLinkService>>();
            _appSettingsMock = new Mock<IOptions<AppSettings>>();
        }

        [Test]
        public async Task TestGetFriendLinkAsync()
        {
            var uid = Guid.NewGuid();
            var friendLinkEntity = new FriendLinkEntity()
            {
                Id = uid,
                LinkUrl = "https://dot.net",
                Title = "Test"
            };

            var tcs = new TaskCompletionSource<FriendLinkEntity>();
            tcs.SetResult(friendLinkEntity);

            var friendlinkRepositoryMock = new Mock<IRepository<FriendLinkEntity>>();
            friendlinkRepositoryMock.Setup(p => p.GetAsync(It.IsAny<Guid>())).Returns(tcs.Task);

            var svc = new FriendLinkService(_loggerMock.Object, _appSettingsMock.Object, friendlinkRepositoryMock.Object);
            var fdLinkResponse = await svc.GetFriendLinkAsync(uid);

            Assert.IsTrue(fdLinkResponse.IsSuccess);
            Assert.AreEqual(friendLinkEntity, fdLinkResponse.Item);
        }

        [TestCase("", "", ExpectedResult = false)]
        [TestCase("996", "", ExpectedResult = false)]
        [TestCase("", "icu", ExpectedResult = false)]
        [TestCase("dotnet", "955", ExpectedResult = false)]
        public async Task<bool> TestAddFriendLinkAsyncInvalidParameter(string title, string linkUrl)
        {
            var friendlinkRepositoryMock = new Mock<IRepository<FriendLinkEntity>>();
            var svc = new FriendLinkService(_loggerMock.Object, _appSettingsMock.Object, friendlinkRepositoryMock.Object);

            var fdLinkResponse = await svc.AddFriendLinkAsync(title, linkUrl);
            return fdLinkResponse.IsSuccess;
        }
    }
}
