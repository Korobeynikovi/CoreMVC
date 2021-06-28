using CoreMVC.Domain;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CoreMVC.Controllers
{
	public class ServicesController : Controller
	{
		private readonly DataManager dataManager;

		public ServicesController(DataManager inDataManager)
		{
			dataManager = inDataManager;
		}

		public IActionResult Index(Guid id)
		{
			if (id != default)
			{
				return View("Show", dataManager.ServiceItems.GetServiceItemById(id));
			}

			ViewBag.TextField = dataManager.TextFields.GetTextFieldByCodeWord("PageServices");
			return View(dataManager.ServiceItems.GetServiceItems());
		}
	}
}
