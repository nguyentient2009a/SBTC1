using Microsoft.AspNetCore.Mvc;
using SBTC1.Models.Entilities;

namespace SBTC1.Models.IEntityRepositories
{
    public interface ITicketRepository
    {
        Ticket Add(Ticket ticket);
        Ticket Update(Ticket ticketChanges);
        bool Delete(int ticketId);
        bool UpdateTicketStatus(int ticketId, string newStatus);
        bool AddUserRating(int ticketId, int userRating);
        Ticket GetTicket(int ticketId);
        Ticket GetTicketFromPEmailAndTicketId(int ticketId, string pEmail);
        List<Ticket> GetAllTicketsFromTrainRouteIdDateAndStatus(string trainName, DateTime dateOfJourney, string ticketStatus);
        List<Ticket> GetAllTicketsFromTrainRouteIdAndDate(string trainName, DateTime dateOfJourney);
    }
}
