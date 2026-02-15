using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Core.Entities
{
    public class Product
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string ImageFile { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.Decimal128)]
        public decimal price { get; set; }

        public string BrandId { get; set; }
        public ProductBrand Brand { get; set; }
        public string TypeId { get; set; }
        public ProductType Type { get; set; }
    }
}
