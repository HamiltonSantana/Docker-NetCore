using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ServerSide.Models;

namespace ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IHubContext<SignalHub> _hubContext;

        public UsersController(IHubContext<SignalHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetUsers()
        {
            using (var context = new ApplicationDbContext())
            {
                var users = context.Users.Select(_ => _).ToArray();
                _hubContext.Clients.All.SendAsync("SendNotification", users);
                return Ok(users);
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult Post([FromBody] User usr)
        {
            int id = 0;
            using(var context = new ApplicationDbContext())
            {
                var result = context.Users.Add(usr);
                context.SaveChanges();
                id = result.Entity.Id;
                var users = context.Users.Select(_ => _).ToArray();
            }
            _hubContext.Clients.All.SendAsync("NotificationMessage", $"Usuario inserido: {usr.Name}, {usr.Phone}");
            return Ok(id);
        }

        [Authorize]
        [HttpDelete]
        public IActionResult Delete([FromBody] User usr)
        {
            using (var context = new ApplicationDbContext())
            {
                var result = context.Users.Select(_ => _).Where( _ => _.Id == usr.Id).Single<User>();
                context.Users.Remove(result);
                context.SaveChanges();
            }
            _hubContext.Clients.All.SendAsync("NotificationMessage", $"Usuario deletado: {usr.Name}, {usr.Phone}");
            return Ok(200);
        }

    }
}
