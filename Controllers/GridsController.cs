using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Application;
using OfficeOpenXml;

namespace Application.Controllers
{
    public class GridsController : Controller
    {
        private SampleEntities db = new SampleEntities();

        // GET: Grids
        public ActionResult Index()
        {
            return View(db.Grids.ToList());
        }

        public ActionResult Grid(string Search)
        {
            var names = from s in db.Grids
                        select s;
            //if (!String.IsNullOrEmpty(Search))
            //{
            //    names = names.Where(s => s.Id.ToString().Contains(Search)
            //                           || s.username.ToLower().Contains(Search)
            //                           || s.first.ToLower().Contains(Search)
            //                           || s.last.ToLower().Contains(Search)
            //                           || s.gender.ToLower().Contains(Search)
            //                           || s.age.ToString().Contains(Search));
            //}
            ViewBag.S = Search;
            return View(names.ToList());
        }

        public ActionResult Search(string Search)
        {
            var names = from s in db.Grids
                        select s;
            if (!String.IsNullOrEmpty(Search))
            {
                names = names.Where(s => s.Id.ToString().Contains(Search)
                                       || s.username.ToLower().Contains(Search)
                                       || s.first.ToLower().Contains(Search)
                                       || s.last.ToLower().Contains(Search)
                                       || s.gender.ToLower().Contains(Search)
                                       || s.age.ToString().Contains(Search));
            }
            return PartialView("_PartialGrid",names.ToList());
        }

        public void Export()
        {
            var names = (from s in db.Grids
                        select s).ToList();
            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Export");

            ws.Cells["A1"].Value = "Export";

            ws.Cells["A2"].Value = "Date";
            ws.Cells["B2"].Value = string.Format("{0:dd/MM/yyyy}",DateTimeOffset.Now);
            ws.Cells["C2"].Value = string.Format("{0:H:mm tt}", DateTimeOffset.Now);

            ws.Cells["A4"].Value = "Id";
            ws.Cells["B4"].Value = "Username";
            ws.Cells["C4"].Value = "First Name";
            ws.Cells["D4"].Value = "Last Name";
            ws.Cells["E4"].Value = "Gender";
            ws.Cells["F4"].Value = "Age";

            int row = 5;
            foreach(var i in names)
            {
                ws.Cells[string.Format("A{0}", row)].Value = i.Id;
                ws.Cells[string.Format("B{0}", row)].Value = i.username;
                ws.Cells[string.Format("C{0}", row)].Value = i.first;
                ws.Cells[string.Format("D{0}", row)].Value = i.last;
                ws.Cells[string.Format("E{0}", row)].Value = i.gender;
                ws.Cells[string.Format("F{0}", row)].Value = i.age;
                row++;
            }

            ws.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename=" + string.Format("{0:dd-MM-yyyy} at {0:H-mm tt}", DateTimeOffset.Now) + ".xlsx");
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();
        }

        // GET: Grids/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grid grid = db.Grids.Find(id);
            if (grid == null)
            {
                return HttpNotFound();
            }
            return View(grid);
        }

        // GET: Grids/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Grids/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,username,first,last,gender,age")] Grid grid)
        {
            if (ModelState.IsValid)
            {
                db.Grids.Add(grid);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(grid);
        }

        // GET: Grids/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grid grid = db.Grids.Find(id);
            if (grid == null)
            {
                return HttpNotFound();
            }
            return View(grid);
        }

        // POST: Grids/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,username,first,last,gender,age")] Grid grid)
        {
            if (ModelState.IsValid)
            {
                db.Entry(grid).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(grid);
        }

        // GET: Grids/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Grid grid = db.Grids.Find(id);
            if (grid == null)
            {
                return HttpNotFound();
            }
            return View(grid);
        }

        // POST: Grids/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Grid grid = db.Grids.Find(id);
            db.Grids.Remove(grid);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
