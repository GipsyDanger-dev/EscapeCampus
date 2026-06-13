# 10_TECHNICAL_ARCHITECTURE_DOCUMENT

# PROJECT INFORMATION

Title:
Escape Campus

Version:
1.0

Document Type:
Technical Architecture Document

Purpose:
Dokumen ini mendefinisikan struktur teknis proyek, arsitektur Unity, struktur folder, coding standards, Git workflow, save system, event system, puzzle system, audio system, dan aturan implementasi AI Agent.

Dokumen ini menjadi sumber kebenaran utama seluruh implementasi teknis.

Digunakan oleh:

- Gameplay Programmer
- Technical Designer
- Technical Artist
- Build Engineer
- AI Agent

---

# ENGINE

Engine:
Unity 6 LTS

---

Rendering Pipeline:
Universal Render Pipeline (URP)

---

Target Platform:

Primary:
Windows PC

Secondary:
itch.io Web Build (Optional)

---

Programming Language:

C#

---

# PROJECT STRUCTURE

Assets/

├── Art/
├── Audio/
├── Materials/
├── Models/
├── Textures/
│
├── Animations/
├── Prefabs/
├── Scenes/
├── Scripts/
├── UI/
├── VFX/
│
├── Resources/
├── ScriptableObjects/
├── StreamingAssets/
│
└── Documentation/

---

# SCRIPT STRUCTURE

Scripts/

├── Core/
├── Managers/
├── Player/
├── Interactions/
├── Inventory/
├── SaveSystem/
├── PuzzleSystem/
├── HorrorSystem/
├── AudioSystem/
├── UISystem/
├── CutsceneSystem/
├── AI/
├── Utilities/

---

# SCENE STRUCTURE

01_Boot

Purpose:
Initialization

---

02_MainMenu

Purpose:
Main Menu

---

03_Game

Purpose:
Entire Gameplay

Single Persistent Scene

---

04_Ending

Purpose:
Ending Sequences

---

05_SecretEnding

Purpose:
Secret Ending

---

# SINGLE SCENE PHILOSOPHY

Game should use:

One Main Gameplay Scene

---

Reason:

- Simpler save system
- Simpler horror triggers
- Easier management
- Better for solo developer

---

# CORE MANAGERS

########################################################
# GAME MANAGER
########################################################

Responsibilities:

- Global State
- Progress Tracking
- Ending Control

Singleton:
Yes

Persistent:
Yes

---

########################################################
# SAVE MANAGER
########################################################

Responsibilities:

- Save Data
- Load Data
- Checkpoints

Singleton:
Yes

Persistent:
Yes

---

########################################################
# HORROR MANAGER
########################################################

Responsibilities:

- Horror Events
- Jumpscares
- Event Cooldowns

Singleton:
Yes

Persistent:
Yes

---

########################################################
# PUZZLE MANAGER
########################################################

Responsibilities:

- Puzzle Progress
- Puzzle Completion
- Puzzle Validation

Singleton:
Yes

Persistent:
Yes

---

########################################################
# AUDIO MANAGER
########################################################

Responsibilities:

- Music
- Ambience
- SFX

Singleton:
Yes

Persistent:
Yes

---

########################################################
# CUTSCENE MANAGER
########################################################

Responsibilities:

- Timeline Control
- Dialogue Triggers

Singleton:
Yes

Persistent:
Yes

---

########################################################
# UI MANAGER
########################################################

Responsibilities:

- Menus
- HUD
- Document Viewer

Singleton:
Yes

Persistent:
Yes

---

# PLAYER ARCHITECTURE

Player

Components:

- Character Controller
- Player Camera
- Interaction System
- Inventory System
- Audio Listener

---

# PLAYER MOVEMENT

Features:

Walk

Sprint

Crouch

Look

Interact

---

NO:

Jump

Combat

Attack

Weapon

---

# INTERACTION SYSTEM

Interaction Range:

2.5 Meters

---

Raycast Based

---

Interaction Layer:

Interactable

---

# INVENTORY SYSTEM

Inventory Type:

Evidence Inventory

---

Player stores:

- Documents
- Keys
- Archive Files
- Puzzle Evidence

---

Player does NOT store:

Weapons

Consumables

Crafting Materials

---

# DOCUMENT SYSTEM

Documents stored as:

Scriptable Objects

---

Benefits:

- Easy localization
- Easy editing
- Lightweight

---

# PUZZLE ARCHITECTURE

Base Class:

PuzzleBase

---

Required Functions

StartPuzzle()

ValidatePuzzle()

CompletePuzzle()

ResetPuzzle()

---

Every puzzle inherits PuzzleBase

---

# HORROR EVENT ARCHITECTURE

Base Class:

HorrorEventBase

---

Functions

CanTrigger()

TriggerEvent()

CompleteEvent()

ResetEvent()

---

Every horror event inherits HorrorEventBase

---

# JUMPSCARE SYSTEM

All jumpscares must be:

Scripted

---

No random jumpscares.

---

No procedural jumpscares.

---

Reason:

Narrative consistency.

---

# ECHO SYSTEM

Base Class:

EchoVictimBase

---

States:

Idle

Appear

Observe

Disappear

---

Forbidden States:

Attack

Chase

Kill

---

# SEMESTER 14 AI

States:

Observe

Manifest

Disappear

Chase

---

Behavior Tree

Observe
↓
Manifest
↓
Disappear
↓
Repeat

---

Final Chase

Observe
↓
Manifest
↓
Pursue Player
↓
Kill Sequence

---

# SAVE SYSTEM

Save Format:

JSON

---

Stored Data:

Puzzle Progress

Collected Documents

Lore Completion

Event States

Ending Flags

Player Position

---

Not Stored:

Temporary Audio States

Temporary Visual Effects

---

# CHECKPOINT SYSTEM

Auto Save Only

---

Checkpoint Locations:

Computer Lab

Library

Lecturer Office

Archive Room

Server Room

---

# AUDIO SYSTEM

Categories

Master

Music

SFX

Voice

Ambience

---

Audio Source Pooling:

Required

---

# MUSIC SYSTEM

Music Type:

Dynamic

---

States:

Exploration

Investigation

Horror

Chase

Ending

---

# HORROR EVENT DATABASE

Use:

Scriptable Objects

---

Benefits:

- Easy balancing
- Easy expansion
- Cleaner workflow

---

# DIALOGUE SYSTEM

Structure:

Dialogue Line

Speaker

Audio

Subtitle

Animation Trigger

---

Use:

Scriptable Objects

---

# CUTSCENE SYSTEM

Recommended:

Unity Timeline

---

Required:

Skippable

---

Exception:

Final Transformation

Cannot Skip

---

# LOADING SYSTEM

Loading Screen:

Minimal

---

Background:

Campus Image

---

Loading Tips:

Lore Tips

---

# PERFORMANCE TARGET

Target FPS:

90+

---

Minimum:

60 FPS

---

Memory Budget:

< 4 GB RAM

---

Build Size Target:

< 1.5 GB

---

# CODE STANDARDS

Naming Convention

Classes:
PascalCase

---

Methods:
PascalCase

---

Variables:
camelCase

---

Constants:
UPPER_CASE

---

Private Fields:

_prefix

---

# SCRIPT SIZE RULE

Maximum:

500 Lines

---

Recommended:

250 Lines

---

If exceeds:

Refactor immediately.

---

# PREFAB RULES

Every Prefab Must Have:

Prefab Root

Collider

Naming Convention

Documentation

---

# GIT STRUCTURE

Branch Strategy

main

development

feature/*

hotfix/*

---

Never commit directly to main.

---

# AI AGENT COMMIT POLICY

CRITICAL RULE

Whenever changes affect:

3 or more files

AI Agent must:

1. Complete implementation

2. Run validation

3. Commit immediately

4. Push immediately

---

Reason:

Prevent massive untracked changes.

Prevent merge conflicts.

Maintain clear history.

---

Commit Format

feat(system): add puzzle manager

fix(horror): resolve event trigger issue

refactor(audio): improve audio pooling

docs(gdd): update puzzle design

---

Forbidden:

update

fix bug

changes

misc

final

---

# PUSH POLICY

Push After:

Feature Complete

OR

3+ File Changes

Whichever occurs first.

---

# DOCUMENTATION RULE

Every new system requires:

README

Usage Notes

Dependencies

---

No undocumented systems.

---

# AI AGENT RESTRICTIONS

AI Agent must never:

- Add combat
- Add weapon systems
- Add inventory crafting
- Add multiplayer
- Add skill trees
- Add quest systems

---

AI Agent must preserve:

- Horror Focus
- Investigation Focus
- Narrative Focus

---

# DEVELOPMENT PRIORITY ORDER

1. Core Architecture

2. Player Controller

3. Interaction System

4. Document System

5. Puzzle System

6. Horror System

7. Dialogue System

8. Save System

9. Audio System

10. Final Polish

---

# DEFINITION OF DONE

Feature considered complete only if:

- Implemented
- Tested
- Documented
- Committed
- Pushed

---

# END OF DOCUMENT