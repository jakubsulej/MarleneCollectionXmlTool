using System;
using System.Collections.Generic;

namespace MarleneCollectionXmlTool.DBAccessLayer.Models;

public partial class WpWcAdminNote
{
    public ulong NoteId { get; set; }

    public string Name { get; set; }

    public string Type { get; set; }

    public string Locale { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public string ContentData { get; set; }

    public string Status { get; set; }

    public string Source { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime? DateReminder { get; set; }

    public bool IsSnoozable { get; set; }

    public string Layout { get; set; }

    public string Image { get; set; }

    public bool IsDeleted { get; set; }

    public bool IsRead { get; set; }

    public string Icon { get; set; }
}
