using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SponsorshipBackend.DAO;
using SponsorshipBackend.Models;
using SponsorshipBackend.ViewModels;

namespace SponsorshipBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SponsorshipController : ControllerBase
    {
        public readonly SponsorshipDao _sponsorshipDao;

        public SponsorshipController(SponsorshipDao sponsorshipDao)
        {
            _sponsorshipDao = sponsorshipDao;
        }

        [HttpPost]
        public async Task<ActionResult<int>> AddPayment(Payment payment)
        {
            if (payment != null)
            {
                if (ModelState.IsValid)
                {
                    int res = await _sponsorshipDao.AddPayment(payment);
                    if (res > 0)
                    {
                        return Ok("Payment Added Successfully: "+res);
                    }
                    return BadRequest("Cannot Add the payment");
                }
                return BadRequest("Provide valid payment fields");
            }
            else
            {
                return BadRequest("Payment Not Found");
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<SponsorPayments>>> GetSponsorPayments()
        {
            List<SponsorPayments> sp =  await _sponsorshipDao.GetSponsorPayments();
            if (sp != null)
            {
                return Ok(sp);
            }
            else
            {
                return BadRequest("Not Found");
            }
        }

        [HttpGet("match-details")]
        public async Task<ActionResult<List<MatchDetails>>> GetMatchDetails()
        {
            List<MatchDetails> md = await _sponsorshipDao.GetMatchDetails();
            if (md != null)
            {
                return Ok(md);
            }
            else
            {
                return BadRequest("Not Found");
            }
        }

        [HttpGet("sponsor-matches")]
        public async Task<ActionResult<List<SponsorMatches>>> GetSponsorsMatches([FromQuery] int year)
        {
            List<SponsorMatches> sm = await _sponsorshipDao.GetSponsorMatches(year);
            if(sm != null)
            {
                return Ok(sm);
            }
            else
            {
                return BadRequest("Not Found");
            }
        }
    }
}
