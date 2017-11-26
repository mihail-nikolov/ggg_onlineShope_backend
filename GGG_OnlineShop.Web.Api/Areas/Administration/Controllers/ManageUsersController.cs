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
    using Models.Users;
    using Infrastructure;
    using InternalApiDB.Models;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [RoutePrefix("api/Administration/ManageUsers")]
    public class ManageUsersController : BaseController
    {
        private readonly IUsersService users;

        public ManageUsersController(IUsersService users)
        {
            this.users = users;
        }

        // TODO deactivate user
        //[HttpPost]
        //[Route("api/ManageUsers/deactivate/{userId}")]
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
        public IHttpActionResult Get()
        {
            try
            {
                var allUsers = this.users.GetAll()
                                .OrderBy(x => x.CreatedOn)
                                .ThenBy(x => x.Id)
                                .To<UserResponseModel>()
                                .ToList();

                return this.Ok(allUsers);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                                                 e.Message));
            }
        }

        [HttpPost]
        [Route("UpdateUserInfo")]
        public IHttpActionResult UpdateUserInfo(UserUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = this.Mapper.Map<User>(model);
                var updatedUser = this.Mapper.Map<UserResponseModel>(this.users.Update(user));

                return this.Ok(updatedUser);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                                                 e.Message));
            }
        }
    }
}
