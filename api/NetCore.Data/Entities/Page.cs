using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using NetCore.Data.Enums;
using NetCore.Data.Interfaces;
using NetCore.Infrastructure.SharedKernel;

namespace NetCore.Data.Entities
{
    [Table("Pages")]
    public class Page : DomainEntity<int>,ISwitchable
    {
        [Required]
        [MaxLength(256)]
        public string Name { set; get; }

        [MaxLength(256)]
        [Required]
        public string Alias { set; get; }

        public string Content { set; get; }
        public Status Status { set; get; }
    }
}
