using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlayLib.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public int PostId { get; set; }     
        public string AuthorId { get; set; } 

        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public Post Post { get; set; }    
        public ApplicationUser Author { get; set; }
    }
}
