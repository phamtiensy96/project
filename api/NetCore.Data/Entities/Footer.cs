using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using NetCore.Infrastructure.SharedKernel;

namespace NetCore.Data.Entities
{
    [Table("Footers")]
    public class Footer : DomainEntity<string>
    {

        [Required]
        public string Content { set; get; }
    }
}
