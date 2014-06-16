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


#include "configparser.h"
#include "nbtools.h"
#include <iostream>
#include <fstream>

using std::wstring; using std::map;

ConfigParser::ConfigParser(LPWSTR filename)
{
    // Load some defaults
    config[L"x"] = L"300";
    config[L"y"] = L"300";
    config[L"keyboardFile"] = L"us_intl.kb";
    config[L"backR"] = L"0";
    config[L"backG"] = L"0";
    config[L"backB"] = L"100";
    config[L"pressedR"] = L"255";
    config[L"pressedG"] = L"255";
    config[L"pressedB"] = L"255";
    config[L"looseR"] = L"100";
    config[L"looseG"] = L"100";
    config[L"looseB"] = L"100";
    config[L"fontR"] = L"0";
    config[L"fontG"] = L"0";
    config[L"fontB"] = L"0";
    config[L"pressedFontR"] = L"0";
    config[L"pressedFontG"] = L"0";
    config[L"pressedFontB"] = L"0";
    config[L"mouseSpeed1R"] = L"100";
    config[L"mouseSpeed1G"] = L"100";
    config[L"mouseSpeed1B"] = L"100";
    config[L"mouseSpeed2R"] = L"255";
    config[L"mouseSpeed2G"] = L"255";
    config[L"mouseSpeed2B"] = L"255";
    config[L"fontName"] = L"Arial";
    config[L"fontNameSmall"] = L"Arial";
    config[L"fontSize"] = L"24";
    config[L"fontSizeSmall"] = L"20";
    config[L"fontWidth"] = L"0";
    config[L"fontWidthSmall"] = L"0";
    config[L"debug"] = L"0";
    config[L"hookMouse"] = L"1";
    config[L"trapKB"] = L"0";
    config[L"trapMouse"] = L"0";
    config[L"scrollHold"] = L"50";
    config[L"scrollCounter"] = L"0";
	config[L"mouseSensitivity"] = L"100";

    // Read the general settings
    wstring word;
    wstring value;
    std::wifstream is(filename);
    while (is.good() && is)
    {
        is >> word >> value;
        config[word] = value;
    }
}

COLORREF ConfigParser::GetColor(wstring cat)
{
    wstring wordr = cat;
    wstring wordg = cat;
    wstring wordb = cat;
    wordr += L"R";
    wordg += L"G";
    wordb += L"B";

    return RGB(GetInt(wordr), GetInt(wordg), GetInt(wordb));
}

wstring ConfigParser::GetColorText(wstring cat, wstring prefix)
{
    wstring wordr = cat;
    wstring wordg = cat;
    wstring wordb = cat;
    wordr += L"R";
    wordg += L"G";
    wordb += L"B";
    wstring colortext = prefix;
    colortext += L"(";
    colortext += GetString(wordr);
    colortext += L",";
    colortext += GetString(wordg);
    colortext += L",";
    colortext += GetString(wordb);
    colortext += L")";
    return colortext;
}

void ConfigParser::SaveSettings(LPWSTR filename)
{
  std::wofstream os;
  os.open(filename, std::ios::trunc);
  map<wstring, wstring>::iterator curr,end;
  for( curr = config.begin(), end = config.end();  curr != end;  curr++ )
      os << curr->first << " " << curr->second <<  "\n";
  os.close();
}

bool ConfigParser::HasItem(wstring word)
{
    return config.find(word) != config.end();
}

int ConfigParser::GetInt(wstring word)
{
    if (!HasItem(word)) return -1;
    wstring s = config[word];
    return NBTools::strToInt(s.c_str()); 
}

bool ConfigParser::GetBool(wstring word)
{
    return GetInt(word) == 1;
}

wstring ConfigParser::GetString(wstring word)
{
    if (!HasItem(word)) return L"";
    return ParseIn(config[word]);
}

void ConfigParser::SetInt(wstring word, int value)
{
    SetString(word, std::to_wstring((long long)value));
}

void ConfigParser::SetBool(wstring word, bool value)
{
    SetString(word, value ? L"1" : L"0");
}

void ConfigParser::SetString(wstring word, wstring value)
{
    config[word] = ParseOut(value);
}

void ConfigParser::SetColor(wstring cat, COLORREF color)
{
    wstring wordr = cat;
    wstring wordg = cat;
    wstring wordb = cat;
    wordr += L"R";
    wordg += L"G";
    wordb += L"B";
    SetInt(wordr, GetRValue(color));
    SetInt(wordg, GetGValue(color));
    SetInt(wordb, GetBValue(color));
}

wstring ConfigParser::ParseIn(wstring text)
{
    return NBTools::doReplace(text, L"%20%", L" ");
}

wstring ConfigParser::ParseOut(wstring text)
{
    return NBTools::doReplace(text, L" ", L"%20%");
}
