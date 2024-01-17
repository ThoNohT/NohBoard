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

                  :::::::Mapping:::::::
--------------------------------------------------------------
 Button (extra info) - unclick file name
--------------------------------------------------------------

Left Click - l1.png 

Right Click - r1.png 

Scroll Button - m3.png 

Scroll Up - scrollup.png 

Scroll Down - scrolldown.png 

Push Left - pushleft.png

Push Rigth pushright.png 

ScrollWheel Lock - wheellock.png 

G9 - g9.png 

G8 - g8.png 

G7 - g7.png 

G5 - g5.png 

G4 - g4.png 

G4 - g4.png 

Lights are to show multi-button functions [scroll down for more info on light use]
 
 
=============================================================================================================================================================================================
