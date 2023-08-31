using System.Threading.Tasks;
using ApplicationLayer.Profiles;
using Microsoft.AspNetCore.Mvc;
using ReactivityAPI.Controllers;

namespace Reactivity.Controllers
{
    public class ProfilesController : BaseApiController
    {
        [HttpGet("{username}")]
        public async Task<IActionResult> GetProfile(string username)
        {
            return HandleResult(await Mediator.Send(new Details.Query{Username = username}));
        }

        [HttpGet("{username}/activities")]
        public async Task<IActionResult> GetUserActivities(string username, string predicate)
        {
            return HandleResult(await Mediator.Send(new ListActivities.Query {Username = username, Predicate = predicate}));
        }
    }
}