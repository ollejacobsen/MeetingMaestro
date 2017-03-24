using System.IO;

namespace MeetingMaestro.Core.Import
{
	public class DataImporter
    {
		public static DataCollector ParseFile(string filePath)
		{
			return ParseFile(new FileInfo(filePath));
		}

		public static DataCollector ParseFile(FileInfo fileInfo)
		{
			if (!fileInfo.Exists)
			{
				throw new FileNotFoundException("Unable to find file with filename", fileInfo.FullName);
			}

			var collector = new DataCollector();
			using (var reader = fileInfo.OpenText())
			{
				collector.Begin();

				while (!reader.EndOfStream)
				{
					collector.Deserialize(reader.ReadLine());
				}

				collector.Done();
			}

			return collector;
		}
	}
}
