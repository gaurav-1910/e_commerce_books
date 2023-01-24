using BulkyBookDataAccess.Repository.IRepository;
using BulkyBookModels;
using Microsoft.AspNetCore.Mvc;


namespace BulkyBookWeb.Areas.Admin.Controllers;
[Area("Admin")]
public class CoverTypeController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public CoverTypeController(IUnitOfWork unitOfWork)
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
    public IActionResult Create()
    {

        return View();
    }
    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CoverType _coverTypes)
    {
        //if (_coverTypes.Name == _coverTypes.DisplayOrder.ToString())
        //{
        //    ModelState.AddModelError("name", "The DisplayOrder Cannot exactly match the Name.");
        //}
        if (ModelState.IsValid)
        {
            _unitOfWork.CoverType.Add(_coverTypes);
            _unitOfWork.Save();
            TempData["success"] = "CoverType Created Successfully";
            return RedirectToAction("Index");
        }
        return View(_coverTypes);
    }

    //GET
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        //var categoryFromDb = _db.Categories.Find(id);
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
    public IActionResult Edit(CoverType _coverType)
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
