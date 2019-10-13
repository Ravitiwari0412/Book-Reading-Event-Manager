namespace Events.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class EventDomain
    {
        public EventDomain()
        {
            this.Comments = new HashSet<CommentDomain>();
            this.IsPublic = true;
            this.StartDateTime = DateTime.Now;
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        public int? Duration { get; set; }

        public string AuthorId { get; set; }

        public  ApplicationUser Author { get; set; }

        public string Description { get; set; }

       public string InviteByEmail { get; set; }

        [MaxLength(200)]
        public string Location { get; set; }

        public bool IsPublic { get; set; }

        public CommentDomain NewComment { get; set; }
        public string OtherDetails { get; set; }

        public  ICollection<CommentDomain> Comments { get; set; }
    }
}
