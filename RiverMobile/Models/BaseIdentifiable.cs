using System;
using System.ComponentModel;

namespace RiverMobile.Models
{
    public abstract class BaseIdentifiable
    {
        public abstract string Type { get; set; }
        public Guid? Id { get; set; }

        public DateTime Created { get; set; }
    }
}
