using ABM.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ABM.Repository
{
    public class FileRepository
    {
        private readonly UnitOfWork unitOfWork = new UnitOfWork();
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ImportantFile> GetAll()
        {
            try
            {
                return unitOfWork.FileRepository.Get();
            }
            catch (Exception)
            {
                return Enumerable.Empty<ImportantFile>();
            }

        }

        // Insert file in database as byte array
        public bool UploadFileInDataBase(HttpPostedFileBase file, ImportantFile importantFile)
        {
            try
            {
                importantFile.fileData = ConvertToBytes(file);
                int fileNotSelected = importantFile.fileData.Length;
                if (fileNotSelected >= 1)
                {
                    unitOfWork.FileRepository.Insert(importantFile);
                    unitOfWork.Save();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Recieve image from uploadImage view and convert it in byte array.
        public byte[] ConvertToBytes(HttpPostedFileBase file)
        {
            try
            {
                byte[] fileBytes;
                BinaryReader reader = new BinaryReader(file.InputStream);
                fileBytes = reader.ReadBytes((int)file.ContentLength);
                return fileBytes;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public void DeleteFile(int id)
        {
            unitOfWork.FileRepository.Delete(id);
            unitOfWork.Save();
        }

        public ImportantFile GetById(int Id)
        {
            var cover = from f in GetAll()
                        where f.id == Id
                        select f;
            return cover.First();
        }

        public void Delete(int id)
        {
            unitOfWork.FileRepository.Delete(id);
            unitOfWork.Save();
        }

    }
}