using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpWcAdminNoteAction
{
    public ulong ActionId { get; set; }

    public ulong NoteId { get; set; }

    public string Name { get; set; }

    public string Label { get; set; }

    public string Query { get; set; }

    public string Status { get; set; }

    public string ActionedText { get; set; }

    public string NonceAction { get; set; }

    public string NonceName { get; set; }
}
