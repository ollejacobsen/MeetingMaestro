using MeetingMaestro.Core.Import.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MeetingMaestro.Core.Import
{
	public class DataCollector
	{
		private List<ImportedEmployee> _employees;
		private List<ImportedBooking> _bookings;
		private Stopwatch _stopwatch;

		public IReadOnlyList<ImportedBooking> Bookings { get { return _bookings; } }

		private Dictionary<ImportedEmployee, List<ImportedBooking>> _bookingsByEmployee;
		public IReadOnlyDictionary<ImportedEmployee, List<ImportedBooking>> BookingsByEmployee { get { return _bookingsByEmployee; } }
		public ImportStatistics Statistics { get; private set; }

		public DataCollector()
		{
			_employees = new List<ImportedEmployee>();
			_bookings = new List<ImportedBooking>();
			_bookingsByEmployee = new Dictionary<ImportedEmployee, List<ImportedBooking>>();
			_stopwatch = new Stopwatch();

			this.Statistics = new ImportStatistics();
		}

		internal void Begin()
		{
			_stopwatch.Start();
		}

		internal void Done()
		{
			foreach (var em in _employees)
			{
				// Check so the employee isn't already added
				if (!_bookingsByEmployee.ContainsKey(em))
				{
					_bookingsByEmployee.Add(em, _bookings.Where(b => b.EmployeeId == em.Id).ToList());
				}
				else
				{
					Statistics.NrOfDuplicatedEmployees++;
				}
			}

			_stopwatch.Stop();

			Statistics.ExecutionTime = _stopwatch.Elapsed;
		}

		internal void Deserialize(string line)
		{
			if (string.IsNullOrWhiteSpace(line))
			{
				Statistics.NrOfEmptyRows++;
				return;
			}

			var content = line.Split(';');
			if (content.Length == 2) // <- Assume employee
			{
				var mappingResult = ImportedEmployee.ValidateAndMap(content);
				if (mappingResult.valid)
				{
					_employees.Add(mappingResult.employee);
				}
				else
				{
					Statistics.InvalidRows.Add(line);
				}
			}
			else if (content.Length >= 3) // <- Assume booking
			{
				var mappingResult = ImportedBooking.ValidateAndMap(content);
				if (mappingResult.valid)
				{
					_bookings.Add(mappingResult.booking);
				}
				else
				{
					Statistics.InvalidRows.Add(line);
				}
			}
			else
			{
				Statistics.UnhandledRows.Add(line);
			}
		}
	}
}
