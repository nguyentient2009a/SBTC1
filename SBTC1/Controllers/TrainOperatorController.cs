using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SBTC1.Models;
using SBTC1.Models.Entilities;
using SBTC1.Models.IEntityRepositories;
using SBTC1.ViewModels.TrainOperator;

namespace SBTC1.Controllers
{
    [Authorize(Roles = AppConstant.TRAIN_OPERATOR)]
    public class TrainOperatorController : Controller
    {
        private readonly IApplicationUserRepository applicationUserRepository;
        private readonly ITrainOperatorRepository trainOperatorRepository;
        private readonly ITrainRepository trainRepository;
        private readonly ITrainRouteRepository trainRouteRepository;
        private readonly ISeatRepository seatRepository;
        private readonly ITicketRepository ticketRepository;
        private readonly ITransactionRepository transactionRepository;
        private readonly IPassengerInfoRepository passengerInfoRepository;

        public TrainOperatorController(
            IApplicationUserRepository applicationUserRepository,
            ITrainOperatorRepository trainOperatorRepository,
            ITrainRepository trainRepository,
            ITrainRouteRepository trainRouteRepository,
            ISeatRepository seatRepository,
            ITicketRepository ticketRepository,
            ITransactionRepository transactionRepository,
            IPassengerInfoRepository passengerInfoRepository)
        {
            this.applicationUserRepository = applicationUserRepository;
            this.trainOperatorRepository = trainOperatorRepository;
            this.trainRepository = trainRepository;
            this.trainRouteRepository = trainRouteRepository;
            this.seatRepository = seatRepository;
            this.ticketRepository = ticketRepository;
            this.transactionRepository = transactionRepository;
            this.passengerInfoRepository = passengerInfoRepository;
        }

        [HttpGet]
        public IActionResult ViewBookingList()
        {
            List<TrainBookingListViewModel> model = new List<TrainBookingListViewModel>();
            ApplicationUser appUser = applicationUserRepository.GetApplicationUserFromEmail(HttpContext.User.Identity.Name);
            if (appUser != null)
            {
                Train train = trainRepository.GetTrainFromTrainOperatorId(appUser.Id);
                // For bus details
                ViewBag.TrainName = train.TrainName;
                ViewBag.RouteSequence = train.RouteSequence;
                ViewBag.TrainTime = train.TrainTime;

                List<Ticket> ticketList = ticketRepository.GetAllTicketsFromTrainRouteIdDateAndStatus(train.TrainName, DateTime.Today, AppConstant.BOOKED);
                foreach (var t in ticketList)
                {
                    var passengerList = transactionRepository.GetPassengerInfoListFromTicketId(t.TicketId);
                    foreach (var p in passengerList)
                    {
                        var obj = new TrainBookingListViewModel
                        {
                            Passenger = p,
                            Source = t.Source,
                            Destination = t.Destination,
                        };
                        model.Add(obj);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ViewPassengerCheckList()
        {
            List<PassengerCheckListViewModel> model = new List<PassengerCheckListViewModel>();
            ApplicationUser appUser = applicationUserRepository.GetApplicationUserFromEmail(HttpContext.User.Identity.Name);
            if (appUser != null)
            {
                Train train = trainRepository.GetTrainFromTrainOperatorId(appUser.Id);
                List<Ticket> ticketList = ticketRepository.GetAllTicketsFromTrainRouteIdAndDate(train.TrainName, DateTime.Today);
                foreach (var t in ticketList)
                {
                    var passengerList = transactionRepository.GetPassengerInfoListFromTicketId(t.TicketId);
                    bool flag = true;
                    if (t.TicketStatus == AppConstant.BOOKED)
                    {
                        flag = false;
                    }
                    else if (t.TicketStatus == AppConstant.CHECKED)
                    {
                        flag = true;
                    }
                    else
                    {
                        continue;
                    }
                    foreach (var p in passengerList)
                    {
                        var obj = new PassengerCheckListViewModel
                        {
                            Passenger = p,
                            Source = t.Source,
                            Destination = t.Destination,
                            TicketId = t.TicketId,
                            IsChecked = flag,
                        };
                        model.Add(obj);
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult ViewPassengerCheckList(List<PassengerCheckListViewModel> model)
        {
            foreach (var p in model)
            {
                if (p.IsChecked)
                {
                    ticketRepository.UpdateTicketStatus(p.TicketId, AppConstant.CHECKED);
                }
                else
                {
                    ticketRepository.UpdateTicketStatus(p.TicketId, AppConstant.BOOKED);
                }
            }
            return RedirectToAction("ViewBookingList");
        }
    }
}
