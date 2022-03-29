using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSoft.TVServer.Helpers
{
	/// <summary> A file system. </summary>
	public static class FileSystem
	{
		#region [Public methods]

		/// <summary> Sanitize path. </summary>
		/// <param name="name"> The name. </param>
		/// <returns> A string. </returns>
		public static string SanitizePath(string name)
		{
			return SanitizePath(name, '_');
		}

		/// <summary> Sanitize path. </summary>
		/// <param name="path"> Full pathname of the file. </param>
		/// <param name="replaceValue"> The replace value. </param>
		/// <returns> A string. </returns>
		public static string SanitizePath(string path, char replaceValue)
		{
			int filenamePos = path.LastIndexOf(Path.DirectorySeparatorChar) + 1;
            StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(path, 0, filenamePos);
			for (int i = filenamePos; i < path.Length; i++)
			{
				char filenameChar = path[i];
				foreach (char c in Path.GetInvalidFileNameChars())
				{
					if (filenameChar.Equals(c))
					{
						filenameChar = replaceValue;
						break;
					}
				}
				sb.Append(filenameChar);
			}
			return sb.ToString();
		}
		#endregion
	}
}
