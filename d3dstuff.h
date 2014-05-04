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

//#define D3D_DEBUG_INFO // To log stuff about directx

#include <d3d9.h>
#include <d3dx9core.h>
#include "kbparser.h"
#include "configparser.h"

#pragma once
#pragma comment (lib, "d3d9.lib")
#pragma comment (lib, "d3dx9.lib")

#define pi 3.14159f
#define steps 16

struct VERTEX {
    float x, y, z, rhw;
    DWORD colour;
};

class D3DStuff
{
private:

    LPDIRECT3D9EX d3d;
    LPDIRECT3DDEVICE9 d3ddev;
    LPDIRECT3DVERTEXBUFFER9 v_buffer;

    ID3DXFont *fontBig;
    ID3DXFont *fontSmall;
    KBInfo *kbinfo;
    ConfigParser *config;
public:
    void prepareFrame();

    void finalizeFrame();
    void initD3D(HWND hWnd, KBInfo * kbinfo, ConfigParser * config);
    void cleanD3D(void);
    void drawFillBox(float x1, float y1, float x2, float y2, D3DCOLOR color);
    void drawBox(float x1, float y1, float x2, float y2, D3DCOLOR color);
    void drawText(RECT &rect, D3DCOLOR color, LPWSTR text, bool smallText);
    void drawCircle(float cx, float cy, float radius, D3DCOLOR color);
    void drawFilledCircle(float cx, float cy, float radius, D3DCOLOR color);
    void drawPartFilledCircle(float angle, float cx, float cy, float radius, D3DCOLOR color1, D3DCOLOR color2);
};
