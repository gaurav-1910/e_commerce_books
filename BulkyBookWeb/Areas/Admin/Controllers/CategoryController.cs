using BulkyBookDataAccess;
using BulkyBookDataAccess.Repository.IRepository;
using BulkyBookModels;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers;

[Area("Admin")]
public class CategoryController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
        IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll();
        return View(objCategoryList);
    }
    //GET
    public IActionResult Create()
    {

        return View();
    }
    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Category _category)
    {
        if(_category.Name == _category.DisplayOrder.ToString())
        {
            ModelState.AddModelError("name", "The DisplayOrder Cannot exactly match the Name.");
        }
        if (ModelState.IsValid)
        {
            _unitOfWork.Category.Add(_category);
            _unitOfWork.Save();
            TempData["success"] = "Category Created Successfully";
            return RedirectToAction("Index");
        }
        return View(_category);
    }

    //GET
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
         //var categoryFromDb = _db.Categories.Find(id);
        var categoryFromDbSingle = _unitOfWork.Category.GetFirstOrDefault(u=>u.Id == id);
        //var categoryToDbSingle = _db.Categories.SingleOrDefault(u=>u.Id == id);
        if (categoryFromDbSingle == null)
        {
            return NotFound();
        }
        return View(categoryFromDbSingle);
    }
    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Category _category)
    {
        if (_category.Name == _category.DisplayOrder.ToString())
        {
            ModelState.AddModelError("name", "The DisplayOrder Cannot exactly match the Name.");
        }
        if (ModelState.IsValid)
        {
            _unitOfWork.Category.Update(_category);
            _unitOfWork.Save();
            TempData["success"] = "Category Updated Successfully";
            return RedirectToAction("Index");
        }
        return View(_category);
    }
    // GET
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        //var deleteCategoryFromDb = _db.Categories.Find(id);
        var categoryFromDbSingle = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
        //var categoryToDbSingle = _db.Categories.SingleOrDefault(u=>u.Id == id);
        if (categoryFromDbSingle == null)
        {
            return NotFound();
        }
        return View(categoryFromDbSingle);
    }
    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePost(int? id)
    {
        var deleteId = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
        if (deleteId == null)
        {
            return NotFound();
        }

           _unitOfWork.Category.Remove(deleteId);
           _unitOfWork.Save();
           TempData["success"] = "Category Deleted Successfully";
           return RedirectToAction("Index");        
    }
}
