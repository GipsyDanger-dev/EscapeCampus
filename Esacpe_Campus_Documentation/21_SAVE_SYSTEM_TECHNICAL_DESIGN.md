# 21_SAVE_SYSTEM_TECHNICAL_DESIGN

# PROJECT INFORMATION

Project:
Escape Campus

Version:
1.0

Document Type:
Technical Design Document

System:
Save System

Priority:
CRITICAL

Purpose:

Mendefinisikan seluruh arsitektur penyimpanan data (Save System) untuk Escape Campus.

Save System harus mampu menyimpan dan memulihkan seluruh progres investigasi, puzzle, dokumen, evidence, event horror, dan status dunia secara akurat.

Tidak boleh ada progres pemain yang hilang.

Tidak boleh ada save corruption yang menyebabkan soft-lock.

Dokumen ini menjadi sumber kebenaran utama implementasi Save System.

---

# SYSTEM OVERVIEW

Save System bertanggung jawab untuk:

- Save Game
- Load Game
- Autosave
- Progress Persistence
- World State Persistence
- Puzzle Persistence
- Evidence Persistence
- Ending Progress Persistence

---

Save System tidak bertanggung jawab untuk:

- Cloud Save
- Multiplayer Sync
- Analytics
- Network Storage

---

########################################################
# DESIGN GOALS
########################################################

Goal 01

Reliable

---

Goal 02

Fast

---

Goal 03

Corruption Resistant

---

Goal 04

Extensible

---

Goal 05

Simple To Debug

---

########################################################
# SAVE ARCHITECTURE
########################################################

Game Systems

↓

Save Manager

↓

Save Serializer

↓

Save File

↓

Disk Storage

---

Load Flow

Disk Storage

↓

Deserializer

↓

Save Manager

↓

Game Systems

---

########################################################
# SCRIPT STRUCTURE
########################################################

Gameplay/

SaveSystem/

SaveManager.cs

SaveData.cs

SaveSerializer.cs

SaveSlot.cs

SaveMetadata.cs

SaveValidator.cs

AutosaveManager.cs

PersistentObject.cs

ISaveable.cs

---

########################################################
# SAVE FILE FORMAT
########################################################

Format

JSON

Mandatory

---

Reason

Human Readable

Easy Debugging

Easy Versioning

---

Extension

.save

---

Example

slot01.save

slot02.save

slot03.save

---

########################################################
# SAVE SLOT SYSTEM
########################################################

Manual Slots

3

---

Autosave Slot

1

---

Total Slots

4

---

Slot Names

Slot 1

Slot 2

Slot 3

Autosave

---

########################################################
# SAVE FILE LOCATION
########################################################

Windows

Application.persistentDataPath

---

Mandatory

Never save inside project folders.

---

########################################################
# MASTER SAVE DATA
########################################################

SaveData

Contains:

PlayerData

WorldData

DocumentData

EvidenceData

PuzzleData

EventData

ObjectiveData

EndingData

Metadata

---

########################################################
# PLAYER DATA
########################################################

Must Save

Position

Rotation

Current Scene

Player State

Crouch State

Current Objective

---

Must Restore Exactly

---

########################################################
# DOCUMENT DATA
########################################################

Must Save

Collected Documents

Viewed Documents

Unlocked Documents

Secret Documents

---

Required

YES

---

########################################################
# EVIDENCE DATA
########################################################

Must Save

Collected Evidence

Evidence Progress

Investigation State

---

########################################################
# OBJECTIVE DATA
########################################################

Must Save

Current Objective

Completed Objectives

Failed Objectives

Hidden Objectives

---

########################################################
# WORLD STATE DATA
########################################################

Must Save

Opened Doors

Destroyed Objects

Collected Items

Activated Switches

Unlocked Areas

Triggered Events

---

World must restore exactly.

---

########################################################
# PUZZLE DATA
########################################################

Must Save

Solved Puzzles

Puzzle Stages

Puzzle Inputs

Unlocked Puzzle Segments

Puzzle Progress

---

Example

Archive Puzzle

Stage 3 of 5

↓

Must restore Stage 3

---

########################################################
# HORROR EVENT DATA
########################################################

Must Save

Triggered Jumpscares

Triggered Echoes

Triggered Distortions

Triggered Manifestations

Story Horror Events

---

Purpose

Prevent repeated events.

---

########################################################
# ENDING DATA
########################################################

Must Save

Ending Requirements

Secret Progress

Hidden Conditions

True Ending Progress

---

########################################################
# METADATA
########################################################

Save Time

---

Play Time

---

Slot Name

---

Game Version

---

Last Scene

---

Screenshot Path (Optional)

---

########################################################
# ISAVEABLE INTERFACE
########################################################

Every persistent system implements:

ISaveable

---

Methods

CaptureState()

RestoreState()

GetUniqueID()

---

Example

```csharp
public interface ISaveable
{
    object CaptureState();

    void RestoreState(object state);

    string GetUniqueID();
}
```

---

########################################################
# PERSISTENT OBJECT RULES
########################################################

Every persistent object requires:

PersistentObject Component

---

Must contain:

UniqueID

---

UniqueID never changes.

Mandatory.

---

########################################################
# UNIQUE ID FORMAT
########################################################

Format

SYSTEM_NAME_GUID

---

Example

DOOR_4E12A

DOCUMENT_82DF1

PUZZLE_11A77

---

Generated once.

Never regenerated.

---

########################################################
# AUTOSAVE SYSTEM
########################################################

Enabled

YES

---

Autosave Trigger

Scene Change

---

Major Puzzle Solved

---

Critical Document Found

---

Ending Progress Updated

---

Manual Save Not Required

---

########################################################
# AUTOSAVE RESTRICTIONS
########################################################

Cannot autosave during:

Cutscene

---

Jumpscare

---

Loading

---

Final Chase

---

########################################################
# SAVE VALIDATION
########################################################

Before save:

Validate Data

---

Validate IDs

---

Validate References

---

Validate Version

---

If invalid

Abort Save

---

Log Error

---

########################################################
# LOAD VALIDATION
########################################################

Before load:

Verify file exists

---

Verify JSON valid

---

Verify version supported

---

Verify critical fields

---

If failed

Load blocked

---

########################################################
# SAVE VERSIONING
########################################################

Mandatory

---

Every save contains:

SaveVersion

---

Example

1.0.0

---

Future compatible migration supported.

---

########################################################
# SAVE MIGRATION
########################################################

If older save detected:

Run Migration

---

Convert Missing Fields

---

Preserve Progress

---

########################################################
# CORRUPTION PROTECTION
########################################################

Method

Backup Save

---

Before overwrite:

Create backup

---

Example

slot01.save

↓

slot01_backup.save

---

########################################################
# RECOVERY SYSTEM
########################################################

If save corrupted:

Attempt backup restore

---

If backup valid:

Recover automatically

---

Notify player

---

########################################################
# PERFORMANCE TARGETS
########################################################

Manual Save

< 1 Second

---

Load

< 2 Seconds

---

Autosave

< 0.5 Seconds

---

########################################################
# MEMORY REQUIREMENTS
########################################################

Maximum Save Size

10 MB

---

Target

1–3 MB

---

########################################################
# SECURITY RULES
########################################################

No encryption required.

---

Reason

Single Player Game

---

Readable JSON preferred.

---

########################################################
# SAVE FREQUENCY RULES
########################################################

Manual Save

Unlimited

---

Autosave

Event Based

---

Never autosave every frame.

---

Never autosave every minute.

---

########################################################
# UI INTEGRATION
########################################################

Save Menu

Required

---

Load Menu

Required

---

Delete Save

Required

---

Autosave Indicator

Required

---

########################################################
# SAVE MENU DATA
########################################################

Display

Slot Name

---

Last Save Time

---

Play Time

---

Current Chapter

---

Current Objective

---

########################################################
# CHAPTER TRACKING
########################################################

Required

YES

---

Chapters

Prologue

Act 1

Act 2

Act 3

Final Chase

Ending

---

Stored in Save Metadata.

---

########################################################
# FAILURE HANDLING
########################################################

If Save Fails

Show Warning

---

Do Not Crash

---

If Load Fails

Return To Main Menu

---

Log Error

---

########################################################
# DEBUG TOOLS
########################################################

Development Only

---

Force Save

---

Force Load

---

Dump Save JSON

---

Inspect World State

---

Repair Save

---

Disabled In Release

---

########################################################
# TEST CASES
########################################################

TC-01

Manual Save Works

---

TC-02

Manual Load Works

---

TC-03

Autosave Works

---

TC-04

Puzzle Progress Restores

---

TC-05

Document Progress Restores

---

TC-06

Evidence Restores

---

TC-07

World State Restores

---

TC-08

Corrupted Save Recovery Works

---

TC-09

Version Validation Works

---

TC-10

No Progress Loss

---

########################################################
# DEFINITION OF DONE
########################################################

Player can:

Save

Load

Autosave

Restore Progress

Restore Puzzles

Restore Documents

Restore Evidence

Restore World State

Restore Ending Progress

---

All Tests Pass

---

No Critical Bugs

---

No Save Corruption

---

Documentation Updated

---

Committed

---

Pushed

---

# END OF DOCUMENT