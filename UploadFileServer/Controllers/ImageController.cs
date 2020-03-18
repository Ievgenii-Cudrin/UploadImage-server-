using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using UploadFileServer.Models;

namespace UploadFileServer.Controllers
{
    public class ImageController : ApiController
    {
        [HttpPost]
        [Route("api/UploadImage")]
        public HttpResponseMessage UploadImage()
        {
            string imageName = null;
            var httpRequest = System.Web.HttpContext.Current.Request;

            //Upload image
            var postedFile = httpRequest.Files["Image"];
            //Createcustom filename
            imageName = new String(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "-");
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(postedFile.FileName);
            string p = new string(HttpContext.Current.Server.MapPath("").Take(75).ToArray());
            int a = HttpContext.Current.Server.MapPath("").Length;
            var filiPathNew = p + "\\Images\\" + imageName;
            var filePath = HttpContext.Current.Server.MapPath("~/Images/" + imageName);
            postedFile.SaveAs(filePath);

            using(Models.DbModel db = new Models.DbModel())
            {
                Image image = new Image()
                {
                    ImageCaption = httpRequest["ImageCaption"],
                    ImageName = imageName
                };
                db.Image.Add(image);
                db.SaveChanges();
            }

            return Request.CreateResponse(HttpStatusCode.Created);
        }
    }
}
