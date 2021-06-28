using CoreMVC.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreMVC.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class HomeController : Controller
	{
		private readonly DataManager dataManager;

		public HomeController(DataManager data)
		{
			dataManager = data;
		}

		public IActionResult Index()
		{
			return View(dataManager.ServiceItems.GetServiceItems());
		}
	}
}
