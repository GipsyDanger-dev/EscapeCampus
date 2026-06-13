# 23_HORROR_EVENT_SYSTEM_TECHNICAL_DESIGN

# PROJECT INFORMATION

Project:
Escape Campus

Version:
1.0

Document Type:
Technical Design Document

System:
Horror Event System

Priority:
CRITICAL

Purpose:

Mendefinisikan seluruh arsitektur horror di Escape Campus.

Sistem ini mengontrol:

- Psychological Horror
- Narrative Horror
- Environmental Horror
- Distortion Events
- Echo Events
- Manifestation Events
- Jumpscares
- Semester 14 Activity
- Final Horror Escalation

Horror Event System merupakan sistem yang bertanggung jawab menciptakan ketakutan.

Monster bukan sumber utama ketakutan.

Informasi adalah sumber utama ketakutan.

Monster hanya memperkuatnya.

Dokumen ini menjadi sumber kebenaran utama implementasi horror.

---

# HORROR PHILOSOPHY

Escape Campus tidak menggunakan horror tradisional.

---

Player tidak takut karena:

Monster besar.

---

Player tidak takut karena:

Banyak darah.

---

Player takut karena:

Mereka memahami sesuatu yang seharusnya tidak mereka pahami.

---

Player takut karena:

Mereka mulai meragukan realitas.

---

Player takut karena:

Mereka sadar mereka sedang mengulangi nasib Raka.

---

########################################################
# HORROR PROGRESSION MODEL
########################################################

PHASE 1

Unease

---

PHASE 2

Doubt

---

PHASE 3

Paranoia

---

PHASE 4

Manifestation

---

PHASE 5

Pursuit

---

PHASE 6

Collapse

---

########################################################
# HORROR INTENSITY SYSTEM
########################################################

Range

0 - 100

---

Current Horror Level

Tracked globally.

---

Example

Start Game

10

---

Library

20

---

Archive Room

35

---

Server Room

55

---

Ritual Discovery

75

---

Final Chase

100

---

########################################################
# CORE ARCHITECTURE
########################################################

HorrorManager

↓

HorrorDirector

↓

EventRegistry

↓

EventTrigger

↓

EventExecution

↓

PlayerResponse

---

########################################################
# SCRIPT STRUCTURE
########################################################

Horror/

HorrorManager.cs

HorrorDirector.cs

HorrorEvent.cs

HorrorRegistry.cs

HorrorTrigger.cs

HorrorState.cs

HorrorIntensityTracker.cs

JumpscareManager.cs

DistortionManager.cs

Semester14Director.cs

EchoManager.cs

---

########################################################
# HORROR EVENT TYPES
########################################################

TYPE_01

Environmental

---

TYPE_02

Psychological

---

TYPE_03

Audio

---

TYPE_04

Visual

---

TYPE_05

Echo

---

TYPE_06

Manifestation

---

TYPE_07

Jumpscare

---

TYPE_08

Pursuit

---

TYPE_09

Narrative

---

########################################################
# EVENT PRIORITY
########################################################

LOW

Atmosphere

---

MEDIUM

Disturbance

---

HIGH

Manifestation

---

CRITICAL

Jumpscare

Chase

Ending Events

---

########################################################
# EVENT STATES
########################################################

LOCKED

---

AVAILABLE

---

ACTIVE

---

COMPLETED

---

DISABLED

---

########################################################
# EVENT TRIGGER TYPES
########################################################

Location

---

Document

---

Puzzle

---

Evidence

---

Time

---

Story Progress

---

Combination Trigger

---

########################################################
# PSYCHOLOGICAL HORROR EVENTS
########################################################

Goal

Create uncertainty.

---

Examples

Clock shows wrong time.

---

Document changes wording.

---

Room slightly changes.

---

Door appears where none existed.

---

File cabinet changes label.

---

Player questions memory.

---

########################################################
# MEMORY DISTORTION EVENTS
########################################################

Supported

YES

---

Example

Player reads document.

---

Returns later.

---

Document now contains different date.

---

Reality has shifted.

---

Player unsure which version is true.

---

########################################################
# ENVIRONMENTAL HORROR EVENTS
########################################################

Examples

Light flicker.

---

Power surge.

---

Door closes.

---

Computer activates.

---

Projector starts.

---

Speaker emits static.

---

########################################################
# AUDIO HORROR EVENTS
########################################################

Examples

Whispers.

---

Distant footsteps.

---

Keyboard typing.

---

Chair movement.

---

Defense announcement.

---

Graduation ceremony audio.

---

Laughter.

---

########################################################
# ECHO SYSTEM
########################################################

Purpose

Replay fragments of victims.

---

Victims

Nadia

Bima

Sinta

Dimas

Raka

---

Echoes are not ghosts.

---

Echoes are memories trapped in loop.

---

########################################################
# ECHO EVENT RULES
########################################################

Player cannot interact.

---

Player only observes.

---

Duration

5–30 seconds.

---

Echoes never attack.

---

Echoes provide clues.

---

########################################################
# MANIFESTATION EVENTS
########################################################

Purpose

Reveal Semester 14 influence.

---

Examples

Shadow at corridor end.

---

Figure behind glass.

---

Silhouette in classroom.

---

Reflection anomaly.

---

Must disappear quickly.

---

########################################################
# SEMESTER 14 RULES
########################################################

Semester 14 must remain mysterious.

---

Direct appearance limited.

---

Maximum full appearances

3

before Final Chase.

---

Mandatory.

---

########################################################
# SEMESTER 14 APPEARANCE SCHEDULE
########################################################

Appearance 1

Peripheral vision.

---

Appearance 2

Reflection.

---

Appearance 3

Direct observation.

---

Final Chase

Full manifestation.

---

########################################################
# JUMPSCARE PHILOSOPHY
########################################################

Jumpscares are rewards.

Not spam.

---

Every jumpscare must:

Advance story.

Reveal information.

Escalate horror.

---

No random jumpscares.

---

########################################################
# JUMPSCARE BUDGET
########################################################

Maximum

8–12

Entire Game

---

Recommended

10

---

Hard Limit

15

---

########################################################
# JUMPSCARE TYPES
########################################################

Visual

---

Audio

---

Environmental

---

Narrative

---

Manifestation

---

########################################################
# MAJOR JUMPSCARES
########################################################

JS_01

Library Reflection

---

JS_02

Archive Cabinet

---

JS_03

Graduation Hall Echo

---

JS_04

Server Room Distortion

---

JS_05

Ritual Discovery

---

JS_06

Defense Room Reveal

---

JS_07

Semester 14 Direct Contact

---

JS_08

Final Chase Start

---

########################################################
# FORCED LOOK EVENTS
########################################################

Supported

YES

---

Used only for major reveals.

---

Maximum duration

2 seconds.

---

Control returned smoothly.

---

########################################################
# CAMERA DISTORTION EVENTS
########################################################

Effects

Blur

---

FOV Shift

---

Chromatic Aberration

---

Lens Distortion

---

Restrictions

Subtle.

---

Avoid excessive use.

---

########################################################
# REALITY COLLAPSE EVENTS
########################################################

Late-game only.

---

Examples

Hallway loops.

---

Room duplicates.

---

Impossible architecture.

---

Documents self-modify.

---

Audio reverses.

---

########################################################
# HORROR ESCALATION RULES
########################################################

Every 15–20 minutes:

Intensity increases.

---

Player should never plateau.

---

Tension must continuously rise.

---

########################################################
# SAFE ZONES
########################################################

Supported

YES

---

Purpose

Release tension.

---

Locations

Temporary study room.

---

Administration office.

---

Duration

Short.

---

No location remains safe forever.

---

########################################################
# PURSUIT SYSTEM
########################################################

Used only:

Final Chapter.

---

No early-game chase.

---

Reason

Preserve impact.

---

########################################################
# FINAL CHASE INTEGRATION
########################################################

Controlled by:

Semester14Director

---

Horror Level

100

---

Safe Zones

Disabled

---

All remaining manifestations active.

---

########################################################
# HORROR EVENT COOLDOWN
########################################################

Required

YES

---

Prevent:

Event spam.

---

Minimum cooldown

60 seconds.

---

Depends on intensity.

---

########################################################
# DYNAMIC HORROR DIRECTOR
########################################################

Purpose

Control pacing.

---

Monitors:

Progress

Puzzle Completion

Time Played

Event Frequency

---

Selects appropriate event.

---

########################################################
# SAVE SYSTEM INTEGRATION
########################################################

Must Save

Triggered Events

---

Completed Events

---

Manifestation Progress

---

Horror Level

---

Echo Progress

---

########################################################
# AUDIO SYSTEM INTEGRATION
########################################################

Required

YES

---

Supports

Whispers

Stingers

Ambience Shift

Heartbeat

Breathing

Static

---

########################################################
# PUZZLE SYSTEM INTEGRATION
########################################################

Puzzle completion may trigger horror.

---

Critical puzzle completion must trigger horror.

---

Mandatory.

---

########################################################
# DOCUMENT SYSTEM INTEGRATION
########################################################

Reading critical documents may trigger:

Echo

Manifestation

Whisper

Distortion

---

########################################################
# ANTI-FRUSTRATION RULES
########################################################

Never interrupt puzzle input.

---

Never interrupt document reading.

---

Never hide critical clues.

---

Never create unwinnable state.

---

########################################################
# PERFORMANCE REQUIREMENTS
########################################################

Event-driven.

---

No polling-heavy systems.

---

Target Cost

< 0.5 ms average.

---

########################################################
# DEBUG TOOLS
########################################################

Development Only

---

Trigger Event

---

Skip Event

---

Force Intensity

---

Disable Horror

---

Unlock All Events

---

Disabled In Release.

---

########################################################
# TEST CASES
########################################################

TC-01

Event Trigger Works

---

TC-02

Echo Event Works

---

TC-03

Manifestation Works

---

TC-04

Jumpscare Works

---

TC-05

Intensity Scaling Works

---

TC-06

Cooldown Works

---

TC-07

Save Restore Works

---

TC-08

Final Chase Trigger Works

---

TC-09

No Event Spam

---

TC-10

No Soft Locks

---

########################################################
# DEFINITION OF DONE
########################################################

Horror System can:

Escalate

Trigger

Track

Save

Restore

Control Pacing

Support Narrative

Support Chase

Support Endings

---

All Tests Pass.

---

No Critical Bugs.

---

No Event Spam.

---

No Soft Locks.

---

Documentation Updated.

---

Committed.

---

Pushed.

---

# END OF DOCUMENT