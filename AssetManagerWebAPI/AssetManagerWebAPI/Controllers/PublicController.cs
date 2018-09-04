using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using AssetLibrary;
using AssetManagerWebAPI.Models;
using Newtonsoft.Json;

namespace AssetManagerWebAPI.Controllers
{
    public class PublicController : ApiController
    {
        [HttpPost]
        [Route("api/public/CheckIn")]
        public void CheckIn(Asset asset)
        {
            if (asset.SerialNumber == "" || asset.SerialNumber == null)
                return;

            using (DBAssetModel ctx = new DBAssetModel())
            {
                var _dbAsset = ctx.DBAssets.Find(asset.SerialNumber);

                if (_dbAsset == null)
                {
                    var dbAsset = new DBAsset(asset);
                    dbAsset.Create();
                    return;
                }

                if (_dbAsset.Deleted == true)
                {
                    _dbAsset.Deleted = false;
                }

                if (_dbAsset.Storage == true)
                {
                    _dbAsset.Storage = false;
                }

                if (_dbAsset.Name != asset.Name)
                {
                    _dbAsset.Name = asset.Name;
                }

                if (_dbAsset.IpAddress != asset.IpAddress)
                {
                    _dbAsset.IpAddress = asset.IpAddress;
                }

                _dbAsset.LastCheckin = DateTime.Now;

                ctx.SaveChanges();
            }
        }
    }
}
