using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClassLibrary1;
using ImageApplication.Models;
using System.IO;

namespace ImageApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            IndexViewModel vm = new IndexViewModel();
            vm.Message = (string)Session["Message"];
            return View(vm);
        }

        public ActionResult Images(int id)
        {
            Manager mgr = new Manager(Properties.Settings.Default.ConStr);
            ImagesViewModel vm = new ImagesViewModel();
            vm.Image = mgr.GetImageById(id);
            vm.Message = (string)TempData["Message"];
            
            if (Session["AllowedId"] != null)
            {
                List<int> ids = (List<int>)Session["AllowedId"];
                if (ids.Contains(id))
                {
                    vm.HasPermission = true;
                    mgr.UpdateView(id);
                }
            }

            return View(vm);
        }

        [HttpPost]
        public ActionResult Images(int id, string password)
        {
            Manager mgr = new Manager(Properties.Settings.Default.ConStr);
            
            Image image = mgr.GetImageById(id);
            if (password != image.Password)
            {
                TempData["Message"] = "Invalid Password";
            }

            else
            {
                List<int> allowedIds;

                if (Session["AllowedId"] == null)
                {
                    allowedIds = new List<int>();
                    Session["AllowedId"] = allowedIds;
                }
                else
                {
                    allowedIds = (List<int>)Session["AllowedId"];
                }
                allowedIds.Add(image.Id);

            }

            return Redirect($"/home/images?id={image.Id}");
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase image, string password)
        {
            string ext = Path.GetExtension(image.FileName);
            string fileName = Guid.NewGuid().ToString() + ext;
            string fullPath = $"{Server.MapPath("/UploadedImages")}\\{fileName}";
            image.SaveAs(fullPath);
            Manager mgr = new Manager(Properties.Settings.Default.ConStr);
            Image i = new Image
            {
                FileName = fileName,
                Password = password,
                Views = 0
            };
            mgr.InsertImage(i);
            Session["Message"] = $"You can share the picture http://localhost:50709/home/images?id={i.Id} And the password of {password}";
            return Redirect("/home/index");
        }

    }
}