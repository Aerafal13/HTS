using System.Globalization;

namespace HTS.Core.Extensions;

/// <summary>
/// Extensions method for manages <see cref="DateTime"/> and <seealso cref="TimeSpan"/>.
/// </summary>
public static class TimeExtensions
{
	private static readonly CultureInfo FrenchCultureInfo = new("fr-FR");

	/// <summary>
	/// Transforms the <paramref name="time"/> to readable french date.
	/// </summary>
	/// <param name="time">The time.</param>
	/// <returns>A readable french date.</returns>
	public static string ToHumanDate(this DateTime time) =>
		string.Concat(
			time.ToString("D", FrenchCultureInfo),
			" à ",
			time.Hour,
			"h",
			time.Minute);
}
