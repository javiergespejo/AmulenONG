using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABM.Business_Logic
{
    public class ValidateFile
    {
        /// <summary>
        /// Validates that the file ends with .jpg, .jpeg and png.
        /// </summary>
        /// <param name="file">File to validate</param>
        /// <param name="extensions">string of extension, separated by ',' (DEFUALT png, jpg, jpeg)</param>
        /// <returns>True if the file extension's is valid, else false</returns>
        public static bool ValidFileExtension(HttpPostedFileBase file, string extensions = "png,jpg,jpeg")
        {
            bool isValid = false;
            List<string> allowedExtensions = extensions.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (file != null)
            {
                var fileName = file.FileName.ToLower();
                isValid = allowedExtensions.Any(y => fileName.EndsWith(y));
            }
            return isValid;
        }
        /// <summary>
        /// Validates the file size
        /// </summary>
        /// <param name="file"> File to validate</param>
        /// <param name="allowedSize"> Max size for the file, in bytes.(DEFAULT 5 MB)</param>
        /// <returns></returns>
        public static bool ValidateFileSize(HttpPostedFileBase file, int allowedSize = 5242880)
        {
            bool isValid = false;
            if (file != null)
            {
                var fileSize = file.ContentLength;
                isValid = fileSize <= allowedSize;
            }
            return isValid;
        }

        public static bool ValidImportantFileExtension(HttpPostedFileBase file, string extensions = "pdf,doc,docx")
        {
            bool isValid = false;
            List<string> allowedExtensions = extensions.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (file != null)
            {
                var fileName = file.FileName.ToLower();
                isValid = allowedExtensions.Any(y => fileName.EndsWith(y));
            }
            return isValid;
        }

    }
}
