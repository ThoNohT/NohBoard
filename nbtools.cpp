/********************************************************************************
 Copyright (C) 2012 Eric Bataille <e.c.p.bataille@gmail.com>

 This program is free software; you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation; either version 2 of the License, or
 (at your option) any later version.

 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 GNU General Public License for more details.

 You should have received a copy of the GNU General Public License
 along with this program; if not, write to the Free Software
 Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307, USA.
********************************************************************************/
                    
#include "nbtools.h"
#include <vector>
#include <windows.h>
#include <time.h>

using std::wstring;

wstring NBTools::GetApplicationPath()
{
    std::vector<WCHAR> curDir(MAX_PATH);
    DWORD result = GetModuleFileName(NULL, &curDir[0], MAX_PATH);
    while(result == curDir.size()) {
        curDir.resize(curDir.size() * 2);
        result = GetModuleFileName(NULL, &curDir[0], (DWORD)curDir.size());
    }
    return wstring(curDir.begin(), curDir.begin() + result);
    return L"";
}

wstring NBTools::GetApplicationDirectory()
{
    wstring appPath = GetApplicationPath();
    size_t pos = appPath.rfind('\\');
    return appPath.substr(0, pos+1);
}


bool NBTools::EndsWith(wstring check, wstring end)
{
    if (check.length() >= end.length()) {
        return (0 == check.compare(check.length() - end.length(), end.length(), end));
    } else {
        return false;
    }

}

wstring NBTools::doReplace(wstring text, wstring find, wstring replace)
{
    while (true)
    {
        size_t pos = text.find(find);
        if (0 > pos || pos > text.length()) return text;
        text = text.replace(pos, find.length(), replace);
    }
}

bool NBTools::IsBright(unsigned long c)
{
    int r = GetRValue(c);
    int g = GetGValue(c);
    int b = GetBValue(c);

    return (r+g+b > 465);
}

wstring NBTools::GetWText(HWND hwnd)
{
    int nCharacters = GetWindowTextLength(hwnd)+1;
    WCHAR * charray = new WCHAR[nCharacters];
    GetWindowText(hwnd, charray, nCharacters);
    std::wstring str = charray;
    return str;
}

int NBTools::strToInt(wstring str)
{
    if (IsInt(str))
        return stoi(str);

    return 0;
}

 bool NBTools::IsInt(wstring & s)
{
   if(s.empty() || ((!isdigit(s[0])) && (s[0] != '-') && (s[0] != '+')))
       return false;

   wchar_t * p;
   wcstol(s.c_str(), &p, 10);

   return (*p == 0);
}

int NBTools::GetClockMs()
{
    return clock() / (CLOCKS_PER_SEC / 1000);
}