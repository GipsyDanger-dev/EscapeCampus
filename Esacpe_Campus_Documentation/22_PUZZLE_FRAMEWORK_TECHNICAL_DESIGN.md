# 22_PUZZLE_FRAMEWORK_TECHNICAL_DESIGN

# PROJECT INFORMATION

Project:
Escape Campus

Version:
1.0

Document Type:
Technical Design Document

System:
Puzzle Framework

Priority:
CRITICAL

Purpose:

Mendefinisikan arsitektur teknis seluruh sistem puzzle di Escape Campus.

Puzzle merupakan inti gameplay utama.

Player tidak hanya mencari kunci atau menekan tombol.

Player harus:

- Menginvestigasi
- Membaca dokumen
- Menghubungkan bukti
- Menyusun timeline
- Mengidentifikasi kontradiksi
- Menarik kesimpulan

Puzzle harus terasa seperti investigasi akademik yang berubah menjadi mimpi buruk psikologis.

Dokumen ini menjadi sumber kebenaran utama seluruh implementasi puzzle.

---

# SYSTEM OVERVIEW

Puzzle Framework bertanggung jawab untuk:

- Puzzle Registration
- Puzzle State Management
- Puzzle Progress Tracking
- Clue Validation
- Multi-Step Puzzle Logic
- Puzzle Dependencies
- Puzzle Completion Events

---

Puzzle Framework tidak bertanggung jawab untuk:

- Save Serialization
- UI Theme
- Inventory Storage
- Document Storage

---

########################################################
# DESIGN PHILOSOPHY
########################################################

Escape Campus bukan game puzzle tradisional.

Puzzle bukan:

Find Key

↓

Open Door

---

Puzzle bukan:

Find Battery

↓

Insert Battery

---

Puzzle harus:

Observe

↓

Investigate

↓

Correlate

↓

Infer

↓

Validate

↓

Progress

---

########################################################
# PUZZLE DIFFICULTY TARGET
########################################################

Easy

0%

---

Intermediate

30%

---

Advanced

40%

---

Expert

30%

---

No tutorial puzzles.

---

No handholding.

---

########################################################
# CORE ARCHITECTURE
########################################################

Puzzle Object

↓

Puzzle Manager

↓

Puzzle Registry

↓

Puzzle State

↓

Completion Validator

↓

Event System

---

########################################################
# SCRIPT STRUCTURE
########################################################

Gameplay/

Puzzles/

PuzzleManager.cs

PuzzleRegistry.cs

PuzzleState.cs

PuzzleValidator.cs

PuzzleDependencyGraph.cs

PuzzleEventChannel.cs

PuzzleTracker.cs

IPuzzle.cs

---

ScriptableObjects/

SO_Puzzle_*

---

########################################################
# PUZZLE IDENTIFICATION
########################################################

Every puzzle requires:

PuzzleID

---

Format

PZ_[NAME]

---

Examples

PZ_ARCHIVE_LOCK

PZ_LIBRARY_TIMELINE

PZ_SERVER_SEQUENCE

PZ_RITUAL_RECONSTRUCTION

PZ_DEFENSE_ROOM

---

Puzzle IDs never change.

---

########################################################
# PUZZLE CATEGORIES
########################################################

CATEGORY_01

Investigation

---

CATEGORY_02

Timeline Reconstruction

---

CATEGORY_03

Document Correlation

---

CATEGORY_04

Evidence Validation

---

CATEGORY_05

Sequence Deduction

---

CATEGORY_06

Psychological Puzzle

---

CATEGORY_07

Reality Distortion Puzzle

---

CATEGORY_08

Ending Puzzle

---

########################################################
# PUZZLE STATES
########################################################

LOCKED

---

DISCOVERED

---

IN_PROGRESS

---

SOLVED

---

FAILED

---

HIDDEN

---

########################################################
# PUZZLE DEPENDENCY SYSTEM
########################################################

Supported

YES

---

Example

Puzzle B

requires

Puzzle A

---

Puzzle C

requires

Puzzle B

---

Puzzle D

requires

A + B + C

---

Must support chain progression.

---

########################################################
# CLUE SYSTEM
########################################################

Clues are independent objects.

---

Each clue contains:

ClueID

Source

Category

Importance

RelatedPuzzle

---

########################################################
# CLUE TYPES
########################################################

Document

---

Evidence

---

Audio

---

Visual

---

Environmental

---

Timeline

---

Character

---

Hidden

---

########################################################
# CLUE DISTRIBUTION RULE
########################################################

No puzzle solved from single clue.

---

Minimum clues per puzzle

3

---

Target

5–10

---

Expert Puzzles

10+

---

########################################################
# INVESTIGATION PUZZLES
########################################################

Player must collect clues.

---

Player must determine:

Who

What

When

Where

Why

---

No direct answers.

---

########################################################
# TIMELINE PUZZLES
########################################################

Player reconstructs chronology.

---

Example

Document A

2023

---

Document B

2024

---

Document C

Missing Date

---

Player deduces order.

---

########################################################
# CONTRADICTION SYSTEM
########################################################

Supported

YES

---

Purpose

Create uncertainty.

---

Example

Document 1

Raka absent.

---

Document 2

Raka attended.

---

Player must identify false record.

---

########################################################
# FALSE CLUE SYSTEM
########################################################

Supported

YES

---

Maximum

20% of clues

---

Purpose

Psychological confusion.

---

Rule

Never impossible.

---

False clues must be logically solvable.

---

########################################################
# REALITY DISTORTION PUZZLES
########################################################

Supported

YES

---

Example

Room changes layout.

---

Document changes text.

---

Timeline shifts.

---

Audio clues mutate.

---

Must always have consistent solution.

---

########################################################
# PSYCHOLOGICAL PUZZLES
########################################################

Goal

Challenge assumptions.

---

Example

Player trusts archive.

---

Archive is corrupted.

---

Real answer hidden elsewhere.

---

########################################################
# ACADEMIC HORROR PUZZLES
########################################################

Theme

University Bureaucracy

Academic Records

Graduation Files

Defense Schedules

Transcript Analysis

Research Logs

Supervisor Reports

---

Puzzle content must fit campus setting.

---

########################################################
# EVIDENCE CORRELATION SYSTEM
########################################################

Player gathers evidence.

---

Evidence linked together.

---

Example

Defense Schedule

+

Attendance Sheet

+

Supervisor Email

↓

Reveal Hidden Fact

---

########################################################
# PASSWORD PUZZLES
########################################################

Allowed

YES

---

Restrictions

Never random.

---

Password must be inferable.

---

Source must exist in world.

---

########################################################
# CODE PUZZLES
########################################################

Allowed

YES

---

Examples

Archive Codes

Student IDs

Room Numbers

Transcript Numbers

Defense Records

---

Must have narrative meaning.

---

########################################################
# MULTI-ROOM PUZZLES
########################################################

Required

YES

---

Player must revisit areas.

---

Player must combine information.

---

Minimum

4 Major Multi-Room Puzzles

---

########################################################
# PUZZLE FAILURE RULES
########################################################

Wrong Answer

Allowed

---

Puzzle Lockout

Forbidden

---

Player may retry indefinitely.

---

########################################################
# HINT SYSTEM
########################################################

Default

Disabled

---

Reason

Preserve difficulty.

---

Optional

Available from settings.

---

Hint Levels

Level 1

Direction

---

Level 2

Strong Hint

---

Level 3

Near Solution

---

########################################################
# HORROR INTEGRATION
########################################################

Puzzle completion may trigger:

Whispers

---

Manifestations

---

Jumpscares

---

Distortions

---

Semester 14 Activity

---

########################################################
# JUMPSCARE INTEGRATION
########################################################

Never random.

---

Always narrative justified.

---

Examples

Solve Ritual Puzzle

↓

Trigger Echo Event

---

Open Secret Archive

↓

Semester 14 Appears

---

########################################################
# PUZZLE EVENT SYSTEM
########################################################

Events

PuzzleStarted

PuzzleUpdated

PuzzleSolved

PuzzleFailed

PuzzleUnlocked

---

Must be broadcast through Event System.

---

########################################################
# SAVE SYSTEM INTEGRATION
########################################################

Must Save

Puzzle State

---

Puzzle Progress

---

Collected Clues

---

Unlocked Puzzles

---

Solved Puzzles

---

########################################################
# ENDING SYSTEM INTEGRATION
########################################################

Good Ending

Requires:

Minimum Puzzle Completion

---

True Ending

Requires:

All Critical Puzzles

---

Secret Ending

Requires:

Hidden Puzzle Chain

---

########################################################
# TARGET PUZZLE COUNT
########################################################

Minor Puzzles

8–12

---

Major Puzzles

5–7

---

Critical Puzzles

3–5

---

Secret Puzzles

2–3

---

Total

18–25

---

########################################################
# MAJOR PUZZLE EXAMPLES
########################################################

PZ_LIBRARY_TIMELINE

Reconstruct Raka's final week.

---

PZ_ARCHIVE_CORRELATION

Find forged academic records.

---

PZ_SERVER_RECOVERY

Recover deleted defense files.

---

PZ_RITUAL_RECONSTRUCTION

Understand ritual loop.

---

PZ_FINAL_DEFENSE

Determine true cause of disappearance.

---

########################################################
# PERFORMANCE REQUIREMENTS
########################################################

Puzzle evaluation event-driven.

---

No heavy polling.

---

No frame-by-frame validation.

---

Target Cost

Negligible

---

########################################################
# DEBUG TOOLS
########################################################

Development Only

---

Unlock All Puzzles

---

Solve Current Puzzle

---

Show Missing Clues

---

Display Dependency Graph

---

Reset Puzzle State

---

Disabled In Release

---

########################################################
# TEST CASES
########################################################

TC-01

Puzzle Discovery Works

---

TC-02

Puzzle Progress Works

---

TC-03

Dependency System Works

---

TC-04

Clue Validation Works

---

TC-05

Timeline Puzzle Works

---

TC-06

False Clues Work

---

TC-07

Save Restore Works

---

TC-08

Ending Validation Works

---

TC-09

Reality Distortion Puzzle Works

---

TC-10

No Soft Locks

---

########################################################
# ANTI-SOFTLOCK RULES
########################################################

Critical clues cannot disappear.

---

Critical clues cannot be destroyed.

---

Critical puzzles always recoverable.

---

Player can always finish game.

---

Mandatory.

---

########################################################
# DEFINITION OF DONE
########################################################

Puzzle System can:

Register

Track

Validate

Unlock

Solve

Save

Restore

Trigger Events

Support Endings

---

All test cases pass.

---

No critical bugs.

---

No soft locks.

---

Documentation updated.

---

Committed.

---

Pushed.

---

# END OF DOCUMENT