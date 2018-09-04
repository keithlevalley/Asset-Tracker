namespace AssetManagerWebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using AssetLibrary;

    public class DBAssetModel : DbContext
    {
        // Your context has been configured to use a 'Model1' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'AssetManagerWebAPI.Models.Model1' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'Model1' 
        // connection string in the application configuration file.
        public DBAssetModel()
            : base("name=DBAssetModel")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<DBAsset> DBAssets { get; set; }
        public virtual DbSet<DBComment> DBComments { get; set; }
    }

    public class DBComment
    {
        public int DBCommentID { get; set; }

        [ForeignKey("DBAsset")]
        public string DBAssetID { get; set; }
        public DBAsset DBAsset { get; set; }

        public string AddedBy { get; set; }
        public DateTime DateAdded { get; set; }
        public string Comment { get; set; }

        public bool AddComment ()
        {
            using (DBAssetModel ctx = new DBAssetModel())
            {
                ctx.DBComments.Add(this);

                if (ctx.SaveChanges() == 1)
                    return true;

                else
                    return false;
            }
        }
    }

    public class DBAsset : Asset
    {
        public ICollection<DBComment> Comments { get; set; }
        public DateTime LastCheckin { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Managed { get; set; }
        public bool Storage { get; set; }
        public bool Deleted { get; set; }

        public DBAsset() { }

        public DBAsset (Asset asset)
        {
            this.SerialNumber = asset.SerialNumber;
            this.Name = asset.Name;
            this.Model = asset.Model;
            this.MACAddress = asset.MACAddress;
            this.IpAddress = asset.IpAddress;
        }

        public bool Create()
        {
            this.LastCheckin = DateTime.Now;
            this.CreationDate = DateTime.Now;
            this.Managed = true;
            this.Storage = false;
            this.Deleted = false;

            using (DBAssetModel ctx = new DBAssetModel())
            {
                ctx.DBAssets.Add(this);

                if (ctx.SaveChanges() == 1)
                    return true;

                else
                    return false;
            }
        }

        // TODO:  Change this so dynamically chooses query parameter rather than if else
        /*
        public IEnumerable<Asset> Read(IEnumerable<string> keyValues, char key)
        {
            var asset = new Asset();

            asset.Deleted = false;
            // Key: Serial Number = S, Name = N
            List<DBAsset> result = null;

            if (key == 'S')
            {
                foreach (var value in keyValues)
                {
                    string tempValue = value;
                    if (tempValue.StartsWith("*"))
                    {
                        using (AssetModel ctx = new AssetModel())
                        {
                            ctx.DBAssets.Where(x => x.Deleted);
                            var test = ctx.DBAssets.Find('5');
                            test.Deleted = false;
                            result.AddRange(ctx.DBAssets.Where(x => x.SerialNumber.StartsWith(value) && x.Deleted == false));
                        }
                    }
                    else if (tempValue.EndsWith("*"))
                    {
                        using (AssetModel ctx = new AssetModel())
                        {
                            result.AddRange(ctx.DBAssets.Where(x => x.SerialNumber.EndsWith(value) && x.Deleted == false));
                        }
                    }
                    else if (tempValue.Contains("*"))
                    {
                        string[] tempArray = new string[2];
                        tempArray[0] = tempValue.Substring(0, tempValue.IndexOf('*'));
                        tempArray[1] = tempValue.Substring(tempValue.IndexOf('*') + 1);

                        using (AssetModel ctx = new AssetModel())
                        {
                            result.AddRange(ctx.DBAssets
                                .Where(x => x.SerialNumber.StartsWith(tempArray[0]) && x.SerialNumber.EndsWith(tempArray[1]) && x.Deleted == false));
                        }
                    }
                    else
                    {
                        using (AssetModel ctx = new AssetModel())
                        {
                            result.AddRange(ctx.DBAssets.Where(x => x.SerialNumber.Contains(value) && x.Deleted == false));
                        }
                    }
                }
            }
            else if (key == 'N')
            {
                foreach (var value in keyValues)
                {
                    string tempValue = value;
                    if (tempValue.StartsWith("*"))
                    {
                        using (AssetModel ctx = new AssetModel())
                        {
                            result.AddRange(ctx.DBAssets.Where(x => x.Name.StartsWith(value) && x.Deleted == false));
                        }
                    }
                    else if (tempValue.EndsWith("*"))
                    {
                        using (AssetModel ctx = new AssetModel())
                        {
                            result.AddRange(ctx.DBAssets.Where(x => x.Name.EndsWith(value) && x.Deleted == false));
                        }
                    }
                    else if (tempValue.Contains("*"))
                    {
                        string[] tempArray = new string[2];
                        tempArray[0] = tempValue.Substring(0, tempValue.IndexOf('*'));
                        tempArray[1] = tempValue.Substring(tempValue.IndexOf('*') + 1);

                        using (AssetModel ctx = new AssetModel())
                        {
                            result.AddRange(ctx.DBAssets
                                .Where(x => x.Name.StartsWith(tempArray[0]) && x.Name.EndsWith(tempArray[1]) && x.Deleted == false));
                        }
                    }
                    else
                    {
                        using (AssetModel ctx = new AssetModel())
                        {
                            result.AddRange(ctx.DBAssets.Where(x => x.Name.Contains(value) && x.Deleted == false));
                        }
                    }
                }
            }
            else
                throw new Exception($"{key} is not a valid search option");

            return result;

            throw new NotImplementedException();
        }
        */

        public bool Delete(bool _delete = true)
        {
            using (DBAssetModel ctx = new DBAssetModel())
            {
                var asset = ctx.DBAssets.Find(this);
                asset.Deleted = _delete;

                if (ctx.SaveChanges() == 1)
                    return true;
                else
                    return false;
            }
        }

        public bool Manage(bool _manage = true)
        {
            using (DBAssetModel ctx = new DBAssetModel())
            {
                var asset = ctx.DBAssets.Find(this);
                asset.Managed = _manage;

                if (ctx.SaveChanges() == 1)
                    return true;
                else
                    return false;
            }
        }

        public bool Store(bool _store = true)
        {
            using (DBAssetModel ctx = new DBAssetModel())
            {
                var asset = ctx.DBAssets.Find(this);
                asset.Storage = _store;

                if (ctx.SaveChanges() == 1)
                    return true;
                else
                    return false;
            }
        }

        public async Task<bool> AddComment(string _comment, string user)
        {
            using (DBAssetModel ctx = new DBAssetModel())
            {
                //var asset = ctx.DBAssets.Find(this);

                var comment = new DBComment();

                comment.DBAssetID = this.SerialNumber;
                comment.Comment = _comment;
                comment.AddedBy = user;
                comment.DateAdded = DateTime.Now;

                ctx.DBComments.Add(comment);

                if (await ctx.SaveChangesAsync() == 1)
                    return true;
                else
                    return false;
            }
        }

        public bool Update()
        {
            using (DBAssetModel ctx = new DBAssetModel())
            {
                var asset = ctx.DBAssets.Find(this);

                if (this.Name != null)
                    asset.Name = this.Name;

                if (this.IpAddress != null)
                    asset.IpAddress = this.IpAddress;

                asset.LastCheckin = DateTime.Now;
                asset.Storage = false;
                asset.Deleted = false;

                if (ctx.SaveChanges() == 1)
                    return true;

                else
                    return false;
            }
        }
    }
}