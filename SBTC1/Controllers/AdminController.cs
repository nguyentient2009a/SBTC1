using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SBTC1.Models;
using SBTC1.Models.Entilities;
using SBTC1.Models.IEntityRepositories;
using SBTC1.Utilities;
using SBTC1.ViewModels.Admin;

namespace SBTC1.Controllers
{
    [Authorize(Roles = AppConstant.ADMIN)]
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IPassengerRepository passengerRepository;
        private readonly ITrainOperatorRepository trainOperatorRepository;
        private readonly IAdminRepository adminRepository;
        private readonly ITrainRepository trainRepository;
        private readonly ITrainRouteRepository trainRouteRepository;
        private readonly IApplicationUserRepository applicationUserRepository;
        private readonly ISeatRepository seatRepository;
        private readonly ITicketRepository ticketRepository;

        public AdminController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IPassengerRepository passengerRepository,
            ITrainOperatorRepository trainOperatorRepository,
            IAdminRepository adminRepository,
            ITrainRepository trainRepository,
            ITrainRouteRepository trainRouteRepository,
            IApplicationUserRepository applicationUserRepository,
            ISeatRepository seatRepository,
            ITicketRepository ticketRepository)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.passengerRepository = passengerRepository;
            this.trainOperatorRepository = trainOperatorRepository;
            this.adminRepository = adminRepository;
            this.trainRepository = trainRepository;
            this.trainRouteRepository = trainRouteRepository;
            this.applicationUserRepository = applicationUserRepository;
            this.seatRepository = seatRepository;
            this.ticketRepository = ticketRepository;
        }

        //Manage Bus methods
        [HttpGet]
        public IActionResult ViewTrainList()
        {
            var trainList = trainRepository.GetAllTrains();
            ViewBag.TotalNoOfBus = trainList.Count();
            List<TrainListViewModel> viewModel = new List<TrainListViewModel>();
            foreach (Train train in trainList)
            {
                TrainOperator trainOperator = trainOperatorRepository.GetTrainOperator(train.TrainOperatorId);
                var model = new TrainListViewModel
                {
                    Train = train,
                    TrainOperatorName = (trainOperator == null) ? "Vacancy" : trainOperator.DisplayName
                };
                viewModel.Add(model);
            }
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult AddNewTrain()
        {
            var freeTrainOperatorsList = trainRepository.GetAvailableTrainOperators();
            ViewBag.AvailTrainOperatorUserList = new SelectList(freeTrainOperatorsList, "Id", "DisplayName");
            return View();
        }

        [HttpPost]
        public IActionResult AddNewTrain(Train model)
        {
            if (ModelState.IsValid)
            {
                model.Ratings = "3.7";
                trainRepository.Add(model);
                return RedirectToAction("ViewBusList", AppConstant.ADMIN);
            }
            return View();
        }

        [HttpGet]
        public IActionResult UpdateTrain(string trainId)
        {
            var model = trainRepository.GetTrain(Convert.ToInt32(trainId));
            var freeTrainOperatorsList = trainRepository.GetAvailableTrainOperators();
            var trainOperatorObj = trainOperatorRepository.GetTrainOperator(model.TrainOperatorId);
            if (trainOperatorObj != null)
            {
                ViewBag.TrainOperatorId = trainOperatorObj.Id;
                ViewBag.TrainOperatorName = trainOperatorObj.DisplayName;
            }
            else
            {
                ViewBag.TrainOperatorId = "";
                ViewBag.TrainOperatorName = "Please Select";
            }
            ViewBag.AvailTrainOperatorUserList = new SelectList(freeTrainOperatorsList, "Id", "DisplayName");
            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateTrain(Train model)
        {
            if (ModelState.IsValid)
            {
                trainRepository.Update(model);
                return RedirectToAction("ViewTrainList", AppConstant.ADMIN);
            }
            return View();
        }

        [HttpPost]
        public IActionResult RemoveTrain(int trainId)
        {
            Train train = trainRepository.GetTrain(trainId);
            if (train != null)
            {
              
                trainRepository.Delete(train.TrainId);
            }
            return RedirectToAction("ViewBusList", AppConstant.ADMIN);
        }

        [HttpGet]
        public IActionResult ViewTrainRouteList(int trainId)
        {
            Train train = trainRepository.GetTrain(trainId);
          TrainOperator trainOperator = trainOperatorRepository.GetTrainOperator(train.TrainOperatorId);
            List<TrainRoute> trainRouteList = trainRouteRepository.GetAllTrainRoutesByTrainId(trainId);
            ViewBag.TotalNoOfTrainRoutes = trainRouteList.Count();
            var model = new TrainRouteListViewModel()
            {
                Train = train,
                TrainRouteList = trainRouteList,
                TrainOperatorName = (trainOperator == null) ? "Vacancy" : trainOperator.DisplayName
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult AddNewTrainRoute(int trainId)
        {
            var train = trainRepository.GetTrain(trainId);
            ViewBag.TrainName = train.TrainName;
            ViewBag.RouteSequence = train.RouteSequence;
            return View();
        }

        [HttpPost]
        public IActionResult AddNewTrainRoute(TrainRoute model, int trainId)
        {
            if (ModelState.IsValid && trainId != 0)
            {
                model.TrainId= trainId;
                trainRouteRepository.AddTrainRoute(model);
                return RedirectToAction("ViewTrainRouteList", AppConstant.ADMIN, new { trainId = model.TrainId });
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult UpdateTrainRoute(int trainRouteId, int trainId)
        {
            var train = trainRepository.GetTrain(trainId);
            ViewBag.TrainName = train.TrainName;
            ViewBag.RouteSequence = train.RouteSequence;
            var model = trainRouteRepository.GetTrainRoute(trainRouteId);
            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateTrainRoute(TrainRoute model)
        {
            if (ModelState.IsValid)
            {
                trainRouteRepository.UpdateTrainRoute(model);
                return RedirectToAction("ViewTrainRouteList", AppConstant.ADMIN, new { trainId = model.TrainId });
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult RemoveTrainRoute(int trainRouteId)
        {
            //Remove from BusRoute
            TrainRoute trainRoute = trainRouteRepository.GetTrainRoute(trainRouteId);
            if (trainRoute != null)
            {
                trainRouteRepository.DeleteTrainRoute(trainRoute.TrainRouteId);
                return RedirectToAction("ViewTrainRouteList", AppConstant.ADMIN, new { trainId = trainRoute.TrainId });
            }
            return View();
        }

        //Manage Users methods
        [HttpGet]
        public IActionResult ViewUserList(string userType)
        {
            if (userType == null)
            {
                userType = AppConstant.TRAIN_OPERATOR;
            }
            var model = new UserListViewModel
            {
                UserType = userType,
                ApplicationUserList = applicationUserRepository.GetApplicationUserListOfType(userType),
            };
            ViewBag.TotalUsers = model.ApplicationUserList.Count;
            return View(model);
        }

        [HttpGet]
        public IActionResult AddNewUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewUser(AddNewUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Store user information in ApplicationUser table
                ApplicationUser applicationUser = FillUserDetails(model);

                //Store user data in AspNetUsers database table
                var result = await userManager.CreateAsync(applicationUser, model.Password);

                //If user is successfully created, then
                if (result.Succeeded)
                {
                    if (applicationUser.UserType == AppConstant.TRAIN_OPERATOR)
                    {
                        await userManager.AddToRoleAsync(applicationUser, AppConstant.TRAIN_OPERATOR);
                    }
                    else if (applicationUser.UserType == AppConstant.ADMIN)
                    {
                        await userManager.AddToRoleAsync(applicationUser, AppConstant.ADMIN);
                    }
                    return RedirectToAction("ViewUserList", AppConstant.ADMIN);
                }
                // If there are any errors, add them to the ModelState object
                // which will be displayed by the validation summary tag helper
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        public ApplicationUser FillUserDetails(AddNewUserViewModel model)
        {
            if (model.UserType == AppConstant.TRAIN_OPERATOR)
            {
                TrainOperator busOperator = new TrainOperator
                {
                    UserType = AppConstant.TRAIN_OPERATOR,
                    DisplayName = model.FirstName + " " + model.LastName,
                    Gender = model.Gender,
                    Email = model.Email,
                    UserName = model.Email,
                    DateOfBirth = model.DateOfBirth,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    Salary = model.Salary,
                    DateOfJoining = model.DateOfJoining,
                };
                return busOperator;
            }
            else if (model.UserType == AppConstant.ADMIN)
            {
                Admin adminUser = new Admin
                {
                    UserType = AppConstant.ADMIN,
                    DisplayName = model.FirstName + " " + model.LastName,
                    Gender = model.Gender,
                    Email = model.Email,
                    UserName = model.Email,
                    DateOfBirth = model.DateOfBirth,
                    PhoneNumber = model.PhoneNumber,
                };
                return adminUser;
            }
            return null;
        }

        [HttpGet]
        public IActionResult UserDetails(string Id)
        {
            ApplicationUser model = applicationUserRepository.GetApplicationUser(Id);
            var viewModel = new UserDetailsViewModel
            {
                User = model,
                Email = model.Email,
                Age = DateUtility.DateDifferenceFromToday(model.DateOfBirth),
            };
            if (model.UserType == AppConstant.TRAIN_OPERATOR)
            {
                var trainOperatorModel = trainOperatorRepository.GetTrainOperator(model.Id);
                viewModel.Address = trainOperatorModel.Address;
                viewModel.Salary = trainOperatorModel.Salary;
                viewModel.Experience = DateUtility.DateDifferenceFromToday(trainOperatorModel.DateOfJoining);
            }
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult UpdateUser(string Id)
        {
            var model = applicationUserRepository.GetApplicationUser(Id);
            var name = model.DisplayName.Split();
            string lastName = "";
            if (name.Length > 1)
            {
                lastName = name[1];
            }
            var viewModel = new UpdateUserViewModel
            {
                FirstName = name[0],
                LastName = lastName,
                Gender = model.Gender,
                PhoneNumber = model.PhoneNumber,
                DateOfBirth = model.DateOfBirth,
                UserType = model.UserType,
                UpdateUserId = Id,
            };
            if (model.UserType == AppConstant.TRAIN_OPERATOR)
            {
                var trainOperatorModel = trainOperatorRepository.GetTrainOperator(Id);
                viewModel.Address = trainOperatorModel.Address;
                viewModel.Salary = trainOperatorModel.Salary;
                viewModel.DateOfJoining = trainOperatorModel.DateOfJoining;
            }
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(UpdateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                string userId = model.UpdateUserId;
                if (model.UserType == AppConstant.PASSENGER || model.UserType == AppConstant.ADMIN)
                {
                    ApplicationUser user = applicationUserRepository.GetApplicationUser(userId);
                    if (user != null)
                    {
                        user.DisplayName = model.FirstName + " " + model.LastName;
                        user.Gender = model.Gender;
                        user.DateOfBirth = model.DateOfBirth;
                        user.PhoneNumber = model.PhoneNumber;
                        //applicationUserRepository.Update(user);
                        await userManager.UpdateAsync(user);
                    }
                }
                else if (model.UserType == AppConstant.TRAIN_OPERATOR)
                {
                    TrainOperator trainOperator = trainOperatorRepository.GetTrainOperator(userId);
                    if (trainOperator != null)
                    {
                        trainOperator.DisplayName = model.FirstName + " " + model.LastName;
                        trainOperator.Gender = model.Gender;
                        trainOperator.DateOfBirth = model.DateOfBirth;
                        trainOperator.PhoneNumber = model.PhoneNumber;
                        trainOperator.Address = model.Address;
                        trainOperator.Salary = model.Salary;
                        trainOperator.DateOfJoining = model.DateOfJoining;
                        // Both the methods are working fine
                        //busOperatorRepository.Update(busOperator);
                        await userManager.UpdateAsync(trainOperator);
                    }
                }
                return RedirectToAction("ViewUserList", AppConstant.ADMIN);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUser(string Id)
        {
            ApplicationUser user = await userManager.FindByIdAsync(Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"ApplicationUser with userId = {Id} cannot be found";
                return View("RoleNotFound");
            }
            //Removing from AspNetUserRoles
            await userManager.RemoveFromRoleAsync(user, user.UserType);
            //Removing from AspNetUsers
            await userManager.DeleteAsync(user);
            return RedirectToAction("ViewUserList", AppConstant.ADMIN);
        }

        // Manage Role Methods for MASTER user
        [HttpGet]
        [Authorize(Roles = AppConstant.MASTER)]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = AppConstant.MASTER)]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };
                IdentityResult result = await roleManager.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", AppConstant.ADMIN);
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = AppConstant.MASTER)]
        public async Task<IActionResult> DeleteRole(string Id)
        {
            var role = await roleManager.FindByIdAsync(Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"ApplicationUser with userId = {Id} cannot be found";
                return View("RoleNotFound");
            }
            else
            {
                var result = await roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", AppConstant.ADMIN);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("ListRoles");
            }
        }

        [HttpGet]
        [Authorize(Roles = AppConstant.MASTER)]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        [Authorize(Roles = AppConstant.MASTER)]
        public async Task<IActionResult> EditRole(string Id)
        {
            var role = await roleManager.FindByIdAsync(Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {Id} cannot be found";
                return View("RoleNotFound");
            }

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.DisplayName);
                }
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = AppConstant.MASTER)]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                return View("RoleNotFound");
            }
            else
            {
                role.Name = model.RoleName;
                var result = await roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
        }

        [HttpGet]
        [Authorize(Roles = AppConstant.MASTER)]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;
            var role = await roleManager.FindByIdAsync(roleId);
            ViewBag.roleName = role.Name;
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("RoleNotFound");
            }

            var model = new List<UserRoleViewModel>();
            foreach (var user in userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    DisplayName = user.DisplayName
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }
                model.Add(userRoleViewModel);
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = AppConstant.MASTER)]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("RoleNotFound");
            }
            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);
                IdentityResult result;
                //If user is selected and is not a member of AspNetRoles table
                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!(model[i].IsSelected) && (await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                    {
                        continue;
                    }
                    else
                    {
                        return RedirectToAction("EditRole", new { Id = roleId });
                    }
                }
            }
            return RedirectToAction("EditRole", new { Id = roleId });
        }
    }
}
