﻿using BookShopEFConsoleApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopEFConsoleApp.Interfaces
{
    public interface ICategory
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<IEnumerable<Category>> GetCategoriesByNameAsync(string name);
        Task<Category> GetCategoryAsync(int id);
        Task<Category> GetCategoryWithBooksAsync(int id);

        Task AddCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(Category category);
    }

}
