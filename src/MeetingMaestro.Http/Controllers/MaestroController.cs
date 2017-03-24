using MeetingMaestro.Core.Services;
using MeetingMaestro.Http.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace MeetingMaestro.Http.Controllers
{
	[Route("maestro")]
	public class MaestroController : Controller
    {
		private MeetingService _meetingService;
		
		public MaestroController(MeetingService meetingService)
		{
			_meetingService = meetingService;
		}

		[HttpGet]
		public IActionResult SuggestMeetingTime(SuggestMeetingTimeRequestModel model)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var chunks = _meetingService.FindAvailableMeetingTimes(model.Ids, 
				model.MeetingLengthInMinutes, 
				model.AtEarliest.Value, 
				model.AtLatest.Value, 
				model.StartHour, 
				model.EndHour);

			return Ok(chunks);
		} 
	}
}
