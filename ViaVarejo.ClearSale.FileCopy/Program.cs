using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using ViaVarejo.ClearSale.FileCopy.Infra;


namespace ViaVarejo.ClearSale.FileCopy
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        private static string pathClearSaleFtp;
        private static string portClearSaleFtp;
        private static string userClearSaleFtp;
        private static string passClearSaleFtp;
        private static string aplicationLogName;
        private static string pathSas;

        private static ConfigurationClearSale credencialsClearSale;

        private static FileHandler fileHandler;

        static void Main(string[] args)
        {
            try
            {
                LoadConfigurationApplication();
                GetFileHandler();
                ProcessFiles();
                
            }
            catch (Exception ex)
            {
                Log.PutEventLog(ex.Message, aplicationLogName, "Application");
            }

        }

        private static void ProcessFiles()
        {
            var fileProcessor = new FileProcessor(pathSas);
            var result = fileProcessor.CheckConnectionFilePath();

            if (result)
            {
                var fileftplist = fileHandler.ListFileFtpServer();

                if (fileftplist.Count == 0)
                    Log.PutEventLog($"Não existem arquivos no servidor FTP", aplicationLogName, "Application");


                foreach (var item in fileftplist)
                {
                    Log.PutEventLog($"Iniciando o download do arquivo: {item}", aplicationLogName, "Application");
                    fileHandler.DownloFile(item);
                    Log.PutEventLog($"Download do arquivo {item} finalizado", aplicationLogName, "Application");

                    Log.PutEventLog($"Iniciando a remoção do arquivo {item} no servidor FTP", aplicationLogName, "Application");
                    fileHandler.DeleteFileServer(item);
                    Log.PutEventLog($"Remoção do arquivo {item} no servidor FTP finalizada", aplicationLogName, "Application");

                }
            }
            else 
                Log.PutEventLog("Pasta de arquivos de destino não está criada!", aplicationLogName, "Application");
        }


        private static FileHandler GetFileHandler()
        {
            fileHandler = new FileHandler(credencialsClearSale, pathSas);
            return fileHandler;
        }


        private static void LoadConfigurationApplication()
        {

            var builder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json");

            Configuration = builder.Build();


            pathClearSaleFtp = Configuration["ClearSaleFTPSettings:uri"];
            userClearSaleFtp = Configuration["ClearSaleFTPSettings:user"];
            passClearSaleFtp = Configuration["ClearSaleFTPSettings:pass"];
            aplicationLogName = Configuration["AplicationLog:value"];

            pathSas = Configuration["SasSettings:path"];


            Log.PutEventLog("Carregando configurações da aplicação", aplicationLogName, "Application");

            credencialsClearSale = new ConfigurationClearSale(pathClearSaleFtp, portClearSaleFtp, userClearSaleFtp, passClearSaleFtp);

            Log.PutEventLog("Configurações da aplicação carregadas com sucesso!", aplicationLogName, "Application");
        }
    }
}
