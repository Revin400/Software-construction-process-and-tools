    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }

    public class Warehouse
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public Contact Contact { get; set; }  // Nested object
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
