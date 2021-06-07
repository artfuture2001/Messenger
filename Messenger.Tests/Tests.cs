using Messenger.Controllers;
using Messenger.Data;
using Messenger.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace Messenger.Tests
{
	[TestClass]
	public class Tests
	{
		[TestMethod]
		public void HomeController_ReturnsView()
		{
			//Arrange
			AppUser user = new AppUser()
			{
				Email = "test@gmail.com",
				UserName = "test@gmail.com",
				Id = "new-id-123",
				FirstName = "Tom",
				LastName = "Parkinson"
			};
			var context = new Mock<ApplicationDbContext>();
			var mockSet = new Mock<DbSet<Message>>();
			mockSet.Setup(m => m.AddRange(
				new Message { Id = 1, Sender = user, SendTime = DateTime.Now, Text = "test1", UserId = user.Id, UserName = user.UserName },
				new Message { Id = 2, Sender = user, SendTime = DateTime.Now, Text = "test2", UserId = user.Id, UserName = user.UserName },
				new Message { Id = 3, Sender = user, SendTime = DateTime.Now, Text = "test3", UserId = user.Id, UserName = user.UserName }
			));
			
			context.Setup(m => m.Set<Message>()).Returns(mockSet.Object);

			//Act
			var mes = new Message { Id = 4, Sender = user, SendTime = DateTime.Now, Text = "test4", UserId = user.Id, UserName = user.UserName };
			context.Object.Messages.Add(mes);
			var mesInDb = context.Object.Messages.FirstOrDefaultAsync(x => x.Id.Equals(4));

			//Assert
			Assert.IsNotNull(context);
			Assert.IsNotNull(mesInDb);
			Assert.AreEqual(mes, mesInDb);
		}
	}
}
