﻿using Bookstore.Domain.ReferenceData;
using Bookstore.Web.Areas.Admin.Mappers.ReferenceData;
using Bookstore.Web.Areas.Admin.Models.ReferenceData;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Threading.Tasks;

namespace Bookstore.Web.Areas.Admin.Controllers
{
    public class ReferenceDataController : AdminAreaControllerBase
    {
        private readonly IReferenceDataService referenceDataService;

        public ReferenceDataController(IReferenceDataService referenceDataService)
        {
            this.referenceDataService = referenceDataService;
        }

        public IActionResult Index(ReferenceDataIndexViewModel model)
        {
            var referenceDataItems = referenceDataService.GetReferenceData(model.Filters.ReferenceDataType);

            model.Items = referenceDataItems.ToReferenceDataIndexListItemViewModels();

            return View(model);
        }

        public IActionResult Create(ReferenceDataType? selectedReferenceDataType = null)
        {
            var model = new ReferenceDataItemCreateUpdateViewModel();

            if (selectedReferenceDataType.HasValue) model.SelectedReferenceDataType = selectedReferenceDataType.Value;

            return View("CreateUpdate", model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ReferenceDataItemCreateUpdateViewModel model)
        {
            var referenceDataItem = model.ToReferenceDataItem();

            await referenceDataService.SaveAsync(referenceDataItem, User.Identity.Name);

            return RedirectToAction("Index");
        }

        public IActionResult Update(int id)
        {
            var referenceDataItem = referenceDataService.GetReferenceDataItem(id);

            return View("CreateUpdate", referenceDataItem.ToReferenceDataItemCreateUpdateViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Update(ReferenceDataItemCreateUpdateViewModel model)
        {
            var referenceDataItem = referenceDataService.GetReferenceDataItem(model.Id);

            model.ToReferenceDataItem(referenceDataItem);

            await referenceDataService.SaveAsync(referenceDataItem, User.Identity.Name);

            return RedirectToAction("Index");
        }
    }
}
