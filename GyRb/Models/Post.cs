﻿namespace GyRb.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? ShortDescription { get; set; }

        //relacion
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string? Description { get; set; }
        public string? Slug { get; set; }
        public string? ThumbnailUrl { get; set; }


    }
}
