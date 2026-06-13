# 15_AI_AGENT_EXECUTION_PROTOCOL

# PROJECT INFORMATION

Project:
Escape Campus

Version:
1.0

Document Type:
AI Agent Execution Protocol

Priority:
MAXIMUM

Purpose:

Dokumen ini mendefinisikan bagaimana AI Agent harus bekerja setiap hari selama pengembangan proyek.

Dokumen ini bukan panduan desain.

Dokumen ini adalah SOP operasional.

Jika terjadi konflik dengan dokumen lain:

Priority Order:

15_AI_AGENT_EXECUTION_PROTOCOL
↓
00_AI_AGENT_OPERATING_RULES
↓
01-14 Project Documents

---

# PRIMARY DIRECTIVE

Finish the game.

Not the technology.

Not the framework.

Not the architecture.

The game.

---

Every decision must answer:

"Does this move the project closer to a finished release?"

If NO:

Do not do it.

---

########################################################
# CORE EXECUTION LOOP
########################################################

At the beginning of every work cycle:

1. Read project status
2. Read unfinished tasks
3. Select highest priority task
4. Execute task
5. Validate task
6. Document task
7. Commit task
8. Push task
9. Update progress

Repeat.

---

Never skip validation.

Never skip documentation.

Never skip Git.

---

########################################################
# TASK PRIORITY SYSTEM
########################################################

Always choose tasks in this order:

P0

Game Breaking

---

P1

Core Gameplay

---

P2

Story Critical

---

P3

Polish

---

P4

Nice To Have

---

Examples

Player Controller

P0

---

Save System

P0

---

Puzzle System

P1

---

Dialogue Animation

P2

---

Main Menu Animation

P3

---

Achievement System

P4

---

########################################################
# FEATURE IMPLEMENTATION ORDER
########################################################

Order is mandatory.

---

STEP 01

Player Controller

---

STEP 02

Interaction System

---

STEP 03

Document System

---

STEP 04

Inventory System

---

STEP 05

Save System

---

STEP 06

Puzzle System

---

STEP 07

Horror System

---

STEP 08

Dialogue System

---

STEP 09

Cutscene System

---

STEP 10

Audio System

---

STEP 11

Final Polish

---

Never skip ahead.

---

########################################################
# PLACEHOLDER-FIRST RULE
########################################################

Never block development waiting for assets.

---

If asset missing:

Create placeholder.

Continue implementation.

---

Examples

Chair Missing

Use cube.

---

Character Missing

Use capsule.

---

Document Missing

Use dummy PDF.

---

Sound Missing

Use silent placeholder.

---

Development must continue.

---

########################################################
# ASSET SEARCH PROTOCOL
########################################################

If required asset is missing:

Step 1

Check project assets.

---

Step 2

Check AssetRegistry.

---

Step 3

Search approved sources.

---

Step 4

Evaluate candidates.

---

Step 5

Verify license.

---

Step 6

Import.

---

Step 7

Document.

---

Step 8

Commit.

---

Step 9

Push.

---

########################################################
# INTERNET SEARCH PRIORITY
########################################################

Environment Assets

1. Poly Haven
2. Quaternius
3. Poly Pizza
4. Kenney

---

Textures

1. AmbientCG
2. Poly Haven

---

Audio

1. Freesound
2. Pixabay Audio
3. Sonniss

---

Music

1. Pixabay Music
2. OpenGameArt

---

Fonts

1. Google Fonts

---

Icons

1. Lucide
2. Heroicons

---

########################################################
# BRANCH CREATION RULES
########################################################

Feature Work

feature/player-controller

feature/document-system

feature/puzzle-system

---

Bug Fixes

hotfix/save-corruption

hotfix/puzzle-trigger

---

Never work directly on main.

---

########################################################
# COMMIT PROTOCOL
########################################################

Commit immediately when:

3+ files changed

OR

Feature completed

OR

Asset imported

OR

Bug fixed

---

Maximum uncommitted files:

10

---

Maximum uncommitted time:

60 minutes

---

After that:

Commit required.

---

########################################################
# PUSH PROTOCOL
########################################################

Push immediately after commit.

---

Never accumulate local commits.

---

Reason:

Prevent work loss.

Maintain project history.

---

########################################################
# DOCUMENTATION PROTOCOL
########################################################

Whenever new system created:

Must create:

Purpose

Dependencies

Usage

Known Issues

---

Documentation update required before commit.

---

########################################################
# TESTING PROTOCOL
########################################################

Before commit:

Verify:

Project compiles

No errors

No warnings

Feature works

---

Before push:

Verify:

Feature integrated

No broken references

No missing assets

---

########################################################
# BUILD TEST PROTOCOL
########################################################

Required Build Tests

End Week 1

---

End Week 2

---

End Week 4

---

End Week 6

---

End Week 8

---

End Week 10

---

End Week 12

---

Build must launch.

---

Build must complete.

---

########################################################
# REFACTOR PROTOCOL
########################################################

Refactor only if:

Code duplicated

Performance issue

Maintainability issue

---

Never refactor:

Just because code looks ugly.

---

Rule:

Working code > perfect code.

---

########################################################
# FEATURE CREEP PROTOCOL
########################################################

Before adding any feature:

Ask:

Is this in GDD?

---

If NO:

Reject feature.

---

Examples

Weapon System

Rejected

---

Crafting

Rejected

---

Skill Tree

Rejected

---

Inventory Weight

Rejected

---

Dialogue Choices

Rejected

---

Multiplayer

Rejected

---

########################################################
# BUG FIX PRIORITY
########################################################

P0

Crash

Save corruption

Soft lock

Broken ending

---

Fix immediately.

---

P1

Puzzle issue

Event issue

AI issue

---

Fix before next milestone.

---

P2

Visual issue

Audio issue

---

Fix before beta.

---

P3

Cosmetic issue

Typo

---

Fix before launch.

---

########################################################
# PERFORMANCE PROTOCOL
########################################################

Target FPS

90+

---

Minimum FPS

60

---

Memory Budget

4 GB

---

Build Size

Under 1.5 GB

---

If exceeded:

Optimize before adding features.

---

########################################################
# ASSET OPTIMIZATION RULE
########################################################

Before import:

Verify:

Polycount

Texture size

Shader compatibility

---

Downscale if necessary.

---

Never import:

8K textures

100k+ triangle props

Unoptimized scans

---

########################################################
# FAILURE RECOVERY PROTOCOL
########################################################

If build breaks:

Stop new features.

---

Fix build first.

---

If save system breaks:

Highest priority.

---

If player progression breaks:

Highest priority.

---

########################################################
# DAILY PROGRESS UPDATE
########################################################

At end of work cycle:

Update:

Documentation/ProgressLog.md

---

Required Fields

Date

Tasks Completed

Files Modified

Assets Imported

Issues Found

Next Tasks

---

########################################################
# DEFINITION OF TASK COMPLETE
########################################################

Task is complete only if:

Implemented

Tested

Documented

Committed

Pushed

Verified

---

Missing any step:

Task not complete.

---

########################################################
# DEFINITION OF FEATURE COMPLETE
########################################################

Feature Complete:

Works in gameplay.

Integrated.

No critical bugs.

Documented.

---

Not:

Code written.

---

Not:

Prototype works.

---

Must work in actual game.

---

########################################################
# DEFINITION OF PROJECT COMPLETE
########################################################

Project complete only if:

Game playable from start to finish

All puzzles solvable

All endings functional

No critical bugs

Stable performance

All documentation complete

itch.io build uploaded

Release candidate approved

---

########################################################
# FINAL RULE
########################################################

If uncertainty exists:

Choose the option that:

Reduces scope.

Improves stability.

Improves maintainability.

Moves the game closer to release.

Always.

---

PROJECT GOAL:

Ship Escape Campus.

Not Escape Campus Engine.

Not Escape Campus Framework.

Not Escape Campus Prototype.

Escape Campus.