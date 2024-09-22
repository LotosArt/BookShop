using BookShopEFConsoleApp.Data;
using BookShopEFConsoleApp.Helpers;
using BookShopEFConsoleApp.Interfaces;
using BookShopEFConsoleApp.Repository;
using BookShopEFConsoleApp.Repositoty;

namespace BookShopEFConsoleApp;

public partial class Program
{
    enum ShopMenu
    {
        Books, Authors, 
        Categories, Orders, 
        SearchAuthors, SearchBooks, 
        SearchCategories, SearchOrders, 
        AddBook, AddAuthor, 
        AddCategory, AddOrder, 
        Exit
    }

    private static IBook _books;
    private static IAuthor _authors;
    private static IOrder _orders;
    private static ICategory _categories;
    private static IPromotion _promotions;
    private static IReview _reviews;


    public static ApplicationContext DbContext() => new ApplicationContextFactory().CreateDbContext();

    static async Task Main()
    {
        Initialize();
        await Menu();

    }
    static async Task Menu()
    {
        int input = 0;
        do
        {
            input = ConsoleHelper.MultipleChoice(true, new ShopMenu());
            switch ((ShopMenu)input)
            {
                case ShopMenu.Books:
                    await ReviewBooks();
                    break;
                case ShopMenu.Authors:
                    await ReviewAuthors();
                    break;
                case ShopMenu.Categories:
                    await ReviewCategories();
                    break;
                case ShopMenu.Orders:
                    await ReviewOrders();
                    break;
                case ShopMenu.SearchAuthors:
                    await SearchAuthors();
                    break;
                case ShopMenu.SearchBooks:
                    await SearchBooks();
                    break;
                case ShopMenu.SearchCategories:
                    await SearchCategories();
                    break;
                case ShopMenu.SearchOrders:
                    await SearchOrders();
                    break;
                case ShopMenu.AddBook:
                    await AddBook();
                    break;
                case ShopMenu.AddAuthor:
                    await AddAuthor();
                    break;
                case ShopMenu.AddCategory:
                    await AddCategory();
                    break;
                case ShopMenu.AddOrder:
                    await AddOrder();
                    break;
                case ShopMenu.Exit:
                    break;
                default:
                    break;
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadLine();

        } while (ShopMenu.Exit != (ShopMenu)input);
    }
    static void Initialize()
    {
        new DbInit().Init(DbContext());
        _books = new BookRepository();
        _authors = new AuthorRepository();
        _orders = new OrderRepository();
        _categories = new CategoryRepository();
        _promotions = new PromotionRepository();
        _reviews = new ReviewRepository();
    }

}