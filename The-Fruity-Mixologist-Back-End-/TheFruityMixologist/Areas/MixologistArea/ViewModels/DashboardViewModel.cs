﻿using TheFruityMixologist.Entities;

namespace TheFruityMixologist.Areas.MixologistArea.ViewModels
{
    public class DashboardViewModel
    {
        public List<Order> AcceptedOrders { get; set; }
        public List<Order> PendingOrders { get; set; }
        public List<Order> LatestOrders { get; set; }
        public List<User> Users { get; set; }
        public double Alcohol { get; set; }
        public double NonAlcohol { get; set; }
        public double SundayOrders { get; set; }
        public double SundayIncome { get; set; }
        public double MondayOrders { get; set; }
        public double MondayIncome { get; set; }
        public double TuesdayOrders { get; set; }
        public double TuesdayIncome { get; set; }
        public double WednesdayOrders { get; set; }
        public double WednesdayIncome { get; set; }
        public double ThursdayOrders { get; set; }
        public double ThursdayIncome { get; set; }
        public double FridayOrders { get; set; }
        public double FridayIncome { get; set; }
        public double SaturdayOrders { get; set; }
        public double SaturdayIncome { get; set; }
    }
}
