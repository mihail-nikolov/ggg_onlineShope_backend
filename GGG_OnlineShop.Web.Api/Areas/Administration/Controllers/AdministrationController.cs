namespace GGG_OnlineShop.Web.Api.Areas.Administration.Controllers
{
    using Api.Controllers;
    using Common;
    using Data.Services.Contracts;
    using Data.Services.JsonParseModels;
    using System;
    using System.Reflection;
    using System.Web;
    using System.Web.Http;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [RoutePrefix("api/Administration")]
    public class AdministrationController : BaseController
    {
        private readonly IGlassesInfoDbFiller dbInfoFiller;
        private readonly string controllerName = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public AdministrationController(IGlassesInfoDbFiller dbInfoFiller, ILogsService dbLogger)
            : base(dbLogger)
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
               HandlExceptionLogging(e, "", controllerName);
               return InternalServerError();                            
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
               HandlExceptionLogging(e, "", controllerName);
               return InternalServerError();                               
            }
        }
    }
}
