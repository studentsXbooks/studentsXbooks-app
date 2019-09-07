using AutoMapper;
using sXb_service.Models;
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
            CreateMap<Listing, ListingViewModel>()
                .ForMember(dest => dest.Title, opts => 
                opts.MapFrom(src => src.UserBook.Book.Title));
            CreateMap<ListingViewModel, Listing>();
            CreateMap<UserBook, UserBookViewModel>();
            CreateMap<UserBookViewModel, UserBook>();
        }
    }
}
