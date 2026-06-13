# Escape Campus — Final Build Checklist

## Pre-Build Steps

### 1. Unity Setup
- [ ] Open project in Unity 6 (6000.0.x)
- [ ] Wait for import to complete
- [ ] Check Console for errors (should be 0)

### 2. Generate Content
- [ ] Menu: `EscapeCampus > Setup Project`
- [ ] Menu: `EscapeCampus > Generate Prototype Content`
- [ ] Verify ScriptableObjects created in `Assets/ScriptableObjects/Documents/` and `Assets/ScriptableObjects/Evidence/`

### 3. Scene Setup
- [ ] Open `LobbyPrototype` scene
- [ ] Verify LevelLayoutBuilder creates 6 areas
- [ ] Verify LevelConfigurator exists
- [ ] Verify BuildConfigurator exists with `isReleaseBuild = true`

### 4. Play Test
- [ ] Press Play
- [ ] Verify player spawns at (0, 1, 2)
- [ ] Verify tutorial UI appears
- [ ] Collect DOC_002 and EV_003 in lobby
- [ ] Move to Main Library Hall
- [ ] Collect DOC_003, DOC_004, EV_001
- [ ] Solve PZ_LIBRARY_TIMELINE
- [ ] Verify phase transition to FirstAnomaly
- [ ] Enter Archive Corridor
- [ ] Verify setpiece triggers
- [ ] Enter Archive Room
- [ ] Collect DOC_001, EV_002
- [ ] Solve PZ_ARCHIVE_CORRELATION
- [ ] Verify phase transition to RealityBreakdown
- [ ] Enter Maintenance Hall (safe zone)
- [ ] Verify horror disabled
- [ ] Enter Graduation Hall
- [ ] Verify final sequence triggers
- [ ] Verify ending UI appears
- [ ] Make choice and verify ending plays

### 5. Save/Load Test
- [ ] Press F5 to save
- [ ] Press ESC to quit
- [ ] Press Play again
- [ ] Press F9 to load
- [ ] Verify state restored correctly

### 6. Build
- [ ] File > Build Settings
- [ ] Add scenes: MainMenu (0), LobbyPrototype (1)
- [ ] Select platform: PC, Mac & Linux Standalone
- [ ] Architecture: x86_64
- [ ] Click Build
- [ ] Test built executable

---

## Placement Verification

### Documents
| ID | Location | Position | Status |
|----|----------|----------|--------|
| DOC_002 | Lobby Entrance | (-3, 1, 1) | [ ] |
| DOC_003 | Main Library | (0, 1, -12) | [ ] |
| DOC_004 | Main Library | (8, 1, -8) | [ ] |
| DOC_001 | Archive Room | (0, 1, -56) | [ ] |

### Evidence
| ID | Location | Position | Status |
|----|----------|----------|--------|
| EV_003 | Lobby Entrance | (3, 1, 1) | [ ] |
| EV_001 | Main Library | (-7, 1, -14) | [ ] |
| EV_002 | Archive Room | (-6, 1, -50) | [ ] |

### Puzzles
| ID | Location | Requirements | Status |
|----|----------|--------------|--------|
| PZ_LIBRARY_TIMELINE | Main Library | DOC_003 + DOC_004 | [ ] |
| PZ_ARCHIVE_CORRELATION | Archive Room | DOC_001 + EV_002 | [ ] |

### SetPieces
| ID | Location | Trigger | Status |
|----|----------|---------|--------|
| SP_LIBRARY_WHISPER_CORRIDOR | Archive Corridor | Volume at (0, 1, -30) | [ ] |

### Safe Zones
| Location | Trigger Area | Status |
|----------|-------------|--------|
| Maintenance Hall | (0, 1, -72) size (8, 3, 6) | [ ] |

---

## Debug Verification

### Tools Disabled in Release
- [ ] PuzzleDebugTool (F2, F3)
- [ ] HorrorDebugTool (F4, F5, F6)
- [ ] Semester14DebugTool (F9, F10, F11)
- [ ] LevelFlowDebugTool (F7, F8)
- [ ] PacingDebugTool (F12)
- [ ] SaveDebugTool (F1)

### Tools Kept
- [ ] Save (F5)
- [ ] Load (F9)
- [ ] Normal gameplay input

---

## Known Issues & Risks

### HIGH RISK
1. **Missing ScriptableObject References**
   - LevelConfigurator may not have DocumentData/EvidenceData assigned
   - FIX: Run "Generate Prototype Content" before play

2. **SetPiece Not Triggering**
   - Trigger volume may be wrong size/position
   - FIX: Verify collider at (0, 1, -30) with size (3, 3, 3)

3. **Phase Deadlock**
   - If puzzle never solved, game softlocks
   - FIX: Timeout fallbacks added (120s, 600s, 900s, 600s)

### MEDIUM RISK
4. **Horror Events Not Enabling**
   - Events disabled at start, enabled by phase transitions
   - FIX: LevelConfigurator enables events per phase

5. **Save State Corruption**
   - Partial save could corrupt game state
   - FIX: Atomic save (all or nothing)

6. **S14 Not Spawning**
   - Requires HorrorLevel >= 40 and StoryPhase >= FirstAnomaly
   - FIX: Force spawn in Phase 7

### LOW RISK
7. **Performance in Graduation Hall**
   - Many lights + fog + S14 = potential FPS drop
   - FIX: Reduce light count if needed

8. **Cursor Not Locking**
   - UI may leave cursor visible
   - FIX: BuildConfigurator resets cursor

---

## Game Flow Timeline

```
0:00 - 5:00   PHASE 1: INTRO (Lobby)
5:00 - 15:00  PHASE 2: EARLY INVESTIGATION (Library)
15:00 - 20:00 PHASE 3: FIRST SETPIECE (Corridor)
20:00 - 35:00 PHASE 4: DEEP INVESTIGATION (Archive)
35:00 - 45:00 PHASE 5: ESCALATION (Global)
45:00 - 50:00 PHASE 6: SAFE ZONE (Maintenance)
50:00 - 60:00 PHASE 7: FINAL SEQUENCE (Graduation)
```

---

## Success Criteria

- [ ] Full loop playable end-to-end
- [ ] At least 1 setpiece executes correctly
- [ ] S14 appears multiple times (all forms)
- [ ] All 4 endings reachable
- [ ] Story understandable via documents + evidence
- [ ] Save/Load fully functional
- [ ] No console errors
- [ ] No softlock possible
- [ ] 30-60 minute playtime
- [ ] itch.io build ready
