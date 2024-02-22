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
    if (ModelState.IsValid)
    {
      if (_dataContext.Blogs.Any(b => b.Name == model.Name))
      {
        ModelState.AddModelError("", "Name must be unique");
      }
      else
      {
        _dataContext.AddBlog(model);
        return RedirectToAction("Index");
      }
    }
    return View();
  }
  public IActionResult DeleteBlog(int id)
  {
    _dataContext.DeleteBlog(_dataContext.Blogs.FirstOrDefault(b => b.BlogId == id));
    return RedirectToAction("Index");
  }
  public IActionResult BlogDetail(int id) => View(new PostViewModel
  {
    blog = _dataContext.Blogs.FirstOrDefault(b => b.BlogId == id),
    Posts = _dataContext.Posts.Where(p => p.BlogId == id)
  });
  public IActionResult AddPost(int id)
  {
    ViewBag.BlogId = id;
    return View(new Post());
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public IActionResult AddPost(int id, Post post)
  {
    post.BlogId = id;
    if (ModelState.IsValid)
    {
      _dataContext.AddPost(post);
      return RedirectToAction("BlogDetail", new { id = id });
    }
    @ViewBag.BlogId = id;
    return View();
  }
  public IActionResult DeletePost(int id)
  {
    Post post = _dataContext.Posts.FirstOrDefault(p => p.PostId == id);
    int BlogId = post.BlogId;
    _dataContext.DeletePost(post);
    return RedirectToAction("BlogDetail", new { id = BlogId });
  }
}
