using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
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

        private static ILogger logger;

        static void Main(string[] args)
        {
            try
            {
                logger = Log.LogActive();

                LoadAppSettings();
                var interval  = Configuration["Interval:value"];
                var callback = new TimerCallback(TimerCallback);
                Timer stateTimer = new Timer(callback, null, 0, Convert.ToInt32(interval));

                for (; ; )
                {   
                   Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                Log.PutEventLog(ex.Message, aplicationLogName, "Application");
            }

        }

        private static void InitializeProcess()
        {
            try
            {
                LoadConfigurationApplication();
                GetFileHandler();
                ProcessFiles();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static void ProcessFiles()
        {
            try
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
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        private static FileHandler GetFileHandler()
        {
            try
            {
                fileHandler = new FileHandler(credencialsClearSale, pathSas);
                return fileHandler;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static void LoadConfigurationApplication()
        {
            try
            {
                pathClearSaleFtp = Configuration["ClearSaleFTPSettings:uri"];
                userClearSaleFtp = Configuration["ClearSaleFTPSettings:user"];
                passClearSaleFtp = Configuration["ClearSaleFTPSettings:pass"];
                aplicationLogName = Configuration["AplicationLog:value"];

                pathSas = Configuration["SasSettings:path"];


                Log.PutEventLog("Carregando configurações da aplicação", aplicationLogName, "Application");

                credencialsClearSale = new ConfigurationClearSale(pathClearSaleFtp, portClearSaleFtp, userClearSaleFtp, passClearSaleFtp);

                Log.PutEventLog("Configurações da aplicação carregadas com sucesso!", aplicationLogName, "Application");

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        private static IConfigurationRoot LoadAppSettings()
        {
            try
            {
                var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json");

                Configuration = builder.Build();

                return Configuration;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        //Time somente para testar o console
        private static void TimerCallback(Object o)
        {
            try
            {
                logger.LogInformation($"Iniciando processo FTP {DateTime.Now}");
                //Console.WriteLine($"Iniciando processo FTP {DateTime.Now}");
                InitializeProcess();
                GC.Collect();

            }
            catch (Exception ex)
            {
                Log.PutEventLog($"Ocorreu uma falha ao realizar o download do arquivo! {ex.Message}", aplicationLogName, "Application");
            }
        }

    }
}
