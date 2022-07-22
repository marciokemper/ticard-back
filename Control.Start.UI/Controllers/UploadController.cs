using Control.Contas.AppServices.Interfaces;
using Control.Contas.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;

namespace Control.Contas.UI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UploadController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;

        public UploadController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost, DisableRequestSizeLimit]
        public ActionResult UploadFile()
        {
            try
            {

                var _file = Request.Form.Files[0];
                string folderName = "uploadtmp";
                string webRootPath = _hostingEnvironment.WebRootPath;
                string newPath = Path.Combine(webRootPath, folderName);
                string fileName = string.Empty;
                string fullPath = string.Empty;

                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (_file.Length > 0)
                {
                    fileName = ContentDispositionHeaderValue.Parse(_file.ContentDisposition).FileName.Trim('"');
                    fullPath = Path.Combine(newPath, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        _file.CopyTo(stream);

                    }
                }

                TryToDelete(fullPath);

                return Json("Upload realizado com sucesso.");
            }
            catch (System.Exception ex)
            {
                return Json("Upload falhou: " + ex.Message);
            }
        }



        private bool TryToDelete(string f)
        {
            try
            {
                System.IO.File.Delete(f);
                return true;
            }
            catch (IOException)
            {
                // B.
                // We could not delete the file.
                return false;
            }
        }

        public class GenericResult
        {
            public string[] Errors { get; set; }
            public bool Success { get; set; }
        }

        public class GenericResult<TResult> : GenericResult
        {
            public TResult Result { get; set; }
        }



    }
}