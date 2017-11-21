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
    public class AdministrationController : BaseController
    {
        private readonly IGlassesInfoDbFiller dbInfoFiller;

        public AdministrationController(IGlassesInfoDbFiller dbInfoFiller)
        {
            this.dbInfoFiller = dbInfoFiller;
        }

        [HttpPost]
        [Route("api/Administration/dbInfoAddFromFile")]
        public IHttpActionResult DbInfoAdd()
        {
            GlassJsonInfoModel[] glasses = null;
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
                this.dbInfoFiller.FillInfo(glasses, postedFilePath);
                return this.Ok("Db filled/updated finished. Check log files for info/errors.");
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                                                 e.Message));
            }
        }


        [HttpPost]
        [Route("api/Administration/dbInfoAdd")]
        public IHttpActionResult DbInfoAdd(GlassJsonInfoModel[] glasses)
        {
            try
            {
                this.dbInfoFiller.FillInfo(glasses, "");
                return this.Ok("Db filled/updated finished. Check log files for info/errors.");
            }
            catch (Exception e)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed,
                                                 e.Message));
            }
        }
    }
}
