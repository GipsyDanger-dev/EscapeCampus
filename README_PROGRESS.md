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

## Task 005 - Horror Foundation System

### Status: COMPLETED

### What Was Built

#### 1. Horror Level System
- **HorrorManager** singleton with:
  - Horror level range: 0-100
  - `SetHorrorLevel(level)` — Set absolute level
  - `AddHorrorLevel(amount)` — Add to current level
  - `GetHorrorLevel()` — Get current level
  - `GetHorrorStage()` — Get current stage
  - Events: `OnHorrorLevelChanged`, `OnHorrorStageChanged`, `OnHorrorEventTriggered`
  - Entity hooks: `OnEscalationForEntity`, `OnStageEscalation`

#### 2. Horror Stages
| Stage | Level | Description |
|-------|-------|-------------|
| Calm | 0-20 | Normal atmosphere |
| Unease | 20-40 | Subtle discomfort |
| Disturbance | 40-60 | Noticeable anomalies |
| Paranoia | 60-80 | Persistent dread |
| Collapse | 80-100 | Reality breaking down |

#### 3. Horror Event System
- **HorrorEvent** abstract base class with:
  - `eventID`, `eventName`, `eventType`
  - `minHorrorLevel` — Minimum level to trigger
  - `cooldown` — Minimum seconds between triggers (default 30s)
  - `isOneTime` — Can only trigger once
  - `CanExecute()` — Check if event can run
  - `Execute()` — Run the event

#### 4. Five Minimal Horror Events
| Event | Type | Description |
|-------|------|-------------|
| LightFlickerEvent | Environmental | Lights flicker erratically |
| WhisperAudioEvent | Audio | 3D spatial whispers play |
| DoorCloseEvent | Environmental | Door closes on its own |
| DocumentTextShiftEvent | UI | Document text briefly corrupts |
| UIGlitchEvent | UI | Screen-wide glitch overlay |

#### 5. Event Trigger System
- **HorrorEventTrigger** with:
  - Weighted random triggering based on horror stage
  - Minimum 30s cooldown between events
  - Time threshold (60s before first event)
  - Trigger contexts: puzzle solved, document collected, evidence collected, stage change
  - Stage-weighted probability (Calm: 10%, Unease: 30%, Disturbance: 50%, Paranoia: 70%, Collapse: 90%)

#### 6. Horror Escalation
- Per puzzle solved: +10 horror level
- Per evidence collected: +5 horror level
- Per document collected: +3 horror level

#### 7. Save Integration
- Horror level saved/restored
- Triggered event IDs saved/restored
- Autosave on horror events

#### 8. Debug Tools
- **F4** = Increase horror level by 10
- **F5** = Decrease horror level by 10
- **F6** = Reset horror system

#### 9. Semester 14 Hooks
- `OnEscalationForEntity` — Entity can subscribe to level changes
- `OnStageEscalation` — Entity can subscribe to stage transitions
- Hooks are one-way (Horror → Entity)
- Entity behavior responds to horror level, not vice versa

### Horror Philosophy
> Horror bukan monster. Horror adalah perubahan kecil yang tidak konsisten, ketidaksesuaian realitas, gangguan persepsi pemain.

### Updated Project Structure
```
Assets/
└── Scripts/
    └── Horror/
        ├── HorrorLevel.cs
        ├── HorrorManager.cs
        ├── HorrorEvent.cs
        ├── HorrorEventTrigger.cs
        ├── HorrorDebugTool.cs
        ├── SEMESTER14_HOOKS.md
        └── Events/
            ├── LightFlickerEvent.cs
            ├── WhisperAudioEvent.cs
            ├── DoorCloseEvent.cs
            ├── DocumentTextShiftEvent.cs
            └── UIGlitchEvent.cs
```

---

## Task 006 - Level Scripting & Narrative Flow System

### Status: COMPLETED

### What Was Built

#### 1. Story Phase System
- **StoryPhase** enum:
  - `Introduction` — Player arrives, orientation
  - `EarlyInvestigation` — First documents, exploring
  - `FirstAnomaly` — Something feels wrong
  - `DeepInvestigation` — Serious research, puzzles
  - `RealityBreakdown` — Horror escalates, reality扭曲
  - `FinalPreparation` — Before the climax
  - `FinalChase` — The escape sequence
  - `Ending` — Resolution

#### 2. Level Flow Manager
- **LevelFlowManager** singleton with:
  - `SetStoryPhase(phase)` — Set current phase
  - `GetCurrentPhase()` — Get current phase
  - `AdvancePhase()` — Move to next phase
  - `GoToPreviousPhase()` — Move to previous phase
  - `MarkPhaseCompleted(phase)` — Mark phase as done
  - Events: `OnPhaseChanged`, `OnPhaseEntered`, `OnPhaseCompleted`

#### 3. Automatic Transition Rules
| From | To | Condition |
|------|-----|-----------|
| Introduction | EarlyInvestigation | 30 seconds elapsed |
| EarlyInvestigation | FirstAnomaly | 1+ puzzle AND horror > 30 |
| FirstAnomaly | DeepInvestigation | 3+ evidence AND 2+ puzzles |
| DeepInvestigation | RealityBreakdown | horror > 60 |
| RealityBreakdown | FinalPreparation | 5+ documents AND 4+ evidence |
| FinalPreparation | FinalChase | horror > 80 |

#### 4. World State Manager
- **WorldStateManager** singleton with 4 states:
  - `Normal` — Standard atmosphere
  - `Suspicious` — Slightly darker, eerie
  - `Corrupted` — Darker, heavier fog
  - `BrokenReality` — Very dark, dense fog

#### 5. Runtime World Changes
- **DoorController** (IInteractable):
  - Open/close doors
  - Lock/unlock dynamically
  - Integration with WorldStateManager
- **LightingController**:
  - Smooth color transitions per world state
  - Smooth intensity transitions
- **AudioAmbienceController**:
  - Crossfade between state-based ambience clips
  - Volume control

#### 6. Narrative Trigger System
- **NarrativeTrigger** with:
  - Multi-condition evaluation (AND/OR logic)
  - Trigger types: PuzzleCompleted, DocumentCollected, EvidenceCollected, HorrorLevelReached, StoryPhaseEntered, TimePlayed
  - Actions: AdvancePhase, TriggerHorrorEvent, ChangeWorldState
  - One-time or repeating triggers

#### 7. Debug Tools
- **F7** = Next story phase
- **F8** = Previous story phase
- On-screen phase display (yellow text at bottom center)

### Story Phase Flow
```
Introduction (30s)
      │
      ▼
EarlyInvestigation ──→ (1 puzzle + horror>30) ──→ FirstAnomaly
      │                                               │
      │                                               ▼
      │                           (3 evidence + 2 puzzles) ──→ DeepInvestigation
      │                                                           │
      │                                                           ▼
      │                                           (horror>60) ──→ RealityBreakdown
      │                                                           │
      │                                                           ▼
      │                                       (5 docs + 4 ev) ──→ FinalPreparation
      │                                                           │
      │                                                           ▼
      │                                           (horror>80) ──→ FinalChase
      │                                                           │
      │                                                           ▼
      │                                                       Ending
```

### Integration Diagram
```
┌─────────────────────────────────────────────────────────────────┐
│                     LEVEL FLOW INTEGRATION                      │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  DocumentManager ──┐                                            │
│                    ├─→ NarrativeTrigger ──→ LevelFlowManager    │
│  EvidenceManager ──┘         │                    │             │
│                              │                    ▼             │
│  PuzzleManager ──────────────┘         WorldStateManager       │
│                                               │                │
│  HorrorManager ──→ LevelFlowManager ─────────┘                │
│       │                │                                       │
│       │                ▼                                       │
│       │         PhaseTransitionRules                           │
│       │                │                                       │
│       ▼                ▼                                       │
│  HorrorEventTrigger   DoorController                          │
│       │               LightingController                       │
│       │               AudioAmbienceController                  │
│       ▼                                                        │
│  Horror Events (Light, Whisper, Door, Text, UI)               │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

### Updated Project Structure
```
Assets/
└── Scripts/
    └── Core/
        ├── StoryPhase.cs
        ├── LevelFlowManager.cs
        ├── WorldStateManager.cs
        ├── NarrativeTrigger.cs
        ├── DoorController.cs
        ├── LightingController.cs
        ├── AudioAmbienceController.cs
        └── LevelFlowDebugTool.cs
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
| Increase Horror Level | F4 |
| Decrease Horror Level | F5 |
| Reset Horror | F6 |
| Next Story Phase | F7 |
| Previous Story Phase | F8 |

---

## Technical Notes

- **Assembly Definitions** used for clean compilation
- **No external assets** - all code-based
- **Prototype-first** approach - visual polish later
- **Singleton pattern** for managers (DocumentManager, EvidenceManager, SaveManager, PuzzleManager, HorrorManager)
- **Event-driven** architecture for UI updates, autosave, puzzle state, and horror triggers
- **ScriptableObject** data architecture for documents, evidence, and puzzle requirements
- **JSON serialization** for save data (human-readable, debuggable)
- **Save files** stored in `Application.persistentDataPath/Saves/`
- **Abstract base class** pattern for puzzles (PuzzleBase) and horror events (HorrorEvent)
- **Requirement validation** pattern (documents + evidence checks)
- **Weighted random** triggering for horror events based on stage
- **Cooldown system** prevents horror event spam (minimum 30s)
- **Entity hooks** prepared for Semester 14 (one-way Horror → Entity)

---

*Last Updated: 2026-06-13*
