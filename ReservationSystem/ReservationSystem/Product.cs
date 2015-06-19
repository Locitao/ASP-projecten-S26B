using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Used to keep track of available products.
/// </summary>
namespace ReservationSystem
{
public class Product
{
    public string Company { get; set; }
    public int Price { get; set; }
    public int Id { get; set; }
    public string Name { get; set; }

	public Product(string company, int price, int id, string name)
	{
	    Company = company;
	    Price = price;
	    Id = id;
	    Name = name;
	}

    public override string ToString()
    {
        string data = Name + ", ID: " + Id + ", price: " + Price + ", brand: " + Company + ".";
        return data;
    }
}
}