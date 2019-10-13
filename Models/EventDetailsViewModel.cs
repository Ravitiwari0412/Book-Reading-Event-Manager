namespace Events.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    
    using Events.Data;
    using Shared.EventsDTO;

    public class EventDetailsViewModel
    {

        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime StartDateTime { get; set; }

        public int? Duration { get; set; }

        public string Author { get; set; }

        public string Location { get; set; }
        public bool IsPublic { get; set; }


        public string Description { get; set; }
        public string InvitedEmails { get; set; }
        public int TotalInvited { get; set; }

        public string AuthorId { get; set; }
        public string OtherDetails { get; set; }
        public bool CanEdit { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }

       
        public static IEnumerable<EventDetailsViewModel> FillViewModel(EventDto e, string authorId, bool isAdmin)
        {
            IList<EventDetailsViewModel> reqEvents = new List<EventDetailsViewModel>();
            EventDetailsViewModel eventViewModel = new EventDetailsViewModel()

            {
                Id = e.Id,
                Title = e.Title,
                StartDateTime = e.StartDateTime,
                Location = e.Location,
                Description = e.Description,
                Duration = e.Duration,
                IsPublic = e.IsPublic,
                Author = e.Author.FullName,
                InvitedEmails = e.InvitedEmails,
                OtherDetails = e.OtherDetails,
                 AuthorId = e.Author.Id,
                 // Comments = e.Comments.AsQueryable().Select(CommentViewModel.ViewModel),
            };
            //string[] emails = e.InvitedEmails.Split(',');
            //eventViewModel.TotalInvited = emails.Length;
            if (isAdmin || eventViewModel.AuthorId == authorId)
            {
                eventViewModel.CanEdit = true;
                     
            }
            reqEvents.Add(eventViewModel);
            return reqEvents;
        }






        public static EventDetailsViewModel FillViewModelFromDto(EventDto e)
        {

            return new EventDetailsViewModel
            {
                Id = e.Id,
                Title = e.Title,
                StartDateTime = e.StartDateTime,
                Duration = e.Duration,
                Location = e.Location,
                Author = e.Author.FullName,
                Description = e.Description,
                IsPublic = e.IsPublic,
                AuthorId = e.AuthorId,
                InvitedEmails = e.InvitedEmails,
                OtherDetails = e.OtherDetails


            };
        }

        public static Expression<Func<EventDomain, EventDetailsViewModel>> ViewModel
        {
            get
            {
                return e => new EventDetailsViewModel()
                {
                    Id = e.Id,
                    Title = e.Title,
                    StartDateTime = e.StartDateTime,
                    Duration = e.Duration,
                    Author = e.Author.FullName,
                    Location = e.Location,
                    Description = e.Description,
                    Comments = e.Comments.AsQueryable().Select(CommentViewModel.ViewModel),
                    AuthorId = e.Author.Id,
                    OtherDetails=e.OtherDetails
                };
            }
        }
    }
}
