# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

---

## [0.1.0] - 2026-06-13

### Added
- Initial Unity 6 project structure
- First Person Controller with WASD, mouse look, sprint, crouch, gravity
- Interaction system with IInteractable interface and raycast detection
- UI system with crosshair and interaction prompts
- Lobby prototype level using primitive cubes
- MainMenu scene with Start/Quit buttons
- LobbyPrototype scene with full gameplay setup
- Editor menu for one-click project setup
- GameManager singleton for scene management
- SceneBootstrapper for automatic scene initialization
- Assembly definition files for clean compilation
- README_PROGRESS.md documenting current state

### Project Structure
- Assets/Scripts/Core/ - GameManager, SceneBootstrapper, LobbyPrototypeBuilder
- Assets/Scripts/Player/ - FirstPersonController
- Assets/Scripts/Interaction/ - IInteractable, InteractionSystem, InteractableObject
- Assets/Scripts/UI/ - UIManager, CrosshairUI
- Assets/Scripts/Editor/ - SceneSetupEditor

---

## [0.0.0] - 2026-06-11

### Added
- Initial repository setup
- Documentation folder
