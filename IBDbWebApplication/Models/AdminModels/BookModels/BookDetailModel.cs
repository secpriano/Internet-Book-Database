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
    public IEnumerable<ReviewModel> ReviewModels { get; set; } = new List<ReviewModel>();

    public BookDetailModel(BookModel bookModel, IEnumerable<ReviewModel> reviewModels) => 
        (BookModel, ReviewModels, ReviewModel, CommentModel) = (bookModel, reviewModels, new() { BookId = (long) bookModel.Id }, new() { BookId = (long) bookModel.Id });

    public BookDetailModel() { }
}