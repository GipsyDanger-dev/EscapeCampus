# 20_DOCUMENT_SYSTEM_TECHNICAL_DESIGN

# PROJECT INFORMATION

Project:
Escape Campus

Version:
1.0

Document Type:
Technical Design Document

System:
Document System

Priority:
CRITICAL

Purpose:

Mendefinisikan arsitektur teknis seluruh sistem dokumen di Escape Campus.

Document System merupakan fondasi utama:

- Lore Delivery
- Puzzle Solving
- Investigation
- Evidence Collection
- Narrative Progression
- Ending Requirements

Sebagian besar gameplay berasal dari membaca, memahami, dan menghubungkan dokumen.

Dokumen ini menjadi sumber kebenaran utama implementasi Document System.

---

# SYSTEM OVERVIEW

Document System bertanggung jawab untuk:

- Document Collection
- Document Viewing
- Document Database
- Evidence Tracking
- Puzzle Clue Management
- Narrative Unlocking
- Progress Tracking

---

Document System tidak bertanggung jawab untuk:

- Save Serialization
- Inventory Logic
- Puzzle Logic
- UI Theme Management

---

########################################################
# DESIGN GOALS
########################################################

Goal 01

Documents are gameplay.

---

Goal 02

Documents contain meaningful information.

---

Goal 03

Documents support investigation.

---

Goal 04

Documents support puzzle solving.

---

Goal 05

Documents support multiple endings.

---

########################################################
# CORE ARCHITECTURE
########################################################

DocumentPickup

↓

DocumentDatabase

↓

DocumentManager

↓

DocumentViewer

↓

EvidenceSystem

↓

NarrativeSystem

---

########################################################
# SCRIPT STRUCTURE
########################################################

Gameplay/

Documents/

DocumentManager.cs

DocumentDatabase.cs

DocumentViewer.cs

DocumentEntry.cs

DocumentCategory.cs

DocumentPickup.cs

DocumentUnlockTracker.cs

DocumentSearchUtility.cs

---

ScriptableObjects/

SO_Document_*

---

########################################################
# DOCUMENT DATA MODEL
########################################################

Every document must contain:

DocumentID

Title

Category

Author

Date

Summary

Content

Tags

EvidenceValue

RequiredForEnding

RequiredForPuzzle

Hidden

Unlocked

---

########################################################
# DOCUMENT ID RULE
########################################################

Format

DOC_[CATEGORY]_[NUMBER]

---

Examples

DOC_EMAIL_001

DOC_ARCHIVE_004

DOC_RITUAL_003

DOC_EVIDENCE_010

DOC_SECRET_001

---

Document IDs never change.

Mandatory.

---

########################################################
# DOCUMENT CATEGORIES
########################################################

EMAIL

---

ACADEMIC

---

ARCHIVE

---

THESIS

---

RITUAL

---

EVIDENCE

---

SECRET

---

ENDING

---

########################################################
# DOCUMENT RARITY
########################################################

COMMON

General Lore

---

IMPORTANT

Puzzle Related

---

CRITICAL

Ending Related

---

SECRET

Secret Ending Related

---

########################################################
# DOCUMENT STORAGE
########################################################

Storage Type

ScriptableObject

Mandatory

---

Reason

Easy management

Easy localization

Easy searching

Easy expansion

---

########################################################
# DOCUMENT PICKUP FLOW
########################################################

Player interacts

↓

DocumentPickup

↓

DocumentManager

↓

Database Updated

↓

UI Notification

↓

Save State Updated

↓

Optional Narrative Event

---

########################################################
# DOCUMENT COLLECTION RULES
########################################################

Document can only be collected once.

---

Duplicate collection forbidden.

---

Already collected

↓

Show:

Document Already Archived

---

########################################################
# DOCUMENT VIEWER SYSTEM
########################################################

Viewer Layout

Left Panel

Document List

---

Right Panel

Document Content

---

Top Area

Title

Author

Date

Category

---

Bottom Area

Tags

Evidence Marker

---

########################################################
# DOCUMENT SORTING
########################################################

Sort By

Date

---

Category

---

Discovery Order

---

Importance

---

Alphabetical

---

########################################################
# DOCUMENT SEARCH SYSTEM
########################################################

Supported

YES

---

Search Fields

Title

Content

Tags

Author

---

Search must be case insensitive.

---

########################################################
# TAG SYSTEM
########################################################

Examples

Raka

Semester14

Graduation

Defense

Archive

Loop

Ritual

Library

ServerRoom

Professor

---

Purpose

Investigation support.

---

########################################################
# EVIDENCE SYSTEM INTEGRATION
########################################################

Every document may generate evidence.

---

Evidence Types

Timeline

Person

Location

Event

Object

---

Example

Document:

Defense Cancellation

↓

Evidence:

Raka Missed Defense

---

########################################################
# PUZZLE SYSTEM INTEGRATION
########################################################

Documents may contain:

Passwords

Codes

Dates

Coordinates

Sequences

Names

Patterns

---

Examples

Archive Code

Server Password

Cabinet Combination

Timeline Clue

---

########################################################
# EXPERT PUZZLE SUPPORT
########################################################

Required

YES

---

Puzzle clues may exist across:

Multiple documents

Different locations

Different categories

Different timestamps

---

Players must connect information manually.

---

No automatic clue solving.

---

########################################################
# DOCUMENT RELATIONSHIP GRAPH
########################################################

Document can reference:

Person

Location

Event

Object

Other Document

---

Purpose

Investigation depth.

---

########################################################
# TIMELINE SYSTEM SUPPORT
########################################################

Documents contain dates.

---

Timeline can be reconstructed.

---

Purpose

Reveal hidden story.

---

########################################################
# DOCUMENT UNLOCK SYSTEM
########################################################

Some documents hidden initially.

---

Unlock Conditions

Puzzle Complete

Evidence Threshold

Story Progress

Secret Trigger

---

########################################################
# SECRET DOCUMENT SYSTEM
########################################################

Secret documents do not appear normally.

---

Unlock only after:

Specific conditions met.

---

Examples

Secret Ending

True Story Reveal

Ritual Explanation

---

########################################################
# NARRATIVE EVENT TRIGGERS
########################################################

Reading document may trigger:

Whispers

Echoes

Flashbacks

Distortions

Jumpscares

Objectives

---

Example

Read Ritual Document

↓

Trigger Whisper Event

---

########################################################
# DOCUMENT NOTIFICATIONS
########################################################

Display

New Document Archived

---

Duration

2 Seconds

---

Position

Top Right

---

########################################################
# DOCUMENT PROGRESSION TRACKING
########################################################

Track:

Collected

Viewed

Important Viewed

Critical Viewed

Secret Viewed

---

Purpose

Ending validation.

---

########################################################
# ENDING SYSTEM INTEGRATION
########################################################

Good Ending Requirements

Minimum Critical Documents Found

---

Secret Ending Requirements

All Secret Documents Found

---

Hidden Ending Requirements

Specific Combination

---

########################################################
# SAVE SYSTEM INTEGRATION
########################################################

Must Save

Collected Documents

Viewed Documents

Unlocked Documents

Secret Progress

Evidence Progress

---

Must Restore

Exactly

---

########################################################
# AUDIO SUPPORT
########################################################

Document Open

Paper Open

---

Document Close

Paper Close

---

Scroll

Paper Rustle

---

Important Discovery

Subtle Audio Cue

---

########################################################
# HORROR DOCUMENT RULES
########################################################

Never use cheap exposition.

---

Avoid

"Monster is behind you."

---

Prefer

Incomplete information.

Contradictions.

Missing pages.

Academic records.

---

Example

Page 1 exists

Page 2 missing

Page 3 exists

---

Creates paranoia.

---

########################################################
# INVESTIGATION PRINCIPLES
########################################################

Players should:

Read

Think

Connect

Infer

Conclude

---

Game should never:

Explain everything directly.

---

########################################################
# DOCUMENT BALANCE TARGET
########################################################

Common Documents

20–25

---

Important Documents

10–15

---

Critical Documents

8–12

---

Secret Documents

5–8

---

Total Target

50+

---

########################################################
# PERFORMANCE REQUIREMENTS
########################################################

All documents loaded from database.

---

No runtime file parsing.

---

No string-heavy searching every frame.

---

Cache search results.

---

Target Cost

Negligible

---

########################################################
# DEBUG TOOLS
########################################################

Development Only

---

Unlock All Documents

---

View Missing Documents

---

Force Secret Documents

---

Export Evidence Graph

---

Disabled In Release

---

########################################################
# TEST CASES
########################################################

TC-01

Document Pickup Works

---

TC-02

Document Viewer Works

---

TC-03

Sorting Works

---

TC-04

Search Works

---

TC-05

Evidence Generation Works

---

TC-06

Puzzle Clues Work

---

TC-07

Unlock Conditions Work

---

TC-08

Save Restore Works

---

TC-09

Secret Documents Work

---

TC-10

Ending Validation Works

---

########################################################
# DEFINITION OF DONE
########################################################

Documents can:

Collect

Store

Read

Search

Sort

Unlock

Generate Evidence

Support Puzzles

Support Endings

---

All tests pass.

---

No critical bugs.

---

Documentation updated.

---

Committed.

---

Pushed.

---

# END OF DOCUMENT