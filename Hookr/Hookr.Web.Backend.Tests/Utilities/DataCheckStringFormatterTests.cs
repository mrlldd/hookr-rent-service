using FluentAssertions;
using Hookr.Web.Backend.Operations.Queries.Auth;
using Hookr.Web.Backend.Utilities;
using NUnit.Framework;

namespace Hookr.Web.Backend.Tests.Utilities
{
    [TestFixture]
    public class DataCheckStringFormatterTests
    {
        [Test]
        public void NotNullInput()
        {
            var formatter = DataCheckStringFormatter<CreateSessionQuery>.Instance;
            var result = formatter.Format(new CreateSessionQuery
            {
                Hash = "testhash",
                Id = 123,
                Username = "username",
                AuthDate = 123,
                FirstName = "firstname",
                PhotoUrl = "photourl"
            });

            result.Should()
                .Be(
                    "auth_date=123\nfirst_name=firstname\nhash=testhash\nid=123\nphoto_url=photourl\nusername=username");
        }

        [Test]
        public void ExceptProperty()
        {
            var result = DataCheckStringFormatter<CreateSessionQuery>.Instance
                .Format(new CreateSessionQuery
                {
                    Hash = "testhash",
                    Id = 123,
                    Username = "username",
                    AuthDate = 123,
                    FirstName = "firstname",
                    PhotoUrl = "photourl"
                }, nameof(CreateSessionQuery.Hash));

            result.Should()
                .Be(
                    "auth_date=123\nfirst_name=firstname\nid=123\nphoto_url=photourl\nusername=username");
        }
    }
}