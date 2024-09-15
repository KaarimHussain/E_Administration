using System;
using System.Collections.Generic;

namespace E_Administration.Models;

public partial class File
{
    public int FileId { get; set; }

    public string? FileName { get; set; }

    public string? FilePath { get; set; }

    public string? Category { get; set; }

    public DateTime? UploadDate { get; set; }

    public string? UploadedBy { get; set; }

    public byte[]? FileData { get; set; }
}
