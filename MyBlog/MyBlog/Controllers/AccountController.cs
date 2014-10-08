using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO; //import namespace for path
using System.Web.Security;

namespace MyBlog.Controllers
{
    public class AccountController : Controller
    {
        //Set up my data context
        Models.BlogEntities db = new Models.BlogEntities();

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        //add an httpPostedFileBase parameter
        [HttpPost]
        public ActionResult Register(Models.Registration register, HttpPostedFileBase Image)
        {
            if (Image != null)
            {
                //save the image to the website
                //Guid characters used to make sure the file name is not repeated so that the file  is not overwritten
                string fileName = Guid.NewGuid().ToString().Substring(0, 6) + Image.FileName;
                //specify the path to save the file to
                string path = Path.Combine(Server.MapPath("~/content/"), fileName);
                //Save the file
                Image.SaveAs(path);
                //update our registration object with the image
                register.Image = "/content/" + fileName;
            }
            //create our Membership user
            Membership.CreateUser(register.UserName, register.Password);
            //create our Author object
            Models.Author author = new Models.Author();
            
            author.Name = register.Name;
            author.ImageUrl = register.Image;
            author.UserName = register.UserName;
            
            //add the author to the database
            db.Authors.Add(author);
            db.SaveChanges();

            //registration complete Log in the user
            FormsAuthentication.SetAuthCookie(register.UserName, false);

            return RedirectToAction("Index", "Posts");
        }

        public ActionResult LogOut()
        {
            //to log out a user do this.....


            FormsAuthentication.SignOut();

            return RedirectToAction("Login", "Account");
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Models.Login login)
        {
            //see if they are a valid user
            if (Membership.ValidateUser(login.UserName, login.Password))
            {
                //credentials are gold so log them in
                FormsAuthentication.SetAuthCookie(login.UserName, false);
                //kick the user to the create post page
                return RedirectToAction("Index", "Posts");
            }
            else
            {
                //bad password or username
                ViewBag.ErrorMessage = "That's not your UserName or Password";
                return View(login);
            }
        }

    }
}
