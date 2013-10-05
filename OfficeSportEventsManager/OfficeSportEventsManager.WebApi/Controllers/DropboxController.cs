using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Diagnostics;
using DotNetOpenAuth.OAuth2;
using OfficeSportEventsManager.WebApi.Attributes;
using OfficeSportEventsManager.Data;
using OfficeSportEventsManager.WebApi.Models;
using System.Web.Http.ValueProviders;
using OfficeSportEventsManager.Libraries;

namespace OfficeSportEventsManager.WebApi.Controllers
{
    public class DropBoxController : BaseApiController
    {
        [ActionName("profilePic")]
        public HttpResponseMessage PostUploadProfilePicture(
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string authToken,
            [FromBody]string picture)
        {
            var responseMsg = this.PerformOperationAndHandleExceptions(
               () =>
               {
                   var context = new OfficeSportsContext();
                   using (context)
                   {
                       var user = context.Users.FirstOrDefault(
                           usr => usr.AuthToken == authToken);

                       if (user == null)
                       {
                           throw new InvalidOperationException("User is not logged in");
                       }
                       DropboxImageUploader.Upload(picture);
                       
                       var response =
                                     this.Request.CreateResponse(HttpStatusCode.Created);
                       return response;
                   }
               });

            return responseMsg;
        }
    }
}
