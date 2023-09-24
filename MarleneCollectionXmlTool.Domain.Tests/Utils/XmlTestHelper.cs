using System.Xml;

namespace MarleneCollectionXmlTool.Domain.Tests.Utils;

internal static class XmlTestHelper
{
    private const string XmlDummyDocumentsNamespace = "XmlDummyDocuments";

    public static XmlDocument GetXmlDocumentFromStaticFile(string fileName)
    {
        var path = Path.Combine(Directory.GetParent(AppContext.BaseDirectory)!
            .Parent!.Parent!.Parent!.FullName, XmlDummyDocumentsNamespace, $"{fileName}.xml");

        var xmlValue = File.ReadAllText(path);

        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlValue);

        return xmlDoc;
    }
}
