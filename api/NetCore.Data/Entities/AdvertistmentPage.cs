using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using NetCore.Infrastructure.SharedKernel;

namespace NetCore.Data.Entities
{
    [Table("AdvertistmentPages")]
    public class AdvertistmentPage : DomainEntity<string>
    {
        public string Name { get; set; }

        public virtual ICollection<AdvertistmentPosition> AdvertistmentPositions { get; set; }
    }
}
