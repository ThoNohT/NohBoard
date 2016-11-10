# NohBoard

NohBoard is a keyboard visualization program. I know certain applications already exist that do just this, display your keyboard on-screen. And even more probably. However, so far I have found none that were both free and easy to use. That's where this program came in, I made it to be free and easy to use, without any fancy graphics, and easily capturable (possibly with chroma key). Furthermore, it's very customizable.

## Rewrite

An initial version was made in C++, this originated from the desire to make something with graphics, and what I knew was [OBS](http://github.com/jp9000/OBS), now replaced by [OBS Studio](http://github.com/jp9000/obs-studio). That's why I started in the same spirit, using C++, and rendering with DirectX. However, having spent most of my time on C# during at least the last decade or so, I decided that I would be much more able to create awesome things in this language. That's when I re-started. Rather than using DirectX, I switched to GDI+, as we're Windows only (I'm sorry, but I just really don't use any other OS, and so far it is still the go-to OS for gaming). No really fancy graphics are required, no 3D is required. This also makes it easier to capture, as a simple window capture in OBS will do the trick now, rather than having to fiddle with game capture which might not work due to a game typically being run at the same time as NohBoard.

## Contributors

**Maintainer / original author**
- Eric "ThoNohT" Bataille (e.c.p.bataille@gmail.com) - Original author

**Contributors**
- Ivan "YaLTeR" Molodetskikh - Added the scroll counter *(NohBoard classic)*
- Michal Mitter - Added button outline *(NohBoard classic)*

**Keyboard layouts**
- BaronBargy
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
* *In the future also edit the currently loaded definition.*

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