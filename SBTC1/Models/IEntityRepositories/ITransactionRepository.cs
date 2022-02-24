using Microsoft.AspNetCore.Mvc;
using SBTC1.Models.Entilities;

namespace SBTC1.Models.IEntityRepositories
{
    public interface ITransactionRepository
    {
        Transaction AddTransaction(Transaction passengerTicket);
        Transaction UpdateTransaction(Transaction passengerTicketChanges);
        bool DeleteTransactionFromTicketId(int ticketId);
        bool DeleteTransaction(int ticketId, int passengerInfoId);
        List<Transaction> GetTransactionDetails(int ticketId);
        List<PassengerInfo> GetPassengerInfoListFromTicketId(int ticketId);
    }
}
