using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace ViaVarejo.ClearSale.FileCopy.Infra
{
    public class FtpClient
    {
        private readonly string _uri;
        private readonly string _user;
        private readonly string _password;
        private readonly string _pathSas;

        public FtpClient(string uri, string user, string password, string pathSas)
        {
            _uri = uri;
            _user = user;
            _password = password;
            _pathSas = pathSas;

        }
        public List<string> GetFileListServer()
        {
            var fileList = new List<string>();
            try
            {

                FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create(_uri);
                ftpRequest.Credentials = new NetworkCredential(_user, _password);

                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                using (FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse())
                {
                    StreamReader streamReader = new StreamReader(response.GetResponseStream());
                    List<string> directories = new List<string>();

                    var line = streamReader.ReadLine();
                    while (line != null)
                    {
                        fileList.Add(line.Trim());
                        line = streamReader.ReadLine();
                    }
                }


                return fileList;
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DownloadFile(string file)
        {
            try
            {
                string inputfilepath = $"{_pathSas}\\{file}";
                string ftpfilepath = $"{_uri}/{file}";
                string ftpfullpath = ftpfilepath;

                using (WebClient request = new WebClient())
                {
                    request.Credentials = new NetworkCredential(_user, _password);
                    byte[] fileData = request.DownloadData(ftpfullpath);

                    using (FileStream _file = File.Create(inputfilepath))
                    {
                        _file.Write(fileData, 0, fileData.Length);
                        _file.Close();
                    }

                }

            }
         
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        public string DeleteFileServer(string filename)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(_uri + "/" + filename);
                request.Method = WebRequestMethods.Ftp.DeleteFile;
                request.Credentials = new NetworkCredential(_user, _password);

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    return response.StatusDescription;
                }
            }

            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
