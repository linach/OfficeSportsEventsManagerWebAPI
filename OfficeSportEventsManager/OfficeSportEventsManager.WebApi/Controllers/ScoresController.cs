using OfficeSportEventsManager.Data;
using OfficeSportEventsManager.WebApi.Attributes;
using OfficeSportEventsManager.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ValueProviders;

namespace OfficeSportEventsManager.WebApi.Controllers
{
    public class ScoresController : BaseApiController
    {
        [ActionName("user")]
        [HttpGet]
        public IEnumerable<ScoreModel> GetScoresForUser(
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string authToken, int userId, string sportType)
        {
            return this.PerformOperationAndHandleExceptions(() =>
            {
                var context = new OfficeSportsContext();

                var user = context.Users.Where(x => x.AuthToken == authToken).FirstOrDefault();

                if (user == null)
                {
                    throw new ArgumentException("User is not logged in");
                }

                var allScores = from s in context.Scores
                                   where s.Player.Id == userId
                                   where s.SportEvent.Type==sportType
                                   select s;

                var scores = from s in allScores
                             select new ScoreModel
                             {
                                 Player = new ParticipantShortModel
                                 {
                                     Id = s.Player.Id,
                                     PictureLink = s.Player.PictureLink,
                                     Username = s.Player.Username
                                 },
                                 Value = s.Value
                             };

                return scores;
            });
        }
    }
}
