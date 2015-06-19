namespace ReservationSystem
{
    /// <summary>
    /// Used to keep track of all products that can still be reserved.
    /// </summary>
    public class Product
    {
        public Product(string company, int price, int id, string name)
        {
            Company = company;
            Price = price;
            Id = id;
            Name = name;
        }

        public string Company { get; set; }
        public int Price { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            var data = Name + ", ID: " + Id + ", price: " + Price + ", brand: " + Company + ".";
            return data;
        }
    }
}