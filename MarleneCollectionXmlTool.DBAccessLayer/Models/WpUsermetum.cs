﻿using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpUsermetum
{
    public ulong UmetaId { get; set; }

    public ulong UserId { get; set; }

    public string MetaKey { get; set; }

    public string MetaValue { get; set; }
}
