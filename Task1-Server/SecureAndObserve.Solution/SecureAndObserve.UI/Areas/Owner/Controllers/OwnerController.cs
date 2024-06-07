using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using SecureAndObserve.Core.Domain.IdentityEntities;
using SecureAndObserve.Core.DTO;
using SecureAndObserve.Core.Enums;
using SecureAndObserve.Core.ServiceContracts;
using System.Globalization;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using SecureAndObserve.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using SecureAndObserve.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using SecureAndObserve.UI.Controllers;

namespace SecureAndObserve.UI.Areas.Owner.Controllers
{
    [Area("Owner")]
    [Route("[controller]")]
    public class OwnerController : Controller
    {
        private readonly ITerritoriesService _territoriesService;
        private readonly IOrdersService _ordersService;
        private readonly IOrderGuardsService _orderGuardsService;
        private readonly IGuardExstensionsService _guardExstensionsService;
        private readonly IRanksService _ranksService;
        private readonly ILogger<OwnerController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public OwnerController(ITerritoriesService territoriesService, ILogger<OwnerController> logger, ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, IOrdersService ordersService, IOrderGuardsService orderGuardsService, IGuardExstensionsService guardExstensionsService, IRanksService ranksService)
        {
            _territoriesService = territoriesService;
            _logger = logger;
            _context = context;
            _signInManager = signInManager;
            _ordersService = ordersService;
            _orderGuardsService = orderGuardsService;
            _guardExstensionsService = guardExstensionsService;
            _ranksService = ranksService;
        }

        [HttpGet]
        [Route("Index")]
        public async Task<IActionResult> Index(string searchBy, string? searchString, string sortBy = nameof(TerritoryResponse.Name), SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            _logger.LogInformation("Index action method of PersonsController");
            _logger.LogDebug($"searchBy: {searchBy}, searchString: {searchString}, sortOrder: {sortOrder}");
            //Search
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                { nameof(TerritoryResponse.Name), "Name" },
                { nameof(TerritoryResponse.Square), "Square" },
                { nameof(TerritoryResponse.Description), "Description" },
                { nameof(TerritoryResponse.Type), "Type" },
            };
            List<TerritoryResponse> territories = await _territoriesService.GetFilteredTerritories(searchBy, searchString);

            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;

            //Sort
            List<TerritoryResponse> sortedTerritories = await _territoriesService.GetSortedTerritories(territories, sortBy, sortOrder);

            ViewBag.CurrentSortBy = sortBy.ToString();
            ViewBag.CurrentSortOrder = sortOrder.ToString();

            //Id of authenticated user
            var userClaims = User.Identity as ClaimsIdentity;
            var userId = userClaims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            List<TerritoryResponse> finalTerritories = new List<TerritoryResponse>();

            foreach (TerritoryResponse territory in sortedTerritories)
            {
                if(Convert.ToString(territory.OwnerId) == userId)
                    finalTerritories.Add(territory);
            }

            List<OrderResponse> orders = await _ordersService.GetAllOrders();
            List<OrderResponse> finalOrder = new List<OrderResponse>();
            foreach (OrderResponse order in orders)
            {
                if (Convert.ToString(order.OwnerId) == userId)
                    finalOrder.Add(order);
            }
            ViewBag.Order = finalOrder;

            return View(finalTerritories);
        }

        [HttpGet]
        [Route("CreateTerritory")]
        public async Task<IActionResult> CreateTerritory()
        {
            return View();
        }

        [HttpPost]
        [Route("CreateTerritory")]
        public async Task<IActionResult> CreateTerritory(TerritoryAddRequest territoryAddRequest)
        {
            if (!ModelState.IsValid)
            {

                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View(territoryAddRequest);
            }

            var userClaims = User.Identity as ClaimsIdentity;
            var userId = userClaims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            Guid guid = new Guid(userId);

            territoryAddRequest.OwnerId = guid;

            //call the service method
            TerritoryResponse personResponse = await _territoriesService.AddTerritory(territoryAddRequest);

            //navigate to Index() action method (its makes another get request to "owner/index")
            return RedirectToAction("Index", "Owner");
        }

        [HttpGet]
        [Route("EditTerritory")]
        public async Task<IActionResult> EditTerritory(Guid territoryID)
        {
            TerritoryResponse? territoryResponse = await _territoriesService.GetTerritoryByTerritoryId(territoryID);
            if (territoryResponse == null)
            {
                return RedirectToAction("Index");
            }

            return View(territoryResponse);
        }

        [HttpPost]
        [Route("EditTerritory")]
        public async Task<IActionResult> EditTerritory(TerritoryResponse territoryResponse)
        {
            if (territoryResponse == null)
            {
                return RedirectToAction("Index");
            }

            var territoryInDb = await _context.Territories.FirstOrDefaultAsync(x => x.Id == territoryResponse.Id);
            if (ModelState.IsValid)
            {
                if (territoryInDb != null)
                {
                    territoryInDb.Name = territoryResponse.Name;
                    territoryInDb.Square = territoryResponse.Square;
                    territoryInDb.Description = territoryResponse.Description;
                    territoryInDb.Type = territoryResponse.Type;
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Owner");
                }
            }
            else
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View(territoryResponse);
            }

            return View(territoryResponse);
        }
        [HttpGet]
        [Route("CreateOrder")]
        public async Task<IActionResult> CreateOrder()
        {
            return View();
        }
        [HttpPost]
        [Route("CreateOrder")]
        public async Task<IActionResult> CreateOrder(OrderAddRequest orderAddRequest)
        {
            if (!ModelState.IsValid)
            {

                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View(orderAddRequest);
            }

            var userClaims = User.Identity as ClaimsIdentity;
            var userId = userClaims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            Guid guid = new Guid(userId);

            orderAddRequest.OwnerId = guid;

            //call the service method
            OrderResponse orderResponse = await _ordersService.AddOrder(orderAddRequest);

            //navigate to Index() action method (its makes another get request to "owner/index")
            return RedirectToAction("Index", "Owner");
        }

        [HttpGet]
        [Route("DeleteOrder/{orderID}")]
        public async Task<IActionResult> DeleteOrder(Guid orderID)
        {
            OrderResponse? orderResponse = await _ordersService.GetOrderByOrderId(orderID);
            if (orderResponse == null)
                return RedirectToAction("Index");
            return View(orderResponse);
        }
        [HttpPost]
        [Route("DeleteOrder/{orderID}")]
        public async Task<IActionResult> DeleteOrder(OrderResponse orderResponseToDelete)
        {
            OrderResponse? orderResponse = await _ordersService.GetOrderByOrderId(orderResponseToDelete.Id);
            if (orderResponse == null)
                return RedirectToAction("Index");

            List<OrderGuards> orderGuardsList = await _context.OrderGuards.Where(x => x.OrderId == orderResponseToDelete.Id).ToListAsync();

            foreach(OrderGuards orderGuards in orderGuardsList)
            {
                await _orderGuardsService.DeleteOrderGuards(orderGuards.Id);
            }

            await _ordersService.DeleteOrder(orderResponse.Id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("OrderGuards/{orderID}")]
        public async Task<IActionResult> OrderGuards(Guid orderID)
        {
            List<OrderGuards> orderGuardsList = await _context.OrderGuards.Where(x => x.OrderId == orderID).ToListAsync();
            List<GuardExstensionsResponse> guardExstensionsList = await _guardExstensionsService.GetAllGuardExstensions();
            List<GuardExstensionsResponse> finalGuardExstensionsList = new List<GuardExstensionsResponse>();
            foreach(OrderGuards orderGuards in orderGuardsList)
            {
                foreach(GuardExstensionsResponse guardExstensions in guardExstensionsList)
                {
                    if (guardExstensions.Id == orderGuards.GuardExstensionsId)
                        finalGuardExstensionsList.Add(guardExstensions);
                }
            }
            List<RankResponse> ranks = await _ranksService.GetAllRanks();
            List<RankResponse> finalRanks = new List<RankResponse>();
            foreach(GuardExstensionsResponse guardExstentions in finalGuardExstensionsList)
            {
                foreach(RankResponse rank in ranks)
                {
                    if(guardExstentions.RankId == rank.Id)
                        finalRanks.Add(rank);
                }
            }
            OrderResponse? orderResponse = await _ordersService.GetOrderByOrderId(orderID);
            double orientedPayPerHour = 0;
            foreach (RankResponse rank in finalRanks)
            {
                double payPerHour = rank.PayPerHour;
                if (orderResponse.TypeOfService == ServiceTypeOptions.PlacementOnPosts.ToString())
                    payPerHour = payPerHour * 1.1;
                if (orderResponse.TypeOfService == ServiceTypeOptions.Patrol.ToString())
                    payPerHour = payPerHour * 1.3;
                if (orderResponse.TypeOfService == SecurityLevelOptions.Medium.ToString())
                    payPerHour = payPerHour * 1.3;
                if (orderResponse.TypeOfService == SecurityLevelOptions.High.ToString())
                    payPerHour = payPerHour * 1.5;

                orientedPayPerHour += payPerHour;
            }


            ViewBag.AmountOfGuards = finalGuardExstensionsList.Count;
            ViewBag.OrientedPayPerHour = Math.Round(orientedPayPerHour,2).ToString();


            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Login), "Home");
        }
    }
}
