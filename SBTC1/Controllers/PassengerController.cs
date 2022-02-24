using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using SBTC1.Models;
using SBTC1.Models.Entilities;
using SBTC1.Models.IEntityRepositories;
using SBTC1.Utilities;
using SBTC1.ViewModels.Passenger;
using System.Globalization;

namespace SBTC1.Controllers
{
    [Authorize]
    public class PassengerController : Controller
    {
        private readonly IPassengerRepository passengerRepository;
        private readonly ITrainRepository trainRepository;
        private readonly ITrainRouteRepository trainRouteRepository;
        private readonly ISeatRepository seatRepository;
        private readonly ITicketRepository ticketRepository;
        private readonly ITransactionRepository transactionRepository;
        private readonly IPassengerInfoRepository passengerInfoRepository;

        public PassengerController(
            IPassengerRepository passengerRepository,
            ITrainRepository trainRepository,
            ITrainRouteRepository trainRouteRepository,
            ISeatRepository seatRepository,
            ITicketRepository ticketRepository,
            ITransactionRepository transactionRepository,
            IPassengerInfoRepository passengerInfoRepository)
        {
            this.passengerRepository = passengerRepository;
            this.trainRepository = trainRepository;
            this.trainRouteRepository = trainRouteRepository;
            this.seatRepository = seatRepository;
            this.ticketRepository = ticketRepository;
            this.passengerInfoRepository = passengerInfoRepository;
            this.transactionRepository = transactionRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Home()
        {
            HttpContext.Session.Clear();
            return View();
        }

        string FormatDateOfJourney(DateTime dateOfJourney)
        {
            string monthZeroPrefix = "", dayZeroPrefix = "";
            if (dateOfJourney.Month < 10)
                monthZeroPrefix = "0";
            if (dateOfJourney.Day < 10)
                dayZeroPrefix = "0";
            return dateOfJourney.Year + "-" + monthZeroPrefix + dateOfJourney.Month + "-" + dayZeroPrefix + dateOfJourney.Day;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ViewTrainSearchResults(PassengerHomeViewModel model, string trainType)
        {
            List<TrainRoute> trainList = trainRouteRepository.SearchTrains(model.Source, model.Destination);
            List<TrainViewModel> trainSearchResults = new List<TrainViewModel>();

            // Storing source, destination and date in ViewBag
            ViewBag.Source = model.Source;
            ViewBag.Destination = model.Destination;
            ViewBag.DateParam = FormatDateOfJourney(model.DateOfJourney);

            string date = model.DateOfJourney.ToLongDateString();
            HttpContext.Session.SetString("Source", model.Source);
            HttpContext.Session.SetString("Destination", model.Destination);
            ViewBag.DateOfJourney = date;

            // Traverse busList to fetch complete search results details
            foreach (TrainRoute trainRoute in trainList)
            {
                var trainObj = trainRepository.GetTrain(trainRoute.TrainId);
                trainSearchResults.Add(
                    new TrainViewModel
                    {
                        Train = trainObj,
                        TrainRoute = trainRoute,
                        Seat = seatRepository.GetSeatDetails(trainRoute.TrainRouteId, model.DateOfJourney),
                    }
                );
            }
            TrainSearchResultViewModel searchModel = new TrainSearchResultViewModel();
            if (trainType != null)
            {
                TrainType tmp = TrainType.Seater;
                if (trainType == "2")
                    tmp = TrainType.Sleeper;
                else if (trainType == "3")
                    tmp = TrainType.AC;
                if (trainType != "0")
                {
                    trainSearchResults = trainSearchResults.FindAll(b => b.Train.TrainType == tmp);
                    searchModel.TrainType = tmp;
                }
            }
            ViewBag.TotalSearchBusCount = trainSearchResults.Count();
            searchModel.TrainLists = trainSearchResults;
            searchModel.DateOfJourney = model.DateOfJourney;
            return View("ViewTrainSearchResults", searchModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ViewSeatLayout(int trainRouteId, string date)
        {
            TrainRoute trainRoute = trainRouteRepository.GetTrainRoute(trainRouteId);
            //DateTime dateOfJourney = DateTime.ParseExact(date, "dd-MM-yyyy hh.mm.ss tt", CultureInfo.InvariantCulture);
            DateTime dateOfJourney = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            HttpContext.Session.SetInt32("BusRouteId", trainRouteId);
            HttpContext.Session.SetString("DateOfJourney", date);
            TrainSeatLayoutViewModel model = new TrainSeatLayoutViewModel
            {
                TrainRoute = trainRoute,
                Train = trainRepository.GetTrain(trainRoute.TrainId),
                Seat = seatRepository.GetSeatDetails(trainRouteId, dateOfJourney),
                DateOfJourney = dateOfJourney,
                BookedSeats = seatRepository.GetBookedSeatList(trainRouteId, dateOfJourney).ToList(),
            };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult PassengerDetails(int seatCount, string selectedSeats, int ticketPrice)
        {
            HttpContext.Session.SetInt32("SeatCount", seatCount);
            HttpContext.Session.SetString("SelectedSeats", selectedSeats);
            HttpContext.Session.SetInt32("TicketPrice", ticketPrice);
            ViewBag.SeatCount = seatCount;
            List<int> selectedSeatList = selectedSeats.Split(",").Select(Int32.Parse).ToList();
            PassengerInfoViewModel model = new PassengerInfoViewModel
            {
                PInfo = new List<PassengerInfo>()
            };
            foreach (int seatNo in selectedSeatList)
            {
                model.PInfo.Add(new PassengerInfo { PSeatNo = seatNo });
            }
            return View("PassengerDetails", model);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult TicketPayment(PassengerInfoViewModel model)
        {
            ViewBag.DateOfJourney = HttpContext.Session.GetString("DateOfJourney");
            ViewBag.Source = HttpContext.Session.GetString("Source");
            ViewBag.Destination = HttpContext.Session.GetString("Destination");

            // Storing passengerinfoviewmodel in Session
            HttpContext.Session.SetString("PEmail", model.PEmail);
            HttpContext.Session.SetString("PPhoneNumber", model.PPhoneNumber);
            if (model.PInfo != null)
            {
                int id = 1;
                foreach (var passenger in model.PInfo)
                {
                    HttpContext.Session.SetString("PName" + id, passenger.PName);
                    HttpContext.Session.SetInt32("PGender" + id, (passenger.PGender == Gender.Male) ? 0 : 1);
                    HttpContext.Session.SetInt32("PSeatNo" + id, passenger.PSeatNo);
                    HttpContext.Session.SetInt32("PAge" + id, passenger.PAge);
                    id++;
                }
            }

            int ticketPrice = (int)HttpContext.Session.GetInt32("TicketPrice");
            TicketPaymentViewModel ticketPaymentViewModel = new TicketPaymentViewModel
            {
                TicketPrice = ticketPrice,
                TotalAmount = ticketPrice + AppConstant.RESERVATION_FEE + AppConstant.TOLL_FEE,
            };
            return View(ticketPaymentViewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ViewTicket(TicketPaymentViewModel model)
        {
            // Fetch all the data from session and model objects
            int trainRouteId = (int)HttpContext.Session.GetInt32("BusRouteId");
            string dateOfJourneyStr = HttpContext.Session.GetString("DateOfJourney");
            DateTime dateOfJourney = DateTime.ParseExact(dateOfJourneyStr, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            int seatCount = (int)HttpContext.Session.GetInt32("SeatCount");
            string selectedSeats = HttpContext.Session.GetString("SelectedSeats");

            // Creating Bus and BusRoute objects
            TrainRoute trainRouteObj = trainRouteRepository.GetTrainRoute(trainRouteId);
            Train trainObj = trainRepository.GetTrain(trainRouteObj.TrainId);

            // Manage Seat table
            Seat seatObj = seatRepository.GetSeatDetails(trainRouteId, dateOfJourney);
            // Check if seat for a given busRouteId and dateOfBooking exists or not
            if (seatObj == null)
            {
                // If seat doesn't exist then create a new seat row from the given dateOfBooking and busRouteId
                seatObj = new Seat
                {
                    TrainRouteId = trainRouteId,
                    AvailableSeats = (trainObj.TotalSeat - seatCount),
                    DateOfJourney = dateOfJourney,
                    SeatStructure = selectedSeats,
                };
                seatRepository.Add(seatObj);
            }
            else
            {
                seatObj.AvailableSeats -= seatCount;
                seatObj.SeatStructure = seatObj.SeatStructure + "," + selectedSeats;
                seatRepository.Update(seatObj);
            }
            // Copy seat structures of other routes for avoiding collision between routes

            // Manage ticket table
            Ticket ticket = new Ticket
            {
                PNRCode = UniqueIdGenerator.GenerateId(trainRouteObj.Source[0] + "" + trainRouteObj.Destination[0]),
                Source = trainRouteObj.Source,
                Destination = trainRouteObj.Destination,
                DateOfJourney = dateOfJourney,
                TrainName = trainObj.TrainName,
                TrainVehicleNumber = trainObj.TrainVehicleNumber,
                ArrivalTime = trainRouteObj.ArrivalTime,
                DepartureTime = trainRouteObj.DepartureTime,
                PassengerCount = seatCount,
                TicketStatus = AppConstant.BOOKED,
                TicketPrice = model.TotalAmount,
                PBankName = model.BankName,
                PBankSecretCode = model.BankSecretCode,
                PEmail = HttpContext.Session.GetString("PEmail"),
                PPhoneNumber = HttpContext.Session.GetString("PPhoneNumber"),
            };

            // Add ticket data to Ticket database table
            ticket = ticketRepository.Add(ticket);

            List<PassengerInfo> passengerList = new List<PassengerInfo>();
            for (int id = 1; id <= seatCount; id++)
            {
                Gender gender = (int)HttpContext.Session.GetInt32("PGender" + id) == 0 ? Gender.Male : Gender.Female;
                PassengerInfo passengerInfo = new PassengerInfo
                {
                    PName = HttpContext.Session.GetString("PName" + id),
                    PGender = gender,
                    PSeatNo = (int)HttpContext.Session.GetInt32("PSeatNo" + id),
                    PAge = (int)HttpContext.Session.GetInt32("PAge" + id),
                };
                // Add passengerinfo data to database
                passengerInfo = passengerInfoRepository.Add(passengerInfo);
                // Add transaction
                transactionRepository.AddTransaction(
                    new Transaction
                    {
                        PassengerInfoId = passengerInfo.PassengerInfoId,
                        TicketId = ticket.TicketId,
                    }
                );
                passengerList.Add(passengerInfo);
            }

            // Put the data in PassengerTicketViewModel for the view purpose
            PassengerTicketViewModel passengerTicketViewModel = new PassengerTicketViewModel
            {
                Ticket = ticket,
                Passengers = passengerList,
            };

            // Sending mail to passenger
            SendEmailToPassenger(passengerTicketViewModel);

            return View(passengerTicketViewModel);
        }

        public void SendEmailToPassenger(PassengerTicketViewModel model)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(AppConstant.ORGANIZATION_NAME, AppConstant.ORGANIZATION_EMAIL_ADDRESS));
            message.To.Add(new MailboxAddress(model.Passengers[0].PName, model.Ticket.PEmail));
            message.Subject = "Bus Ticket Booked from " + model.Ticket.Source
                                + " to " + model.Ticket.Destination
                                + " on " + model.Ticket.DateOfJourney.ToLongDateString();
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = WriteEmail(model),
            };

            // Send mail logic
            using var client = new MailKit.Net.Smtp.SmtpClient();
            client.Connect(AppConstant.SMTP_ADDRESS, AppConstant.SMTP_PORT, false);
            client.Authenticate(AppConstant.ORGANIZATION_EMAIL_ADDRESS, AppConstant.ORGANIZATION_PASSWORD);
            client.Send(message);
            client.Disconnect(true);
        }

        private string WriteEmail(PassengerTicketViewModel model)
        {
            string messageBody = "<table border=" + 1 + " cellpadding=" + 12 + " cellspacing=" + 0 + " width = " + 80 + "%>" +
                "<tbody>" +
                        "<tr>" +
                            "<td><b>Ticket Id:</b></td>" +
                            "<td>" + model.Ticket.TicketId + "</td>" +
                            "<td><b>PNR Code:</b></td>" +
                            "<td>" + model.Ticket.PNRCode + "</td>" +
                        "</tr>" +
                        "<tr>" +
                            "<td><b>Source:</b></td>" +
                            "<td>" + model.Ticket.Source + "</td>" +
                            "<td><b>Destination:</b></td>" +
                            "<td>" + model.Ticket.Destination + "</td>" +
                        "</tr>" +
                        "<tr>" +
                            "<td><b>Departure Time:</b></td>" +
                            "<td>" + model.Ticket.DepartureTime.ToShortTimeString() + "</td>" +
                            "<td><b>Arrival Time:</b></td>" +
                            "<td>" + model.Ticket.ArrivalTime.ToShortTimeString() + "</td>" +
                        "</tr>" +
                        "<tr>" +
                            "<td><b>Date of Journey:</b></td>" +
                            "<td>" + model.Ticket.DateOfJourney.ToLongDateString() + "</td>" +
                            "<td><b>Ticket Price:</b></td>" +
                            "<td> INR " + model.Ticket.TicketPrice + "</td>" +
                        "</tr>" +
                        "<tr>" +
                            "<td><b>Bus Name:</b></td>" +
                            "<td>" + model.Ticket.TrainName + "</td>" +
                            "<td><b>Bus Vehicle Number:</b></td>" +
                            "<td>" + model.Ticket.TrainVehicleNumber + "</td>" +
                        "</tr>" +
                        "<tr>" +
                            "<td><b>Passenger Count:</b></td>" +
                            "<td>" + model.Ticket.PassengerCount + "</td>" +
                            "<td></td><td></td>" +
                        "</tr>";
            foreach (var passenger in model.Passengers)
            {
                messageBody += "<tr>" +
                                    "<td><b>Passenger Name:</b></td>" +
                                    "<td>" + passenger.PName + "</td>" +
                                    "<td><b>Seat No.:</b></td>" +
                                    "<td>" + passenger.PSeatNo + "</td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td><b>Age:</b></td>" +
                                    "<td>" + passenger.PAge + "</td>" +
                                    "<td><b>Gender:</b></td>" +
                                    "<td>" + passenger.PGender + "</td>" +
                                "</tr>";
            }
            messageBody += "</tbody>";
            messageBody += "</table>";
            return messageBody;
        }

        [HttpGet]
        public IActionResult ManageBookings()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ManageBookings(ManageBookingsViewModel model)
        {
            // Fetch ticket details from ticket id
            Ticket ticket = ticketRepository.GetTicketFromPEmailAndTicketId(model.TicketId, model.PEmail);
            if (ticket == null)
            {
                return View("ManageBookings");
            }
            // Fetch passenger infos associated with ticket
            List<PassengerInfo> passengers = transactionRepository.GetPassengerInfoListFromTicketId(model.TicketId);
            // Create Passenger Ticket View Model Object
            PassengerTicketViewModel ptModel = new PassengerTicketViewModel
            {
                Ticket = ticket,
                Passengers = passengers,
            };
            return View("ManageTicket", ptModel);
        }

        [HttpPost]
        public IActionResult RateBus(int ticketId, int userRatings)
        {
            ticketRepository.AddUserRating(ticketId, userRatings);
            // Update total ratings of the bus
            Ticket t = ticketRepository.GetTicket(ticketId);
            TrainRoute trainRouteObj = trainRouteRepository.GetTrainRouteFromTrainName(t.TrainName, t.Source, t.Destination);
            Train train = trainRepository.GetTrain(trainRouteObj.TrainId);
            double mean = (train.TotalRateCounts * Double.Parse(train.Ratings) + userRatings) / (train.TotalRateCounts + 1);
            train.TotalRateCounts += 1;
            train.Ratings = mean.ToString();
            trainRepository.Update(train);
            // Update ticket status
            ticketRepository.UpdateTicketStatus(ticketId, AppConstant.RATED);
            return View("ManageBookings");
        }

        [HttpPost]
        public IActionResult CancelTicket(int ticketId)
        {
            Ticket t = ticketRepository.GetTicket(ticketId);
            TrainRoute trainRouteObj = trainRouteRepository.GetTrainRouteFromTrainName(t.TrainName, t.Source, t.Destination);
            Seat seat = seatRepository.GetSeatDetails(trainRouteObj.TrainRouteId, t.DateOfJourney);
            // Remove seat numbers from bookedList in Seat obj
            List<int> bookedList = seatRepository.GetBookedSeatList(trainRouteObj.TrainRouteId, t.DateOfJourney).ToList();
            List<PassengerInfo> passengers = transactionRepository.GetPassengerInfoListFromTicketId(ticketId);
            foreach (var p in passengers)
            {
                bookedList.Remove(p.PSeatNo);
                // Remove data from passengerinfo and transaction data
                transactionRepository.DeleteTransaction(ticketId, p.PassengerInfoId);
                passengerInfoRepository.Delete(p.PassengerInfoId);
            }
            seat.SeatStructure = string.Join(",", bookedList);
            seat.AvailableSeats += t.PassengerCount;
            seatRepository.Update(seat);
            // Update ticket status
            ticketRepository.UpdateTicketStatus(ticketId, AppConstant.CANCELLED);
            return View("ManageBookings");
        }
    }
}
