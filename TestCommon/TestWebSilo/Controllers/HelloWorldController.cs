using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using System.Threading.Tasks;
using TestGrainInterfaces;

namespace TestWebSilo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize]
    public class HelloWorldController : Controller
    {
        private readonly IGrainFactory grainFactory;

        public HelloWorldController(IGrainFactory grainFactory)
        {
            this.grainFactory = grainFactory;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetHelloWorldAsync(int grainId)
        {
            var grain = grainFactory.GetGrain<IHello>(grainId);
            var result =  await grain.SayHello("afafafa");
            return Ok(result);
        }
    }
}
