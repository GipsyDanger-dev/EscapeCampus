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

## [0.2.0] - 2026-06-13

### Added
- DocumentData ScriptableObject (DocumentID, Title, Category, Content, IsCritical, Thumbnail)
- DocumentPickup component for world interaction
- DocumentManager singleton for tracking collected documents
- DocumentViewer UI with scrollable content and ESC close
- EvidenceData ScriptableObject (EvidenceID, Title, Description, RelatedDocuments, IsCritical)
- EvidencePickup component for world interaction
- EvidenceManager singleton for tracking collected evidence
- Investigation Journal UI with Documents/Evidence tabs (Press J)
- 5 prototype documents (1 critical: Dean's Confidential Memo)
- 3 prototype evidence items (1 critical: Covert Research Activity)
- Editor tools for generating and assigning prototype content
- Document and Evidence pickup markers in lobby prototype (yellow/green cubes)

### Changed
- SceneBootstrapper now creates DocumentManager, EvidenceManager, DocumentViewer, and InvestigationJournal
- LobbyPrototypeBuilder now spawns document and evidence pickup objects

### Project Structure
- Assets/ScriptableObjects/ - DocumentData.cs, EvidenceData.cs, Documents/, Evidence/
- Assets/Scripts/Documents/ - DocumentManager, DocumentPickup, DocumentViewer
- Assets/Scripts/Evidence/ - EvidenceManager, EvidencePickup
- Assets/Scripts/UI/ - InvestigationJournal
- Assets/Scripts/Editor/ - PrototypeContentGenerator, PrototypeContentPlacer

---

## [0.0.0] - 2026-06-11

### Added
- Initial repository setup
- Documentation folder
