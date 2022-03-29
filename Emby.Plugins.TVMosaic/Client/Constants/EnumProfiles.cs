using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TSoft.TVServer.Constants
{
    /// <summary> Values that represent enumeration profiles. </summary>
	public enum EnumProfiles
	{
        /// <summary> An enum constant representing the native option. </summary>
		NATIVE,

        /// <summary> An enum constant representing the tsmobile option. </summary>
		TSMOBILE,

        /// <summary> An enum constant representing the tspassthrough option. </summary>
		TSPASSTHROUGH,

        /// <summary> An enum constant representing the ts 1080 option. </summary>
		TS1080,

        /// <summary> An enum constant representing the ts 720 option. </summary>
		TS720,

        /// <summary> An enum constant representing the ts 576 option. </summary>
		TS576, 

        /// <summary> An enum constant representing the ts 480 option. </summary>
		TS480,

        /// <summary> An enum constant representing the ts 360 option. </summary>
		TS360,

        /// <summary> An enum constant representing the ts 240 option. </summary>
		TS240,
	}
}
