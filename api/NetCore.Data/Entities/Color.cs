using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using NetCore.Infrastructure.SharedKernel;

namespace NetCore.Data.Entities
{
    [Table("Colors")]
    public class Color : DomainEntity<int>
    {

        [StringLength(250)]
        public string Name
        {
            get; set;
        }

        [StringLength(250)]
        public string Code { get; set; }
    }
}
