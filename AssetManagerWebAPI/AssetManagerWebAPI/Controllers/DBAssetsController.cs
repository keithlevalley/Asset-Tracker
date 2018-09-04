using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AssetManagerWebAPI.Models;

namespace AssetManagerWebAPI.Controllers
{
    [RequireHttps]
    public class DBAssetsController : Controller
    {
        private DBAssetModel db = new DBAssetModel();

        // GET: DBAssets
        public ActionResult Index(string SearchString, string sortBy, string sortOrder = "asc")
        {
            // ViewBag allows index to pass in params when calling from HTML
            ViewBag.NameSortParm = sortBy;
            ViewBag.SearchParam = SearchString;
            ViewBag.SearchOrder = sortOrder;

            var assets = db.DBAssets.Where(d => !d.Deleted);

            if (!String.IsNullOrEmpty(SearchString))
            {
                assets = assets.Where(s => s.Name.Contains(SearchString) ||
                s.SerialNumber.Contains(SearchString) ||
                s.Model.Contains(SearchString));
            }

            // sort by ascending
            if (sortOrder == "asc")
            {
                switch (sortBy)
                {
                    case "name":
                        assets = assets.OrderBy(s => s.Name);
                        break;
                    case "serialNumber":
                        assets = assets.OrderBy(s => s.SerialNumber);
                        break;
                    case "lastCheckin":
                        assets = assets.OrderBy(s => s.LastCheckin);
                        break;
                    case "model":
                        assets = assets.OrderBy(s => s.Model);
                        break;
                    case "storage":
                        assets = assets.OrderBy(s => s.Storage);
                        break;
                    case "managed":
                        assets = assets.OrderBy(s => s.Managed);
                        break;
                    default:
                        assets = assets.OrderBy(s => s.Name);
                        break;
                }
            }
            // sort by descending
            else
            {
                switch (sortBy)
                {
                    case "name":
                        assets = assets.OrderByDescending(s => s.Name);
                        break;
                    case "serialNumber":
                        assets = assets.OrderByDescending(s => s.SerialNumber);
                        break;
                    case "lastCheckin":
                        assets = assets.OrderByDescending(s => s.LastCheckin);
                        break;
                    case "model":
                        assets = assets.OrderByDescending(s => s.Model);
                        break;
                    case "storage":
                        assets = assets.OrderByDescending(s => s.Storage);
                        break;
                    case "managed":
                        assets = assets.OrderByDescending(s => s.Managed);
                        break;
                    default:
                        assets = assets.OrderByDescending(s => s.Name);
                        break;
                }
            }
            
            return View(assets);
        }
        
        public ActionResult MissedCheckin(int timeSinceCheckin = 7)
        {
            var returnList = new List<DBAsset>();

            var assets = db.DBAssets.Where(d => !d.Deleted &&
                d.Managed &&
                !d.Storage);

            foreach (var asset in assets)
            {
                if (asset.LastCheckin.AddDays(timeSinceCheckin) < DateTime.Now)
                    returnList.Add(asset);
            }

            return View(returnList);
        }

        public async Task<ActionResult> DeletedMachines()
        {
            var assets = db.DBAssets.Where(d => d.Deleted);

            return View(await assets.ToListAsync());
        }

        public ActionResult Comments(string id)
        {
            ViewBag.AssetID = id;
            //var commentList = new List<DBComment>();

            var comments = db.DBComments.Where(c => c.DBAssetID == id).OrderBy(o => o.DateAdded);
            
            return View(comments);
        }

        public async Task<ActionResult> AddComment(string id, string comment)
        {
            var _asset = db.DBAssets.FindAsync(id);

            await _asset.Result.AddComment(comment, User.Identity.Name);

            return RedirectToAction("Comments", new { id });
        }

        /*
         Asset Dump is meant to be an API that simply returns all
         DBAssets in a JSON string.  This allows the data to be used
         by an external program, such as PowerShell, for advanced
         queries, sorting, export to CSV files, etc.

         I placed the function in DBAssets controller rather than the
         PublicController to make securing access to this function easier.
        */
        public string AssetDump ()
        {
            var assets = db.DBAssets.Where(d => !d.Deleted);

            var json = new JavaScriptSerializer().Serialize(assets);

            return json;
        }

        // GET: DBAssets/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DBAsset dBAsset = await db.DBAssets.FindAsync(id);
            if (dBAsset == null)
            {
                return HttpNotFound();
            }
            return View(dBAsset);
        }

        // GET: DBAssets/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DBAssets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SerialNumber,LastCheckin,CreationDate,Managed,Storage,Deleted,Name,Model,MACAddress,IpAddress")] DBAsset dBAsset)
        {
            if (ModelState.IsValid)
            {
                db.DBAssets.Add(dBAsset);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(dBAsset);
        }

        // GET: DBAssets/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dBAsset = await db.DBAssets.FindAsync(id);
            if (dBAsset == null)
            {
                return HttpNotFound();
            }
            return View(dBAsset);
        }

        // POST: DBAssets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SerialNumber,LastCheckin,CreationDate,Managed,Storage,Deleted,Name,Model,MACAddress,IpAddress")] DBAsset dBAsset)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dBAsset).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(dBAsset);
        }

        // GET: DBAssets/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dBAsset = await db.DBAssets.FindAsync(id);
            if (dBAsset == null)
            {
                return HttpNotFound();
            }
            return View(dBAsset);
        }

        // Delete does not actually remove the object from the Database
            // To remove an object you will need to use Purge
        // POST: DBAssets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var dBAsset = await db.DBAssets.FindAsync(id);
            dBAsset.Deleted = true;
            //db.DBAssets.Remove(dBAsset);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // Physically removes the object from the Database
        // GET: DBAssets/Purge/5
        public async Task<ActionResult> Purge(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Remove any comments attached to the DBAsset being deleted
            var comments = db.DBComments.Where(c => c.DBAssetID == id);

            if (comments != null)
            {
                db.DBComments.RemoveRange(comments);
                await db.SaveChangesAsync();
            }

            var dBAsset = await db.DBAssets.FindAsync(id);

            if (dBAsset == null)
            {
                return HttpNotFound();
            }

            db.DBAssets.Remove(dBAsset);

            await db.SaveChangesAsync();

            return RedirectToAction("DeletedMachines");
        }

        [HttpPost]
        public async Task<int> DeleteList([System.Web.Http.FromBody]string[] id)
        {
            foreach (var asset in id)
            {
                var dBAsset = await db.DBAssets.FindAsync(asset);

                if (dBAsset != null)
                    dBAsset.Deleted = true;
            }

            return await db.SaveChangesAsync();
        }

        // Physically removes the object from the Database
        // GET: DBAssets/Undelete/5
        public async Task<ActionResult> Undelete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var dBAsset = await db.DBAssets.FindAsync(id);

            if (dBAsset == null)
            {
                return HttpNotFound();
            }

            dBAsset.Deleted = false;

            await db.SaveChangesAsync();

            return RedirectToAction("DeletedMachines");
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
