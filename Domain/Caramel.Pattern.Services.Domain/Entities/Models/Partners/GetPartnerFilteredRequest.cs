using Caramel.Pattern.Services.Domain.Entities.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caramel.Pattern.Services.Domain.Entities.Models.Partners
{
    public class GetPartnerFilteredRequest
    {
        public Pagination Pagination { get; set; }
        public PartnerFilter PartnerFilter { get; set; }
    }
}
