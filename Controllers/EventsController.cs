using System.Web.Mvc;

namespace Events.Web.Controllers
{
    using System;
    using System.Data;
    using System.Linq;
    
    using BookEvents.Business.Services;
   
    
    using Events.Web.Extensions;
    using Events.Web.Filters;
    using Events.Web.Models;
   
   

    using Microsoft.AspNet.Identity;
    using Shared.EventsDTO;

    /// <summary>
    /// Controller For CRUD Operations Only Accessible to Logged In User
    /// </summary>
    [AuthAttribute]
    [Authorize]
    public class EventsController : BaseController
    {
        BookEventService eventService = new BookEventService();


        /// <summary>
        /// Deals with support Functionality Of Application
        /// </summary>
        /// <returns></returns>
        public ActionResult Support()
        {
            return Redirect("https://cas.nagarro.com/login?service=https%3A%2F%2Fhelpdesk.nagarro.com%2Fkayako%2F__apps%2Fcas%2Fcas_index.php"); // redirects to external url
        }

        /// <summary>
        /// Home Page Of controller
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //Accessing current LoggedIn user
            string currentUserId = this.User.Identity.GetUserId();


            //Getting All Public and private Events From BusinessService
            var upcomingPassedEventsFromBusiness = eventService.GetAllUpcomingPassedPublicEvents(currentUserId);

            //Mapping EventsDto To EventViewModels
            var upcomingPassedViewModelEvents = EventViewModel.FillViewModelsFromDtos(upcomingPassedEventsFromBusiness);

            //Differentiating Passed And Future Events
            var upcomingEvents = upcomingPassedViewModelEvents.Where(e => e.StartDateTime > DateTime.Now);
            var passedEvents = upcomingPassedViewModelEvents.Where(e => e.StartDateTime <= DateTime.Now);


            return View(new UpcomingPassedEventsViewModel()
            {
                UpcomingEvents = upcomingEvents,
                PassedEvents = passedEvents
            });
        }

        /// <summary>
        /// Action For My Events Page deals with only userLoggedIn events
        /// </summary>
        /// <returns></returns>
        public ActionResult My()
        {
            string currentUserId = this.User.Identity.GetUserId();
            
            //Getting all events Of Given AuthorId
            var upcomingPassedEventsFromBusiness = eventService.GetAllUpcominPassedEventsByAuthorId(currentUserId);

            var upcomingPassedViewModelEvents = EventViewModel.FillViewModelsFromDtos(upcomingPassedEventsFromBusiness);
            var upcomingEvents = upcomingPassedViewModelEvents.Where(e => e.StartDateTime > DateTime.Now);
            var passedEvents = upcomingPassedViewModelEvents.Where(e => e.StartDateTime <= DateTime.Now);

            
            return View(new UpcomingPassedEventsViewModel()
            {
                UpcomingEvents = upcomingEvents,
                PassedEvents = passedEvents
            });
        }

        /// <summary>
        /// Deals With All the events in which current user Has been Invited
        /// </summary>
        /// <returns></returns>
       
        public ActionResult EventsInvitedTo()
        {
            string currentUserId = this.User.Identity.GetUserId();
            //Getting Current LoggedIn User Email
            string currentUserEmail = this.User.Identity.GetUserName();

            // Getting All events in which user is been invited
            var upcomingPassedEventsFromBusiness = eventService.GetAllUpcomingPassedInvitedEventsByEmail(currentUserEmail);

            var upcomingPassedViewModelEvents = EventViewModel.FillViewModelsFromDtos(upcomingPassedEventsFromBusiness);
            var upcomingEvents = upcomingPassedViewModelEvents.Where(e => e.StartDateTime > DateTime.Now);
            var passedEvents = upcomingPassedViewModelEvents.Where(e => e.StartDateTime <= DateTime.Now);

            return View("EventsInvitedTo", new UpcomingPassedEventsViewModel()
            {
                UpcomingEvents = upcomingEvents,
                PassedEvents = passedEvents
            });
        }

        [HttpGet]
        public ActionResult Create()
        {
            return this.View("Create");
        }
        
        /// <summary>
        /// Action For Creating New Event
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title, StartDateTime, Location, InviteByEmail,Description,Duration,IsPublic")]EventInputModel model)
        {
            try
            {
                // Validation 
                if (model != null && this.ModelState.IsValid)
                {
                    var e = new EventDto()
                    {
                        AuthorId = this.User.Identity.GetUserId(),
                        Title = model.Title,
                        StartDateTime = model.StartDateTime,
                        Duration = model.Duration,
                        Description = model.Description,
                        Location = model.Location,
                        IsPublic = model.IsPublic,
                        InvitedEmails = model.InviteByEmail
                    };
                    
                    // Adding of EventDto to Service Layer
                    eventService.AddEventDto(e);
                    eventService.SaveChanges();
                    this.AddNotification("Event created.", NotificationType.INFO);
                    return this.RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }



            return this.View(model);
        }



        #region For editing previous created events with given id

        [HttpGet]
        public ActionResult Edit(int id)
        {
            string currentUserId = this.User.Identity.GetUserId();
            var isAdmin = this.IsAdmin();
            var eventToEdit = eventService.LoadEditEventDto(id, currentUserId, isAdmin);
            if (eventToEdit == null)
            {
                this.AddNotification("Cannot edit event #" + id, NotificationType.ERROR);
                return this.RedirectToAction("My");
            }

            var model = EventInputModel.CreateFromEvent(eventToEdit);
            return this.View("Edit", model);
        }
        #endregion

        /// <summary>
        /// Action For Editing Previous Created events
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EventInputModel model)
        {
            string currentUserId = this.User.Identity.GetUserId();
            var isAdmin = this.IsAdmin();

            // Loading of event which has permissions for admin and current Logged In user
            var eventToEdit = eventService.LoadEditEventDto(id, currentUserId, isAdmin);
            if (eventToEdit == null)
            {
                this.AddNotification("Cannot edit event #" + id, NotificationType.ERROR);
                return this.RedirectToAction("My");
            }

            if (model != null && this.ModelState.IsValid)
            {
                eventToEdit.Title = model.Title;
                eventToEdit.StartDateTime = model.StartDateTime;
                eventToEdit.Duration = model.Duration;
                eventToEdit.Description = model.Description;
                eventToEdit.Location = model.Location;
                eventToEdit.IsPublic = model.IsPublic;

                

               
                eventService.SaveChanges();
                // Adding Notification To Event Editing Completed
                this.AddNotification("Event edited.", NotificationType.INFO);
                return this.RedirectToAction("My");
            }

            return this.View(model);
        }


        #region Get Method For deleting an event with given id
        [HttpGet]
        public ActionResult Delete(int id)
        {
            string currentUserId = this.User.Identity.GetUserId();
            var isAdmin = this.IsAdmin();
            var eventToDelete = eventService.LoadEditEventDto(id, currentUserId, isAdmin);
            
            if (eventToDelete == null)
            {
                this.AddNotification("Cannot delete event #" + id, NotificationType.ERROR);
                return this.RedirectToAction("My");
            }

            var model = EventInputModel.CreateFromEvent(eventToDelete);
            return this.View("Delete", model);
        }
        #endregion

        /// <summary>
        /// Method To delete Event Dtos From Services
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, EventInputModel model)
        {
            string currentUserId = this.User.Identity.GetUserId();
            var isAdmin = this.IsAdmin();
            var eventToDelete = eventService.LoadEditEventDto(id, currentUserId, isAdmin);
            if (eventToDelete == null)
            {
                this.AddNotification("Cannot delete event #" + id, NotificationType.ERROR);
                return this.RedirectToAction("My");
            }
            eventService.RemoveEventDto(eventToDelete);
            
            this.AddNotification("Event deleted.", NotificationType.INFO);
            return this.RedirectToAction("My");
        }

        

       
    }
}
