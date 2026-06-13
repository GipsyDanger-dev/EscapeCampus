# 25_FINAL_CHASE_SYSTEM

# PROJECT INFORMATION

Project:
Escape Campus

Version:
1.0

Document Type:
Technical Design Document

System:
Final Chase System

Priority:
CRITICAL

Purpose:

Mendefinisikan seluruh desain teknis, alur gameplay, pacing horror, behavior Semester 14, failure state, checkpoint system, dan ending transition untuk Final Chase.

Final Chase adalah klimaks Escape Campus.

Ini bukan sekadar adegan kejar-kejaran.

Ini adalah momen ketika seluruh kebohongan, manipulasi, dan ritual loop yang selama ini tersembunyi akhirnya runtuh.

Player harus merasa:

- Terpojok
- Panik
- Tidak yakin terhadap realitas
- Terkejar waktu
- Terkejar Semester 14

Dan pada saat yang sama:

- Memahami kebenaran
- Mengingat seluruh petunjuk yang telah ditemukan
- Menentukan nasib dirinya sendiri

Dokumen ini menjadi sumber kebenaran utama implementasi Final Chase.

---

# DESIGN GOALS

Goal 01

Finale harus terasa berbeda dari seluruh game.

---

Goal 02

Memanfaatkan seluruh sistem yang telah dipelajari player.

---

Goal 03

Memberikan tekanan maksimum.

---

Goal 04

Tidak berubah menjadi game action.

---

Goal 05

Tetap mempertahankan psychological horror.

---

########################################################
# TARGET DURATION
########################################################

Minimum

6 Minutes

---

Recommended

8 Minutes

---

Maximum

10 Minutes

---

Hard Limit

12 Minutes

---

Reason

Lebih lama dari itu menyebabkan kelelahan.

---

########################################################
# TRIGGER CONDITIONS
########################################################

Final Chase hanya aktif jika:

Critical Puzzle Completed

AND

Ritual Reconstruction Complete

AND

Defense Room Accessed

---

Mandatory

YES

---

########################################################
# NARRATIVE TRIGGER
########################################################

Player memasuki ruang sidang akhir.

---

Player menemukan dokumen terakhir.

---

Player mengetahui:

Raka menciptakan loop.

---

Raka menolak kelulusan.

---

Raka menggunakan ritual untuk mencegah mahasiswa lain lulus.

---

Player membaca halaman terakhir ritual.

---

Event Trigger:

FINAL_CHASE_START

---

########################################################
# CHASE STRUCTURE
########################################################

PHASE 01

Recognition

---

PHASE 02

Reality Collapse

---

PHASE 03

Pursuit

---

PHASE 04

Defense Escape

---

PHASE 05

Graduation Hall

---

PHASE 06

Final Confrontation

---

########################################################
# PHASE 01
# RECOGNITION
########################################################

Duration

30–60 Seconds

---

Purpose

Shock.

---

Behavior

Semester 14 muncul penuh untuk pertama kalinya.

---

Tidak menyerang.

---

Hanya berdiri.

---

Player kehilangan kontrol selama 2 detik.

---

Forced Look Active.

---

Audio:

Whispers.

Graduation choir.

Distorted announcements.

---

########################################################
# PHASE 02
# REALITY COLLAPSE
########################################################

Duration

1–2 Minutes

---

Purpose

Disorientasi.

---

Effects

Hallway loops.

---

Room duplicates.

---

Doors move.

---

Signs change.

---

Documents vanish.

---

Maps become invalid.

---

Player harus tetap menuju Exit Marker.

---

Marker sengaja tidak stabil.

---

########################################################
# PHASE 03
# PURSUIT
########################################################

Duration

2–3 Minutes

---

Purpose

Pressure.

---

Semester 14 begins pursuit.

---

Speed

95% player sprint speed.

---

Rule

Player slightly faster.

---

Player can escape.

---

No combat.

---

Contact

Failure State.

---

########################################################
# PURSUIT MECHANICS
########################################################

Semester 14 can:

Break doors.

---

Open shortcuts.

---

Disable lights.

---

Trigger manifestations.

---

Cannot teleport directly in front of player.

---

########################################################
# CHASE AUDIO SYSTEM
########################################################

Layers

Heartbeat

---

Heavy Breathing

---

Footsteps

---

Whispers

---

Graduation Ceremony

---

Defense Announcements

---

Audio intensity scales dynamically.

---

########################################################
# PHASE 04
# DEFENSE ESCAPE
########################################################

Duration

1–2 Minutes

---

Purpose

Skill Test.

---

Player reaches Faculty Wing.

---

Must solve final mini-investigation.

---

Example

Identify correct defense room.

---

Wrong room

↓

Dead End.

---

Semester 14 approaches.

---

Correct room

↓

Progress.

---

########################################################
# PHASE 05
# GRADUATION HALL
########################################################

Duration

1–2 Minutes

---

Purpose

Narrative climax.

---

Player enters distorted graduation hall.

---

Graduation ceremony appears active.

---

Audience consists of:

Echo Victims.

---

Empty faces.

---

All staring at player.

---

Announcements repeat.

---

"Peserta sidang ke-14 dipersilakan maju."

---

########################################################
# PHASE 06
# FINAL CONFRONTATION
########################################################

Duration

1 Minute

---

Purpose

Ending selection.

---

Player must choose:

Destroy Ritual

or

Continue Ritual

---

Choice affects ending.

---

########################################################
# ENDING CONDITIONS
########################################################

ENDING_A

Graduate

---

Player destroys ritual.

---

Loop ends.

---

Good Ending.

---

########################################################
# ENDING_B

Escape

---

Player flees.

---

Loop survives.

---

Neutral Ending.

---

########################################################
# ENDING_C

Join Loop

---

Player repeats ritual.

---

Secret Ending.

---

########################################################
# ENDING_D

Semester 14

---

Player collected all secret evidence.

---

Reveals full truth.

---

True Ending.

---

########################################################
# FAILURE STATE
########################################################

Triggered if:

Semester 14 catches player.

---

Player falls.

---

Screen distortion.

---

Loop restart sequence.

---

Dialogue:

"Kamu belum boleh lulus."

---

Load checkpoint.

---

########################################################
# CHECKPOINT SYSTEM
########################################################

Required

YES

---

Checkpoints

CP_01

Recognition

---

CP_02

Reality Collapse

---

CP_03

Pursuit

---

CP_04

Graduation Hall

---

Maximum Progress Loss

2 Minutes

---

########################################################
# HORROR RULES
########################################################

No random jumpscares.

---

All scares narrative driven.

---

Every scare reveals information.

---

Mandatory.

---

########################################################
# FINAL JUMPSCARES
########################################################

JS_FINAL_01

Recognition Reveal

---

JS_FINAL_02

Mirror Hallway

---

JS_FINAL_03

Graduation Audience

---

JS_FINAL_04

Final Face Reveal

---

########################################################
# FINAL FACE REVEAL
########################################################

Only once.

---

Only True Ending.

---

Maximum duration

2 seconds.

---

Face partially visible.

---

Never fully reveal.

---

########################################################
# PLAYER ABILITIES
########################################################

Allowed

Run

Look

Interact

Use Evidence

---

Forbidden

Hide

Fight

Attack

---

########################################################
# UI RULES
########################################################

HUD Minimal.

---

Objectives simplified.

---

Focus on tension.

---

########################################################
# SAVE SYSTEM INTEGRATION
########################################################

Must Save

Current Phase

---

Checkpoint

---

Ending Progress

---

Choice State

---

########################################################
# PERFORMANCE REQUIREMENTS
########################################################

Stable FPS required.

---

No large loading screens.

---

Streaming preferred.

---

Target Cost

< 1 ms average logic cost.

---

########################################################
# DEBUG TOOLS
########################################################

Development Only

---

Skip To Phase

---

Spawn Semester14

---

Complete Chase

---

Force Ending

---

Teleport Checkpoint

---

Disabled In Release.

---

########################################################
# TEST CASES
########################################################

TC-01

Recognition Trigger Works

---

TC-02

Reality Collapse Works

---

TC-03

Pursuit Works

---

TC-04

Defense Puzzle Works

---

TC-05

Graduation Hall Works

---

TC-06

Ending Selection Works

---

TC-07

Checkpoint Works

---

TC-08

Failure State Works

---

TC-09

Save Restore Works

---

TC-10

No Soft Locks

---

########################################################
# ANTI-FRUSTRATION RULES
########################################################

Player always has valid route.

---

No impossible navigation.

---

No unavoidable death.

---

No hidden instant fail.

---

Mandatory.

---

########################################################
# DEFINITION OF DONE
########################################################

Final Chase can:

Start

Escalate

Pursue

Checkpoint

Save

Restore

Branch Endings

Support Horror

Support Narrative

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