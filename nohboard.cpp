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


#include "nohboard.h"
#include <sstream>

int CapsLetters(bool changeOnCaps)
{
    return (changeOnCaps && (((GetKeyState(VK_CAPITAL) & 0x0001)!=0) ^ shiftDown()) || (!changeOnCaps && shiftDown()));
}

void render()
{
    ds->prepareFrame();
#if version == 1
    // Loop through all keys defined for this keyboard
    typedef std::map<int, KeyInfo>::iterator it_type;
    for(it_type iterator = kbinfo->definedKeys.begin(); iterator != kbinfo->definedKeys.end(); iterator++)
    {
        KeyInfo * key = &iterator->second;
        RECT rect = { (long)key->x, (long)key->y, (long)(key->x + key->width), (long)(key->y + key->height) };
        if (pressed[key->id])
        {
            ds->drawFillBox(key->x, key->y,
                            key->x + key->width, key->y + key->height, 
                            D3DCOLOR_XRGB(config->GetInt("pressedR"), config->GetInt("pressedG"), config->GetInt("pressedB")));
        }
        else
        {
            ds->drawFillBox(key->x, key->y,
                            key->x + key->width, key->y + key->height, 
                            D3DCOLOR_XRGB(config->GetInt("looseR"), config->GetInt("looseG"), config->GetInt("looseB")));
        }
        ds->drawText(rect, D3DCOLOR_XRGB(config->GetInt("fontR"), config->GetInt("fontG"), config->GetInt("fontB")),
                        CapsLetters(key->changeOnCaps) ? (LPSTR)key->shiftText.c_str() : (LPSTR)key->text.c_str());
    }
#else if version == 2
    lnode * cur = fPressed;
    // Loop through all pressed nodes
    while (cur != NULL)
    {
        // The key is not defined
        if (kbinfo->definedKeys.find(cur->code) == kbinfo->definedKeys.end())
        {
            // Next pressed key
            cur = cur->next;
        }

        KeyInfo * key = &kbinfo->definedKeys[cur->code];
        RECT rect = { (long)key->x, (long)key->y, (long)(key->x + key->width), (long)(key->y + key->height) };
        ds->drawFillBox(key->x, key->y,
                        key->x + key->width, key->y + key->height, 
                        D3DCOLOR_XRGB(config->GetInt("pressedR"), config->GetInt("pressedG"), config->GetInt("pressedB")));
        ds->drawText(rect, D3DCOLOR_XRGB(config->GetInt("fontR"), config->GetInt("fontG"), config->GetInt("fontB")),
                    CapsLetters(key->changeOnCaps) ? (LPSTR)key->shiftText.c_str() : (LPSTR)key->text.c_str());

        // Next pressed key
        cur = cur->next;
    }
#endif
    ds->finalizeFrame();
}

LRESULT CALLBACK WindowProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
    switch(message)
    {
        case WM_DESTROY:
            {
                PostQuitMessage(0);
                return 0;
            }
            break;
    }

    return DefWindowProc (hWnd, message, wParam, lParam);
}

LRESULT CALLBACK KeyboardHook(int nCode , WPARAM wParam , LPARAM lParam)
{
    KBDLLHOOKSTRUCT *info = (KBDLLHOOKSTRUCT*)lParam;

    switch (wParam) {
    case WM_KEYDOWN:
    case WM_SYSKEYDOWN:
        {
#if version == 1
            pressed[info->vkCode] = true;
#else if version == 2
            fPressed = insert(fPressed, info->vkCode);
#endif
            if (info->vkCode == 160) shiftDown1 = true;
            if (info->vkCode == 161) shiftDown2 = true;
        
#ifdef debug
            // Display the last pressed keycode in the window title
            std::ostringstream convert;
            convert << info->vkCode;
            std::string result = convert.str();
            SetWindowText(hWnd, (LPSTR)result.c_str());
#endif
        }
        break;

    case WM_KEYUP:
    case WM_SYSKEYUP:
#if version == 1
        pressed[info->vkCode] = false;
#else if version == 2
        fPressed = remove(fPressed, info->vkCode);
#endif 
        if (info->vkCode == 160) shiftDown1 = false;
        if (info->vkCode == 161) shiftDown2 = false;
        break;
    }

    return CallNextHookEx(keyboardHook, nCode, wParam, lParam);
}

void SetKeyInfo(KeyInfo &info, int id, float x, float y,
                float width, float height,
                LPSTR text, LPSTR shiftText)
{
    info.id = id;
    info.x = x; info.y = y;
    info.width = width; info.height = height;
    info.text = text; info.shiftText = shiftText;
}

bool LoadKeyboard()
{
    kbinfo = KBParser::ParseFile((LPSTR)config->GetString("keyboardFile").c_str());

    if (kbinfo == NULL) return false;

    if (kbinfo->KBVersion != keyboardVersion) MessageBox(hWnd, "The keyboard configuration version is unequal to the required version, while this might still work, the results are unpredictable.", "Warning", MB_ICONWARNING | MB_OK);
    return true;
}

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
    config = new ConfigParser(configfile);

    if (!LoadKeyboard()) {
        MessageBox(hWnd, "The keyboard config file could not be read, I will close now.", "Keyboard error", MB_ICONERROR | MB_OK);
        return 0;
    }

    WNDCLASSEX wc;
    ZeroMemory(&wc, sizeof(WNDCLASSEX));
    wc.cbSize = sizeof(WNDCLASSEX);
    wc.style = CS_HREDRAW | CS_VREDRAW;
    wc.lpfnWndProc = WindowProc;
    wc.hInstance = hInstance;
    wc.hCursor = LoadCursor(NULL, IDC_ARROW);
    wc.hbrBackground = (HBRUSH)COLOR_WINDOW;
    wc.lpszClassName = "NohBoardClass";
    RegisterClassEx(&wc);

    // create the window and use the result as the handle
    hWnd = CreateWindowEx(NULL, "NohBoardClass", "NohBoard", WS_EX_TOOLWINDOW | WS_EX_LAYERED,
                          300, 300,    // position of the window
                          kbinfo->width, kbinfo->height,    // dimensions of the window
                          NULL, NULL,    // parent null, menus null
                          hInstance, NULL);    // application / multiple window
    ShowWindow(hWnd, nCmdShow);

    // Low level keyboard hook
    keyboardHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardHook, NULL, NULL);
   
    ds = new D3DStuff;
    ds->initD3D(hWnd, kbinfo, config);
    
    // Message loop
    MSG msg;
    bool bStopping = false;
    while(!bStopping)
    {
        while(PeekMessage(&msg, NULL, 0, 0, PM_REMOVE))
        { TranslateMessage(&msg); DispatchMessage(&msg); }

        if(msg.message == WM_QUIT)
            bStopping = true;

        render();

        // Every 20 ms should be enough (20 fps)
        Sleep(50);
    }

    delete kbinfo;

    ds->cleanD3D();

    UnhookWindowsHookEx(keyboardHook);

    delete ds;

    config->SaveSettings(configfile);
    delete config;
    return msg.wParam;
}
