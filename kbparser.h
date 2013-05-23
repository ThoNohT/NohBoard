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


#include <windows.h>
#include <string>
#include <map>

#pragma once

struct KeyInfo
{
    unsigned short id;
    float x, y;
    float width, height;
    bool changeOnCaps;
    bool smalltext;
    std::wstring text, shiftText;
};

struct KBInfo
{
    int width, height;
    int nKeysDefined;
    int KBVersion;
    std::wstring Category;
    bool hasMouse;

    std::map<int, KeyInfo> definedKeys;
};

class KBParser
{
private:
    static int ParseValue(KBInfo * kbinfo, std::wstring word, std::wstring value, int n);
    static void SetKeyInfo(KeyInfo &info, int id, float x, float y,
                float width, float height, int changeOnCaps, int smallText,
                std::wstring text, std::wstring shiftText);
    static std::wstring ParseStuff(std::wstring text);
public:
    static KBInfo * ParseFile(std::wstring filename, bool full);
};
