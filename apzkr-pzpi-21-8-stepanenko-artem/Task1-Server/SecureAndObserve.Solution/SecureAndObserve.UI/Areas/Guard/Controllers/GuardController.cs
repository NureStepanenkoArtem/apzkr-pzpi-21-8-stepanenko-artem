using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureAndObserve.Core.Domain.Entities;
using SecureAndObserve.Core.Domain.IdentityEntities;
using SecureAndObserve.Core.DTO;
using SecureAndObserve.Core.Enums;
using SecureAndObserve.Core.ServiceContracts;
using SecureAndObserve.Infrastructure.DbContext;
using SecureAndObserve.UI.Areas.Owner.Controllers;
using SecureAndObserve.UI.Controllers;
using System.Globalization;
using System.Security.Claims;

namespace SecureAndObserve.UI.Areas.Guard.Controllers
{
    [Area("Guard")]
    [Route("[controller]")]
    public class GuardController : Controller
    {

        private readonly ITerritoriesService _territoriesService;
        private readonly IOrdersService _ordersService;
        private readonly IGuardExstensionsService _guardExstensionsService;
        private readonly IOrderGuardsService _orderGuardsService;
        private readonly IGuardReportsService _guardReportsService;
        private readonly IEquipmentService _equipmentService;
        private readonly IEquipmentClaimsService _equipmentClaimsService;
        private readonly ILogger<GuardController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public GuardController(ITerritoriesService territoriesService, ILogger<GuardController> logger, ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, IOrdersService ordersService, IGuardExstensionsService guardExstensionsService, IOrderGuardsService orderGuardsService, IGuardReportsService guardReportsService, IEquipmentService equipmentService, IEquipmentClaimsService equipmentClaimsService)
        {
            _territoriesService = territoriesService;
            _logger = logger;
            _context = context;
            _signInManager = signInManager;
            _ordersService = ordersService;
            _guardExstensionsService = guardExstensionsService;
            _orderGuardsService = orderGuardsService;
            _guardReportsService = guardReportsService;
            _equipmentService = equipmentService;
            _equipmentClaimsService = equipmentClaimsService;
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

            List<GuardExstensionsResponse> guardExstensionsList = await _guardExstensionsService.GetAllGuardExstensions();
            Guid guardExstentionsId = new Guid();

            foreach(GuardExstensionsResponse guardExstensionsResponse in guardExstensionsList)
            {
                if(Convert.ToString(guardExstensionsResponse.UserId) == userId)
                    guardExstentionsId = guardExstensionsResponse.Id;
            }
            List<OrderGuards> orderGuards = await _context.OrderGuards.ToListAsync();
            List<OrderGuards> finalOrderGuards = new List<OrderGuards>();
            foreach(OrderGuards orderGuardsObject in orderGuards)
            {
                if(orderGuardsObject.GuardExstensionsId == guardExstentionsId)
                    finalOrderGuards.Add(orderGuardsObject);
            }
            List<OrderResponse> ordersList = await _ordersService.GetAllOrders();
            List<OrderResponse> finalOrdersList = new List<OrderResponse>();
            foreach(OrderResponse orderResponse in ordersList)
            {
                foreach(OrderGuards orderGuardsObject in finalOrderGuards)
                {
                    if(orderGuardsObject.OrderId == orderResponse.Id)
                        finalOrdersList.Add(orderResponse);
                }
            }

            List<TerritoryResponse> territoriesList = await _territoriesService.GetAllTerritories();

            foreach(TerritoryResponse territory in territoriesList)
            {
                foreach(OrderResponse order in finalOrdersList)
                {
                    if(territory.OwnerId == order.OwnerId)
                        finalTerritories.Add(territory);
                }
            }

            return View(finalTerritories);
        }

        [HttpGet]
        [Route("Orders")]
        public async Task<IActionResult> Orders(string searchBy, string? searchString, string sortBy = nameof(TerritoryResponse.Name), SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                { nameof(OrderResponse.TypeOfService), "TypeOfService" },
                { nameof(OrderResponse.SecurityLevel), "SecurityLevel" },
            };
            List<TerritoryResponse> territories = await _territoriesService.GetFilteredTerritories(searchBy, searchString);

            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;

            //Sort
            List<TerritoryResponse> sortedTerritories = await _territoriesService.GetSortedTerritories(territories, sortBy, sortOrder);

            ViewBag.CurrentSortBy = sortBy.ToString();
            ViewBag.CurrentSortOrder = sortOrder.ToString();

            List<OrderResponse> orders = await _ordersService.GetAllOrders();
            ViewBag.Order = orders;
            return View(orders);
        }

        [HttpGet]
        [Route("AddOrder/{orderID}")]
        public async Task<IActionResult> AddOrder(Guid orderID)
        {
            OrderResponse? orderResponse = await _ordersService.GetOrderByOrderId(orderID);
            if (orderResponse == null)
                return RedirectToAction("Index");
            return View(orderResponse);
        }

        [HttpPost]
        [Route("AddOrder/{orderID}")]
        public async Task<IActionResult> AddOrder(OrderResponse orderResponseToAdd)
        {
            OrderResponse? orderResponse = await _ordersService.GetOrderByOrderId(orderResponseToAdd.Id);
            if (orderResponse == null)
                return RedirectToAction("Index");

            var userClaims = User.Identity as ClaimsIdentity;
            var userId = userClaims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            Guid userGuid = new Guid(userId);
            GuardExstensions? guardExstensionsResponse = await _context.GuardExstensions.FirstOrDefaultAsync(x => x.UserId == userGuid);

            List<OrderGuardsResponse> orderGuardsResponses = await _orderGuardsService.GetAllOrderGuards();
            List<OrderGuardsResponse> finalOrderGuardsResponses = new List<OrderGuardsResponse>();
            foreach(OrderGuardsResponse orderGuardResponse in orderGuardsResponses)
            {
                if(orderGuardResponse.GuardExstensionsId == guardExstensionsResponse.Id)
                    finalOrderGuardsResponses.Add(orderGuardResponse);
            }
            bool isInOrder = false;
            foreach(OrderGuardsResponse orderGuardsResponseObject in finalOrderGuardsResponses)
            {
                if(orderGuardsResponseObject.OrderId == orderResponseToAdd.Id)
                    isInOrder = true;
            }

            if(!isInOrder)
            {
                OrderGuardsAddRequest orderGuardsAddRequest = new OrderGuardsAddRequest();
                orderGuardsAddRequest.OrderId = orderResponseToAdd.Id;
                orderGuardsAddRequest.GuardExstensionsId = guardExstensionsResponse.Id;
                OrderGuardsResponse orderGuardsFinalResponse = await _orderGuardsService.AddOrderGuards(orderGuardsAddRequest);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("ViewOrder/{territoryID}")]
        public async Task<IActionResult> ViewOrderDetails(Guid territoryID)
        {
            string? ownerId = _context.Territories.Where(x => x.Id == territoryID).Select(x => x.OwnerId).FirstOrDefault().ToString();
            Guid ownerGuid = new Guid(ownerId);
            string? orderId = _context.Orders.Where(x => x.OwnerId == ownerGuid).Select(x => x.Id).FirstOrDefault().ToString();
            Guid orderGuid = new Guid(orderId);
            OrderResponse? orderResponse = await _ordersService.GetOrderByOrderId(orderGuid);

            var userClaims = User.Identity as ClaimsIdentity;
            var userId = userClaims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            Guid userGuid = new Guid(userId);
            string? guardExstentionsId = _context.GuardExstensions.Where(x => x.UserId == userGuid).Select(x => x.Id).FirstOrDefault().ToString();
            string? rankId = _context.GuardExstensions.Where(x => x.UserId == userGuid).Select(x => x.RankId).FirstOrDefault().ToString();
            Guid rankGuid = new Guid(rankId);
            string payPerHour = _context.Ranks.Where(x => x.Id == rankGuid).Select(x => x.PayPerHour).FirstOrDefault().ToString();

            double orientedPayPerHour = Convert.ToDouble(payPerHour);

            if (orderResponse.TypeOfService == ServiceTypeOptions.PlacementOnPosts.ToString())
                orientedPayPerHour = orientedPayPerHour * 1.1;
            if (orderResponse.TypeOfService == ServiceTypeOptions.Patrol.ToString())
                orientedPayPerHour = orientedPayPerHour * 1.3;

            if (orderResponse.TypeOfService == SecurityLevelOptions.Medium.ToString())
                orientedPayPerHour = orientedPayPerHour * 1.3;
            if (orderResponse.TypeOfService == SecurityLevelOptions.High.ToString())
                orientedPayPerHour = orientedPayPerHour * 1.5;

            ViewBag.OrientedPayPerHour = orientedPayPerHour;
            ViewBag.OwnerEmail = _context.Users.Where(x => x.Id == ownerGuid).Select(x => x.Email).FirstOrDefault().ToString();
            return View(orderResponse);
        }

        [HttpPost]
        [Route("ViewOrder/{territoryID}")]
        public async Task<IActionResult> ViewOrderDetails(OrderResponse orderResponseToDelete)
        {
            string? orderGuardsId = _context.OrderGuards.Where(x => x.OrderId == orderResponseToDelete.Id).Select(x => x.Id).FirstOrDefault().ToString();
            Guid orderGuardsGuid = new Guid(orderGuardsId);

            OrderGuardsResponse? orderGuardsResponse = await _orderGuardsService.GetOrderGuardsByOrderGuardsId(orderGuardsGuid);

            if (orderGuardsResponse == null)
                return RedirectToAction("Index");

            await _orderGuardsService.DeleteOrderGuards(orderGuardsResponse.Id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Route("ViewReports/{territoryID}")]
        public async Task<IActionResult> ViewReports(Guid territoryID)
        {
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                { nameof(GuardReportResponse.Date), "Date" },
                { nameof(GuardReportResponse.Message), "Message" },
                { nameof(GuardReportResponse.Descriptions), "Descriptions" },
            };

            string? ownerId = _context.Territories.Where(x => x.Id == territoryID).Select(x => x.OwnerId).FirstOrDefault().ToString();
            Guid ownerGuid = new Guid(ownerId);
            string? orderId = _context.Orders.Where(x => x.OwnerId == ownerGuid).Select(x => x.Id).FirstOrDefault().ToString();
            Guid orderGuid = new Guid(orderId);
            OrderResponse? orderResponse = await _ordersService.GetOrderByOrderId(orderGuid);

            List<GuardReportResponse> guardReportResponseList = await _guardReportsService.GetAllGuardReports();
            List<GuardReportResponse> finalGuardReportResponseList = new List<GuardReportResponse>();
            foreach(GuardReportResponse guardReport in guardReportResponseList)
            {
                if(guardReport.OrderId == orderResponse.Id)
                finalGuardReportResponseList.Add(guardReport);
            }
            ViewBag.TerritoryId = territoryID.ToString();
            return View(finalGuardReportResponseList);
        }
        [HttpGet]
        [Route("CreateReport/{territoryID}")]
        public async Task<IActionResult> CreateReport(Guid territoryID)
        {
            string? ownerId = _context.Territories.Where(x => x.Id == territoryID).Select(x => x.OwnerId).FirstOrDefault().ToString();
            Guid ownerGuid = new Guid(ownerId);
            string? orderId = _context.Orders.Where(x => x.OwnerId == ownerGuid).Select(x => x.Id).FirstOrDefault().ToString();
            Guid orderGuid = new Guid(orderId);
            OrderResponse? orderResponse = await _ordersService.GetOrderByOrderId(orderGuid);

            var userClaims = User.Identity as ClaimsIdentity;
            var userId = userClaims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            Guid userGuid = new Guid(userId);
            string? guardExstentionsId = _context.GuardExstensions.Where(x => x.UserId == userGuid).Select(x => x.Id).FirstOrDefault().ToString();

            GuardReportAddRequest guardReportAddRequest = new GuardReportAddRequest();
            guardReportAddRequest.OrderId = orderGuid;
            guardReportAddRequest.GuardExstensionsId = new Guid(guardExstentionsId);
            guardReportAddRequest.Date = DateTime.Now;

            ViewBag.TerritoryId = territoryID;

            return View(guardReportAddRequest);
        }
        [HttpPost]
        [Route("CreateReport/{territoryID}")]
        public async Task<IActionResult> CreateReport(GuardReportAddRequest guardReportAddRequest)
        {
            GuardReportResponse guardReportResponse = await _guardReportsService.AddGuardReport(guardReportAddRequest);

            return RedirectToAction("Index");
        }
        [HttpGet]
        [Route("MyEquipment/")]
        public async Task<IActionResult> MyEquipment()
        {
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                { nameof(EquipmentResponse.Name), "Name" },
                { nameof(EquipmentResponse.Type), "Type" },
            };

            var userClaims = User.Identity as ClaimsIdentity;
            var userId = userClaims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var guardExstentionsId = _context.GuardExstensions.Where(x => x.UserId == new Guid(userId)).Select(x => x.Id).FirstOrDefault().ToString();

            List<EquipmentResponse> equipmentResponseList = await _equipmentService.GetAllEquipment();
            List<EquipmentClaims> equipmentClaimsList = _context.EquipmentClaims.Where(x => x.GuardExstensionsId == new Guid(guardExstentionsId)).ToList();
            List<EquipmentResponse> finalEquipmentResponseList = new List<EquipmentResponse>();
            foreach(EquipmentClaims equipmentClaims in equipmentClaimsList)
            {
                foreach(EquipmentResponse equipment in equipmentResponseList)
                {
                    if(equipmentClaims.GuardExstensionsId == new Guid(guardExstentionsId) && equipmentClaims.EquipmentId == equipment.Id)
                    {
                        finalEquipmentResponseList.Add(equipment);
                    }
                }
            }

            return View(finalEquipmentResponseList);
        }
        [HttpGet]
        [Route("AddEquipment")]
        public async Task<IActionResult> AddEquipment()
        {
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                { nameof(EquipmentResponse.Name), "Name" },
                { nameof(EquipmentResponse.Type), "Type" },
            };

            List<EquipmentResponse> equipmentResponseList = await _equipmentService.GetAllEquipment();

            return View(equipmentResponseList);
        }
        [HttpGet]
        [Route("EquipmentDetails/{equipmentID}")]
        public async Task<IActionResult> EquipmentDetails(Guid equipmentID)
        {
            EquipmentResponse? equipmentResponse = await _equipmentService.GetEquipmentByEquipmentId(equipmentID);
            if (equipmentResponse == null)
                return RedirectToAction("Index");


            return View(equipmentResponse);
        }
        [HttpPost]
        [Route("EquipmentDetails/{equipmentID}")]
        public async Task<IActionResult> EquipmentDetails(EquipmentResponse equipmentResponse)
        {
            var userClaims = User.Identity as ClaimsIdentity;
            var userId = userClaims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var guardExstentionsId = _context.GuardExstensions.Where(x => x.UserId == new Guid(userId)).Select(x => x.Id).FirstOrDefault().ToString();

            EquipmentClaims? equipmentClaims = await _context.EquipmentClaims.FirstOrDefaultAsync(x => x.GuardExstensionsId == new Guid(guardExstentionsId) && x.EquipmentId == equipmentResponse.Id);
            if (equipmentClaims == null)
            {
                EquipmentClaimsAddRequest equipmentClaimsAddRequest = new EquipmentClaimsAddRequest();
                equipmentClaimsAddRequest.EquipmentId = equipmentResponse.Id;
                equipmentClaimsAddRequest.GuardExstensionsId = new Guid(guardExstentionsId);
                EquipmentClaimsResponse equipmentClaimsResponse = await _equipmentClaimsService.AddEquipmentClaims(equipmentClaimsAddRequest);

            }

            return RedirectToAction("MyEquipment");
        }

        [HttpGet]
        [Route("UnpinEquipment/{equipmentID}")]
        public async Task<IActionResult> UnpinEquipment(Guid equipmentID)
        {
            var userClaims = User.Identity as ClaimsIdentity;
            var userId = userClaims.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var guardExstentionsId = _context.GuardExstensions.Where(x => x.UserId == new Guid(userId)).Select(x => x.Id).FirstOrDefault().ToString();

            EquipmentClaims? equipmentClaims = await _context.EquipmentClaims.FirstOrDefaultAsync(x => x.GuardExstensionsId == new Guid(guardExstentionsId) && x.EquipmentId == equipmentID);
            EquipmentResponse? equipmentResponse = await _equipmentService.GetEquipmentByEquipmentId(equipmentID);
            if (equipmentClaims != null)
            {
                ViewBag.Name = equipmentResponse.Name;
                ViewBag.Type = equipmentResponse.Type;
            }
            return View(equipmentClaims);
        }
        [HttpPost]
        [Route("UnpinEquipment/{equipmentID}")]
        public async Task<IActionResult> UnpinEquipment(EquipmentClaims equipmentClaims)
        {
            bool deleteEqCl = await _equipmentClaimsService.DeleteEquipmentClaims(equipmentClaims.Id); 
            return RedirectToAction("MyEquipment");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Login), "Home");
        }
    }
}
