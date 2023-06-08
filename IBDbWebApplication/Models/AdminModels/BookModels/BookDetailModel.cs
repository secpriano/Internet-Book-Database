using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using IBDbWebApplication.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace IBDbWebApplication.Models.AdminModels.BookModels;

public class BookDetailModel
{
    [BindProperty]
    public ReviewModel ReviewModel { get; set; }
    
    [BindProperty]
    public CommentModel CommentModel { get; set; }
    
    public BookModel BookModel { get; set; }
    public IEnumerable<ReviewViewModel> ReviewViewModels { get; set; } = new List<ReviewViewModel>();

    public BookDetailModel(BookModel bookModel, IEnumerable<ReviewViewModel> reviewViewModels) => 
        (BookModel, ReviewViewModels, ReviewModel, CommentModel) = (bookModel, reviewViewModels, new() { BookId = (long) bookModel.Id }, new() { BookId = (long) bookModel.Id });

    public BookDetailModel() { }
}