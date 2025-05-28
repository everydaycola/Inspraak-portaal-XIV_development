using BL.Interfaces;
using Domain.GlobalDtos;
using Microsoft.AspNetCore.Mvc;

namespace UI_MVC.Controllers.api
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommunesController : ControllerBase
    {
        private readonly ICommuneManager _communeManager;

        public CommunesController(ICommuneManager communeManager)
        {
            _communeManager = communeManager;
        }

        // GET: api/Communes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Commune>>> GetCommunes()
        {
            var communes = await _communeManager.GetCommunes();
            return Ok(communes);
        }
        
        // GET: api/Communes/basic
        [HttpGet("basic")]
        public async Task<ActionResult<IEnumerable<CommuneBasicDto>>> GetCommunesBasic()
        {
            var communes = await _communeManager.GetCommunesBasic();
            return Ok(communes);
        }
    }
}