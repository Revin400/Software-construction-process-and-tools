using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class Supplier
{
    public int Id { get; set; }

    public string Code { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Address_extra { get; set; }
    public string City { get; set; }
    public int ZipCode { get; set; }
    public string Province { get; set; }
    public string Country { get; set; }
    public string ContactName { get; set; }
    public string ContactPhone { get; set; }
    public string Reference { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}