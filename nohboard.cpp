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
    EnterCriticalSection(&csKB);
#if method == 1
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
                            D3DCOLOR_XRGB(config->GetInt(L"pressedR"), config->GetInt(L"pressedG"), config->GetInt(L"pressedB")));
        }
        else
        {
            ds->drawFillBox(key->x, key->y,
                            key->x + key->width, key->y + key->height, 
                            D3DCOLOR_XRGB(config->GetInt(L"looseR"), config->GetInt(L"looseG"), config->GetInt(L"looseB")));
        }
        ds->drawText(rect, D3DCOLOR_XRGB(config->GetInt(L"fontR"), config->GetInt(L"fontG"), config->GetInt(L"fontB")),
            CapsLetters(key->changeOnCaps) ? (LPWSTR)key->shiftText.c_str() : (LPWSTR)key->text.c_str());
    }
#else if method == 2
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
        if (cur == NULL) break;

        KeyInfo * key = &kbinfo->definedKeys[cur->code];
        RECT rect = { (long)key->x, (long)key->y, (long)(key->x + key->width), (long)(key->y + key->height) };
        ds->drawFillBox(key->x, key->y,
                        key->x + key->width, key->y + key->height, 
                        D3DCOLOR_XRGB(config->GetInt("pressedR"), config->GetInt("pressedG"), config->GetInt("pressedB")));
        ds->drawText(rect, D3DCOLOR_XRGB(config->GetInt("fontR"), config->GetInt("fontG"), config->GetInt("fontB")),
                    CapsLetters(key->changeOnCaps) ? (LPWSTR)key->shiftText.c_str() : (LPWSTR)key->text.c_str());

        // Next pressed key
        cur = cur->next;
    }
#endif
    LeaveCriticalSection(&csKB);
    ds->finalizeFrame();
}


void SaveKBLayout(HWND hwnd)
{
    // Ensure that the keyboard layout is saved
    HWND hwndKBCombo = GetDlgItem(hwnd, IDC_KBLAYOUT);
    int nCharacters = GetWindowTextLength(hwndKBCombo)+1;
    WCHAR * newLayout = new WCHAR[nCharacters];
    GetWindowText(hwndKBCombo, newLayout, nCharacters);
    std::wstring newLayoutStr = newLayout;
    if (newLayoutStr != initialLayout)
        config->SetString(L"keyboardFile", newLayoutStr);
    delete newLayout;
}

void SaveWindowPosition(HWND hwnd)
{
    // Don't store when minimized, it will place the window somewhere in a far away land
    if (IsIconic(hwnd)) return;
    
    // Store window position
    RECT windowPos;
    GetWindowRect(hwnd, &windowPos);
    config->SetInt(L"x", windowPos.left);
    config->SetInt(L"y", windowPos.top);
}

void UpdateSettingsTitle(HWND hwnd)
{
    HWND hwndKBCombo = GetDlgItem(hwnd, IDC_KBLAYOUT);
    int nCharacters = GetWindowTextLength(hwndKBCombo)+1;
    WCHAR * newLayout = new WCHAR[nCharacters];
    GetWindowText(hwndKBCombo, newLayout, nCharacters);
    std::wstring newLayoutStr = newLayout;
    if (newLayoutStr == initialLayout)
    {
        SetWindowText(hwnd, L"NohBoard settings");
        bRestart = false;
    }
    else
    {
        SetWindowText(hwnd, L"NohBoard settings (restart required)");
        bRestart = true;
    }
}

void ChangeColor(HWND hwnd, std::wstring cat, DWORD ctrlID, std::wstring description)
{
    CHOOSECOLOR chooserData;
    ZeroMemory(&chooserData, sizeof(chooserData));
    chooserData.lStructSize = sizeof(chooserData);
    chooserData.hwndOwner = GetParent(hwnd);
    chooserData.Flags = CC_RGBINIT | CC_FULLOPEN;
    chooserData.rgbResult = config->GetColor(cat);
    chooserData.lpCustColors = custColors;

    if(ChooseColor(&chooserData))
    {
        config->SetColor(cat, chooserData.rgbResult);
        HWND hwndLabel = GetDlgItem(hwnd, ctrlID);
        SetWindowText(hwndLabel, config->GetColorText(cat,description).c_str());
    }
}

INT_PTR CALLBACK SettingsProc(HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam)
{
     switch(message)
        {
            case WM_INITDIALOG:
                {
                    for(int i=0; i<16; i++) custColors[i] = 0xC0C0C0;

                    // Set the text for the color labels
                    HWND hwndBGColor = GetDlgItem(hwnd, IDC_BGCOLOR);
                    HWND hwndLooseColor = GetDlgItem(hwnd, IDC_LOOSECOLOR);
                    HWND hwndPressedColor = GetDlgItem(hwnd, IDC_PRESSEDCOLOR);
                    HWND hwndFontColor = GetDlgItem(hwnd, IDC_FONTCOLOR);
                    SetWindowText(hwndBGColor, config->GetColorText(L"back", L"Background color: ").c_str());
                    SetWindowText(hwndLooseColor, config->GetColorText(L"loose", L"Loose key color: ").c_str());
                    SetWindowText(hwndPressedColor, config->GetColorText(L"pressed", L"Pressed key color: ").c_str());
                    SetWindowText(hwndFontColor, config->GetColorText(L"font", L"Font color: ").c_str());

                    // Find all files in the current directory
                    std::wstring appDir = NBTools::GetApplicationDirectory();
                    appDir += L"*";
                    HWND hwndKBCombo = GetDlgItem(hwnd, IDC_KBLAYOUT);
                    PVOID oldFSRVal;
                    Wow64DisableWow64FsRedirection(&oldFSRVal);
                    WIN32_FIND_DATA ffd;
                    HANDLE hFind = FindFirstFile(appDir.c_str(), &ffd);
                    std::wstring currentLayout = config->GetString(L"keyboardFile");
                    if (INVALID_HANDLE_VALUE != hFind)
                    {
                        do
                        {
                            std::wstring name = ffd.cFileName;
                            if (!NBTools::EndsWith(name, L".kb")) continue;
                            SendMessage(hwndKBCombo, CB_ADDSTRING, 0, (LPARAM)name.c_str());
                        } while(FindNextFile(hFind, &ffd) != 0);
                        FindClose(hFind);
                        SendMessage(hwndKBCombo, CB_SETCURSEL, SendMessage(hwndKBCombo, CB_FINDSTRINGEXACT, -1, (LPARAM)currentLayout.c_str()), 0);
                    }
                    Wow64RevertWow64FsRedirection(&oldFSRVal);

                    UpdateSettingsTitle(hwnd);
                    return TRUE;
                }
                break;
            case WM_CTLCOLORSTATIC:
		        if(GetWindowLong((HWND)lParam, GWL_ID) == IDC_BGCOLOR)
		        {
			        HDC hdc = (HDC)wParam;
                    SetTextColor(hdc, config->GetColor(L"back"));
                    SetBkColor(hdc, GetSysColor(COLOR_3DFACE));
			        return (INT_PTR)GetSysColorBrush(COLOR_3DFACE);
		        }
		        if(GetWindowLong((HWND)lParam, GWL_ID) == IDC_LOOSECOLOR)
		        {
			        HDC hdc = (HDC)wParam;
                    SetTextColor(hdc, config->GetColor(L"loose"));
                    SetBkColor(hdc, GetSysColor(COLOR_3DFACE));
			        return (INT_PTR)GetSysColorBrush(COLOR_3DFACE);
		        }
		        if(GetWindowLong((HWND)lParam, GWL_ID) == IDC_PRESSEDCOLOR)
		        {
			        HDC hdc = (HDC)wParam;
                    SetTextColor(hdc, config->GetColor(L"pressed"));
                    SetBkColor(hdc, GetSysColor(COLOR_3DFACE));
			        return (INT_PTR)GetSysColorBrush(COLOR_3DFACE);
		        }
		        if(GetWindowLong((HWND)lParam, GWL_ID) == IDC_FONTCOLOR)
		        {
			        HDC hdc = (HDC)wParam;
                    SetTextColor(hdc, config->GetColor(L"font"));
                    SetBkColor(hdc, GetSysColor(COLOR_3DFACE));
			        return (INT_PTR)GetSysColorBrush(COLOR_3DFACE);
		        }
                break;

            case WM_COMMAND:
                switch (LOWORD(wParam))
                {
                case IDCLOSE:
                    SaveKBLayout(hwnd);
                    EndDialog(hwnd, IDCANCEL);
                    if (bRestart)
                        bStopping = true;
                    break;
                case IDC_CHANGEBGCOLOR:
                    ChangeColor(hwnd, L"back", IDC_BGCOLOR, L"Background color: ");
                    RedrawWindow(hwnd, NULL, NULL, RDW_ERASE);
                    break;
                case IDC_CHANGELOOSECOLOR:
                     ChangeColor(hwnd, L"loose", IDC_LOOSECOLOR, L"Loose key color: ");
                    RedrawWindow(hwnd, NULL, NULL, RDW_ERASE);
                    break;
                case IDC_CHANGEPRESSEDCOLOR:
                    ChangeColor(hwnd, L"pressed", IDC_PRESSEDCOLOR, L"Pressed key color: ");
                    RedrawWindow(hwnd, NULL, NULL, RDW_ERASE);
                    break;
                case IDC_CHANGEFONTCOLOR:
                    ChangeColor(hwnd, L"font", IDC_FONTCOLOR, L"Font color: ");
                    RedrawWindow(hwnd, NULL, NULL, RDW_ERASE);
                    break;
                case IDC_KBLAYOUT:
                    if (HIWORD(wParam) == CBN_SELCHANGE)
                    {
                        UpdateSettingsTitle(hwnd);
                    }
                    break;
                }
                break;
            case WM_CLOSE:
                SaveKBLayout(hwnd);
                EndDialog(hwnd, IDCANCEL);
                if (bRestart)
                    bStopping = true;
                break;
        }

     return FALSE;
}

LRESULT HandleCommand(HWND hWnd, WPARAM wParam, LPARAM lParam)
{
    switch (LOWORD(wParam))
    {
    case ID_LOADSETTINGS:
        DialogBox(hInstMain, MAKEINTRESOURCE(IDD_SETTINGS), hWnd, SettingsProc);
        break;
    case ID_EXITNOHBOARD:
        bStopping = true;
        SaveWindowPosition(hWnd);
        break;
    }

    return 0;
}

LRESULT CALLBACK WindowProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
    switch(message)
    {
    case WM_CLOSE:
        SaveWindowPosition(hWnd);
        break;
        case WM_DESTROY:
            PostQuitMessage(0);
            return 0;
            break;
        case WM_PAINT:
            bRender = true;
            break;
        case WM_RBUTTONUP:
            {
                HMENU hMenu = CreatePopupMenu();
	            AppendMenu(hMenu, MF_STRING, ID_LOADSETTINGS, L"Settings");
                AppendMenu(hMenu, MF_STRING, ID_EXITNOHBOARD, L"Exit");
                SetForegroundWindow(hWnd);
                POINT p;
                GetCursorPos(&p);
                TrackPopupMenu(hMenu, TPM_LEFTALIGN, p.x, p.y, 0, hWnd, NULL);
                DestroyMenu(hMenu);
            }
            break;
        case WM_COMMAND:
            return HandleCommand(hWnd, wParam, lParam);
            break;
    }

    return DefWindowProc (hWnd, message, wParam, lParam);
}

LRESULT CALLBACK KeyboardHook(int nCode , WPARAM wParam , LPARAM lParam)
{
    KBDLLHOOKSTRUCT *info = (KBDLLHOOKSTRUCT*)lParam;

    bool extended = (info->flags & LLKHF_EXTENDED) != 0;
    int code = (extended && info->vkCode == 13) ? 1025 : info->vkCode;


    switch (wParam) {
    case WM_KEYDOWN:
    case WM_SYSKEYDOWN:
        {
            EnterCriticalSection(&csKB);
#if method == 1
            
            pressed[code] = true;
#else if method == 2
            fPressed = insert(fPressed, code);
#endif
            if (info->vkCode == 160) shiftDown1 = true;
            if (info->vkCode == 161) shiftDown2 = true;

            if (config->GetInt(L"debug") == 1)
            {
                // Display the last pressed keycode in the window title
                std::wostringstream convert;
                convert << code;
                std::wstring result = convert.str();
                SetWindowText(hWnd, (LPWSTR)result.c_str());
            }
        }
        LeaveCriticalSection(&csKB);
        bRender = true;
        break;

    case WM_KEYUP:
    case WM_SYSKEYUP:
        EnterCriticalSection(&csKB);
#if method == 1
        pressed[code] = false;
#else if method == 2
        fPressed = remove(fPressed, code);
#endif 
        if (info->vkCode == 160) shiftDown1 = false;
        if (info->vkCode == 161) shiftDown2 = false;
        LeaveCriticalSection(&csKB);
        bRender = true;
        break;
    }

    return CallNextHookEx(keyboardHook, nCode, wParam, lParam);
}

bool LoadKeyboard()
{
    kbinfo = KBParser::ParseFile((LPWSTR)config->GetString(L"keyboardFile").c_str());

    if (kbinfo == NULL) return false;

    if (kbinfo->KBVersion != keyboardVersion) MessageBox(hWnd, L"The keyboard configuration version is unequal to the required version, while this might still work, the results are unpredictable.", L"Warning", MB_ICONWARNING | MB_OK);
    return true;
}

DWORD WINAPI RenderThread(LPVOID lpParam) 
{ 
    int count = 0;
    while(!bStopping)
    {
        if (bRender)
        {
            render();
            bRender = false;
            count = 0;
        } else {
            count++;
            if (count > 9)
                bRender = true;
        }
        // Every 33 ms should be enough (30 fps)
        Sleep(33);
    }
    return 0;
} 

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
    hInstMain = hInstance;
    config = new ConfigParser(configfile);
    initialLayout = config->GetString(L"keyboardFile"); // Store this so we know if it has changed

    if (!LoadKeyboard()) {
        MessageBox(hWnd, L"The keyboard config file could not be read, I will close now.", L"Keyboard error", MB_ICONERROR | MB_OK);
        return 0;
    }

    WNDCLASSEX wc;
    ZeroMemory(&wc, sizeof(WNDCLASSEX));
    wc.cbSize = sizeof(WNDCLASSEX);
    wc.style = CS_HREDRAW | CS_VREDRAW;
    wc.lpfnWndProc = WindowProc;
    wc.hInstance = hInstance;
    wc.hIcon = LoadIcon(hInstance, MAKEINTRESOURCE(IDI_ICON1));
    wc.hCursor = LoadCursor(NULL, IDC_ARROW);
    wc.hbrBackground = (HBRUSH)COLOR_WINDOW;
    wc.lpszClassName = L"NohBoardClass";
    RegisterClassEx(&wc);

    // create the window and use the result as the handle
    hWnd = CreateWindowEx(NULL, L"NohBoardClass", version_string, WS_OVERLAPPED | WS_MINIMIZEBOX | WS_SYSMENU,
                          config->GetInt(L"x"), config->GetInt(L"y"), // position of the window
                          kbinfo->width, kbinfo->height,              // dimensions of the window
                          NULL, NULL,                                 // parent null, menus null
                          hInstance, NULL);                           // application, multiple window
    ShowWindow(hWnd, nCmdShow);

    // Low level keyboard hook
    keyboardHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardHook, NULL, NULL);
   
    ds = new D3DStuff;
    ds->initD3D(hWnd, kbinfo, config);

    // Start threading stuff and critical section
    if (!InitializeCriticalSectionAndSpinCount(&csKB,0x00000400)) 
        return 0;
    DWORD   dwRThreadId;
    HANDLE  hRThread;
    hRThread = CreateThread( NULL, 0, RenderThread, NULL, 0, &dwRThreadId);   

    // Message loop
    MSG msg;
    int counter = 0;
    while(!bStopping)
    {
        while(PeekMessage(&msg, NULL, 0, 0, PM_REMOVE))
        { TranslateMessage(&msg); DispatchMessage(&msg); }

        if(msg.message == WM_QUIT)
            bStopping = true;

        Sleep(5);
    }

    // Merge message loop and delete critical section
    WaitForSingleObject(hRThread, INFINITE);
    DeleteCriticalSection(&csKB);

    // Stop handling the keyboard
    delete kbinfo;
    UnhookWindowsHookEx(keyboardHook);

    // Close direct3d
    ds->cleanD3D();
    delete ds;

    // Save settings and end
    config->SaveSettings(configfile);
    delete config;

    // Restart the program if required
    if (bRestart)
    {
        std::wstring wAppPath = NBTools::GetApplicationPath();
        std::string appPath;
        appPath.assign(wAppPath.begin(), wAppPath.end());
        WinExec(appPath.c_str(), SW_SHOW);
    }
        
    return msg.wParam;
}
