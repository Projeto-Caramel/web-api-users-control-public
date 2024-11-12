using Caramel.Pattern.Services.Domain.Enums.Parterns;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caramel.Pattern.Services.Domain.Entities.Models.Partners
{
    public class PartnerFilter
    {
        public string? Name { get; set; }
        public OrganizationType Type { get; set; }
    }
}
