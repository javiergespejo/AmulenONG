using ABM.Business_Logic;
using ABM.Filters;
using ABM.Repository;
using ABM.ViewModels;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABM.Controllers
{
    public class FileController : Controller
    {
        const int administrador = 1;

        private readonly FileRepository _fileRepository;

        public FileController()
        {
            _fileRepository = new FileRepository();
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            try
            {
                var files = from i in _fileRepository.GetAll()
                            select new ImportantFileViewModel()
                            {
                                Id = i.id,
                                Description = i.description,
                                EditDate = i.editDate,
                                UserId = i.UserId
                            };

                return View(files.ToList());
            }
            catch (Exception)
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        [AuthorizeUser(new int[] { administrador })]
        public ActionResult UploadFile()
        {
            return View();
        }

        //POST: Home/UploadImage
        [AuthorizeUser(new int[] { administrador })]
        [HttpPost]
        [Route("~/File/UploadFile")]
        public ActionResult UploadFile(ImportantFileViewModel model)
        {
            try
            {
                HttpPostedFileBase file = Request.Files["FileData"];
                if (ValidateFile.ValidImportantFileExtension(file))
                {
                    if (ValidateFile.ValidateFileSize(file))
                    {
                        var user = (Models.User)Session["User"];
                        model.UserId = user.id;
                        bool isUploaded = _fileRepository.UploadFileInDataBase(file, model.ToEntity());
                        if (isUploaded)
                        {
                            TempData["SucessMessage"] = "El archivo se ha guardado correctamente!";
                            return RedirectToAction("UploadFile", "File");
                        }
                        throw new Exception("Hubo un error al cargar la imagen.");
                    }
                    throw new Exception("El archivo supera el tamaño maximo permitido de 5 MB.");
                }
                throw new Exception("El formato del archivo debe ser pdf.");
            }
            catch (Exception e)
            {
                TempData["Error"] = e.Message;
                return RedirectToAction("UploadFile", "File");
            }

        }

        [AllowAnonymous]
        public ActionResult Delete(int id)
        {
            try
            {
                _fileRepository.Delete(id);
                TempData["SucessMessage"] = "El archivo se eliminó correctamente";
                return RedirectToAction("UploadFile", "File");
            }
            catch (Exception)
            {
                TempData["Error"] = "Hubo un problema al eliminar el archivo.";
                return RedirectToAction("UploadFile", "File");
            }
        }

        [AllowAnonymous]
        public ActionResult Download(int id)
        {
            try
            {
                var file = _fileRepository.GetById(id);
                byte[] cover = file.fileData;
                string description = file.description + ".pdf";
                string fileName = description.Replace(' ', '_');

                if (cover != null)
                {
                    return File(cover, "text/plain", fileName);
                }

                return RedirectToAction("NotFound", "Error");

            }
            catch (Exception)
            {
                TempData["Error"] = "El archivo solicitado no se encuentra disponible";
                return RedirectToAction("UploadFile");
            }
        }
    }
}