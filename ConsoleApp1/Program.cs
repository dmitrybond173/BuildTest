using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ConsoleApp1
{
    class Program
    {
        private const string HELP_LICENSE_TEXT =
            "Customizable Performance Counters Service.                                      \r\n" +
            "                                                                                \r\n" +
            "Copyright (c) 2012 Dmitry Bondarenko, Kyiv, Ukraine                             \r\n" +
            "                                                                                \r\n" +
            "This software is provided 'as-is', without any express or implied warranty.     \r\n" +
            "In no event will the authors be held liable for any damages arising from        \r\n" +
            "the use of this software.                                                       \r\n" +
            "                                                                                \r\n" +
            "Permission is granted to anyone to use this software for any purpose,           \r\n" +
            "including commercial applications, and to alter it and redistribute it freely,  \r\n" +
            "subject to the following restrictions:                                          \r\n" +
            "                                                                                \r\n" +
            "1. The origin of this software must not be misrepresented; you must not claim   \r\n" +
            "   that you wrote the original software. If you use this software in a product, \r\n" +
            "   an acknowledgment in the product documentation would be appreciated but      \r\n" +
            "   is not required.                                                             \r\n" +
            "                                                                                \r\n" +
            "2. Altered source versions must be plainly marked as such, and must not be      \r\n" +
            "   misrepresented as being the original software.                               \r\n" +
            "                                                                                \r\n" +
            "3. This notice may not be removed or altered from any source distribution.      \r\n" +
            "\r\n" +
            "                                                                                \r\n" +
            "=============================================================================   \r\n" +
            "                                                                                \r\n" +
            "Welcome to send your feedbacks, bug reports, sugestiongs to dima_ben@ukr.net    \r\n" +
            "";

        private const string HELP_TEXT_TITLE =
            "Customizable Performance Counters $(version)\r\n" +
            "(c) 2012 Dmitry Bondarenko (dima_ben@ukr.net)\r\n" +
            "";

        private const string HELP_TEXT_USAGE =
            "Usage: CustomizablePerfCounters [options] \r\n" +
            "";

        private const string HELP_TEXT =
            HELP_TEXT_USAGE +
            "\r\n" +
            "Supported options:\r\n" +
            "  -?  - print this message\r\n" +
            "  -eula  - print end-user license agreement text\r\n" +
            "  -config=[configFilename] - name of performance counters config file\r\n" +
            "  \r\n" +
            "";

        static int Main(string[] args)
        {
            Console.WriteLine(HELP_TEXT_TITLE.Replace("$(version)", GetVersionStr()));

            Program tool = new Program();
            return tool.Run(args);
        }

        private static string GetVersionStr()
        {
            return Assembly.GetEntryAssembly().GetName().Version.ToString();
        }

        private int Run(string[] args)
        {
            try
            {
                if (!AnalyzeCmdLine(args))
                    return 1;



                return 0;
            }
            catch (Exception exc)
            {
                Console.WriteLine("ERR({0}): {1}", exc.GetType(), exc.Message);
                return 2;
            }
        }

        private bool AnalyzeCmdLine(string[] args)
        {
            foreach (string arg in args)
            {
                string pn = arg.Trim(" \t".ToCharArray());
                if (pn.StartsWith("-") || pn.StartsWith("/"))
                {
                    string pv = "";
                    pn = pn.Remove(0, 1);
                    int p = pn.IndexOf('=');
                    if (p < 0) p = pn.IndexOf(':');
                    if (p >= 0)
                    {
                        pv = pn.Remove(0, p + 1);
                        pn = pn.Remove(p, pn.Length - p);
                    }
                    pn = pn.ToLower();
                    if (pn == "?" || pn == "help")
                    {
                        Console.WriteLine(HELP_TEXT);
                        return false;
                    }
                    else if (pn == "copyright" || pn == "license" || pn == "eula")
                    {
                        Console.WriteLine(HELP_LICENSE_TEXT);
                        return false;
                    }
                    else if (pn == "config")
                    {
                        //this.settings.ConfigFilename = pv;
                    }
                    else if (pn == "option1")
                    {
                        this.settings.Option1 = true;
                    }
                }
                else
                {
                    addFiles(pn);
                }
            }
            return true;
        }

        private void addFiles(string pFilespec)
        {
            string path = Path.GetDirectoryName(pFilespec);
            string mask = Path.GetFileName(pFilespec);
            if (string.IsNullOrEmpty(path))
                path = Directory.GetCurrentDirectory();
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] files = dir.GetFiles(mask);
            this.settings.Files.AddRange(files);
        }

        private ToolSettings settings = new ToolSettings();
    }

    public class ToolSettings
    {
        public bool Option1 = true;
        public List<FileInfo> Files = new List<FileInfo>();
    }

}
