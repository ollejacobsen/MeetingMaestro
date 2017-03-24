using Microsoft.AspNetCore.Mvc;
using System.Text;
using MeetingMaestro.Http.Models;
using Microsoft.Extensions.Options;

namespace MeetingMaestro.Http.Controllers
{
	[Route("")]
    public class RootController : Controller
    {
		private MaestroConfiguration _config;

		public RootController(IOptions<MaestroConfiguration> config)
		{
			_config = config.Value;
		}

        public string Index()
        {
			var exampleModel = new
			{
				ids = new[] { "103222943108469712161093620402295866178", "276908764613820584354290536660008166629" },
				atEarliest = "2015-01-20",
				atLatest = "2015-01-20",
				startHour = 8,
				endHour = 17,
				meetingLengthInMinutes = 60
			};

			var message = new StringBuilder();
			message.AppendLine("Välkommen till MeetingMaestro!");
			message.AppendLine($"Vid uppstart läser websajten in följande fil: {_config.ImportFilePath}");
			message.AppendLine("Det finns en endpoint som är intressant [GET] /maestro som tar nedantsående exempelmodell:");
			message.AppendLine($"\n {Newtonsoft.Json.JsonConvert.SerializeObject(exampleModel, Newtonsoft.Json.Formatting.Indented)} \n");
			message.AppendLine("/maestro?ids=103222943108469712161093620402295866178&ids=276908764613820584354290536660008166629&MeetingLengthInMinutes=60&AtEarliest=2015-01-20&AtLatest=2015-01-20&startHour=8&endHour=17");


			return message.ToString();
        }
    }
}