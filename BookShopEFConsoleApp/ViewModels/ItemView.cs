using BookShopEFConsoleApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopEFConsoleApp.ViewModels
{
    public struct ItemView : IShow<int>
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
}
