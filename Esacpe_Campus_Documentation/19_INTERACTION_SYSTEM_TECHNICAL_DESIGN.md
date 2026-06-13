# 19_INTERACTION_SYSTEM_TECHNICAL_DESIGN

# PROJECT INFORMATION

Project:
Escape Campus

Version:
1.0

Document Type:
Technical Design Document

System:
Interaction System

Priority:
CRITICAL

Purpose:

Mendefinisikan seluruh arsitektur sistem interaksi Escape Campus.

Sistem ini merupakan fondasi utama gameplay.

Seluruh objek yang dapat digunakan pemain harus menggunakan sistem ini.

Termasuk:

- Documents
- Doors
- Computers
- Puzzle Objects
- Evidence Objects
- Cabinets
- Switches
- Key Items
- Ending Triggers

Dokumen ini menjadi sumber kebenaran utama untuk seluruh implementasi interaction.

---

# SYSTEM OVERVIEW

Interaction System bertanggung jawab untuk:

- Target Detection
- Interaction Validation
- Prompt Display
- Interaction Execution
- Interaction Restrictions
- Interaction Events

---

Interaction System tidak bertanggung jawab untuk:

- UI Logic
- Save Logic
- Puzzle Logic
- Inventory Logic

---

########################################################
# DESIGN GOALS
########################################################

Goal 01

Simple

---

Goal 02

Reliable

---

Goal 03

Reusable

---

Goal 04

Extendable

---

Goal 05

Low Performance Cost

---

########################################################
# CORE ARCHITECTURE
########################################################

PlayerInteraction

↓

InteractionDetector

↓

IInteractable

↓

Target Object

---

Only one active interaction target allowed.

---

Never interact with multiple objects simultaneously.

---

########################################################
# PLAYER INTERACTION FLOW
########################################################

Step 01

Player looks at object

↓

Step 02

Raycast hits object

↓

Step 03

Check IInteractable

↓

Step 04

Validate interaction

↓

Step 05

Display prompt

↓

Step 06

Player presses E

↓

Step 07

Execute interaction

↓

Step 08

Raise event

---

########################################################
# SCRIPT STRUCTURE
########################################################

Gameplay/

Interaction/

InteractionManager.cs

InteractionDetector.cs

InteractionPromptUI.cs

InteractionEventChannel.cs

IInteractable.cs

InteractionData.cs

---

########################################################
# INTERACTION DETECTOR
########################################################

Purpose

Detect interactable objects.

---

Method

Raycast

---

Origin

InteractionOrigin

---

Distance

2.5 meters

---

Layer

Interactable

---

Update Frequency

Every Frame

---

Maximum Hits

1

---

Reason

Reduce ambiguity.

---

########################################################
# INTERACTION PRIORITY
########################################################

If multiple colliders overlap:

Priority Order

1. Puzzle Object

2. Document

3. Key Item

4. Computer

5. Cabinet

6. Door

7. Misc Object

---

Must always choose highest priority.

---

########################################################
# INTERACTABLE INTERFACE
########################################################

Every interactable object must implement:

IInteractable

---

Required Methods

bool CanInteract()

string GetPrompt()

void Interact()

---

Optional Methods

OnFocus()

OnLoseFocus()

---

########################################################
# EXAMPLE INTERFACE
########################################################

```csharp
public interface IInteractable
{
    bool CanInteract();

    string GetPrompt();

    void Interact();

    void OnFocus();

    void OnLoseFocus();
}
```

---

########################################################
# INTERACTION STATES
########################################################

STATE_NONE

No target.

---

STATE_FOCUS

Looking at object.

---

STATE_AVAILABLE

Can interact.

---

STATE_BLOCKED

Cannot interact.

---

STATE_INTERACTING

Interaction running.

---

########################################################
# INTERACTION PROMPT SYSTEM
########################################################

Display Location

Center Screen

---

Format

[E] Interact

---

Examples

[E] Read Document

[E] Open Cabinet

[E] Access Terminal

[E] Collect Evidence

---

Prompt updates dynamically.

---

########################################################
# FOCUS SYSTEM
########################################################

When player looks at object:

OnFocus()

called once.

---

When player looks away:

OnLoseFocus()

called once.

---

Purpose

Highlighting

Audio feedback

Visual feedback

---

########################################################
# OBJECT HIGHLIGHT RULES
########################################################

Allowed

Outline

Subtle Glow

Material Swap

---

Forbidden

Bright Neon Effects

Cartoon Highlights

---

Intensity

Very Low

---

Must preserve immersion.

---

########################################################
# INTERACTION LOCKS
########################################################

Interaction denied if:

Cutscene Active

---

Dialogue Active

---

Jumpscare Active

---

Player Stunned

---

Game Paused

---

########################################################
# DOOR INTERACTION
########################################################

Prompt

[E] Open Door

---

Interaction

Play Animation

Play Audio

Raise Event

---

Optional

Locked State

---

########################################################
# DOCUMENT INTERACTION
########################################################

Prompt

[E] Read Document

---

Interaction

Open Document Viewer

---

Pause Movement

YES

---

Pause Camera

NO

---

########################################################
# COMPUTER INTERACTION
########################################################

Prompt

[E] Access Terminal

---

Interaction

Open Computer UI

---

Lock Movement

YES

---

Lock Interaction

YES

---

########################################################
# PUZZLE INTERACTION
########################################################

Prompt

[E] Examine

---

Interaction

Launch Puzzle Module

---

Puzzle controls interaction state.

---

########################################################
# ITEM PICKUP INTERACTION
########################################################

Prompt

[E] Collect

---

Interaction

Add to Inventory

Destroy World Object

Play Audio

Raise Event

---

########################################################
# EVIDENCE INTERACTION
########################################################

Prompt

[E] Collect Evidence

---

Interaction

Evidence Database Update

Inventory Update

Optional Narrative Trigger

---

########################################################
# TERMINAL INTERACTION
########################################################

Prompt

[E] Use Terminal

---

Supports

Password Entry

File Reading

Log Review

Puzzle Integration

---

########################################################
# INTERACTION EVENT SYSTEM
########################################################

Purpose

Loose coupling.

---

Interaction must raise events.

---

Examples

DocumentRead

PuzzleStarted

PuzzleCompleted

DoorOpened

EvidenceCollected

EndingTriggered

---

########################################################
# EVENT ARCHITECTURE
########################################################

Interaction

↓

Event Channel

↓

Subscriber Systems

---

Never hard reference unrelated systems.

---

########################################################
# AUDIO FEEDBACK
########################################################

Focus Audio

Optional

---

Interact Audio

Required

---

Examples

Paper Rustle

Button Click

Door Handle

Cabinet Slide

---

########################################################
# HORROR INTEGRATION
########################################################

Interaction may trigger:

Whispers

Light Flickers

Distortions

Echo Events

Jumpscares

---

Examples

Read Ritual Document

↓

Trigger Whisper

---

Open Cabinet

↓

Trigger Manifestation

---

########################################################
# INTERACTION COOLDOWN
########################################################

Default

0.2 Seconds

---

Purpose

Prevent spam.

---

########################################################
# SAVE SYSTEM INTEGRATION
########################################################

Must Save

Collected Documents

---

Collected Evidence

---

Opened Doors

---

Solved Puzzles

---

Activated Events

---

########################################################
# MULTI-STAGE INTERACTION
########################################################

Supported

YES

---

Example

Locked Cabinet

↓

Need Key

↓

Unlock

↓

Open

↓

Collect Document

---

Each stage tracked independently.

---

########################################################
# FAIL-SAFE RULES
########################################################

If object missing component

Ignore safely.

---

If prompt missing

Use:

[E] Interact

---

If interaction fails

Log Warning

Do not crash.

---

########################################################
# DEBUG TOOLS
########################################################

Development Build Only

---

Show Current Target

---

Show Interaction Distance

---

Show Prompt Source

---

Show Event Trigger

---

Disabled in Release.

---

########################################################
# PERFORMANCE REQUIREMENTS
########################################################

Target Cost

< 0.1 ms

---

One Raycast Per Frame

Maximum

---

No Physics Overlap Spam

---

No FindObjectOfType Runtime

---

Cache references.

Mandatory.

---

########################################################
# TEST CASES
########################################################

TC-01

Document Read Works

---

TC-02

Door Open Works

---

TC-03

Puzzle Launch Works

---

TC-04

Pickup Works

---

TC-05

Focus Events Work

---

TC-06

Interaction Lock Works

---

TC-07

Cooldown Works

---

TC-08

Save Restore Works

---

TC-09

Event Trigger Works

---

TC-10

No Performance Issues

---

########################################################
# DEFINITION OF DONE
########################################################

Player can:

Read

Open

Collect

Inspect

Use

Trigger

---

All interaction types functional.

---

All test cases pass.

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