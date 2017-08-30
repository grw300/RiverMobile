using JsonApiSerializer.JsonApi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace RiverMobile.Models
{
    public class Personal : BaseIdentifiable
    {
        public override string Type { get; set; } = "personnel";

        public string Name { get; set; }
        public string Role { get; set; }
        public Guid? WorkingDepartmentId { get; set; }
        [JsonProperty(propertyName: "working-department")]
        public Relationship<Department> WorkingDepartment { get; set; }
        // This is a Many-To-Many relationship and must have a intermediary entity (a PersonalDepartment) in order for ef to create
        // the appropriate migration; this is a high priority feature for EF 8 - don't worry about this for now. All personal have 
        // one department untill further notice.
        //[HasMany("departments")]
        //public IEnumerable<Department> Departments { get; set; }
        public Relationship<IEnumerable<Stamp>> Stamps { get; set; }
    }
}
