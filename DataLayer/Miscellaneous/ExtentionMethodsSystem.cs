using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using DataLayer;
using System.IO;
using Utility;
using System.Linq;

namespace System
{
    public static class ExtentionMethodsImage
    {
        public static AppSetting AppSetting { get; set; }
      //  static long qulityImageConfig;
        public const string defaultImage = "defaultImage";
        public const int ImgDetailsSize = 800;
        public const int ImgListSize = 150;
        public const int ImgListSizeUI = 263;
       static long qulityImageConfig = 0L;

        #region ImageTools
        public static long QulityImageConfig
        {
            get
            {
                return Convert.ToInt64(AppSetting.QulityImageConfigStr);
            }
        }
        public static string FullVirtualDefaultImagePath { get { return string.Concat(AppSetting.ImagePathInVirtual, "/", defaultImage, ".jpg"); } }
        public static string GetOtherFilePath(this string orginalFileName )
        {
            if (string.IsNullOrWhiteSpace(orginalFileName)) return "";
            return string.Concat(AppSetting.ImagePathOtherFileVirtual, "/", orginalFileName );
        }
        public static string GetUrlOginalImg(this string orginalFilename)
        {
            if (string.IsNullOrWhiteSpace(orginalFilename)) return FullVirtualDefaultImagePath;
            return string.Concat(AppSetting.ImagePathInVirtual, "/", orginalFilename, ".jpg");
        }
        public static string GetUrlDetailsImg(this string orginalFilename)
        {
            if (string.IsNullOrWhiteSpace(orginalFilename)) return FullVirtualDefaultImagePath;
            return string.Concat(AppSetting.ImagePathInVirtual, "/", orginalFilename, ImgDetailsSize, "_", ImgDetailsSize, ".jpg");
        }
        public static string GetUrlListImg(this string orginalFilename)
        {
            if (string.IsNullOrWhiteSpace(orginalFilename)) return FullVirtualDefaultImagePath;
            return string.Concat(AppSetting.ImagePathInVirtual, "/", orginalFilename, ImgListSize, "_", ImgListSize, ".jpg");
        }

        public static string GetUrlListImgUI(this string orginalFilename)
        {
            if (string.IsNullOrWhiteSpace(orginalFilename)) return FullVirtualDefaultImagePath;
            return string.Concat(AppSetting.ImagePathInVirtual, "/", orginalFilename, ImgListSizeUI, "_", ImgListSizeUI, ".jpg");
        }
        public static string ImgOrginalName(this string orginalFilename)
        {
            if (orginalFilename == null) return FullVirtualDefaultImagePath ;
            string fileName = orginalFilename + ".jpg";
            var orginalFileExist = File.Exists(string.Concat(AppSetting.ImagePathInServer, "\\", fileName));
            if (orginalFileExist)
            {
                  return string.Concat(AppSetting.ImagePathInVirtual, "/", fileName);
            }
            else
            {
                return FullVirtualDefaultImagePath;
            }
        }
        static string checkAndCreateImageFromOrginal(int size,string name)
        {
            string fileNameO = string.Concat(name, ".jpg");
            string fullFileNameO = string.Concat(AppSetting.ImagePathInServer, "\\", fileNameO);
            var fileExistO = File.Exists(fullFileNameO);
            if (!fileExistO)
            {
                return "";
            }

            string fileName =string.Concat( name ,size,"_",size, ".jpg");
            string fullFileName = string.Concat(AppSetting.ImagePathInServer, "\\", fileName);
            var fileExist = File.Exists(fullFileName);
            if (fileExist)
            {
                return fullFileName;
            }
            else
            {
                ImageProcessing imageProcessing = new ImageProcessing(QulityImageConfig);
                imageProcessing.ReSizeImage(fullFileNameO, fullFileName, size);
                if( File.Exists(fullFileName)) return fullFileName;
                else return FullVirtualDefaultImagePath;
            }
        }
        public static bool CheckImageRatio(this string orginalFileName)
        {
            ImageProcessing imageProcessing = new ImageProcessing(QulityImageConfig);
            string fileNameO = string.Concat(orginalFileName, ".jpg");
            string fullFileNameO = string.Concat(AppSetting.ImagePathInServer, "\\", fileNameO);
            return imageProcessing.CheckImageRatio(fullFileNameO);
        }
        
        public static void SaveImgList(this string orginalFileName)
        {
            if (orginalFileName == null) return ;
             checkAndCreateImageFromOrginal(ImgListSize, orginalFileName );
        }

        public static void SaveImgListUI(this string orginalFileName)
        {
            if (orginalFileName == null) return;
            checkAndCreateImageFromOrginal(ImgListSizeUI, orginalFileName);
        }
        public static void SaveImgDetails(this string orginalFileName)
        {
            if (orginalFileName == null) return ;
             checkAndCreateImageFromOrginal(ImgDetailsSize, orginalFileName);
        }

        public  static bool RemoveFile(this string fullFileName)
        {
            var fileExist = File.Exists(fullFileName);
            if (fileExist)
            {
                File.Delete(fullFileName);
            }
            return true;
        }
        public static bool RemoveAllInsatnceImg(this string orginalFileName)
        {
            if (orginalFileName == null) return false;

            string fileName = string.Concat(orginalFileName,  ".jpg");
            string fullFileName = string.Concat(AppSetting.ImagePathInServer, "\\", fileName);
            var fileExist = File.Exists(fullFileName);
            if (fileExist)
            {
                File.Delete(fullFileName);
            }

            string fileNamed = string.Concat(orginalFileName, ImgDetailsSize, "_", ImgDetailsSize, ".jpg");
            string fullFileNamed = string.Concat(AppSetting.ImagePathInServer, "\\", fileNamed);
            var fileExistd = File.Exists(fullFileNamed);
            if (fileExistd)
            {
                File.Delete(fullFileNamed);
            }

            string fileNamel = string.Concat(orginalFileName, ImgListSize, "_", ImgListSize, ".jpg");
            string fullFileNamel = string.Concat(AppSetting.ImagePathInServer, "\\", fileNamel);
            var fileExistl = File.Exists(fullFileNamel);
            if (fileExistl)
            {
                File.Delete(fullFileNamel);
            }

            string fileNameUI = string.Concat(orginalFileName, ImgListSizeUI, "_", ImgListSizeUI, ".jpg");
            string fullFileNameUI = string.Concat(AppSetting.ImagePathInServer, "\\", fileNameUI);
            var fileExistUI = File.Exists(fullFileNameUI);
            if (fileExistUI)
            {
                File.Delete(fullFileNameUI);
            }
            return true;
        }

        public static bool RemoveAllSizeImgOtherthanOrginalFile(this string orginalFileName)
        {
            if (orginalFileName == null) return false;

            string fileNamed = string.Concat(orginalFileName, ImgDetailsSize, "_", ImgDetailsSize, ".jpg");
            string fullFileNamed = string.Concat(AppSetting.ImagePathInServer, "\\", fileNamed);
            var fileExistd = File.Exists(fullFileNamed);
            if (fileExistd)
            {
                File.Delete(fullFileNamed);
            }

            string fileNamel = string.Concat(orginalFileName, ImgListSize, "_", ImgListSize, ".jpg");
            string fullFileNamel = string.Concat(AppSetting.ImagePathInServer, "\\", fileNamel);
            var fileExistl = File.Exists(fullFileNamel);
            if (fileExistl)
            {
                File.Delete(fullFileNamel);
            }

            string fileNameUI = string.Concat(orginalFileName, ImgListSizeUI, "_", ImgListSizeUI, ".jpg");
            string fullFileNameUI = string.Concat(AppSetting.ImagePathInServer, "\\", fileNameUI);
            var fileExistUI = File.Exists(fullFileNameUI);
            if (fileExistUI)
            {
                File.Delete(fullFileNameUI);
            }
            return true;
        }

        public static string GetAllFileListFolder()
        {
            DirectoryInfo d = new DirectoryInfo(AppSetting.ImagePathInServer);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.jpg"); //Getting Text files
            string str = "";
           var  FilesOrdered = Files.OrderBy(f => f.Name).ToList();
            foreach (FileInfo file in FilesOrdered)
            {
                
                str = str + ", " + file.Name + Environment.NewLine;
            }

            return str;
        }

        public static bool RemoveAllFileSizeListFolder (string path)
        {
            DirectoryInfo d = new DirectoryInfo(path);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.jpg"); //Getting Text files

            var removeList = Files.Where(f => f.Name.Length >= 41).ToList();
            foreach (FileInfo file in removeList)
            {
                RemoveFile(file.FullName);
              //  RemoveAllSizeImgOtherthanOrginalFile(file.Name.Replace(".jpg", ""));
            }
            return true; ;
        }

        public static bool CreateImagesReSize( string path)
        {
            DirectoryInfo d = new DirectoryInfo(path);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.jpg"); //Getting Text files
         
            foreach (FileInfo file in Files)
            {
                if (Files.Where(f=>f.Name.Contains( file.Name.GetLenghStr(35) ) ).Count() == 1 ) 
                {
                    file.Name.Replace(".jpg", "").SaveImgDetails();
                    file.Name.Replace(".jpg", "").SaveImgListUI();
                    file.Name.Replace(".jpg", "").SaveImgList();
                }

            }

            return true; ;
        }

        #endregion
    }
}
