using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Locs4Youth.Utils
{
    public class ValidateDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) return true;
            var date = (DateTime)value;

            //-------------------------------------------
            //  Check date not being in the past
            //-------------------------------------------
            if (date < DateTime.Today)
            {
                ErrorMessage = "Datum kan niet in het verleden zijn.";
                return false;
            }

            return true;
        }
    }

    public class ValidateFileAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) return true;
            var postedFile = value as HttpPostedFileBase;

            //-------------------------------------------
            //  Check image size (max 1MB)
            //-------------------------------------------
            if (postedFile.ContentLength > 1 * 1024 * 1024)
            {
                ErrorMessage = "De afbeelding kan maximaal 1MB groot zijn.";
                return false;
            }

            //-------------------------------------------
            //  Check the image mime types
            //-------------------------------------------
            if (postedFile.ContentType.ToLower() != "image/jpg" &&
                        postedFile.ContentType.ToLower() != "image/jpeg" &&
                        postedFile.ContentType.ToLower() != "image/pjpeg" &&
            //             postedFile.ContentType.ToLower() != "image/gif" &&
                        postedFile.ContentType.ToLower() != "image/x-png" &&
                        postedFile.ContentType.ToLower() != "image/png")
            {
                ErrorMessage = "Dit bestand is geen afbeelding.";
                return false;
            }

            //-------------------------------------------
            //  Check the image extension
            //-------------------------------------------
            if (Path.GetExtension(postedFile.FileName).ToLower() != ".jpg"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".png"
            //    && Path.GetExtension(postedFile.FileName).ToLower() != ".gif"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".jpeg")
            {
                ErrorMessage = "Enkel afbeeldingen met de extensies JPG, JPEG of PNG zijn toegelaten.";
                return false;
            }

            //-------------------------------------------
            //  Attempt to read the file and check the first bytes
            //-------------------------------------------
            string msg = "Ongeldig bestand.";
            try
            {
                if (!postedFile.InputStream.CanRead)
                {
                    ErrorMessage = msg;
                    return false;
                }

                if (postedFile.ContentLength < 512)
                {
                    ErrorMessage = msg;
                    return false;
                }

                byte[] buffer = new byte[512];
                postedFile.InputStream.Read(buffer, 0, 512);
                string content = System.Text.Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                {
                    ErrorMessage = msg;
                    return false;
                }
            }
            catch (Exception)
            {
                ErrorMessage = msg;
                return false;
            }

            //-------------------------------------------
            //  Try to instantiate new Bitmap, if .NET will throw exception
            //  we can assume that it's not a valid image
            //-------------------------------------------

            try
            {
                using (var bitmap = new System.Drawing.Bitmap(postedFile.InputStream))
                {
                }
            }
            catch (Exception)
            {
                ErrorMessage = "Ongeldige afbeelding.";
                return false;
            }

            return true;
        }
    }
}