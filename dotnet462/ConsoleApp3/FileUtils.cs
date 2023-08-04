using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    public static class FileUtils
    {
        public static string GetConfig()
        {
            var dirFile = @"C:\Test\Config\Data.txt";
            return File.ReadAllText(dirFile);
        }
    }
}
