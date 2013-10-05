using OfficeSportEventsManager.WebApi.Attributes;
using OfficeSportEventsManager.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ValueProviders;
using OfficeSportEventsManager.Data;
using OfficeSportEventsManager.Models;

namespace OfficeSportEventsManager.WebApi.Controllers
{
    public class CompaniesController : BaseApiController
    {
        [ActionName("all")]
        public IQueryable<CompanyModelShort> GetAll(
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string authToken)
        {
            return this.PerformOperationAndHandleExceptions(() =>
            {
                var context = new OfficeSportsContext();
                var companyModels =
                (from allCompanies in context.Companies
                 select new CompanyModelShort
                 {
                     Id = allCompanies.Id,
                     Name = allCompanies.Name,
                     Longitude = allCompanies.Longitude,
                     Latitude = allCompanies.Latitude,
                     Participants = from allParticipants in allCompanies.Participants
                                    select new ParticipantModel
                                    {
                                        Id = allParticipants.Id,
                                        Username = allParticipants.Username,
                                        IsAdmin = allParticipants.IsAdmin,
                                        PictureLink = allParticipants.PictureLink
                                    }
                 }
                 ).OrderBy(c => c.Name);

                return companyModels;
            });
        }

        [ActionName("user")]
        public CompanyModelShort GetCompanyByUser(
            [ValueProvider(typeof(HeaderValueProviderFactory<string>))] string authToken)
        {
            return this.PerformOperationAndHandleExceptions(() =>
             {
                 var context = new OfficeSportsContext();
                
                 var companyModel =
                     (from allCompanies in context.Companies
                      where allCompanies.Participants.Any(x=>x.AuthToken==authToken)
                      select new CompanyModelShort
                      {
                          Id = allCompanies.Id,
                          Name = allCompanies.Name,
                          Longitude = allCompanies.Longitude,
                          Latitude = allCompanies.Latitude,
                          Participants = from allParticipants in allCompanies.Participants
                                         select new ParticipantModel
                                         {
                                             Id = allParticipants.Id,
                                             Username = allParticipants.Username,
                                             IsAdmin = allParticipants.IsAdmin,
                                             PictureLink = allParticipants.PictureLink
                                         }
                      }).FirstOrDefault();

                 return companyModel;
             });
        }

        [ActionName("new")]
        [HttpPost]
        public HttpResponseMessage PostCompany([ValueProvider(typeof(HeaderValueProviderFactory<string>))] string authToken,
        [FromBody] CompanyModel newCompanyModel)
        {
            return this.PerformOperationAndHandleExceptions(() =>
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

                    Company newCompany = new Company
                        {
                            Name = newCompanyModel.Name,
                            Latitude = newCompanyModel.Latitude,
                            Longitude = newCompanyModel.Longitude,
                        };
                    user.IsAdmin = true;
                    newCompany.Participants.Add(user);
                    context.Companies.Add(newCompany);
                    context.SaveChanges();

                    var response =
                        this.Request.CreateResponse(HttpStatusCode.Created,
                                        newCompany.Id);
                    return response;
                }
            });
        }


        [ActionName("join")]
        [HttpPut]
        public HttpResponseMessage PutJoinCompany([ValueProvider(typeof(HeaderValueProviderFactory<string>))] string authToken, int id)
        {
            return this.PerformOperationAndHandleExceptions(() =>
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

                    Company newCompany = context.Companies.Where(x => x.Id == id).FirstOrDefault();
                    newCompany.Participants.Add(user);
                    context.SaveChanges();

                    var response =
                        this.Request.CreateResponse(HttpStatusCode.OK);
                    return response;
                }
            });
        }

        [ActionName("location")]
        public IEnumerable<CompanyModelShort> GetCompaniesByLocation(double latitude, double longitude)
        {
            return this.PerformOperationAndHandleExceptions(() =>
            {
                var context = new OfficeSportsContext();

                var companyModel =
                    (from allCompanies in context.Companies
                     where ((Math.Abs ((double)(allCompanies.Latitude - latitude))) <= 2 &&
                      (Math.Abs((double)(allCompanies.Longitude - longitude))) <= 2)
                     select new CompanyModelShort
                     {
                         Id = allCompanies.Id,
                         Name = allCompanies.Name,
                         Longitude = allCompanies.Longitude,
                         Latitude = allCompanies.Latitude,
                         Participants = from allParticipants in allCompanies.Participants
                                        select new ParticipantModel
                                        {
                                            Id = allParticipants.Id,
                                            Username = allParticipants.Username,
                                            PictureLink = allParticipants.PictureLink,
                                        }
                     });

                return companyModel;
            });
        }
       
    }
}
