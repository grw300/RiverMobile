using System;
using System.Collections.Generic;
using System.Text;

namespace RiverMobile.Models
{
    /// <summary>
    /// A region used to detect iBeacon hardware.
    /// </summary>
    public class BeaconRegion
    {
        Guid guid;
        /// <summary>
        /// The unique ID of the beacons being targeted.
        /// This property must be parsable as a <seealso cref="Guid"/>.
        /// </summary>
        public string Uuid
        {
            get
            {
                return guid.ToString();
            }
            private set
            {
                if (guid.ToString() == value)
                    return;
                guid = new Guid(value);
            }
        }
        public ushort? Major { get; private set; }
        public ushort? Minor { get; private set; }
        public string Id { get; private set; }

        /// <summary>
        /// Constructs a representation of a region used to detect iBeacon hardware.
        /// </summary>
        /// <param name="guid">The unique ID of the beacons being targeted.
        /// This property must be parsable as a <seealso cref="Guid"/></param>
        /// <param name="id">A unique identifier to associate with the returned
        /// region object. You use this identifier to differentiate regions
        /// within your application.</param>
        /// <param name="major">The value identifying a group of beacons.</param>
        /// <param name="minor">The value identifying a specific beacon within a group.</param>
        public BeaconRegion(string guid, string id, ushort? major, ushort? minor)
        {
            Uuid = guid;
            Major = major;
            Minor = minor;
            Id = id;
        }

        public BeaconRegion(string guid, string id, ushort? major)
            : this(guid, id, major, null)
        { }

        public BeaconRegion(string guid, string id)
            : this(guid, id, null, null)
        { }
    }
}
