using BookShopEFConsoleApp.Data;
using BookShopEFConsoleApp.Interfaces;
using BookShopEFConsoleApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShopEFConsoleApp.Repositoty;

public class BookRepository : IBook
{

    public async Task<IEnumerable<Book>> GetAllBooksAsync()
    {
        using (ApplicationContext context = Program.DbContext())
        {
            return await context.Books.ToListAsync();
        }
        
    }

    public async Task<IEnumerable<Book>> GetAllBooksWithAuthorsAsync()
    {
        await using (ApplicationContext context = Program.DbContext())
        {
            return await context.Books.Include(b => b.Authors).ToListAsync();    
        }
    }

    public async Task<Book?> GetBookAsync(int id)
    {
        await using (ApplicationContext context = Program.DbContext())
        {
            return await context.Books.Include(b => b.Authors).FirstOrDefaultAsync(b => b.Id == id);
        }
    }

    public async Task<IEnumerable<Book>> GetBooksByNameAsync(string name)
    {
        await using (ApplicationContext context = Program.DbContext())
        {
            return await context.Books.Where(b => b.Title.Contains(name)).ToListAsync();
        }
    }

    public async Task<Book?> GetBookWithPromotionAsync(int id)
    {
        await using (ApplicationContext context = Program.DbContext())
        {
            return await  context.Books.Include(b => b.Promotion).FirstOrDefaultAsync(b => b.Id == id);
        }
    }

    public async Task<Book?> GetBookWithAuthorsAsync(int id)
    {
        await using (ApplicationContext context = Program.DbContext())
        {
            return await context.Books.Include(b => b.Authors).FirstOrDefaultAsync(b => b.Id == id);
        }
    }

    public async Task<Book?> GetBookWithCategoryAndAuthorsAsync(int id)
    {
        await using (ApplicationContext context = Program.DbContext())
        {
            return await context.Books
                .Include(b => b.Category)
                .Include(b => b.Authors)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
    }

    public async Task<Book?> GetBookWithAuthorsAndReviewAsync(int id)
    {
        await using (ApplicationContext context = Program.DbContext())
        {
            return await context.Books
                .Include(b => b.Reviews)
                .Include(b => b.Authors)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
    }

    public async Task<Book?> GetBooksWithAuthorsAndReviewAndCategoryAsync(int id)
    {
        await using (ApplicationContext context = Program.DbContext())
        {
            return await context.Books
                .Include(b => b.Reviews)
                .Include(b => b.Authors)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
    }

    public async Task AddBookAsync(Book book)
    {
        await using (ApplicationContext context = Program.DbContext())
        {
            context.Books.Add(book);
            await context.SaveChangesAsync();
        }
    }

    public async Task DeleteBookAsync(Book book)
    {
        await using (ApplicationContext context = Program.DbContext())
        {
            context.Books.Remove(book);
            await context.SaveChangesAsync();
        }
    }

    public async Task EditBookAsync(Book book)
    {
        await using (ApplicationContext context = Program.DbContext())
        {
            var currentBook = await context.Books.FirstOrDefaultAsync(b => b.Id == book.Id);
            if (currentBook != null)
            {
                currentBook.Title = book.Title;
                currentBook.Description = book.Description;
                currentBook.PublishedOn = book.PublishedOn;
                currentBook.Publisher = book.Publisher;
                currentBook.Price = book.Price;
                currentBook.Promotion = book.Promotion;
                currentBook.CategoryId = book.CategoryId;
                currentBook.Authors = new List<Author>();
                foreach (Author author in book.Authors)
                {
                    currentBook.Authors.Add(author);
                }
                currentBook.Reviews = new List<Review>();
                foreach (Review review in book.Reviews)
                {
                    currentBook.Reviews.Add(review);
                }
                await context.SaveChangesAsync();
            }
        }
    }
}