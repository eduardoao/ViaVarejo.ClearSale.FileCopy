using System;
using System.Collections.Generic;
using ViaVarejo.ClearSale.FileCopy.Infra;

namespace ViaVarejo.ClearSale.FileCopy
{
    internal class FileHandler
    {
        private ConfigurationClearSale pathClearSaleFtp;
        private string pathSas;

        private readonly FtpClient ftp;

        public FileHandler(ConfigurationClearSale pathClearSaleFtp, string pathSas)
        {
          
            this.pathClearSaleFtp = pathClearSaleFtp;           
            this.pathSas = pathSas;

            ftp = new FtpClient(pathClearSaleFtp.uriClearSaleFtp, pathClearSaleFtp.userClearSaleFtp, pathClearSaleFtp.passClearSaleFtp, pathSas);

        }

       
        public List<string> ListFileFtpServer()
        {
            try
            {
                var filelist = ftp.GetFileListServer();
                return filelist;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public void DownloFile(string filename)
        {
            try
            {
                ftp.DownloadFile(filename);   
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteFileServer(string filename)
        {
            try
            {
                ftp.DeleteFileServer(filename);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}