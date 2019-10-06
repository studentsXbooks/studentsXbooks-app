﻿using sXb_service.Helpers.ModelValidation;
using System;
using System.ComponentModel.DataAnnotations;

namespace sXb_service.Models.ViewModels
{
    public class ListingDetailsViewModel
    {
        public Guid Id { get; set; }

        [StringLength(265, MinimumLength = 1, ErrorMessage = "not working {2}")]
        public string Title { get; set; }

        [ISBNValidation(ISBNType = ISBNTypes.ISBN10)]
        public string ISBN10 { get; set; }

        [StringLength(300, MinimumLength = 1)]
        public string Description { get; set; }

        public string UserId { get; set; }

        [StringLength(25, MinimumLength = 1)]
        public string FirstName { get; set; }

        [StringLength(25, MinimumLength = 1)]
        public string LastName { get; set; }

        [StringLength(25, MinimumLength = 1)]
        public string MiddleName { get; set; }

        [RangeAttribute(typeof(decimal), "0", "9223372036854775807")]
        public decimal Price { get; set; }

        public Condition Condition { get; set; }
    }
}