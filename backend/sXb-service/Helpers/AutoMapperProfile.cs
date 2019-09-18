using AutoMapper;
using sXb_service.Models;
using sXb_service.Models.ViewModels;
using sXb_service.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sXb_service.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Book, BookViewModel>();
            CreateMap<BookViewModel, Book>();
            //CreateMap<Listing, ListingDetailsViewModel>()
            //    .ForMember(dest => dest.Title, opts => 
            //    opts.MapFrom(src => src.UserBook.Book.Title))
            //    .ForMember(dest => dest.ISBN, opts =>
            //    opts.MapFrom(src => src.UserBook.Book.ISBN))
            //    .ForMember(dest => dest.ImageURL, opts =>
            //    opts.MapFrom(src => src.UserBook.Book.ImageURL))
            //    .ForMember(dest => dest.Author, opts =>
            //    opts.MapFrom(src => src.UserBook.Book.Author));
            CreateMap<Listing, ListingViewModel>();
            CreateMap<ListingViewModel, Listing>();
            //CreateMap<UserBook, UserBookViewModel>();
            //CreateMap<UserBookViewModel, UserBook>();
        }
    }
}
