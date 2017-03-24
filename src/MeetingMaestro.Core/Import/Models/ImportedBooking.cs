using MeetingMaestro.Core.Models;
using System;

namespace MeetingMaestro.Core.Import.Models
{
	public class ImportedBooking
	{
		public string EmployeeId { get; set; }
		public DateRange Range { get; set; }
		public string Data { get; set; }

		public ImportedBooking(string employeeId, DateTime firstDate, DateTime secondDate)
		{
			this.EmployeeId = employeeId;
			this.Range = new DateRange(firstDate, secondDate);
		}

		public static (bool valid, ImportedBooking booking) ValidateAndMap(string[] content)
		{
			var employeeId = string.IsNullOrWhiteSpace(content[0]) ? null : content[0];
			var validFirstDate = DateTime.TryParse(content[1], out DateTime firstDate);
			var validSecondDate = DateTime.TryParse(content[2], out DateTime secondDate);
			var data = content[3];

			if (employeeId != null && validFirstDate && validSecondDate)
			{
				var model = new ImportedBooking(employeeId, firstDate, secondDate)
				{
					Data = data
				};
				return (true, model);
			}

			return (false, null);
		}
	}
}
