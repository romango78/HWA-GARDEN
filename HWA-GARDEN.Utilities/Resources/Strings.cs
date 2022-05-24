using System.Diagnostics;
using System.Globalization;
using System.Resources;

namespace HWA.GARDEN.Utilities.Resources
{
    /// <summary>Represents a strings resource manager.</summary>
    [DebuggerStepThrough]
    internal static class Strings
    {
        private static readonly ResourceManager _resourceManager = CreateResourceManager();

        /// <summary>Returns the value of the specified string.</summary>
        /// <param name="name">The name of the string to retrieve.</param>
        /// <returns>The value of the resource localized for the caller's current UI culture, or <see langword="null" /> if <paramref name="name" /> cannot be found in a resource set.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
        public static string GetString(string name)
        {
            return _resourceManager.GetString(name, CultureInfo.CurrentCulture);
        }

        private static ResourceManager CreateResourceManager()
        {
            var baseName = typeof(Strings).Namespace + "." + nameof(Strings);

            return new ResourceManager(baseName, typeof(Strings).Assembly);
        }
    }
}
