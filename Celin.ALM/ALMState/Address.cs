
using System;

namespace Celin
{
    public class Address
    {
        public string addressLine { get; set; }
        public string adminDistrict { get; set; }
        public string countryRegion { get; set; }
        public string formattedAddress { get; set; }
        public string locality { get; set; }
        public string postalCode { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public DateTime? timestamp { get; set; }
        public double? distance { get; set; }
    }

}