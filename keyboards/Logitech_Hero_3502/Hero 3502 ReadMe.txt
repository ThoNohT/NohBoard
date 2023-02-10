++++++++++++++++++++++++++++++++++++++++++++++++++++++++
+                                                      +
+         HH   HH  EEEEEE  RRRRRR     OOOO             +
+         HH   HH  EE      RR   RR   OO  OO            +
+         HH   HH  EEEE    RRRRRR   OO    0O           +
+         HHHHHHH  EEEE    RR  RR   O0    00           +
+         HH   HH  EE      RR   RR   00  00            +
+         HH   HH  EEEEEE  RR    RR   0000             +
+                                                      +
++++++++++++++++++++++++++++++++++++++++++++++++++++++++

         Logitech Hero (G3502) 3D Mouse Map
               by: 3Douglas "3D" Pihl
========================================================


******This is a NohBoard keyboard styling & button structure specifically for the Logitech G3502 aka the "Hero" Logitech gaming mouse******



Hero has 11 programable buttons and this setup takes advantage of this dual button setup and builds on it. By layering the images for the mouse, we can have multiple inputs for the same physical button (more on this later). This allows for almost all Hero mouse clicks to be transversable to the viewers including some multi-button clicks + functions so the users know if specific commands/actions were used. This can be great for more complicated games for example: this one is designed around playing soloPAW style EldenRing (key bindings and mapping at end).

******************************************************************************************************************************

Notes to consider:
  
  - All buttons with the same trigger (exact same) will light up together


  - Adding buttons to watch for works by all the listed buttons have to be pressed for the activation of that button. So, if you have 2 button keycodes for a single visual button, both of those listed keycodes will have to pressed (together) to activate the visiual click change


  - The Left/Right Clicks cannot be triggered in combination with another button that doesn't get confused as other buttons (like scroll up/down)
   
   
  - Left Click is keycode 1, Right Click is keycode 2, Scroll up is keycode 1, Scroll down is keycode 2, M3 (Mouse Wheel Click In) is keycode 3 


  - The triggers (Wheel Lock & Thumb-Trigger) cannot be set for trigger by NohBoard (These buttons may not have a keycode or traditional keycode) but can be for the light up for clicking so some multi-pressed button actions can be relayed through the triggers & lights


  - You can adjust what activates and what buttons are used (alternative avaiable in "OG Buttons" folder)


+ The imgaes naming I choose is like an API [button action .filename]

  - If there's a number in the image name (ie: G7 button's image name starts with the g7 part of it's button name), then the letter on the left of the number will be lower case for un-pressed & uppercase for shift/pressed

  - The only command so far is "clicked"

  - The command for un-pressed is by not having any command (ie: just the button name so for G7 button the un-pressed image name would be g7.png)

  - Non-number containing names stay fully lower cased for both pressed and un-pressed events

  ++ The images are in the images folder in the app "keyboards" folder within it's category folder

    -- "g7.png" is the G7 button unclicked image

    -- "G7clicked.png" is the G7 button clicked image
 
 
 
 
=============================================================================================================================================================================================
 
 
 
 
Note: This is a simple 3D model button catcher for the Hero (G3502) mouse for streamers that use this mouse like soloPAW & strongPAW gamers.  This specific verison is setup for Elden Ring (soloPAW).


*********Because 11 of the buttons are programable you may have to adjust the mouse configuration to better suite your needs. Right click on the NohBoard popup and click "start editing" to be able to load maps, change and edit buttons.*********
 
 
 
 
=============================================================================================================================================================================================

                  :::::::Key Bindings:::::::
--------------------------------------------------------------
 Button (extra info) - unclick file name - keyboard/mouse key
--------------------------------------------------------------

Left Click (non-G button) - l1.png - Left Click

Right Click (non-G button) - r1.png - Right Click

Left Click (G button) - l1.png - e

Right Click (G button) - r1.png - r

Scroll Button (non-G button) - m3.png - e

Scroll Button (G button) - m3.png - Arrow Down

Scroll Up - scrollup.png - Scroll Wheel Up

Scroll Down - scrolldown.png - Scroll Wheel Down

Push Left (scrollwheel, non-G button) - pushleft.png - Shift

Push Left (scrollwheel, G button) - pushleft.png - Arrow Up

Push Right (scrollwheel, non-G button) - pushright.png - q

Push Rigth (scrollwheel, G button) - pushright.png - ESC

ScrollWheel Lock - wheellock.png - (light only, command unused)

G9 (non-G button) - g9.png - Arrow Left (multi {G + L1 + G7})

G9 (G button) - N/A - [Swtich Profiles: in Mouse Program]

G8 (non-G button) - g8.png - Space Bar

G8 (G button) - g8.png - Arrow Left

G7 (non-G button) - g7.png - f

G7 (G button) - g7.png - Arrow Right

G5 (non-G button) - g5.png - w

G5 (G button) - g5.png - g

G4 (non-G button) - g4.png - s

G4 (G button) - g4.png - Left Click

Lights are to show multi-button functions [scroll down for more info on light use]
 
 
 
 
=============================================================================================================================================================================================

                  :::::::Action Bindings:::::::
                  For Elden Ring soloPAW Players
--------------------------------------------------------------
Button (extra info) - Action (extra info) - Menu Action
--------------------------------------------------------------

Left Click (non-G button) - Left Hand

Right Click (non-G button) - Right Hand

Left Click (G button) - Action Button (E) - Accept Action/OKAY

Right Click (G button) - Use Item (belt window)

Scroll Button (non-G button) - Action Button (E) - Accept Action/OKAY

Scroll Button (G button) - Switch Item - Down Button

Push Left (scrollwheel, non-G button) - Camera Reset & Lock On - Up Button

Push Left (scrollwheel, G button) - Switch Sorcery - Back/Exit

Push Right (scrollwheel, non-G button) - Shift Button

Push Rigth (scrollwheel, G button) - Menu

G8 (non-G button) - Doge/Roll

G8 (G button) - Switch Left Hand - Left Arrow

G7 (non-G button) - Jump

G7 (G button) - Switch Right Hand - Right Arrow

G5 (non-G button) - Forward (move character) - Help

G5 (G button) - Map (g)

G4 (non-G button) - Backward (move character)

G4 (G button) - Right Click

L1 (or R1) + Push Right - Strong Attack

R1 + Push Left - Use Skill

************** There is no bound turn left and right for left/right doge/roll **************


Lights are set to multi functions::

1 Light on: 
             a) G + L1 + G8 - D-Pad Quick Call (left)
             b) G + L1 + pushright - D-Pad Quick Call (up)
             c) G + G5 + G8 - Forward + Doge

2 Lights on:
             a) G + L1 + pushleft - Double Hand Weapon action
             b) G + L1 + G7 - D-Pad Quick Call (right)
             c) G + R1 + pushright - Power Attack

3 Lights on:
             a) G + R1 + M3 - Switch sorcery + R button
             b) G + M3 (mouse wheel click in) - Switch Item (belt)

G icon on:
             a) G4 - Backward (move character)
             b) PrintScreen (some devices only)
             c) F6 (was with FN but should just be F6)

--------------------------------------------------------------

To Activate G9 in this setup:
   G + L1 + G7 - D-Pad Quick Call (right)
 
 
=============================================================================================================================================================================================
