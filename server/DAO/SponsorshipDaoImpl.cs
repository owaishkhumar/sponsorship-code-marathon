using SponsorshipBackend.Models;
using SponsorshipBackend.ViewModels;
using Npgsql;  

namespace SponsorshipBackend.DAO
{
    public class SponsorshipDaoImpl : SponsorshipDao
    {

        NpgsqlConnection _connection;

        public SponsorshipDaoImpl(NpgsqlConnection connection)
        {
            _connection = connection;
        }
        public async Task<int> AddPayment (Payment payment)
        {
            int rowInserted = 0;
            string? message;
            string insertQuery = $"INSERT INTO sponsorship.payments (ContractID, PaymentDate, AmountPaid, PaymentStatus) VALUES ({payment.ContractId}, '{payment.PaymentDate}', {payment.AmountPaid}, '{payment.PaymentStatus}')";

            try
            {
                using var cmd = _connection.CreateCommand();
                cmd.CommandText = insertQuery;
                await _connection.OpenAsync();
                rowInserted = await cmd.ExecuteNonQueryAsync();
                await _connection.CloseAsync();
            }
            catch (Exception ex)
            {
                message = $"Error inserting payment: {ex.Message}"; 
                Console.WriteLine(message);
            }
            return rowInserted;
        }


        public async Task<List<SponsorPayments>> GetSponsorPayments()
        {
            List<SponsorPayments> sponsorsPayment =  new List<SponsorPayments>();

            try
            {
                using var cmd = _connection.CreateCommand();
                cmd.CommandText = "select s.sponsorname, s.industrytype, s.contactemail, s.phone, sum(p.amountpaid) as totalpayment, count(p.paymentid) as noofpayment , max(paymentdate)  as latestpayment from sponsorship.sponsors s join sponsorship.contracts c on s.sponsorid = c.sponsorid join sponsorship.payments p on c.contractid = p.contractid group by s.sponsorname, s.sponsorname, s.industrytype, s.contactemail, s.phone";
                await _connection.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();
                if (reader is not null)
                {
                    sponsorsPayment = new List<SponsorPayments>();
                    while (await reader.ReadAsync())
                    {
                        SponsorPayments sp = new SponsorPayments
                        {
                            SponsorName = reader["sponsorname"]?.ToString(),
                            IndustryType = reader["industrytype"]?.ToString(),
                            ContactEmail = reader["contactemail"]?.ToString(),
                            Phone = reader["phone"]?.ToString(),
                            TotalPayment = reader.GetDecimal(reader.GetOrdinal("totalpayment")),
                            NoOfPayment = reader.GetInt32(reader.GetOrdinal("noofpayment")),
                            LatestPayment = reader["latestpayment"]?.ToString()
                        };
                        sponsorsPayment.Add(sp);
                    }
                }
                await _connection.CloseAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting sponsor payments: {ex.Message}");
            }
            return sponsorsPayment;
        }
        public async Task<List<MatchDetails>> GetMatchDetails()
        {
            List<MatchDetails> listMatch = new List<MatchDetails>();
            try
            {
                using var cmd = _connection.CreateCommand();
                cmd.CommandText = "select m.matchname, m.matchdate, m.location, sum(p.amountpaid) as totalamount from sponsorship.matches m join sponsorship.contracts c\ton m.matchid = c.matchid join sponsorship.payments p on p.contractid = c.contractid group by m.matchname, m.matchdate, m.location";
                await _connection.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();
                if (reader != null)
                {
                    while (await reader.ReadAsync())
                    {
                        MatchDetails match = new MatchDetails
                        {
                            MatchName = reader["matchname"]?.ToString(),
                            MatchDate = reader["matchdate"]?.ToString(),
                            Location = reader["location"]?.ToString(),
                            TotalAmount = reader.GetDecimal(reader.GetOrdinal("totalamount"))
                        };
                        listMatch.Add(match);
                    }
                }
                await _connection.CloseAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting sponsor payments: {ex.Message}");
            }
            return listMatch;
        }

        public async Task<List<SponsorMatches>> GetSponsorMatches(int year)
        {
            List<SponsorMatches> sponsorMatches = new List<SponsorMatches>();
            try
            {
                using var cmd = _connection.CreateCommand();
                cmd.CommandText = $"select  s.sponsorname, count(m.*) as numberofmatches from sponsorship.matches m join sponsorship.contracts c on m.matchid = c.matchid join sponsorship.sponsors s on s.sponsorid = c.sponsorid where extract (year from m.matchdate) = {year} group by s.sponsorname";
                await _connection.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();
                if (reader != null)
                {
                    while (await reader.ReadAsync())
                    {
                        SponsorMatches sm = new SponsorMatches
                        {
                            SponsorName = reader["sponsorname"].ToString(),
                            NumberOfMatches = reader.GetInt32(reader.GetOrdinal("numberofmatches"))
                        };
                        sponsorMatches.Add(sm);
                    }
                }
                await _connection.CloseAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting sponsor payments: {ex.Message}");
            }
            return sponsorMatches;
        }
    }
}
