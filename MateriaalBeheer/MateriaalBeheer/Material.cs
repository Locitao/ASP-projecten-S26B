using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MaterialRenting
{
    public class Material
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Serie { get; set; }
        public int Price { get; set; }
        public string Barcode { get; set; }
        public List<DateTime[]> RentingTimes { get; set; }
        public Status Status { get; set; }

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
            Status = Status.Free;
            return Status;
            
        }

        public void AddDates(DateTime dateFrom, DateTime dateTo)
        {
            DateTime[] dates = new DateTime[2];
            dates[0] = dateFrom;
            dates[1] = dateTo;
            RentingTimes.Add(dates);
        }

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