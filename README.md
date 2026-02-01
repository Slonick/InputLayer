> ⚠️ **Project Status: Discontinued**
>
> This plugin is **no longer maintained**.
>
> I’ve decided to step away from Playnite entirely after long-term frustration with the ecosystem of frequently broken and unstable plugins.  
> As a result, **InputLayer will not receive further updates, bug fixes, or support**.
>
> The repository remains available for reference or for anyone who wishes to fork and continue development.

# 🎮 InputLayer (Playnite Plugin)
**InputLayer** is a [Playnite](https://playnite.link/) plugin that enhances your gamepad experience with advanced
keyboard input management capabilities for Windows.

![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/Slonick/InputLayer/.github%2Fworkflows%2Frelease.yml?style=for-the-badge)
![GitHub Downloads (specific asset, latest release)](https://img.shields.io/github/downloads/Slonick/InputLayer/latest/InputLayer.pext?displayAssetName=false&style=for-the-badge&label=Downloads&link=https%3A%2F%2Fgithub.com%2FSlonick%2FInputLayer%2Freleases%2Flatest%2Fdownload%2FInputLayer.pext)

## ✨ Features
- **Controller Button Remapping**: Map gamepad buttons to keyboard keys for enhanced control
- **Customizable Actions**: Create custom button-to-key bindings for any controller button
- **SDL2-Based**: Works with a wide range of gamepad types, not just XInput controllers
- **Background Monitoring**: Lightweight background service that runs seamlessly without impacting performance
- **Live Configuration**: Configure and test button mappings on-the-fly without restarting Playnite
- **Multi-Language Support**: Full localization for English, German, French, Spanish, Russian, and Ukrainian
- **Persistent Settings**: Your configurations are saved and automatically restored
- **User-Friendly Interface**: Intuitive settings panel for easy configuration

## 🎮 Supported Controllers
Thanks to SDL2, this plugin supports a wide variety of gamepads including:
- Xbox controllers (360, One, Series X|S)
- PlayStation controllers (DualShock 4, DualSense)
- Nintendo Switch Pro Controller
- Steam Controller
- And many other DirectInput and XInput devices

## 🔧 Installation
1. [Download the latest `.pext`](https://github.com/Slonick/InputLayer/releases/latest/download/InputLayer.pext).
2. Open the `.pext` file — Playnite will install the plugin automatically.
3. Restart Playnite.

The plugin should now appear in the extensions list.

## ⚙️ Configuration
1. Open Playnite **Desktop Mode**.
2. Go to **Settings** → **Extensions** → **InputLayer**.
3. Configure your preferred input settings.
4. Test your configuration to verify everything works as expected.

## 💡 Recommendations

### Avoiding Button Conflicts

If you're using button combinations in your games (especially combinations involving the Guide/Home button), you may experience conflicts with other software that also monitors controller input.

**Common conflicts:**
- **Steam**: The Steam overlay intercepts the Guide button by default. To fix this, go to **Steam Settings → Controller** and disable **"Enable Guide Button"**.
- **Xbox Game Bar**: Windows' built-in Xbox Game Bar also uses the Guide button. You can disable it in **Windows Settings → Gaming → Xbox Game Bar**.
- **Controller companion apps**: Applications like DS4Windows, reWASD, or other controller mapping software may conflict with InputLayer. Consider disabling them or adjusting their settings to avoid double-mapping.

If your button combinations aren't working as expected, check whether any other software is intercepting the same buttons. You may need to disable or adjust those applications for InputLayer to function properly.

## 🐛 Support
For issues, questions, or contributions:
- Open an issue on [GitHub](https://github.com/Slonick/InputLayer/issues)
- Check existing issues before creating a new one

## 🙏 Acknowledgments
Special thanks to:
- [Playnite](https://github.com/JosefNemec/Playnite) - for the excellent source code that helped guide this plugin's development
- [SDL2-CS](https://github.com/flibitijibibo/SDL2-CS) - for the C# bindings that power the controller support

## ☕ Support the Project
If you find this plugin useful, consider supporting its development:

[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/slonick)

---
Made with ❤️ for the Playnite community
