using System;
using System.Linq;
using AutoMapper;
using sXb_service.Models;
using sXb_service.Models.ViewModels;

namespace sXb_service.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            CreateMap<CreateListingViewModel, Book>()
              .ForMember(dest => dest.Title, opts =>
               opts.MapFrom(src => src.Title))
              .ForMember(dest => dest.ISBN10, opts =>
               opts.MapFrom(src => src.ISBN10))
              .ForMember(dest => dest.ISBN13, opts =>
               opts.MapFrom(src => src.ISBN13))
              .ForMember(dest => dest.Authors, opts =>
               opts.MapFrom(src => src.Authors))
              .ForMember(dest => dest.Description, opts =>
               opts.MapFrom(src => src.Description));

            CreateMap<CreateListingViewModel, Listing>()
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
               opts.MapFrom(src => Enum.GetName(typeof(Condition), src.Condition)))
              .ForMember(dest => dest.ISBN10, opts =>
               opts.MapFrom(src => src.Book.ISBN10))
              .ForMember(dest => dest.ISBN13, opts =>
               opts.MapFrom(src => src.Book.ISBN13))
              .ForMember(dest => dest.Authors, opts =>
              opts.MapFrom(src => src.Book.Authors))
              .ForMember(dest => dest.Thumbnail, opts => opts.MapFrom(src => src.Book.Thumbnail));


            CreateMap<Listing, ListingDetailsViewModel>()
              .ForMember(dest => dest.Id, opts =>
               opts.MapFrom(src => src.Id))
              .ForMember(dest => dest.Title, opts =>
               opts.MapFrom(src => src.Book.Title))
              .ForMember(dest => dest.ISBN10, opts =>
               opts.MapFrom(src => src.Book.ISBN10))
              .ForMember(dest => dest.ISBN13, opts =>
               opts.MapFrom(src => src.Book.ISBN13))
              .ForMember(dest => dest.Description, opts =>
               opts.MapFrom(src => src.Book.Description))
              .ForMember(dest => dest.SmallThumbnail, opts =>
               opts.MapFrom(src => src.Book.SmallThumbnail))
              .ForMember(dest => dest.Thumbnail, opts =>
               opts.MapFrom(src => src.Book.Thumbnail))
              .ForMember(dest => dest.Authors, opts =>
               opts.MapFrom(src => src.Book.Authors))
              .ForMember(dest => dest.UserId, opts =>
               opts.MapFrom(src => src.UserId))
              .ForMember(dest => dest.Price, opts =>
               opts.MapFrom(src => src.Price))
              .ForMember(dest => dest.ContactOption, opts =>
               opts.MapFrom(src => src.ContactOption))
              .ForMember(dest => dest.Condition, opts =>
               opts.MapFrom(src => Enum.GetName(typeof(Condition), src.Condition)));

            CreateMap<VolumeInfo, BookApiResult>()
              .ForMember(dest => dest.Title, opts =>
               opts.MapFrom(src => src.Title))
              .ForMember(dest => dest.Description, opts =>
               opts.MapFrom(src => src.Description))
              .ForMember(dest => dest.ISBN10, opts =>
               opts.MapFrom(src => src.IndustryIdentifiers.Where(x => x.Type == "ISBN_10").Select(x => x.Identifier).FirstOrDefault()))
              .ForMember(dest => dest.ISBN13, opts =>
               opts.MapFrom(src => src.IndustryIdentifiers.Where(x => x.Type == "ISBN_13").Select(x => x.Identifier).FirstOrDefault()))
              .ForMember(dest => dest.SmallThumbnail, opts =>
               opts.MapFrom(src => src.ImageLinks.SmallThumbnail))
              .ForMember(dest => dest.Thumbnail, opts =>
               opts.MapFrom(src => src.ImageLinks.Thumbnail))
              .ForMember(dest => dest.Authors, opts =>
               opts.MapFrom(src => src.Authors));


            CreateMap<User, UserInfoViewModel>()
            .ForMember(dest => dest.Email, opts => opts.MapFrom(src => src.Email))
            .ForMember(dest => dest.UserName, opts => opts.MapFrom(src => src.UserName));

        }
    }
}