using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingMaestro.Http.Models.Request
{
	public class SuggestMeetingTimeRequestModel
	{
		[Required]
		[MinLength(1)]
		public IEnumerable<string> Ids { get; set; }

		[Range(30, 1440)]
		public int MeetingLengthInMinutes { get; set; }

		[Required]
		[DataType(DataType.DateTime)]
		public DateTime? AtEarliest { get; set; }

		[Required]
		[DataType(DataType.DateTime)]
		public DateTime? AtLatest { get; set; }

		[Range(0, 24)]
		public int StartHour { get; set; }

		[Range(0, 24)]
		public int EndHour { get; set; }
	}
}
