# 11_UI_UX_DOCUMENT

# PROJECT INFORMATION

Title:
Escape Campus

Version:
1.0

Document Type:
UI / UX Design Document

Purpose:
Mendefinisikan seluruh User Interface (UI), User Experience (UX), HUD, Menu System, Document Viewer, Accessibility, Navigation Flow, Interaction Feedback, dan UX Rules.

Dokumen ini menjadi sumber kebenaran utama seluruh desain antarmuka pengguna.

Digunakan oleh:

- UI Designer
- UX Designer
- Gameplay Programmer
- Technical Designer
- AI Agent

---

# UI PHILOSOPHY

Escape Campus adalah game horror investigasi.

UI harus:

- Minimalis
- Tidak mengganggu immersion
- Mudah dipahami
- Cepat digunakan

---

Player harus merasa:

"Aku sedang menjelajahi kampus."

Bukan:

"Aku sedang membuka game menu."

---

# UI DESIGN PILLARS

PILLAR 01

Invisible Interface

UI seminimal mungkin.

---

PILLAR 02

Fast Access

Informasi penting mudah ditemukan.

---

PILLAR 03

Document Driven

Dokumen adalah fitur utama.

---

PILLAR 04

Academic Identity

UI terasa seperti sistem akademik kampus.

---

# COLOR SYSTEM

Primary Background

#FFFFFF

---

Secondary Background

#F2F2F2

---

Text Primary

#1F1F1F

---

Text Secondary

#666666

---

Accent Color

#E3A008

---

Danger

#B91C1C

---

Success

#15803D

---

# TYPOGRAPHY

Primary Font

Inter

---

Fallback

Roboto

---

Title

24–32 px

---

Subtitle

18–22 px

---

Body

14–18 px

---

# MAIN MENU

########################################################

SCREEN

Main Menu

---

Background

Campus at Night

Static Camera

Slow Wind Effect

---

Music

Low Volume Ambience

---

Buttons

Continue

New Game

Settings

Credits

Quit

---

Layout

Centered Vertical

---

No Jump Scares

No Monster Appearances

No Loud Audio

---

########################################################

NEW GAME FLOW

New Game

↓

Difficulty Notice

↓

Start Intro Cutscene

↓

Gameplay

---

# PAUSE MENU

Layout

Simple Overlay

---

Options

Resume

Documents

Settings

Main Menu

Quit Desktop

---

Background

Blurred Gameplay

---

# SETTINGS MENU

Tabs

Graphics

Audio

Gameplay

Accessibility

---

########################################################
# GRAPHICS
########################################################

Resolution

Fullscreen

VSync

FPS Limit

Quality Preset

Brightness

---

########################################################
# AUDIO
########################################################

Master

Music

SFX

Voice

Ambience

---

########################################################
# GAMEPLAY
########################################################

Mouse Sensitivity

Invert Y

FOV

---

########################################################
# ACCESSIBILITY
########################################################

Subtitle Size

Subtitle Background

Jumpscare Warning

Motion Reduction

Colorblind Support

---

# HUD DESIGN

PHILOSOPHY

No clutter.

---

Visible Elements

Interaction Prompt

Objective Prompt

Subtitles

---

Hidden By Default

Inventory

Document List

Objectives

---

# INTERACTION PROMPT

Display:

[E]

Interact

---

Distance

2.5 Meters

---

Fade In

0.2 Seconds

---

Fade Out

0.2 Seconds

---

# OBJECTIVE DISPLAY

Top Left

---

Example

Find Raka's Thesis

---

Only one active objective visible.

---

No objective spam.

---

# SUBTITLE SYSTEM

Position

Bottom Center

---

Background

Semi Transparent

---

Speaker Name

Visible

---

Example

RAKA

"You found everything."

---

# DOCUMENT SYSTEM

########################################################

MOST IMPORTANT UI

########################################################

Purpose

Core investigation mechanic.

---

Opening Animation

Student Folder Opens

---

Sound

Paper Turn

---

Layout

Left Side:

Document List

---

Right Side:

Document Content

---

Document Categories

Academic Records

Emails

Thesis Files

Research Notes

Ritual Documents

---

Player Can:

Read

Scroll

Zoom

Close

---

Player Cannot:

Edit

Delete

Modify

---

# EVIDENCE BOARD

Optional Feature

---

Purpose

Review important clues.

---

Sections

People

Locations

Documents

Events

---

Automatically updates.

---

# INVENTORY SCREEN

Design

Academic Folder

---

Tabs

Keys

Evidence

Archives

Special Items

---

No item grid.

---

No RPG inventory.

---

# SAVE SYSTEM UI

Auto Save Only

---

Notification

Top Right

---

Display

Saving...

---

Duration

2 Seconds

---

No Manual Save

---

# CHECKPOINT UI

Display

Checkpoint Reached

---

Duration

2 Seconds

---

No interruption.

---

# JUMPSCARE UX RULES

Never:

Lock player controls unfairly.

---

Never:

Chain multiple jumpscares.

---

Minimum Gap

5 Minutes

Between major scares.

---

# HORROR FEEDBACK

Player should know:

Something happened.

---

Player should not know:

What happened.

---

Example

Correct:

Light flickers.

---

Wrong:

Popup explains event.

---

# DEATH SCREEN

Title

You Remained In The Loop

---

Options

Retry Checkpoint

Main Menu

---

Background

Distorted Campus

---

No Score

No Statistics

---

No Rank System

---

# ENDING SCREEN

Good Ending

Display

The Loop Has Ended

---

Secret Ending

Display

He Finally Moved Forward

---

# CREDITS SCREEN

Background

Morning Campus

---

Music

Soft Piano

---

Include

Development Team

Assets

Music

Special Thanks

---

# ACCESSIBILITY REQUIREMENTS

Required

Subtitles

Volume Controls

Brightness Controls

---

Recommended

Colorblind Mode

Jumpscare Warning

---

# JUMPSCARE WARNING MODE

Optional

Default:
Off

---

If Enabled

Before major jumpscare:

Small icon appears briefly.

---

Purpose

Accessibility

Not difficulty reduction.

---

# UX FLOW

Main Menu

↓

New Game

↓

Intro Cutscene

↓

Gameplay

↓

Puzzle

↓

Investigation

↓

Reveal

↓

Final Chase

↓

Ending

↓

Credits

---

# AI AGENT UI RULES

AI Agent must:

- Keep UI minimal
- Preserve immersion
- Prioritize readability
- Prioritize document readability

---

AI Agent must never:

- Add minimap
- Add quest tracker
- Add health bar
- Add stamina bar
- Add XP bar
- Add level system
- Add RPG UI

---

# IMMERSION RULE

Player should feel:

"I am investigating."

Not:

"I am managing menus."

---

# DEFINITION OF GOOD UI

Player forgets the UI exists.

Player remembers the story.

---

# END OF DOCUMENT