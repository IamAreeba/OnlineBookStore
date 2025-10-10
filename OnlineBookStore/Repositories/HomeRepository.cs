
using Microsoft.EntityFrameworkCore;

namespace OnlineBookStore.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _db;

        public HomeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Genre>> Genres()
        {
            return await _db.Genres.ToListAsync();
        }



        public async Task<IEnumerable<Book>> GetBooks(string sTerm="", int genreId = 0)
        {
            sTerm = sTerm.ToLower();
            IEnumerable<Book> books = await (from book in _db.Books
                         join genre in _db.Genres
                         on book.GenreId equals genre.Id
                         where string.IsNullOrWhiteSpace(sTerm) || (book != null && book.BookName.ToLower().Contains(sTerm))
                         select new Book
                         {
                             Id = book.Id,
                             BookName = book.BookName,
                             AuthorName = book.AuthorName,
                             Price = book.Price,
                             Image = book.Image,
                             GenreName = genre.GenreName,
                             GenreId = book.GenreId,
                         }
                         ).ToListAsync(); 

            if(genreId > 0)
            {
                books = books.Where(a => a.GenreId == genreId).ToList();
            }
            return books;
        }
    }
}
