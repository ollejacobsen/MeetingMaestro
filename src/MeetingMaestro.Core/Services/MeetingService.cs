using MeetingMaestro.Core.Import.Models;
using MeetingMaestro.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetingMaestro.Core.Services
{
    public class MeetingService
    {
		private List<ImportedBooking> _data;

		public MeetingService(IEnumerable<ImportedBooking> data)
		{
			_data = data.ToList();
		}

		public IEnumerable<ImportedBooking> GetBookings(string employeeId)
		{
			return _data
				.Where(x => x.EmployeeId == employeeId);
		}

		public IList<DateRange> FindAvailableMeetingTimes(IEnumerable<string> employeeIds, 
			int lengthInMinutes, 
			DateTime earliest, 
			DateTime latest,
			int startHour = 0,
			int endHour= 0)
		{
			if (employeeIds == null)
				throw new ArgumentNullException(nameof(employeeIds));
			if (employeeIds.Count() == 0)
				throw new ArgumentOutOfRangeException(nameof(employeeIds), "You must supply atleast one employee id");

			// Dirty: Sanitize the hour input
			if (startHour < 0 || startHour > 23) startHour = 0;
			if (endHour <= 0 || endHour > 24) endHour = 24;

			// Create chunks to check against
			var chunks = CreateDateRangeChunks(lengthInMinutes, earliest, latest, startHour, endHour);
			var chunkRange = new DateRange(chunks.First().Earliest, chunks.Last().Latest);

			// Get all relevant bookings for the employees during the period
			var possibleInterferingBookings = _data
					.Where(x => employeeIds.Contains(x.EmployeeId) && chunks.Any(r => x.Range.Overlaps(r)))
					.ToList();

			// Remove all chunks that overlaps
			var availableChunks = chunks
				.Where(x => !possibleInterferingBookings.Any(b => b.Range.Overlaps(x)))
				.ToList();

			// Also create a between times
			// filter away nonexisting employees
			return availableChunks;
		}

		internal List<DateRange> CreateDateRangeChunks(int lengthInMinutes,
			DateTime earliest,
			DateTime latest,
			int startHour = 0,
			int endHour = 0)
		{
			var chunks = new List<DateRange>();

			// Dirty: Sanitize the hour input
			if (startHour < 0 || startHour > 23) startHour = 0;
			if (endHour <= 0 || endHour > 24) endHour = 24;

			var startItem = new DateTime(earliest.Year, earliest.Month, earliest.Day, startHour, 0, 0);
			var endItem = endHour > 23
				? new DateTime(latest.Year, latest.Month, latest.Day, 23, 59, 59)
				: new DateTime(latest.Year, latest.Month, latest.Day, endHour, 0, 0);

			var iterationItem = startItem;

			while (iterationItem < endItem)
			{
				var chunk = new DateRange(iterationItem, iterationItem.AddMinutes(lengthInMinutes));
				iterationItem = chunk.Latest;

				chunks.Add(chunk);
			}

			return chunks;
		}
	}
}
