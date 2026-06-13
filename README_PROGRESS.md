# Escape Campus - Progress Report

---

## Task 001 - Project Foundation

### Status: COMPLETED

### What Was Built
- Unity 6 project structure with all required folders
- First Person Controller (WASD, mouse look, sprint, crouch, gravity)
- Interaction System (IInteractable, raycast, E key)
- UI System (crosshair, interaction prompts)
- Lobby Prototype Level (5 rooms with primitive cubes)
- MainMenu and LobbyPrototype scenes
- GameManager and SceneBootstrapper

---

## Task 002 - Document System & Evidence System

### Status: COMPLETED

### What Was Built

#### 1. Document System
- **DocumentData** ScriptableObject with fields:
  - DocumentID, Title, Category, Content, IsCritical, Thumbnail
  - Categories: Note, Letter, Report, Academic, Personal, Official, Other
- **DocumentPickup** component (IInteractable)
  - Player presses E to collect
  - Opens Document Viewer automatically
  - Persists in DocumentManager (not destroyed)
- **DocumentManager** singleton
  - Tracks all collected documents
  - Prevents duplicate collection
  - Event: OnDocumentCollected
  - Query by category, critical status
- **DocumentViewer** UI
  - Full-screen overlay
  - Title, category, scrollable content
  - ESC to close
  - Disables player movement while reading

#### 2. Evidence System
- **EvidenceData** ScriptableObject with fields:
  - EvidenceID, Title, Description, RelatedDocuments, IsCritical, Thumbnail
- **EvidencePickup** component (IInteractable)
  - Player presses E to collect
  - Persists in EvidenceManager
- **EvidenceManager** singleton
  - Tracks all collected evidence
  - Prevents duplicate collection
  - Event: OnEvidenceCollected
  - Query by critical status

#### 3. Investigation Journal
- **InvestigationJournal** UI (Press J to open)
  - Tab 1: Documents list
  - Tab 2: Evidence list
  - Detail panel for selected item
  - Shows related documents for evidence
  - ESC to close
  - Disables player movement while open

#### 4. Prototype Content (5 Documents, 3 Evidence)
- **Documents:**
  1. DOC_001: Dean's Confidential Memo (CRITICAL)
  2. DOC_002: Student Complaint Letter
  3. DOC_003: Library Checkout Log
  4. DOC_004: Maintenance Request Form
  5. DOC_005: Professor's Research Notes
- **Evidence:**
  1. EV_001: Strange Lock Installation
  2. EV_002: Covert Research Activity (CRITICAL)
  3. EV_003: Missing Cryptography Book

#### 5. Editor Tools
- `EscapeCampus > Generate Prototype Content` - Creates ScriptableObjects
- `EscapeCampus > Assign Prototype Content to Pickups` - Links data to pickups

### Updated Project Structure
```
Assets/
├── ScriptableObjects/
│   ├── DocumentData.cs
│   ├── EvidenceData.cs
│   ├── Documents/          (5 .asset files)
│   └── Evidence/           (3 .asset files)
└── Scripts/
    ├── Documents/
    │   ├── DocumentManager.cs
    │   ├── DocumentPickup.cs
    │   └── DocumentViewer.cs
    ├── Evidence/
    │   ├── EvidenceManager.cs
    │   └── EvidencePickup.cs
    ├── UI/
    │   └── InvestigationJournal.cs
    └── Editor/
        ├── PrototypeContentGenerator.cs
        └── PrototypeContentPlacer.cs
```

---

## How to Use

### Quick Start
1. Open the project in Unity 6
2. Go to menu: `EscapeCampus > Setup Project`
3. Go to menu: `EscapeCampus > Generate Prototype Content`
4. Press Play on LobbyPrototype scene
5. Walk to yellow/green cubes to collect documents/evidence
6. Press J to open Investigation Journal

### Controls
| Action | Key |
|--------|-----|
| Move | WASD |
| Look | Mouse |
| Sprint | Left Shift |
| Crouch | Left Control |
| Interact | E |
| Open Journal | J |
| Close UI | ESC |

---

## Technical Notes

- **Assembly Definitions** used for clean compilation
- **No external assets** - all code-based
- **Prototype-first** approach - visual polish later
- **Singleton pattern** for managers (DocumentManager, EvidenceManager)
- **Event-driven** architecture for UI updates
- **ScriptableObject** data architecture for documents/evidence

---

*Last Updated: 2026-06-13*
