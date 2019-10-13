namespace Events.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
   
    using Events.Data;
    using Shared.EventsDTO;

    public class EventViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime StartDateTime { get; set; }

        public int? Duration { get; set; }

        public string Author { get; set; }
       // public string AuthorId { get; set; }

       // public string Description { get; set; }

        public string Location { get; set; }

        public int TotalInvited { get; set; }
        //public bool IsPublic { get; set; }
        //public string OtherDetails { get; set; }
        public string InvitedEmails { get; set; }

       // public ICollection<CommentDomain> Comments { get; set; }

       

        public static IEnumerable<EventViewModel> FillViewModelsFromDtos(IEnumerable<EventDto> events)
        {
            IList<EventViewModel> requiredEvents = new List<EventViewModel>();
            foreach (var reqEvent in events)
            {
                var eventViewModel = new EventViewModel
                {
                    Id = reqEvent.Id,
                    Title = reqEvent.Title,
                    StartDateTime = reqEvent.StartDateTime,
                    Location = reqEvent.Location,
                   // Description = reqEvent.Description,
                    Duration = reqEvent.Duration,
                   // IsPublic = reqEvent.IsPublic,
                  //  AuthorId = reqEvent.AuthorId,
                  //  InvitedEmails = reqEvent.InvitedEmails,
                  //  Author = reqEvent.Author.FullName,
                  //  OtherDetails = reqEvent.OtherDetails

                };
                //string[] emails = reqEvent.InvitedEmails.Split(',');
                //eventViewModel.TotalInvited = emails.Length;
                requiredEvents.Add(eventViewModel);
            }
            return requiredEvents;
        }


      




        public static Expression<Func<EventDomain, EventViewModel>> ViewModel
        {

            get
            {
                return e => new EventViewModel()


                {
                    Id = e.Id,
                    Title = e.Title,
                    StartDateTime = e.StartDateTime,
                    Duration = e.Duration,
                    Location = e.Location,
                    Author = e.Author.FullName,
                    //Description= e.Description,
                    //IsPublic=e.IsPublic,
                    //AuthorId=e.AuthorId,
                    //InvitedEmails=e.InviteByEmail,
                    //OtherDetails=e.OtherDetails
                    
                    
                    

                };
            }
        }
    }
}