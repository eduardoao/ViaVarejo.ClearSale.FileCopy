﻿using System;
using System.IO;

namespace ViaVarejo.ClearSale.FileCopy
{
    internal class FileProcessor
    {
        private readonly string path;

        public FileProcessor(string path)
        {
            this.path = path;
        }

        public bool CheckConnectionFilePath()
        {
            //string rootPath = new DirectoryInfo(path).Parent.Parent.FullName;
            //string FiledirectoryPath = Path.GetDirectoryName(path);
            try
            {
                if (Directory.Exists(path))
                    return true;
                else
                    return false;              
            }
            catch
            {
                return false;
            }

        }
    }
}