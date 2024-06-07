using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using SecureAndObserve.Core.Domain.Entities;
using SecureAndObserve.Core.Domain.IdentityEntities;
using SecureAndObserve.Core.DTO;
using SecureAndObserve.Core.Enums;
using SecureAndObserve.Core.ServiceContracts;
using SecureAndObserve.Infrastructure.DbContext;
using SecureAndObserve.UI.Areas.Owner.Controllers;
using SecureAndObserve.UI.Controllers;
using System.Security.Claims;

namespace SecureAndObserve.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("[controller]")]
    public class AdminController : Controller
    {
        private readonly ITerritoriesService _territoriesService;
        private readonly IOrdersService _ordersService;
        private readonly IOrderGuardsService _orderGuardsService;
        private readonly IGuardExstensionsService _guardExstensionsService;
        private readonly IEquipmentService _equipmentService;
        private readonly IRanksService _ranksService;
        private readonly ILogger<OwnerController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AdminController(ITerritoriesService territoriesService, ILogger<OwnerController> logger, ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, IOrdersService ordersService, IOrderGuardsService orderGuardsService, IGuardExstensionsService guardExstensionsService, IRanksService ranksService, IEquipmentService equipmentService)
        {
            _territoriesService = territoriesService;
            _logger = logger;
            _context = context;
            _signInManager = signInManager;
            _ordersService = ordersService;
            _orderGuardsService = orderGuardsService;
            _guardExstensionsService = guardExstensionsService;
            _ranksService = ranksService;
            _equipmentService = equipmentService;
        }

        [HttpGet]
        [Route("Territories")]
        public async Task<IActionResult> Territories(string searchBy, string? searchString, string sortBy = nameof(TerritoryResponse.Name), SortOrderOptions sortOrder = SortOrderOptions.ASC)
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
            return View(sortedTerritories);
        }
        [HttpGet]
        [Route("Territories/EditTerritory/{territoryID}")]
        public async Task<IActionResult> EditTerritory(Guid territoryID)
        {
            TerritoryResponse? territoryResponse = await _territoriesService.GetTerritoryByTerritoryId(territoryID);
            if (territoryResponse == null)
            {
                return RedirectToAction("Territories");
            }

            return View(territoryResponse);
        }

        [HttpPost]
        [Route("Territories/EditTerritory/{territoryID}")]
        public async Task<IActionResult> EditTerritory(TerritoryResponse territoryResponse)
        {
            if (territoryResponse == null)
            {
                return RedirectToAction("Territories");
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
                    return RedirectToAction("Territories", "Admin");
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
        [Route("Territories/DeleteTerritory/{territoryID}")]
        public async Task<IActionResult> DeleteTerritory(Guid territoryID)
        {
            TerritoryResponse? territoryResponse = await _territoriesService.GetTerritoryByTerritoryId(territoryID);
            if (territoryResponse == null)
            {
                return RedirectToAction("Territories");
            }

            return View(territoryResponse);
        }

        [HttpPost]
        [Route("Territories/DeleteTerritory/{territoryID}")]
        public async Task<IActionResult> DeleteTerritory(TerritoryResponse territory)
        {
            if (territory == null)
            {
                return RedirectToAction("Territories");
            }

            var territoryInDb = await _context.Territories.FirstOrDefaultAsync(x => x.Id == territory.Id);
            if (ModelState.IsValid)
            {
                if (territoryInDb != null)
                {
                    Order? order = await _context.Orders.FirstOrDefaultAsync(x => x.OwnerId == territoryInDb.OwnerId);
                    if (order != null)
                    {
                        List<OrderGuards> orderGuards = await _context.OrderGuards.Where(x => x.OrderId == order.Id).ToListAsync();
                        List<GuardReport> guardReports = await _context.GuardReports.Where(x => x.OrderId == order.Id).ToListAsync();
                        foreach(GuardReport guardReport in guardReports)
                        {
                            _context.GuardReports.Remove(guardReport);
                        }
                        foreach (OrderGuards orderGuard in orderGuards)
                        {
                            _context.OrderGuards.Remove(orderGuard);
                        }
                        _context.Orders.Remove(order);
                        await _context.SaveChangesAsync();
                    }
                    _context.Territories.Remove(territoryInDb);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Territories", "Admin");
                }
            }
            else
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View(territory);
            }

            return View(territory);
        }

        [HttpGet]
        [Route("Orders")]
        public async Task<IActionResult> Orders(string searchBy, string? searchString, string sortBy = nameof(TerritoryResponse.Name), SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            _logger.LogInformation("Index action method of PersonsController");
            _logger.LogDebug($"searchBy: {searchBy}, searchString: {searchString}, sortOrder: {sortOrder}");
            //Search
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                { nameof(OrderResponse.OwnerId), "OwnerId" },
                { nameof(OrderResponse.TypeOfService), "TypeOfService" },
                { nameof(OrderResponse.SecurityLevel), "SecurityLevel" },
            };
            List<OrderResponse> orders = await _ordersService.GetAllOrders();

            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;

            //Sort
            List<OrderResponse> sortedOrders = orders;

            ViewBag.CurrentSortBy = sortBy.ToString();
            ViewBag.CurrentSortOrder = sortOrder.ToString();
            return View(sortedOrders);
        }

        [HttpGet]
        [Route("Orders/EditOrder/{orderID}")]
        public async Task<IActionResult> EditOrder(Guid orderID)
        {
            OrderResponse? orderResponse = await _ordersService.GetOrderByOrderId(orderID);
            if (orderResponse == null)
            {
                return RedirectToAction("Orders");
            }

            return View(orderResponse);
        }

        [HttpPost]
        [Route("Orders/EditOrder/{orderID}")]
        public async Task<IActionResult> EditOrder(OrderResponse orderResponse)
        {
            if (orderResponse == null)
            {
                return RedirectToAction("Orders");
            }

            var orderInDb = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderResponse.Id);
            if (ModelState.IsValid)
            {
                if (orderInDb != null)
                {
                    orderInDb.OwnerId = orderResponse.OwnerId;
                    orderInDb.SecurityLevel = orderResponse.SecurityLevel;
                    orderInDb.TypeOfService = orderResponse.TypeOfService;
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Orders", "Admin");
                }
            }
            else
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View(orderResponse);
            }

            return View(orderResponse);
        }
        [HttpGet]
        [Route("Orders/DeleteOrder/{orderID}")]
        public async Task<IActionResult> DeleteOrder(Guid orderID)
        {
            OrderResponse? orderResponse = await _ordersService.GetOrderByOrderId(orderID);
            if (orderResponse == null)
            {
                return RedirectToAction("Orders");
            }

            return View(orderResponse);
        }

        [HttpPost]
        [Route("Orders/DeleteOrder/{orderID}")]
        public async Task<IActionResult> DeleteOrder(OrderResponse orderToDelete)
        {
            if (orderToDelete == null)
            {
                return RedirectToAction("Orders");
            }

            if (ModelState.IsValid)
            {
                    Order? order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderToDelete.Id);
                    if (order != null)
                    {
                        List<OrderGuards> orderGuards = await _context.OrderGuards.Where(x => x.OrderId == order.Id).ToListAsync();
                        List<GuardReport> guardReports = await _context.GuardReports.Where(x => x.OrderId == order.Id).ToListAsync();
                        foreach (GuardReport guardReport in guardReports)
                        {
                            _context.GuardReports.Remove(guardReport);
                        }
                        foreach (OrderGuards orderGuard in orderGuards)
                        {
                            _context.OrderGuards.Remove(orderGuard);
                        }
                        _context.Orders.Remove(order);
                        await _context.SaveChangesAsync();
                    }
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Orders", "Admin");
                }
            else
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View(orderToDelete);
            }
        }

        [HttpGet]
        [Route("Ranks")]
        public async Task<IActionResult> Ranks(string searchBy, string? searchString, string sortBy = nameof(TerritoryResponse.Name), SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            _logger.LogInformation("Index action method of PersonsController");
            _logger.LogDebug($"searchBy: {searchBy}, searchString: {searchString}, sortOrder: {sortOrder}");
            //Search
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                { nameof(RankResponse.Name), "Name" },
                { nameof(RankResponse.PayPerHour), "PayPerHour" },
                { nameof(RankResponse.Description), "Description" },
            };
            List<RankResponse> ranks = await _ranksService.GetAllRanks();

            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;

            //Sort
            List<RankResponse> sortedRanks = ranks;

            ViewBag.CurrentSortBy = sortBy.ToString();
            ViewBag.CurrentSortOrder = sortOrder.ToString();
            return View(sortedRanks);
        }

        [HttpGet]
        [Route("Ranks/EditRank/{rankID}")]
        public async Task<IActionResult> EditRank(Guid rankID)
        {
            RankResponse? rankResponse = await _ranksService.GetRankByRankId(rankID);
            if (rankResponse == null)
            {
                return RedirectToAction("Ranks");
            }

            return View(rankResponse);
        }
        [HttpPost]
        [Route("Ranks/EditRank/{rankID}")]
        public async Task<IActionResult> EditRank(RankResponse rankResponse)
        {
            if (rankResponse == null)
            {
                return RedirectToAction("Ranks");
            }

            var rankInDb = await _context.Ranks.FirstOrDefaultAsync(x => x.Id == rankResponse.Id);
            if (ModelState.IsValid)
            {
                if (rankInDb != null)
                {
                    rankInDb.Name = rankResponse.Name;
                    rankInDb.PayPerHour = rankResponse.PayPerHour;
                    rankInDb.Description = rankResponse.Description;
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Ranks", "Admin");
                }
            }
            else
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View(rankResponse);
            }

            return View(rankResponse);
        }

        [HttpGet]
        [Route("Equipment")]
        public async Task<IActionResult> Equipment(string searchBy, string? searchString, string sortBy = nameof(TerritoryResponse.Name), SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            _logger.LogInformation("Index action method of PersonsController");
            _logger.LogDebug($"searchBy: {searchBy}, searchString: {searchString}, sortOrder: {sortOrder}");
            //Search
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                { nameof(EquipmentResponse.Name), "Name" },
                { nameof(EquipmentResponse.Type), "Type" },
                { nameof(EquipmentResponse.Amount), "Amount" },
            };
            List<EquipmentResponse> equipments = await _equipmentService.GetAllEquipment();

            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;

            //Sort
            List<EquipmentResponse> equpmentSort = equipments;

            ViewBag.CurrentSortBy = sortBy.ToString();
            ViewBag.CurrentSortOrder = sortOrder.ToString();
            return View(equpmentSort);
        }

        [HttpGet]
        [Route("Equipment/EditEquipment/{equipmentID}")]
        public async Task<IActionResult> EditEquipment(Guid equipmentID)
        {
            EquipmentResponse? equipmentResponse = await _equipmentService.GetEquipmentByEquipmentId(equipmentID);
            if (equipmentResponse == null)
            {
                return RedirectToAction("Equipment");
            }

            return View(equipmentResponse);
        }
        [HttpPost]
        [Route("Equipment/EditEquipment/{equipmentID}")]
        public async Task<IActionResult> EditEquipment(EquipmentResponse equipmentResponse)
        {
            if (equipmentResponse == null)
            {
                return RedirectToAction("Equipment");
            }

            var equipmentInDb = await _context.Equipment.FirstOrDefaultAsync(x => x.Id == equipmentResponse.Id);
            if (ModelState.IsValid)
            {
                if (equipmentInDb != null)
                {
                    equipmentInDb.Name = equipmentResponse.Name;
                    equipmentInDb.Type = equipmentResponse.Type;
                    equipmentInDb.Amount = equipmentResponse.Amount;
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Equipment", "Admin");
                }
            }
            else
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View(equipmentResponse);
            }

            return View(equipmentResponse);
        }
        [HttpGet]
        [Route("Equipment/DeleteEquipment/{equipmentID}")]
        public async Task<IActionResult> DeleteEquipment(Guid equipmentID)
        {
            EquipmentResponse? equipmentResponse = await _equipmentService.GetEquipmentByEquipmentId(equipmentID);
            if (equipmentResponse == null)
            {
                return RedirectToAction("Equipment");
            }

            return View(equipmentResponse);
        }
        [HttpPost]
        [Route("Equipment/DeleteEquipment/{equipmentID}")]
        public async Task<IActionResult> DeleteEquipment(EquipmentResponse equipmentToDelete)
        {
            if (equipmentToDelete == null)
            {
                return RedirectToAction("Equipment");
            }

            if (ModelState.IsValid)
            {
                Equipment? equipment = await _context.Equipment.FirstOrDefaultAsync(x => x.Id == equipmentToDelete.Id);
                if (equipment != null)
                {
                    List<EquipmentClaims> equipmentClaims = await _context.EquipmentClaims.Where(x => x.EquipmentId == equipment.Id).ToListAsync();
                    foreach (EquipmentClaims equipmentClaim in equipmentClaims)
                    {
                        _context.EquipmentClaims.Remove(equipmentClaim);
                    }
                    _context.Equipment.Remove(equipment);
                    await _context.SaveChangesAsync();
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Equipment", "Admin");
            }
            else
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View(equipmentToDelete);
            }
        }

        [HttpGet]
        [Route("Equipment/AddEquipment/")]
        public async Task<IActionResult> AddEquipment()
        {
            return View();
        }

        [HttpPost]
        [Route("Equipment/AddEquipment/")]
        public async Task<IActionResult> AddEquipment(EquipmentAddRequest equipmentAddRequest)
        {
            if (!ModelState.IsValid)
            {

                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View(equipmentAddRequest);
            }
            //call the service method
            EquipmentResponse equipmentResponse = await _equipmentService.AddEquipment(equipmentAddRequest);

            //navigate to Index() action method (its makes another get request to "owner/index")
            return RedirectToAction("Equipment", "Admin");
        }


        [Route("Equipment/EquipmentPDF/")]
        public async Task<IActionResult> EquipmentPDF()
        {
            List<EquipmentResponse> equipment = await _equipmentService.GetAllEquipment();

            //Return view as pdf
            return new ViewAsPdf("EquipmentPDF", equipment, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins()
                {
                    Top = 20,
                    Right = 20,
                    Bottom = 20,
                    Left = 20
                },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Login), "Home");
        }
    }
}
