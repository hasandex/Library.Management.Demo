using System;
using System.Collections.Generic;

namespace Library.Management.Demo.Models;

public partial class Book
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int AuthorId { get; set; }

    public int CategoryId { get; set; }

    public int PublisherId { get; set; }

    public DateTime? PublishedYear { get; set; }

    public int Quantity { get; set; }

    public virtual Author Author { get; set; } = null!;

    public virtual ICollection<BookEdition> BookEditions { get; set; } = new List<BookEdition>();

    public virtual ICollection<BookLibrary> BookLibraries { get; set; } = new List<BookLibrary>();

    public virtual Category Category { get; set; } = null!;

    public virtual Publisher Publisher { get; set; } = null!;

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
