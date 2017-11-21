namespace GGG_OnlineShop.Web.Api.Areas.Administration.Controllers
{
    using System.Linq;
    using Common;
    using Api.Controllers;
    using Data.Services.Contracts;
    using System.Web.Http;
    using System;
    using System.Net.Http;
    using System.Net;
    using ViewModels.Users;
    using Infrastructure;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public class ManageUsersController : BaseController
    {
        private readonly IUsersService users;

        public ManageUsersController(IUsersService users)
        {
            this.users = users;
        }

        // TODO should not be able to remove users
        //[HttpPost]
        //[Route("api/ManageUsers/Delete/{userId}")]
        //public IHttpActionResult Delete(string userId)
        //{
        //    try
        //    {
        //        var bulstat = this.users.GetById(userId).Bulstat;
        //        this.users.Delete(userId);
        //        return this.Ok($"user with {bulstat} removed successfully");
        //    }
        //    catch (Exception e)
        //    {
        //        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
        //                                         e.Message));
        //    }
        //}

        [HttpGet]
        [Route("api/Administration/ManageUsers")]
        public IHttpActionResult Get()
        {
            try
            {
                var users = this.users.GetAll()
                                .OrderBy(x => x.CreatedOn)
                                .ThenBy(x => x.Id)
                                .To<UserModel>()
                                .ToList();

                return this.Ok(users);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                                                 e.Message));
            }
        }
    }
}
