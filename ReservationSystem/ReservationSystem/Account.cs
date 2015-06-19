/// <summary>
/// Used to keep track of a users' account.
/// </summary>
namespace ReservationSystem
{
public class Account
{
    public string Username { get; set; }
    public string Email { get; set; }

	public Account(string username, string email)
	{
	    Username = username;
	    Email = email;
	}

    public override string ToString()
    {
        string data = "Username: " + Username + ", email: " + Email + ".";
        return data;
    }
}
}