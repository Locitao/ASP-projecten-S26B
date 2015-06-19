using System;
using System.Collections.Generic;

namespace MaterialRenting
{
    /// <summary>
    /// This class contains all needed information about Materials
    /// </summary>
    public class Material
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Serie { get; set; }
        public int Price { get; set; }
        public string Barcode { get; set; }
        public List<DateTime[]> RentingTimes { get; set; }
        public Status Status { get; set; }

        /// <summary>
        /// load all the necessary data into this object
        /// </summary>
        /// <param name="id">the 'productexemplaar' ID from the database</param>
        /// <param name="brand">the brand of this product</param>
        /// <param name="serie">the serie of this product</param>
        /// <param name="price">the current price of this product</param>
        /// <param name="barcode">the barcode of this product</param>
        /// <param name="rentingTimes">List </param>
        public Material(int id, string brand, string serie, int price, string barcode, List<DateTime[]> rentingTimes)
        {
            Id = id;
            Brand = brand;
            Serie = serie;
            Price = price;
            Barcode = barcode;
            RentingTimes = rentingTimes;
            Status = Status.Undefined;
        }

        /// <summary>
        /// This method checks the status from the item in between the inputted dates
        /// </summary>
        /// <param name="dateFrom">The date from when the status will be checked</param>
        /// <param name="dateTo">The date untill the status will be checked</param>
        /// <returns></returns>
        public Status CheckStatus(DateTime dateFrom, DateTime dateTo)
        {
            foreach (DateTime[] date in RentingTimes)
            {
                if (dateFrom >= date[0] && dateFrom <= date[1])
                {
                    Status = Status.Reserved;
                    return Status;
                }
                else if (dateTo >= date[0] && dateTo <= date[1])
                {
                    Status = Status.Reserved;
                    return Status;
                }
                else if (dateFrom <= date[0] && dateTo >= date[1])
                {
                    Status = Status.Reserved;
                    return Status;
                }
            }
            // if the dates dont overlap each other the status is free
            Status = Status.Free;
            return Status;
        }

        /// <summary>
        /// Add the dates too the list when the items are lent
        /// </summary>
        /// <param name="dateFrom">The date the renting period will start</param>
        /// <param name="dateTo">The date the renting period will end</param>
        public bool AddDates(DateTime dateFrom, DateTime dateTo)
        {
            if (CheckStatus(dateFrom, dateTo) == Status.Free)
            {
                DateTime[] dates = new DateTime[2];
                dates[0] = dateFrom;
                dates[1] = dateTo;
                RentingTimes.Add(dates);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// this method overrides the ToString method to show this Materials information
        /// </summary>
        /// <returns>string with information about this Material</returns>
        public override string ToString()
        {
            if (Status == Status.Undefined)
            {
                return Id + ", " + Brand + ", " + Serie;
            }
            else
            {
                return Id + ", " + Status.ToString() + ", " + Brand + ", " + Serie;
            }
        }
    }
}