using JsonApiSerializer.JsonApi;
using Newtonsoft.Json;
using System;

namespace RiverMobile.Models
{
    public class Stamp : BaseIdentifiable
    {
        public override string Type { get; set; } = "stamps";

        public DateTime Time { get; set; }
        public int Location { get; set; }
        public Guid? PersonalId { get; set; }
        [JsonProperty(propertyName: "owner-personal")]
        public Relationship<Personal> Personal { get; set; }
    }
}
