using System.Collections.Immutable;
using System.Xml;

namespace Shimakaze.MinimalApi.Plus.SourceGenerator.Internal;

internal static class XmlDocumentExtensions
{
    extension(XmlDocument document)
    {
        public static XmlDocument LoadXml(string xml)
        {
            XmlDocument doc = new();
            doc.LoadXml(xml);
            return doc;
        }

        public string GetDocumentTagText(string tagName)
        {
            IEnumerable<string> enumerable = document
                .GetElementsByTagName(tagName)
                .OfType<XmlElement>()
                .Select(i => i.InnerText.Trim());

            return string.Join("\r\n", enumerable);
        }

        public IReadOnlyDictionary<string, string> ParseParameterDocuments()
        {
            return document
                 .GetElementsByTagName("param")
                 .OfType<XmlElement>()
                 .ToImmutableDictionary(i => i.GetAttribute("name"), i => i.InnerText.Trim());
        }
    }
}
