using Events.Data.Domain_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Events.Web.Extensions;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using System.Security.Principal;
using System.Data.Entity;

namespace Events.Data.Repository
{
    public  class BookEventRepo
    {

        protected ApplicationDbContext db = new ApplicationDbContext();

        #region Fetch all upcoming passed events from database
        public UpcomingPassedEvents GetAllUpcomingPassedEvents(string authorId)
        {

            if (authorId == null)
            {
                var events = this.db.Events

              .Where(e => e.IsPublic)
              .OrderBy(e => e.StartDateTime);

                return new UpcomingPassedEvents()
                {
                    Events = events
                };

            }
             else if (authorId.Equals("163e5e51-5aac-4ce6-b60b-fc1ccc5604c1"))
            {
                var events = this.db.Events

              .Where(e => e.IsPublic || !e.IsPublic)
              .OrderBy(e => e.StartDateTime);

                return new UpcomingPassedEvents()
                {
                    Events = events
                };

            }
            else
            {
                var events = this.db.Events

               .Where(e => e.IsPublic)
               .OrderBy(e => e.StartDateTime);

                return new UpcomingPassedEvents()
                {
                    Events = events
                };

            }



            
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="authorId"></param>
        /// <returns></returns>
        public UpcomingPassedEvents GetAllUpcomingPassedEventsByAuthorId(string authorId)
        {

            var events = this.db.Events
                .Where(e => e.AuthorId == authorId)
                .OrderBy(e => e.StartDateTime);


            return new UpcomingPassedEvents()
            {
                Events = events
            };
        }

        public void saveChanges()
        {
            db.SaveChanges();
        }
        public void RemoveEvent(EventDomain e)
        {
            var eventToDelete = db.Events.Where(x=> x.Id == e.Id).FirstOrDefault();
            db.Events.Remove(eventToDelete);
            db.SaveChanges();
        }
        public void AddEvent(EventDomain e)
        {
            
            db.Events.Add(e);
            db.SaveChanges();
        }



        public  EventDomain LoadEditEvent(int id,string authorId , bool isAdmin)
        {
            
            
            var eventToEdit = this.db.Events.Include(c => c.Author)
                .Where(e => e.Id == id ) // for editing future events only 
                .FirstOrDefault(e => e.AuthorId == authorId || isAdmin);
            return eventToEdit;
        }

        public UpcomingPassedEvents GetAllInvitedEventsByEmail(string currentUserEmail)
        {
            var events = this.db.Events
               .Where(e => e.InviteByEmail.Contains(currentUserEmail))
               .OrderBy(e => e.StartDateTime);

           
            return new UpcomingPassedEvents()
            {
                Events=events
            };


        }



        public EventDetailsAttribute GetDetailsById(int id)
        {
            // string currentUserId = db.HttpContext.User.Identity.Name;

            //var eventDetails = this.db.Events
            //    .Where(e => e.Id == id)
            //    .Where(e => e.IsPublic);
            
            var eventDetails = this.db.Events.Include(c => c.Author)
               .Where(e => e.Id == id)
               .Where(e => e.IsPublic || (e.AuthorId != null))
           
               .FirstOrDefault();

           // var eventDetails = this.db.Events.FirstOrDefault(e => e.Id == id && e.IsPublic == true);



            return new EventDetailsAttribute()
            {
                DetailedEvent = eventDetails
            };

        }
    }
}
#endregion