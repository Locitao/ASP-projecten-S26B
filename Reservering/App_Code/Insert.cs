using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using Reservering;

namespace Reservering
{
    /// <summary>
    /// This class will be used for all the insert statements.
    /// </summary>
    class Insert
    {
        Connection conn = new Connection();

        /// <summary>
        /// Method to be used for inserting a reservation.
        /// </summary>
        /// <returns>True or false, wether or not it succeeded.</returns>
        public bool Insert_Reservation()
        {
            return true;
        }

        /// <summary>
        /// Method to be used to insert a new account.
        /// </summary>
        /// <returns>True or false, wether or not it succeeded.</returns>
        public bool Insert_Account()
        {
            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Insert_Acc_Res()
        {
            return true;
        }
    }

}
