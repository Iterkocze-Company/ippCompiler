/*  This file is part of Iterkocze ippCompiler and it's under BSD-3-Clause License.
    Copyright (c) 2021, Iterkocze-Company
    All rights reserved.
    https://github.com/Iterkocze-Company/ippCompiler
*/
using System.Net;

namespace ippCompiler
{
    public static class PackageManager
    {
        public static void DownloadMacros()
        {
            WebClient client = new();
            client.DownloadFile("https://nfpm.xlx.pl/Macros/Macros.cpp", "nfpm.txt");
        }
    }
}
