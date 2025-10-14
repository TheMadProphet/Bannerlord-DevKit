# Bannerlord DevKit by Prophet ⚙️

*Modding & debug toolkit for Mount & Blade II: Bannerlord*

---

## 🧰 Overview

**Bannerlord DevKit** is a **developer toolkit and debugging suite** built to streamline the modding workflow for
*Mount & Blade II: Bannerlord*.  
Created by **Prophet**, this mod provides in-game tools for testing, inspecting, and tweaking game systems in real
time.

---

## ⚙️ Features

- 🧩 **Debug windows** for different systems
- 🪶 **Entity inspectors** (parties, missions, agents, etc.)
- 🧰 **Custom utility buttons and actions** for faster testing
- 💡 **Extensible API** for modders to register their own tools

---

## 🚀 Installation

1. Download the latest release from GitHub.
2. Extract the folder into your Bannerlord installation directory: `Mount & Blade II Bannerlord/Modules/`
3. Enable **Bannerlord DevKit** in the game launcher before starting the game. (Last in load order)

---

## 🧪 Usage

- TODO: Press **F11** (default hotkey) to open the **DevKit Debug Menu**.
- Use the available tabs to access categories such as:
- *Spawner*, *Economy Tools*, *Battle Simulator*, *Entity Inspector*, etc.
- Developers can expand DevKit by adding new debug windows or tools using the provided API.

---
## TODO - Previews

---

## 🧩 For Mod Developers

You can easily extend **Bannerlord DevKit** with your own tools or debug windows.

Example:

```csharp
DevKitAPI.RegisterWindow(new MyCustomDebugWindow());
```

This allows your mod’s debugging or testing functionality to integrate seamlessly into the DevKit interface.

## 📜 Credits

Created and maintained by Prophet (TheMadProphet)
Built for developers who prefer less waiting and more creating.

