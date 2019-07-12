using Moq;
using StoreData;
using StoreData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace StoreTestProject
{
    public class OrderMethodsUnitTests
    {
        [Fact]
        public void GetNumberOfItems_NumberOfItems()
        {
            var mock = new Mock<IOrderService>();
            mock.Setup(m => m.GetNumberOfItems(1)).Returns(4);

            Assert.Equal(4, mock.Object.GetNumberOfItems(1));
        }


        [Fact]
        public void GetOrdersByUser_NotNull_StatusId()
        {
            Order order = new Order
            {
                Id = 1,
                StatusId = 3

            };

            List<Order> ordersToReturn = new List<Order> { order };

            var mock = new Mock<IOrderService>();
            mock.Setup(m => m.GetOrdersByUser(1)).Returns(ordersToReturn);

            Assert.NotEmpty(mock.Object.GetOrdersByUser(1));
            Assert.Equal(3, mock.Object.GetOrdersByUser(1).First().StatusId);
        }
 
        [Fact]
        public void GetPaymentMethod_NotNull_PaymentMethodName_()
        {
            var mock = new Mock<IOrderService>();
            mock.Setup(m => m.GetPaymentMethod(1)).Returns("CreditCard");

            Assert.NotNull(mock.Object.GetPaymentMethod(1));
            Assert.Equal("CreditCard", mock.Object.GetPaymentMethod(1));
        }
       
        [Fact]
        public void GetConfirmedOrders_NotNull_OrderId_StatusId()
        {
            Order order1 = new Order { Id = 1, StatusId = 3 };
            Order order2 = new Order { Id = 2, StatusId = 3 };

            List<Order> ordersToReturn = new List<Order> {order1,order2};

            var mock = new Mock<IOrderService>();
            mock.Setup(m => m.GetConfirmedOrders()).Returns(ordersToReturn);

            Assert.NotEmpty(mock.Object.GetConfirmedOrders());
            Assert.Equal(1, mock.Object.GetConfirmedOrders().First().Id);
            Assert.Equal(2, mock.Object.GetConfirmedOrders().Last().Id);
            Assert.Equal(3, mock.Object.GetConfirmedOrders().First().StatusId);
        }
 
        [Fact]
        public void GetPendingOrders_NotNull_OrderId_StatusId()
        {
            Order order1 = new Order { Id = 1, StatusId = 3 };
            Order order2 = new Order { Id = 5, StatusId = 3 };

            List<Order> ordersToReturn = new List<Order> { order1, order2 };

            var mock = new Mock<IOrderService>();
            mock.Setup(m => m.GetPendingOrders()).Returns(ordersToReturn);

            Assert.NotEmpty(mock.Object.GetPendingOrders());
            Assert.Equal(1, mock.Object.GetPendingOrders().First().Id);
            Assert.Equal(5, mock.Object.GetPendingOrders().Last().Id);
            Assert.Equal(3, mock.Object.GetPendingOrders().First().StatusId);
        }

        [Fact]
        public void GetAllOrders_NotNull_OrderId_StatusId()
        {
            Order order1 = new Order { Id = 4, StatusId = 3 };
            Order order2 = new Order { Id = 3, StatusId = 3 };

            List<Order> ordersToReturn = new List<Order> { order1, order2 };

            var mock = new Mock<IOrderService>();
            mock.Setup(m => m.GetAllOrders()).Returns(ordersToReturn);

            Assert.NotEmpty(mock.Object.GetAllOrders());
            Assert.Equal(4, mock.Object.GetAllOrders().First().Id);
            Assert.Equal(3, mock.Object.GetAllOrders().Last().Id);
            Assert.Equal(3, mock.Object.GetAllOrders().First().StatusId);
        }

        [Fact]
        public void GetOrdersByBillingInfo_NotNull_OrderId_BillingInfoId()
        {
            Order order1 = new Order { Id = 1, BillingInfoId = 45 };
            Order order2 = new Order { Id = 2, BillingInfoId = 44 };

            List<Order> ordersToReturn = new List<Order> { order1, order2 };

            var mock = new Mock<IOrderService>();
            mock.Setup(m => m.GetOrdersByBillingInfo(1)).Returns(ordersToReturn);

            Assert.NotEmpty(mock.Object.GetOrdersByBillingInfo(1));
            Assert.Equal(1, mock.Object.GetOrdersByBillingInfo(1).First().Id);
            Assert.Equal(2, mock.Object.GetOrdersByBillingInfo(1).Last().Id);
            Assert.Equal(45, mock.Object.GetOrdersByBillingInfo(1).First().BillingInfoId);
            Assert.Equal(44, mock.Object.GetOrdersByBillingInfo(1).Last().BillingInfoId);
        }

        [Fact]
        public void GetOrderByInvoiceId_NotNull_OrderId_InvoiceId()
        {
            Order order1 = new Order { Id = 1, InvoiceId = 45 };
            Order order2 = new Order { Id = 2, InvoiceId = 45 };

            List<Order> ordersToReturn = new List<Order> { order1, order2 };

            var mock = new Mock<IOrderService>();
            mock.Setup(m => m.GetOrderByInvoiceId(45)).Returns(ordersToReturn);

            Assert.NotEmpty(mock.Object.GetOrderByInvoiceId(45));
            Assert.Equal(1, mock.Object.GetOrderByInvoiceId(45).First().Id);
            Assert.Equal(2, mock.Object.GetOrderByInvoiceId(45).Last().Id);
            Assert.Equal(45, mock.Object.GetOrderByInvoiceId(45).First().InvoiceId);
            Assert.Equal(45, mock.Object.GetOrderByInvoiceId(45).Last().InvoiceId);
        }

        [Fact]
        public void GetBillingInfoById_NotNull_BillingInfoId()
        {
            BillingInfo billingInfoToReturn = new BillingInfo
            {
                Id = 1,
                Adress = "fakeAdress"
            };

            var mock = new Mock<IOrderService>();
            mock.Setup(m => m.GetBillingInfoById(1)).Returns(billingInfoToReturn);

            Assert.NotNull(mock.Object.GetBillingInfoById(1));
            Assert.Equal(1, mock.Object.GetBillingInfoById(1).Id);
            Assert.Equal("fakeAdress", mock.Object.GetBillingInfoById(1).Adress);
        }

        [Fact]
        public void GetStatus_NotNull_StatusText()
        {
            Status statusToReturn = new Status
            {
                Id = 1,
                StatusText = "statusText"
            };

            var mock = new Mock<IOrderService>();
            mock.Setup(m => m.GetStatus(1)).Returns(statusToReturn.StatusText);

            Assert.NotNull(mock.Object.GetStatus(1));
            Assert.Equal("statusText", mock.Object.GetStatus(1));
        }

        [Fact]
        public void GetInvoices_NotNull_InvoiceId_UserId()
        {
            Invoice invoice1 = new Invoice { Id = 1, UserId = 1 };
            Invoice invoice2 = new Invoice { Id = 1, UserId = 2 };

            List<Invoice> invoicesToReturn = new List<Invoice> { invoice1, invoice2 };

            var mock = new Mock<IOrderService>();
            mock.Setup(m => m.GetInvoices("fakeUser")).Returns(invoicesToReturn);

            Assert.NotEmpty(mock.Object.GetInvoices("fakeUser"));
            Assert.Equal(1, mock.Object.GetInvoices("fakeUser").First().Id);
            Assert.Equal(1, mock.Object.GetInvoices("fakeUser").Last().Id);
            Assert.Equal(1, mock.Object.GetInvoices("fakeUser").First().UserId);
            Assert.Equal(2, mock.Object.GetInvoices("fakeUser").Last().UserId);
        }

        [Fact]
        public void GetInvoice_NotNull_InvoiceId_UserId()
        {
            Invoice invoice1 = new Invoice { Id = 1, UserId = 1 };
  
            var mock = new Mock<IOrderService>();
            mock.Setup(m => m.GetInvoice(1)).Returns(invoice1);

            Assert.NotNull(mock.Object.GetInvoice(1));
            Assert.Equal(1, mock.Object.GetInvoice(1).Id);
            Assert.Equal(1, mock.Object.GetInvoice(1).UserId);
        }

        [Fact]
        public void GetLastInvoice_NotNull_InvoiceId_UserId()
        {
            Invoice invoice1 = new Invoice { Id = 1, UserId = 1 };

            var mock = new Mock<IOrderService>();
            mock.Setup(m => m.GetLastInvoice()).Returns(invoice1);

            Assert.NotNull(mock.Object.GetLastInvoice());
            Assert.Equal(1, mock.Object.GetLastInvoice().Id);
            Assert.Equal(1, mock.Object.GetLastInvoice().UserId);
        }       

        [Fact]
        public void GetInvoiceDate_NotNull_InvoiceDate()
        {
            var date = DateTimeOffset.Now;
            Invoice invoice1 = new Invoice { Id = 1, UserId = 1, InvoiceDate = date };

            var mock = new Mock<IOrderService>();
            mock.Setup(m => m.GetInvoiceDate(1)).Returns(invoice1.InvoiceDate);

            Assert.Equal(date,mock.Object.GetInvoiceDate(1));
        }

    }
}
