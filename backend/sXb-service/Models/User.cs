using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace sXb_service.Models
{
    [Table("Users", Schema = "sXb")]
    public class User : IdentityUser
    {

        [DataType(DataType.Text), MaxLength(128)]
        public string FirstName { get; set; }
        [DataType(DataType.Text), MaxLength(128)]
        public string LastName { get; set; }

        [DataType(DataType.Text)]
        public string PicturePath { get; set; }
    }
}
