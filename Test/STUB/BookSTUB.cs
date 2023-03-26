﻿using Interface.DTO;
using Interface.Interfaces;

namespace Test.STUB;

public class BookSTUB : IBookData
{
    private static AuthorSTUB _authorStub = new();
    private static PublisherSTUB _publisherStub = new();
    private static GenreSTUB _genreStub = new();
    private static ThemeSTUB _themeStub = new();
    private static SettingSTUB _settingStub = new();
    
    public List<BookDTO> Books = new()
    {
        new(1, "2037162530194", "The Hobbit", "A hobbit is a small, furry creature that lives in a hole in the ground.", 
            new(1937, 9, 21), 310, _authorStub.Authors.FindAll(author => author is {Id: 1}),
            _publisherStub.Publishers.Find(publisher => publisher is {Id: 2}), 
            _genreStub.Genres.FindAll(genre => genre is {Id: 1} or {Id: 3}), 
            _themeStub.Themes.FindAll(theme => theme is {Id: 1} or {Id: 2}), 
            _settingStub.Settings.FindAll(setting => setting is {Id: 1} or {Id: 2})),
        new(2, "9780312866272", "The Wheel of Time", "The Wheel of Time is a series of high fantasy novels written by American author James Oliver Rigney Jr., under his pen name Robert Jordan. The series, set in the fictional world of the Two Rivers, revolves around a group of people who travel the world in order to combat the forces of the Dark One, who is trying to destroy the world and everything in it.", 
            new(1990, 1, 1), 800, _authorStub.Authors.FindAll(author => author is {Id: 1}),
            _publisherStub.Publishers.Find(publisher => publisher is {Id: 1}), 
            _genreStub.Genres.FindAll(genre => genre is {Id: 1} or {Id: 3}), 
            _themeStub.Themes.FindAll(theme => theme is {Id: 1} or {Id: 2}), 
            _settingStub.Settings.FindAll(setting => setting is {Id: 1} or {Id: 2}))
    };
    
    public bool Add(BookDTO entity)
    {
        Books.Add(entity);
        return Books.Exists(entity.Equals);
    }

    public BookDTO GetById(long id)
    {
        throw new NotImplementedException();
    }

    public BookDTO Update(BookDTO entity)
    {
        Books[(Index)(entity.Id - 1)!] = entity;
        return Books[(Index)(entity.Id - 1)!];
    }

    public bool Delete(long id)=> Books.Remove(Books.Find(book => book.Id == id));

    public IEnumerable<BookDTO> GetAll()
    {
        throw new NotImplementedException();
    }
}