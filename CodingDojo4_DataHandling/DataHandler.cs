﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingDojo4_DataHandling
{
    public class DataHandler
    {
        //für ordner und endung für logfiles definieren
        private const string folder = @"Files/";
        private const string extension = ".txt";

        //name des files zuweisen
        public String[] Load(string name)
        {
            String[] lines = File.ReadAllLines(folder + name);
            return lines;
        }

        //Speicherung
        public void Save(List<string> data)
        {
            File.WriteAllLines(folder + DateTime.Now.ToShortDateString() + "_" + DateTime.Now.ToFileTimeUtc() + extension, data.ToArray());
        }

        //
        public string[] QueryFilesFromFolder()
        {
            if (!Directory.Exists(folder)) { Directory.CreateDirectory(folder); }
            DirectoryInfo info = new DirectoryInfo(folder);
            var result = info.GetFiles("*" + extension);
            string[] temp = new string[result.Length];
            int i = 0;
            foreach (var item in result)
            {
                temp[i] = item.Name;
                i++;
            }
            return temp;
        }

        //Löschen
        public void Delete(string name)
        {
            if (File.Exists(folder + name))
            {
                File.Delete(folder + name);
            }
        }

        //schaun ob es existiert
        public bool CheckIfFileExists(string name)
        {
            return File.Exists(folder + name + extension);
        }
    }
}
