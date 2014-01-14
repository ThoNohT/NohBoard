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


#include <string>
#include <Windows.h>

class NBTools
{
public:
    static std::wstring GetApplicationPath();
    static std::wstring GetApplicationDirectory();
    static bool EndsWith(std::wstring check, std::wstring end);
    static std::wstring doReplace(std::wstring text, std::wstring find, std::wstring replace);
    static bool IsBright(unsigned long c);
    static std::wstring GetWText(HWND hwnd);
    static int strToInt(std::wstring str);
    static bool IsInt(std::wstring & s);
    static int NBTools::GetClockMs();
};


// Custom key definitions, we use 1025 and up these
enum{
    CKEY_ENTER = 1025,  // Enter key on numpad
    CKEY_LMBUTTON,      // Left mouse button
    CKEY_RMBUTTON,      // Right mouse button
    CKEY_MOUSESPEED,    // Mouse speed meter
    CKEY_SCROLL_UP,     // Scroll up
    CKEY_SCROLL_DOWN,   // Scroll down
    CKEY_SCROLL_RIGHT,  // Scroll right
    CKEY_SCROLL_LEFT,   // Scroll left
    CKEY_MMBUTTON,      // Middle mouse button
    CKEY_X1MBUTTON,     // First X-button
    CKEY_X2MBUTTON      // Second X-button
};