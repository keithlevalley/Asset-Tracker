using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;

namespace AssetLibrary
{
    public class Asset
    {
        [Key]
        public string SerialNumber { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string MACAddress { get; set; }
        public string IpAddress { get; set; }

        public Asset() { }

        public Asset (string SerialNumber, string Name, string Model, string MACAddress, string IpAddress)
        {
            this.SerialNumber = SerialNumber;
            this.Name = Name;
            this.Model = Model;
            this.MACAddress = MACAddress;
            this.IpAddress = IpAddress;
        }
    }
}
