![alt text](<Screenshot 2026-01-23 211231.png>)
#🎮 Pokemon XY - Godot C# Edition

A grid-based RPG engine inspired by Pokémon X/Y, built with Godot 4.3+ and C# (.NET 8). This project focuses on a modular architecture using State Machines and custom Loggers for a scalable development experience.

## Features

- Grid-Based Movement: Pixel-perfect tile movement with automated grid snapping.
- Modular State Machine: A custom StateMachine node for managing entity behaviors (Player, NPCs, Battles).
- Custom Logging System: A rich-text, color-coded Logger that tracks namespaces and calling methods for easier debugging.
- Animation State Controller: Signal-driven animation system that handles transitions between walking, idling, and turning.
- Tailwind-Inspired UI: Integrated color palette and design system ready for UI implementation.

## 🗺️ Development Roadmap

Phase 1: Core Engine 🟢
[x] Custom C# Logger

[x] Grid-Based Movement

[x] Signal-Based Animation

[ ] Collision Detection logic

Phase 2: World & Interaction 🟡
[ ] Dialogue System

[ ] NPC AI & Wandering

## Installation

1. Clone the repository:

```
git clone https://github.com/your-username/pokemon-xy-c.git
```

2. Open in Godot:

- Ensure you have the Godot Engine - .NET Edition installed.
- Import project.godot.

3. Build the Solution:

- Click the Hammer Icon in the top right of the Godot editor.
- This generates the .sln and .csproj files for your IDE.

4. Editor Setup:

- Set Globals.cs as an Autoload in Project Settings.

## 📂 Project Structure

```
scripts/
├── core/           # Singletons (Globals, Logger)
├── gameplay/       # Movement, Input, Animation logic
├── utilities/      # State Machines, Math helpers
├── ui/             # Tailwind-integrated UI components
assets/
├── sprites/        # Character and NPC sheets
└── tilesets/       # Environment textures and collisions
```

## Acknowledgements

- [Engine](https://godotengine.org/)
- Inspiration: Pokémon X/Y (Nintendo/GameFreak)
- Tutorial Foundations: Inspired by The Nerdy Canuck's Pokémon Clone series.
