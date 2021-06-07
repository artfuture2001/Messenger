using Messenger.Data;
using Messenger.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Messenger.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly ILogger<HomeController> _logger;

		public UserManager<AppUser> UserManager { get; set;}

		public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<AppUser> userManager)
		{
			_logger = logger;
			_context = context;
			UserManager = userManager;
		}

		public async Task<IActionResult> Index()
		{
			var currentUser = await UserManager.GetUserAsync(User);
			if(User.Identity.IsAuthenticated)
			{
				ViewBag.CurrentUserName = currentUser.FirstName + " " + currentUser.LastName;
				ViewBag.UserId = currentUser.Id;
				ViewBag.FirstName = currentUser.FirstName;
				ViewBag.LastName = currentUser.LastName;
			}
			var messages = _context.Messages.ToList();
			foreach(var mes in messages)
			{
				var user = _context.Users.AsEnumerable().FirstOrDefault(x => x.Id.Equals(mes.UserId));
				if(user != null)
				{
					mes.Sender = user;
				}
			}
			return View(messages);
		}

		public async Task<IActionResult> Create(Message message)
		{
			if(ModelState.IsValid)
			{
				var currentUser = await UserManager.GetUserAsync(User);
				var userName = currentUser.FirstName + " " + currentUser.LastName;
				var sender = await UserManager.GetUserAsync(User);
				message.Sender = sender;
				message.UserId = sender.Id;
				message.SendTime = DateTime.Now;
				message.UserName = userName;
				_context.Add(message);
				_context.SaveChanges();
				ViewBag.MessageId = _context.Messages.AsEnumerable().FirstOrDefault().Id;
				return Ok();
			}
			return Error();
		}

		public async Task<IActionResult> Remove(int id)
		{
			var message = _context.Messages.AsEnumerable().FirstOrDefault(x => x.Id.Equals(id));
			if (message != null)
			{
				_context.Remove(message);
				await _context.SaveChangesAsync();
				return Ok();
			}
			return Error();
		}

		[HttpPost]
		public async Task<IActionResult> RemoveMes(Message message)
		{
			var currentUser = await UserManager.GetUserAsync(User);
			var mes = _context.Messages.AsEnumerable()
				.Where(x => x.UserId.Equals(currentUser.Id))
				.FirstOrDefault(x => x.Text.Equals(message.Text));

			if (mes != null)
			{
				_context.Remove(mes);
				await _context.SaveChangesAsync();
				return Ok();
			}
			return Error();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
