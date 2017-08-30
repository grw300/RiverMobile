using JsonApiSerializer.JsonApi;
using System;
using System.Collections.Generic;

namespace RiverMobile.Models
{
    public class Organization : BaseIdentifiable
    {
        public override string Type { get; set; } = "organizations";

        public string Name { get; set; }
        public Relationship<IEnumerable<Site>> Sites { get; set; }
    }
}
