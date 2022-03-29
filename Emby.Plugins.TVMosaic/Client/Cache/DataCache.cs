using System;
using System.Collections.Generic;
using System.Linq;

namespace TSoft.TVServer.Cache
{
    /// <summary> A data cache. </summary>
    /// <typeparam name="T"> Generic type parameter. </typeparam>
    public class DataCache<T>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the TSoft.TVServer.Cache.DataCache&lt;T&gt; class. </summary>
        public DataCache()
        {
            this.IsValid = true;
        }
        #endregion

        #region Properties
        /// <summary> Gets or sets the data. </summary>
        /// <value> The data. </value>
        public List<T> Data { get; set; }

        /// <summary> Gets or sets the duration. </summary>
        /// <value> The duration. </value>
        public TimeSpan Duration { get; set; }

        /// <summary> Gets or sets the Date/Time of the date. </summary>
        /// <value> The date. </value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this TSoft.TVServer.Cache.DataCache&lt;T&gt;
        /// is valid. </summary>
        /// <value>
        /// True if this TSoft.TVServer.Cache.DataCache&lt;T&gt; is valid, false if not. </value>
        public bool IsValid { get; set; }

        #endregion

    }
}
