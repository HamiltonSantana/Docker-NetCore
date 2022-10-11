using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ServerSide.Domain.Models;
using ServerSide.Infra.Context;
using ServerSide.Infra.Repository.Interface;

namespace ServerSide.Api.Controllers
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
        public IActionResult GetUsers([FromServices] IUserRepository _userRepository)
        {
            var users = _userRepository.GetUsers();
            _hubContext.Clients.All.SendAsync("SendNotification", users);
            return Ok(users);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Post([FromBody] User usr)
        {
            int id = 0;
            using(var context = new DatabaseContext())
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
        public IActionResult Delete([FromBody] int id)
        {
            using (var context = new DatabaseContext())
            {   
                var result = context.Users.Select(_ => _).Where( _ => _.Id == id).Single<User>();
                context.Users.Remove(result);
                context.SaveChanges();
                _hubContext.Clients.All.SendAsync("NotificationMessage", $"Usuario deletado: {result.Name}, {result.Phone}");
            }
            return Ok(200);
        }

        [Authorize]
        [HttpPut]
        public IActionResult Put([FromBody] User usr)
        {
            using (var context = new DatabaseContext())
            {
                var result = context.Users.Select(_ => _).Where(_ => _.Id == usr.Id).Single<User>();
                result.Phone = usr.Phone;
                result.Pwd = usr.Pwd;
                context.Users.Update(result);
                context.SaveChanges();
            }
            _hubContext.Clients.All.SendAsync("NotificationMessage", $"Usuario atualizado: {usr.Name}, {usr.Phone}");
            return Ok(200);
        }
    }
}
