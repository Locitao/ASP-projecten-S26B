using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Reservering;

/// <summary>
/// Summary description for Select
/// </summary>
public class Select
{
    public int Select_Persoon(string voornaam, string achternaam)
    {
        try
        {
            var sql = "SELECT ID FROM PERSOON WHERE voornaam = '" + voornaam + "' AND achternaam = '" + achternaam + "'";
            var id = 0;
            var data = Connection.ExecuteQuery(sql);

            foreach (Dictionary<string, object> row in data)
            {
                id = Convert.ToInt32(row["ID"]);
            }
            return id;
        }
        catch (Exception)
        {
            
            throw;
        }
        
    }

    public List<Dictionary<string, object>> Select_Test_Personen()
    {
        try
        {
            var sql = "SELECT \"locatie_id\" FROM plek";
            var data = Connection.ExecuteQuery(sql);
            return data;
        }
        catch (Exception)
        {
            throw;
        }
    }
}