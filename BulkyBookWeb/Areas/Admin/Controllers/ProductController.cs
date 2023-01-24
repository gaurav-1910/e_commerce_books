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

    public ProductController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    [HttpGet]
    public IActionResult Index()
    {
        IEnumerable<CoverType> objCoverTypeList = _unitOfWork.CoverType.GetAll();
        return View(objCoverTypeList);
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
            //update product
        }
        
        return View(productVM);
    }
    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(CoverType _coverType)
    {
       
        if (ModelState.IsValid)
        {
            _unitOfWork.CoverType.Update(_coverType);
            _unitOfWork.Save();
            TempData["success"] = "CoverType Updated Successfully";
            return RedirectToAction("Index");
        }
        return View(_coverType);
    }
    // GET
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        //var deleteCategoryFromDb = _db.Categories.Find(id);
        var coverTypeFromDbSingle = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
        //var categoryToDbSingle = _db.Categories.SingleOrDefault(u=>u.Id == id);
        if (coverTypeFromDbSingle == null)
        {
            return NotFound();
        }
        return View(coverTypeFromDbSingle);
    }
    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePost(int? id)
    {
        var deleteId = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
        if (deleteId == null)
        {
            return NotFound();
        }

        _unitOfWork.CoverType.Remove(deleteId);
        _unitOfWork.Save();
        TempData["success"] = "CoverType Deleted Successfully";
        return RedirectToAction("Index");
    }
}
