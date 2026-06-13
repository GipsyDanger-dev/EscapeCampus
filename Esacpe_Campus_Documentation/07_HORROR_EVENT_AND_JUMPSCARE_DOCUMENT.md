# 07_HORROR_EVENT_AND_JUMPSCARE_DOCUMENT

# PROJECT INFORMATION

Title:
Escape Campus

Version:
1.0

Document Type:
Horror Event & Jumpscare Design Document

Purpose:
Mendefinisikan seluruh horror event, psychological event, environmental distortion, echo victim encounter, jumpscare, chase event, trigger logic, dan pacing horror.

Dokumen ini menjadi sumber kebenaran utama untuk seluruh sistem horror dalam game.

Digunakan oleh:

- Game Designer
- Narrative Designer
- Level Designer
- Audio Designer
- Technical Designer
- AI Agent

---

# HORROR PHILOSOPHY

Escape Campus menggunakan:

Psychological Horror First

Jumpscare Second

---

Ketakutan utama harus berasal dari:

- Ketidakpastian
- Kesendirian
- Distorsi realitas
- Tekanan akademik
- Kehadiran Semester 14

---

Ketakutan tidak boleh berasal dari:

- Monster muncul setiap saat
- Jumpscare murahan
- Suara keras tanpa konteks

---

# HORROR GOLDEN RULE

Fear comes from anticipation.

Not from jumpscares.

Player harus menghabiskan lebih banyak waktu menunggu ketakutan dibanding mengalami ketakutan.

---

# PACING STRUCTURE

PHASE 1

0–15 Menit

Mood:
Uneasy

Player Feeling:
"Kayaknya ada yang aneh."

---

PHASE 2

15–30 Menit

Mood:
Investigation

Player Feeling:
"Aku tidak sendirian."

---

PHASE 3

30–50 Menit

Mood:
Paranoia

Player Feeling:
"Ada sesuatu yang mengawasiku."

---

PHASE 4

50–70 Menit

Mood:
Panic

Player Feeling:
"Aku harus keluar."

---

PHASE 5

70–90 Menit

Mood:
Survival

Player Feeling:
"Lari."

---

########################################################
# PSYCHOLOGICAL EVENTS
########################################################

PE-001

Location:
Computer Lab

Trigger:
Power outage

Event:

Semua monitor mati.

Satu monitor tetap menyala.

Menampilkan:

LOGIN FAILED

LOGIN FAILED

LOGIN FAILED

Padahal pemain belum login.

---

PE-002

Location:
Computer Lab

Trigger:
3 menit eksplorasi

Event:

Nama pemain muncul di daftar mahasiswa.

Status:

FAILED

---

PE-003

Location:
Corridor Floor 2

Trigger:
Pertama kali melewati koridor

Event:

Ada mahasiswa berdiri di ujung lorong.

Saat didekati:

Menghilang.

---

PE-004

Location:
Classroom A

Trigger:
Puzzle selesai

Event:

Seluruh kursi menghadap pemain.

---

PE-005

Location:
Classroom B

Trigger:
Masuk ruangan kedua kali

Event:

Semua foto wisuda berubah.

Wajah pemain muncul.

---

PE-006

Location:
Toilet

Trigger:
Lihat cermin

Event:

Refleksi terlambat 2 detik.

---

PE-007

Location:
Toilet

Trigger:
Lihat cermin kedua kali

Event:

Refleksi tidak mengikuti gerakan.

---

PE-008

Location:
Library

Trigger:
Eksplorasi

Event:

Terdengar suara halaman buku dibalik.

Tidak ada orang.

---

PE-009

Location:
Library

Trigger:
Ambil dokumen Raka

Event:

Rak buku berubah posisi.

---

PE-010

Location:
Library

Trigger:
Kembali ke area sebelumnya

Event:

Semua kursi terbalik.

---

PE-011

Location:
Lecturer Office

Trigger:
Masuk pertama kali

Event:

Komputer dosen menyala sendiri.

Email terbuka.

Subject:

Where Is Raka?

---

PE-012

Location:
Lecturer Office

Trigger:
Buka lemari arsip

Event:

Terdengar suara:

"Jangan buka."

---

PE-013

Location:
Security Room

Trigger:
Melihat CCTV

Event:

Pemain terlihat di kamera.

Padahal kamera mengarah ke ruangan kosong.

---

PE-014

Location:
Security Room

Trigger:
Lihat CCTV kedua kali

Event:

Semester 14 berdiri di belakang pemain.

Saat menoleh:

Tidak ada apa-apa.

---

PE-015

Location:
Archive Room

Trigger:
Masuk pertama kali

Event:

Semua lemari arsip terbuka perlahan.

---

PE-016

Location:
Archive Room

Trigger:
Ambil dokumen ritual

Event:

Seluruh lemari tertutup bersamaan.

---

PE-017

Location:
Any Corridor

Trigger:
Random scripted

Event:

Terdengar suara sidang.

Tidak ada sumber suara.

---

PE-018

Location:
Any Corridor

Trigger:
Dekat event lore

Event:

Terdengar suara:

"Revisi lagi."

---

PE-019

Location:
Floor 3

Trigger:
Setelah reveal Raka

Event:

Nama ruangan berubah.

Menjadi:

RAKA PRASETYA

---

PE-020

Location:
Library

Trigger:
Akhir game

Event:

Seluruh buku berubah judul.

Semua menjadi:

THESIS REJECTED

---

########################################################
# ECHO VICTIM EVENTS
########################################################

EV-001

Nadia muncul mengetik.

Menghilang saat didekati.

---

EV-002

Laptop Nadia menampilkan:

REVISION 27

---

EV-003

Nadia menangis.

---

EV-004

Bima duduk di depan ruang dosen.

---

EV-005

Bima masih di tempat yang sama 30 menit kemudian.

---

EV-006

Bima bertanya:

"Sudah lima menit kan?"

---

EV-007

Sinta presentasi.

---

EV-008

Slide tidak pernah berubah.

---

EV-009

Suara tepuk tangan kosong.

---

EV-010

Dimas berdiri memakai toga.

---

EV-011

Dimas perlahan menoleh.

---

EV-012

Wajah Dimas kosong.

---

EV-013

Seluruh Echo muncul di koridor jauh.

---

EV-014

Seluruh Echo hilang bersamaan.

---

EV-015

Secret Ending Event.

Semua Echo menghilang permanen.

---

########################################################
# MINOR JUMPSCARES
########################################################

JS-001

Monitor Face

Location:
Computer Lab

Trigger:
Login berhasil

Effect:

Wajah Semester 14 muncul 0.5 detik.

Audio:
Sharp Hit

---

JS-002

Bookshelf Collapse

Location:
Library

Trigger:
Ambil arsip Raka

Effect:

Rak jatuh tepat di depan pemain.

---

JS-003

Bathroom Stall

Location:
Toilet

Trigger:
Buka bilik terakhir

Effect:

Semester 14 berdiri di dalam.

Menghilang seketika.

---

JS-004

Document Grab

Location:
Lecturer Office

Trigger:
Ambil dokumen

Effect:

Tangan keluar dari tumpukan arsip.

---

JS-005

Graduation Photo

Location:
Hallway

Trigger:
Periksa foto

Effect:

Mata pada foto bergerak.

---

JS-006

CCTV Flash

Location:
Security Room

Trigger:
Periksa monitor

Effect:

Semester 14 muncul 1 frame.

---

JS-007

Mirror Distortion

Location:
Toilet

Trigger:
Lihat cermin lama

Effect:

Refleksi tersenyum.

---

JS-008

Library Whisper

Location:
Library

Trigger:
Puzzle hampir selesai

Effect:

Bisikan tepat di telinga pemain.

---

########################################################
# MAJOR JUMPSCARES
########################################################

MJS-001

First Full Appearance

Location:
Library

Trigger:
Lore Progress 40%

Effect:

Semester 14 muncul penuh pertama kali.

---

MJS-002

Archive Ambush

Location:
Archive Room

Trigger:
Dokumen ritual ditemukan

Effect:

Semester 14 muncul dari belakang lemari.

---

MJS-003

CCTV Manifestation

Location:
Security Room

Trigger:
Lore Progress 60%

Effect:

Semester 14 keluar dari monitor.

---

MJS-004

Classroom Distortion

Location:
Classroom B

Trigger:
Backtracking

Effect:

Semester 14 duduk di kursi siswa.

---

MJS-005

Server Room Manifestation

Location:
Server Room

Trigger:
Puzzle akhir selesai

Effect:

Reveal penuh.

Dialogue dimulai.

---

MJS-006

Final Chase Start

Location:
Server Room

Trigger:
Cutscene selesai

Effect:

Semester 14 berubah ke Final Ritual Form.

---

########################################################
# ACADEMIC LOOP DISTORTIONS
########################################################

ALD-001

Poster Kelulusan berubah menjadi:

SIDANG DITUNDA

---

ALD-002

Nilai A berubah menjadi E

---

ALD-003

Nama dosen berubah menjadi:

RAKA PRASETYA

---

ALD-004

Tanggal sidang selalu berubah.

---

ALD-005

Jam kampus berhenti.

---

ALD-006

Nomor ruangan berubah.

---

ALD-007

Daftar mahasiswa berubah.

Nama pemain muncul.

---

ALD-008

Pengumuman:

ANDA TIDAK LULUS

---

########################################################
# FINAL CHASE EVENTS
########################################################

FC-001

Server Room Lockdown

---

FC-002

Lampu merah aktif

---

FC-003

Semester 14 muncul

---

FC-004

Koridor berubah bentuk

---

FC-005

Rak runtuh

---

FC-006

Tangga retak

---

FC-007

Pengumuman kampus rusak

---

FC-008

Echo Victims muncul melihat pemain

---

FC-009

Lobby Collapse

---

FC-010

Exit Gate Reach

Final Cutscene Trigger

---

########################################################
# AUDIO HORROR RULES
########################################################

Semester 14

Audio:

- Whisper
- Breathing
- Paper Scratching

---

Final Form

Audio:

- Distorted Student Voices
- Graduation Announcements
- Heavy Low Frequency

---

Echo Victims

Audio:

- Sad
- Human
- Quiet

---

IMPORTANT

No loud sound spam.

No random scream spam.

---

########################################################
# TECHNICAL TRIGGER RULES
########################################################

Every major horror event:

One-time only.

---

Every major jumpscare:

One-time only.

---

Psychological events:

Can repeat if narratively justified.

---

Echo events:

Can repeat.

Must have cooldown.

---

Cooldown Minimum:

120 seconds

---

No more than:

2 jumpscares within 5 minutes.

---

########################################################
# AI AGENT RESTRICTIONS
########################################################

AI Agent must never:

- Add random monsters
- Add combat
- Add weapon systems
- Add boss fights
- Add survival mechanics

---

AI Agent must prioritize:

- Psychological horror
- Environmental storytelling
- Narrative consistency
- Tragic horror

---

# END OF DOCUMENT