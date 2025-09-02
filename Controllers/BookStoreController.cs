using Microsoft.AspNetCore.Mvc;
using industri.Model;

namespace industri.Controllers;

[ApiController]
[Route("api/books")]
public class BookController : ControllerBase
{
    private readonly ILogger<BookController> _logger;

    // Simulasi database in-memory
    private static List<Book> Books = new List<Book>
    {
        new Book { Id = 1, Title = "Laskar Pelangi", Author = "Andrea Hirata", Price = 75000, Genre = "Fiksi" },
        new Book { Id = 2, Title = "Bumi Manusia", Author = "Pramoedya Ananta Toer", Price = 90000, Genre = "Sejarah" },
        new Book { Id = 3, Title = "Belajar C#", Author = "John Doe", Price = 120000, Genre = "Teknologi" }
    };

    public BookController(ILogger<BookController> logger)
    {
        _logger = logger;
    }

    // GET: api/books
    [HttpGet]
    public ActionResult<IEnumerable<Book>> GetAll()
    {
        return Ok(new
        {
            message = "Daftar buku",
            data = Books
        });
    }

    // GET: api/books/{id}
    [HttpGet("{id}")]
    public ActionResult<Book> GetById(int id)
    {
        var book = Books.FirstOrDefault(b => b.Id == id);
        if (book == null) return NotFound(new { message = "Buku tidak ditemukan" });
        return Ok(new
        {
            message = "Buku ditemukan",
            data = book
        });
    }

    // POST: api/books
    [HttpPost]
    public ActionResult<Book> Create(Book newBook)
    {
        newBook.Id = Books.Count > 0 ? Books.Max(b => b.Id) + 1 : 1;
        Books.Add(newBook);

        return CreatedAtAction(nameof(GetById), new { id = newBook.Id }, newBook);
    }

    // PUT: api/books/{id}
    [HttpPut("{id}")]
    public ActionResult<Book> Update(int id, Book updatedBook)
    {
        var book = Books.FirstOrDefault(b => b.Id == id);
        if (book == null) return NotFound(new { message = "Buku tidak ditemukan" });

        // merge manual
        if (!string.IsNullOrEmpty(updatedBook.Title)) book.Title = updatedBook.Title;
        if (!string.IsNullOrEmpty(updatedBook.Author)) book.Author = updatedBook.Author;
        if (updatedBook.Price > 0) book.Price = updatedBook.Price;
        if (!string.IsNullOrEmpty(updatedBook.Genre)) book.Genre = updatedBook.Genre;

        return Ok(new
        {
            message = "Buku berhasil diperbarui",
            data = book
        });
    }

    // DELETE: api/books/{id}
    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var book = Books.FirstOrDefault(b => b.Id == id);
        if (book == null) return NotFound(new { message = "Buku tidak ditemukan" });

        Books.Remove(book);
        return Ok(new { message = "Buku berhasil dibuang" });
    }
}


