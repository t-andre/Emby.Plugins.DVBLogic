using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSoft.TVServer.Entities
{
    /// <summary> Information about the tuner. </summary>
    public class TunerInfo
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Entities.TunerInfo class. </summary>
        public TunerInfo()
        {
            IsEnabled = true;
            Id = "908D12B6-DE6A-4920-B8CF-5229238F725A";
        }

        #endregion

        #region Properties
        /// <summary> Gets or sets the identifier. </summary>
        /// <value> The identifier. </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this TSoft.TVServer.Entities.TunerInfo is
        /// enabled. </summary>
        /// <value>
        /// true if this TSoft.TVServer.Entities.TunerInfo is enabled, false if not. </value>
        public bool IsEnabled { get; set; }

        /// <summary> Gets or sets the type. </summary>
        /// <value> The type. </value>
        public string Type { get; set; }

        #endregion

    }
}
