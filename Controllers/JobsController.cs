using BlackBoxInc.Services;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace BlackBoxInc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : Controller
    {
        private readonly IEmailService _mailMan;
        public JobsController(IEmailService mailMan)
        {
            _mailMan = mailMan;
        }
        // GET
        [HttpPost]
        [Route("CreateBackgroundJob")]
        public ActionResult CreateBackgroundJob()
        {
            BackgroundJob.Enqueue(() => Console.WriteLine("Background task created!!"));
            return Ok();
        }

        [HttpPost]
        [Route("ScheduledJob")]
        public ActionResult ScheduledJob()
        {
            var time = DateTime.UtcNow.AddSeconds(5);
            var offset = new DateTimeOffset(time);
            BackgroundJob.Schedule(() => Console.WriteLine($"Scheduled job triggered"), offset);
            return Ok();
        }

        [HttpPost]
        [Route("ContinuingJobs")]
        public ActionResult ContinuingJobs()
        {
            var time = DateTime.UtcNow.AddSeconds(5);
            var offset = new DateTimeOffset(time);
            var jobId = BackgroundJob.Schedule(() => Console.WriteLine($"Scheduled job 2 triggered"), offset);

            var job2Id = BackgroundJob.ContinueJobWith(jobId, () => Console.WriteLine("Scheduled job 3 triggered"));
            var job3Id = BackgroundJob.ContinueJobWith(job2Id, () => Console.WriteLine("Scheduled job 4 triggered"));
            return Ok();
        }

        [HttpPost]
        [Route("RecurringJobs")]
        public ActionResult RecurringJobs()
        {
            RecurringJob.AddOrUpdate("RecurringJob1", () => Console.WriteLine("RecurringJobs 1"), "*/2 * * * * *");
            return Ok();
        }

        [HttpPost]
        [Route("NotifyUserWhileSignedIn")]
        public ActionResult UpdateUserByMail()
        {
            // BackgroundJob.Enqueue<IEmailService>(x => x.SendEmailAsync("ikeoluwa.jesse@gmail.com", "Reminder",
                // "This is a friendly reminder that you are currently logged in on the application.  If you have already signed out and you receive this mail, kindly contact support"));
            RecurringJob.AddOrUpdate<IEmailService>("Follow Up", x => x.SendEmailAsync("ikeoluwa.jesse@gmail.com", "Reminder", "This is a friendly reminder that you are currently logged in on the application.  If you have already signed out and you recieve this mail, kindly contact support"), "*/2 * * * * *");
            return Ok();
        }
    }   
}