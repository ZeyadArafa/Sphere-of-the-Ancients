# Sphere of the Ancients

**Sphere of the Ancients** is a 3D action-adventure game developed in Unity. Players control a magical sphere navigating a perilous mountain landscape to reach the Summit Portal. Dodge falling rocks, collect blue shards to maintain your protective shield, and survive the climb.

This game is available as:

* A standalone executable (`.exe`) for immediate play.
* A complete Unity project for developers to explore and expand.

Built using Unity’s **Built-in Render Pipeline**, the game delivers a dynamic and immersive experience on large-scale terrains.

---

## 📑 Table of Contents

* [Overview](#overview)
* [Key Features](#key-features)
* [Running the Executable](#running-the-executable)
* [Development Setup](#development-setup)
* [Usage](#usage)
* [Controls](#controls)
* [Architecture](#architecture)
* [Contributing](#contributing)
* [License](#license)
* [Contact](#contact)

---

## 🧭 Overview

In *Sphere of the Ancients*, players guide a mystical sphere up a procedurally generated mountain to reach the Summit Portal. With a limited shield count, players must avoid falling rocks and collect blue shards to replenish defenses. The game features:

* Real-time hazard spawning
* Terrain-based navigation
* Visual and environmental effects (wind, rain)
* A win/loss system based on shield integrity

---

## ✨ Key Features

### 🎮 Gameplay Mechanics

* **Physics-Based Movement**: Controlled using `WASD`, `Left Shift` (sprint), and `Space` (jump), powered by Rigidbody torque and force-based movement.
* **Shield System**: Start with 29 blue shields. Lose 1 per rock hit, regain 1 per shard collected (max 29).
* **Dynamic Hazards**: Rocks spawn randomly at Z: 30–40, Y: 40, and are destroyed once stationary.
* **Shard System**: Shards spawn every 5 seconds with 25% chance; collect to restore shields.

### 🌍 Environment

* **Terrain**: 2000x2000 units with 600-unit height from a heightmap. Distinct "Pathway" and "Mountains" layers with 2200 trees per type.
* **Wind Effects**: Simulates eastward wind (Turbulence: 0.1, Pulse Magnitude: 0.4, Frequency: 0.5).
* **Rain**: Rain follows the player, including splash particle effects on terrain.

### 🌀 Portal & Game Logic

* **Win Condition**: Reach Summit Portal within 1 unit with at least one shield. Triggers green shield visuals and ascending rotation animation.
* **Lose Condition**: Deplete all shields. The sphere core turns black, input is disabled, and motion freezes.

### 🧪 Bonus Features

* Real-time GUI: Armor, speed, portal distance, and status shown via Unity GUI Labels.
* Visual FX: Transparent shaders, point lights, particle systems.

---

## ▶️ Running the Executable

### 1. Download

* Locate `SphereOfTheAncients.exe` in the `Build` folder or on the [Releases](#) page.
* Ensure required files (`UnityPlayer.dll`, data folders) are in the same directory.

### 2. System Requirements

* **OS**: Windows 7/8/10/11 (64-bit)
* **Graphics**: DirectX 11 compatible GPU
* **Disk Space**: \~500 MB

### 3. Launch

* Double-click `SphereOfTheAncients.exe`.
* Select resolution/graphics settings (if prompted).
* Start the game and follow on-screen instructions.

### 4. Troubleshooting

* Ensure all files are present.
* Update GPU drivers.
* Visit the [Issues](https://github.com/your-username/sphere-of-the-ancients/issues) page for known problems or to submit a bug.

---

## 💻 Development Setup

### 1. Clone the Repository

```bash
git clone https://github.com/your-username/sphere-of-the-ancients.git
cd sphere-of-the-ancients
```

### 2. Install Unity

* Use [Unity Hub](https://unity.com/download) to install **Unity Editor 2022.3 LTS**.
* In Unity Hub, click **Add** and select the project folder.

### 3. Project Configuration

* Open using the **Built-in Render Pipeline**.
* Import all required assets: Portal, Rocks, Shards, Heightmap, Trees, etc.
* Set terrain to (0, 0, 0), size: 2000x2000x600 units.
* Apply terrain layers: "Pathway", "Mountains".
* Place 2200 trees per type with base colliders.
* Set shadow distance to **1000 units** under Lighting Settings.

### 4. Build & Run

* Open the main scene.
* Press **Play** or go to **File > Build Settings** to create a new executable.
* Ensure build target is set to **Windows**.

---

## 🎯 Usage

### Objective

Reach the Summit Portal while maintaining at least **one active shield**.

### Gameplay Summary

* Avoid falling rocks.
* Collect blue shards to regain shield points.
* Monitor stats via on-screen GUI.

### Game States

* **Win**: Enter the portal area (within 1 unit) with at least one shield.
* **Lose**: All shields lost — sphere freezes and turns black.

---

## 🎮 Controls

| Action | Input                              |
| ------ | ---------------------------------- |
| Move   | `W`, `A`, `S`, `D`                 |
| Sprint | `Left Shift`                       |
| Jump   | `Space` (grounded)                 |
| Camera | Auto-follow, aligned with movement |

---

## 🛠️ Architecture

Key C# scripts in the project:

* **`MagicSphereController.cs`**
  Controls sphere movement (torque: 50 walk, 100 sprint), shield logic, win/loss detection.
  Starts at `(180, 15, 3)` with a gold transparent core and point light.

* **`PortalBehavior.cs`**
  Handles portal proximity checks and triggers win sequence (shield color change, animation).

* **`MagicSphereSetup.cs`**
  Initializes core materials, shield shaders (blue/red/green), and point lights.

* **`RockSpawner.cs` / `RockBehavior.cs`**
  Spawns rocks randomly, applies physics, and handles shield collision.

* **`ShardSpawner.cs` / `ShardBehavior.cs`**
  Spawns collectible shards, restores shields on contact, applies light/shader.

* **`RainFollowPlayer.cs` / `RainSplashSpawner.cs`**
  Manages rain effect positioning and splash particles on terrain.

* **`SphereStatsDisplay.cs`**
  Displays GUI stats: armor, speed, portal distance, and game status.

> ⚠️ All scripts require proper Inspector assignments and include debug logs for testing. Refer to inline comments for detailed setup instructions.

---

## 🤝 Contributing

Contributions are welcome! To contribute:

1. **Fork the repository**
2. **Create a feature branch**

```bash
git checkout -b feature/your-feature-name
```

3. **Commit your changes**

```bash
git commit -m "Add your feature"
```

4. **Push and submit a pull request**

```bash
git push origin feature/your-feature-name
```

> Please follow Unity’s coding conventions, test thoroughly, and document your changes.

---

## 📄 License

This project is licensed under the [MIT License](LICENSE).

---

## 📬 Contact
For issues, suggestions, or questions, please open an issue on the repository.

You can also contact the maintainer via GitHub Profile – ZeyadArafa.
---

**Thank you for exploring *Sphere of the Ancients*!**
Happy climbing!

---

