using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class ItemLine
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

