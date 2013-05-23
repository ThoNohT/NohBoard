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

class ConfigParser
{
    std::map<std::wstring, std::wstring> config;

private:
    bool HasItem(std::wstring word);
    static std::wstring ParseIn(std::wstring text);
    static std::wstring ParseOut(std::wstring text);
public:
    void SaveSettings(LPWSTR filename);
    ConfigParser::ConfigParser(LPWSTR filename);
    int GetInt(std::wstring word);
    bool GetBool(std::wstring word);
    std::wstring GetString(std::wstring word);
    COLORREF GetColor(std::wstring cat);
    std::wstring ConfigParser::GetColorText(std::wstring cat, std::wstring prefix);

    void SetInt(std::wstring word, int value);
    void SetBool(std::wstring word, bool value);
    void SetString(std::wstring word, std::wstring value);
    void SetColor(std::wstring cat, COLORREF color);
};
