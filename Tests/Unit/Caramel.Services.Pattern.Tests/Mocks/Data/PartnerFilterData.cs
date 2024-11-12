using Caramel.Pattern.Services.Domain.Entities.Models.Partners;
using Caramel.Pattern.Services.Domain.Enums.Parterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caramel.Services.Pattern.Tests.Mocks.Data
{
    public class PartnerFilterData
    {
        public static Dictionary<string, PartnerFilter> Data = new Dictionary<string, PartnerFilter>
        {
            {
                "Basic", new PartnerFilter()
                {
                    Name = "Ong Teste",
                    Type = OrganizationType.ONG
                }
            }
        };
    }
}
