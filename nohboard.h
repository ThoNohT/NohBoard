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

#include "resource.h"
#include "nbtools.h"
#include "d3dstuff.h"
#include "kbparser.h"
#include "cbuffer.h"
#include "configparser.h"
#include <time.h>
#include <string>
#include <vector>
// Version 0xMMmmbb (Major.minor.build)
#define version 0x001600
#define version_string L"NohBoard v0.16b"
#define keyboardVersion 3
#define configfile L"NohBoard.config"

// I changed from not sizable to sizable windows and now I need to add some magic numbers to the
// widths and heights of the windows because for some reason the current numbers aren't good enough
// anymore, and I don't want to bother forcing everyone to update their keyboard files.
#define extraX 10
#define extraY 10

// Define the maximum interval between two renders / keyboard, mouse samples (30 fps currently).
#define max_interval 33
#define mouseSmooth 5

// Typedefs to make stuff easier
typedef std::vector<std::wstring> StrVect;
typedef std::map<std::wstring, StrVect> StrVectMap;

// Threading
bool bStopping = false;
bool bRestart = false;
bool bRender = false;
bool bRtReady = false;
CRITICAL_SECTION csKB;
D3DStuff *ds;

HHOOK keyboardHook = NULL;
HHOOK mouseHook = NULL;
HWND hWnd;
HINSTANCE hInstMain;

// List of key pressed statuses
std::vector<int> fPressed;
POINT mousePos;
CBuffer<double> mouseDiffX(mouseSmooth);
CBuffer<double> mouseDiffY(mouseSmooth);
clock_t scrollTimers[4] = { 0, 0, 0, 0 };
unsigned int scrollCounters[4] = { 0, 0, 0, 0 };
DWORD lastMouseCapture;
bool shiftDown1 = false;
bool shiftDown2 = false;
bool shiftDown() { return shiftDown1 || shiftDown2; }
bool enableTraps = false;

// Settings window
COLORREF custColors[16];
StrVectMap foundLayouts;
// The following settings require restart and need to be tracked for changes
std::wstring initialLayout;
std::wstring initialLFS, initialSFS, initialLFW, initialSFW, initialLF, initialSF, initialHookMouse, initialTrapMouse;

// Configuration stuff
std::wstring appDir;
ConfigParser * config;
KBInfo *kbinfo;

// Window sizing
float aspect;
float lastw;
float lasth;

enum
{
    ID_LOADSETTINGS=5000,
    ID_EXITNOHBOARD,
    ID_RESETSIZE,
    ID_RESTART
};

enum LoadKBResult
{
    LKB_SUCCESS,
    LKB_LOADED_OTHER_FILE,
    LKB_NOT_FOUND,
    LKB_PARSE_ERROR,
    LKB_WRONG_VERSION
};
