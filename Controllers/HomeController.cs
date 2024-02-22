using Microsoft.AspNetCore.Mvc;
using System.Linq;

public class HomeController(DataContext db) : Controller
{
  // this controller depends on the DataContext
  private readonly DataContext _dataContext = db;

  public IActionResult Index() => View(_dataContext.Blogs.OrderBy(b => b.Name));
  public IActionResult AddBlog() => View(new Blog());
  [HttpPost]
  [ValidateAntiForgeryToken]
  public IActionResult AddBlog(Blog model)
  {
    _dataContext.AddBlog(model);
    return RedirectToAction("Index");
  }
}
