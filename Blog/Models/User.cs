using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class User
    {
        [Key]
        public int UserId  {get;set;}
     
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }

        [Required]
        [Range(typeof(DateTime), "1/1/1920", "12/31/2001", ErrorMessage = "Must be 18 or older")]
        public string Password { get; set; }

       public List<Post> CreatedPosts { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

    }

    public class Post
    {
        [Key]
        public int PostId { get; set; }

        public string PostDescription { get; set; }

        public int Like { get; set; } = 0;


        public User Creator { get; set; }
    }
}
