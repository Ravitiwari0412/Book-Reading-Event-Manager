namespace Events.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using BookEvents.Business.Services;
    using Events.Web.Models;

    using Microsoft.AspNet.Identity;


    /// <summary>
    /// Controller for Anonymous Users
    /// </summary>
    public class HomeController : BaseController
    {
        /// <summary>
        /// instance of BusinessService for accessing database
        /// </summary>
        BookEventService eventService = new BookEventService();
        public ActionResult Index()
        {
            string currentUserId = this.User.Identity.GetUserId();

            var upcomingPassedEventsFromBusiness = eventService.GetAllUpcomingPassedPublicEvents(currentUserId);

            #region Mapping of EventDtos From Service To EventViewModel
            var upcomingPassedViewModelEvents = EventViewModel.FillViewModelsFromDtos(upcomingPassedEventsFromBusiness);
            #endregion

            #region Dividing future and pastevents
            var upcomingEvents = upcomingPassedViewModelEvents.Where(e => e.StartDateTime > DateTime.Now);
            var passedEvents = upcomingPassedViewModelEvents.Where(e => e.StartDateTime <= DateTime.Now);
            #endregion


            return View(new UpcomingPassedEventsViewModel()
            {
                UpcomingEvents = upcomingEvents,
                PassedEvents = passedEvents
            });
        }

        /// <summary>
        /// To get a specific event with given Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        
        public ActionResult EventDetailsById(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Accessing current User
            var currentUserId = this.User.Identity.GetUserId();
            var isAdmin = this.IsAdmin();
            //Getting event from BusinessService in form of EventDto
            var eventDetailsFromBusiness = eventService.GetEventById(id);

            //Mapping EventDto To EventDetaisViewModel

            var eventDetails = EventDetailsViewModel.FillViewModel(eventDetailsFromBusiness,currentUserId,isAdmin).FirstOrDefault();

            
            

            return this.PartialView("EventDetailsById",eventDetails);
           
        }
      
    }
}
