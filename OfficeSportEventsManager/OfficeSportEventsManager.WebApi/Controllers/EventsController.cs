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
using OfficeSportEventsManager.Models;

namespace OfficeSportEventsManager.WebApi.Controllers
{
    public class EventsController : BaseApiController
    {
        [ActionName("past")]
        [HttpGet]
        public IEnumerable<EventModel> GetAll(
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string authToken, int companyId)
        {
            return this.PerformOperationAndHandleExceptions(() =>
            {
                var context = new OfficeSportsContext();
                var eventModels =
                (from allEvents in context.SportEvents
                 where allEvents.Company.Id == companyId
                 select allEvents);

                var events = from eventQuery in eventModels
                             where eventQuery.Date < DateTime.Now
                             select new EventModel
                             {
                                 Id = eventQuery.Id,
                                 Date = eventQuery.Date,
                                 PictureLink = eventQuery.PictureLink,
                                 Type = eventQuery.Type,
                                 ScoresList = from scores in eventQuery.ScoresList
                                              select new ScoreModel
                                              {
                                                  Player = new ParticipantShortModel
                                                  {
                                                      Id = scores.Player.Id,
                                                      PictureLink = scores.Player.PictureLink,
                                                      Username = scores.Player.Username
                                                  },
                                                  Value = scores.Value
                                              }
                             };

                return events;
            });
        }

        [ActionName("details")]
        [HttpGet]
        public HttpResponseMessage GetDetails(
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string authToken, int eventId)
        {
            return this.PerformOperationAndHandleExceptions(() =>
            {
                var context = new OfficeSportsContext();
                var user = context.Users.Where(x => x.AuthToken == authToken).FirstOrDefault();

                if (user == null)
                {
                    throw new ArgumentException("User is not logged in!");
                }
                var queryEvent = (from allEvents in context.SportEvents
                                 where allEvents.Id == eventId
                                 select new EventModel
                                 {
                                     Id = allEvents.Id,
                                     Type = allEvents.Type,
                                     Date = allEvents.Date,
                                     PictureLink = allEvents.PictureLink,
                                     ScoresList = from allScores in allEvents.ScoresList
                                                  select new ScoreModel
                                                  {
                                                      Player = new ParticipantShortModel
                                                      {
                                                          Id = allScores.Player.Id,
                                                          PictureLink = allScores.Player.PictureLink,
                                                          Username = allScores.Player.Username
                                                      },
                                                      Value = allScores.Value
                                                  }
                                 }).FirstOrDefault();

                var response = this.Request.CreateResponse(HttpStatusCode.Created, queryEvent);
                return response;
            });
        }

        [ActionName("future")]
        [HttpGet]
        public IEnumerable<EventModel> GetAllFuture(
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string authToken, int companyId)
        {
            return this.PerformOperationAndHandleExceptions(() =>
            {
                var context = new OfficeSportsContext();
                var eventModels =
                (from allEvents in context.SportEvents
                 where allEvents.Company.Id == companyId
                 select allEvents);

                var events = from eventQuery in eventModels
                             where eventQuery.Date > DateTime.Now
                             select new EventModel
                             {
                                 Id = eventQuery.Id,
                                 Date = eventQuery.Date,
                                 PictureLink = eventQuery.PictureLink,
                                 Type = eventQuery.Type,
                                 ScoresList = from scores in eventQuery.ScoresList
                                              select new ScoreModel
                                              {
                                                  Player = new ParticipantShortModel
                                                  {
                                                      Id = scores.Player.Id,
                                                      PictureLink = scores.Player.PictureLink,
                                                      Username = scores.Player.Username
                                                  },
                                                  Value = scores.Value
                                              }
                             };

                return events;
            });
        }

        [ActionName("add")]
        [HttpPost]
        public HttpResponseMessage AddNewEvent(
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string authToken, int companyId, [FromBody] EventModel newEvent)
        {
            return this.PerformOperationAndHandleExceptions(() =>
            {
                var context = new OfficeSportsContext();

                var company = context.Companies.Where(x => x.Id == companyId).FirstOrDefault();
                var user = context.Users.Where(x => x.AuthToken == authToken).FirstOrDefault();

                SportEvent ev = new SportEvent
                {
                    Date = newEvent.Date,
                    Type = newEvent.Type,
                    PictureLink=newEvent.PictureLink
                };
                ev.ParticipatingPlayers.Add(user);
                context.SportEvents.Add(ev);
                ev.Company = company;
                context.SaveChanges();

                user.SportEvents.Add(ev);
                context.SaveChanges();
                var response =
                       this.Request.CreateResponse(HttpStatusCode.Created, ev);
                return response;
            });
        }

        [ActionName("addScoreList")]
        [HttpPost]
        public HttpResponseMessage AddScoreList(
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string authToken, int eventId,
            [FromBody] IEnumerable<ScoreModelShort> scoreModels)
        {
            return this.PerformOperationAndHandleExceptions(() =>
            {
                var context = new OfficeSportsContext();

                var user = context.Users.Where(x => x.AuthToken == authToken).FirstOrDefault();

                if (user == null)
                {
                    throw new ArgumentException("User is not logged in");
                }

                var eventModel = context.SportEvents.Where(x => x.Id == eventId).FirstOrDefault();
                if (eventModel == null)
                {
                    throw new ArgumentException("Unexisting Event");
                }
                foreach (var score in scoreModels)
                {
                    var participant = context.Users.Where(x => x.Username == score.Username).FirstOrDefault();
                    if (participant != null)
                    {
                        context.Scores.Add(new Score
                        {
                            Player = participant,
                            Value = score.Value,
                            SportEvent = eventModel,
                        });

                        context.SaveChanges();
                    }
                    else
                    {
                        throw new ArgumentException("User does not exist");
                    }
                }

                var response =
                       this.Request.CreateResponse(HttpStatusCode.Created);
                return response;
            });
        }

        [ActionName("enroll")]
        [HttpPut]
        public HttpResponseMessage EnrollInEvent([ValueProvider(typeof(HeaderValueProviderFactory<string>))] string authToken,
            int eventId)
        {
             return this.PerformOperationAndHandleExceptions(() =>
            {
                var context = new OfficeSportsContext();

                var user = context.Users.Where(x => x.AuthToken == authToken).FirstOrDefault();

                if (user == null)
                {
                    throw new ArgumentException("User is not logged in");
                }

                var eventToEnroll = context.SportEvents.Where(x => x.Id == eventId).FirstOrDefault();

                eventToEnroll.ParticipatingPlayers.Add(user);
                context.SaveChanges();

                var response =
                    this.Request.CreateResponse(HttpStatusCode.OK);
                return response;
            });
        }

        [ActionName("participants")]
        [HttpGet]
        public IEnumerable<ParticipantShortModel> GetParticipantsForEvent(
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string authToken, int eventId)
        {
            return this.PerformOperationAndHandleExceptions(() =>
            {
                var context = new OfficeSportsContext();

                var user = context.Users.Where(x => x.AuthToken == authToken).FirstOrDefault();

                if (user == null)
                {
                    throw new ArgumentException("User is not logged in");
                }

                var participants = from u in context.Users
                                   from i in u.SportEvents
                                   where i.Id == eventId
                                   select u;

                var events = context.SportEvents.ToList().Where(x => x.Id == eventId).FirstOrDefault();

                if (events==null)
                {
                    throw new ArgumentException("Unexisting event");
                }

                //var participants = events.ParticipatingPlayers;

                var participantsModels = from allPart in participants
                                         select new ParticipantShortModel
                                         {
                                             Id = allPart.Id,
                                             PictureLink = allPart.PictureLink,
                                             Username = allPart.Username
                                         };
                return participantsModels;
            });
        }
    }
}
