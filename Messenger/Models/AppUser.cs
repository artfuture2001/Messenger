using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger.Models
{
	public class AppUser : IdentityUser
	{
		[Required]
		[Display(Name = "First Name")]
		public string FirstName { get; set; }
		[Required]
		[Display(Name = "Last Name")]
		public string LastName { get; set; }
		public AppUser()
		{
			Messages = new HashSet<Message>();
		}
		public virtual ICollection<Message> Messages { get; set; }
	}
}
