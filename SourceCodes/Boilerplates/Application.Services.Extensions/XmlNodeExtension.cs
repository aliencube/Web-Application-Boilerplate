using System.Xml;
using System.Xml.Linq;

namespace Application.Services.Extensions
{
	/// <summary>
	/// This extends the XmlNode related methods.
	/// </summary>
	public static class XmlNodeExtension
	{
		/// <summary>
		/// Converts XmlNode to XElement.
		/// </summary>
		/// <param name="node">XmlNode instance.</param>
		/// <returns>Returns the XElement converted from XmlNode.</returns>
		public static XElement ToXElement(this XmlNode node)
		{
			if (node == null)
				return null;

			var xml = new XDocument();
			using (var writer = xml.CreateWriter())
			{
				node.WriteTo(writer);
			}
			return xml.Root;
		}
	}
}