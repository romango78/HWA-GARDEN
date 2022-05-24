using HWA.GARDEN.Utilities.Resources;
using System.Diagnostics;


namespace HWA.GARDEN.Utilities.Validation
{
    /// <summary>Provides common runtime checks that throw <see cref="ArgumentException" /> upon failure.</summary>
    [DebuggerStepThrough]
    public static class Requires
    {
        /// <summary>Throws an exception if a condition does not evaluate to true.</summary>
        /// <param name="condition">The condition to evaluate.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <exception cref="ArgumentException"><paramref name="condition" /> does not evaluate to true.</exception>
        public static void Argument(bool condition, string parameterName, string message = null)
        {
            if (!condition)
            {
                if (message == null)
                {
                    message = Strings.GetString("requires:value-not-valid");
                }

                throw new ArgumentException(message, parameterName);
            }
        }

        /// <summary>Throws an exception if the specified parameter's value is <see langword="null" />.</summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="value">The value of the argument.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
        public static void NotNull<T>(T value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        /// <summary>Throws an exception if the specified parameter's value is <see langword="null" /> or empty string.</summary>
        /// <param name="value">The value of the argument.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
        /// <exception cref="ArgumentException"><paramref name="value" /> is empty string.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
        public static void NotNullOrEmpty(string value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
            if (value.Length == 0)
            {
                var message = Strings.GetString("requires:string-empty");

                throw new ArgumentException(message, parameterName);
            }
        }

        /// <summary>Throws an exception if the specified parameter's value is <see langword="null" />, empty string, or whitespace.</summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="value">The value of the argument.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
        /// <exception cref="ArgumentException"><paramref name="value" /> is empty string.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
        public static void NotNullOrWhiteSpace(string value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
            if (value.Length == 0)
            {
                var message = Strings.GetString("requires:string-empty");

                throw new ArgumentException(message, parameterName);
            }

            for (var i = 0; i < value.Length; i++)
            {
                if (!char.IsWhiteSpace(value[i]))
                {
                    return;
                }
            }

            {
                var message = Strings.GetString("requires:string-white-space");

                throw new ArgumentException(message, parameterName);
            }
        }

        /// <summary>Throws an exception if the specified parameter's value is <see langword="null" /> or has no elements.</summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="value">The value of the argument.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
        /// <exception cref="ArgumentException"><paramref name="value" /> has no elements.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
        public static void NotNullOrEmpty<T>(IEnumerable<T> values, string parameterName)
        {
            if (values == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            using (var enumerator = values.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    return;
                }
            }

            {
                var message = Strings.GetString("requires:collection-empty");

                throw new ArgumentException(message, parameterName);
            }
        }

        /// <summary>Throws an exception if the specified parameter's value is <see langword="null" /> or has an element with a null value.</summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="value">The value of the argument.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
        /// <exception cref="ArgumentException"><paramref name="value" /> has an element with a null value.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is <see langword="null" />.</exception>
        public static void NotNullOrNullElements<T>(IEnumerable<T> values, string parameterName)
        {
            if (values == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            foreach (var value in values)
            {
                if (value == null)
                {
                    var message = Strings.GetString("requires:collection-null-value");

                    throw new ArgumentException(message, parameterName);
                }
            }
        }

        /// <summary>Throws an excpetion if a range condition does not evaluate to true.</summary>
        /// <param name="condition">The condition to evaluate.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="condition" /> does not evaluate to true.</exception>
        public static void Range(bool condition, string parameterName, string message = null)
        {
            if (!condition)
            {
                if (message == null)
                {
                    message = Strings.GetString("requires:numeric-out-of-range");
                }

                throw new ArgumentOutOfRangeException(parameterName, message);
            }
        }
    }
}
