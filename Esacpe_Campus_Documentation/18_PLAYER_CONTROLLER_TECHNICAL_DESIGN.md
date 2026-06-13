# 18_PLAYER_CONTROLLER_TECHNICAL_DESIGN

# PROJECT INFORMATION

Project:
Escape Campus

Version:
1.0

Document Type:
Technical Design Document

System:
Player Controller

Priority:
CRITICAL

Purpose:

Mendefinisikan seluruh arsitektur teknis Player Controller.

Dokumen ini menjadi sumber kebenaran utama untuk:

- Movement
- Camera
- Interaction
- Audio
- State Machine
- Input Handling

Tidak boleh ada implementasi Player Controller yang menyimpang dari dokumen ini.

---

# SYSTEM OVERVIEW

Player Controller bertanggung jawab untuk:

- Movement
- Looking
- Interacting
- Footstep Audio
- State Management
- Horror Event Integration

Player Controller tidak bertanggung jawab untuk:

- Inventory
- Save System
- Puzzle Logic
- UI Logic
- Dialogue Logic

---

########################################################
# DESIGN GOALS
########################################################

Goal 01

Movement terasa realistis.

---

Goal 02

Movement responsif.

---

Goal 03

Tidak terasa seperti FPS shooter.

---

Goal 04

Mendukung horror pacing.

---

Goal 05

Mudah diperluas.

---

########################################################
# PLAYER HIERARCHY
########################################################

PlayerRoot

├── CameraRoot
│
├── MainCamera
│
├── InteractionOrigin
│
├── AudioRoot
│
└── CharacterController

---

No Rigidbody.

Use CharacterController.

Mandatory.

---

########################################################
# PLAYER COMPONENTS
########################################################

PlayerController

Master Coordinator

---

PlayerMovement

Movement Logic

---

PlayerCamera

Camera Logic

---

PlayerInteraction

Interaction Logic

---

PlayerAudio

Footstep System

---

PlayerStateMachine

State Management

---

PlayerEffects

Visual Effects

---

########################################################
# SCRIPT STRUCTURE
########################################################

Characters/

Player/

PlayerController.cs

PlayerMovement.cs

PlayerCamera.cs

PlayerInteraction.cs

PlayerAudio.cs

PlayerEffects.cs

PlayerStateMachine.cs

---

########################################################
# MOVEMENT SYSTEM
########################################################

Movement Type

First Person

---

Walk Speed

3.5 m/s

---

Sprint Speed

5.5 m/s

---

Crouch Speed

2.0 m/s

---

Gravity

-20

---

Jump

Disabled

---

Reason

Reduce complexity.

Increase horror tension.

---

########################################################
# MOVEMENT STATES
########################################################

STATE_IDLE

---

STATE_WALK

---

STATE_SPRINT

---

STATE_CROUCH

---

STATE_INTERACT

---

STATE_STUNNED

---

STATE_CHASE

---

########################################################
# STATE TRANSITIONS
########################################################

Idle

↓

Walk

---

Walk

↓

Sprint

---

Walk

↓

Crouch

---

Sprint

↓

Walk

---

Any State

↓

Stunned

---

Stunned

↓

Previous State

---

########################################################
# SPRINT RULES
########################################################

Sprint Allowed

YES

---

Sprint Restrictions

Cannot Sprint During

Interaction

Cutscene

Stunned

Final Dialogue

---

Sprint Input

Left Shift

---

########################################################
# CROUCH RULES
########################################################

Input

CTRL

---

Toggle

YES

---

Height

1.0

---

Standing Height

1.8

---

Smooth Transition

Required

Duration

0.15 sec

---

########################################################
# CAMERA SYSTEM
########################################################

Camera Type

First Person

---

Field Of View

Default

75

---

Sprint FOV

82

---

Transition Time

0.2 sec

---

Near Clip

0.05

---

Far Clip

1000

---

########################################################
# CAMERA ROTATION
########################################################

Mouse Sensitivity

Configurable

---

Default

1.0

---

Pitch Clamp

-85

to

85

---

Yaw

Unlimited

---

########################################################
# HEAD BOB SYSTEM
########################################################

Enabled

YES

---

Walk

Low Intensity

---

Sprint

Medium Intensity

---

Crouch

Very Low

---

Disable During

Cutscenes

Jumpscares

---

########################################################
# CAMERA SWAY
########################################################

Enabled

YES

---

Purpose

Natural movement.

---

Intensity

Low

---

Never exceed:

2 degrees

---

########################################################
# INTERACTION SYSTEM
########################################################

Method

Raycast

---

Origin

InteractionOrigin

---

Distance

2.5 meters

---

Layer Mask

Interactable

---

Interaction Key

E

---

########################################################
# INTERACTABLE INTERFACE
########################################################

Mandatory

Every interactable object implements:

IInteractable

---

Methods

Interact()

GetPrompt()

CanInteract()

---

Example

Door

Document

Computer

Puzzle

---

########################################################
# INTERACTION PROMPT
########################################################

Display

[E] Interact

---

Position

Screen Center

---

Show Only

When target valid.

---

########################################################
# FOOTSTEP SYSTEM
########################################################

Surface Types

Concrete

Tile

Wood

Metal

Paper

---

Detection Method

Raycast Down

---

Audio Variation

Minimum

5 sounds per surface

---

Randomized

Required

---

########################################################
# PLAYER AUDIO
########################################################

Supported Sounds

Footsteps

Breathing

Clothing Rustle

Impact

Stunned Audio

---

Not Supported

Voice Acting

---

########################################################
# HORROR INTEGRATION
########################################################

Supported Events

Forced Look

Camera Shake

FOV Distortion

Slow Movement

Temporary Blindness

---

########################################################
# FORCED LOOK SYSTEM
########################################################

Used For

Major Horror Events

---

Behavior

Camera rotates automatically.

---

Duration

0.5–2 sec

---

Must return control smoothly.

---

########################################################
# STUNNED SYSTEM
########################################################

Used By

Major Jumpscares

---

Movement

Disabled

---

Camera

Limited

---

Duration

1–3 sec

---

########################################################
# CHASE STATE
########################################################

Purpose

Final Chase Sequence

---

Sprint Speed

6.5 m/s

---

FOV

90

---

Heartbeat

Enabled

---

Heavy Breathing

Enabled

---

########################################################
# INPUT SYSTEM
########################################################

Unity Input System

Mandatory

---

Actions

Move

Look

Sprint

Crouch

Interact

Pause

---

Forbidden

Legacy Input Manager

---

########################################################
# SAVE SYSTEM SUPPORT
########################################################

Must Save

Position

Rotation

State

---

Must Restore

Position

Rotation

State

---

########################################################
# PERFORMANCE REQUIREMENTS
########################################################

Target FPS Impact

< 0.2 ms

---

No FindObjectOfType

Runtime

---

No GetComponent

Every Frame

---

Cache References

Mandatory

---

No Garbage Allocation

Per Frame

---

########################################################
# DEBUG TOOLS
########################################################

Development Only

---

Show Interaction Ray

---

Show State

---

Show Surface Type

---

Disabled In Release Build

---

########################################################
# ERROR HANDLING
########################################################

If Interaction Target Missing

Ignore Safely

---

If Audio Missing

Log Warning

Continue

---

If Camera Missing

Stop Controller

Log Error

---

########################################################
# TEST CASES
########################################################

TC-01

Walk Works

---

TC-02

Sprint Works

---

TC-03

Crouch Works

---

TC-04

Interaction Works

---

TC-05

Footsteps Correct

---

TC-06

State Changes Correct

---

TC-07

Save Restore Works

---

TC-08

Forced Look Works

---

TC-09

Stunned Works

---

TC-10

No Performance Spikes

---

########################################################
# DEFINITION OF DONE
########################################################

Player can:

Move

Look

Sprint

Crouch

Interact

Save

Load

Experience Horror Events

---

No Critical Bugs

---

Passes All Test Cases

---

Documentation Updated

---

Committed

---

Pushed

---

# END OF DOCUMENT