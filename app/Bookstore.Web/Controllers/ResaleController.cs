using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bookstore.Web.ViewModel.Resale;
using Bookstore.Web.Helpers;
using Bookstore.Domain.Offers;
using Bookstore.Domain.ReferenceData;

namespace Bookstore.Web.Controllers
{
    public class ResaleController : Controller
    {
        private readonly IReferenceDataService referenceDataService;
        private readonly IOfferService offerService;

        public ResaleController(IReferenceDataService referenceDataService, IOfferService offerService)
        {
            this.referenceDataService = referenceDataService;
            this.offerService = offerService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Offer> offers = await offerService.GetOffersAsync(User.GetSub());

            return View(new ResaleIndexViewModel(offers));
        }

        public async Task<IActionResult> Create()
        {
            IEnumerable<ReferenceDataItem> referenceDataDtos = await referenceDataService.GetAllReferenceDataAsync();

            return View(new ResaleCreateViewModel(referenceDataDtos));
        }

        [HttpPost]
        public async Task<IActionResult> Create(ResaleCreateViewModel resaleViewModel)
        {
            if (!ModelState.IsValid) return View();

            CreateOfferDto dto = new CreateOfferDto(
                User.GetSub(), 
                resaleViewModel.BookName, 
                resaleViewModel.Author, 
                resaleViewModel.ISBN, 
                resaleViewModel.SelectedBookTypeId, 
                resaleViewModel.SelectedConditionId, 
                resaleViewModel.SelectedGenreId, 
                resaleViewModel.SelectedPublisherId, 
                resaleViewModel.BookPrice);

            await offerService.CreateOfferAsync(dto);

            return RedirectToAction(nameof(Index));
        }
    }
}