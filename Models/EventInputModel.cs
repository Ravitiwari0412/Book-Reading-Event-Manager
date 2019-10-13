namespace Events.Web.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
   
    using Events.Data;
    using Shared.EventsDTO;

    public class EventInputModel
    {
        [Required(ErrorMessage = "Event title is required.")]
        [StringLength(200, ErrorMessage = "The {0} must be between {2} and {1} characters long.", 
            MinimumLength = 1)]
        [Display(Name = "Title *")]
        public string Title { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Date and Time *")]
        public DateTime StartDateTime { get; set; }
        [Range(0,4)]
        public int? Duration { get; set; }

        public string Description { get; set; }

        [MaxLength(200)]
        public string Location { get; set; }

        public CommentDomain NewComment { get; set; }

        [Required]
        [Display(Name = "Invite Via Email :")]
        public string InviteByEmail { get; set; }


        [Display(Name = "Is Public?")]
        public bool IsPublic { get; set; }


      #region CreateFromEvent maps from EventDto of service to EventInputModel
        public static EventInputModel CreateFromEvent(EventDto e)
        {
            return new EventInputModel()
            {
                Title = e.Title,
                StartDateTime = e.StartDateTime,
                Duration = e.Duration,
                Location = e.Location,
                Description = e.Description,
                IsPublic = e.IsPublic,
                NewComment=e.NewComment
            };
        }
        #endregion
    }
}