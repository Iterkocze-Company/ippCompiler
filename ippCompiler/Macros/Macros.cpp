/*  This file is part of Iterkocze ippCompiler and it's under BSD-3-Clause License.
    Copyright (c) 2021, Iterkocze-Company
    All rights reserved.
    https://github.com/Iterkocze-Company/ippCompiler
*/

bool MacroContains(std::string s1, std::string s2)
{
    if (s1.find(s2) != std::string::npos)
        return true;
    else
        return false;
}