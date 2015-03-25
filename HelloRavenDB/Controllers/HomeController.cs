using HelloRavenDB.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace HelloRavenDB.Controllers
{
    public class HomeController : RavenController
    {
        public ActionResult InitSamples()
        {
            var book1 = new Book()
            {
                Author = "Huan",
                Id = Guid.NewGuid(),
                Title = "Book ONE"
            };

            RavenSession.Store(book1);

            return RedirectToAction("Index");
        }

        //
        // GET: /Home/

        public ActionResult Index()
        {
            var books = RavenSession.Query<Book>().ToList();
            return View(books);
        }

    }
}
