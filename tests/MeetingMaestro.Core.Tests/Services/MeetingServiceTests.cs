using MeetingMaestro.Core.Import.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MeetingMaestro.Core.Tests.Services
{
	public class MeetingServiceTests
	{
		[Theory]
		[InlineData("freebusy-subset.txt")]
        public void GetBookings_GivenValidId_Returns3Bookings(string filename)
		{
			var collector = Import.DataImporter.ParseFile(ResourceHelper.GetFileInfo(filename));
			var srv = new Core.Services.MeetingService(collector.Bookings);

			var irmasBookings = srv.GetBookings("103222943108469712161093620402295866178");

			Assert.Equal(3, irmasBookings.Count());
		}

		[Fact]
		public void CreateChunks_GivenLength30During1Hour_ShouldReturnTwoChunks()
		{
			var srv = new Core.Services.MeetingService(new List<ImportedBooking>());

			var chunks = srv.CreateDateRangeChunks( 
				lengthInMinutes: 30,
				earliest: DateTime.Parse("2016-01-02"),
				latest: DateTime.Parse("2016-01-02"),
				startHour: 7,
				endHour: 8);

			Assert.Equal(2, chunks.Count);
		}

		[Fact]
		public void CreateChunks_GivenA24hPeriod_ShouldReturn48Chunks()
		{
			var srv = new Core.Services.MeetingService(new List<ImportedBooking>());

			var chunks = srv.CreateDateRangeChunks(
				lengthInMinutes: 30,
				earliest: DateTime.Parse("2016-01-02"),
				latest: DateTime.Parse("2016-01-02"));

			Assert.Equal(48, chunks.Count);
		}

		[Fact]
		public void FindAvailableMeetingTimes_GivenSingleEmployeeWithKnownAvailability_ShouldReturn2Chunks()
		{
			var collector = Import.DataImporter.ParseFile(ResourceHelper.GetFileInfo("freebusy-subset.txt"));
			var srv = new Core.Services.MeetingService(collector.Bookings);

			var availableTimes = srv.FindAvailableMeetingTimes(new[] { "103222943108469712161093620402295866178" }, 60,
				DateTime.Parse("2016-01-02"), DateTime.Parse("2016-01-02"), 8, 10);

			Assert.Equal(2, availableTimes.Count);
		}

		[Fact]
		public void FindAvailableMeetingTimes_GivenMultipleEmployesWithKnownAvailability_ShouldReturn2Chunks()
		{
			var collector = Import.DataImporter.ParseFile(ResourceHelper.GetFileInfo("freebusy-subset.txt"));
			var srv = new Core.Services.MeetingService(collector.Bookings);

			var availableTimes = srv.FindAvailableMeetingTimes(new[] {
					"103222943108469712161093620402295866178", "276908764613820584354290536660008166629" }, 
					60, DateTime.Parse("2016-01-02"), DateTime.Parse("2016-01-02"), 8, 10);

			Assert.Equal(2, availableTimes.Count);
		}

		[Fact]
		public void FindAvailableMeetingTimes_GivenSingleEmployeeWithKnownUnavailability_ShouldReturn0Chunks()
		{
			var collector = Import.DataImporter.ParseFile(ResourceHelper.GetFileInfo("freebusy-subset.txt"));
			var srv = new Core.Services.MeetingService(collector.Bookings);

			var availableTimes = srv.FindAvailableMeetingTimes(new[] { "103222943108469712161093620402295866178" }, 60,
				DateTime.Parse("2015-01-20"), DateTime.Parse("2015-01-20"), 8, 13);

			Assert.Equal(0, availableTimes.Count);
		}


		[Fact]
		public void FindAvailableMeetingTimes_GivenSingleEmployeeWithKnownSemiAvailability_ShouldReturn1Chunk()
		{
			var collector = Import.DataImporter.ParseFile(ResourceHelper.GetFileInfo("freebusy-subset.txt"));
			var srv = new Core.Services.MeetingService(collector.Bookings);

			var availableTimes = srv.FindAvailableMeetingTimes(new[] { "103222943108469712161093620402295866178" }, 60,
				DateTime.Parse("2015-01-20"), DateTime.Parse("2015-01-20"), 8, 14);

			Assert.Equal(1, availableTimes.Count);
		}
	}
}
