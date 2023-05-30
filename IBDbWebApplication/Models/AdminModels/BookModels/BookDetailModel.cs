﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using IBDbWebApplication.Models.Entity;

namespace IBDbWebApplication.Models.AdminModels.BookModels;

public class BookDetailModel
{
    [Key]
    public long BookId { get; set; }
    
    [DisplayName("Title")]
    [Required(ErrorMessage = "A title is required")]
    [StringLength(100, ErrorMessage = "Content must be more than 1 character long and less than 100 characters", MinimumLength = 1)]
    public string Title { get; set; }
    
    [DisplayName("Content")]
    [Required(ErrorMessage = "Content is required")]
    [StringLength(2000, ErrorMessage = "Content must be more than 2 characters long and less than 2000 characters", MinimumLength = 2 )]
    public string Content { get; set; }

    public BookModel? BookModel { get; set; } = null;
    public IEnumerable<ReviewModel> ReviewModels { get; set; } = new List<ReviewModel>();

    public BookDetailModel(BookModel bookModel, IEnumerable<ReviewModel> reviewModels) =>
        (BookId, BookModel, ReviewModels) = ((long)bookModel.Id, bookModel, reviewModels);

    public BookDetailModel()
    {
        
    }
}