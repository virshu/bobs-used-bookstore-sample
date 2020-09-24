﻿using BOBS_Backend.Models.Order;
using BOBS_Backend.ViewModel.ManageOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOBS_Backend.Repository.OrdersInterface
{
    public interface IOrderRepository
    {

        /*
         * Order Detail Repository Interface
         */

        Task<Order> FindOrderById(long id);

        Task<ManageOrderViewModel> GetAllOrders(int pageNum);

        Task<ManageOrderViewModel> FilterList(string filterValue, string searchString, int pageNum);


        IQueryable<Order> FilterOrder(string filterValue, string searchString, string inBetween, string operand, string negate);

     }
}