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

## Task 007 - Semester 14 Observation System (Presence Layer 1)

### Status: COMPLETED

### What Was Built

#### 1. Core Principle
> Semester 14 TIDAK MENYERANG. Semester 14 mengamati, mempengaruhi lingkungan, muncul secara tidak konsisten, tidak bisa dijelaskan oleh player.

#### 2. Observation Types
| Type | Behavior | Visibility |
|------|----------|------------|
| Static | Entity visible in distance, motionless | Always visible |
| Peripheral | Only at camera edge, disappears when looked at | PeripheralVisibility component |
| Mirror | Only in reflections, very transparent | Low alpha |
| MissingFrame | Brief flashes at intervals | MissingFrameBlink component |

#### 3. Spawn Rules
- **Horror Level:** >= 40 required
- **Story Phase:** >= FirstAnomaly required
- **Cooldown:** 60-120 seconds (random)
- **Chance:** 30% base, increases with horror stage
- **Duration:** 5 seconds per observation
- **No interaction:** Entity has no collider

#### 4. Environment Response
When Semester 14 is observed:
- Lights slightly desync (random flicker)
- Audio drops for 0.5 seconds
- UI flicker micro event
- Nearby doors randomly lock/unlock

#### 5. Data Tracking
- Total observations count
- Last observation timestamp
- Observation type frequency
- All persisted in save data

#### 6. Debug Tools
- **F9** = Force spawn observation
- **F10** = Clear all observations
- **F11** = Toggle debug panel
- Debug panel shows: active type, distance to player, total observations, last trigger time

### Observation Behavior Explanation

```
┌─────────────────────────────────────────────────────────────┐
│                 SEMESTER 14 OBSERVATION FLOW                 │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│  CanSpawn() Check:                                           │
│  ├─ Not already observing?                                   │
│  ├─ HorrorLevel >= 40?                                       │
│  └─ StoryPhase >= FirstAnomaly?                              │
│         │                                                    │
│         ▼                                                    │
│  Random Chance (30-50%)                                      │
│         │                                                    │
│         ▼                                                    │
│  SelectObservationType()                                     │
│  ├─ Level < 50: Static (70%) / Peripheral (30%)             │
│  ├─ Level 50-70: Static (30%) / Peripheral (30%) / Mirror   │
│  └─ Level > 70: All types including MissingFrame            │
│         │                                                    │
│         ▼                                                    │
│  SpawnObservation()                                          │
│  ├─ Create dark capsule (no collider)                        │
│  ├─ Apply type behavior                                      │
│  ├─ Trigger environment response                             │
│  │   ├─ Light desync                                         │
│  │   ├─ Audio drop (0.5s)                                    │
│  │   ├─ UI flicker                                           │
│  │   └─ Door change                                          │
│  └─ Auto despawn after 5s                                    │
│         │                                                    │
│         ▼                                                    │
│  ResetCooldown (60-120s)                                     │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

### Integration with Horror & Story System

```
┌───────────────────────────────────────────────────────────────┐
│              SEMESTER 14 INTEGRATION DIAGRAM                   │
├───────────────────────────────────────────────────────────────┤
│                                                                │
│  HorrorManager ──→ Semester14Observer                         │
│       │                │                                       │
│       │                ├─ Reads horror level                   │
│       │                ├─ Reads horror stage                   │
│       │                └─ Subscribes to stage changes          │
│       │                                                        │
│  LevelFlowManager ──→ Semester14Observer                      │
│       │                │                                       │
│       │                └─ Checks StoryPhase >= FirstAnomaly    │
│       │                                                        │
│  WorldStateManager ←─ Semester14Observer                      │
│       │                │                                       │
│       │                └─ Door lock/unlock changes             │
│       │                                                        │
│  HorrorEventTrigger ←─ Semester14Observer                    │
│                            │                                   │
│                            └─ Triggers UIGlitchEvent           │
│                                                                │
│  SaveManager ──→ Semester14Observer                           │
│                        │                                       │
│                        └─ Saves/loads observation data         │
│                                                                │
│  Entity Hooks (Semester 14):                                  │
│  ├─ OnObservationStarted(type)                                │
│  ├─ OnObservationEnded(type)                                  │
│  └─ OnEnvironmentResponse                                     │
│                                                                │
│  NO CHASE. NO COMBAT. NO INTERACTION.                         │
│                                                                │
└───────────────────────────────────────────────────────────────┘
```

### Updated Project Structure
```
Assets/
└── Scripts/
    └── Horror/
        └── Semester14/
            ├── ObservationType.cs
            ├── Semester14Observer.cs
            ├── PeripheralVisibility.cs
            ├── MissingFrameBlink.cs
            └── Semester14DebugTool.cs
```

---

## Task 008 - Scripted Horror Set-Piece System

### Status: COMPLETED

### What Was Built

#### 1. Core Principle
> Horror tidak hanya random. Horror harus disengaja, terstruktur, memorable, unavoidable.

#### 2. SetPiece Manager
- **SetPieceManager** singleton with:
  - `RegisterSetPiece(setPiece)` — Register in system
  - `TriggerSetPiece(id)` — Start execution
  - `CompleteSetPiece(id)` — Mark as done
  - `GetActiveSetPiece()` — Get currently running setpiece
  - Events: `OnSetPieceTriggered`, `OnSetPieceCompleted`, `OnSetPieceStateChanged`

#### 3. SetPiece State System
| State | Description |
|-------|-------------|
| Idle | Ready to trigger |
| Triggering | Conditions met, starting |
| Active | Currently executing |
| Ending | Winding down |
| Completed | Done, won't trigger again |

#### 4. SetPiece Types
| Type | Description |
|------|-------------|
| ForcedCamera | Camera control override |
| EnvironmentalCollapse | World changes around player |
| CorridorEvent | Linear sequence in hallway |
| ObservationFreeze | Time freeze with entity |
| EscapeSequence | Running sequence (no AI yet) |

#### 5. Scripted Camera Controller
- **ScriptedCameraController** with:
  - `ForceLookAt(target, duration)` — Smooth look at point
  - `SlowDrag(direction, speed, duration)` — Gradual camera movement
  - `MicroHeadShake(intensity, duration)` — Subtle shake effect
  - `SetFOV(targetFOV, duration)` — Zoom effect
  - `SmoothRotation(target, duration)` — Smooth rotation

#### 6. Trigger System
- **SetPieceTriggerVolume** — Collider-based trigger when player enters
- Conditions: StoryPhase, HorrorLevel, PuzzleCompletion
- Supports both manual and automatic trigger

#### 7. First Mandatory SetPiece: SP_LIBRARY_WHISPER_CORRIDOR
**Description:** Player walks in library corridor

**Sequence:**
1. Lights slowly turn off behind player (3s)
2. Whisper audio increases (2s)
3. Door at end closes by itself (0.8s)
4. Semester 14 appears for 1 frame behind glass (0.1s)
5. Camera forces look backward (1.5s)
6. Nothing is there — pause (1.5s)
7. Player control restored

#### 8. Fail-Safe System
If setpiece fails:
- Skip gracefully
- Restore player control
- Release camera control
- Mark as completed (prevent re-trigger)
- Continue game
- **NO GAME BREAKS**

#### 9. Save Integration
- SetPiece states saved/loaded
- Completed setpieces won't re-trigger

### SetPiece Flow Explanation

```
┌─────────────────────────────────────────────────────────────┐
│                   SETPIECE LIFECYCLE                          │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│  1. REGISTRATION                                             │
│     └─ SetPieceBase registers with SetPieceManager           │
│                                                              │
│  2. TRIGGER CONDITIONS                                       │
│     ├─ StoryPhase >= requiredPhase                           │
│     ├─ HorrorLevel >= requiredHorrorLevel                    │
│     ├─ PuzzleCompleted(requiredPuzzleID)                     │
│     └─ Player enters trigger volume                          │
│                                                              │
│  3. TRIGGER                                                  │
│     ├─ SetPieceManager checks:                               │
│     │   ├─ Not already active?                               │
│     │   ├─ Not completed?                                    │
│     │   └─ CanTrigger() conditions met?                      │
│     └─ State: Idle → Triggering → Active                     │
│                                                              │
│  4. EXECUTION                                                │
│     ├─ LockPlayerControl()                                   │
│     ├─ ScriptedCameraController.TakeControl()                │
│     ├─ Execute phases (coroutine)                            │
│     │   ├─ Phase 1: Lights off behind                        │
│     │   ├─ Phase 2: Whisper increases                        │
│     │   ├─ Phase 3: Door closes                              │
│     │   ├─ Phase 4: S14 flash (1 frame)                      │
│     │   ├─ Phase 5: Force look back                          │
│     │   └─ Phase 6: Nothing there                            │
│     └─ State: Active → Ending → Completed                    │
│                                                              │
│  5. COMPLETION                                               │
│     ├─ UnlockPlayerControl()                                 │
│     ├─ CameraController.ReleaseControl()                     │
│     └─ SetPieceManager.CompleteSetPiece()                    │
│                                                              │
│  6. FAIL-SAFE (if error)                                     │
│     ├─ RestorePlayerControl()                                │
│     ├─ CameraController.ReleaseControl()                     │
│     └─ Mark as Completed (prevent re-trigger)                │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

### Integration Diagram

```
┌───────────────────────────────────────────────────────────────┐
│                   SETPIECE INTEGRATION                         │
├───────────────────────────────────────────────────────────────┤
│                                                                │
│  LevelFlowManager ──→ SetPieceBase                            │
│       │                │                                       │
│       │                └─ Checks requiredPhase                 │
│       │                                                        │
│  HorrorManager ──→ SetPieceBase                               │
│       │                │                                       │
│       │                └─ Checks requiredHorrorLevel           │
│       │                                                        │
│  PuzzleManager ──→ SetPieceBase                               │
│       │                │                                       │
│       │                └─ Checks requiredPuzzleID              │
│       │                                                        │
│  Semester14Observer ←─ LibraryWhisperCorridorSetPiece         │
│       │                                                        │
│       └─ S14 flash during setpiece                            │
│                                                                │
│  WorldStateManager ←─ SetPieceBase                            │
│       │                                                        │
│       └─ Door/light changes during setpiece                   │
│                                                                │
│  ScriptedCameraController                                     │
│       │                                                        │
│       ├─ ForceLookAt                                          │
│       ├─ SlowDrag                                             │
│       ├─ MicroHeadShake                                       │
│       └─ SetFOV                                               │
│                                                                │
│  SaveManager ──→ SetPieceManager                              │
│                        │                                       │
│                        └─ Saves/loads setpiece states          │
│                                                                │
└───────────────────────────────────────────────────────────────┘
```

### Updated Project Structure
```
Assets/
└── Scripts/
    └── Horror/
        └── SetPieces/
            ├── SetPieceState.cs
            ├── SetPieceManager.cs
            ├── SetPieceBase.cs
            ├── ScriptedCameraController.cs
            ├── SetPieceTriggerVolume.cs
            └── LibraryWhisperCorridorSetPiece.cs
```

---

## Task 009 - Narrative Revelation & Ending Structure System

### Status: COMPLETED

### What Was Built

#### 1. Core Principle
> Semua sistem sebelumnya harus mengarah ke: satu kebenaran, satu konflik utama, satu final decision.

#### 2. Ending Manager
- **EndingManager** singleton with:
  - `EvaluateEndingCondition()` — Check which ending is achievable
  - `TriggerEnding(type)` — Start ending sequence
  - `MakeFinalDecision(decision)` — Player makes irreversible choice
  - `SetEndingPhase(phase)` — Progress through ending sequence
  - Events: `OnEndingPhaseChanged`, `OnEndingTriggered`, `OnFinalDecisionMade`

#### 3. Ending Types
| Ending | Name | Description |
|--------|------|-------------|
| Good | The End of the Loop | Destroy ritual, Raka is free |
| Bad | The Cycle Continues | Escape but loop continues |
| Secret | The Third Path | Join the system |
| True | The Truth Revealed | Semester 14 is Raka |

#### 4. Ending Conditions
| Ending | Docs | Evidence | Puzzles | Horror Peak | S14 Obs | Phase |
|--------|------|----------|---------|-------------|---------|-------|
| Good | 4+ (1 crit) | 3+ (1 crit) | 2+ | 50+ | 3+ | FinalPrep |
| Bad | 2+ | 1+ | 1+ | 30+ | 1+ | DeepInv |
| Secret | 5+ (1 crit) | 4+ (1 crit) | 3+ | 40-70 | 5+ | Reality |
| True | 5+ (1 crit) | 4+ (1 crit) | 3+ | 70+ | 7+ | FinalPrep |

#### 5. Truth Reveal Manager
- **TruthRevealManager** with 6 truth fragments:
  1. The Campus History (2+ documents)
  2. The Ritual (1+ critical document)
  3. The Loop (2+ puzzles)
  4. Semester 14's Identity (5+ observations, 3+ evidence)
  5. Raka's Story (4+ documents, 4+ evidence)
  6. The Final Choice (FinalPreparation phase)
- `GenerateFinalNarrativeSummary()` — Complete story recap

#### 6. Final Decision
| Decision | Result | Description |
|----------|--------|-------------|
| Destroy Ritual | Good Ending | End the loop, campus collapses |
| Continue Loop | Bad Ending | Escape, but cycle continues |
| Escape Without Truth | Secret Ending | Join the system |

#### 7. Ending Sequence Flow
```
Final SetPiece triggers
        │
        ▼
WorldState = BrokenReality
        │
        ▼
S14 becomes constant presence
        │
        ▼
Player teleported to Graduation Hall
        │
        ▼
Final Decision UI appears
        │
        ├─ Destroy Ritual → Good Ending
        ├─ Continue Loop → Bad Ending
        └─ Escape → Secret Ending
        │
        ▼
Ending narrative plays
        │
        ▼
Credits
```

#### 8. Ending Narratives
Each ending has full narrative text explaining:
- What happened
- Why it happened
- What it means
- The fate of Raka
- The fate of the campus

#### 9. Save Integration
- Ending type saved/loaded
- Final decision saved/loaded
- Horror level peak saved/loaded
- Truth fragments saved/loaded

### Ending Flow Diagram

```
┌─────────────────────────────────────────────────────────────┐
│                    ENDING FLOW DIAGRAM                        │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│  All Systems Feed Into EndingManager:                        │
│  ├─ DocumentManager → totalDocuments, criticalDocuments      │
│  ├─ EvidenceManager → totalEvidence, criticalEvidence        │
│  ├─ PuzzleManager → puzzlesSolved                            │
│  ├─ HorrorManager → horrorLevelPeak                          │
│  ├─ SetPieceManager → setPiecesCompleted                     │
│  ├─ Semester14Observer → observations                        │
│  └─ LevelFlowManager → currentPhase                          │
│                                                              │
│  EndingManager.EvaluateEndingCondition()                     │
│  ├─ Check True Ending conditions (highest priority)          │
│  ├─ Check Good Ending conditions                             │
│  ├─ Check Secret Ending conditions                           │
│  └─ Check Bad Ending conditions (lowest priority)            │
│                                                              │
│  When StoryPhase = FinalChase:                               │
│  ├─ SetEndingPhase(WorldBreakdown)                           │
│  ├─ WorldState → BrokenReality                               │
│  ├─ S14 becomes constant                                     │
│  ├─ Player → Graduation Hall                                 │
│  ├─ ShowFinalDecisionUI()                                    │
│  │   ├─ Destroy Ritual → TriggerEnding(Good)                 │
│  │   ├─ Continue Loop → TriggerEnding(Bad)                   │
│  │   └─ Escape → TriggerEnding(Secret)                       │
│  └─ PlayEndingSequence() → Credits                           │
│                                                              │
│  TruthRevealManager:                                         │
│  ├─ Evaluates fragments based on collected data              │
│  ├─ Reveals fragments as conditions met                      │
│  └─ Generates final narrative summary                        │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

### Narrative Integration Explanation

```
┌───────────────────────────────────────────────────────────────┐
│                 NARRATIVE INTEGRATION                           │
├───────────────────────────────────────────────────────────────┤
│                                                                │
│  DOCUMENTS ──→ Truth Fragments ──→ Final Narrative             │
│       │              │                    │                     │
│       │              ▼                    ▼                     │
│       │     "Campus History"      "What you learned"           │
│       │     "The Ritual"                                        │
│       │                                                        │
│  EVIDENCE ──→ Truth Fragments ──→ Ending Conditions            │
│       │              │                    │                     │
│       │              ▼                    ▼                     │
│       │     "Raka's Story"        "What ending you deserve"    │
│       │     "S14 Identity"                                     │
│       │                                                        │
│  PUZZLES ──→ Truth Fragments ──→ Ending Requirements           │
│       │              │                    │                     │
│       │              ▼                    ▼                     │
│       │     "The Loop"            "Did you solve enough?"      │
│       │                                                        │
│  HORROR ──→ Peak Tracking ──→ Ending Tier                     │
│       │              │                    │                     │
│       │              ▼                    ▼                     │
│       │     "What you endured"   "How much truth you can take" │
│       │                                                        │
│  S14 OBSERVATIONS ──→ Truth Fragments ──→ True Ending         │
│       │              │                    │                     │
│       │              ▼                    ▼                     │
│       │     "S14 Identity"        "Understanding Raka"         │
│       │                                                        │
│  SETPIECES ──→ Ending Conditions ──→ Final Chase Trigger       │
│       │              │                    │                     │
│       │              ▼                    ▼                     │
│       │     "Scripted moments"    "Climax readiness"           │
│                                                                │
│  ALL SYSTEMS CONVERGE INTO:                                    │
│  ├─ One truth (Raka = S14 = the loop)                         │
│  ├─ One conflict (the ritual)                                  │
│  └─ One final decision (destroy/continue/escape)               │
│                                                                │
└───────────────────────────────────────────────────────────────┘
```

### Updated Project Structure
```
Assets/
└── Scripts/
    ├── Core/
    │   └── Ending/
    │       ├── EndingType.cs
    │       ├── EndingConditions.cs
    │       ├── EndingManager.cs
    │       └── TruthRevealManager.cs
    └── UI/
        └── EndingUI.cs
```

---

## Task 010 - Game Pacing & Experience Director System

### Status: COMPLETED

### What Was Built

#### 1. Core Principle
> Game bukan hanya sistem. Game adalah pengalaman yang diatur ritmenya: tension, relief, discovery, shock, silence.

#### 2. Experience Director
- **ExperienceDirector** singleton with:
  - `EvaluateGameState()` — Analyze all systems for pacing
  - `AdjustPacing()` — Modify horror/event frequency
  - `TriggerNarrativeBeat(beat)` — Set current beat
  - `ControlTensionCurve(target, duration)` — Smooth tension control
  - `SetSafeZone(safe)` — Enter/exit safe zone
  - `ShouldAllowHorrorEvent()` — Gate horror events
  - `ShouldAllowSetPiece()` — Gate setpieces
  - Events: `OnTensionStateChanged`, `OnBeatChanged`, `OnTensionLevelChanged`, `OnSafeZoneChanged`

#### 3. Tension Curve System
| State | Level | Description |
|-------|-------|-------------|
| Calm | 0-20 | Peaceful, exploring |
| Suspense | 20-40 | Something feels off |
| Unease | 40-60 | Active discomfort |
| Fear | 60-80 | Real danger present |
| Panic | 80-100 | Overwhelming terror |

#### 4. Narrative Beat System
| Beat | Purpose | Tension Effect |
|------|---------|----------------|
| Exploration | Player explores freely | Neutral |
| Discovery | Finding documents/evidence | +5 |
| Horror | Horror event active | +10 |
| SetPiece | Scripted moment | +20 |
| Revelation | Truth fragment revealed | +5 |
| Silence | Breathing space | -15 |

#### 5. Automatic Pacing Rules
- Tension builds from horror level, recent events, story phase
- Tension decays naturally over time
- Safe zones accelerate decay
- Beats rotate dynamically based on state
- Too many horror events → force silence
- Too many setpieces → force silence

#### 6. Safe Zones
- **SafeZone** component (collider trigger)
- Player enters → tension decays faster
- Horror events suppressed
- Setpieces blocked
- Narrative breathing space

#### 7. Dynamic Event Control
- `ShouldAllowHorrorEvent()` — Checks safe zone, frequency, beat
- `ShouldAllowSetPiece()` — Checks safe zone, count, beat
- `SuppressRandomness(duration)` — Temporarily block random events
- `ForceNarrativeTrigger(id)` — Override for scripted moments

#### 8. Final Chase Preparation
When StoryPhase = FinalChase:
- Tension escalates to 90+
- WorldState locked to BrokenReality
- Horror beat forced
- S14 becomes permanent presence
- Beat duration shortened (faster pacing)

#### 9. Debug Tool
- **F12** = Toggle pacing overlay
- Shows: TensionLevel with visual bar
- Shows: Current Beat, Next Expected Beat
- Shows: Safe Zone status
- Shows: Horror Level, Horror Stage
- Shows: Story Phase

### Pacing Curve Explanation

```
┌─────────────────────────────────────────────────────────────┐
│                    PACING CURVE                              │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│  Tension                                                     │
│  100 ┤                                           ████        │
│      │                                         ██    ██      │
│   80 ┤                                       ██        ██    │
│      │                                      █            █   │
│   60 ┤                           ████      █              █  │
│      │                         ██    ██   █                █ │
│   40 ┤              ████      █        █ █                  █│
│      │            ██    ██   █          ██                   │
│   20 ┤  ████    █        █ █                                  │
│      │██    ██ █          █                                   │
│    0 ┤─────────────────────────────────────────────────────  │
│      │  Calm   Suspense  Unease   Fear    Panic              │
│      │                                                       │
│      │  Exploration → Discovery → Horror → Silence →         │
│      │  Exploration → SetPiece → Revelation → Silence →      │
│      │  Discovery → Horror → Horror → SILENCE (forced) →     │
│      │  Exploration → Discovery → SetPiece → ...             │
│                                                              │
│  SAFE ZONE: Tension decays rapidly, events suppressed        │
│  FINAL CHASE: Tension locked high, beats accelerate          │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

### System Integration Diagram

```
┌───────────────────────────────────────────────────────────────┐
│                 EXPERIENCE DIRECTOR INTEGRATION                 │
├───────────────────────────────────────────────────────────────┤
│                                                                │
│  INPUTS (What feeds the director):                             │
│  ├─ HorrorManager → horror level, stage, events               │
│  ├─ LevelFlowManager → story phase                             │
│  ├─ PuzzleManager → completion rate                            │
│  ├─ SetPieceManager → setpiece activity                        │
│  ├─ Semester14Observer → observation frequency                 │
│  └─ SafeZone → player location                                 │
│                                                                │
│  PROCESSING (What the director decides):                       │
│  ├─ TensionLevel (0-100) → TensionState                       │
│  ├─ NarrativeBeat rotation (queue-based)                       │
│  ├─ Event gating (ShouldAllow*)                                │
│  └─ Pacing adjustment (beat duration, frequency)              │
│                                                                │
│  OUTPUTS (What the director controls):                         │
│  ├─ HorrorManager → delay/allow events                         │
│  ├─ SetPieceManager → delay/allow setpieces                    │
│  ├─ LevelFlowManager → phase transitions                       │
│  ├─ WorldStateManager → atmosphere changes                     │
│  └─ ExperienceDirector → tension curve                         │
│                                                                │
│  SAFE ZONES:                                                   │
│  ├─ Collider-based trigger                                     │
│  ├─ Tension decay accelerated                                  │
│  ├─ Horror events suppressed                                   │
│  └─ Setpieces blocked                                          │
│                                                                │
│  FINAL CHASE PREPARATION:                                      │
│  ├─ Tension → 90+                                              │
│  ├─ WorldState → BrokenReality (locked)                        │
│  ├─ Horror beat forced                                         │
│  ├─ S14 permanent                                              │
│  └─ Beat duration shortened                                    │
│                                                                │
└───────────────────────────────────────────────────────────────┘
```

### Updated Project Structure
```
Assets/
└── Scripts/
    └── Core/
        └── Pacing/
            ├── TensionLevel.cs
            ├── ExperienceDirector.cs
            ├── SafeZone.cs
            └── PacingDebugTool.cs
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
| Force Spawn Observation | F9 |
| Clear All Observations | F10 |
| Toggle S14 Debug Panel | F11 |
| Toggle Pacing Overlay | F12 |
| Force Spawn Observation | F9 |
| Clear All Observations | F10 |
| Toggle S14 Debug Panel | F11 |

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
