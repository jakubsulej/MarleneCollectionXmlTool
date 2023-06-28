namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpPostmetum
{
    public WpPostmetum(ulong postId, string metaKey, string metaValue)
    {
        PostId = postId;
        MetaKey = metaKey;
        MetaValue = metaValue;
    }

    public WpPostmetum() { }

    public ulong MetaId { get; set; }

    public ulong PostId { get; set; }

    public string MetaKey { get; set; }

    public string MetaValue { get; set; }
}
