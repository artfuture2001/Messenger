using Messenger.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Messenger.Data
{
	public class ApplicationDbContext : IdentityDbContext<AppUser>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.Entity<Message>()
				.HasOne<AppUser>(a => a.Sender)
				.WithMany(d => d.Messages)
				.HasForeignKey(d => d.UserName);
		}

		public DbSet<Message> Messages { get; set; }
	}
}
