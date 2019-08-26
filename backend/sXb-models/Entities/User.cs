using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace sXb_models.Entities
{
    [Table("Users", Schema = "sXb")]
    public class User : IdentityUser
    {
        [DataType(DataType.Text), MaxLength(255)]
        public string FirstName { get; set; }
        [DataType(DataType.Text), MaxLength(255)]
        public string LastName { get; set; }

        [DataType(DataType.Text), MaxLength(255)]
        public string Username { get; set; }

        [DataType(DataType.Text)]
        public string PicturePath { get; set; }
    }
}
