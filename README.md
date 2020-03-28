# ThemeSelector
Theme Selector for Unity UI

Theme Selector was first created for a casino game project where player can switch between themes in the game. It has been made as DLL file so developers cannot modify the core files.

This tool is made with the performance and memory priority. It has built in fade in / out feature for whole screen. When a theme change triggered, it fades out the screen, and nulls sprites, materials, or fonts of all the GOs that are going to be affected from theme change. Then it calls GC to free the memory, and lastly it loads the appropriate assets of the new theme. 

USAGE:
- Define the themes in the "Themes" enum file,
- Put ImageThemeController to scene and use it to change between themes and also define default starting theme,
- Add ImageThemeHolder to the scene and fill each theme with appropriate assets,
  - Sprites, Materials, and Font Files are assets that can be given to theme holder. These assets are referenced as their paths instead of actually referencing them as sprites, materials or font files. This allows system to load only the active theme assets to the memory.
  
Tool uses tagging system, so you need to change tag of the gameobjects that you want to be affected from theme change. System automatically finds GOs with the appropriate tag, and when a theme change triggered, it notifies them.

NOTES:
- It requires no dependancy to any other 3rd party tool,
 - System does not support multiscene and works only in one scene,
