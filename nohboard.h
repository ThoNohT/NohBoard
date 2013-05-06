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


#include "d3dstuff.h"
#include "kbparser.h"
#include "configparser.h"
#include "llist.h"
#include <string>

#define version 1
#define method 1
#define keyboardVersion 1
#define configfile L"NohBoard.config"

//#define debug // This enables some debugging functions

// Threading
bool bStopping = false;
CRITICAL_SECTION csKB;

D3DStuff *ds;

HHOOK keyboardHook = NULL;
HWND hWnd;

// List of key pressed statuses
lnode *fPressed = NULL;
bool pressed[256] = { false };
bool shiftDown1 = false;
bool shiftDown2 = false;
bool shiftDown() { return shiftDown1 || shiftDown2; }

// Configuration stuff
ConfigParser * config;
KBInfo *kbinfo;
