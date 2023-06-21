using BulkyBookDataAccess.Repository.IRepository;
using BulkyBookModels;
using BulkyBookModels.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers;
[Area("Admin")]
public class ProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
    {
        _unitOfWork = unitOfWork;
        _webHostEnvironment = webHostEnvironment;
    }
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
    //GET
    //public IActionResult Create()
    //{

    //    return View();
    //}
    ////POST
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public IActionResult Create(CoverType _coverTypes)
    //{
    //    //if (_coverTypes.Name == _coverTypes.DisplayOrder.ToString())
    //    //{
    //    //    ModelState.AddModelError("name", "The DisplayOrder Cannot exactly match the Name.");
    //    //}
    //    if (ModelState.IsValid)
    //    {
    //        _unitOfWork.CoverType.Add(_coverTypes);
    //        _unitOfWork.Save();
    //        TempData["success"] = "CoverType Created Successfully";
    //        return RedirectToAction("Index");
    //    }
    //    return View(_coverTypes);
    //}

    //GET
    public IActionResult Upsert(int? id)
    {
        ProductVM productVM = new()
        {
            product = new(),
            CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            }),
            CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            }),

        };

        if (id == null || id == 0)
        {
            //create product
            //ViewBag.CategoryList = CategoryList;
            //ViewData["CoverTypeList"] = CoverTypeList;
            return View(productVM);
        }
        else 
        {
            productVM.product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
            return View(productVM);
        }
        
        
    }
    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(ProductVM _productView, IFormFile? file)
    {
       
        if (ModelState.IsValid)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (file != null) 
            {
                string fileName = Guid.NewGuid().ToString();
                var uploads = Path.Combine(wwwRootPath, @"images\products");
                var extension = Path.GetExtension(file.FileName);

                if (_productView.product.ImageUrl != null) 
                {
                    var oldImagePath = Path.Combine(wwwRootPath, _productView.product.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath)) 
                    {
                        System.IO.File.Exists(oldImagePath);
                    }
                }
                using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create)) 
                {
                    file.CopyTo(fileStreams);
                }
                _productView.product.ImageUrl = @"\images\products\" + fileName + extension;
            }

            if (_productView.product.Id == 0)
            {
                _unitOfWork.Product.Add(_productView.product);
            }
            else 
            {
                _unitOfWork.Product.Update(_productView.product);
            }
            _unitOfWork.Save();
            TempData["success"] = "Product Added Successfully";
            return RedirectToAction("Index");
        }
        return View(_productView);
    }
    //// GET
    //public IActionResult Delete(int? id)
    //{
    //    if (id == null || id == 0)
    //    {
    //        return NotFound();
    //    }
    //    //var deleteCategoryFromDb = _db.Categories.Find(id);
    //    var coverTypeFromDbSingle = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
    //    //var categoryToDbSingle = _db.Categories.SingleOrDefault(u=>u.Id == id);
    //    if (coverTypeFromDbSingle == null)
    //    {
    //        return NotFound();
    //    }
    //    return View(coverTypeFromDbSingle);
    //}
    ////POST
    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public IActionResult DeletePost(int? id)
    //{
    //    var deleteId = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
    //    if (deleteId == null)
    //    {
    //        return NotFound();
    //    }

    //    _unitOfWork.CoverType.Remove(deleteId);
    //    _unitOfWork.Save();
    //    TempData["success"] = "CoverType Deleted Successfully";
    //    return RedirectToAction("Index");
    //}

    #region  API CALLS 

    [HttpGet]
    public IActionResult GetAll()
    {
        var productList = _unitOfWork.Product.GetAll(includeProperties:"Category,CoverType");
        return Json(new { data = productList });
    }
    [HttpDelete]
   
    public IActionResult DeletePOST(int? id)
    {
        var deleteId = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
        if (deleteId == null)
        {
            return Json(new { success = false, message = "Error while deleting" });
        }
        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, deleteId.ImageUrl.TrimStart('\\'));
        if (System.IO.File.Exists(oldImagePath))
        {
            System.IO.File.Exists(oldImagePath);
        }

        _unitOfWork.Product.Remove(deleteId);
        _unitOfWork.Save();
        return Json(new { success = true, message = "Delete Successful" });
       
    }

    #endregion


}
