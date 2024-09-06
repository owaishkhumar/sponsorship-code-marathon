
using SponsorshipBackend.Models;
using SponsorshipBackend.ViewModels;

namespace SponsorshipBackend.DAO
{
    public interface SponsorshipDao
    {
        public Task<int> AddPayment (Payment payment);

        public Task<List<SponsorPayments>> GetSponsorPayments();

        public Task<List<MatchDetails>> GetMatchDetails();

        public Task<List<SponsorMatches>> GetSponsorMatches(int year);

    }
}
