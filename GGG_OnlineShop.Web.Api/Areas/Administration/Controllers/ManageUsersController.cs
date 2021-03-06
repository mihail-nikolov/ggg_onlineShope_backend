﻿using System.Web;

namespace GGG_OnlineShop.Web.Api.Areas.Administration.Controllers
{
    using System.Linq;
    using Common;
    using Api.Controllers;
    using Data.Services.Contracts;
    using System.Web.Http;
    using System;
    using System.Net.Http;
    using Models.Users;
    using Infrastructure;
    using InternalApiDB.Models;
    using Api.Models;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity.Owin;
    using System.Reflection;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [RoutePrefix("api/Administration/ManageUsers")]
    public class ManageUsersController : BaseController
    {
        private readonly IUsersService users;
        private readonly IEmailsService emails;
        private ApplicationUserManager _userManager;
        private readonly string controllerName = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public ManageUsersController(IUsersService users, IEmailsService emails, ILogsService dbLogger)
            : base(dbLogger)
        {
            this.users = users;
            this.emails = emails;
        }

        public ManageUsersController(IUsersService users, IEmailsService emails, ApplicationUserManager manager, ILogsService dbLogger)
            : base(dbLogger)
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
                                         .OrderByDescending(x => x.CreatedOn)
                                         .ThenBy(x => x.Id)
                                         .To<UserResponseModel>()
                                         .ToList();

                return this.Ok(allUsers);
            }
            catch (Exception e)
            {
                HandlExceptionLogging(e, "", controllerName);
                return InternalServerError();
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

                if (!this.users.IsCompanyAndBulstatCompatibiltyValid(user))
                {
                    return BadRequest(GlobalConstants.InvalidCompanyBulstatCombination);
                }

                var updatedUser = this.Mapper.Map<UserResponseModel>(this.users.Update(user));

                return this.Ok(updatedUser);
            }
            catch (Exception e)
            {
                HandlExceptionLogging(e, "", controllerName);
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("SendEmailConfirmation")]
        public async Task<IHttpActionResult> SendEmailConfirmation(AccountEmailRequestModel model)
        {
            try
            {
                User user = this.users.GetByEmail(model.Email);
                string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                string encodedCode = HttpUtility.UrlEncode(code);
                string encodedUserId = HttpUtility.UrlEncode(user.Id);
                string fullCallbackUrl = $"{GlobalConstants.AppDomainPath}/confirmemail?userid={encodedUserId}&code={encodedCode}";

                await this.emails.SendEmail(user.Email, GlobalConstants.ConfirmEmailSubject,
                                       string.Format(GlobalConstants.ConfirmEmailBody, fullCallbackUrl), GlobalConstants.SMTPServer,
                                       GlobalConstants.EmailPrimary, GlobalConstants.EmailPrimaryPassword);

                return Ok();
            }
            catch (Exception e)
            {
                HandlExceptionLogging(e, "", controllerName);
                return InternalServerError();
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
        //        
        //                                         
        //    }
        //}
    }
}
