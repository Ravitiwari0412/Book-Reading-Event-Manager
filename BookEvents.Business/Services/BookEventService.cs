using System;
using System.Collections.Generic;

using Events.Data;
using Events.Data.Repository;
using Shared.EventsDTO;

namespace BookEvents.Business.Services
{

    /// <summary>
    /// Service class accessing models in repository used in web controllers 
    /// </summary>
    public class BookEventService
    {

        // creating instance of repository
        BookEventRepo bookEventRepo = new BookEventRepo();


        /// <summary>
        /// method to delete eventDto
        /// </summary>
        /// <param name="e"></param>
        public void RemoveEventDto(EventDto e)
        {
            var eventDomain = FillEventFromDto(e);

            bookEventRepo.RemoveEvent(eventDomain);
        }


        /// <summary>
        /// For getting all passed and future events 
        /// </summary>Get all passed and future events 
        /// <param name="authorId"></param>
        /// <returns></returns>
        public IEnumerable<EventDto> GetAllUpcomingPassedPublicEvents(string authorId)
        {
            var events = bookEventRepo.GetAllUpcomingPassedEvents(authorId);
            return FillModelDtoFromEntity(events.Events);
        }

       

        /// <summary>
        /// For getting a specific event of a given Id
        /// </summary> 
        /// <param name="id"></param>
        /// <returns></returns>
        public EventDto GetEventById(int id)
        {
            var reqEvent = bookEventRepo.GetDetailsById(id);
            return FillViewModel(reqEvent.DetailedEvent);
        }

        /// <summary>
        /// For getting  All events created by authorId
        /// </summary>
        /// <param name="authorId"></param>
        /// <returns></returns>
        public IEnumerable<EventDto> GetAllUpcominPassedEventsByAuthorId(string authorId)
        {
            var authorEvents = bookEventRepo.GetAllUpcomingPassedEventsByAuthorId(authorId);
            return FillModelDtoFromEntity(authorEvents.Events);
        }

       /// <summary>
       /// For Accessing Invitations Of Events Invited To
       /// </summary>
       /// <param name="email"></param>
       /// <returns></returns>
        public IEnumerable<EventDto> GetAllUpcomingPassedInvitedEventsByEmail(string email)
        {
            var eventsInvitedTo = bookEventRepo.GetAllInvitedEventsByEmail(email);
            return FillModelDtoFromEntity(eventsInvitedTo.Events);
        }
       

        /// <summary>
        /// For adding  newly event into database
        /// </summary>
        /// <param name="e"></param>
        public void AddEventDto(EventDto e)
        {
            var eventDomain = FillEventFromDto(e);

            bookEventRepo.AddEvent(eventDomain);
        }


        public void SaveChanges()
        {
            bookEventRepo.saveChanges();
        }
        #region Mapping For single event To EventDto
        private static EventDto FillViewModel(EventDomain e)
        {
            
            var eventDto = new EventDto()

            {
                Id = e.Id,
                Title = e.Title,
                StartDateTime = e.StartDateTime,
                Location = e.Location,
                Description = e.Description,
                Duration = e.Duration,
                IsPublic = e.IsPublic,
                Author = e.Author,
                InvitedEmails = e.InviteByEmail,
                OtherDetails = e.OtherDetails,
                 AuthorId = e.Author.Id,
                //  Comments = e.Comments.AsQueryable().Select(CommentViewModel.ViewModel),
            };
            
            return eventDto;
        }
        #endregion 

        #region Reverse Mapping For single EventDto mapping
        private static EventDomain FillEventFromDto(EventDto e)
        {

            var eventDomain = new EventDomain()

            {
                Id = e.Id,
                Title = e.Title,
                StartDateTime = e.StartDateTime,
                Location = e.Location,
                Description = e.Description,
                Duration = e.Duration,
                IsPublic = e.IsPublic,
                Author = e.Author,
                InviteByEmail = e.InvitedEmails,
                OtherDetails = e.OtherDetails,
                AuthorId = e.AuthorId,
                //  Comments = e.Comments.AsQueryable().Select(CommentViewModel.ViewModel),
            };

            return eventDomain;
        }
        #endregion

        /// <summary>
        /// For loading a given specific event that can be edited By authorised User
        /// </summary>
        /// <param name="id"></param>
        /// <param name="authorId"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        public EventDto LoadEditEventDto(int id, string authorId, bool isAdmin)
        {
            var loadedEvent = bookEventRepo.LoadEditEvent(id, authorId, isAdmin);
            return FillViewModel(loadedEvent);
        }


        #region Mapping events From events To EvenDtos
        private IEnumerable<EventDto> FillModelDtoFromEntity(IEnumerable<EventDomain> events)
        {
            IList<EventDto> requiredEvents = new List<EventDto>();
            foreach(var reqEvent in events)
            {
                var eventDto = new EventDto
                {
                    Id=reqEvent.Id,
                    Title=reqEvent.Title,
                    StartDateTime=reqEvent.StartDateTime,
                    Location=reqEvent.Location,
                    Description=reqEvent.Description,
                    Duration=reqEvent.Duration,
                    IsPublic=reqEvent.IsPublic,
                    AuthorId=reqEvent.AuthorId,
                    InvitedEmails=reqEvent.InviteByEmail,
                    Author=reqEvent.Author,
                    OtherDetails=reqEvent.OtherDetails

                };
                requiredEvents.Add(eventDto);
            }
            return requiredEvents;
        }
        #endregion
    }
}
