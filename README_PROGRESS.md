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

## Task 003 - Save System Foundation

### Status: COMPLETED

### What Was Built

#### 1. Save Data Model
- **SaveData** serializable class with:
  - Player position (X, Y, Z)
  - Player rotation (Euler X, Y, Z)
  - Collected document IDs (List<string>)
  - Collected evidence IDs (List<string>)
  - Current scene name
  - Play time in seconds
  - Save timestamp
  - Save ID (GUID)
- **PlayerSaveData** helper class for position/rotation conversion

#### 2. Save Manager
- **SaveManager** singleton with:
  - `SaveGame(isAutosave)` - Save to manual or autosave slot
  - `LoadGame(fromAutosave)` - Load from manual or autosave slot
  - `DeleteSave(autosave)` - Delete specific save file
  - `HasSave(autosave)` - Check if save exists
  - `GetCurrentSaveData()` - Get save data without applying
  - JSON serialization to `Application.persistentDataPath/Saves/`
  - Events: OnGameSaved, OnGameLoaded, OnSaveDeleted

#### 3. Save Slots
- **Manual Save** - F5 key, `manual_save.json`
- **Autosave** - Event-based, `autosave.json`

#### 4. Player Save/Restore
- Position saved and restored
- Rotation saved and restored
- CharacterController disabled during teleport to prevent physics issues

#### 5. Document Save/Restore
- Collected document IDs persisted
- On load, ScriptableObjects re-matched by ID
- DocumentManager state restored

#### 6. Evidence Save/Restore
- Collected evidence IDs persisted
- On load, ScriptableObjects re-matched by ID
- EvidenceManager state restored

#### 7. Autosave Triggers
- Document collected → autosave
- Evidence collected → autosave
- Event-based (no timer)

#### 8. Save UI
- **F5** = Manual Save
- **F9** = Load Manual Save
- On-screen message: "Game Saved (Manual Save)"
- On-screen message: "Game Loaded (Manual Save)"
- Message auto-hides after 2 seconds

#### 9. Debug Tool
- **F1** = Toggle debug overlay
- Shows: save file existence, save path, play time
- Shows: collected documents (list with critical tag)
- Shows: collected evidence (list with critical tag)
- Shows: player position (X, Y, Z)
- Auto-updates every 0.5 seconds

### Updated Project Structure
```
Assets/
└── Scripts/
    ├── Save/
    │   ├── SaveData.cs
    │   └── SaveManager.cs
    └── UI/
        ├── SaveUI.cs
        └── SaveDebugTool.cs
```

---

## Task 004 - Puzzle Framework Foundation

### Status: COMPLETED

### What Was Built

#### 1. Puzzle State System
- **PuzzleState** enum:
  - `Locked` — Cannot be started
  - `Unlocked` — Ready to start
  - `InProgress` — Player is working on it
  - `Solved` — Completed

#### 2. Puzzle Manager
- **PuzzleManager** singleton with:
  - `RegisterPuzzle(puzzle)` — Register puzzle in system
  - `GetPuzzleState(id)` — Get current state
  - `SetPuzzleState(id, state)` — Set state (with validation)
  - `IsPuzzleCompleted(id)` — Check if solved
  - `UnlockAllPuzzles()` — Debug: unlock all
  - `SolveCurrentPuzzle()` — Debug: solve active puzzle
  - `GetSaveData()` / `LoadSaveData()` — Save/Load integration
  - Events: `OnPuzzleStateChanged`, `OnPuzzleCompleted`

#### 3. Puzzle Base Class
- **PuzzleBase** abstract class with:
  - `puzzleID` (string)
  - `puzzleName` (string)
  - `initialState` (PuzzleState)
  - `requiredDocuments` (List<DocumentData>)
  - `requiredEvidence` (List<EvidenceData>)
  - `ValidateConditions()` — Checks document/evidence requirements
  - `StartPuzzle()` — Start if unlocked and requirements met
  - `CompletePuzzle()` — Mark as solved
  - `Unlock()` — Change from Locked to Unlocked
  - `OnStateChanged()` — Override for state reactions

#### 4. Puzzle Trigger
- **PuzzleTrigger** (IInteractable) with:
  - State-aware prompts (locked/unlocked/in-progress/solved)
  - Press E to start puzzle
  - Only works if unlocked
  - `OnPuzzleLocked()` virtual method for custom locked behavior

#### 5. Document & Evidence Integration
- PuzzleBase reads from DocumentManager and EvidenceManager
- `ValidateConditions()` checks all required documents collected
- `ValidateConditions()` checks all required evidence collected
- Puzzle cannot start unless requirements are met

#### 6. First Dummy Puzzle
- **PZ_LIBRARY_TIMELINE** (LibraryTimelinePuzzle):
  - Requires: DOC_002 (Student Complaint) and DOC_004 (Maintenance Request)
  - Mechanic: Player reads 2 documents, determines which event happened first
  - UI: Question text + 2 buttons (A or B)
  - Correct answer: Maintenance Request happened first
  - Feedback: Green for correct, red for incorrect
  - Auto-completes after 1.5s delay on correct answer

#### 7. Debug System
- **F2** = Unlock all puzzles
- **F3** = Solve current in-progress puzzle
- Console logging of all puzzle states on debug actions

#### 8. Save System Integration
- Puzzle states saved to `SaveData.puzzleStates`
- `PuzzleSaveEntry` class (puzzleID + state string)
- Puzzle state restored on load
- Autosave triggered on puzzle completion

### Puzzle Flow
```
1. Player collects required documents
2. Puzzle automatically unlocks (or manually via F2)
3. Player interacts with PuzzleTrigger (E key)
4. PuzzleBase validates conditions (documents/evidence)
5. Puzzle UI appears
6. Player solves puzzle
7. Puzzle state → Solved
8. Autosave triggered
```

### Updated Project Structure
```
Assets/
└── Scripts/
    └── Puzzle/
        ├── PuzzleState.cs
        ├── PuzzleManager.cs
        ├── PuzzleBase.cs
        ├── PuzzleTrigger.cs
        ├── LibraryTimelinePuzzle.cs
        └── PuzzleDebugTool.cs
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
| Save Game | F5 |
| Load Game | F9 |
| Save Debug Overlay | F1 |
| Unlock All Puzzles | F2 |
| Solve Current Puzzle | F3 |

---

## Technical Notes

- **Assembly Definitions** used for clean compilation
- **No external assets** - all code-based
- **Prototype-first** approach - visual polish later
- **Singleton pattern** for managers (DocumentManager, EvidenceManager, SaveManager, PuzzleManager)
- **Event-driven** architecture for UI updates, autosave, and puzzle state
- **ScriptableObject** data architecture for documents, evidence, and puzzle requirements
- **JSON serialization** for save data (human-readable, debuggable)
- **Save files** stored in `Application.persistentDataPath/Saves/`
- **Abstract base class** pattern for puzzles (PuzzleBase)
- **Requirement validation** pattern (documents + evidence checks)

---

*Last Updated: 2026-06-13*
