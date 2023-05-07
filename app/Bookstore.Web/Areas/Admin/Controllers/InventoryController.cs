using System.Collections.Generic;
using System.Threading.Tasks;
using Bookstore.Domain;
using Microsoft.AspNetCore.Mvc;
using Bookstore.Web.Areas.Admin.Models.Inventory;
using Bookstore.Domain.Books;
using Bookstore.Domain.ReferenceData;

namespace Bookstore.Web.Areas.Admin.Controllers
{
    public class InventoryController : AdminAreaControllerBase
    {
        private readonly IBookService bookService;
        private readonly IReferenceDataService referenceDataService;

        public InventoryController(IBookService bookService, IReferenceDataService referenceDataService)
        {
            this.bookService = bookService;
            this.referenceDataService = referenceDataService;
        }

        public async Task<IActionResult> Index(BookFilters filters, int pageIndex = 1, int pageSize = 10)
        {
            IPaginatedList<Book> books = await bookService.GetBooksAsync(filters, pageIndex, pageSize);
            IEnumerable<ReferenceDataItem> referenceDataItems = await referenceDataService.GetAllReferenceDataAsync();

            return View(new InventoryIndexViewModel(books, referenceDataItems));
        }

        public async Task<IActionResult> Details(int id)
        {
            Book book = await bookService.GetBookAsync(id);

            return View(new InventoryDetailsViewModel(book));
        }

        public async Task<IActionResult> Create()
        {
            IEnumerable<ReferenceDataItem> referenceDataItemDtos = await referenceDataService.GetAllReferenceDataAsync();

            return View("CreateUpdate", new InventoryCreateUpdateViewModel(referenceDataItemDtos));
        }

        [HttpPost]
        public async Task<IActionResult> Create(InventoryCreateUpdateViewModel model)
        {
            if (!ModelState.IsValid) return await InvalidCreateUpdateView(model);

            CreateBookDto dto = new(
                model.Name, 
                model.Author, 
                model.SelectedBookTypeId, 
                model.SelectedConditionId, 
                model.SelectedGenreId, 
                model.SelectedPublisherId, 
                model.Year, 
                model.ISBN, 
                model.Summary, 
                model.Price, 
                model.Quantity, 
                model.CoverImage?.OpenReadStream(), 
                model.CoverImage?.FileName);

            BookResult result = await bookService.AddAsync(dto);

            return await ProcessBookResultAsync(model, result, $"{model.Name} has been added to inventory");
        }

        public async Task<IActionResult> Update(int id)
        {
            Book book = await bookService.GetBookAsync(id);
            IEnumerable<ReferenceDataItem> referenceDataDtos = await referenceDataService.GetAllReferenceDataAsync();

            return View("CreateUpdate", new InventoryCreateUpdateViewModel(referenceDataDtos, book));
        }

        [HttpPost]
        public async Task<IActionResult> Update(InventoryCreateUpdateViewModel model)
        {
            if (!ModelState.IsValid) return await InvalidCreateUpdateView(model);

            UpdateBookDto dto = new(
                model.Id,
                model.Name,
                model.Author,
                model.SelectedBookTypeId,
                model.SelectedConditionId,
                model.SelectedGenreId,
                model.SelectedPublisherId,
                model.Year,
                model.ISBN,
                model.Summary,
                model.Price,
                model.Quantity,
                model.CoverImage?.OpenReadStream(),
                model.CoverImage?.FileName);

            BookResult result = await bookService.UpdateAsync(dto);

            return await ProcessBookResultAsync(model, result, $"{model.Name} has been updated");
        }

        private async Task<IActionResult> ProcessBookResultAsync(InventoryCreateUpdateViewModel model, BookResult result, string successMessage)
        {
            if (result.IsSuccess)
            {
                TempData["Message"] = successMessage;

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError(nameof(model.CoverImage), result.ErrorMessage);

                return await InvalidCreateUpdateView(model);
            }
        }

        private async Task<IActionResult> InvalidCreateUpdateView(InventoryCreateUpdateViewModel model)
        {
            IEnumerable<ReferenceDataItem> referenceDataItemDtos = await referenceDataService.GetAllReferenceDataAsync();

            model.AddReferenceData(referenceDataItemDtos);

            return View("CreateUpdate", model);
        }
    }
}