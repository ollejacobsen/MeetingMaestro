using System;
using System.IO;

namespace MeetingMaestro.Core.Tests
{
	public class ResourceHelper
    {
		public static string ResourceDirectory()
		{
			return Path.Combine(AppContext.BaseDirectory, "../../../@resources/");
		}

		public static FileInfo GetFileInfo(string filename)
		{
			var fileInfo = new FileInfo($"{ResourceDirectory()}{filename}");
			if (!fileInfo.Exists)
			{
				throw new FileNotFoundException("Unable to find file with filename", fileInfo.FullName);
			}

			return fileInfo;
		}

		public static string GetFileContent(string filename)
		{
			var fileInfo = GetFileInfo(filename);

			var content = string.Empty;

			using (var r = fileInfo.OpenText())
			{
				content = r.ReadToEnd();
			}

			return content;
		}
	}
}
