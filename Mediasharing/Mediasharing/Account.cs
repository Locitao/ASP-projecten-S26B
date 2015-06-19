using System;
using System.Data;
using System.Linq;

namespace Mediasharing
{

    public class Account
    {
        #region Properties
        public int Id { get; set; }
        public string Username { get; set; }
        #endregion

        #region Constructors
        public Account(string username)
        {
            Username = username;
        }

        public Account(int id, string username)
        {
            Id = id;
            Username = username;
        }
        #endregion

        
        //Nog implementeren
        public static Account Login(int id)
        {
            //Initialize.
            Database database = Database.Instance;
            DataSet dataset = new DataSet();

            //Fill dataset.
            dataset = database.GetData("SELECT \"ID\" AS ID, \"gebruikersnaam\" AS GEBRUIKERSNAAM FROM ACCOUNT WHERE ID =  " + "'" + id + "'");

            //Check if the dataset is empty or not.
            if (!IsEmpty(dataset))
            {
                Account user = new Account(Convert.ToInt32(dataset.Tables[0].Rows[0]["ID"]),
                    Convert.ToString(dataset.Tables[0].Rows[0]["GEBRUIKERSNAAM"]));
                return user;
            }
            else return null;
        }

        public static bool IsEmpty(DataSet dataSet)
        {
            return dataSet.Tables.Cast<DataTable>().All(table => table.Rows.Count == 0);
        }
    }
}