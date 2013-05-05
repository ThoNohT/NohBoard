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


#include "kbparser.h"
#include <iostream>
#include <fstream>

using std::string;

KBInfo * KBParser::ParseFile(string filename)
{
    KBInfo *kbinfo = new KBInfo;

    string word;
    int value;
    std::ifstream is(filename);
    if (!is.good()) return NULL;
    
    // Read the general settings
    int n = 0;
    while (is && n < 4)
    {
        is >> word >> value;
        n = ParseValue(kbinfo, word, value, n);
    }

    if (n < 4) return NULL;

    // Read the key definitions

    unsigned short id;
    int changeOnCaps;
    float x, y, width, height;
    string text, shifttext;
    int i = 0;
    while (is && i < kbinfo->nKeysDefined)
    {
        is >> word >> id >> x >> y >> width >> height >> text >> shifttext >> changeOnCaps;
        if (word == "key")
        {
            // Sanitize strings
            text = ParseStuff(text);
            shifttext = ParseStuff(shifttext);
            kbinfo->definedKeys[id] = KeyInfo();
            SetKeyInfo(kbinfo->definedKeys[id], id, x, y, width, height, changeOnCaps, text, shifttext);
            i += 1;
        }
    }

    if (i < kbinfo->nKeysDefined) return NULL;

    return kbinfo;
}

string KBParser::ParseStuff(string text)
{
    text = doReplace(text, "%20%", " ");
    return doReplace(text, "%0%", "");
}

string KBParser::doReplace(string text, string find, string replace)
{
    while (true)
    {
        size_t pos = text.find(find);
        if (0 > pos || pos > text.length()) return text;
        text = text.replace(pos, find.length(), replace);
    }
}

int KBParser::ParseValue(KBInfo * kbinfo, string word, int value, int n)
{
    if (word == "width")
    {
        kbinfo->width = value;
        return n+1;
    }
    if (word == "height")
    {
        kbinfo->height = value;
        return n+1;
    }
    if (word == "nKeysDefined")
    {
        kbinfo->nKeysDefined = value;
        return n+1;
    }
    if (word == "KBVersion")
    {
        kbinfo->KBVersion = value;
        return n+1;
    }
    return n;
}

void KBParser::SetKeyInfo(KeyInfo &info, int id, float x, float y,
                float width, float height, int changeOnCaps,
                string text, string shiftText)
{
    info.id = id;
    info.x = x; info.y = y;
    info.width = width; info.height = height;
    info.changeOnCaps = changeOnCaps == 1;
    info.text = text; info.shiftText = shiftText;
}
