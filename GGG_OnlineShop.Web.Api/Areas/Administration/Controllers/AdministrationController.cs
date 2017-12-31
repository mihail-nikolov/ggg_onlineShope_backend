namespace GGG_OnlineShop.Web.Api.Areas.Administration.Controllers
{
    using Api.Controllers;
    using Common;
    using Data.Services.Contracts;
    using Data.Services.JsonParseModels;
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using System.Web.Http;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [RoutePrefix("api/Administration")]
    public class AdministrationController : BaseController
    {
        private readonly IGlassesInfoDbFiller dbInfoFiller;

        public AdministrationController(IGlassesInfoDbFiller dbInfoFiller)
        {
            this.dbInfoFiller = dbInfoFiller;
        }

        [HttpPost]
        [Route("dbInfoAddFromFile")]
        public IHttpActionResult DbInfoAddFromFile()
        {
            var httpRequest = HttpContext.Current.Request;
            string postedFilePath = string.Empty;

            if (httpRequest.Files.Count > 0)
            {
                var postedFile = httpRequest.Files[0];

                postedFilePath = HttpContext.Current.Server.MapPath("~/" + postedFile.FileName);
                postedFile.SaveAs(postedFilePath);
            }

            try
            {
                this.dbInfoFiller.FillInfo(null, postedFilePath);
                return this.Ok(GlobalConstants.DbFilledInFinishedMessage);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                                                 e.Message));
            }
        }


        [HttpPost]
        [Route("dbInfoAdd")]
        public IHttpActionResult DbInfoAdd(GlassJsonInfoModel[] glasses)
        {
            try
            {
                this.dbInfoFiller.FillInfo(glasses, "");
                return this.Ok(GlobalConstants.DbFilledInFinishedMessage);
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                                                 e.Message));
            }
        }
    }
}
