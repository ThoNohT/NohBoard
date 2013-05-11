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
#include "nbtools.h"
#include <iostream>
#include <fstream>

using std::wstring;

KBInfo * KBParser::ParseFile(wstring filename)
{
    KBInfo *kbinfo = new KBInfo;
    wstring word;
    int value;

    std::wifstream is(filename);
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
    wstring text, shifttext;
    int i = 0;
    while (is && i < kbinfo->nKeysDefined)
    {
        is >> word >> id >> x >> y >> width >> height >> text >> shifttext >> changeOnCaps;
        if (word == L"key")
        {
            // Sanitize strings
            text = ParseStuff(text);
            shifttext = ParseStuff(shifttext);
            kbinfo->definedKeys[i] = KeyInfo();
            SetKeyInfo(kbinfo->definedKeys[i], id, x, y, width, height, changeOnCaps, text, shifttext);
            i += 1;
        }
    }

    if (i < kbinfo->nKeysDefined) return NULL;

    return kbinfo;
}

wstring KBParser::ParseStuff(wstring text)
{
    // Workaround: To allow for some special characters now, simply add these conversions
    WCHAR letters[10] = { 'A', 'O', 'E', 'I', 'U', 'a', 'o', 'e', 'i', 'u' };
    wstring dac[10] = { L"Ä", L"Ö", L"Ë", L"Ï", L"Ü", L"ä", L"ö", L"ë", L"ï", L"ü" };
    wstring til[10] = { L"Ã", L"Õ", L"Ẽ", L"Ï", L"Ũ", L"ã", L"õ",  L"ẽ", L"ï", L"ũ" };
    wstring cir[10] = { L"Â", L"Ô", L"Ê", L"Î", L"Ũ", L"â", L"ô",  L"ê", L"î", L"ũ" };
    wstring gra[10] = { L"À", L"Ò", L"È", L"Ì", L"Ù", L"à", L"ò",  L"è", L"ì", L"ù" };
    wstring aig[10] = { L"Á", L"Ó", L"É", L"Í", L"Ú", L"á", L"ó",  L"é", L"í", L"ú" };
    for (int i = 0; i < 10; i++)
    {
        wstring dacn =  L"%\"";
        dacn += letters[i];
        dacn += '%';

        wstring tiln = L"%~";
        tiln += letters[i];
        tiln += '%';

        wstring cirn = L"%^";
        cirn += letters[i];
        cirn += '%';

        wstring gran = L"%'";
        gran += letters[i];
        gran += '%';

        wstring aign = L"%";
        aign += letters[i];
        aign += L"'%";

        text = NBTools::doReplace(text, dacn, dac[i]);
        text = NBTools::doReplace(text, tiln, til[i]);
        text = NBTools::doReplace(text, cirn, cir[i]);
        text = NBTools::doReplace(text, gran, gra[i]);
        text = NBTools::doReplace(text, aign, aig[i]);
    }
    // Some more special characters
    text = NBTools::doReplace(text, L"%/o%", L"ø");
    text = NBTools::doReplace(text, L"%/O%", L"Ø");
    text = NBTools::doReplace(text, L"%ae%", L"æ");
    text = NBTools::doReplace(text, L"%AE%", L"Æ");
    text = NBTools::doReplace(text, L"%oe%", L"œ");
    text = NBTools::doReplace(text, L"%OE%", L"Œ");
    text = NBTools::doReplace(text, L"%,c%", L"ç");
    text = NBTools::doReplace(text, L"%,C%", L"Ç");
    text = NBTools::doReplace(text, L"%''%", L"´");
    text = NBTools::doReplace(text, L"%par%", L"§");

    // Signs
    text = NBTools::doReplace(text, L"%up%", L"↑");
    text = NBTools::doReplace(text, L"%down%", L"↓");
    text = NBTools::doReplace(text, L"%left%", L"←");
    text = NBTools::doReplace(text, L"%right%", L"→");
    text = NBTools::doReplace(text, L"%return%", L"↵");
    text = NBTools::doReplace(text, L"%shift%", L"⇑");

    // whitespace
    text = NBTools::doReplace(text, L"%20%", L" ");
    return NBTools::doReplace(text, L"%0%", L"");
}

int KBParser::ParseValue(KBInfo * kbinfo, wstring word, int value, int n)
{
    if (word == L"width")
    {
        kbinfo->width = value;
        return n+1;
    }
    if (word == L"height")
    {
        kbinfo->height = value;
        return n+1;
    }
    if (word == L"nKeysDefined")
    {
        kbinfo->nKeysDefined = value;
        return n+1;
    }
    if (word == L"KBVersion")
    {
        kbinfo->KBVersion = value;
        return n+1;
    }
    return n;
}

void KBParser::SetKeyInfo(KeyInfo &info, int id, float x, float y,
                float width, float height, int changeOnCaps,
                wstring text, wstring shiftText)
{
    info.id = id;
    info.x = x; info.y = y;
    info.width = width; info.height = height;
    info.changeOnCaps = changeOnCaps == 1;
    info.text = text; info.shiftText = shiftText;
}
