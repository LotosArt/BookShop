using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookShopEFConsoleApp.Models;

namespace BookShopEFConsoleApp.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Comment { get; set; }
        public byte Stars { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }
        
        public override string ToString()
        {
            return String.Format("User - {0}\nComment - {1}\nStars - {2}", UserName, Comment, Stars);
        }
    }

}
