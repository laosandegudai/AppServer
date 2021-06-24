using System;

using ASC.Core.Common.Extensions;

namespace ASC.Core.Common.Utils
{
    public static class Guard
    {
        public static TValue NotNull<TValue>(this TValue value, string paramName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(paramName);
            }

            return value;
        }

        public static string NotNullOrWhiteSpace(this string val, string paramName = null, string message = null)
        {
            if (val.IsNullOrWhiteSpace())
            {
                throw message == null
                    ? new ArgumentException(paramName)
                    : new ArgumentException(message, paramName);
            }

            return val;
        }

        public static int IsPositive(this int value, string paramName)
        {
            if (value <= 0)
            {
                throw new ArgumentException("Argument must be greater than zero", paramName);
            }

            return value;
        }
    }
}
