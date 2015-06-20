namespace ReservationSystem
{
    /// <summary>
    /// Used to keep track of the account placing a reservation.
    /// </summary>
    public class Account
    {
        public Account(string username, string email)
        {
            Username = username;
            Email = email;
        }

        public string Username { get; set; }
        public string Email { get; set; }

        public override string ToString()
        {
            var data = "Username: " + Username + ", email: " + Email + ".";
            return data;
        }
    }
}