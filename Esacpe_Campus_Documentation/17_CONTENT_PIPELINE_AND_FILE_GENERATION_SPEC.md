# 17_CONTENT_PIPELINE_AND_FILE_GENERATION_SPEC

# PROJECT INFORMATION

Project:
Escape Campus

Version:
1.0

Document Type:
Content Pipeline & File Generation Specification

Priority:
MAXIMUM

Purpose:

Dokumen ini mendefinisikan standar pembuatan seluruh file, folder, scene, script, prefab, ScriptableObject, asset, dokumentasi, branch Git, dan workflow produksi.

Tujuan utama:

Menjaga konsistensi proyek dari awal hingga rilis.

Dokumen ini wajib dipatuhi oleh seluruh AI Agent.

Tidak ada file baru yang boleh dibuat tanpa mengikuti spesifikasi ini.

---

# PRIMARY OBJECTIVE

Consistency over speed.

Maintainability over convenience.

Scalability over shortcuts.

---

########################################################
# PROJECT ROOT STRUCTURE
########################################################

```text
ProjectRoot/

├── Assets/
├── Packages/
├── ProjectSettings/
├── Documentation/
├── Builds/
├── Tools/
└── .github/
```

---

# UNITY ASSETS STRUCTURE

```text
Assets/

├── Art/
├── Audio/
├── Characters/
├── Core/
├── Gameplay/
├── Horror/
├── Levels/
├── Narrative/
├── UI/
├── VFX/
├── Resources/
└── ThirdParty/
```

---

########################################################
# ART STRUCTURE
########################################################

```text
Art/

├── Materials/
├── Models/
├── Textures/
├── Decals/
├── Props/
└── Environment/
```

---

########################################################
# AUDIO STRUCTURE
########################################################

```text
Audio/

├── Music/
├── Ambience/
├── SFX/
├── Dialogue/
├── Horror/
└── UI/
```

---

########################################################
# CHARACTER STRUCTURE
########################################################

```text
Characters/

├── Arga/
├── Raka/
├── Semester14/
├── Nadia/
├── Bima/
├── Sinta/
└── Dimas/
```

---

########################################################
# GAMEPLAY STRUCTURE
########################################################

```text
Gameplay/

├── Interaction/
├── Inventory/
├── Documents/
├── Puzzles/
├── SaveSystem/
├── Objectives/
└── Evidence/
```

---

########################################################
# HORROR STRUCTURE
########################################################

```text
Horror/

├── Events/
├── Jumpscares/
├── Echoes/
├── Distortions/
├── Chase/
└── Semester14/
```

---

########################################################
# LEVEL STRUCTURE
########################################################

```text
Levels/

├── Scenes/
├── Prefabs/
├── Lighting/
└── Navigation/
```

---

########################################################
# SCENE NAMING RULES
########################################################

Format:

SC_[Number]_[Name]

---

Examples

SC_00_Boot

SC_01_MainMenu

SC_02_Intro

SC_03_Campus

SC_04_Library

SC_05_ComputerLab

SC_06_ArchiveRoom

SC_07_ServerRoom

SC_08_FinalChase

SC_09_Ending

---

Forbidden

FinalScene

NewScene

Scene1

TestScene

TempScene

---

########################################################
# SCRIPT NAMING RULES
########################################################

Format:

PascalCase

---

Correct

PlayerController

DocumentManager

PuzzleManager

SaveManager

Semester14Controller

---

Incorrect

playerController

controller

testScript

manager2

---

One public class per file.

Mandatory.

---

########################################################
# SCRIPT DIRECTORY RULES
########################################################

Example

```text
Gameplay/
└── Documents/

    DocumentManager.cs
    DocumentDatabase.cs
    DocumentViewer.cs
    DocumentEntry.cs
```

---

No unrelated scripts in same folder.

---

########################################################
# PREFAB NAMING RULES
########################################################

Format:

PF_[Category]_[Name]

---

Examples

PF_Player

PF_DocumentViewer

PF_Semester14

PF_ArchiveCabinet

PF_EvidenceBoard

---

Forbidden

NewPrefab

FinalPrefab

Object1

Cabinet2

---

########################################################
# MATERIAL NAMING RULES
########################################################

Format:

MAT_[Name]

---

Examples

MAT_ConcreteFloor

MAT_WhiteWall

MAT_OfficeDesk

MAT_RustMetal

---

########################################################
# TEXTURE NAMING RULES
########################################################

Format

T_[Asset]_[Type]

---

Examples

T_Desk_Albedo

T_Desk_Normal

T_Desk_Roughness

T_Wall_Albedo

---

########################################################
# AUDIO NAMING RULES
########################################################

Music

MU_[Name]

Example

MU_Exploration

MU_Chase

MU_Ending

---

SFX

SFX_[Name]

Example

SFX_DoorOpen

SFX_CabinetClose

SFX_PaperFlip

---

Ambience

AMB_[Name]

Example

AMB_Library

AMB_ServerRoom

AMB_Hallway

---

Dialogue

VO_[Character]_[ID]

Example

VO_Raka_001

VO_Nadia_002

---

########################################################
# ANIMATION NAMING RULES
########################################################

AN_[Character]_[Action]

---

Examples

AN_Raka_Idle

AN_Raka_Walk

AN_Raka_Chase

AN_Semester14_Reveal

---

########################################################
# SCRIPTABLE OBJECT RULES
########################################################

Format

SO_[Type]_[Name]

---

Examples

SO_Document_RakaTranscript

SO_Puzzle_ArchiveCabinet

SO_Objective_FindTranscript

---

########################################################
# DOCUMENT FILE RULES
########################################################

Format

DOC_[Category]_[Name]

---

Examples

DOC_Lore_RakaJournal

DOC_Evidence_DefenseSchedule

DOC_Email_Supervisor

---

########################################################
# UNITY SCRIPT TEMPLATE
########################################################

Every new script must contain:

Header

Summary

Responsibility

Dependencies

---

Template

```csharp
using UnityEngine;

/// <summary>
/// Controls player movement.
/// </summary>
public class PlayerController : MonoBehaviour
{
}
```

---

########################################################
# MANAGER RULES
########################################################

Manager allowed only if:

System-wide responsibility exists.

---

Allowed

GameManager

AudioManager

SaveManager

PuzzleManager

ObjectiveManager

---

Forbidden

DoorManager

PaperManager

ChairManager

LightManager

---

Use component-based design.

---

########################################################
# SINGLETON RULES
########################################################

Singleton allowed only for:

GameManager

AudioManager

SaveManager

SettingsManager

---

Everything else:

Avoid singleton.

---

########################################################
# HORROR EVENT FILE RULES
########################################################

Format

HE_[EventName]

---

Examples

HE_FlickeringLights

HE_WhisperLibrary

HE_BookFall

HE_GraduationEcho

---

########################################################
# PUZZLE FILE RULES
########################################################

Format

PZ_[PuzzleName]

---

Examples

PZ_ArchiveLock

PZ_TranscriptPuzzle

PZ_ServerSequence

---

########################################################
# GIT BRANCH RULES
########################################################

Format

feature/[name]

---

Examples

feature/player-controller

feature/document-system

feature/puzzle-framework

---

Bug Fixes

hotfix/[name]

---

Examples

hotfix/save-corruption

hotfix/ending-trigger

---

Documentation

docs/[name]

---

Examples

docs/gdd-update

docs/audio-documentation

---

########################################################
# COMMIT MESSAGE RULES
########################################################

Format

type(scope): description

---

Examples

feat(player): add sprint movement

feat(audio): implement ambience system

feat(puzzle): add archive puzzle

fix(save): resolve corruption issue

docs(gdd): update puzzle design

refactor(audio): simplify mixer routing

---

Forbidden

update

changes

fix

final

new

done

---

########################################################
# README STRUCTURE
########################################################

Every module requires:

Purpose

Dependencies

Usage

Limitations

Future Work

---

########################################################
# TEST FILE RULES
########################################################

Never commit:

TestScene

PrototypeScene

ExperimentScene

---

Before release:

Delete all temporary files.

---

########################################################
# AUTO DOCUMENTATION RULE
########################################################

Whenever new system created:

Update:

Documentation/ProgressLog.md

---

Update:

Relevant design document

---

Update:

Asset registry if applicable

---

########################################################
# GENERATED FILE VALIDATION
########################################################

Before creating any file:

Verify:

Naming compliance

Folder compliance

Dependency compliance

Scope compliance

---

If not compliant:

Do not create file.

---

########################################################
# DEFINITION OF GOOD FILE STRUCTURE
########################################################

A new developer can understand:

Where a file belongs

Why it exists

How it is used

Within 30 seconds.

---

If not:

Structure is wrong.

---

########################################################
# FINAL RULE
########################################################

Consistency is more important than cleverness.

Readable systems survive.

Messy systems collapse.

Always choose consistency.

---

# END OF DOCUMENT