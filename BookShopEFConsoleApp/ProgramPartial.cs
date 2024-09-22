using BookShopEFConsoleApp.Helpers;
using BookShopEFConsoleApp.Models;
using BookShopEFConsoleApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopEFConsoleApp;

public partial class Program
{
    //Authors
    static async Task ReviewAuthors()
    {
        var allAuthors = await _authors.GetAllAuthorsAsync();
        var authors = allAuthors.Select(e => new ItemView { Id = e.Id, Value = e.Name }).ToList();
        int result = ItemsHelper.MultipleChoice(true, authors, true);
        if (result != 0)
        {
            var currentAuthor = await _authors.GetAuthorAsync(result);
            await AuthorInfo(currentAuthor);
        }
    }
    static async Task AuthorInfo(Author currentAuthor)
    {
        int result = ItemsHelper.MultipleChoice(true, new List<ItemView>
            {
             new ItemView { Id = 1, Value = "Browse books"},
             new ItemView { Id = 2, Value = "Edit author"},
             new ItemView { Id = 3, Value = "Delete author"},
            },
            IsMenu: true, message: String.Format("{0}\n", currentAuthor), startY: 5, optionsPerLine: 1);

        switch (result)
        {
            case 1:
                {
                    await BrowseAuthorBooks(currentAuthor);
                    break;
                }
            case 2:
                {
                    await EditAuthor(currentAuthor);
                    Console.ReadLine();
                    break;
                }
            case 3:
                {
                    await RemoveAuthor(currentAuthor);
                    Console.ReadLine();
                    break;
                }
        }
        await ReviewAuthors();
    }
    static async Task AddAuthor()
    {
        string authorName = InputHelper.GetString("author 'Name' with 'Surname'");
        await _authors.AddAuthorAsync(new Author
        {
            Name = authorName
        });
        Console.WriteLine("Author successfully added.");
    }
    static async Task EditAuthor(Author currentAuthor)
    {
        Console.WriteLine("Changing: {0}", currentAuthor.Name);
        currentAuthor.Name = InputHelper.GetString("author 'Name' with 'SurName'");
        await _authors.EditAuthorAsync(currentAuthor);
        Console.WriteLine("Author successfully changed.");
    }
    static async Task RemoveAuthor(Author currentAuthor)
    {
        int result = ItemsHelper.MultipleChoice(true, new List<ItemView>
            {
             new ItemView { Id = 1, Value = "Yes"},
             new ItemView { Id = 0, Value = "No"},
            }, message: String.Format("[Are you sure you want to delete the author {0} ?]\n", currentAuthor.Name), startY: 2);

        if (result == 1)
        {
            await _authors.DeleteAuthorAsync(currentAuthor);
            Console.WriteLine("The author has been successfully deleted.");
        }
        else
        {
            Console.WriteLine("Press any key to continue...");
        }
    }
    static async Task SearchAuthors()
    {
        string authorName = InputHelper.GetString("author name or surname");
        var currentAuthors = await _authors.GetAuthorsByNameAsync(authorName);
        if (currentAuthors.Count() > 0)
        {
            var authors = currentAuthors.Select(e => new ItemView { Id = e.Id, Value = e.Name }).ToList();
            int result = ItemsHelper.MultipleChoice(true, authors, true);
            if (result != 0)
            {
                var currentAuthor = await _authors.GetAuthorAsync(result);
                await AuthorInfo(currentAuthor);
            }
        }
        else
        {
            Console.WriteLine("No authors were found by this attribute.");
        }
    }
    static async Task BrowseAuthorBooks(Author currentAuthor)
    {
        var authorWithBooks = await _authors.GetAuthorWhithBooksAsync(currentAuthor.Id);
        if (authorWithBooks is not null)
        {
            if (authorWithBooks.Books.Count > 0)
            {
                await ReviewBooks(authorWithBooks.Books);
            }
            else
            {
                Console.Clear();
                Console.WriteLine("No books by the specified author were found.");
            }
        }
        else
        {
            Console.WriteLine("Author not found.");
        }
    }


    //Reviews
    static async Task ReviewReviews(int currentBookId)
    {
        var allReviews = await _reviews.GetAllReviewsAsync(currentBookId);
        var reviews = allReviews.Select(e => new ItemView { Id = e.Id, Value = e.Comment }).ToList();
        int result = ItemsHelper.MultipleChoice(true, reviews, true);
        if (result != 0)
        {
            var currentReview = await _reviews.GetReviewAsync(result);
            await ReviewInfo(currentReview);
        }
    }
    static async Task ReviewInfo(Review currentReview)
    {
        int result = ItemsHelper.MultipleChoice(true, new List<ItemView>
            {
             new ItemView { Id = 1, Value = "Update review"},
             new ItemView { Id = 2, Value = "Delete review"},
            },
            IsMenu: true, message: String.Format("{0}\n", currentReview), startY: 8, optionsPerLine: 1);
    
        switch (result)
        { 
            case 1:
            {
                await UpdateReview(currentReview);
                Console.ReadLine();
                break;
            }
            case 2:
            {
                await RemoveReview(currentReview);
                Console.ReadLine();
                break;
            }
        }
        await ReviewReviews(currentReview.BookId);
    }
    static async Task UpdateReview(Review currentReview)
    {
        currentReview.Comment = InputHelper.GetString("book review");
        currentReview.Stars = (byte)InputHelper.GetInt("stars");
        await _reviews.EditReviewAsync(currentReview);
        Console.WriteLine("Review successfully updated.");    
    }
    static async Task AddReview(int currentBookId)
    {
        Book book = await _books.GetBookAsync(currentBookId);
        if (book is not null)
        {
            string userName = InputHelper.GetString("author 'Name'");
            string userEmail = InputHelper.GetString("author email");
            string comment = InputHelper.GetString("book review");
            byte stars = (byte)InputHelper.GetInt("stars");
            await _reviews.AddReviewAsync(new Review
            {
                UserName = userName,
                UserEmail = userEmail,
                Comment = comment,
                Stars = stars,
                BookId = currentBookId
            
            });
            Console.WriteLine("Review successfully added.");    
        }
        else
        {
            Console.WriteLine("Book not found."); 
        }
        
    }
    static async Task RemoveReview(Review currentReview)
    {
        int result = ItemsHelper.MultipleChoice(true, new List<ItemView>
        {
            new ItemView { Id = 1, Value = "Yes"},
            new ItemView { Id = 0, Value = "No"},
        }, message: String.Format("[Are you sure you want to delete the review {0} ?]\n", currentReview.Comment), startY: 2);

        if (result == 1)
        {
            await _reviews.DeleteReviewAsync(currentReview);
            Console.WriteLine("The review has been successfully deleted.");
        }
        else
        {
            Console.WriteLine("Press any key to continue...");
        }
    }

    //Categories
    static async Task ReviewCategories()
    {
        var allCategories = await _categories.GetAllCategoriesAsync();
        var categories = allCategories.Select(e => new ItemView { Id = e.Id, Value = e.Name }).ToList();
        int result = ItemsHelper.MultipleChoice(true, categories, true);
        if (result != 0)
        {
            var currentCategory = await _categories.GetCategoryAsync(result);
            await CategoryInfo(currentCategory);
        }
    }
    static async Task CategoryInfo(Category currentCategory)
    {
        int result = ItemsHelper.MultipleChoice(true, new List<ItemView>
            {
             new ItemView { Id = 1, Value = "Browse books"},
             new ItemView { Id = 2, Value = "Edit category"},
             new ItemView { Id = 3, Value = "Delete category"},
            },
            IsMenu: true, message: String.Format("{0}\n", currentCategory), startY: 5, optionsPerLine: 1);

        switch (result)
        {
            case 1:
                {
                    await BrowseCategoryBooks(currentCategory);
                    break;
                }
            case 2:
                {
                    await EditCategory(currentCategory);
                    Console.ReadLine();
                    break;
                }
            case 3:
                {
                    await RemoveCategory(currentCategory);
                    Console.ReadLine();
                    break;
                }
        }
        await ReviewCategories();
    }
    static async Task AddCategory()
    {
        string categoryName = InputHelper.GetString("category 'Name'");
        string categoryDesc = InputHelper.GetString("category 'Description'");
        await _categories.AddCategoryAsync(new Category
        {
            Name = categoryName,
            Description = categoryDesc
        });
        Console.WriteLine("Category successfully added.");
    }
    static async Task EditCategory(Category currentCategory)
    {
        Console.WriteLine("Changing: {0}", currentCategory.Name);
        currentCategory.Name = InputHelper.GetString("category name");
        currentCategory.Description = InputHelper.GetString("category description");
        await _categories.UpdateCategoryAsync(currentCategory);
        Console.WriteLine("Category successfully changed.\r\n");
    }
    static async Task RemoveCategory(Category currentCategory)
    {
        int result = ItemsHelper.MultipleChoice(true, new List<ItemView>
            {
             new ItemView { Id = 1, Value = "Yes"},
             new ItemView { Id = 0, Value = "No"},
            }, message: String.Format("[Are you sure you want to delete the category {0} ?]\n", currentCategory.Name), startY: 2);

        if (result == 1)
        {
            await _categories.DeleteCategoryAsync(currentCategory);
            Console.WriteLine("The category has been successfully deleted.");
        }
        else
        {
            Console.WriteLine("Press any key to continue...");
        }
    }
    static async Task SearchCategories()
    {
        string categoryName = InputHelper.GetString("category name");
        var currentCategories = await _categories.GetCategoriesByNameAsync(categoryName);
        if (currentCategories.Count() > 0)
        {
            var categories = currentCategories.Select(e => new ItemView { Id = e.Id, Value = e.Name }).ToList();
            int result = ItemsHelper.MultipleChoice(true, categories, true);
            if (result != 0)
            {
                var currentCategory = await _categories.GetCategoryAsync(result);
                await CategoryInfo(currentCategory);
            }
        }
        else
        {
            Console.WriteLine("No categories were found by this attribute.");
        }
    }
    static async Task BrowseCategoryBooks(Category currentCategory)
    {
        var categoryWithBooks = await _categories.GetCategoryWithBooksAsync(currentCategory.Id);
        if (categoryWithBooks is not null)
        {
            if (categoryWithBooks.Books.Count > 0)
            {
                await ReviewBooks(categoryWithBooks.Books);
            }
            else
            {
                Console.Clear();
                Console.WriteLine("No books by the specified category were found.");
            }
        }
        else
        {
            Console.WriteLine("Category not found.");
        }
    }


    //Books
    static async Task ReviewBooks(ICollection<Book>? authorBooks = null)
    {
        var allBooks = authorBooks is null ? await _books.GetAllBooksAsync() : authorBooks;
        var books = allBooks.Select(e => new ItemView { Id = e.Id, Value = e.Title }).ToList();
        int result = ItemsHelper.MultipleChoice(true, books, true);
        if (result != 0)
        {
            var currentBook = await _books.GetBookWithCategoryAndAuthorsAsync(result);
            await BookInfo(currentBook);
        }
    }
    static async Task BookInfo(Book currentBook)
    {
        int result = ItemsHelper.MultipleChoice(true, new List<ItemView>
            {
             new ItemView { Id = 1, Value = "Browse authors"},
             new ItemView { Id = 2, Value = "Edit book"},
             new ItemView { Id = 3, Value = "Delete book"},
             new ItemView { Id = 4, Value = "Promotions Info"},
             new ItemView { Id = 5, Value = "Reviews Info"},
             new ItemView { Id = 6, Value = "Add review"}
            },
            IsMenu: true, message: String.Format("[{0}]\n", currentBook), startY: 10, optionsPerLine: 1);

        switch (result)
        {
            case 1:
                {
                    await BrowseAuthors(currentBook);
                    break;
                }
            case 2:
                {
                    await EditBook(currentBook);
                    Console.ReadLine();
                    break;
                }
            case 3:
                {
                    await RemoveBook(currentBook);
                    Console.ReadLine();
                    break;
                }
            case 4:
                {
                    await PromotionInfo(currentBook);
                    Console.ReadLine();
                    break;
                }
            case 5:
                {
                    await ReviewReviews(currentBook.Id);
                    break;
                }
            case 6:
            {
                await AddReview(currentBook.Id);
                break;
            }
        }
        await ReviewBooks();
    }
    static async Task<Book> CreateOrUpdateBook(Book? currentBook = null)
    {
        if (currentBook is not null)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("When editing, all fields are changed, that's the will of the author of the program (or his laziness)..\n");
            Console.ResetColor();
        }
        var allCategories = await _categories.GetAllCategoriesAsync();
        var allAuthors = await _authors.GetAllAuthorsAsync();

        string bookName = InputHelper.GetString("book 'Name'");
        string bookDesc = InputHelper.GetString("book 'Description'");
        DateOnly bookDate = InputHelper.GetDate("book 'Published on'");
        string bookPublisher = InputHelper.GetString("book 'Publisher'");
        decimal bookPrice = InputHelper.GetDecimal("book 'Price'");

        //Сохраняем историю ввода, так как
        //экран при выборе постоянно очищается
        string buffer = String.Format("Enter book 'Name': {0}\nEnter book 'Description': {1}\nEnter book 'Published on': {2}\n" +
            "Enter book 'Publisher': {3}\nEnter book 'Price': {3}\nChoose 'Category':", bookName,
            bookDesc, bookDate, bookPublisher, bookPrice);

        var categories = allCategories.Select(e => new ItemView { Id = e.Id, Value = e.Name }).ToList();
        int categoryId = ItemsHelper.MultipleChoice(true, categories, message: buffer, startY: 10);

        buffer += String.Format("Category with id: {0}\nChoose author:", categoryId);
        var authors = allAuthors.Select(e => new ItemView { Id = e.Id, Value = e.Name }).ToList();
        List<Author> selectedAuthors = new List<Author>(1);
        int authorId = -1;
        while (authorId != 0)
        {
            //Передаем новую коллекцию, чтобы не менять основную
            authorId = ItemsHelper.MultipleChoice(true, new List<ItemView>(authors), true, message: buffer, startY: 10);
            if (authorId != 0)
            {
                selectedAuthors.Add(new Author { Id = authorId });
                authors = authors.Where(e => e.Id != authorId).ToList();
            }
        }
        return new Book
        {
            Id = currentBook?.Id ?? 0,
            Title = bookName,
            Description = bookDesc,
            PublishedOn = bookDate.ToDateTime(TimeOnly.Parse("0:0")),
            Publisher = bookPublisher,
            Price = bookPrice,
            CategoryId = categoryId,
            Authors = selectedAuthors
        };
    }
    static async Task AddBook()
    {
        await _books.AddBookAsync(await CreateOrUpdateBook());
        Console.WriteLine("Book successfully added.");
    }
    static async Task EditBook(Book currentBook)
    {
        await _books.EditBookAsync(await CreateOrUpdateBook(currentBook));
        Console.WriteLine("Book successfully changed.");
    }
    static async Task RemoveBook(Book currentBook)
    {
        int result = ItemsHelper.MultipleChoice(true, new List<ItemView>
            {
             new ItemView { Id = 1, Value = "Yes"},
             new ItemView { Id = 0, Value = "No"},
            }, message: String.Format("[Are you sure you want to delete this book {0} ?]\n", currentBook.Title), startY: 2);

        if (result == 1)
        {
            await _books.DeleteBookAsync(currentBook);
            Console.WriteLine("The book has been successfully deleted.");
        }
        else
        {
            Console.WriteLine("Press any key to continue...");
        }
    }
    static async Task BrowseAuthors(Book currentBook)
    {
        var authors = currentBook.Authors.Select(e => new ItemView { Id = e.Id, Value = e.Name }).ToList();
        if (authors.Count > 0)
        {
            int result = ItemsHelper.MultipleChoice(true, authors, true);
            if (result != 0)
            {
                var currentAuthor = await _authors.GetAuthorAsync(result);
                await AuthorInfo(currentAuthor);
            }
        }
        else
        {
            Console.WriteLine("This book has no added authors.");
            Console.ReadLine();
        }
    }
    static async Task SearchBooks()
    {
        string bookName = InputHelper.GetString("book title");
        var currentBooks = await _books.GetBooksByNameAsync(bookName);
        if (currentBooks.Count() > 0)
        {
            var books = currentBooks.Select(e => new ItemView
            {
                Id = e.Id,
                Value = e.Title
            }).ToList();

            int result = ItemsHelper.MultipleChoice(true, books, true);
            if (result != 0)
            {
                await BookInfo(await _books.GetBookAsync(result));
            }
        }
        else
        {
            Console.WriteLine("No books were found by this attribute.");
        }
    }


    //Promotions
    static async Task PromotionInfo(Book currentBook)
    {
        var bookWithPromotion = await _books.GetBookWithPromotionAsync(currentBook.Id);
        var menuList = new List<ItemView>(1);
        if (bookWithPromotion.Promotion is null)
        {
            menuList.Add(new ItemView { Id = 1, Value = "Add promotion" });
        }
        else
        {
            menuList.Add(new ItemView { Id = 2, Value = "Edit promotion" });
            menuList.Add(new ItemView { Id = 3, Value = "Remove promotion" });
        }
        int result = ItemsHelper.MultipleChoice(true, menuList,
            IsMenu: true, message: String.Format("{0}\n\nPromotion: {1}", currentBook, bookWithPromotion.Promotion), startY: 10, optionsPerLine: 1);

        switch (result)
        {
            case 1:
                {
                    await AddPromotion(currentBook);
                    Console.ReadLine();
                    break;
                }
            case 2:
                {
                    await EditPromotion(bookWithPromotion);
                    Console.ReadLine();
                    break;
                }
            case 3:
                {
                    await RemovePromotion(bookWithPromotion.Promotion!);
                    Console.ReadLine();
                    break;
                }
        }
        await BookInfo(currentBook);
    }
    static async Task CreateOrUpdatePromotion(Book currentBook)
    {
        bool isNew = currentBook.Promotion is null;
        string promotionTitle = InputHelper.GetString("promotion 'Title'");
        int result = ItemsHelper.MultipleChoice(true, new List<ItemView>()
        {
        new ItemView{ Id = 1, Value = "Percentage of cost"},
        new ItemView{ Id = 2, Value = "The amount of cost"},
        }, true, message: "Wath is the format of the discount?", startY: 10, optionsPerLine: 1);

        decimal value = InputHelper.GetDecimal(result == 1 ? "percent" : "amount");
        Promotion promotion = new Promotion
        {
            Id = currentBook?.Promotion?.Id ?? 0,
            Name = promotionTitle,
            BookId = currentBook.Id
        };
        if (result == 1)
        {
            promotion.Percent = value;
        }
        else
        {
            promotion.Amount = value;
        }
        if (isNew)
        {
            await _promotions.AddPromotionAsync(promotion);
        }
        else
        {
            await _promotions.EditPromotionAsync(promotion);
        }
    }
    static async Task AddPromotion(Book currentBook)
    {
        await CreateOrUpdatePromotion(currentBook);
        Console.WriteLine("Promotion successfully added.");
    }
    static async Task RemovePromotion(Promotion currentPromotion)
    {
        int result = ItemsHelper.MultipleChoice(true, new List<ItemView>
            {
             new ItemView { Id = 1, Value = "Yes"},
             new ItemView { Id = 0, Value = "No"},
            }, message: String.Format("[Are you sure you want to delete this promotion {0} ?]\n", currentPromotion), startY: 2);

        if (result == 1)
        {
            await _promotions.DeletePromotionAsync(currentPromotion);
            Console.WriteLine("The promotion has been successfully deleted.");
        }
        else
        {
            Console.WriteLine("Press any key to continue...");
        }
    }
    static async Task EditPromotion(Book currentBook)
    {
        await CreateOrUpdatePromotion(currentBook);
        Console.WriteLine("Promotion successfully changed.");
    }
    
    
    //Orders
    static async Task ReviewOrders()
    {
        var allOrders = await _orders.GetAllOrdersAsync();
        var orders = allOrders.Select(e => new ItemView
        {
            Id = e.Id,
            Value = String.Format("{0} ({1}, {2})",
            e.CustomerName, e.City, e.Address)
        }).ToList();
        int result = ItemsHelper.MultipleChoice(true, orders, true, optionsPerLine: 1);
        if (result != 0)
        {
            var currentOrder = await _orders.GetOrderWithOrderLinesAndBooksAsync(result);
            await OrderInfo(currentOrder);
        }
    }
    static async Task OrderInfo(Order currentOrder)
    {
        List<ItemView> items = new List<ItemView>
            {
             new ItemView { Id = 1, Value = "Browse books"},
             new ItemView { Id = 2, Value = "Edit order"},
             new ItemView { Id = 3, Value = "Delete order"}
            };
        if (currentOrder.Shipped == false)
        {
            items.Add(new ItemView { Id = 4, Value = "Close order" });
        }
        decimal totalSum = currentOrder.Lines.Sum(e => e.Book.Price);
        decimal totalSumWithDiscounts = totalSum;
        foreach (Book book in currentOrder.Lines.Select(e => e.Book))
        {
            if (book.Promotion is not null)
            {
                if (book.Promotion.Percent is not null)
                {
                    totalSumWithDiscounts -= book.Price * book.Promotion.Percent.Value / 100;
                }
                else
                {
                    totalSumWithDiscounts -= book.Promotion.Amount ?? 0;
                }
            }
        }
        int result = ItemsHelper.MultipleChoice(true, items,
            IsMenu: true, message: String.Format("{0}\n\nTotal cost: {1}\nCost including discounts: {2}",
            currentOrder, totalSum, totalSumWithDiscounts), startY: 8, optionsPerLine: 1);

        switch (result)
        {
            case 1:
                {
                    await BrowseBooks(currentOrder);
                    break;
                }
            case 2:
                {
                    await EditOrder(currentOrder);
                    Console.ReadLine();
                    break;
                }
            case 3:
                {
                    await RemoveOrder(currentOrder);
                    Console.ReadLine();
                    break;
                }
            case 4:
                {
                    await CloseOrder(currentOrder);
                    Console.ReadLine();
                    break;
                }
        }
        await ReviewOrders();
    }
    static async Task BrowseBooks(Order currentOrder)
    {
        var books = currentOrder.Lines.Select(e => new ItemView { Id = e.Book.Id, Value = e.Book.Title + $"(Count: {e.Quantity})" }).ToList();
        if (books.Count > 0)
        {
            int result = ItemsHelper.MultipleChoice(true, books, true, optionsPerLine: 1);
            if (result != 0)
            {
                var currentBook = await _books.GetBookWithCategoryAndAuthorsAsync(result);
                await BookInfo(currentBook);
            }
        }
        else
        {
            Console.WriteLine("This order has no added books.");
            Console.ReadLine();
        }
    }
    static async Task EditOrder(Order currentOrder)
    {
        await _orders.UpdateOrderAsync(await CreateOrUpdateOrder(currentOrder));
        Console.WriteLine("Order successfully changed.");
    }
    static async Task AddOrder()
    {
        await _orders.AddOrderAsync(await CreateOrUpdateOrder());
        Console.WriteLine("Order successfully added.");
    }
    static async Task<Order> CreateOrUpdateOrder(Order? currentOrder = null)
    {
        if (currentOrder is not null)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("When editing, all fields are changed, that's the will of the author of the program (or his laziness)..\n");
            Console.ResetColor();
        }

        var allBooks = await _books.GetAllBooksAsync();

        string customerName = InputHelper.GetString("customer 'Name'");
        string customerCity = InputHelper.GetString("customer 'City'");
        string customerAddress = InputHelper.GetString("customer 'Address'");
        Order order = new Order
        {
            Id = currentOrder?.Id ?? 0,
            CustomerName = customerName,
            City = customerCity,
            Address = customerAddress
        };
        //Сохраняем историю ввода, так как
        //экран при выборе постоянно очищается
        string buffer = String.Format("Enter customer 'Name': {0}\nEnter customer 'City': {1}\nEnter customer 'Address: {2}" +
            "\nSelected books:\n", customerName, customerCity, customerAddress);
        var books = allBooks.Select(e => new ItemView { Id = e.Id, Value = e.Title }).ToList();
        Dictionary<int, (string name, int count)> bufferBooks = new Dictionary<int, (string, int)>();
        int bookId = -1, startY = 5;
        while (bookId != 0)
        {
            //Передаем новую коллекцию, чтобы не менять основную
            bookId = ItemsHelper.MultipleChoice(true, new List<ItemView>(books), true,
                message: buffer + String.Join("\n", bufferBooks.Select(e => e.Value)), startY: startY++);
            if (bookId != 0)
            {
                if (bufferBooks.ContainsKey(bookId))
                {
                    bufferBooks[bookId] = (bufferBooks[bookId].name, bufferBooks[bookId].count + 1);
                }
                else
                {
                    bufferBooks.Add(bookId, (name: allBooks.FirstOrDefault(e => e.Id == bookId)!.Title, count: 1));
                }
            }
        }
        order.Lines = bufferBooks.Select(e => new OrderLine
        {
            BookId = e.Key,
            Quantity = e.Value.count
        }).ToList();
        return order;
    }
    static async Task RemoveOrder(Order currentOrder)
    {
        int result = ItemsHelper.MultipleChoice(true, new List<ItemView>
            {
             new ItemView { Id = 1, Value = "Yes"},
             new ItemView { Id = 0, Value = "No"},
            }, message: String.Format("[Are you sure you want to delete this order {0} ?]\n", currentOrder.CustomerName), startY: 2);

        if (result == 1)
        {
            await _orders.DeleteOrderAsync(currentOrder);
            Console.WriteLine("The order has been successfully deleted.");
        }
        else
        {
            Console.WriteLine("Press any key to continue...");
        }
    }
    static async Task CloseOrder(Order currentOrder)
    {
        int result = ItemsHelper.MultipleChoice(true, new List<ItemView>
            {
             new ItemView { Id = 1, Value = "Yes"},
             new ItemView { Id = 0, Value = "No"},
            }, message: String.Format("[Has this order been delivered? \nUpdate the status for the order as 'Completed'? {0} ?]\n",
            currentOrder.CustomerName), startY: 3);

        if (result == 1)
        {
            currentOrder.Shipped = true;
            await _orders.UpdateOrderAsync(currentOrder);
            Console.WriteLine("The order was successfully completed!");
        }
        else
        {
            Console.WriteLine("Press any key to continue...");
        }
    }
    static async Task SearchOrders()
    {
        int result = ItemsHelper.MultipleChoice(true, new List<ItemView>
            {
             new ItemView { Id = 1, Value = "Customer 'Address'"},
             new ItemView { Id = 2, Value = "Сustomer 'Name'"},
            }, message: String.Format("Select the data for which to start searching for orders?"), startY: 2, optionsPerLine: 1, IsMenu: true);
        if (result == 1)
        {
            string address = InputHelper.GetString("customer 'Address'");
            var currentOrders = await _orders.GetAllOrdersByAddressAsync(address);
            if (currentOrders.Count() > 0)
            {
                await Search(currentOrders);
            }
            else
            {
                Console.WriteLine("Orders with this address were not found.");
            }
        }
        else if (result == 2)
        {
            string name = InputHelper.GetString("customer 'Name'");
            var currentOrders = await _orders.GetAllOrdersByNameAsync(name);
            if (currentOrders.Count() > 0)
            {
                await Search(currentOrders);
            }
            else
            {
                Console.WriteLine("Orders with this name were not found.");
            }
        }
    }
    static async Task Search(IEnumerable<Order> currentOrders)
    {
        var orders = currentOrders.Select(e => new ItemView { Id = e.Id, Value = e.CustomerName }).ToList();
        int result = ItemsHelper.MultipleChoice(true, orders, true);
        if (result != 0)
        {
            var currentOrder = await _orders.GetOrderWithOrderLinesAndBooksAsync(result);
            await OrderInfo(currentOrder);
        }
    }

}
