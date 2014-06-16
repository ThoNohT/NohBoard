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
#include <iostream>
#include <sstream>
#include <fstream>
#include <algorithm>

bool KeyIsDown(int code)
{
    if (code < 256)
        return GetKeyState(code) < 0;

    // Special cases (mouse buttons could be 0 and 1 ideally,
    // but let's not screw up all .kb files and just perform this conversion).
    if (code == CKEY_LMBUTTON)
        return GetKeyState(VK_LBUTTON) < 0;
    if (code == CKEY_RMBUTTON)
        return GetKeyState(VK_RBUTTON) < 0;

    // All others, ignore.
    return true;
}

void CheckKeys()
{
    EnterCriticalSection(&csKB);
    auto it = fPressed.cbegin();
    while (it != fPressed.cend()) {
        if (!KeyIsDown(*it))
            it = fPressed.erase(it);
        else
            ++it;
    }
    LeaveCriticalSection(&csKB);
}

void CheckScrolls()
{
    time_t currentClock = NBTools::GetClockMs();
    EnterCriticalSection(&csKB);
    auto it = fPressed.cbegin();
    while (it != fPressed.cend()) {
        if ((*it == CKEY_SCROLL_UP && currentClock > scrollTimers[0])
        || (*it == CKEY_SCROLL_DOWN && currentClock > scrollTimers[1])
        || (*it == CKEY_SCROLL_RIGHT && currentClock > scrollTimers[2])
        || (*it == CKEY_SCROLL_LEFT && currentClock > scrollTimers[3])) {
            scrollCounters[*it - CKEY_SCROLL_UP] = 0; // Reset the scroll count
            it = fPressed.erase(it);
            bRender = true;
        }
        else
            ++it;
    }
    LeaveCriticalSection(&csKB);
}

int CapsLetters(bool changeOnCaps)
{
    return (changeOnCaps && (((GetKeyState(VK_CAPITAL) & 0x0001)!=0) ^ shiftDown()) || (!changeOnCaps && shiftDown()));
}

void RenderMouseSpeed(KeyInfo *key)
{
    float sizeX = key->width / 2;
    float sizeY = key->width / 2;
    float centerX = key->x + sizeX;
    float centerY = key->y + sizeY;

    float endX = centerX + sizeX * (float)mouseDiffX.average() / 2;
    float endY = centerY + sizeY * (float)mouseDiffY.average() / 2;

	// The second color should be averaged over the two specified colours, based upon how far out the thingymabob is.
	float length = (float)min(1.0f, sqrt(pow(mouseDiffX.average(), 2) + pow( mouseDiffY.average(), 2)) / 4);
	int r2 = config->GetInt(L"mouseSpeed1R") * ( 1 - length) + config->GetInt(L"mouseSpeed2R") * length;
	int g2 = config->GetInt(L"mouseSpeed1G") * ( 1 - length) + config->GetInt(L"mouseSpeed2G") * length;
	int b2 = config->GetInt(L"mouseSpeed1B") * ( 1 - length) + config->GetInt(L"mouseSpeed2B") * length;

    float dx = endX - centerX;
    float dy = endY - centerY;
	
	D3DCOLOR color1 = D3DCOLOR_XRGB(config->GetInt(L"mouseSpeed1R"), config->GetInt(L"mouseSpeed1G"), config->GetInt(L"mouseSpeed1B"));
	D3DCOLOR color2 = D3DCOLOR_XRGB(r2, g2, b2);
	
	// The length of the thingymabob
    float r = sqrt(pow(dx, 2) + pow(dy, 2));

	r *= ((float)config->GetInt(L"mouseSensitivity")) / 100;

	r = min(sizeX, r);

	// The edge
    ds->drawCircle(centerX, centerY, sizeX, color1);

	// The center
    ds->drawFilledCircle(centerX, centerY, sizeX / 5, color1);

    if (r > 0.01) {
        // The angle of the thingymabob
        float angle = (dx < 0) * pi + atan(dy / dx);

        float circleX = centerX + sizeX * cos(angle);
        float circleY = centerY + sizeY * sin(angle);
		
        // The actual value
        ds->drawPartFilledCircle(angle, centerX, centerY, r, color1, color2);

        // The end
        ds->drawFilledCircle(circleX, circleY, sizeX / 5,  color2);
    }
}

void render()
{
    if (IsIconic(hWnd)) return;

    ds->prepareFrame();

    // Copy to a local list for rendering
    EnterCriticalSection(&csKB);
    std::vector<int> fpRender(fPressed);
    LeaveCriticalSection(&csKB);

    // Scroll count
    bool scrollCounter = config->GetBool(L"scrollCounter");
    wchar_t scrollCount[16];

    // Loop through all keys defined for this keyboard
    typedef std::map<int, KeyInfo>::iterator it_type;
    for(it_type iterator = kbinfo->definedKeys.begin(); iterator != kbinfo->definedKeys.end(); iterator++)
    {
        KeyInfo * key = &iterator->second;

        if (key->id == CKEY_MOUSESPEED)
        {
            RenderMouseSpeed(key);
            continue;
        }

        RECT rect = { (long)key->x, (long)key->y, (long)(key->x + key->width), (long)(key->y + key->height) };
        if (std::find(fpRender.cbegin(), fpRender.cend(), key->id) != fpRender.cend())
        {
            ds->drawFillBox(key->x, key->y,
                            key->x + key->width, key->y + key->height, 
                            D3DCOLOR_XRGB(config->GetInt(L"pressedR"), config->GetInt(L"pressedG"), config->GetInt(L"pressedB")));

            // If we're rendering the scroll keys, we might want to
            // render the scroll count instead of their names
            if ((key->id == CKEY_SCROLL_UP
                || key->id == CKEY_SCROLL_DOWN
                || key->id == CKEY_SCROLL_LEFT
                || key->id == CKEY_SCROLL_RIGHT)
                && scrollCounter)
            {
                wsprintf(scrollCount, L"%u", scrollCounters[key->id - CKEY_SCROLL_UP]);

                ds->drawText(rect, D3DCOLOR_XRGB(config->GetInt(L"pressedFontR"), config->GetInt(L"pressedFontG"), config->GetInt(L"pressedFontB")),
                            scrollCount, key->smalltext);
            }
            else
            {
                // Render every other key the normal way
                ds->drawText(rect, D3DCOLOR_XRGB(config->GetInt(L"pressedFontR"), config->GetInt(L"pressedFontG"), config->GetInt(L"pressedFontB")),
                            CapsLetters(key->changeOnCaps) ? (LPWSTR)key->shiftText.c_str() : (LPWSTR)key->text.c_str(), key->smalltext);
            }
        }
        else
        {
            ds->drawFillBox(key->x, key->y,
                            key->x + key->width, key->y + key->height, 
                            D3DCOLOR_XRGB(config->GetInt(L"looseR"), config->GetInt(L"looseG"), config->GetInt(L"looseB")));
            ds->drawText(rect, D3DCOLOR_XRGB(config->GetInt(L"fontR"), config->GetInt(L"fontG"), config->GetInt(L"fontB")),
                        CapsLetters(key->changeOnCaps) ? (LPWSTR)key->shiftText.c_str() : (LPWSTR)key->text.c_str(), key->smalltext);
        }
        
    }
    ds->finalizeFrame();

    // Clear the local list
    fpRender.clear();
}

void SaveKBLayout(HWND hwnd)
{
    // Ensure that the keyboard layout is saved
    std::wstring newLayoutStr = NBTools::GetWText(GetDlgItem(hwnd, IDC_KBLAYOUT));
    if (newLayoutStr != initialLayout)
        config->SetString(L"keyboardFile", newLayoutStr);
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
    std::wstring newLayoutStr = NBTools::GetWText(GetDlgItem(hwnd, IDC_KBLAYOUT));
    // Check all settings that might require a restart
    bool settingsDiffer = false;
    settingsDiffer = settingsDiffer || (initialLayout != newLayoutStr);
    settingsDiffer = settingsDiffer || (initialLFS != config->GetString(L"fontSize"));
    settingsDiffer = settingsDiffer || (initialSFS != config->GetString(L"fontSizeSmall"));
    settingsDiffer = settingsDiffer || (initialLFW != config->GetString(L"fontWidth"));
    settingsDiffer = settingsDiffer || (initialSFW != config->GetString(L"fontWidthSmall"));
    settingsDiffer = settingsDiffer || (initialLF != config->GetString(L"fontName"));
    settingsDiffer = settingsDiffer || (initialSF != config->GetString(L"fontNameSmall"));
    settingsDiffer = settingsDiffer || (initialHookMouse != config->GetString(L"hookMouse"));
    settingsDiffer = settingsDiffer || (initialTrapMouse != config->GetString(L"trapMouse"));
    if (!settingsDiffer)
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

void FillFoundLayouts()
{
    std::wstring appDir = NBTools::GetApplicationDirectory();
    appDir += L"*";
    PVOID oldFSRVal;
    Wow64DisableWow64FsRedirection(&oldFSRVal);
    WIN32_FIND_DATA ffd;
    HANDLE hFind = FindFirstFile(appDir.c_str(), &ffd);
    
    // Clear thingy before starting
    typedef StrVectMap::iterator it_type;
    for(it_type cur = foundLayouts.begin(); cur != foundLayouts.end(); cur++)
        cur->second.clear();
    foundLayouts.clear();

    if (INVALID_HANDLE_VALUE != hFind)
    {
        // Loop the files and fill up the found keyboard layouts thingy
        do
        {
            std::wstring name = ffd.cFileName;
            if (!NBTools::EndsWith(name, L".kb")) continue;
            
            // Only parse the right version files
            int kbVersion = KBParser::ParseVersion((LPWSTR)name.c_str());
            if (kbVersion != keyboardVersion) continue;

            // Find the category for this kbInfo
            KBInfo * newKbInfo = KBParser::ParseFile((LPWSTR)name.c_str(), false);
            if (foundLayouts.find(newKbInfo->Category) == foundLayouts.end())
                foundLayouts[newKbInfo->Category] = StrVect();
            // Add the file to its own category
            foundLayouts[newKbInfo->Category].push_back(name);
        } while(FindNextFile(hFind, &ffd) != 0);
        FindClose(hFind);
    }
    Wow64RevertWow64FsRedirection(&oldFSRVal);
}

LRESULT HandleSettingsCommand(HWND hwnd, WPARAM wParam, LPARAM lParam)
{
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
    case IDC_CHANGEPRESSEDFONTCOLOR:
        ChangeColor(hwnd, L"pressedFont", IDC_PRESSEDFONTCOLOR, L"Pressed font color: ");
        RedrawWindow(hwnd, NULL, NULL, RDW_ERASE);
        break;
	case IDC_CHANGEMOUSESPEEDCOLOR1:
		ChangeColor(hwnd, L"mouseSpeed1", IDC_MOUSESPEEDCOLOR1, L"Mouse speed color 1: ");
        RedrawWindow(hwnd, NULL, NULL, RDW_ERASE);
        break;
	case IDC_CHANGEMOUSESPEEDCOLOR2:
		ChangeColor(hwnd, L"mouseSpeed2", IDC_MOUSESPEEDCOLOR2, L"Mouse speed color 2: ");
        RedrawWindow(hwnd, NULL, NULL, RDW_ERASE);
        break;
    case IDC_KBLAYOUT:
        if (HIWORD(wParam) == CBN_SELCHANGE)
            UpdateSettingsTitle(hwnd);
        break;
    case IDC_KBCAT:
        if (HIWORD(wParam) == CBN_SELCHANGE)
        {
            HWND hwndKBLayoutCombo = GetDlgItem(hwnd, IDC_KBLAYOUT);
            std::wstring newCatStr = NBTools::GetWText(GetDlgItem(hwnd, IDC_KBCAT));

            //Clear the combo
            SendMessage(hwndKBLayoutCombo, CB_RESETCONTENT, 0, 0);

            // Fill layouts for the new category
            StrVect layouts = foundLayouts[newCatStr];
            for(StrVect::size_type i = 0; i != layouts.size(); i++)
                SendMessage(hwndKBLayoutCombo, CB_ADDSTRING, 0, (LPARAM)layouts[i].c_str());
            SendMessage(hwndKBLayoutCombo, CB_SETCURSEL, 0, 0);
            UpdateSettingsTitle(hwnd);
        }
        break;
    case IDC_LFONTSIZE:
        if (HIWORD(wParam) == EN_UPDATE)
        {
            config->SetInt(L"fontSize", NBTools::strToInt(NBTools::GetWText(GetDlgItem(hwnd, IDC_LFONTSIZE))));
            UpdateSettingsTitle(hwnd);
        }
        break;
    case IDC_SFONTSIZE:
        if (HIWORD(wParam) == EN_UPDATE)
        {
            config->SetInt(L"fontSizeSmall", NBTools::strToInt(NBTools::GetWText(GetDlgItem(hwnd, IDC_SFONTSIZE))));
            UpdateSettingsTitle(hwnd);
        }
        break;
    case IDC_LFONTWIDTH:
        if (HIWORD(wParam) == EN_UPDATE)
        {
            config->SetInt(L"fontWidth", NBTools::strToInt(NBTools::GetWText(GetDlgItem(hwnd, IDC_LFONTWIDTH))));
            UpdateSettingsTitle(hwnd);
        }
        break;
    case IDC_SFONTWIDTH:
        if (HIWORD(wParam) == EN_UPDATE)
        {
            config->SetInt(L"fontWidthSmall", NBTools::strToInt(NBTools::GetWText(GetDlgItem(hwnd, IDC_SFONTWIDTH))));
            UpdateSettingsTitle(hwnd);
        }
        break;
    case IDC_LFONTNAME:
        if (HIWORD(wParam) == EN_UPDATE)
        {
            config->SetString(L"fontName", NBTools::GetWText(GetDlgItem(hwnd, IDC_LFONTNAME)));
            UpdateSettingsTitle(hwnd);
        }
        break;
    case IDC_SFONTNAME:
        if (HIWORD(wParam) == EN_UPDATE)
        {
            config->SetString(L"fontNameSmall", NBTools::GetWText(GetDlgItem(hwnd, IDC_SFONTNAME)));
            UpdateSettingsTitle(hwnd);
        }
        break;
    case IDC_HOOKMOUSE:
        if (HIWORD(wParam) == BN_CLICKED)
        {
            HWND hwndHMCheck = GetDlgItem(hwnd, IDC_HOOKMOUSE);
            config->SetBool(L"hookMouse", BST_CHECKED == SendMessage(hwndHMCheck, BM_GETCHECK, 0, 0));
            UpdateSettingsTitle(hwnd);
        }
        break;
    case IDC_TRAPKB:
        if (HIWORD(wParam) == BN_CLICKED)
        {
            HWND hwndTKCheck = GetDlgItem(hwnd, IDC_TRAPKB);
            config->SetBool(L"trapKB", BST_CHECKED == SendMessage(hwndTKCheck, BM_GETCHECK, 0, 0));
            UpdateSettingsTitle(hwnd);
        }
        break;
    case IDC_TRAPMOUSE:
        if (HIWORD(wParam) == BN_CLICKED)
        {
            HWND hwndTMCheck = GetDlgItem(hwnd, IDC_TRAPMOUSE);
            config->SetBool(L"trapMouse", BST_CHECKED == SendMessage(hwndTMCheck, BM_GETCHECK, 0, 0));
            UpdateSettingsTitle(hwnd);
        }
        break;
    }
    return 0;
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
                    HWND hwndPressedFontColor = GetDlgItem(hwnd, IDC_PRESSEDFONTCOLOR);
					HWND hwndMouseSpeedColor1 = GetDlgItem(hwnd, IDC_MOUSESPEEDCOLOR1);
					HWND hwndMouseSpeedColor2 = GetDlgItem(hwnd, IDC_MOUSESPEEDCOLOR2);
                    HWND hwndLFSize = GetDlgItem(hwnd, IDC_LFONTSIZE);
                    HWND hwndSFSize = GetDlgItem(hwnd, IDC_SFONTSIZE);
                    HWND hwndLFWidth = GetDlgItem(hwnd, IDC_LFONTWIDTH);
                    HWND hwndSFWidth = GetDlgItem(hwnd, IDC_SFONTWIDTH);
                    HWND hwndLF = GetDlgItem(hwnd, IDC_LFONTNAME);
                    HWND hwndSF = GetDlgItem(hwnd, IDC_SFONTNAME);
                    HWND hwndHookMouse = GetDlgItem(hwnd, IDC_HOOKMOUSE);
                    HWND hwndTrapKB = GetDlgItem(hwnd, IDC_TRAPKB);
                    HWND hwndTrapMouse = GetDlgItem(hwnd, IDC_TRAPMOUSE);
                    SetWindowText(hwndBGColor, config->GetColorText(L"back", L"Background color: ").c_str());
                    SetWindowText(hwndLooseColor, config->GetColorText(L"loose", L"Loose key color: ").c_str());
                    SetWindowText(hwndPressedColor, config->GetColorText(L"pressed", L"Pressed key color: ").c_str());
                    SetWindowText(hwndFontColor, config->GetColorText(L"font", L"Font color: ").c_str());
                    SetWindowText(hwndPressedFontColor, config->GetColorText(L"pressedFont", L"Pressed font color: ").c_str());
					SetWindowText(hwndMouseSpeedColor1, config->GetColorText(L"mouseSpeed1", L"Mouse speed color 1: ").c_str());
					SetWindowText(hwndMouseSpeedColor2, config->GetColorText(L"mouseSpeed2", L"Mouse speed color 2: ").c_str());
                    SetWindowText(hwndLFSize, config->GetString(L"fontSize").c_str());
                    SetWindowText(hwndSFSize, config->GetString(L"fontSizeSmall").c_str());
                    SetWindowText(hwndLFWidth, config->GetString(L"fontWidth").c_str());
                    SetWindowText(hwndSFWidth, config->GetString(L"fontWidthSmall").c_str());
                    SetWindowText(hwndLF, config->GetString(L"fontName").c_str());
                    SetWindowText(hwndSF, config->GetString(L"fontNameSmall").c_str());

                    // set hook mouse checkbox state
                    SendMessage(hwndHookMouse, BM_SETCHECK, config->GetBool(L"hookMouse") ? BST_CHECKED : BST_UNCHECKED, 0);
                    // Set trap kb checkbox state
                    SendMessage(hwndTrapKB, BM_SETCHECK, config->GetBool(L"trapKB") ? BST_CHECKED : BST_UNCHECKED, 0);
                    // Set trap mouse checkbox state
                    SendMessage(hwndTrapMouse, BM_SETCHECK, config->GetBool(L"trapMouse") ? BST_CHECKED : BST_UNCHECKED, 0);

                    // Find all files in the current directory
                    HWND hwndKBCatCombo = GetDlgItem(hwnd, IDC_KBCAT);
                    HWND hwndKBLayoutCombo = GetDlgItem(hwnd, IDC_KBLAYOUT);
                    FillFoundLayouts();
                    // These are the items that need to be highlighted at the start
                    std::wstring currentLayout = config->GetString(L"keyboardFile");
                    std::wstring currentCategory = kbinfo->Category;

                    // Fill categories
                    for(StrVectMap::iterator cur = foundLayouts.begin(); cur != foundLayouts.end(); cur++) {
                        SendMessage(hwndKBCatCombo, CB_ADDSTRING, 0, (LPARAM)cur->first.c_str());

                        if (cur->first == currentCategory) {
                            // Fill layouts based on current category
                            for(StrVect::size_type i = 0; i != cur->second.size(); i++)
                                SendMessage(hwndKBLayoutCombo, CB_ADDSTRING, 0, (LPARAM)cur->second[i].c_str());

                            // Set initial layout
                            SendMessage(hwndKBLayoutCombo, CB_SETCURSEL, SendMessage(hwndKBLayoutCombo, CB_FINDSTRINGEXACT, -1, (LPARAM)currentLayout.c_str()), 0);                    
                        }
                    }
                    // Set initial category
                    SendMessage(hwndKBCatCombo, CB_SETCURSEL, SendMessage(hwndKBCatCombo, CB_FINDSTRINGEXACT, -1, (LPARAM)currentCategory.c_str()), 0);                    
                    UpdateSettingsTitle(hwnd);
                    return TRUE;
                }
                break;
            case WM_CTLCOLORSTATIC:
                {
                    std::wstring colorName;
                    // Set parameters for the different color controls
                    HDC hdc = (HDC)wParam;
                    switch (GetWindowLong((HWND)lParam, GWL_ID))
                    {
                    case IDC_BGCOLOR:
                        colorName = L"back";
                        break;
                    case IDC_LOOSECOLOR:
                        colorName = L"loose";
                        break;
                    case IDC_PRESSEDCOLOR:
                        colorName = L"pressed";
                        break;
                    case IDC_FONTCOLOR:
                        colorName = L"font";
                        break;
                    case IDC_PRESSEDFONTCOLOR:
                        colorName = L"pressedFont";
                        break;
					case IDC_MOUSESPEEDCOLOR1:
                        colorName = L"mouseSpeed1";
                        break;
					case IDC_MOUSESPEEDCOLOR2:
                        colorName = L"mouseSpeed2";
                        break;
                    default:
                        return false;
                        break;
                    }
                    COLORREF c = config->GetColor(colorName);
                    SetTextColor(hdc, c);
                    SetBkColor(hdc, NBTools::IsBright(c) ? RGB(100, 100, 100) : GetSysColor(COLOR_3DFACE));
                    return (INT_PTR)GetSysColorBrush(COLOR_3DFACE);
                }
                break;
            case WM_COMMAND:
                return HandleSettingsCommand(hwnd, wParam, lParam);
                break;
            case WM_CLOSE:
                SaveKBLayout(hwnd);
                EndDialog(hwnd, IDCANCEL);
                if (bRestart)
                    bStopping = true;
                break;
            case WM_NOTIFY:
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
    case ID_RESETSIZE:
        SetWindowPos(hWnd, NULL, 0, 0, kbinfo->width + extraX, kbinfo->height + extraY, SWP_NOMOVE);
        break;
    case ID_RESTART:
        bStopping = true;
        bRestart = true;
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
        bStopping = true;
        SaveWindowPosition(hWnd);
        return 0;
        break;
    case WM_DESTROY:
        bStopping = true;
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
            AppendMenu(hMenu, MF_STRING, ID_RESETSIZE, L"Reset window size");
            AppendMenu(hMenu, MF_STRING, ID_EXITNOHBOARD, L"Exit");
            AppendMenu(hMenu, MF_STRING, ID_RESTART, L"Restart");
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
    case WM_WINDOWPOSCHANGING:
        {
            // Lock aspect ratio
            LPWINDOWPOS wp = (LPWINDOWPOS)lParam;
            wp->cy = (int) ((float)wp->cx / aspect);
        }
        break;
    }

    return DefWindowProc (hWnd, message, wParam, lParam);
}

LRESULT CALLBACK KeyboardHook(int nCode, WPARAM wParam, LPARAM lParam)
{
    // Race conditions occur when hook is processed too early, apparently
    if (!bRtReady) return CallNextHookEx(keyboardHook, nCode, wParam, lParam);

    KBDLLHOOKSTRUCT *info = (KBDLLHOOKSTRUCT*)lParam;

    bool extended = (info->flags & LLKHF_EXTENDED) != 0;
    int code = (extended && info->vkCode == 13) ? CKEY_ENTER : info->vkCode;

    switch (wParam) {
    case WM_KEYDOWN:
    case WM_SYSKEYDOWN:
        {
            // Add to pressed list
            EnterCriticalSection(&csKB);
            if (std::find(fPressed.cbegin(), fPressed.cend(), code) == fPressed.cend())
                fPressed.push_back(code);
            LeaveCriticalSection(&csKB);
            if (info->vkCode == 160) shiftDown1 = true;
            if (info->vkCode == 161) shiftDown2 = true;

            if (config->GetBool(L"debug"))
            {
                // Display the last pressed keycode in the window title
                std::wostringstream convert;
                convert << code;
                std::wstring result = convert.str();
                SetWindowText(hWnd, (LPWSTR)result.c_str());
            }
        }
        
        bRender = true;
        break;

    case WM_KEYUP:
    case WM_SYSKEYUP:
        // We might need to toggle the traps, so yea..
        if(code == VK_SCROLL)
            enableTraps = !enableTraps;

        // Remove from pressed list
        EnterCriticalSection(&csKB);
        if (std::find(fPressed.cbegin(), fPressed.cend(), code) != fPressed.cend())
            fPressed.erase(std::find(fPressed.cbegin(), fPressed.cend(), code));
        LeaveCriticalSection(&csKB);

        if (info->vkCode == 160) shiftDown1 = false;
        if (info->vkCode == 161) shiftDown2 = false;
        bRender = true;
        break;
    }

    // Trap the keyboard input if requested.
    if (config->GetBool(L"trapKB") && enableTraps)
        return 1;
    else
        return CallNextHookEx(keyboardHook, nCode, wParam, lParam);
}


LRESULT CALLBACK MouseHook(int nCode, WPARAM wParam, LPARAM lParam)
{
    // Race conditions occur when hook is processed too early, apparently
    if (!bRtReady) return CallNextHookEx(mouseHook, nCode, wParam, lParam);
    
    if (nCode < 0) return CallNextHookEx(mouseHook, nCode, wParam, lParam);


    int code = 0;
    int subCode = 0;
    switch (wParam)
    {
    case WM_LBUTTONDOWN:
        if (code == 0) code = CKEY_LMBUTTON;
    case WM_RBUTTONDOWN:
        if (code == 0) code = CKEY_RMBUTTON; 
    case WM_MBUTTONDOWN:
        if (code == 0) code = CKEY_MMBUTTON;
    case WM_XBUTTONDOWN:
        if (code == 0 && wParam == WM_XBUTTONDOWN)
        // Prevent this code being executed by fallthrough.
        {
            subCode = HIWORD(((tagMSLLHOOKSTRUCT*)lParam)->mouseData);
            if (subCode == XBUTTON1)
                code = CKEY_X1MBUTTON;
            if (subCode == XBUTTON2)
                code = CKEY_X2MBUTTON;

        }
    case WM_MOUSEWHEEL:
        if (code == 0 && wParam == WM_MOUSEWHEEL)
        {
            subCode = HIWORD(((tagMSLLHOOKSTRUCT*)lParam)->mouseData);
            if (subCode == 120)
            {
                code = CKEY_SCROLL_UP;
                scrollTimers[0] = NBTools::GetClockMs() + config->GetInt(L"scrollHold");
                scrollCounters[0]++;
            }
            if (subCode == 65416)
            {
                code = CKEY_SCROLL_DOWN;
                scrollTimers[1] = NBTools::GetClockMs() + config->GetInt(L"scrollHold");
                scrollCounters[1]++;
            }
        }

    case WM_MOUSEHWHEEL:
        if (code == 0 && wParam == WM_MOUSEHWHEEL)
        {
            subCode = HIWORD(((tagMSLLHOOKSTRUCT*)lParam)->mouseData);
            if (subCode == 120)
            {
                code = CKEY_SCROLL_RIGHT;
                scrollTimers[2] = NBTools::GetClockMs() + config->GetInt(L"scrollHold");
                scrollCounters[2]++;
            }
            if (subCode == 65416)
            {
                code = CKEY_SCROLL_LEFT;
                scrollTimers[3] = NBTools::GetClockMs() + config->GetInt(L"scrollHold");
                scrollCounters[3]++;
            }
        }

        // Add to pressed list
        EnterCriticalSection(&csKB);
        if (std::find(fPressed.cbegin(), fPressed.cend(), code) == fPressed.cend())
            fPressed.push_back(code);
        LeaveCriticalSection(&csKB);

        bRender = true;
        break;

    case WM_LBUTTONUP:
        if (code == 0) code = CKEY_LMBUTTON;
    case WM_RBUTTONUP:
        if (code == 0) code = CKEY_RMBUTTON;
    case WM_MBUTTONUP:
        if (code == 0) code = CKEY_MMBUTTON;
    case WM_XBUTTONUP:
        if (code == 0 && wParam == WM_XBUTTONUP)
        // Prevent this code being executed by fallthrough.
        {
            subCode = HIWORD(((tagMSLLHOOKSTRUCT*)lParam)->mouseData);
            if (subCode == XBUTTON1)
                code = CKEY_X1MBUTTON;
            if (subCode == XBUTTON2)
                code = CKEY_X2MBUTTON;
        }

        // Remove from pressed list
        EnterCriticalSection(&csKB);
        if (std::find(fPressed.cbegin(), fPressed.cend(), code) != fPressed.cend())
            fPressed.erase(std::find(fPressed.cbegin(), fPressed.cend(), code));
        LeaveCriticalSection(&csKB);
        bRender = true;
        break;

    case WM_MOUSEMOVE:
        if (lastMouseCapture == NULL) {
            // We can't calculate a difference the first time anyway, so just se the first capture time.
            mousePos = ((tagMSLLHOOKSTRUCT*)lParam)->pt;
            lastMouseCapture = ((tagMSLLHOOKSTRUCT*)lParam)->time;
            break;
        }

        // We have prior information now, so let's cuddle some bears and hunt a duck.
        DWORD currentTime = ((tagMSLLHOOKSTRUCT*)lParam)->time;

        // Don't do it too quick, if the system don't think time has passed the duck'll divide by zero
        // 30 ducks per bear should be sufficient
        if (currentTime - lastMouseCapture < max_interval)
            break;
        POINT newMousePos = ((tagMSLLHOOKSTRUCT*)lParam)->pt;

        mouseDiffX.add(((double)(newMousePos.x - mousePos.x) / (double)(currentTime - lastMouseCapture)));
        mouseDiffY.add(((double)(newMousePos.y - mousePos.y) / (double)(currentTime - lastMouseCapture)));

        // Update the stored values
        lastMouseCapture = currentTime;
        mousePos = newMousePos;

        bRender = true;
        break;
    }
    
    if (config->GetBool(L"trapMouse") && enableTraps)
        return 1;
    else
        return CallNextHookEx(mouseHook, nCode, wParam, lParam);
}

bool fexists(const wchar_t *filename)
{
  std::wifstream ifile(filename);
  return 0 != ifile;
}

LoadKBResult LoadKeyboard()
{
    bool foundAnotherFile = false;
    if (!fexists((appDir + config->GetString(L"keyboardFile")).c_str()))
    {
        PVOID oldFSRVal;
        Wow64DisableWow64FsRedirection(&oldFSRVal);
        WIN32_FIND_DATA ffd;
        HANDLE hFind = FindFirstFile((appDir + L"*").c_str(), &ffd);
        if (INVALID_HANDLE_VALUE != hFind)
        {
            do
            {
                std::wstring name = ffd.cFileName;
                if (!NBTools::EndsWith(name, L".kb")) continue;

                int kbVersion = KBParser::ParseVersion(name);
                if (kbVersion == keyboardVersion)
                {
                    config->SetString(L"keyboardFile", name);
                    foundAnotherFile = true;
                    break;
                }
            } while(FindNextFile(hFind, &ffd) != 0);
            FindClose(hFind);
        }

        Wow64RevertWow64FsRedirection(&oldFSRVal);
        if (!foundAnotherFile)
        {
            return LKB_NOT_FOUND;
        }
    }

    kbinfo = KBParser::ParseFile((LPWSTR)(appDir + config->GetString(L"keyboardFile")).c_str(), true);

    if (kbinfo == NULL) return LKB_PARSE_ERROR;

    if (kbinfo->KBVersion != keyboardVersion) 
        return LKB_WRONG_VERSION;

    return foundAnotherFile ? LKB_LOADED_OTHER_FILE : LKB_SUCCESS;
}

DWORD WINAPI RenderThread(LPVOID lpParam) 
{ 
    int count = 0;

    while(!bStopping)
    {
        // There's no other way to decide to stop showing a scroll button
        // other than just checking every time we render.
        CheckScrolls();

        if (bRender)
        {
            bRender = false;
            render();

            // Ok, now start processing keys and clicks
            if (!bRtReady) bRtReady = true;
            count = 0;
        } else {
            // We want to reset stuff for mouse movement right away
            if (count < mouseSmooth && config->GetBool(L"hookMouse") && kbinfo->hasMouse)
            {
                mouseDiffX.add(0.0f);
                mouseDiffY.add(0.0f);
                bRender = true;
            }

            count++;
            if (count > 9) {
                CheckKeys();
                bRender = true;
            }
            
        }

        // FPS is defined somewhere else now, but let's stick to it.
        Sleep(max_interval);
    }

    return 0;
} 

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
    hInstMain = hInstance;

    appDir = NBTools::GetApplicationDirectory();

    config = new ConfigParser((LPWSTR)(appDir + configfile).c_str());

    LoadKBResult lkbResult = LoadKeyboard();

    initialLayout = config->GetString(L"keyboardFile"); // Store this so we know if it has changed
    initialLFS = config->GetString(L"fontSize");
    initialSFS = config->GetString(L"fontSizeSmall");
    initialLFW = config->GetString(L"fontWidth");
    initialSFW = config->GetString(L"fontWidthSmall");
    initialLF = config->GetString(L"fontName");
    initialSF = config->GetString(L"fontNameSmall");
    initialHookMouse = config->GetString(L"hookMouse");
    initialTrapMouse = config->GetString(L"trapMouse");
    // Traps don't require restart. Yay!

    switch (lkbResult)
    {
    case LKB_LOADED_OTHER_FILE:
        MessageBox(hWnd, L"The keyboard layout file was not found, another layout file is now opened in stead, please go to settings to select the correct layout.", L"Warning", MB_ICONWARNING | MB_OK);
        break;
    case LKB_WRONG_VERSION:
        MessageBox(hWnd, L"The keyboard layout file has an incorrect version, while this might still work, there is no guarantee.\r\nPlease download the latest keyboard files to ensure correct behaviour.", L"Warning", MB_ICONWARNING | MB_OK);
        break;
    case LKB_PARSE_ERROR:
        MessageBox(hWnd, L"There was an error parsing the keyboard file, I will close now.", L"Keyboard error", MB_ICONERROR | MB_OK);
        return 0;
        break;
    case LKB_NOT_FOUND:
        MessageBox(hWnd, L"No keyboard file could be found, I will close now.", L"Warning", MB_ICONWARNING | MB_OK);
        return 0;
        break;
    case LKB_SUCCESS:
        break;
    }

    // Create the window
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
    hWnd = CreateWindowEx(NULL, L"NohBoardClass", version_string, WS_OVERLAPPED | WS_MINIMIZEBOX | WS_SYSMENU | WS_SIZEBOX,
                          config->GetInt(L"x"), config->GetInt(L"y"), // coordinates of the window
                          kbinfo->width + extraX,                     // width of the window
                          kbinfo->height + extraY,                    // dimensions of the window
                          NULL, NULL,                                 // parent null, menus null
                          hInstance, NULL);                           // application, multiple window
    ShowWindow(hWnd, nCmdShow);

    // Calculate the window aspect ratio
    aspect = (float)((float)(kbinfo->width + extraX) / (float)(kbinfo->height + extraY));

    // Low level keyboard hook
    keyboardHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardHook, NULL, NULL);

    // Low level mouse hook
    if ((config->GetBool(L"hookMouse") && kbinfo->hasMouse) || config->GetBool(L"trapMouse"))
        mouseHook = SetWindowsHookEx(WH_MOUSE_LL, MouseHook, NULL, NULL);
   
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
    bool bStop = false;
    while(!bStop)
    {
        if (bStopping) {
            // Stop handling the keyboard and mouse, before the last message handling,
            // so we can dispatch any still incoming messages
            UnhookWindowsHookEx(keyboardHook);
            if ((config->GetBool(L"hookMouse") && kbinfo->hasMouse) || config->GetBool(L"trapMouse"))
                UnhookWindowsHookEx(mouseHook);
            bStop = true;
        }

        // Wait for a message
        GetMessage(&msg, NULL, 0, 0); TranslateMessage(&msg); DispatchMessage(&msg);
        // Then process any other messages
        while(PeekMessage(&msg, NULL, 0, 0, PM_REMOVE))
        { TranslateMessage(&msg); DispatchMessage(&msg); }
    }

    // Merge message loop and delete critical section
    WaitForSingleObject(hRThread, INFINITE);

    DeleteCriticalSection(&csKB);

    delete kbinfo;

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
