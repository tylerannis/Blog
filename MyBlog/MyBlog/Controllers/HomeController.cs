using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyBlog.Controllers
{
    public class HomeController : Controller
    {
        Models.BlogEntities db = new Models.BlogEntities();
        //
        // GET: /Home/
        
        public ActionResult Index()
        {
            
            return View(db.Posts.OrderByDescending(x => x.DateCreated));
        }
        public ActionResult AddComment(Models.Comment commentToAdd)
        {
            //make sure the comment is fully filled out
            commentToAdd.DateCreated = DateTime.Now;
            
            //add the comment
            db.Comments.Add(commentToAdd);
            //save cahnges
            db.SaveChanges();
            //for now until we ajax it we'll kick the user back to the index
            //return RedirectToAction("Index", "Home");
            //ajax'd the post so we will return a partail view
            return PartialView("Comment", commentToAdd);
        }
        public ActionResult LikePost(int id)
        {
            Models.Post thePost = db.Posts.Find(id);
            thePost.Likes++;
            db.SaveChanges();
            return Content(thePost.Likes + " likes");
        }

    }
}
