# 12_AUDIO_AND_SOUND_DESIGN_DOCUMENT

# PROJECT INFORMATION

Title:
Escape Campus

Version:
1.0

Document Type:
Audio & Sound Design Document

Priority:
HIGH

Purpose:

Mendefinisikan seluruh sistem audio, ambience, sound effects, dialogue audio, horror audio, dynamic music, chase music, audio trigger system, mixer architecture, dan audio implementation rules.

Dokumen ini menjadi sumber kebenaran utama seluruh audio dalam game.

Digunakan oleh:

- Audio Designer
- Sound Designer
- Composer
- Technical Audio Designer
- Gameplay Programmer
- AI Agent

---

# AUDIO PHILOSOPHY

Audio adalah sumber ketakutan utama.

Visual hanya mengkonfirmasi ketakutan.

Audio menciptakan ketakutan.

---

Target Perasaan Pemain

0–15 Menit

"Ada sesuatu yang aneh."

---

15–30 Menit

"Ada seseorang di sini."

---

30–50 Menit

"Aku sedang diawasi."

---

50–70 Menit

"Aku harus pergi."

---

70–90 Menit

"Lari."

---

# AUDIO PILLARS

PILLAR 01

Silence Is A Weapon

---

Jangan takut menggunakan keheningan.

---

PILLAR 02

Environmental Audio

---

Kampus harus terdengar hidup.

Meskipun kosong.

---

PILLAR 03

Psychological Audio

---

Suara harus membuat pemain meragukan apa yang mereka dengar.

---

PILLAR 04

Academic Horror

---

Sumber horror utama berasal dari elemen kampus.

Bukan suara monster.

---

PILLAR 05

Escalation

---

Audio semakin agresif seiring progress.

---

########################################################
# AUDIO MIXER STRUCTURE
########################################################

Master

├── Music
├── Ambience
├── SFX
├── Dialogue
├── Horror
└── UI

---

# DEFAULT MIX LEVELS

Master

0 dB

---

Music

-10 dB

---

Ambience

-5 dB

---

Dialogue

0 dB

---

Horror

-3 dB

---

UI

-8 dB

---

########################################################
# GAME PHASE AUDIO
########################################################

PHASE 1

Normal Campus

0–15 Minutes

---

Music

Minimal

---

Ambience

Dominant

---

PHASE 2

Investigation

15–30 Minutes

---

Music

Rare

---

Ambience

Strong

---

PHASE 3

Paranoia

30–50 Minutes

---

Music

Subtle Drones

---

PHASE 4

Horror

50–70 Minutes

---

Music

Distorted Layers

---

PHASE 5

Chase

70–90 Minutes

---

Music

Aggressive

---

########################################################
# ROOM AMBIENCE DESIGN
########################################################

COMPUTER LAB

---

Loop Sounds

Computer Fan

Electrical Buzz

Monitor Hum

Fluorescent Light Buzz

Air Conditioning

---

Random Events

Keyboard Typing

Mouse Click

System Error Beep

Chair Movement

---

LIBRARY

---

Loop Sounds

Air Vent

Page Turning

Shelf Creak

Building Noise

---

Random Events

Book Drop

Distant Page Flip

Footstep Echo

Whisper Layer

---

CLASSROOM

---

Loop Sounds

Fan Rotation

Outdoor Wind

Chair Creak

---

Random Events

Marker Drop

Desk Tap

Notebook Flip

---

LECTURER OFFICE

---

Loop Sounds

Clock Tick

Computer Fan

Paper Rustle

---

Random Events

Drawer Open

Keyboard Typing

Document Shuffle

---

ARCHIVE ROOM

---

Loop Sounds

Air Vent

Metal Cabinet Resonance

Paper Movement

---

Random Events

Cabinet Knock

Folder Slide

Metal Click

---

SERVER ROOM

---

Loop Sounds

Heavy Server Hum

Electrical Buzz

Cooling Fans

---

Random Events

Voltage Spike

Power Flicker

Server Alarm

---

########################################################
# PSYCHOLOGICAL HORROR AUDIO
########################################################

WHISPER SYSTEM

Purpose:

Create paranoia.

---

Player must never clearly understand words.

---

Allowed:

Breathing

Murmurs

Half Sentences

---

Forbidden:

Long Monologues

---

Example

Correct

"...ra..."

"...ga..."

"...tinggal..."

---

Incorrect

"Arga, aku sedang mengawasimu."

---

########################################################
# ACADEMIC HORROR AUDIO
########################################################

Allowed Horror Sources

---

Printer

---

Keyboard

---

Projector

---

School Bell

---

Announcement System

---

Paper

---

Student Voices

---

Graduation Applause

---

Defense Presentation Audio

---

Forbidden Sources

Chains

Demonic Chants

Hell Sounds

Monster Roars

Fantasy Sounds

---

########################################################
# SEMESTER 14 AUDIO PROFILE
########################################################

Normal Form

---

Breathing

Paper Movement

Distant Footsteps

Cloth Rustle

---

Voice

Male

Age 22–25

Exhausted

Sleep Deprived

Broken

---

Tone

Sad

Not Evil

---

Not Angry

---

FINAL FORM AUDIO PROFILE

---

Layers

Male Voice

Female Voice

Student Voices

Graduation Audio

Paper Noise

---

All mixed together.

---

Purpose

Mental Collapse

---

Not Demon Possession

---

########################################################
# ECHO VICTIM AUDIO
########################################################

NADIA

---

Typing

Mouse Clicks

Sobbing

---

BIMA

---

Foot Tapping

Chair Movement

Deep Breathing

---

SINTA

---

Presentation Clicker

Page Turning

---

DIMAS

---

Graduation Applause

Camera Shutter

---

########################################################
# DYNAMIC MUSIC SYSTEM
########################################################

STATE 01

Exploration

---

Music:

None

---

Ambience Only

---

STATE 02

Investigation

---

Music:

Very Soft Drone

---

STATE 03

Danger

---

Music:

Tension Layer

---

STATE 04

Reveal

---

Music:

Low Strings

---

STATE 05

Final Confrontation

---

Music:

Emotional Horror

---

STATE 06

Chase

---

Music:

High Intensity

---

STATE 07

Ending

---

Music:

Soft Piano

---

########################################################
# JUMPSCARE AUDIO RULES
########################################################

CRITICAL

---

Do NOT use:

Hollywood Horror Hit

---

Do NOT use:

Overused Horror Sting

---

Do NOT use:

Scream.wav

---

Preferred

Environmental Shock

---

Examples

Light Explosion

Metal Slam

Electrical Failure

Book Impact

Glass Crack

---

Reason

Feels more believable.

---

########################################################
# CHASE AUDIO SYSTEM
########################################################

Layer 1

Heartbeat

---

Layer 2

Heavy Breathing

---

Layer 3

Music

---

Layer 4

Semester 14 Voice

---

Layer 5

Environmental Collapse

---

Dynamic Volume

Depends on distance.

---

Closer Enemy

Higher Volume

---

Far Enemy

Lower Volume

---

########################################################
# FOOTSTEP SYSTEM
########################################################

Required Surfaces

Concrete

Tile

Wood

Metal

Paper

---

Footstep variation

Minimum:

5 Variants Per Surface

---

Avoid repetition.

---

########################################################
# DOCUMENT AUDIO
########################################################

Open Document

Paper Open

---

Close Document

Paper Close

---

Scroll

Paper Move

---

Zoom

Soft Rustle

---

########################################################
# UI AUDIO
########################################################

Button Hover

Soft Tick

---

Button Click

Soft Confirm

---

Pause Menu

Folder Open

---

Document Viewer

Paper Movement

---

UI sounds must remain subtle.

---

########################################################
# SUBTITLE AUDIO RULES
########################################################

Every dialogue:

Must support subtitles.

---

Every whisper:

Optional subtitle.

---

Hidden whispers:

No subtitle.

---

Purpose:

Maintain mystery.

---

########################################################
# AUDIO PERFORMANCE RULES
########################################################

Target Format

WAV

For Important Sounds

---

OGG

For Ambience

---

Maximum Audio Length

Ambient Loops:

3–10 Minutes

---

Voice Clips:

Under 20 Seconds

---

Memory Budget

Under 500 MB

---

########################################################
# AUDIO IMPLEMENTATION RULES
########################################################

Use Audio Pooling

Required

---

No Runtime Audio Loading Spam

---

No Hundreds Of Active Audio Sources

---

Recommended

Max Simultaneous Audio Sources

32

---

Hard Limit

64

---

########################################################
# AI AGENT AUDIO ACQUISITION RULES
########################################################

Search Priority

1. Freesound
2. Pixabay Audio
3. Sonniss GDC Packs
4. OpenGameArt

---

Search Keywords

empty hallway ambience

university ambience

library ambience

computer room ambience

paper rustle

server room hum

fluorescent buzz

distant whisper

footsteps concrete

graduation applause

---

Every imported audio asset must be:

Licensed

Documented

Optimized

Registered

---

########################################################
# AUDIO QUALITY CHECKLIST
########################################################

Before approval:

✓ No clipping

✓ No distortion

✓ Correct volume

✓ Correct loop points

✓ Correct mixer routing

✓ Subtitle compatibility

✓ Performance tested

✓ Documentation updated

---

########################################################
# DEFINITION OF GOOD AUDIO
########################################################

Player hears something.

Player becomes uncomfortable.

Player investigates.

Player finds nothing.

Player becomes more afraid.

---

This is successful horror audio.

---

# END OF DOCUMENT