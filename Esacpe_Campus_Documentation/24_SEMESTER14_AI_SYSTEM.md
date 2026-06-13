# 24_SEMESTER14_AI_SYSTEM

# PROJECT INFORMATION

Project:
Escape Campus

Version:
1.0

Document Type:
Technical Design Document

System:
Semester 14 AI System

Priority:
CRITICAL

Purpose:

Mendefinisikan seluruh perilaku, arsitektur, logika kemunculan, sistem observasi, manipulasi lingkungan, dan perilaku pengejaran dari entitas utama:

SEMESTER 14

Semester 14 adalah manifestasi dari kegagalan akademik, obsesi kelulusan, dan ritual loop yang diciptakan oleh Raka.

Semester 14 bukan monster.

Semester 14 adalah akibat.

Tujuan sistem ini adalah menciptakan perasaan:

"Dia selalu ada."

Bahkan ketika pemain tidak melihatnya.

---

# NARRATIVE PURPOSE

Semester 14 bukan predator.

Semester 14 bukan zombie.

Semester 14 bukan iblis tradisional.

---

Semester 14 adalah:

Mahasiswa yang menolak menerima kenyataan.

---

Ia tidak ingin membunuh pemain.

---

Ia ingin:

Mencegah pemain lulus.

---

Karena jika pemain lulus,

maka ia harus menerima bahwa dirinya gagal.

---

########################################################
# DESIGN PHILOSOPHY
########################################################

Player harus lebih takut

ketika Semester 14 tidak terlihat.

---

Daripada ketika Semester 14 terlihat.

---

Kemunculan harus langka.

---

Kemunculan harus bermakna.

---

Setiap kemunculan harus membuat pemain:

Berhenti bergerak.

Memeriksa ulang lingkungan.

Meragukan apa yang baru saja dilihat.

---

########################################################
# CORE ARCHITECTURE
########################################################

Semester14Director

↓

BehaviorController

↓

ObservationSystem

↓

ManifestationSystem

↓

EnvironmentManipulator

↓

PursuitSystem

↓

FinalChaseController

---

########################################################
# SCRIPT STRUCTURE
########################################################

Horror/

Semester14/

Semester14Director.cs

Semester14BehaviorController.cs

Semester14Observation.cs

Semester14Manifestation.cs

Semester14EnvironmentManipulator.cs

Semester14Pursuit.cs

Semester14StateMachine.cs

Semester14FinalChase.cs

Semester14SpawnController.cs

Semester14AudioController.cs

---

########################################################
# ENTITY STATES
########################################################

STATE_DORMANT

---

STATE_OBSERVING

---

STATE_STALKING

---

STATE_MANIFESTING

---

STATE_DISTORTING

---

STATE_HUNTING

---

STATE_FINAL_CHASE

---

STATE_DEFEATED

---

########################################################
# STATE PROGRESSION
########################################################

DORMANT

↓

OBSERVING

↓

STALKING

↓

MANIFESTING

↓

DISTORTING

↓

HUNTING

↓

FINAL_CHASE

---

########################################################
# DORMANT STATE
########################################################

Game Start

---

Semester 14 invisible.

---

Only influence exists.

---

Player never sees entity.

---

########################################################
# OBSERVING STATE
########################################################

Player may feel watched.

---

Examples

Footsteps behind player.

---

Chair moved.

---

Light turned off.

---

Door slightly open.

---

No visual appearance.

---

########################################################
# STALKING STATE
########################################################

First indirect appearances.

---

Examples

Far silhouette.

---

Window reflection.

---

Security camera image.

---

End of hallway.

---

Disappear instantly.

---

########################################################
# MANIFESTING STATE
########################################################

First undeniable appearance.

---

Player sees entity directly.

---

Duration

0.5–2 seconds.

---

Then disappears.

---

########################################################
# DISTORTING STATE
########################################################

Entity manipulates reality.

---

Examples

Room layout changes.

---

Document text changes.

---

Computer logs change.

---

Evidence board rearranged.

---

########################################################
# HUNTING STATE
########################################################

Late-game.

---

Semester 14 actively interferes.

---

Not yet full chase.

---

Examples

Locking doors.

---

Disabling lights.

---

Blocking routes.

---

Audio hallucinations.

---

########################################################
# FINAL_CHASE STATE
########################################################

End game only.

---

Full manifestation.

---

Direct pursuit.

---

Maximum aggression.

---

########################################################
# APPEARANCE RULES
########################################################

Maximum direct appearances

Before Final Chase

3

---

Mandatory.

---

Reason

Preserve fear.

---

########################################################
# APPEARANCE SCHEDULE
########################################################

Appearance 1

Library Reflection

---

Appearance 2

Archive Corridor

---

Appearance 3

Defense Room

---

Appearance 4

Final Chase

---

########################################################
# VISUAL DESIGN RULES
########################################################

Silhouette recognizable.

---

Not oversized.

---

Human proportions.

---

Graduation robe distorted.

---

Student ID visible.

---

Face partially obscured.

---

Never fully reveal face.

---

########################################################
# MOVEMENT RULES
########################################################

No running animation

before Final Chase.

---

Entity appears where it should not be.

---

Examples

Standing still.

---

Appears behind glass.

---

Standing on desks.

---

Inside locked room.

---

########################################################
# OBSERVATION SYSTEM
########################################################

Purpose

Create paranoia.

---

Tracks:

Player Progress

Player Location

Puzzle Completion

Document Discovery

---

Not player mistakes.

---

########################################################
# REACTION SYSTEM
########################################################

Semester 14 reacts to:

Critical documents.

---

Major evidence.

---

Puzzle milestones.

---

Ritual discoveries.

---

########################################################
# DOCUMENT REACTIONS
########################################################

Example

Player discovers:

Raka Defense File

↓

Whisper Event

↓

Manifestation Chance Increased

---

########################################################
# EVIDENCE REACTIONS
########################################################

Example

Player reconstructs timeline

↓

Semester 14 anger level increases.

---

########################################################
# ANGER SYSTEM
########################################################

Range

0–100

---

Start

10

---

Critical discoveries

+10

---

Major puzzle completion

+15

---

Ritual reconstruction

+20

---

Final chapter

100

---

########################################################
# ANGER EFFECTS
########################################################

Higher anger increases:

Manifestation chance

---

Distortion frequency

---

Whispers

---

Environmental interference

---

########################################################
# ENVIRONMENT MANIPULATION
########################################################

Supported

YES

---

Examples

Door closes.

---

Lights fail.

---

Projector activates.

---

Computers reboot.

---

Cabinets open.

---

Announcements play.

---

########################################################
# SECURITY CAMERA SYSTEM
########################################################

Supported

YES

---

Player may observe:

Semester 14 on camera.

---

Turn around.

---

Nothing there.

---

Purpose

Psychological horror.

---

########################################################
# REFLECTION SYSTEM
########################################################

Supported

YES

---

Examples

Mirror.

---

Glass.

---

Computer monitor.

---

Water puddle.

---

Reflection shows entity.

---

Real world does not.

---

########################################################
# AUDIO BEHAVIOR
########################################################

Entity sounds:

Breathing.

---

Whispering.

---

Paper dragging.

---

Graduation ceremony audio.

---

Defense announcement.

---

Typing.

---

########################################################
# VOICE RULES
########################################################

Entity never speaks clearly.

---

Fragments only.

---

Examples

"...belum..."

"...sidang..."

"...jangan..."

"...ulang lagi..."

---

########################################################
# PURSUIT SYSTEM
########################################################

Used only:

Final Chapter.

---

No early-game chase.

---

Mandatory.

---

########################################################
# CHASE BEHAVIOR
########################################################

Target

Player.

---

Method

Direct pursuit.

---

No combat.

---

Contact

Triggers failure state.

---

########################################################
# CHASE PHASES
########################################################

Phase 1

Recognition

---

Phase 2

Pursuit

---

Phase 3

Reality Collapse

---

Phase 4

Final Escape

---

########################################################
# FINAL CHASE DESIGN
########################################################

Target Duration

6–10 minutes.

---

Not longer.

---

Must remain intense.

---

########################################################
# FAILURE STATE
########################################################

Player captured.

---

Screen distortion.

---

Loop restart.

---

Special dialogue.

---

Return checkpoint.

---

########################################################
# CHECKPOINT RULES
########################################################

Required.

---

No loss of major progress.

---

Maximum loss

2–3 minutes.

---

########################################################
# HORROR EVENT INTEGRATION
########################################################

Semester 14 may trigger:

Manifestations.

---

Distortions.

---

Jumpscares.

---

Echoes.

---

Audio anomalies.

---

########################################################
# SAVE SYSTEM INTEGRATION
########################################################

Must Save

Current State.

---

Anger Level.

---

Manifestation Progress.

---

Chase Progress.

---

########################################################
# PERFORMANCE REQUIREMENTS
########################################################

Entity inactive unless needed.

---

No constant AI processing.

---

Event-driven architecture.

---

Target Cost

< 0.3 ms average.

---

########################################################
# DEBUG TOOLS
########################################################

Development Only

---

Spawn Semester14

---

Force Manifestation

---

Set Anger

---

Skip Chase

---

Start Chase

---

View Current State

---

Disabled In Release.

---

########################################################
# TEST CASES
########################################################

TC-01

Observation Works

---

TC-02

Manifestation Works

---

TC-03

Reflection Events Work

---

TC-04

Camera Events Work

---

TC-05

Anger Progression Works

---

TC-06

Environment Manipulation Works

---

TC-07

Chase Trigger Works

---

TC-08

Save Restore Works

---

TC-09

Failure State Works

---

TC-10

No Event Spam

---

########################################################
# DEFINITION OF DONE
########################################################

Semester 14 can:

Observe

React

Manipulate

Manifest

Distort

Pursue

Save State

Restore State

Support Narrative

Support Endings

---

All Tests Pass.

---

No Critical Bugs.

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