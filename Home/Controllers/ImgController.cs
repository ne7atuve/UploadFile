using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Home.Models;
using Home.Controllers;

namespace Home.Controllers
{
    public class ImgController : Controller
    {
        private readonly IWebHostEnvironment _iweb;
        
        public ImgController(IWebHostEnvironment iweb)
        {
            _iweb = iweb;
        }



        public IActionResult Index()
        {
            ImageClass ic = new ImageClass();
            var displayimg = Path.Combine(_iweb.WebRootPath, "UploadFile");
            DirectoryInfo di = new DirectoryInfo(displayimg);
            FileInfo[] fileinfo = di.GetFiles();
            ic.Fileimage = fileinfo;
            return View(ic);
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile imgfile)
        {
            string ext = Path.GetExtension(imgfile.FileName);
            if (ext == ".jpg" || ext == ".JPG" || ext == ".pdf" || ext == ".mp3" || ext == ".docx" || ext == ".doc")
            {
                var imgsave = Path.Combine(_iweb.WebRootPath, "UploadFile", imgfile.FileName);
                var stream = new FileStream(imgsave, FileMode.Create);
                await imgfile.CopyToAsync(stream);
                stream.Close();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(string imgdel)
        {
            imgdel = Path.Combine(_iweb.WebRootPath, "UploadFile", imgdel);
            FileInfo fi = new FileInfo(imgdel);
            if (fi != null)
            {
                System.IO.File.Delete(imgdel);
            }
            return RedirectToAction("Index");
        }

        public VirtualFileResult Download(string imgdel)
        {
            var filepath = Path.Combine("~/UploadFile/", imgdel);
            return File(filepath, "text/plain", imgdel);
        }



    }
}