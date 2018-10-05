# NohBoard

NohBoard is a keyboard visualization program. I know certain applications already exist that do just this, display your keyboard on-screen. And even more probably. However, so far I have found none that were both free and easy to use. That's where this program came in, I made it to be free and easy to use, without any fancy graphics, and easily capturable (possibly with chroma key). Furthermore, it's very customizable.

## Rewrite

An initial version was made in C++, this originated from the desire to make something with graphics, and what I knew was [OBS](http://github.com/jp9000/OBS), now replaced by [OBS Studio](http://github.com/jp9000/obs-studio). That's why I started in the same spirit, using C++, and rendering with DirectX. However, having spent most of my time on C# during at least the last decade or so, I decided that I would be much more able to create awesome things in this language. That's when I re-started. Rather than using DirectX, I switched to GDI+, as we're Windows only (I'm sorry, but I just really don't use any other OS, and so far it is still the go-to OS for gaming). No really fancy graphics are required, no 3D is required. This also makes it easier to capture, as a simple window capture in OBS will do the trick now, rather than having to fiddle with game capture which might not work due to a game typically being run at the same time as NohBoard.

## Contributors

**Maintainer / original author**
- Eric "ThoNohT" Bataille (e.c.p.bataille@gmail.com) - Original author

**Contributors**
- Marius "Buttercak3" Becker - Various bugfixes
- Ivan "YaLTeR" Molodetskikh - Added the scroll counter *(NohBoard classic)*
- Michal Mitter - Added button outline *(NohBoard classic)*

**Keyboard layouts**
- BaronBargy
- Burning Fish
- Cloudwolf
- Daigtas
- Floatingthru
- HAJohnny
- Helixia
- joao7yt
- kernel1337
- Krazy
- layarion
- MCCrafterTV
- MtB1980
- TicTacFoe
- ToxicMirror
- WayZHC
- wingsltd
- zolia
- SirDifferential

If you want to contribute, either with code, with keyboard definitions or keyboard styles, feel free to fork this repository and provide your changes via a pull request, or other means of submitting your changes back to me.

## Changelog

For the changelog, see the [Releases](https://github.com/ThoNohT/NohBoard/releases) page.

## Instructions

NohBoard does not contain an installer, it is provided as a completely portable program. Just unpack the ZIP file somewhere, and run NohBoard.exe. The [.NET Framework 4.5](https://www.microsoft.com/en-us/download/details.aspx?id=30653) has to be installed.

By default, no keyboard is loaded. You will see an empty window. All configuration options are accessible via a context menu, which is opened by right-clicking the main window. From here you have several options:
* Open the general application settings
* Open keyboard definitions and styles
* Edit the currently loaded style for the keyboard and the default for its buttons
* If over a key, edit the specific style of this key
* Edit the currently loaded definition

### Capturing

As this version of NohBoard uses GDI+ on a windows form, it is best captured by using Window Capture, both in OBS and OBS Studio. In OBS Studio, there is the possibility of adding a color key, to remove the background color. The best suitable background color for this is green. You could use cropping to only capture a part of the window.

### Folder structure

When editing keyboards and styles, it is important to know the folder structure NohBoard requires. The folder structure for NohBoard as of **v0.3.0** is described below.

- NohBoard/
  - keyboards/
     - *category name*/
        - images/
        - *keyboard name*/
           - keyboard.json
           - *style name*.style
  - global/
    - *style name*.style

Where for every name that is in italics, there can be multiple instances of them in that folder, thus allowing multiple categories, multiple keyboards per categories, and multiple styles per keyboard. The global folder contains styles that are saved as global style, and can be applied to any keyboard definition.

### Editing styles

When editing styles, it is important to know where to place images for this style. It is impossible to provide an absolute path, or even subfolders in the filename box. This is done intentionally to be able to maintain all images in a single folder. This folder is the images/ folder right in the category subdirectory. This should be convenient for keyboard creators, if they create a category for themselves, they can place all their images in the images/ folder in their own category and re-use these images among all their keyboards and/or styles. If keyboard or style specific files are required, one could always use a naming pattern like *keyboard*\_*style*\_*image*.png for example.

The usage of the other fields should be pretty straightforward.

### Edit mode

Since v1.0.0, NohBoard contains an edit mode. Edit mode can be enabled by right clicking a loaded keyboard, and selecting 'Start Editing'. Similarly, it can be disabled by choosing 'Stop Editing'.

From edit mode, many operations are possible:
- Adding or removing elements
- Copy-pasting (using `Ctrl` + `C`, `Ctrl` + `V`) elements
- Moving elements
- Moving edges (between corner points) of elements
- Moving boundaries (corner points) of elements
- Adding or removing boundaries of elements
- Moving text inside elements
- Editing the properties and styles of elements
- Editing the properties and styles of the keyboard
- Changing the z-index of elements
- Resizing keyboard window

Most edits are possibly by dragging the mouse from a point over an element. The element will highlight the manipulation that is going to happen, e.g. while hovering over a boundary point, it will indicate that that boundary point will be manipulated. There are also modifiers possible.

- `Alt` key over an element moves the text inside the element.
- `Alt` key over the keyboard moves all elements at the same time.
- `Ctrl` key over an element will always move the entire element, rather than a boundary or edge that is highlighted.

When an element has a custom text position, it is annoying if the text position resets itself to some computed value whenever a boundary point or edge is moved. To prevent this, there is the *update text position* menu, which when unchecked, leaves the text position as it is whenever a manipulation is done that is not moving the entire element.

When clicking on an element, it is selected. Selections are sticky when it comes to moving the element, if no mouse movement is done while pressing the left mouse key, the selection will be undone or a new element will be selected if it's present where the mouse click event occured. If two or more elements are overlapping, clicking on the overlapping area will cycle through those elements. Ending a selection is done by pressing `Esc`, or `Enter`.

When right clicking an element, or a boundary point or edge of an element, the context menu will contain options for editing the element. These options include opening the properties and style window, removing the entire element, adding boundary points, if an edge is highlighted, and removing a boundary point if one is highlighted. The *move* menu allows you to move elements on top of others, i.e. change the z-index of the element. The properties and style for the entire keyboard can always be opened from the right click menu while in edit mode.

All actions can be undone using `Ctrl` + `Z`, and redone using `Ctrl` + `Shift` + `Z`.

#### Properties and Style

- **Keyboard Properties**
  - **Size**  
On this window, you will be able to change the width and height, in pixels, of the keyboard window. It's more precise than dragging the window borders and corners.

- **Keyboard Style**
  - **Keyboard**
    - **Background**  
The background of the keyboard can be a solid color (chosen by double clicking the colored square on the left of the *Background Color* text) or an image from images/ folder located in the category subdirectory (besides the name, you also need to provide the image format).

  - **Loose Keys**/**Pressed Keys**
    - **Background**  
The background of the loose/pressed keys can be a solid color or an image from images/ folder located in the category subdirectory.

    - **Text**  
Here you can change the font type and color of the loose/pressed keys.

    - **Outline**  
Here you can opt if you want to show the outline of the loose/pressed keys by ticking the CheckBox. You can also change its color and its width.

  - **MouseSpeedIndicator**
    - **General**  
The *Color 1* refers to the color of the mouse speed indicator outer circle as well as the color of the direction indicator when the mouse is moving slowly. The *Color 2* refers to the color of the direction indicator when the mouse is moving quickly. The mouse speed is relative to the *Mouse sensitivity* value in the *Settings*. You can also change its width.

- **Elements Properties**
  - **Keyboard Key Properties**
    - **Text**/**Shift Text**  
Text shown on the element when, respectively, the 'Shift' key isn't/is pressed or 'Caps Lock' isn't/is activated.

    - **Text Position**  
Text position relative to the keyboard itself. The *Center* button automatically calculates the center of the element.

    - **Boundaries**  
Here you are presented with a TextBox where you can enter the *x* and *y* values. You can add this new boundary by using the *Add* button or update an existing one by selecting one in the boundaries list and using the *Update* button. The boundary list in right below where you can select each boundary and remove, move up or move down. The *Rectangle* button is there to make your life easier allowing you to create a rectangle key by just providing the upper left boundary position and the width and height.

    - **Change captalization on Caps Lock key**  
If ticked, this CheckBox will make the element sensitive to 'Caps Lock'. This means that the *Shift Text* will be used if 'Caps Lock **sensitive** keys' option is marked in *Settings*. If this CheckBox is not ticked, the *Shift Text* will only be used when if 'Caps Lock **insensitive** keys' option is marked in *Settings*. This allows all possible combinations when it's about using or not the *Shift Text*. An example of this is if the user wants only the number keys not to follow the *Shift Text*, like *5* turning *%*, she/he can untick this option on all the number keys and in *Settings* let only 'Caps Lock **sensitive** keys' checked.

    - **Key codes**  
Here you are presented with a TextBox where you can enter a key-code. You can add this new key-code by using the *Add* button. The boundary list in right below where you can select each key-code and remove it. The *Detect* button is there to make your life easier allowing you to detect a specific key-code of a key by just pressing the key on your physical keyboard. By adding more than one key-code you can make special shortcut elements, e.g., by adding the 162 and 65 key-codes, you can make an element that will light up when 'Ctrl' + 'A' is used.

  - **Mouse Key Properties**
    - **KeyCode**  
In the drop-down list you can choose which mouse button will light up the element. Currently there're 5 options: *LeftButton*, *MiddleButton*, *RightButton*, *X1Button* and *X2Button* (these last two being the side buttons of the mouse).

    - **Text**  
Text shown on the element.

    - **Text Position**  
Text position relative to the keyboard itself. The *Center* button automatically calculates the center of the element.

    - **Boundaries**  
Here you are presented with a TextBox where you can enter the *x* and *y* values. You can add this new boundary by using the *Add* button or update an existing one by selecting one in the boundaries list and using the *Update* button. The boundary list in right below where you can select each boundary and remove, move up or move down. The *Rectangle* button is there to make your life easier allowing you to create a rectangle key by just providing the upper left boundary position and the width and height.

  - **Mouse Speed Indicator Properties**
    - **Location**  
The *x* and *y* where the center of the mouse speed indicator will be located.

    - **Radius**  
The radius of the mouse speed indicator outer circle.

- **Elements Style**
  - **Loose**/**Pressed**
    - **Overwrite default style**  
If checked, the *Keyboard Style* settings will be overwritten by the settings in this window for the current element.

    - **Background**  
The background of the loose/pressed keys can be a solid color or an image from images/ folder located in the category subdirectory.

    - **Text**  
Here you can change the font type and color of the loose/pressed keys.

    - **Outline**  
Here you can opt if you want to show the outline of the loose/pressed keys by ticking the CheckBox. You can also change its color and its width.

## Keyboard Files

All keyboard files are in the JSON format. NohBoard uses a serialization method that inserts newlines where needed to make the files as humanly readable as possible. This is done in order to encourage users to make their own versions of keyboards. This JSON format may seem a lot more bloated in comparison to the plain-text keyboard files, however it does allow for much greater configurability.

There are two types of keyboard files: Definitions and Styles. Definitions define the size of a keyboard and which elements are inside it. Styles define how these keyboards and their elements look. Styles may be global, or definition specific. By default a style is global, but once element-specific styles are added they become definition specific and can no longer be saved as a global style.

The sections below describe how these files are built-up. I use the shorthand [] to define that something is a list of items, meaning the structure inside of it can be repeated an arbitrary number of times. If multiple types of elements are possible in a list, these are separated by a comma (,). Subtypes are defined the first time they occur, any later time only the name is provided.

### Definitions

```
KeyboardDefinition {
- Version: String
- Width: Int
- Height: Int
- Elements: [
  KeyboardKeyDefinition {
  - Id: Int
  - Boundaries: [ TPoint { - X: Int - Y: Int } ]
  - TextPosition: TPoint
  - KeyCodes: [ Int ]
  - Text: String
  - ShiftText: String
  - ChangeOnCaps: Bool
  },
  MouseKeyDefinition {
  - Id: Int
  - Boundaries: [ TPoint ]
  - TextPosition: TPoint
  - KeyCodes: [ Int ]
  - Text: String
  },
  MouseScrollDefinition {
  - Id: Int
  - Boundaries: [ TPoint ]
  - TextPosition: TPoint
  - KeyCodes: [ Int ]
  - Text: String
  },
  MouseSpeedIndicatorDefinition {
  - Id: Int
  - Location: TPoint
  - Radius: Int
  }]
}
```

Note that only the ``KeyboardKeyDefinition`` allows for multiple key-codes, however for convenience in the implementation, they are all serialized as having a list there. The keyboard key-codes are the ones that belong to the keys, they can be found online. There is one exception: The second Enter button is defined by key-code 1025 because it normally has the same key-code as the regular enter button; 13.

For mouse key-codes, I use the following list:
* Left button = 0
* Right button = 1
* Middle button = 2
* First X button = 3
* Second X button = 4

For the scroll directions, the following codes are used:
* Up = 0
* Down = 1
* Right = 2
* Left = 3

### Styles

```
KeyboardStyle {
- BackgroundColor: Color { - Red: Byte - Green: Byte - Blue: Byte }
- BackgroundImageFileName: String
- DefaultKeyStyle: KeyStyle {
  - Loose: KeySubStyle {
    - Background: Color
    - Text: Color
    - Outline: Color
    - ShowOutline: Bool
    - OutlineWidth: Int
    - Font: Font {
      - FontFamily: String
      - Size: Float
      - Style: FontStyle { Regular = 0 | Bold = 1 | Italic = 2 | Underline = 4 | Strikeout = 8 }
      }
    - BackgroundImageFileName: String
    }
  }
  - Pressed: KeySubStyle
},
- DefaultMouseIndicatorStyle: MouseSpeedIndicatorStyle {
  - InnerColor: Color
  - OuterColor: Color
  - OutlineWidth: Int
}
- ElementStyles: [
  ElementId: Int => Style: KeyStyle,
  ElementId: Int => Style: MouseSpeedIndicatorStyle
  ]
}
```

ElementStyles is a dictionary, or an associative array. here, ``ElementId`` indicates the ``Id`` property of an ``ElementDefinition``. This is the element this style is applicable to.

## Donations

Donations are neither required nor requested. They are, however, always appreciated, and due to some demand, there now is the possibility to [donate](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=FFB9XFRWE5EK2).
Note that donations are to be made purely for appreciation of performed work, and not as a means of prioritizing or requesting future work. They will not in any way impact the speed or order in which features are implemented.

## License

NohBoard is licensed under the GPL version 2. The license agreement is attached in this repository and can be found [here](https://github.com/ThoNohT/NohBoard/blob/master/LICENSE).
