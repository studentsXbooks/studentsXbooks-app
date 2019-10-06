using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using sXb_service.Models;
using sXb_service.Models.ViewModels;
using sXb_service.ViewModels;

namespace sXb_service.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Book, BookViewModel>();
            CreateMap<BookViewModel, Book>();
            CreateMap<ListingDetailsViewModel, Book>()
                .ForMember(dest => dest.Title, opts =>
                   opts.MapFrom(src => src.Title))
                .ForMember(dest => dest.ISBN10, opts =>
                   opts.MapFrom(src => src.ISBN10))
                .ForMember(dest => dest.Description, opts =>
                   opts.MapFrom(src => src.Description));
            CreateMap<ListingDetailsViewModel, Author>()
                .ForMember(dest => dest.FirstName, opts =>
                   opts.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opts =>
                   opts.MapFrom(src => src.LastName))
                .ForMember(dest => dest.MiddleName, opts =>
                   opts.MapFrom(src => src.MiddleName));
            CreateMap<ListingDetailsViewModel, Listing>()
                .ForMember(dest => dest.Price, opts =>
                   opts.MapFrom(src => src.Price))
                .ForMember(dest => dest.Condition, opts =>
                   opts.MapFrom(src => src.Condition))
                .ForMember(dest => dest.UserId, opts =>
                    opts.MapFrom(src => src.UserId));

            CreateMap<Listing, ListingPreviewViewModel>()
               .ForMember(dest => dest.Id, opts =>
                 opts.MapFrom(src => src.Id))
               .ForMember(dest => dest.Title, opts =>
                 opts.MapFrom(src => src.Book.Title))
               .ForMember(dest => dest.Price, opts =>
                 opts.MapFrom(src => src.Price))
               .ForMember(dest => dest.Condition, opts =>
                 opts.MapFrom(src => src.Condition))
                .ForMember(dest => dest.Authors, opts =>
                opts.MapFrom(src => src.Book.BookAuthors.Select(x => x.Author.FullName.ToString())));

            CreateMap<Listing, ListingDetailsViewModel>()
                .ForMember(dest => dest.Id, opts =>
                  opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opts =>
                  opts.MapFrom(src => src.Book.Title))
                .ForMember(dest => dest.ISBN10, opts =>
                  opts.MapFrom(src => src.Book.ISBN10))
                .ForMember(dest => dest.Description, opts =>
                  opts.MapFrom(src => src.Book.Description))
                .ForMember(dest => dest.FirstName, opts =>
                  opts.MapFrom(src => src.Book.BookAuthors.FirstOrDefault().Author.FirstName))
                .ForMember(dest => dest.LastName, opts =>
                  opts.MapFrom(src => src.Book.BookAuthors.FirstOrDefault().Author.LastName))
                .ForMember(dest => dest.MiddleName, opts =>
                  opts.MapFrom(src => src.Book.BookAuthors.FirstOrDefault().Author.MiddleName))
                .ForMember(dest => dest.UserId, opts =>
                  opts.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Price, opts =>
                  opts.MapFrom(src => src.Price))
                .ForMember(dest => dest.Condition, opts =>
                  opts.MapFrom(src => src.Condition));
            CreateMap<ListingViewModel, Listing>();
            //CreateMap<UserBook, UserBookViewModel>();
            //CreateMap<UserBookViewModel, UserBook>();
        }
    }
}