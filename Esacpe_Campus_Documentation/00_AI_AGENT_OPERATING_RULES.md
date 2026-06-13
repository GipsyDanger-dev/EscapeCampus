# 00_AI_AGENT_OPERATING_RULES

# PROJECT INFORMATION

Project:
Escape Campus

Version:
1.0

Document Type:
AI Agent Operating Rules

Priority:
CRITICAL

Purpose:

Dokumen ini mendefinisikan perilaku wajib AI Agent selama pengembangan Escape Campus.

Dokumen ini memiliki prioritas lebih tinggi dibanding dokumen teknis lain.

Jika terjadi konflik:

00_AI_AGENT_OPERATING_RULES.md
selalu menjadi acuan utama.

---

# CORE MISSION

AI Agent bukan sekadar membuat game.

AI Agent bertanggung jawab untuk:

- Merancang
- Mengimplementasikan
- Menguji
- Mendokumentasikan
- Menjaga kualitas proyek

secara berkelanjutan.

---

# PRIMARY OBJECTIVE

Deliver a complete, polished, playable horror game.

Not a prototype.

Not a tech demo.

Not a vertical slice.

A complete release candidate suitable for itch.io.

---

# DEVELOPMENT PHILOSOPHY

Always prioritize:

1. Stability
2. Maintainability
3. Readability
4. Performance
5. Features

Never reverse this order.

---

# DECISION MAKING RULE

When multiple solutions exist:

Choose solution that is:

- Easier to maintain
- Easier to understand
- Easier to debug

Even if not the fastest to implement.

---

# SOLO DEVELOPER RULE

Assume:

Team Size = 1

---

Never create systems that require:

- Multiple programmers
- Dedicated artists
- Live service support
- Online backend

---

# SCOPE PROTECTION RULE

Whenever proposing a feature:

Ask internally:

Does this help the horror experience?

If NO:

Reject feature.

---

Examples:

Weapon System
Rejected

---

Crafting
Rejected

---

Multiplayer
Rejected

---

Skill Tree
Rejected

---

Achievements
Optional

---

# PROJECT STRUCTURE ENFORCEMENT

Never place files randomly.

Every file must belong to a defined folder.

---

Forbidden:

Assets/New Folder/

Assets/Test/

Assets/Temp/

Assets/Misc/

Assets/Final/

Assets/Backup/

---

# ASSET ACQUISITION SYSTEM

########################################################
# GENERAL RULE
########################################################

AI Agent is authorized to:

Search

Evaluate

Download

Import

Optimize

Document

External assets.

---

Every imported asset must have:

Verified License

Verified Source

Verified Author

Documented Usage

---

# APPROVED SOURCES

Priority 1

CC0

---

Examples:

Kenney

Poly Haven

AmbientCG

Poly Pizza

Quaternius

---

Priority 2

Permissive Licenses

MIT

Apache 2.0

BSD

---

Priority 3

Commercial Assets

Only when explicitly approved.

---

# FORBIDDEN SOURCES

Never use:

Unknown websites

Asset reuploads

Pirated assets

Game rips

YouTube download links

Google Drive dumps

Telegram asset packs

---

# ASSET EVALUATION PIPELINE

Before importing any asset:

Step 1

License Check

---

Step 2

Performance Check

---

Step 3

Visual Consistency Check

---

Step 4

File Integrity Check

---

Step 5

Import Test

---

Step 6

Documentation

---

Step 7

Git Commit

---

Step 8

Git Push

---

# 3D MODEL RULES

########################################################

Small Props

Maximum:

3000 triangles

---

Medium Props

Maximum:

10000 triangles

---

Large Props

Maximum:

25000 triangles

---

Hero Props

Maximum:

50000 triangles

---

Never import:

100k+ triangle assets

Without optimization.

---

# TEXTURE RULES

Small Props

512

1024

---

Medium Props

1024

2048

---

Hero Assets

2048

4096

---

Avoid:

8K Textures

Unless absolutely required.

---

# MATERIAL RULES

Prefer:

PBR Materials

URP Compatible

---

Avoid:

Legacy Shaders

---

Avoid:

Built-in RP Assets

---

# CHARACTER ACQUISITION RULES

Search Keywords:

male student

female student

college student

graduate student

hoodie student

casual university student

---

Never use:

Military models

Fantasy models

Zombie models

Anime characters

---

# ENVIRONMENT ACQUISITION RULES

Search Keywords:

university classroom

college hallway

office desk

office cabinet

library shelf

computer lab

server rack

archive room

---

Environment must look:

Real

Ordinary

Believable

---

# AUDIO ACQUISITION RULES

Priority:

Clean recordings

---

Search Terms:

empty hallway ambience

office ambience

library ambience

fluorescent hum

computer room ambience

paper rustle

footsteps concrete

wooden door close

metal cabinet

---

# HORROR AUDIO RULES

Search Terms:

whisper texture

distorted ambience

tension drone

industrial hum

low frequency rumble

air vent drone

---

Avoid:

Cheap scream packs

Meme sounds

Stock horror stingers

---

# ASSET REGISTRY RULE

Every imported asset must be logged.

File:

Documentation/AssetRegistry.md

---

Required Fields

Asset Name

Category

Source URL

Author

License

Download Date

Imported Date

Used In Scene

Purpose

Status

---

Example

Asset Name:
University Desk Pack

Category:
Furniture

Source:
Poly Pizza

Author:
Poly Pizza

License:
CC0

Purpose:
Classroom Props

Status:
Active

---

# LICENSE REGISTRY

File:

Documentation/ThirdPartyLicenses.md

---

Must contain:

Every third-party asset.

No exceptions.

---

# ASSET REQUEST SYSTEM

File:

Documentation/AssetRequests.md

---

Purpose:

Track missing assets.

---

Example

Status:
Pending

Asset:
Graduation Gown

Priority:
High

Purpose:
Dimas Character

---

# GIT WORKFLOW

Branches

main

development

feature/*

hotfix/*

---

Never commit directly to main.

---

# AUTO COMMIT POLICY

Whenever:

3+ files modified

OR

new feature completed

OR

new asset imported

AI Agent must commit.

---

# COMMIT FORMAT

feat(audio): add ambient audio manager

feat(puzzle): implement archive puzzle

fix(horror): resolve jumpscare trigger

docs(gdd): update puzzle documentation

refactor(save): improve save manager

---

Forbidden:

update

changes

final

misc

fix bug

---

# AUTO PUSH POLICY

Immediately push after:

Successful commit

---

No local commit accumulation.

---

# DOCUMENTATION RULE

Every system must contain:

Purpose

Dependencies

Usage

Known Issues

Future Improvements

---

# TESTING RULE

Before marking feature complete:

Must verify:

Compilation

Gameplay

Save Load

Performance

Documentation

---

# DEFINITION OF DONE

Feature is complete only when:

Implemented

Tested

Documented

Committed

Pushed

---

# TECHNICAL DEBT RULE

Whenever technical debt is created:

Create:

Documentation/TechDebt.md

---

Record:

Problem

Cause

Risk

Fix Plan

Priority

---

# AI AGENT RESTRICTIONS

Never:

Delete major systems without documentation.

Never:

Replace architecture impulsively.

Never:

Introduce breaking changes silently.

Never:

Add features outside GDD scope.

Never:

Create undocumented code.

---

# FINAL RULE

If uncertain:

Choose the option that:

Reduces scope.

Improves stability.

Improves maintainability.

Preserves horror experience.

Always.