﻿using BookShopEFConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopEFConsoleApp.Interfaces
{
    public interface IPromotion
    {
        Task<IEnumerable<Promotion>> GetAllPromotionsAsync();
        Task<Promotion> GetPromotionAsync(int id);

        Task AddPromotionAsync(Promotion promotion);
        Task EditPromotionAsync(Promotion promotion);
        Task DeletePromotionAsync(Promotion promotion);
    }

}
