using ProjectLibrary.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TeamplateHotel.Controllers
{
    public class MemberViewController : Controller
    {
        //
        // GET: /Member/
        [HttpGet]
        public ActionResult Index()
        {
            using (var db = new MyDbDataContext())
            {
                List<Member> members =  db.Members.ToList();
                
                return View(members);
            }
        }

        public ActionResult Details(int id) {
            using (var db = new MyDbDataContext())
            {
                var member = db.Members.SingleOrDefault(m=>m.ID == id);
                if(member == null)
                {
                    return null;
                }
                else
                {
                    return View(member);
                }
            }
                
        }

    }
}
