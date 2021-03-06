using System;
using FluentAssertions;
using Knapcode.SocketToMe.Http;
using Xunit;

namespace Knapcode.SocketToMe.Tests.Http
{
    public class ExchangeIdTests
    {
        [Fact]
        public void Properties()
        {
            // ARRANGE
            var exchangeId = new ExchangeId(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero), new Guid("F7E38533-F06B-407C-B26D-DC8EE9E961D4"));

            // ACT, ASSERT
            exchangeId.When.EqualsExact(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero)).Should().BeTrue();
            exchangeId.Unique.Should().Be(new Guid("F7E38533-F06B-407C-B26D-DC8EE9E961D4"));
        }

        [Fact]
        public void Equals()
        {
            // ARRANGE
            var a = new ExchangeId(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero), new Guid("F7E38533-F06B-407C-B26D-DC8EE9E961D4"));
            var b = new ExchangeId(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero), new Guid("F7E38533-F06B-407C-B26D-DC8EE9E961D4"));

            // ACT, ASSERT
            a.Should().Be(b);
        }

        [Fact]
        public void ConsistentHashCode()
        {
            // ARRANGE
            var a = new ExchangeId(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero), new Guid("F7E38533-F06B-407C-B26D-DC8EE9E961D4"));
            var b = new ExchangeId(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero), new Guid("F7E38533-F06B-407C-B26D-DC8EE9E961D4"));

            // ACT, ASSERT
            a.GetHashCode().Should().Be(b.GetHashCode());
        }

        [Fact]
        public void ConvertsToUniversalTime()
        {
            // ARRANGE
            var exchangeId = new ExchangeId(new DateTimeOffset(2000, 1, 1, 8, 0, 0, TimeSpan.FromHours(8)), new Guid("F7E38533-F06B-407C-B26D-DC8EE9E961D4"));

            // ACT, ASSERT
            exchangeId.When.EqualsExact(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero)).Should().BeTrue();
        }

        [Fact]
        public void ConvertsToString()
        {
            // ARRANGE
            var exchangeId = new ExchangeId(new DateTimeOffset(2000, 1, 1, 8, 0, 0, TimeSpan.FromHours(8)), new Guid("F7E38533-F06B-407C-B26D-DC8EE9E961D4"));

            // ACT, ASSERT
            exchangeId.ToString().Should().Be("2000-01-01T00:00:00.0000000+00:00 f7e38533-f06b-407c-b26d-dc8ee9e961d4");
        }

        [Fact]
        public void DifferentUnique()
        {
            // ARRANGE
            var a = new ExchangeId(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero), new Guid("F7E38533-F06B-407C-B26D-DC8EE9E961D4"));
            var b = new ExchangeId(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero), new Guid("BA6F0E9D-38CE-4119-B74F-2E8315B322C7"));

            // ACT, ASSERT
            a.Should().NotBe(b);
        }

        [Fact]
        public void DifferentWhen()
        {
            // ARRANGE
            var a = new ExchangeId(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero), new Guid("F7E38533-F06B-407C-B26D-DC8EE9E961D4"));
            var b = new ExchangeId(new DateTimeOffset(2000, 2, 2, 0, 0, 0, TimeSpan.Zero), new Guid("F7E38533-F06B-407C-B26D-DC8EE9E961D4"));

            // ACT, ASSERT
            a.Should().NotBe(b);
        }

        [Fact]
        public void Empty()
        {
            // ARRANGE
            var exchangeId = new ExchangeId();

            // ACT, ASSERT
            exchangeId.Should().Be(ExchangeId.Empty);
        }

        [Fact]
        public void New()
        {
            // ARRANGE
            var before = DateTimeOffset.UtcNow;
            var exchangeId = ExchangeId.NewExchangeId();
            var after = DateTimeOffset.UtcNow;

            // ACT, ASSERT
            exchangeId.When.Should().BeOnOrAfter(before);
            exchangeId.When.Should().BeOnOrBefore(after);
            exchangeId.Unique.Should().NotBeEmpty();
        }

        [Fact]
        public void ToAscendingString()
        {
            // ARRANGE
            var before = new ExchangeId(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero), new Guid("DACD1424-DAA5-485B-81A4-E99C2BD66F1E"));
            var after = new ExchangeId(new DateTimeOffset(2000, 1, 2, 0, 0, 0, TimeSpan.Zero), new Guid("DACD1424-DAA5-485B-81A4-E99C2BD66F1E"));

            // ACT
            var beforeString = before.ToString("A");
            var afterString = after.ToString("A");
            string.Compare(beforeString, afterString, StringComparison.Ordinal).Should().BeNegative();
        }

        [Fact]
        public void ToDescendingString()
        {
            // ARRANGE
            var before = new ExchangeId(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero), new Guid("DACD1424-DAA5-485B-81A4-E99C2BD66F1E"));
            var after = new ExchangeId(new DateTimeOffset(2000, 1, 2, 0, 0, 0, TimeSpan.Zero), new Guid("DACD1424-DAA5-485B-81A4-E99C2BD66F1E"));

            // ACT
            var beforeString = before.ToString("D");
            var afterString = after.ToString("D");
            string.Compare(beforeString, afterString, StringComparison.Ordinal).Should().BePositive();
        }
    }
};