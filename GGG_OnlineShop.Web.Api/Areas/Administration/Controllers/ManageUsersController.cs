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
    using Api.Models;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity.Owin;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [RoutePrefix("api/Administration/ManageUsers")]
    public class ManageUsersController : BaseController
    {
        private readonly IUsersService users;
        private readonly IEmailsService emails;
        private ApplicationUserManager _userManager;


        public ManageUsersController(IUsersService users, IEmailsService emails)
        {
            this.users = users;
            this.emails = emails;
        }

        public ManageUsersController(IUsersService users, IEmailsService emails, ApplicationUserManager manager)
        {
            this.users = users;
            this.emails = emails;
            this.UserManager = manager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [HttpGet]
        [Route("")]
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

            if (model.IsCompany && string.IsNullOrEmpty(model.Bulstat))
            {
                return BadRequest(GlobalConstants.InvalidCompanyBulstatCombination);
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

        [HttpPost]
        [Route("SendEmailConfirmation")]
        public async Task<IHttpActionResult> SendEmailConfirmation(AccountEmailRequestModel model)
        {
            try
            {
                var user = this.users.GetByEmail(model.Email);
                var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                var callbackUrl = this.Url.Route("GGG_OnlineShop_WithAction", new { Controller = "Account", Action = "ConfirmEmail", userId = user.Id, code = code });

                var fullCallbackUrl = GlobalConstants.AppDomainPath + callbackUrl;

                this.emails.SendEmail(GlobalConstants.EmalToSendFrom, GlobalConstants.ConfirmEmailSubject,
                                            string.Format(GlobalConstants.ConfirmEmailBody, fullCallbackUrl), GlobalConstants.SMTPServer,
                                            GlobalConstants.EmalToSendFrom, GlobalConstants.EmalToSendFromPassword);

                return Ok(fullCallbackUrl);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                                                 e.Message));
            }
        }

        // does not remove the user!
        //[HttpPost]
        //[Route("RemoveUser/{userId}")]
        //public IHttpActionResult RemoveUser(string userId)
        //{
        //    try
        //    {
        //        var user = this.users.GetById(userId);
        //        if (this.User.Identity.Name == user.UserName)
        //        {
        //            return this.BadRequest(GlobalConstants.CannotRemoveAdminErrorMessage);
        //        }

        //        if (user.OrderedItems.Any())
        //        {
        //            this.users.CleanUserInfoFromOrders(user);
        //        }

        //        IHttpActionResult result;
        //        IdentityResult userRemove = UserManager.Delete(user);
        //        if (userRemove.Succeeded)
        //        {
        //            result = this.Ok();
        //        }
        //        else
        //        {
        //            result = this.BadRequest(string.Join(", ", userRemove.Errors));
        //        }

        //        return result;
        //        //var bulstat = this.users.GetById(userId).Bulstat;
        //        //this.users.Delete(userId);
        //        //return this.Ok($"user with {bulstat} removed successfully");
        //    }
        //    catch (Exception e)
        //    {
        //        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
        //                                         e.Message));
        //    }
        //}
    }
}
