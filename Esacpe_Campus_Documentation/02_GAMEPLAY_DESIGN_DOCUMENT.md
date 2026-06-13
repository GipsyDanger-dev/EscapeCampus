# GAMEPLAY DESIGN DOCUMENT

## PROJECT INFORMATION

### Game Title

Escape Campus

### Document Version

1.0

### Purpose

Dokumen ini menjelaskan seluruh mekanik gameplay Escape Campus secara detail.

Dokumen ini menjadi acuan implementasi sistem gameplay oleh programmer dan AI Agent.

---

# 1. GAMEPLAY OVERVIEW

Escape Campus adalah game horror puzzle first-person dimana pemain harus mengeksplorasi gedung kampus yang mengalami fenomena loop waktu supernatural.

Pemain tidak memiliki kemampuan menyerang.

Seluruh progress dilakukan melalui:

* Eksplorasi
* Pengumpulan informasi
* Penyelesaian puzzle
* Membuka area baru
* Menghindari ancaman supernatural

---

# 2. CAMERA SYSTEM

## Perspective

First Person Perspective.

Pemain melihat dunia melalui sudut pandang karakter utama.

---

## Camera Behavior

Camera Features:

* Head movement saat berjalan
* Camera shake saat kejadian horror
* Camera shake saat dikejar
* FOV adjustment saat sprint

---

## Default Camera Settings

Field Of View:

90°

Sprint FOV:

100°

Camera Shake:

Enabled

---

# 3. PLAYER SYSTEM

## Player Character

Nama:

Arga

Role:

Mahasiswa tingkat akhir yang terjebak dalam loop kampus.

---

# 3.1 Movement System

Player memiliki tiga mode pergerakan.

---

## Walking

Default movement.

Speed:

4 meter/detik

Digunakan untuk:

* Eksplorasi
* Membaca clue
* Puzzle solving

---

## Sprinting

Mode lari.

Speed:

7 meter/detik

Digunakan untuk:

* Melarikan diri
* Menghindari Semester 14

Restriction:

Sprint memiliki stamina.

---

## Crouching

Mode jongkok.

Speed:

2 meter/detik

Digunakan untuk:

* Bergerak perlahan
* Membangun ketegangan

---

# 3.2 Stamina System

Stamina hanya digunakan saat sprint.

Maximum:

100%

Consumption:

20% per detik

Recovery:

15% per detik ketika tidak sprint.

---

Rules:

Jika stamina 0:

Player tidak dapat sprint selama 3 detik.

---

# 3.3 Player Interaction

Semua interaksi menggunakan sistem raycast.

---

## Interaction Distance

Maximum:

3 meter

---

## Interaction Button

Default:

E

---

## Interaction Types

### Pickup

Mengambil item.

Contoh:

* Key
* Document
* Puzzle Object

---

### Examine

Melihat objek secara dekat.

Contoh:

* Foto
* Dokumen
* Catatan

---

### Activate

Mengaktifkan objek.

Contoh:

* Komputer
* Switch
* Panel Server

---

# 4. INTERACTION SYSTEM

## Overview

Sistem interaksi menggunakan raycast dari kamera pemain.

Flow:

Player melihat objek

↓

Raycast mendeteksi objek

↓

UI Interaction muncul

↓

Player menekan E

↓

Event berjalan

---

## Interaction UI

Format:

```
[E] Interact
```

atau

```
[E] Pick Up Key
```

---

# 5. INVENTORY SYSTEM

## Purpose

Inventory digunakan untuk menyimpan item penting yang ditemukan pemain.

---

## Inventory Limitation

Maximum Slot:

8

Format:

2 x 4 grid

---

## Item Categories

## Key Item

Item untuk membuka akses.

Contoh:

* Lab Key
* Server Card

---

## Puzzle Item

Item yang digunakan dalam puzzle.

Contoh:

* Password Note
* USB Drive

---

## Story Item

Item untuk lore.

Contoh:

* Diary
* Document
* Photo

---

# 5.1 Item Data Structure

Setiap item memiliki:

```
Item ID

Item Name

Item Type

Description

Icon

Usage Function
```

---

Contoh:

```
ID:
KEY_SERVER_01

Name:
Server Room Access Card

Type:
Key Item

Description:
Kartu akses ruang server lama.
```

---

# 6. DOOR SYSTEM

## Purpose

Mengatur seluruh pintu dalam game.

---

## Door States

Setiap pintu memiliki 4 kondisi.

---

## Locked

Pintu tidak dapat dibuka.

Player membutuhkan item tertentu.

---

## Unlocked

Pintu dapat dibuka.

---

## Opened

Pintu sedang terbuka.

---

## Closed

Pintu dapat ditutup kembali.

---

# Door Interaction

Jika Locked:

UI:

"Door Locked"

---

Jika membutuhkan item:

UI:

"Requires Server Access Card"

---

# 7. OBJECT SYSTEM

Objek dunia dapat memiliki behavior khusus.

---

## Static Object

Tidak dapat berinteraksi.

Contoh:

* Kursi
* Meja
* Rak

---

## Interactive Object

Dapat digunakan.

Contoh:

* Komputer
* Printer
* CCTV

---

## Horror Object

Objek yang memiliki event horror.

Contoh:

* Lampu
* Telepon
* Monitor

---

# 8. PUZZLE SYSTEM

## Puzzle Philosophy

Puzzle harus:

* Memiliki hubungan dengan cerita
* Memiliki petunjuk
* Tidak random
* Dapat diselesaikan tanpa walkthrough

---

# Puzzle Difficulty

Easy:

40%

Medium:

50%

Hard:

10%

---

# Puzzle Categories

## Code Puzzle

Contoh:

Password komputer.

---

## Searching Puzzle

Mencari item tertentu.

---

## Logic Puzzle

Menghubungkan informasi.

---

## Environment Puzzle

Mengubah kondisi lingkungan.

---

# 9. PROGRESSION SYSTEM

Progression bersifat linear.

Tujuan:

Menjaga pacing horror.

---

Flow:

START

↓

Lab Komputer

↓

Library

↓

Ruang Dosen

↓

Server Room

↓

Chase

↓

Ending

---

# 10. HORROR EVENT SYSTEM

## Purpose

Mengatur kejadian horror berdasarkan progress pemain.

---

Event Types:

## Audio Event

Contoh:

* Bisikan
* Langkah kaki
* Tangisan

---

## Visual Event

Contoh:

* Bayangan
* Objek bergerak
* Lampu mati

---

## Jumpscare Event

Contoh:

Semester 14 muncul.

---

# Event Trigger

Trigger dapat berupa:

* Player masuk area
* Player mengambil item
* Player menyelesaikan puzzle
* Timer

---

# 11. SEMESTER 14 SYSTEM

## Overview

Semester 14 adalah antagonis utama.

Dia tidak muncul terus menerus.

Tujuan:

Membangun rasa takut.

---

# Behavior States

## State 1: Hidden

Tidak terlihat.

Digunakan untuk stalking.

---

## State 2: Observe

Mengawasi pemain dari jauh.

Contoh:

* Ujung lorong
* Balik kaca

---

## State 3: Manifest

Muncul untuk jumpscare.

---

## State 4: Chase

Mengejar pemain.

---

# 12. CHASE SYSTEM

## Trigger

Aktif setelah pemain mengambil Dokumen Ritual di Server Room.

---

## Duration

3–5 menit.

---

## Objective

Pemain harus mencapai gerbang kampus.

---

## Chase Rules

Semester 14:

* Tidak dapat dibunuh
* Selalu mengejar
* Dapat membuka pintu tertentu
* Tidak muncul terlalu dekat agar pemain memiliki kesempatan

---

# Chase Path

Server Room

↓

Emergency Stair

↓

Library

↓

Lobby

↓

Main Gate

---

# 13. CHECKPOINT SYSTEM

Game memiliki checkpoint otomatis.

Checkpoint aktif setelah:

* Menyelesaikan puzzle
* Membuka area baru
* Memulai chase

---

# 14. SAVE SYSTEM

Save dilakukan otomatis.

Save Data:

```
Current Scene

Completed Puzzle

Collected Items

Unlocked Door

Story Progress
```

---

# 15. ENDING SYSTEM

## Ending Trigger

Berdasarkan progress utama.

---

## Good Ending

Condition:

Player menghancurkan ritual.

Result:

Loop berhenti.

Player kembali ke dunia normal.

---

## Secret Ending

Condition:

Player menyelesaikan semua lore.

Result:

Semester 14 masih terlihat dalam foto wisuda.

---

# 16. GAMEPLAY BALANCE

Target waktu:

Introduction:

5 menit

Exploration:

20 menit

Puzzle:

15 menit

Chase:

5 menit

Ending:

5 menit

---

# 17. DEVELOPMENT PRIORITY

Urutan implementasi:

1. Player Controller

2. Interaction System

3. Door System

4. Inventory

5. Puzzle Framework

6. Horror Event System

7. Semester 14 AI

8. Chase System

9. Save System

10. Ending System

---

# 18. NON-FEATURE LIST

Tidak dibuat:

* Combat System
* Weapon System
* Multiplayer
* Character Skill
* Crafting
* Open World
* Random Enemy Spawn

Tujuan:

Menjaga fokus pada horror narrative experience.
