using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpBlcInstance
{
    public uint InstanceId { get; set; }

    public uint LinkId { get; set; }

    public uint ContainerId { get; set; }

    public string ContainerType { get; set; }

    public string LinkText { get; set; }

    public string ParserType { get; set; }

    public string ContainerField { get; set; }

    public string LinkContext { get; set; }

    public string RawUrl { get; set; }
}
