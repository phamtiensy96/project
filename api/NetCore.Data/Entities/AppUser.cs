using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using NetCore.Data.Interfaces;
using NetCore.Data.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCore.Data.Entities
{
    [Table("AppUsers")]
    public class AppUser : IdentityUser<Guid>, IDateTracking, ISwitchable
    {
        public AppUser() { }
        public AppUser(Guid id, string fullName, string userName,
            string email, string phoneNumber, string avatar, Status status,string address)
        {
            Id = id;
            FullName = fullName;
            UserName = userName;
            Email = email;
            PhoneNumber = phoneNumber;
            Avatar = avatar;
            Status = status;
            Address = address;
        }
        public string FullName { get; set; }

        public DateTime? BirthDay { set; get; }

        public string Address { get; set; }

        [Column(TypeName ="decimal(18,2)")]
        public decimal Balance { get; set; }

        public string Avatar { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public Status Status { get; set; }
    }
}
