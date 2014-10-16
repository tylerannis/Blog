using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyBlog.Models;
using System.IO;
namespace MyBlog.Controllers
{
    public class PostsController : Controller
    {
        private BlogEntities db = new BlogEntities();

        //
        // GET: /Posts/

        public ActionResult Index()
        {
            var posts = db.Posts.Include(p => p.Author);
            return View(posts.ToList());
        }

        //
        // GET: /Posts/Details/5

        public ActionResult Details(int id = 0)
        {
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        //
        // GET: /Posts/Create

        public ActionResult Create()
        {
            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "Name");
            return View();
        }

        //
        // POST: /Posts/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Post post, HttpPostedFileBase ImageUrl)
        {
            if (ModelState.IsValid)
            {
                if (ImageUrl != null)
                {
                    //Guid characters used to make sure the file name is not repeated so that the file  is not overwritten
                string fileName = Guid.NewGuid().ToString().Substring(0, 6) + ImageUrl.FileName;
                //specify the path to save the file to
                string path = Path.Combine(Server.MapPath("~/content/"), fileName);
                //Save the file
                ImageUrl.SaveAs(path);
                //update our post object with the image
                post.ImageUrl = "/content/" + fileName;
                }
                post.Likes = 0;
                post.DateCreated = DateTime.Now;
                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "Name", post.AuthorID);
            return View(post);
        }

        //
        // GET: /Posts/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "Name", post.AuthorID);
            return View(post);
        }

        //
        // POST: /Posts/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Post post, HttpPostedFileBase ImageUrl)
        {
            if (ModelState.IsValid)
            {
                if (ImageUrl != null)
                {
                    //Guid characters used to make sure the file name is not repeated so that the file  is not overwritten
                    string fileName = Guid.NewGuid().ToString().Substring(0, 6) + ImageUrl.FileName;
                    //specify the path to save the file to
                    string path = Path.Combine(Server.MapPath("~/content/"), fileName);
                    //Save the file
                    ImageUrl.SaveAs(path);
                    //update our post object with the image
                    post.ImageUrl = "/content/" + fileName;
                }
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "Name", post.AuthorID);
            return View(post);
        }

        //
        // GET: /Posts/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        //
        // POST: /Posts/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}