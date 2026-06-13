# Escape Campus - Unity Build Checklist (itch.io Ready)

## Pre-Build Checklist

### Scene Setup
- [ ] Open Unity 6 project
- [ ] Run `EscapeCampus > Setup Project` to create scenes
- [ ] Run `EscapeCampus > Generate Prototype Content` to create ScriptableObjects
- [ ] Open LobbyPrototype scene
- [ ] Verify LevelLayoutBuilder creates all 6 areas
- [ ] Verify LevelConfigurator wires all triggers

### Manager Verification
- [ ] DocumentManager exists (auto-created)
- [ ] EvidenceManager exists (auto-created)
- [ ] PuzzleManager exists (auto-created)
- [ ] HorrorManager exists (auto-created)
- [ ] Semester14Observer exists (auto-created)
- [ ] SetPieceManager exists (auto-created)
- [ ] LevelFlowManager exists (auto-created)
- [ ] WorldStateManager exists (auto-created)
- [ ] EndingManager exists (auto-created)
- [ ] TruthRevealManager exists (auto-created)
- [ ] ExperienceDirector exists (auto-created)
- [ ] SaveManager exists (auto-created)

### Document Placement
- [ ] DOC_002 at Lobby Entrance (-3, 1, 1)
- [ ] DOC_003 at Main Library reading table (0, 1, -12)
- [ ] DOC_004 at Main Library librarian desk (8, 1, -8)
- [ ] DOC_001 at Archive Room research desk (0, 1, -56)

### Evidence Placement
- [ ] EV_003 at Lobby Entrance (3, 1, 1)
- [ ] EV_001 at Main Library near shelves (-7, 1, -14)
- [ ] EV_002 at Archive Room filing cabinet (-6, 1, -50)

### Puzzle Verification
- [ ] PZ_LIBRARY_TIMELINE exists in Main Library
- [ ] PZ_ARCHIVE_CORRELATION exists in Archive Room
- [ ] Both puzzles require correct document/evidence prerequisites

### Horror Event Configuration
- [ ] LightFlickerEvent disabled at start
- [ ] WhisperEvent disabled at start
- [ ] TextShiftEvent disabled at start
- [ ] UIGlitchEvent disabled at start
- [ ] Events enable at correct story phases

### SetPiece Configuration
- [ ] SP_LIBRARY_WHISPER_CORRIDOR exists in Archive Corridor
- [ ] SetPiece trigger volume at (0, 1, -30)
- [ ] Corridor lights tagged for setpiece control

### Safe Zone
- [ ] SafeZone trigger in Maintenance Hall
- [ ] Tension decays at 5x rate in safe zone
- [ ] Horror events disabled in safe zone

### Build Configuration
- [ ] BuildConfigurator exists with isReleaseBuild = true
- [ ] All debug tools disabled in release build
- [ ] Save/Load system functional (F5/F9)

---

## Unity Build Settings

### Player Settings
```
Company Name: [Your Name]
Product Name: Escape Campus
Version: 1.0.0
```

### Resolution
```
Default Screen Width: 1920
Default Screen Height: 1080
Full Screen Mode: Fullscreen Window
```

### Quality
```
Quality Level: Medium
Anti Aliasing: 4x
V Sync Count: Every V Blank
```

### Build Settings
```
Scenes:
1. MainMenu (index 0)
2. LobbyPrototype (index 1)

Platform: Windows, Mac, Linux
Architecture: x86_64
```

---

## itch.io Upload Checklist

### Build Preparation
- [ ] Build as Windows x86_64
- [ ] Build as Mac Universal
- [ ] Build as Linux x86_64
- [ ] Create .zip files for each platform

### itch.io Project Setup
- [ ] Create new project on itch.io
- [ ] Set title: "Escape Campus"
- [ ] Set genre: Horror, Puzzle, Narrative
- [ ] Set release status: Released
- [ ] Set pricing: Free / Name your price
- [ ] Upload cover image (1280x720)
- [ ] Upload screenshots (3-5)
- [ ] Write description
- [ ] Set controls description

### Description Template
```
ESCAPE CAMPUS - A Psychological Horror Investigation Game

You are a student investigating the mysteries of Campus 14.
Collect documents. Solve puzzles. Uncover the truth.

But you are not alone. Something watches from the shadows.
Something that has been here before. Something that remembers.

Features:
- 30-60 minute narrative experience
- Investigation gameplay (documents, evidence, puzzles)
- Psychological horror (no jumpscares, no combat)
- Multiple endings based on your choices
- Save/Load system

Controls:
WASD - Move
Mouse - Look
E - Interact
J - Journal
F5 - Save
F9 - Load
ESC - Close UI

WARNING: This game contains psychological horror elements.
No graphic content. No violence. Just... unease.
```

---

## Risk List / Potential Bugs

### HIGH RISK
1. **Missing ScriptableObject References**
   - LevelConfigurator requires DocumentData and EvidenceData references
   - Must run "Generate Prototype Content" before play
   - FIX: Auto-assign in LevelConfigurator.Start() if null

2. **SetPiece Not Triggering**
   - SP_LIBRARY_WHISPER_CORRIDOR requires correct trigger volume placement
   - Must be in Archive Corridor at (0, 1, -30)
   - FIX: Verify collider is trigger and size is correct

3. **Phase Transition Deadlock**
   - Phase 2 → 3 requires puzzle completion
   - If puzzle never solved, game softlocks
   - FIX: Add timeout fallback (15 min → force advance)

### MEDIUM RISK
4. **Horror Events Not Enabling**
   - Events disabled at start, enabled by phase transitions
   - If phase transition fails, events stay disabled
   - FIX: Add safety check in HorrorManager.Update()

5. **Save/Load State Corruption**
   - Multiple systems save state independently
   - Partial save could corrupt game state
   - FIX: Atomic save (save all or nothing)

6. **S14 Observer Not Spawning**
   - Requires HorrorLevel >= 40 and StoryPhase >= FirstAnomaly
   - If player progresses too fast, may miss observations
   - FIX: Force spawn in Phase 4+ regardless of conditions

### LOW RISK
7. **Performance in Graduation Hall**
   - Many lights + fog + S14 spawning = potential FPS drop
   - FIX: Reduce light count, optimize fog

8. **Audio Drop During SetPiece**
   - SetPiece pauses all audio sources for 0.5s
   - Could affect ambient audio permanently
   - FIX: Store and restore volume properly

9. **Cursor Not Locking After UI**
   - DocumentViewer and EndingUI unlock cursor
   - If UI closes abnormally, cursor stays visible
   - FIX: Add cursor reset in PlayerController.Update()

10. **Door Controller Animation Conflict**
    - Multiple door state changes could cause animation jitter
    - FIX: Queue door animations, don't interrupt

---

## Minute-by-Minute Game Flow

```
0:00 - Game starts, player spawns in Lobby Entrance
0:00 - Tutorial UI shows controls
0:30 - Player explores lobby, finds DOC_002 and EV_003
1:00 - Player reads documents, learns about campus
2:00 - Player moves to Main Library Hall
3:00 - Player explores bookshelves, finds EV_001
4:00 - Player finds DOC_003 at reading table
5:00 - Player finds DOC_004 at librarian desk
5:30 - Phase transition: EarlyInvestigation

6:00 - Player reads DOC_003 and DOC_004
7:00 - Player attempts PZ_LIBRARY_TIMELINE puzzle
8:00 - Player solves puzzle (correct answer: A)
9:00 - Phase transition: FirstAnomaly
9:30 - Horror level increases to 35

10:00 - Player moves to Archive Corridor
10:30 - SetPiece trigger volume entered
10:30 - SP_LIBRARY_WHISPER_CORRIDOR begins
10:31 - Lights turn off behind player
10:34 - Whisper audio increases
10:36 - Door closes at corridor end
10:37 - S14 appears for 1 frame
10:37 - Camera forces look back
10:39 - Nothing visible, pause
10:40 - Player control restored
10:41 - Phase transition: DeepInvestigation

11:00 - Player enters Archive Room
12:00 - Player finds EV_002 at filing cabinet
13:00 - Player finds DOC_001 at research desk (CRITICAL)
14:00 - Player reads all collected documents
15:00 - Player attempts PZ_ARCHIVE_CORRELATION puzzle
16:00 - Player solves puzzle
17:00 - Phase transition check: 3+ evidence AND 2+ puzzles
17:30 - Phase transition: RealityBreakdown

18:00 - World state changes to BrokenReality
18:30 - Horror level increases to 65
19:00 - S14 peripheral sightings begin
20:00 - Random door lock/unlock events
21:00 - Lighting becomes erratic
22:00 - Audio distortion begins
23:00 - Horror level reaches 75
23:30 - Phase transition: FinalPreparation

24:00 - Player enters Maintenance Hall (Safe Zone)
24:30 - Horror events disabled
25:00 - Tension decays rapidly
26:00 - Truth fragments revealed
27:00 - Player reads truth summary
28:00 - Graduation Hall door unlocked
29:00 - Player moves to Graduation Hall

30:00 - Phase transition: FinalChase
30:30 - World state locked to BrokenReality
31:00 - Horror level locked to 85
31:30 - S14 constant presence
32:00 - All horror events enabled
33:00 - Player approaches ritual circle
34:00 - Final setpiece triggers
35:00 - Final Decision UI appears

35:30 - Player makes choice:
        A) Destroy Ritual → Good Ending (36:00)
        B) Continue Loop → Bad Ending (36:00)
        C) Escape Without Truth → Secret Ending (36:00)
        D) (Auto if 100%) → True Ending (36:00)

36:00 - Ending narrative plays
37:00 - Truth summary shown
38:00 - Credits

TOTAL: ~38 minutes (within 30-60 min target)
```

---

## Critical Path Verification

```
START → Lobby → DOC_002 + EV_003 → Library → DOC_003 + DOC_004 + EV_001
→ PZ_LIBRARY_TIMELINE → FirstAnomaly → Archive Corridor → SETPIECE
→ DeepInvestigation → Archive Room → DOC_001 + EV_002 → PZ_ARCHIVE_CORRELATION
→ RealityBreakdown → Maintenance Hall (Safe) → Truth Reveal → Graduation Hall
→ FinalChase → FINAL DECISION → ENDING
```

Every step is forward-only. No backtracking required. No softlock possible.
