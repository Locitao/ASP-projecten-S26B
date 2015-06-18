using System;
/// <summary>
/// Locations on the camping.
/// </summary>
public class Location
{
    /// <summary>
    /// Fields.
    /// </summary>
    public int Id { get; set; }
    public int Capacity { get; set; }
    public int Nr { get; set; }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="capacity"></param>
    /// <param name="nr"></param>
	public Location(int id, int capacity, int nr)
	{
	    Id = id;
	    Capacity = capacity;
	    Nr = nr;
	}

    /// <summary>
    /// ToString method to be able to load it into a listbox.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        string data = "ID: " + Convert.ToString(Id) + ", capacity: " + Convert.ToString(Capacity) + ".";
        return data;
    }
}