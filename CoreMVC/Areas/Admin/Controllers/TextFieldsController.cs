using CoreMVC.Domain;
using CoreMVC.Domain.Entities;
using CoreMVC.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreMVC.Areas.Admin.Controllers
{
	[Area("admin")]
	public class TextFieldsController : Controller
	{
		private readonly DataManager dataManager;
		public TextFieldsController(DataManager data)
		{
			dataManager = data;
		}

		public IActionResult Edit(string codeWord)
		{
			var entity = dataManager.TextFields.GetTextFieldByCodeWord(codeWord);
			return View(entity);
		}

		//[HttpPost]
		//public IActionResult Edit(TextField model)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		dataManager.TextFields.SaveTextField(model);
		//		return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).CutController());
		//	}
		//	return View(model);
		//}
	}
}
