using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoAppWebApi.Models
{
    public class Todo
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        [Required(ErrorMessage = "Title is Required")]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Status is Required")]
        public string Status { get; set; }

        public DateTime Created_at { get; set; }
        public DateTime Updated_at { get; set; }
        public DateTime Deleted_at { get; set; }
    }
}
