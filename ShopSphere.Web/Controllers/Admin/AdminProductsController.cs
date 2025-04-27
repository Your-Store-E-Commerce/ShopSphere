using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopSphere.Data.Entities.Data;
using ShopSphere.Services.Interfaces;
using ShopSphere.Web.Helper;
using ShopSphere.Web.Models.Product;

namespace ShopSphere.Web.Controllers.Admin
{
    public class AdminProductsController : Controller
    {
        private readonly IProductsServices _productsServices;
        private readonly IMapper _mapper;

        public AdminProductsController(IProductsServices productsServices, IMapper mapper)
        {
            _productsServices = productsServices;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET: AdminProducts/Create
        public async Task<IActionResult> Create()
        {
            var model = new ProductFormViewModel();
            await PrepareProductFormViewModelAsync(model);
            return View(model);
        }

        // POST: AdminProducts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = _mapper.Map<Product>(model);

                //// Handle image upload (if applicable)
                if (model.ImageFile != null)
                {
                    string fileName = DocumentSetting.UploadFiles(model.ImageFile, "images/product");
                    product.PictureUrl = fileName;
                }

                await _productsServices.CreateAsync(product);
                return RedirectToAction(nameof(Index));
            }

            await PrepareProductFormViewModelAsync(model);
            return View(model);
        }

        // GET: AdminProducts/Edit
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productsServices.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            var model = _mapper.Map<ProductFormViewModel>(product);
            await PrepareProductFormViewModelAsync(model);

            return View(model);
        }

        // POST: AdminProducts/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingProduct = await _productsServices.GetProductByIdAsync(model.Id);

                if (existingProduct == null)
                    return NotFound();

                var product = _mapper.Map<Product>(model);

                ////// Handle image upload (if applicable)

                if (model.ImageFile != null)
                {
                    DocumentSetting.DeleteFile(existingProduct.PictureUrl, "images/product");

                    string fileName = DocumentSetting.UploadFiles(model.ImageFile, "images/product");
                    product.PictureUrl = fileName;
                }
                else

                    // لو مفيش صورة جديدة، نخلي الصورة القديمة زي ما هي
                    product.PictureUrl = existingProduct.PictureUrl;

                await _productsServices.UpdateAsync(product.Id, product);
                return RedirectToAction(nameof(Index));

            }

            await PrepareProductFormViewModelAsync(model);
            return View(model);
        }

        private async Task PrepareProductFormViewModelAsync(ProductFormViewModel model)
        {
            // جلب البراندات والأنواع من الـ service
            var brands = await _productsServices.GetBrandsAsync();
            var types = await _productsServices.GetTypesAsync();

            // إنشاء SelectList للبراندات والأنواع
            model.Brands = new SelectList(brands, "Id", "Name");
            model.Types = new SelectList(types, "Id", "Name");
        }
    }
}



