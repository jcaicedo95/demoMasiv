
using DemoMasiv.Core.LogicLayer;
using DemoMasiv.Core.LogicLayer.Class;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoMasiv.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GamblingRouletteController : ControllerBase
    {
        private readonly IRedisCacheService _redisCacheService;
        public GamblingRouletteController(IRedisCacheService redisCacheService)
        {
            _redisCacheService = redisCacheService;
        }

        [HttpPost]
        [Route("createRoulette")]
        public async Task<IActionResult> CreateRoulte()
        {
            var idRoulette = await _redisCacheService.CreateRoulette();
            return Ok(idRoulette);
        }

        [HttpPost]
        [Route("startRoulette")]
        public async Task<IActionResult> StartRoulette([FromBody]int idRoulette)
        {
            var activatedRoulette = await _redisCacheService.StartRoulete(idRoulette);
            return Ok(activatedRoulette);
        }

        [HttpPost]
        [Route("gamblingAOnNumber")]
        public async Task<IActionResult> GamblingAOnNumber([FromBody] 
                                            BetRouletteRequest betRouletteRequest)
        {
            var UserId = Request.Headers["UserId"].ToString();
            if(UserId != "" && UserId != null)
            {
                var betExist = await _redisCacheService
                    .GamblingAOnNumber(betRouletteRequest, Convert.ToInt32(UserId));
                return Ok(betExist);
            }
            else {
                return BadRequest("No se ha proporcionado Id de usuario autenticado");
            }
            
        }

        [HttpPost]
        [Route("closeGamblingRoulette")]
        public async Task<IActionResult> CloseGamblingRoulette([FromBody] int idRoulette)
        {
            var gamblingObjetc = await _redisCacheService.CloseGamblingRoulette(idRoulette);
            return Ok(gamblingObjetc);
        }

        [HttpPost]
        [Route("inventoryGlamblingRoulettes")]
        public async Task<IActionResult> InventoryGlamblingRoulettes()
        {
            var inventoryRoulettes = await _redisCacheService.InventoryRoulettesResults();
            return Ok(inventoryRoulettes);
        }


    }
}
