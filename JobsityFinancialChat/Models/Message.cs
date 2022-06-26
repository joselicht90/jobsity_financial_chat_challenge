using System;
using System.ComponentModel.DataAnnotations;

namespace Josbity.Models
{
    public class Message
    {
        public int Id { get; set; }
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string Text { get; set; } = string.Empty;    
        public DateTime When { get; set; }
        public string UserID { get; set; } = Guid.NewGuid().ToString(); 
        public virtual JobsityUser? Sender { get; set; }

        public Message()
        {
            When = DateTime.Now;
        }
    }
}
