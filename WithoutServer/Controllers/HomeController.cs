using CommonClassLib;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace WithoutServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IDisplayMessage _displayMessage;
        public HomeController(IBackgroundJobClient backgroundJobClient, IDisplayMessage displayMessage)
        {
            _backgroundJobClient = backgroundJobClient;
            _displayMessage = displayMessage;
        }
        public ActionResult StartAJob()
        {
            _backgroundJobClient.Enqueue(() => _displayMessage.DisplyText("From Without-Server"));
            return Ok("Success");
        }
    }
}
