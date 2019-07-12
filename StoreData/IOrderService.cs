using StoreData.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StoreData
{
    public interface IOrderService 
    {

        Task<Order> AddOrder(Order order);
        Task<Order> RemoveOrder(int orderId);
        IEnumerable<Order>GetOrdersByUser(int userId);
        IEnumerable<Order> GetConfirmedOrders();
        IEnumerable<Order> GetPendingOrders();
        IEnumerable<Order> GetAllOrders();
        IEnumerable<Order> GetOrdersByBillingInfo(int billInfoId);
        IEnumerable<Order> GetOrderByInvoiceId(int invoiceId);
        IEnumerable<Invoice> GetInvoices(string userName);
        Invoice GetInvoice(int invoiceId);
        Invoice GetLastInvoice();
        DateTimeOffset GetInvoiceDate(int invoiceId);
        Task<Invoice> AddInvoice(Invoice invoice);
        int GetNumberOfItems(int userId);
        Task UpdateOrders(string PaymentMethod, int BillingInfoId, int userId);
        BillingInfo GetBillingInfoById(int infoId);
        Task<BillingInfo> AddBillingInfo(BillingInfo info);
        Task ChangeOrderStatus(int userId, int status);
        Task ChangeOrderStatusesToPend(int billingInfoId);
        Task ChangeOrderStatuses(int billingInfoId, int statusId);
        string GetPaymentMethod(int paymentId);
        string GetStatus(int statusId);
        PaymentMethod AddPaymentMethod(PaymentMethod paymentMehod);
        IEnumerable<PaymentMethod> GetAllPaymentMethods();
        Status AddStatus(Status status);



    }
}
