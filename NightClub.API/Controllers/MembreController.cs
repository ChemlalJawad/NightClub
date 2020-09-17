using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NightClub.Service.Membre;
using NightClub.Service.Membre.Requete;

namespace NightClub.API.Controllers
{
    [Route("api/membres")]
    [ApiController]
    public class MembreController : ControllerBase
    {
        private readonly IMembreService _membreService;

        public MembreController(IMembreService membreService)
        {
            _membreService = membreService;
        }

        [HttpPost]
        public ActionResult CreateMember([FromBody] CreerMembreRequete requete)
        {
            var membre = _membreService.CreerMembre(requete);

            //var membreViewModel = _mapper.Map<GetMemberViewModel>(member);

            return Ok(membre);
        }

        [HttpPost("{membreId}/blacklist")]      
        public ActionResult BlacklisterMembre([FromRoute] int membreId, [FromBody] BlacklisterMembreRequete requete)
        {
            requete.MembreId = membreId;
            var membre = _membreService.BlacklisterMembre(requete);

            //var membreViewModel = _mapper.Map<GetMemberViewModel>(member);

            return Ok(membre);
        }

        [HttpPost("{membreId}/modifierMembre")]
        public ActionResult Modifiermembre([FromRoute] int membreId, [FromBody] ModifierMembreRequete requete)
        {
            requete.MembreId = membreId;
            var membre = _membreService.ModifierMembre(requete);

            //var membreViewModel = _mapper.Map<GetMemberViewModel>(member);

            return Ok(membre);
        }
        
        [HttpPost("{membreId}/nouvelleCarteMembre")]
        public ActionResult GenererNouvelleCarteMembre([FromRoute] int membreId)
        {
            
            var membre = _membreService.GenerNouvelleCarteMembre(membreId);

            //var membreViewModel = _mapper.Map<GetMemberViewModel>(member);

            return Ok(membre);
        }



    }
}
