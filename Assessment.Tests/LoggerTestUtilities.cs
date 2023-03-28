namespace Assessment.Tests
{
    using System;
    using Microsoft.Extensions.Logging;
    using Moq;

    public static class LoggerTestUtilities
    {
        public static string VerifyWasCalledContains<T>(this Mock<ILogger<T>> logger, string messageShouldContainThisString, LogLevel expectedLogLevel, Times? times = null)
        {
            string messageShouldContainThisString2 = messageShouldContainThisString;
            string loggedMessage = string.Empty;
            logger.VerifyLog(expectedLogLevel, times, new Func<object, Type, bool>(State));
            return loggedMessage;
            bool State(object v, Type t)
            {
                loggedMessage = v.ToString();
                return v.ToString()!.Contains(messageShouldContainThisString2);
            }
        }

        public static string VerifyWasCalledEquals<T>(this Mock<ILogger<T>> logger, string messageShouldEqualThisString, LogLevel expectedLogLevel, Times? times = null)
        {
            string messageShouldEqualThisString2 = messageShouldEqualThisString;
            string loggedMessage = string.Empty;
            logger.VerifyLog(expectedLogLevel, times, new Func<object, Type, bool>(State));
            return loggedMessage;
            bool State(object v, Type t)
            {
                loggedMessage = v.ToString();
                return v.ToString()!.Equals(messageShouldEqualThisString2);
            }
        }

        private static void VerifyLog<T>(this Mock<ILogger<T>> logger, LogLevel expectedLogLevel, Times? times, Func<object, Type, bool> state)
        {
            Func<object, Type, bool> state2 = state;
            logger.Verify((ILogger<T> x) => x.Log(It.Is((LogLevel l) => (int)l == (int)expectedLogLevel), It.IsAny<EventId>(), It.Is<It.IsAnyType>((object v, Type t) => state2(v, t)), It.IsAny<Exception>(), It.Is<Func<It.IsAnyType, Exception, string>>((object v, Type t) => true)), times ?? Times.AtLeastOnce());
        }
    }
}
