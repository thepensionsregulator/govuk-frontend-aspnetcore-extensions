using ThePensionsRegulator.Frontend.Models;
using ThePensionsRegulator.Frontend.Services;

namespace ThePensionsRegulator.Frontend.Tests.Services
{
    public class TprSessionTimeOutMessageSelectorTests
    {
        private readonly TprSessionTimeOutModel _fallback;
        private readonly TprSessionTimeOutModel _warning;
        private readonly TprSessionTimeOutModel[] _messages;

        public TprSessionTimeOutMessageSelectorTests()
        {
            _fallback = new TprSessionTimeOutModel { MessageKey = "default" };
            _warning = new TprSessionTimeOutModel { MessageKey = "warning" };
            _messages = [_fallback, _warning];
        }

        [Theory]
        [InlineData("warning")]
        [InlineData("WARNING")]
        [InlineData("WaRnInG")]
        public void SelectByKey_ShouldReturnMatchingMessage_WhenKeyMatchesRegardlessOfCase(string messageKey)
        {
            // Arrange
            var messages = _messages;

            // Act
            var result = TprSessionTimeOutMessageSelector.SelectByKey(messages, messageKey);

            // Assert
            Assert.Same(_warning, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t\r\n")]
        [InlineData("unknown")]
        public void SelectByKey_ShouldReturnFirstMessage_WhenKeyIsMissingOrUnmatched(string? messageKey)
        {
            // Arrange
            var messages = _messages;

            // Act
            var result = TprSessionTimeOutMessageSelector.SelectByKey(messages, messageKey);

            // Assert
            Assert.Same(_fallback, result);
        }

        [Fact]
        public void SelectByKey_ShouldReturnFirstMatch_WhenKeysAreDuplicated()
        {
            // Arrange
            var duplicate = new TprSessionTimeOutModel { MessageKey = "WARNING" };
            TprSessionTimeOutModel[] messages = [.. _messages, duplicate];

            // Act
            var result = TprSessionTimeOutMessageSelector.SelectByKey(messages, "warning");

            // Assert
            Assert.Same(_warning, result);
        }

        [Fact]
        public void SelectByKey_ShouldReturnMatchingMessage_WhenCollectionContainsNullEntries()
        {
            // Arrange
            TprSessionTimeOutModel[] messages = [null!, _fallback, null!, _warning];

            // Act
            var result = TprSessionTimeOutMessageSelector.SelectByKey(messages, "warning");

            // Assert
            Assert.Same(_warning, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("default")]
        public void SelectByKey_ShouldReturnFirstNonNullMessage_WhenKeyIsUnmatched(string? fallbackKey)
        {
            // Arrange
            _fallback.MessageKey = fallbackKey;
            TprSessionTimeOutModel[] messages = [null!, .. _messages];

            // Act
            var result = TprSessionTimeOutMessageSelector.SelectByKey(messages, "unknown");

            // Assert
            Assert.Same(_fallback, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t\r\n")]
        public void SelectByKey_ShouldReturnFirstMessage_WhenLaterMessageHasSameMissingKey(string? messageKey)
        {
            // Arrange
            _warning.MessageKey = messageKey;

            // Act
            var result = TprSessionTimeOutMessageSelector.SelectByKey(_messages, messageKey);

            // Assert
            Assert.Same(_fallback, result);
        }

        [Fact]
        public void SelectByKey_ShouldReturnNull_WhenMessagesAreEmpty()
        {
            // Arrange
            TprSessionTimeOutModel[] messages = [];

            // Act
            var result = TprSessionTimeOutMessageSelector.SelectByKey(messages, "warning");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void SelectByKey_ShouldReturnNull_WhenAllMessagesAreNull()
        {
            // Arrange
            TprSessionTimeOutModel[] messages = [null!, null!];

            // Act
            var result = TprSessionTimeOutMessageSelector.SelectByKey(messages, "warning");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void SelectByKey_ShouldThrowArgumentNullException_WhenMessagesAreNull()
        {
            // Arrange
            IEnumerable<TprSessionTimeOutModel> messages = null!;

            // Act
            var exception = Assert.Throws<ArgumentNullException>(() =>
                TprSessionTimeOutMessageSelector.SelectByKey(messages, "warning"));

            // Assert
            Assert.Equal("messages", exception.ParamName);
        }
    }
}
