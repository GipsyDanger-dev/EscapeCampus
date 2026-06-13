# 04_LEVEL_DESIGN_DOCUMENT

# PROJECT INFORMATION

Title:
Escape Campus

Version:
1.0

Purpose:
Dokumen ini mendefinisikan seluruh struktur level, layout ruangan, progression area, lokasi puzzle, lokasi horror event, lokasi jumpscare, dan jalur chase.

Dokumen ini menjadi referensi utama bagi:

* Level Designer
* Environment Artist
* Gameplay Programmer
* AI Agent

---

# 1. LEVEL DESIGN PHILOSOPHY

Level harus:

* Mudah dinavigasi
* Mendukung storytelling
* Mendukung horror
* Mendukung puzzle
* Tidak membuat pemain tersesat terlalu lama

Target waktu tersesat maksimum:

2 menit

Jika pemain tersesat lebih dari itu berarti level gagal.

---

# 2. BUILDING OVERVIEW

Nama:

Gedung Fakultas Teknologi Informasi

Jumlah Lantai:

4

Total Area Explorable:

8 Area Utama

---

# 3. FLOOR STRUCTURE

FLOOR 1

* Main Lobby
* Security Room
* Main Exit

---

FLOOR 2

* Computer Laboratory
* Classroom A
* Classroom B
* Toilet

---

FLOOR 3

* Library
* Lecturer Office

---

FLOOR 4

* Server Room
* Old Archive Room

---

# 4. PLAYER PROGRESSION FLOW

START

↓

Computer Laboratory

↓

Classroom A

↓

Classroom B

↓

Library

↓

Lecturer Office

↓

Archive Room

↓

Server Room

↓

Chase Sequence

↓

Main Lobby

↓

Exit Gate

↓

Ending

---

# 5. FLOOR 1

======================
MAIN LOBBY
==========

Purpose:

* Hub Area
* Final Escape Area

---

Player Access:

Beginning:
Locked

Ending:
Accessible

---

Visual Design:

* Modern university lobby
* Reception desk
* Information board
* Vending machine
* Student lounge

---

Atmosphere:

Beginning:
Normal

Ending:
Distorted

---

Events:

No major horror events during beginning.

Major horror events occur during chase.

---

Jumpscare:

None

---

Collectibles:

Photo Frame 01

Campus Brochure

---

======================
SECURITY ROOM
=============

Purpose:

Story Area

---

Contains:

* CCTV monitor
* Security logs
* Campus map

---

Lore:

Security reports mentioning strange sightings.

---

Puzzle:

CCTV Observation Puzzle

Reward:

Library Access Hint

---

# 6. FLOOR 2

======================
COMPUTER LABORATORY
===================

Purpose:

Starting Area

---

Size:

Medium

---

Contains:

* 30 computers
* Printer station
* Lecturer desk
* Whiteboard

---

Story Event:

Player enters to print revision.

Power outage occurs.

Loop begins.

---

Jumpscare:

JS-001

Computer Monitor Face

---

Puzzle:

PZ-001

Computer Password

Reward:

Laboratory Exit Key

---

Collectibles:

Student Note 01

Student Note 02

---

======================
CLASSROOM A
===========

Purpose:

Exploration Area

---

Contains:

* Projector
* Student desks
* Notice board

---

Puzzle:

PZ-002

Class Schedule Puzzle

---

Reward:

Library Code

---

Horror Event:

Footsteps Heard Behind Player

---

======================
CLASSROOM B
===========

Purpose:

Lore Area

---

Contains:

* Graduation photos
* Student records

---

Reveal:

First mention of Raka.

---

Horror Event:

Shadow Crossing Window

---

Collectibles:

Photo 03

Photo 04

Old Student Record

---

======================
TOILET
======

Purpose:

Psychological Horror Area

---

Contains:

* Mirrors
* Stalls
* Broken lights

---

Jumpscare:

JS-003

Bathroom Stall Event

---

No puzzle.

---

# 7. FLOOR 3

======================
LIBRARY
=======

Purpose:

Major Puzzle Area

---

Contains:

* Bookshelves
* Reading desks
* Archive section

---

Puzzle:

PZ-003

Library Archive Puzzle

---

Reward:

Lecturer Office Access Card

---

Jumpscare:

JS-002

Bookshelf Collapse

---

Horror Events:

Whispering

Moving Books

---

Collectibles:

Diary Page 01

Diary Page 02

Diary Page 03

---

======================
LECTURER OFFICE
===============

Purpose:

Story Reveal Area

---

Contains:

* Lecturer desks
* Filing cabinets
* Computers

---

Puzzle:

PZ-004

Academic Record Puzzle

---

Reward:

Archive Room Key

---

Major Story Reveal:

Raka identified as Semester 14.

---

Jumpscare:

JS-004

Document Grab Event

---

Collectibles:

Email Printouts

Failed Thesis Review

Warning Letters

---

# 8. FLOOR 4

======================
ARCHIVE ROOM
============

Purpose:

Ritual Investigation

---

Contains:

* Old cabinets
* Archived student records
* Ritual symbols

---

Puzzle:

PZ-005

Archive Combination Puzzle

---

Reward:

Server Room Access Card

---

Horror Events:

Flickering Lights

Moving Cabinets

---

Collectibles:

Cult Document

Ritual Journal

---

======================
SERVER ROOM
===========

Purpose:

Final Investigation Area

---

Contains:

* Server racks
* Main control panel
* Ritual altar

---

Puzzle:

PZ-006

Server Activation Puzzle

---

Reward:

Ritual Document

---

Major Story Reveal:

Truth behind loop.

---

Jumpscare:

JS-006

Semester 14 Full Appearance

---

Event:

Trigger Chase Sequence

---

# 9. CHASE LEVEL DESIGN

======================
CHASE START
===========

Location:

Server Room

---

Trigger:

Player obtains Ritual Document

---

Cutscene:

Semester 14 appears.

Dialogue begins.

---

======================
CHASE PATH 1
============

Server Room

↓

Archive Room

↓

Emergency Stair

---

Hazards:

Falling shelves

Blocked corridors

---

======================
CHASE PATH 2
============

Emergency Stair

↓

Library

↓

Floor 2 Corridor

---

Hazards:

Locked doors

Quick Time Events

---

======================
CHASE PATH 3
============

Floor 2

↓

Lobby

↓

Exit Gate

---

Hazards:

Environmental collapse

Visual distortion

---

======================
CHASE END
=========

Location:

Main Exit

---

Trigger:

Player reaches gate.

---

Final Cutscene begins.

---

# 10. COLLECTIBLE DISTRIBUTION

Total Collectibles:

18

---

Story Documents:

10

---

Photos:

4

---

Ritual Notes:

4

---

Purpose:

Unlock Secret Ending.

---

# 11. ENVIRONMENTAL STORYTELLING RULES

Every room must contain:

* At least 1 visual clue
* At least 1 story clue
* At least 1 atmosphere element

---

Examples:

Computer Lab:
Open thesis file

Library:
Missing books

Office:
Rejected thesis documents

Server Room:
Ritual symbols

---

# 12. LEVEL DESIGN RESTRICTIONS

Do Not Create:

* Maze Layouts
* Randomized Rooms
* Procedural Levels
* Large Open Areas

Reason:

Player should feel lost emotionally, not geographically.

---

# END OF LEVEL DESIGN DOCUMENT
