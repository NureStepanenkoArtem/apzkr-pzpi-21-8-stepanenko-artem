using OfficeOpenXml;
using SecureAndObserve.Core.Domain.Entities;
using SecureAndObserve.Core.Domain.RepositoryContracts;
using SecureAndObserve.Core.DTO;
using SecureAndObserve.Core.Enums;
using SecureAndObserve.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecureAndObserve.Core.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository _ordersRepository;

        public OrdersService(IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }
        public async Task<OrderResponse> AddOrder(OrderAddRequest? orderAddRequest)
        {
            if(orderAddRequest == null)
                throw new ArgumentNullException(nameof(orderAddRequest));
            if(orderAddRequest.OwnerId == null)
                throw new ArgumentException(nameof(orderAddRequest.OwnerId));
            Order order = orderAddRequest.ToOrder();
            order.Id = Guid.NewGuid();
            await _ordersRepository.AddOrder(order);
            return order.ToOrderResponse();
        }
        public async Task<List<OrderResponse>> GetAllOrders()
        {
            List<Order> orders = await _ordersRepository.GetAllOrders();
            return orders.Select(order => order.ToOrderResponse()).ToList();
        }
        public async Task<OrderResponse?> GetOrderByOrderId(Guid? orderId)
        {
            if (orderId == null)
                return null;
            Order? order = await _ordersRepository.GetOrderByOrderId(orderId.Value);
            if(order == null)
                return null;
            return order.ToOrderResponse();
        }
        public async Task<int> CostOfService(Guid orderId)
        {
            return await _ordersRepository.CostOfService(orderId);
        }

        public async Task<MemoryStream> GetOrdersExcel()
        {
            MemoryStream memoryStream = new MemoryStream();
            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets.Add("OrdersSheet");
                workSheet.Cells["A1"].Value = "ID";
                workSheet.Cells["B1"].Value = "OwnerId";
                workSheet.Cells["C1"].Value = "TypeOfService";
                workSheet.Cells["D1"].Value = "Security level";

                using (ExcelRange headerCells = workSheet.Cells["A1:D1"])
                {
                    headerCells.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    headerCells.Style.Font.Bold = true;
                }

                int row = 2;
                List<OrderResponse> orders = await GetAllOrders();
                foreach (OrderResponse order in orders)
                {
                    workSheet.Cells[row, 1].Value = order.Id;
                    workSheet.Cells[row, 2].Value = order.OwnerId;
                    workSheet.Cells[row, 3].Value = order.TypeOfService;
                    workSheet.Cells[row, 4].Value = order.SecurityLevel;
                    row++;
                }

                workSheet.Cells[$"A1:H{row}"].AutoFitColumns();

                await excelPackage.SaveAsync();
            }

            memoryStream.Position = 0;
            return memoryStream;
        }

        public async Task<bool> DeleteOrder(Guid? orderID)
        {
            if (orderID == null)
            {
                throw new ArgumentNullException(nameof(orderID));
            }
            Order? order = await _ordersRepository.GetOrderByOrderId(orderID.Value);
            if (order == null)
            {
                return false;
            }
            await _ordersRepository.DeleteOrderByOrderID(orderID.Value);
            return true;
        }

        
    }
    
}
