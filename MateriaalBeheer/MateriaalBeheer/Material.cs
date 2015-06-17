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
            this.Id = id;
            this.Brand = brand;
            this.Serie = serie;
            this.Price = price;
            this.Barcode = barcode;
            this.RentingTimes = rentingTimes;
        }

        public Status CheckStatus(DateTime dateFrom, DateTime dateTo)
        {
            foreach (DateTime[] date in RentingTimes)
            {
                if (dateFrom >= date[0] && dateFrom <= date[1])
                {
                    Status = Status.Reserved;
                }
                else if (dateTo >= date[0] && dateTo <= date[0])
                {
                    Status = Status.Reserved;
                }
                else if (dateFrom <= date[0] && dateTo >= date[1])
                {
                    Status = Status.Reserved;
                }
                else
                {
                    Status = Status.Free;
                }
            }
            return Status;
        }
    }
}