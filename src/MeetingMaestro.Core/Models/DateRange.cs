using System;

namespace MeetingMaestro.Core.Models
{
	public class DateRange
	{
		private DateTime _earliest;
		public DateTime Earliest
		{
			get { return _earliest; }
		}

		private DateTime _latest;
		public DateTime Latest
		{
			get { return _latest; }
		}

		public DateRange(DateTime start, DateTime end)
		{
			if (start <= end)
			{
				this._earliest = start;
				this._latest = end;
			}
			else
			{
				this._earliest = end;
				this._latest = start;
			}
		}

		public bool Overlaps(DateRange compare)
		{
			return this.Includes(compare.Earliest) || compare.Includes(this.Earliest);
		}

		public bool Includes(DateTime compare)
		{
			return (this.Earliest <= compare && this.Latest > compare);
		}
	}
}
