using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mediasharing
{
    /// <summary>
    /// This class converts the C# datetime to a string that can be inserted into the oracle database.
    /// </summary>
    public static class OracleDate
    {
        #region Methods
        public static string GetOracleDate()
        {
            return (DateTime.Now.ToString("dd-MMM-yyy"));
        }
        #endregion
    }
}