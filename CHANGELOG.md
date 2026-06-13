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

## [0.3.0] - 2026-06-13

### Added
- SaveData model with JSON serialization (player position, documents, evidence, play time, timestamp)
- SaveManager singleton with Save/Load/Delete/HasSave API
- Save slots: 1 manual slot (F5) + 1 autosave slot (event-based)
- Player position and rotation save/restore with CharacterController handling
- Document collection persistence (collected IDs saved/restored)
- Evidence collection persistence (collected IDs saved/restored)
- Event-based autosave triggers (document collected, evidence collected)
- SaveUI with F5 save / F9 load hotkeys and on-screen confirmation messages
- SaveDebugTool overlay (F1 toggle) showing save file info, document/evidence counts, player position
- Save files stored in Application.persistentDataPath/Saves/

### Changed
- SceneBootstrapper now creates SaveManager, SaveUI, and SaveDebugTool
- DocumentManager and EvidenceManager integrated with SaveManager for autosave

### Project Structure
- Assets/Scripts/Save/ - SaveData, SaveManager
- Assets/Scripts/UI/ - SaveUI, SaveDebugTool

---

## [0.4.0] - 2026-06-13

### Added
- PuzzleState enum (Locked, Unlocked, InProgress, Solved)
- PuzzleManager singleton with Register/GetState/SetState/IsCompleted API
- PuzzleBase abstract class with document/evidence requirement validation
- PuzzleTrigger interactable (press E to start puzzle, state-aware prompts)
- LibraryTimelinePuzzle - first dummy puzzle (read 2 docs, determine event order)
- PuzzleDebugTool (F2 unlock all, F3 solve current, console state logging)
- Puzzle state persistence in SaveData (JSON serialized)
- Puzzle autosave on completion
- PuzzleManager events: OnPuzzleStateChanged, OnPuzzleCompleted

### Changed
- SaveData now includes puzzleStates list (PuzzleSaveEntry with puzzleID and state)
- SaveManager now saves/restores puzzle state
- SaveManager subscribes to PuzzleManager.OnPuzzleCompleted for autosave
- SceneBootstrapper now creates PuzzleManager and PuzzleDebugTool

### Project Structure
- Assets/Scripts/Puzzle/ - PuzzleState, PuzzleManager, PuzzleBase, PuzzleTrigger, LibraryTimelinePuzzle, PuzzleDebugTool

---

## [0.5.0] - 2026-06-13

### Added
- HorrorManager singleton with horror level (0-100) and stage system
- HorrorStage enum (Calm, Unease, Disturbance, Paranoia, Collapse)
- HorrorEvent base class with 4 event types (Audio, Visual, Environmental, UI)
- LightFlickerEvent - environmental light flicker effect
- WhisperAudioEvent - 3D spatial audio whispers
- DoorCloseEvent - mysterious door closing without player interaction
- DocumentTextShiftEvent - temporary text corruption in document viewer
- UIGlitchEvent - screen-wide UI glitch overlay
- HorrorEventTrigger with weighted random triggering and cooldown (min 30s)
- Trigger conditions: puzzle solved, document collected, evidence collected, stage change
- Horror level escalation per game action (puzzle +10, evidence +5, document +3)
- HorrorSaveData for persistence (horror level, triggered events)
- HorrorDebugTool (F4 increase, F5 decrease, F6 reset)
- SEMESTER14_HOOKS.md documenting entity integration points

### Changed
- SaveData now includes HorrorSaveEntry (horrorLevel, triggeredEventIDs)
- SaveManager now saves/restores horror state
- SceneBootstrapper now creates HorrorManager, HorrorEventTrigger, HorrorDebugTool

### Project Structure
- Assets/Scripts/Horror/ - HorrorLevel, HorrorManager, HorrorEvent, HorrorEventTrigger, HorrorDebugTool, Events/

---

## [0.6.0] - 2026-06-13

### Added
- StoryPhase enum (Introduction, EarlyInvestigation, FirstAnomaly, DeepInvestigation, RealityBreakdown, FinalPreparation, FinalChase, Ending)
- LevelFlowManager singleton with phase management and transition rules
- PhaseTransitionRule system with conditional advancement (puzzle count, evidence count, horror level)
- WorldStateManager with 4 world states (Normal, Suspicious, Corrupted, BrokenReality)
- WorldState environment effects (ambient light, fog color, fog density)
- DoorController (IInteractable) with lock/unlock and WorldStateManager integration
- LightingController with smooth state-based color/intensity transitions
- AudioAmbienceController with crossfade between state-based ambience clips
- WorldObject component for runtime object repositioning
- NarrativeTrigger system with multi-condition evaluation (AND/OR logic)
- Trigger conditions: PuzzleCompleted, DocumentCollected, EvidenceCollected, HorrorLevelReached, StoryPhaseEntered, TimePlayed
- LevelFlowDebugTool (F7 next phase, F8 previous phase, on-screen phase display)

### Changed
- SceneBootstrapper now creates LevelFlowManager, WorldStateManager, LevelFlowDebugTool
- WorldStateManager automatically applies ambient/fog settings based on state

### Project Structure
- Assets/Scripts/Core/ - StoryPhase, LevelFlowManager, WorldStateManager, NarrativeTrigger, DoorController, LightingController, AudioAmbienceController, LevelFlowDebugTool

---

## [0.7.0] - 2026-06-13

### Added
- Semester14Observer singleton with passive entity observation system
- 4 observation types: Static, Peripheral, Mirror, MissingFrame
- ObservationType enum with descriptions
- Spawn rules: HorrorLevel >= 40, StoryPhase >= FirstAnomaly, cooldown 60-120s
- Weighted spawn chance that increases with horror stage
- No interaction rule: entity has no collider, cannot be triggered manually
- Environment response on observation: light desync, audio drop, UI flicker, door change
- PeripheralVisibility component: only visible when not directly looked at
- MissingFrameBlink component: brief flashes at intervals
- Observation tracking: total observations, last time, type frequency
- ObservationSaveEntry for save persistence
- Semester14DebugTool (F9 force spawn, F10 clear, F11 toggle panel)
- ObservationPoint system for predefined spawn locations

### Changed
- SaveData now includes ObservationSaveEntry
- SaveManager now saves/restores observation data
- SceneBootstrapper now creates Semester14Observer and Semester14DebugTool

### Project Structure
- Assets/Scripts/Horror/Semester14/ - ObservationType, Semester14Observer, PeripheralVisibility, MissingFrameBlink, Semester14DebugTool

---

## [0.8.0] - 2026-06-13

### Added
- SetPieceManager singleton with Register/Trigger/Complete/GetActive API
- SetPieceState enum (Idle, Triggering, Active, Ending, Completed)
- SetPieceType enum (ForcedCamera, EnvironmentalCollapse, CorridorEvent, ObservationFreeze, EscapeSequence)
- SetPieceBase abstract class with trigger conditions, fail-safe recovery, timeout
- ScriptedCameraController with ForceLookAt, SlowDrag, MicroHeadShake, SetFOV, SmoothRotation
- SetPieceTriggerVolume for automatic trigger when player enters area
- LibraryWhisperCorridorSetPiece (SP_LIBRARY_WHISPER_CORRIDOR) - first mandatory setpiece
  - Lights turn off behind player
  - Whisper audio increases
  - Door closes by itself
  - Semester 14 appears for 1 frame behind glass
  - Camera forces look back
  - Nothing is there
- SetPieceSaveEntry for save persistence
- Fail-safe system: graceful skip, restore player control, continue game

### Changed
- SaveData now includes List<SetPieceSaveEntry>
- SaveManager now saves/restores setpiece states
- SceneBootstrapper now creates SetPieceManager

### Project Structure
- Assets/Scripts/Horror/SetPieces/ - SetPieceState, SetPieceManager, SetPieceBase, ScriptedCameraController, SetPieceTriggerVolume, LibraryWhisperCorridorSetPiece

---

## [0.9.0] - 2026-06-13

### Added
- EndingManager singleton with EvaluateEndingCondition/TriggerEnding/LoadEndingSequence
- EndingType enum (Good, Bad, Secret, True)
- EndingPhase enum (NotStarted through Credits)
- EndingConditions class with configurable requirements per ending
- EndingEvaluationData gathering from all game systems
- TruthRevealManager with 6 truth fragments and narrative summary
- TruthFragment system with condition-based revelation
- FinalDecision enum (DestroyRitual, ContinueLoop, EscapeWithoutTruth)
- EndingUI with final decision buttons and ending narrative display
- 4 ending narratives with full story text:
  - Good: "The End of the Loop" — destroy ritual, Raka is free
  - Bad: "The Cycle Continues" — escape but loop continues
  - Secret: "The Third Path" — join the system
  - True: "The Truth Revealed" — Semester 14 is Raka
- Ending sequence flow: WorldBreakdown → ConstantPresence → GraduationHall → FinalDecision → EndingSequence → Credits
- EndingSaveEntry and TruthRevealSaveData for persistence

### Changed
- SaveData now includes EndingSaveEntry and revealedTruthFragments
- SaveManager now saves/restores ending state and truth fragments
- SceneBootstrapper now creates EndingManager and TruthRevealManager

### Project Structure
- Assets/Scripts/Core/Ending/ - EndingType, EndingConditions, EndingManager, TruthRevealManager
- Assets/Scripts/UI/EndingUI.cs

---

## [1.0.0] - 2026-06-13

### Added
- ExperienceDirector singleton with EvaluateGameState/AdjustPacing/TriggerNarrativeBeat/ControlTensionCurve
- TensionLevel system (0-100) with 5 states: Calm, Suspense, Unease, Fear, Panic
- NarrativeBeat enum with 6 beat types: Exploration, Discovery, Horror, SetPiece, Revelation, Silence
- Automatic pacing rules based on horror level, time, setpiece frequency, story phase
- SafeZone component (collider trigger) with tension decay and event suppression
- Beat rotation system with queue-based dynamic sequencing
- Dynamic event control: ShouldAllowHorrorEvent, ShouldAllowSetPiece, SuppressRandomness
- Final Chase preparation: escalate tension, lock world state, S14 permanent presence
- PacingDebugTool (F12 toggle) with tension bar, current beat, next expected beat
- PacingSaveEntry for persistence

### Changed
- SaveData now includes PacingSaveEntry (tensionLevel, currentBeat, totalPlayTime)
- SaveManager now saves/restores pacing state
- SceneBootstrapper now creates ExperienceDirector and PacingDebugTool

### Project Structure
- Assets/Scripts/Core/Pacing/ - TensionLevel, ExperienceDirector, SafeZone, PacingDebugTool

---

## [0.0.0] - 2026-06-11

### Added
- Initial repository setup
- Documentation folder
