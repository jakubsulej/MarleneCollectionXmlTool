using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpPmxiImage
{
    public ulong Id { get; set; }

    public ulong AttachmentId { get; set; }

    public string ImageUrl { get; set; }

    public string ImageFilename { get; set; }
}
