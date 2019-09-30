using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;
using Microsoft.Win32;
using System.Diagnostics;

namespace Xr.AutoUpdate
{
    /// <summary> 
    /// 适用与ZIP压缩 
    /// </summary> 
    public class ZipHelper
    {
        #region 压缩

        /// <summary> 
        /// 递归压缩文件夹的内部方法 
        /// </summary> 
        /// <param name="folderToZip">要压缩的文件夹路径</param> 
        /// <param name="zipStream">压缩输出流</param> 
        /// <param name="parentFolderName">此文件夹的上级文件夹</param> 
        /// <returns></returns> 
        private static bool ZipDirectory(string folderToZip, ZipOutputStream zipStream, string parentFolderName)
        {
            bool result = true;
            string[] folders, files;
            ZipEntry ent = null;
            FileStream fs = null;
            Crc32 crc = new Crc32();

            try
            {
                ent = new ZipEntry(Path.Combine(parentFolderName, Path.GetFileName(folderToZip) + "/"));
                zipStream.PutNextEntry(ent);
                zipStream.Flush();

                files = Directory.GetFiles(folderToZip);
                foreach (string file in files)
                {
                    fs = File.OpenRead(file);

                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    ent = new ZipEntry(Path.Combine(parentFolderName, Path.GetFileName(folderToZip) + "/" + Path.GetFileName(file)));
                    ent.DateTime = DateTime.Now;
                    ent.Size = fs.Length;

                    fs.Close();

                    crc.Reset();
                    crc.Update(buffer);

                    ent.Crc = crc.Value;
                    zipStream.PutNextEntry(ent);
                    zipStream.Write(buffer, 0, buffer.Length);
                }

            }
            catch
            {
                result = false;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
                if (ent != null)
                {
                    ent = null;
                }
                GC.Collect();
                GC.Collect(1);
            }

            folders = Directory.GetDirectories(folderToZip);
            foreach (string folder in folders)
                if (!ZipDirectory(folder, zipStream, folderToZip))
                    return false;

            return result;
        }

        /// <summary> 
        /// 压缩文件夹  
        /// </summary> 
        /// <param name="folderToZip">要压缩的文件夹路径</param> 
        /// <param name="zipedFile">压缩文件完整路径</param> 
        /// <param name="password">密码</param> 
        /// <returns>是否压缩成功</returns> 
        public static bool ZipDirectory(string folderToZip, string zipedFile, string password)
        {
            bool result = false;
            if (!Directory.Exists(folderToZip))
                return result;

            ZipOutputStream zipStream = new ZipOutputStream(File.Create(zipedFile));
            zipStream.SetLevel(6);
            if (!string.IsNullOrEmpty(password)) zipStream.Password = password;

            result = ZipDirectory(folderToZip, zipStream, "");

            zipStream.Finish();
            zipStream.Close();

            return result;
        }

        /// <summary> 
        /// 压缩文件夹 
        /// </summary> 
        /// <param name="folderToZip">要压缩的文件夹路径</param> 
        /// <param name="zipedFile">压缩文件完整路径</param> 
        /// <returns>是否压缩成功</returns> 
        public static bool ZipDirectory(string folderToZip, string zipedFile)
        {
            bool result = ZipDirectory(folderToZip, zipedFile, null);
            return result;
        }

        /// <summary> 
        /// 压缩文件 
        /// </summary> 
        /// <param name="fileToZip">要压缩的文件全名</param> 
        /// <param name="zipedFile">压缩后的文件名</param> 
        /// <param name="password">密码</param> 
        /// <returns>压缩结果</returns> 
        public static bool ZipFile(string fileToZip, string zipedFile, string password)
        {
            bool result = true;
            ZipOutputStream zipStream = null;
            FileStream fs = null;
            ZipEntry ent = null;

            if (!File.Exists(fileToZip))
                return false;

            try
            {
                fs = File.OpenRead(fileToZip);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                fs.Close();

                fs = File.Create(zipedFile);
                zipStream = new ZipOutputStream(fs);
                if (!string.IsNullOrEmpty(password)) zipStream.Password = password;
                ent = new ZipEntry(Path.GetFileName(fileToZip));
                zipStream.PutNextEntry(ent);
                zipStream.SetLevel(6);

                zipStream.Write(buffer, 0, buffer.Length);

            }
            catch
            {
                result = false;
            }
            finally
            {
                if (zipStream != null)
                {
                    zipStream.Finish();
                    zipStream.Close();
                }
                if (ent != null)
                {
                    ent = null;
                }
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
            GC.Collect();
            GC.Collect(1);

            return result;
        }

        /// <summary> 
        /// 压缩文件 
        /// </summary> 
        /// <param name="fileToZip">要压缩的文件全名</param> 
        /// <param name="zipedFile">压缩后的文件名</param> 
        /// <returns>压缩结果</returns> 
        public static bool ZipFile(string fileToZip, string zipedFile)
        {
            bool result = ZipFile(fileToZip, zipedFile, null);
            return result;
        }

        /// <summary> 
        /// 压缩文件或文件夹 
        /// </summary> 
        /// <param name="fileToZip">要压缩的路径</param> 
        /// <param name="zipedFile">压缩后的文件名</param> 
        /// <param name="password">密码</param> 
        /// <returns>压缩结果</returns> 
        public static bool Zip(string fileToZip, string zipedFile, string password)
        {
            bool result = false;
            if (Directory.Exists(fileToZip))
                result = ZipDirectory(fileToZip, zipedFile, password);
            else if (File.Exists(fileToZip))
                result = ZipFile(fileToZip, zipedFile, password);

            return result;
        }

        /// <summary> 
        /// 压缩文件或文件夹 
        /// </summary> 
        /// <param name="fileToZip">要压缩的路径</param> 
        /// <param name="zipedFile">压缩后的文件名</param> 
        /// <returns>压缩结果</returns> 
        public static bool Zip(string fileToZip, string zipedFile)
        {
            bool result = Zip(fileToZip, zipedFile, null);
            return result;

        }

        #endregion

        #region 解压

        ///// <summary> 
        ///// 解压功能(解压压缩文件到指定目录) 
        ///// </summary> 
        ///// <param name="fileToUnZip">待解压的文件</param> 
        ///// <param name="zipedFolder">指定解压目标目录</param> 
        ///// <param name="password">密码</param> 
        ///// <returns>解压结果</returns> 
        //public static bool UnZip(string fileToUnZip, string zipedFolder, string password)
        //{
        //    bool result = true;
        //    FileStream fs = null;
        //    ZipInputStream zipStream = null;
        //    ZipEntry ent = null;
        //    string fileName;

        //    if (!File.Exists(fileToUnZip))
        //        return false;

        //    if (!Directory.Exists(zipedFolder))
        //        Directory.CreateDirectory(zipedFolder);

        //    try
        //    {
        //        zipStream = new ZipInputStream(File.OpenRead(fileToUnZip));
        //        if (!string.IsNullOrEmpty(password)) zipStream.Password = password;
        //        while ((ent = zipStream.GetNextEntry()) != null)
        //        {
        //            if (!string.IsNullOrEmpty(ent.Name))
        //            {
        //                fileName = Path.Combine(zipedFolder, ent.Name);
        //                fileName = fileName.Replace('/', '\\');//change by Mr.HopeGi 

        //                if (fileName.EndsWith("\\"))
        //                {
        //                    Directory.CreateDirectory(fileName);
        //                    continue;
        //                }

        //                fs = File.Create(fileName);
        //                int size = 2048;
        //                byte[] data = new byte[size];
        //                while (true)
        //                {
        //                    size = fs.Read(data, 0, data.Length);
        //                    if (size > 0)
        //                        fs.Write(data, 0, data.Length);
        //                    else
        //                        break;
        //                }
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        result = false;
        //    }
        //    finally
        //    {
        //        if (fs != null)
        //        {
        //            fs.Close();
        //            fs.Dispose();
        //        }
        //        if (zipStream != null)
        //        {
        //            zipStream.Close();
        //            zipStream.Dispose();
        //        }
        //        if (ent != null)
        //        {
        //            ent = null;
        //        }
        //        GC.Collect();
        //        GC.Collect(1);
        //    }
        //    return result;
        //}

        /// <summary> 
        /// 解压功能(解压压缩文件到指定目录) 
        /// </summary> 
        /// <param name="fileToUnZip">待解压的文件</param> 
        /// <param name="zipedFolder">指定解压目标目录</param> 
        /// <returns>解压结果</returns> 
        //public static bool UnZip(string fileToUnZip, string zipedFolder)
        //{
        //    bool result = UnZip(fileToUnZip, zipedFolder, null);
        //    return result;
        //}

        #endregion

        #region  解压文件 包括.rar 和zip

        /// <summary>
        ///解压文件
        /// </summary>
        /// <param name="fileFromUnZip">解压前的文件路径（绝对路径）</param>
        /// <param name="fileToUnZip">解压后的文件目录（绝对路径）</param>
        public static void UnpackFileRarOrZip(string fileFromUnZip, string fileToUnZip)
        {
            //获取压缩类型
            string unType = fileFromUnZip.Substring(fileFromUnZip.LastIndexOf(".") + 1, 3).ToLower();

            switch (unType)
            {
                case "rar":
                    UnRar(fileFromUnZip, fileToUnZip);
                    break;
                default:
                    UnZip(fileFromUnZip, fileToUnZip);
                    break;

            }
        }


        #endregion



        #region  解压文件 .rar文件

        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="unRarPatch"></param>
        /// <param name="rarPatch"></param>
        /// <param name="rarName"></param>
        /// <returns></returns>
        public static void UnRar(string fileFromUnZip, string fileToUnZip)
        {
            string the_rar;
            RegistryKey the_Reg;
            object the_Obj;
            string the_Info;

            try
            {
                the_Reg = Registry.LocalMachine.OpenSubKey(
                         @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe");
                the_Obj = the_Reg.GetValue("");
                the_rar = the_Obj.ToString();
                the_Reg.Close();
                //the_rar = the_rar.Substring(1, the_rar.Length - 7);

                if (Directory.Exists(fileToUnZip) == false)
                {
                    Directory.CreateDirectory(fileToUnZip);
                }
                the_Info = "x " + Path.GetFileName(fileFromUnZip) + " " + fileToUnZip + " -y";

                ProcessStartInfo the_StartInfo = new ProcessStartInfo();
                the_StartInfo.FileName = the_rar;
                the_StartInfo.Arguments = the_Info;
                the_StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                the_StartInfo.WorkingDirectory = Path.GetDirectoryName(fileFromUnZip);//获取压缩包路径

                Process the_Process = new Process();
                the_Process.StartInfo = the_StartInfo;
                the_Process.Start();
                the_Process.WaitForExit();
                the_Process.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return unRarPatch;
        }

        #endregion



        #region  解压文件 .zip文件

        /// <summary>
        /// 解压功能(解压压缩文件到指定目录)
        /// </summary>
        /// <param name="FileToUpZip">待解压的文件</param>
        /// <param name="ZipedFolder">指定解压目标目录</param>
        public static void UnZip(string FileToUpZip, string ZipedFolder)
        {
            if (!File.Exists(FileToUpZip))
            {
                return;
            }

            if (!Directory.Exists(ZipedFolder))
            {
                Directory.CreateDirectory(ZipedFolder);
            }

            ICSharpCode.SharpZipLib.Zip.ZipInputStream s = null;
            ICSharpCode.SharpZipLib.Zip.ZipEntry theEntry = null;

            string fileName;
            FileStream streamWriter = null;
            try
            {
                s = new ICSharpCode.SharpZipLib.Zip.ZipInputStream(File.OpenRead(FileToUpZip));
                while ((theEntry = s.GetNextEntry()) != null)
                {

                    if (theEntry.Name != String.Empty)
                    {
                        fileName = Path.Combine(ZipedFolder, theEntry.Name);
                        ///判断文件路径是否是文件夹

                        if (fileName.EndsWith("/") || fileName.EndsWith("\\"))
                        {
                            Directory.CreateDirectory(fileName);
                            continue;
                        }

                        streamWriter = File.Create(fileName);
                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
            finally
            {
                if (streamWriter != null)
                {
                    streamWriter.Close();
                    streamWriter = null;
                }
                if (theEntry != null)
                {
                    theEntry = null;
                }
                if (s != null)
                {
                    s.Close();
                    s = null;
                }
                GC.Collect();
                GC.Collect(1);
            }
        }


        #endregion
    }

}
