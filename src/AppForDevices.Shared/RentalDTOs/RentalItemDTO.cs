using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AppForDevices.Shared.RentalDTOs
{
    public class RentalItemDTO //Related to first get Method
    {
        public RentalItemDTO()
        {

        }
        public RentalItemDTO(string model, QualityType? qualityType, double priceForRental, int deviceId, int quantity)
        {
            Model = model;
            QualityType = qualityType;
            PriceForRental = priceForRental;
            DeviceId = deviceId;
            Quantity = quantity;
        }
        public RentalItemDTO(int deviceId, string name, string model, int quantity)
        {
            DeviceId = deviceId;
            Name = name;
            Model = model;
            Quantity = quantity;
        }
        public RentalItemDTO(int deviceId, string name, string model, int quantity, double priceForRental)
        {
            DeviceId = deviceId;
            Name = name;
            Model = model;
            Quantity = quantity;
            PriceForRental = priceForRental;
        }
        public RentalItemDTO(string model, QualityType? qualityType, double priceForRental, int deviceId, int quantity, string name)
        {
            Model = model;
            QualityType = qualityType;
            PriceForRental = priceForRental;
            DeviceId = deviceId;
            Quantity = quantity;
            Name = name;
        }
        public string Model { get; set; }
        public QualityType? QualityType { get; set; }
        public double PriceForRental { get; set; }
        public int DeviceId { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is RentalItemDTO dTO &&
                   Model == dTO.Model &&
                   QualityType == dTO.QualityType &&
                   PriceForRental == dTO.PriceForRental &&
                   DeviceId == dTO.DeviceId &&
                   Quantity == dTO.Quantity &&
                   Name == dTO.Name;
        }
    }
}
