using Caramel.Pattern.Services.Domain.Entities.DTOs.Request;
using Caramel.Pattern.Services.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Caramel.Pattern.Services.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        public BaseController() { }

        public IEnumerable<TEntity> ReturnPaginated<TEntity>(IEnumerable<TEntity> collection, Pagination pagination)
        {
            if (pagination.Page == 0) pagination.Page = 1;
            if (pagination.Size == 0) pagination.Size = 10;

            return collection
                .Skip((pagination.Page - 1) * pagination.Size)
                .Take(pagination.Size)
                .ToList();
        }
    }
}
