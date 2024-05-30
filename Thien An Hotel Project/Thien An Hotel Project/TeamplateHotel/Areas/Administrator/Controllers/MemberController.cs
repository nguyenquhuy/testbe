using ProjectLibrary.Config;
using ProjectLibrary.Database;
using ProjectLibrary.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeamplateHotel.Areas.Administrator.EntityModel;

namespace TeamplateHotel.Areas.Administrator.Controllers
{
    public class MemberController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Messages = CommentController.Messages(TempData["Messages"]);
            ViewBag.Title = "Trang Member";
            return View();
        }

        [HttpPost]
        public JsonResult List(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                using (var db = new MyDbDataContext())
                {
                    List<EMember> records = db.Members.Select(a => new EMember
                    {
                        Id = a.ID,
                        Name = a.Name,
                        Description = a.Description,
                        PhoneNumber = a.PhoneNumber,
                        Email = a.Email,
                        Image = a.Image
                    }).ToList();

                    // Return result to jTable
                    return Json(new { Result = "OK", Records = records, TotalRecordCount = records.Count });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", message = ex.Message });
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Title = "Thêm Member";
            var eMember  = new EMember();
            return View(eMember);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(EMember model)
        {
            using (var db = new MyDbDataContext())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var member = new Member
                        {
                            Name = model.Name,
                            Description = model.Description,
                            Image = model.Image,
                            PhoneNumber = model.PhoneNumber,
                            Email = model.Email,                            
                        };

                        db.Members.InsertOnSubmit(member);
                        db.SubmitChanges();


                        TempData["Messages"] = "Thêm phòng thành công.";
                        return RedirectToAction("Index");
                    }
                    catch (Exception exception)
                    {
                        ViewBag.Messages = "Error: " + exception.Message;
                        return View(model);
                    }
                }
                return View(model);
            }
        }

    }
}
