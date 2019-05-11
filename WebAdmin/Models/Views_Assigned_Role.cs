using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdmin.Models
{
    [Table("Views_Assigned_Roles")]
    public class Views_Assigned_Role
    {
        [Key,Column(Order=0)]
        public string Userid { get; set; }

        [Key, Column(Order = 1)]
        public string RoleId { get; set; }

        public string Username { get; set; }

        public string Role { get; set; }
    }
}
