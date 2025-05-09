using BL.Interfaces;
using Domain.CitizenPanel;
using Microsoft.AspNetCore.Mvc;
using UI_MVC.Models.Dto.communeDtos;

namespace UI_MVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommuneController : ControllerBase
    {
        private readonly ICommuneManager _communeManager;

        public CommuneController(ICommuneManager communeManager)
        {
            _communeManager = communeManager;
        }

        // GET: api/Commune
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Commune>>> GetCommunes()
        {
            var communes = await _communeManager.GetCommunes();
            return Ok(communes);
        }
        
        // GET: api/Commune/basic
        [HttpGet("basic")]
        public async Task<ActionResult<IEnumerable<CommuneBasicDto>>> GetCommunesBasic()
        {
            var communes = await _communeManager.GetCommunesBasic();
            return Ok(communes);
        }
    }
}