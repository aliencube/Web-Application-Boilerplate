using System;
using System.Xml;
using System.Xml.Linq;

namespace Application.Services.Extensions
{
	/// <summary>
	/// This extends the XElement related methods.
	/// </summary>
	public static class XElementExtension
	{
		/// <summary>
		/// Convert the value of XElement to Int32 data type.
		/// </summary>
		/// <param name="entry">XElement.</param>
		/// <returns>Returns the value of XElement to Int32 data type.</returns>
		public static int ToInt32(this XElement entry)
		{
			if (entry == null)
				return Int32.MinValue;
			if (String.IsNullOrWhiteSpace(entry.Value))
				return Int32.MinValue;

			var result = Int32.MinValue;
			int iresult;
			if (Int32.TryParse(entry.Value, out iresult))
				result = iresult;
			return result;
		}

		/// <summary>
		/// Convert the value of XElement to String data type.
		/// </summary>
		/// <param name="entry">XElement.</param>
		/// <returns>Returns the value of XElement to String data type.</returns>
		public static string ToText(this XElement entry)
		{
			if (entry == null)
				return null;

			return String.IsNullOrWhiteSpace(entry.Value) ? null : entry.Value;
		}

		/// <summary>
		/// Converts XElement to XmlNode.
		/// </summary>
		/// <param name="element">XElement instance.</param>
		/// <returns>Returns the XmlNode converted from XElement.</returns>
		public static XmlNode ToXmlNode(this XElement element)
		{
			if (element == null)
				return null;

			var xml = new XmlDocument();
			using (var reader = element.CreateReader())
			{
				xml.Load(reader);
			}
			XmlNode node = xml.DocumentElement;
			return node != null ? node : null;
		}
	}
}
