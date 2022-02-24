using Microsoft.EntityFrameworkCore;
using SBTC1.Models.Entilities;
using SBTC1.Models.IEntityRepositories;

namespace SBTC1.Models.Repositories
{

    public class TicketRepository : ITicketRepository
    {
        private readonly AppDbContext context;
        public TicketRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Ticket Add(Ticket ticket)
        {
            context.Ticket.Add(ticket);
            context.SaveChanges();
            return ticket;
        }

        public bool AddUserRating(int ticketId, int userRating)
        {
            Ticket t = GetTicket(ticketId);
            t.PUserRatings = userRating;
            t = Update(t);
            if (t == null)
            {
                return false;
            }
            return true;
        }

        public bool Delete(int ticketId)
        {
            Ticket ticket = GetTicket(ticketId);
            if (ticket != null)
            {
                context.Ticket.Remove(ticket);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<Ticket> GetAllTicketsFromTrainRouteIdAndDate(string trainName, DateTime dateOfJourney)
        {
            throw new NotImplementedException();
        }

        public List<Ticket> GetAllTicketsFromTrainRouteIdDateAndStatus(string trainName, DateTime dateOfJourney, string ticketStatus)
        {
            throw new NotImplementedException();
        }

        public List<Ticket> GetAllTicketsFromTrainsRouteIdAndDate(string trainName, DateTime dateOfJourney)
        {
            List<Ticket> ticketList = context.Ticket.Where(t => t.DateOfJourney == dateOfJourney && t.TrainName == trainName).ToList();
            return ticketList;
        }

        public List<Ticket> GetAllTicketsFromTrainsRouteIdDateAndStatus(string trainName, DateTime dateOfJourney, string ticketStatus)
        {
            List<Ticket> ticketList = context.Ticket.Where(t => t.DateOfJourney == dateOfJourney && t.TrainName == trainName && t.TicketStatus == ticketStatus).ToList();
            return ticketList;
        }

        public Ticket GetTicket(int ticketId)
        {
            return context.Ticket.FirstOrDefault(t => t.TicketId == ticketId);
        }

        public Ticket GetTicketFromPEmailAndTicketId(int ticketId, string pEmail)
        {
            return context.Ticket.FirstOrDefault(t => t.TicketId == ticketId && t.PEmail == pEmail);
        }

        public Ticket Update(Ticket ticketChanges)
        {
            var ticket = context.Ticket.Attach(ticketChanges);
            ticket.State = EntityState.Modified;
            context.SaveChanges();
            return ticketChanges;
        }

        public bool UpdateTicketStatus(int ticketId, string newStatus)
        {
            Ticket t = GetTicket(ticketId);
            t.TicketStatus = newStatus;
            t = Update(t);
            if (t == null)
            {
                return false;
            }
            return true;
        }
    }
}
