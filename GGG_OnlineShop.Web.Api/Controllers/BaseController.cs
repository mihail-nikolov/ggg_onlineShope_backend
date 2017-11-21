namespace GGG_OnlineShop.Web.Api.Controllers
{
    using AutoMapper;
    using Infrastructure;
    using System.Web.Http;

    public abstract class BaseController : ApiController
    {
        //public ICacheService Cache { get; set; }

        protected IMapper Mapper
        {
            get
            {
                return AutoMapperConfig.Configuration.CreateMapper();
            }
        }
    }
}
