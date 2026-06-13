# 06_CHARACTER_DESIGN_DOCUMENT

# PROJECT INFORMATION

Title:
Escape Campus

Version:
1.0

Document Type:
Character Design Document

Purpose:
Dokumen ini mendefinisikan seluruh karakter, visual design, animation behavior, AI behavior, audio profile, spawning rules, cutscene behavior, dan implementation rules.

Semua sistem karakter dalam game wajib mengikuti dokumen ini.

---

# CHARACTER OVERVIEW

Total Canon Characters:

7

---

Playable Character

1. Arga

---

Primary Antagonist

2. Raka Prasetya

3. Semester 14

4. Semester 14 Final Ritual Form

---

Echo Victims

5. Nadia

6. Bima

7. Sinta

8. Dimas

---

# MAIN CHARACTER

########################################################
# ARGA
########################################################

Character ID:
CHR_PLAYER_01

Name:
Arga

Age:
22

Gender:
Male

Role:
Player Character

Status:
Alive

Semester:
8

Department:
Information Technology

---

PERSONALITY

Traits:

- Rational
- Curious
- Responsible
- Persistent

Weaknesses:

- Overconfident
- Easily pressured under extreme stress

---

STORY FUNCTION

Acts as the player's perspective.

Learns the truth about:

- The Loop
- Raka
- Previous Victims

---

VISUAL DESIGN

Height:
175 cm

Body Type:
Average

Hair:
Short Black

Clothing:

- Black Hoodie
- Campus ID Card
- Jeans
- Sneakers

---

ANIMATION SET

Required Animations:

- Idle
- Walk
- Sprint
- Crouch
- Interact
- Pick Up Item
- Examine Object
- Heavy Breathing

---

VOICE PROFILE

Male

Age Range:
20–25

Characteristics:

- Natural
- Nervous
- Realistic

---

DIALOGUE STYLE

Never heroic.

Never acts like an action protagonist.

Examples:

"What the hell was that?"

"Okay... okay... think."

"No... this isn't real."

---

########################################################
# RAKA PRASETYA
########################################################

Character ID:
CHR_RAKA_01

Name:
Raka Prasetya

Alias:
Semester 14

Age:
24

Role:
Main Antagonist

Status:
Dead

---

BACKGROUND

Former outstanding student.

Achievements:

- Academic scholarship
- Organization leader
- High GPA

---

DOWNFALL

Repeated thesis failures.

Isolation.

Depression.

Obsession.

Ritual involvement.

Transformation into loop entity.

---

PERSONALITY

Before Ritual:

- Intelligent
- Helpful
- Ambitious

After Ritual:

- Bitter
- Lonely
- Broken

---

IMPORTANT RULE

Raka is NOT evil.

Raka is tragic.

Player must understand his pain.

---

VOICE PROFILE

Male

Soft

Exhausted

Emotionally unstable

---

DIALOGUE EXAMPLES

"I was supposed to graduate."

"I did everything right."

"Why them?"

"Why not me?"

---

########################################################
# SEMESTER 14
########################################################

Character ID:
CHR_MONSTER_01

Type:
Manifestation

Role:
Primary Horror Entity

---

DESCRIPTION

Physical manifestation of Raka's obsession.

Appears throughout the game.

---

VISUAL DESIGN

Height:
210 cm

Body Type:
Thin

Extremely elongated limbs.

---

HEAD

Partially covered by thesis papers.

Student ID fused into skin.

---

BODY

Old campus jacket.

Academic documents embedded into flesh.

Pages constantly moving.

---

MOVEMENT STYLE

Unnatural.

Jerky.

Silent.

Sudden stops.

---

HORROR RULE

Must rarely appear.

Quality over quantity.

---

SPAWN RULE

Can appear:

- End of hallway
- Library shelves
- Behind glass
- CCTV screens

Cannot appear randomly.

Every appearance must have narrative purpose.

---

JUMPSCARE RULE

Maximum Major Jumpscares:

6

Must be scripted.

Never procedural.

---

BEHAVIOR STATES

State 1

Observe

Distance:
Far

Purpose:
Psychological pressure

---

State 2

Manifest

Purpose:
Story progression

---

State 3

Jumpscare

Purpose:
Shock event

---

State 4

Chase

Purpose:
Final gameplay sequence

---

########################################################
# SEMESTER 14 FINAL RITUAL FORM
########################################################

Character ID:
CHR_BOSS_01

Type:
Final Form

---

DESCRIPTION

Final transformation.

Appears after ritual source is discovered.

---

VISUAL DESIGN

Height:
250 cm

---

BODY

Made of:

- Thesis papers
- Student IDs
- Graduation photos
- Ritual symbols

---

HEAD

Multiple student faces appear briefly.

Faces fade in and out.

---

MOVEMENT

Aggressive.

Fast.

Violent.

---

APPEARANCE RULE

Only appears:

- Final Server Room Sequence
- Final Chase

Nowhere else.

---

########################################################
# ECHO VICTIM 01
########################################################

Name:
Nadia

Alias:
The Endless Revision

Character ID:
ECHO_01

---

VISUAL

Female

Laptop

Backpack

Dark circles under eyes

---

BEHAVIOR

Always typing.

Never finishes.

---

DIALOGUE

"Sebentar lagi selesai."

"Aku tinggal revisi sedikit lagi."

---

SPAWN LOCATIONS

- Library
- Computer Lab

---

STORY PURPOSE

Represents burnout.

---

########################################################
# ECHO VICTIM 02
########################################################

Name:
Bima

Alias:
The Waiting Student

Character ID:
ECHO_02

---

VISUAL

Male

Holding thesis folder

Sitting outside office

---

BEHAVIOR

Waiting forever.

---

DIALOGUE

"Dosen bilang lima menit."

"Harusnya sebentar lagi."

---

SPAWN LOCATIONS

- Lecturer Office
- Hallway

---

STORY PURPOSE

Represents endless delay.

---

########################################################
# ECHO VICTIM 03
########################################################

Name:
Sinta

Alias:
The Missing Presenter

Character ID:
ECHO_03

---

VISUAL

Female

Presentation remote

Formal clothing

---

BEHAVIOR

Repeating same slide.

---

DIALOGUE

"Slide berikutnya."

"Slide berikutnya."

---

SPAWN LOCATIONS

- Classroom
- Seminar Room

---

STORY PURPOSE

Represents fear of public failure.

---

########################################################
# ECHO VICTIM 04
########################################################

Name:
Dimas

Alias:
The Graduate

Character ID:
ECHO_04

---

VISUAL

Graduation gown

Always facing away

---

BEHAVIOR

Appears successful.

Actually trapped.

---

DIALOGUE

"Aku sudah lulus."

"Kenapa aku masih di sini?"

---

SPAWN LOCATIONS

- Lobby
- Graduation Board Area

---

STORY PURPOSE

Represents false hope.

---

# ECHO ENTITY RULES

Echoes:

Cannot attack.

Cannot chase.

Cannot kill player.

Cannot trigger game over.

---

Can:

Appear suddenly.

Disappear suddenly.

Appear in reflections.

Appear in CCTV.

Deliver lore.

Guide player.

Mislead player.

---

# ANIMATION REQUIREMENTS

Semester 14

Required:

- Idle
- Observe
- Turn Head
- Slow Walk
- Fast Walk
- Chase
- Jumpscare
- Spawn
- Despawn

---

Echo Victims

Required:

- Idle
- Loop Animation
- Fade Out
- Fade In

---

# AUDIO DESIGN

Semester 14

Audio Layers:

- Whisper
- Breathing
- Paper Movement

---

Final Form

Audio Layers:

- Distorted Voices
- Heavy Bass
- Academic Announcements

---

Echo Victims

Audio:

Subtle.

Human.

Sad.

---

# IMPLEMENTATION RULES

AI Agent must:

1. Never add new monsters.

2. Never replace Semester 14.

3. Never allow Echo Victims to attack.

4. Never create combat.

5. Maintain narrative consistency.

6. Keep Raka tragic.

7. Keep horror psychological first.

8. Use jumpscares sparingly.

---

# END OF DOCUMENT