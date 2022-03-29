using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TSoft.TVServer.Helpers
{
	/// <summary> An enumeration extensions. </summary>
	public static class EnumExtensions
	{
		#region [Public methods]

		/// <summary>
		/// A string extension method that converts this TSoft.TVServer.Helpers.EnumExtensions to
		/// an enumeration. </summary>
		/// <typeparam name="T"> Generic type parameter. </typeparam>
		/// <param name="value"> The value to act on. </param>
		/// <returns> The given data converted to a T. </returns>
		public static T ToEnum<T>(this string value)
		{
			return (T)Enum.Parse(typeof(T), value, true);
		}

		/// <summary>
		/// A string extension method that converts this TSoft.TVServer.Helpers.EnumExtensions to
		/// an enumeration. </summary>
		/// <typeparam name="T"> Generic type parameter. </typeparam>
		/// <param name="value"> The value to act on. </param>
		/// <param name="defaultValue"> The default value. </param>
		/// <returns> The given data converted to a T. </returns>
		public static T ToEnum<T>(this string value, T defaultValue) where T : struct
		{
			if (string.IsNullOrEmpty(value))
			{
				return defaultValue;
			}

			return Enum.TryParse<T>(value, true, out T result) ? result : defaultValue;
		}
		#endregion
	}
}
