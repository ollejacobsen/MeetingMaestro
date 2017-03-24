using System;
using System.Collections.Generic;
using System.Text;

namespace MeetingMaestro.Core.Import.Models
{
    public class ImportStatistics
    {
		public TimeSpan ExecutionTime { get; set; }
		public int NrOfEmptyRows;
		public int NrOfDuplicatedEmployees;
		public List<string> InvalidRows { get; set; }
		public List<string> UnhandledRows { get; set; }
		
		public ImportStatistics()
		{
			this.InvalidRows = new List<string>();
			this.UnhandledRows = new List<string>();
		}
	}
}
