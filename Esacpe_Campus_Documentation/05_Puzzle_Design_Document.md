# 05_PUZZLE_DESIGN_DOCUMENT

# PROJECT INFORMATION

Title:
Escape Campus

Version:
2.0

Document Type:
Puzzle Design Document

Purpose:
Dokumen ini mendefinisikan seluruh puzzle yang terdapat dalam Escape Campus.

Dokumen ini digunakan oleh:

- Game Designer
- Level Designer
- Narrative Designer
- Gameplay Programmer
- AI Agent

Semua puzzle dalam game harus mengikuti aturan dan struktur pada dokumen ini.

---

# PUZZLE DESIGN PHILOSOPHY

Escape Campus menggunakan pendekatan:

EXPERT HORROR INVESTIGATION

dan bukan

TRIAL AND ERROR PUZZLE

Tujuan puzzle bukan membuat pemain menebak.

Tujuan puzzle adalah membuat pemain:

- Mengamati lingkungan
- Mengumpulkan bukti
- Menghubungkan petunjuk
- Membuat deduksi
- Menemukan kebenaran

Pemain harus merasa seperti sedang melakukan investigasi terhadap misteri kampus.

---

# DESIGN PRINCIPLES

Setiap puzzle wajib:

1. Memiliki hubungan langsung dengan cerita.
2. Memiliki petunjuk yang cukup.
3. Dapat diselesaikan secara logis.
4. Memiliki satu solusi benar.
5. Tidak membutuhkan pengetahuan di luar game.
6. Tidak bergantung pada keberuntungan.
7. Tidak menggunakan kode acak tanpa konteks.

---

# INVESTIGATION DEPTH SCALE

LEVEL 1

Single Room Investigation

Petunjuk berasal dari satu ruangan.

Digunakan hanya untuk tutorial.

---

LEVEL 2

Multi Room Investigation

Petunjuk berasal dari 2–3 ruangan.

---

LEVEL 3

Cross Floor Investigation

Petunjuk berasal dari beberapa lantai.

---

LEVEL 4

Narrative Investigation

Pemain harus memahami cerita.

---

LEVEL 5

Master Investigation

Pemain harus:

- Mengingat informasi lama
- Menghubungkan banyak petunjuk
- Memahami kronologi
- Menafsirkan dokumen

Digunakan pada puzzle akhir.

---

# DIFFICULTY DISTRIBUTION

Tutorial:
0%

Easy:
0%

Intermediate:
30%

Expert:
40%

Master:
30%

---

# PUZZLE COMPLEXITY RULE

Minimum clue:

3

Recommended:

5

Expert:

7

Master:

8–12

---

# CLUE DISTRIBUTION RULE

Visual Clue:
30%

Document Clue:
30%

Environmental Clue:
20%

Audio Clue:
20%

---

# MEMORY REQUIREMENT

Game mengasumsikan pemain mampu:

- Mengingat nama karakter
- Mengingat lokasi
- Mengingat dokumen
- Menghubungkan petunjuk

Player diperbolehkan mencatat secara manual.

Game tidak wajib menyediakan notebook otomatis.

---

# BRUTE FORCE PROTECTION

Password, simbol, dan kombinasi tidak boleh dapat diselesaikan dengan brute force.

Jika pemain salah berkali-kali:

- Sistem reset
- Event horror muncul
- Petunjuk tambahan diberikan

---

# NARRATIVE CONNECTION RULE

Setiap puzzle harus mengungkap salah satu:

- Masa lalu Raka
- Ritual
- Korban sebelumnya
- Sejarah kampus
- Motivasi Semester 14

Puzzle tanpa hubungan naratif tidak boleh dibuat.

---

# PUZZLE OVERVIEW

PZ-001
Laboratory Authentication

Difficulty:
Intermediate

---

PZ-002
Academic Schedule Reconstruction

Difficulty:
Intermediate

---

PZ-003
The Missing Thesis

Difficulty:
Expert Investigation

---

PZ-004
The Truth About Semester 14

Difficulty:
Expert Investigation

---

PZ-005
Cult Ritual Decryption

Difficulty:
Master Investigation

---

PZ-006
Loop Core Synchronization

Difficulty:
Master Investigation

---

PZ-S01
Forgotten Graduation

Difficulty:
Master Optional Puzzle

Unlock:
Secret Ending

---

############################################################
# PZ-001
############################################################

TITLE

Laboratory Authentication

---

LOCATION

Computer Laboratory

---

DIFFICULTY

Intermediate

---

STORY PURPOSE

Memperkenalkan investigasi.

Mengenalkan bahwa kampus tidak lagi normal.

---

SCENARIO

Pemain harus mengakses komputer laboratorium utama.

Komputer meminta:

- Username
- Password

---

CLUE 01

Whiteboard

"Tolong ganti password setelah proses akreditasi selesai."

---

CLUE 02

Poster Akreditasi

Akreditasi Terakhir:
2021

---

CLUE 03

Monitor Rusak

Username terlihat sebagian:

LAB_ADMIN

---

PLAYER OBJECTIVE

Masuk ke sistem laboratorium.

---

SOLUTION

Username:
LAB_ADMIN

Password:
AKREDITASI2021

---

REWARD

Laboratory Exit Key

---

HORROR EVENT

Saat login berhasil.

Semua monitor menyala bersamaan.

JS-001 terjadi.

---

############################################################
# PZ-002
############################################################

TITLE

Academic Schedule Reconstruction

---

LOCATION

Classroom A

---

DIFFICULTY

Intermediate

---

STORY PURPOSE

Mengajarkan pemain menghubungkan informasi.

---

SCENARIO

Jadwal kuliah rusak.

Pemain harus menentukan urutan kelas.

---

CLUE 01

Papan Pengumuman

Basis Data sebelum Jaringan.

---

CLUE 02

Catatan Mahasiswa

AI setelah Basis Data.

---

CLUE 03

Kalender

Jaringan bukan kelas terakhir.

---

CLUE 04

Presensi

AI berada pada sesi sore.

---

CLUE 05

Email Dosen

Basis Data selalu pagi.

---

OBJECTIVE

Menentukan urutan kelas.

---

SOLUTION

Basis Data

Jaringan

AI

---

REWARD

Library Access Code

---

HORROR EVENT

Saat puzzle selesai.

Suara langkah kaki muncul di belakang pemain.

---

############################################################
# PZ-003
############################################################

TITLE

The Missing Thesis

---

LOCATION

Library

---

DIFFICULTY

Expert Investigation

---

STORY PURPOSE

Reveal pertama tentang Raka.

---

SCENARIO

Pemain mencari skripsi milik Raka.

Tidak ada petunjuk langsung.

---

CLUE 01

Diary Page 01

"Aku menyembunyikannya di tempat yang tidak pernah berubah."

---

CLUE 02

Diary Page 02

"Aku tidak pernah melupakan nomor itu."

---

CLUE 03

Graduation Photo

Raka berdiri di posisi ke-14.

---

CLUE 04

Student Record

Nomor mahasiswa digunakan sebagai indeks arsip.

---

CLUE 05

Library Guide

Arsip disusun berdasarkan:

Angkatan-Jurusan-Nomor Mahasiswa

---

CLUE 06

Security Database

NIM Raka:
14027

---

CLUE 07

Yearbook

Raka masuk tahun 2019.

---

PLAYER OBJECTIVE

Menemukan arsip skripsi yang benar.

---

SOLUTION

2019-TI-14027

---

REWARD

Lecturer Office Access Card

---

LORE REVEAL

Raka pernah menjadi mahasiswa berprestasi.

---

HORROR EVENT

Rak perpustakaan berpindah posisi.

JS-002 terjadi.

---

############################################################
# PZ-004
############################################################

TITLE

The Truth About Semester 14

---

LOCATION

Lecturer Office

---

DIFFICULTY

Expert Investigation

---

STORY PURPOSE

Mengungkap identitas Semester 14.

---

SCENARIO

Terdapat 5 berkas mahasiswa.

Pemain harus menentukan siapa Semester 14.

---

CLUE 01

IPK di atas 3.5

---

CLUE 02

Mengikuti organisasi keamanan jaringan.

---

CLUE 03

Proposal skripsi jaringan.

---

CLUE 04

Semester lebih dari 12.

---

CLUE 05

Surat peringatan akademik.

---

CLUE 06

Email dosen pembimbing.

---

CLUE 07

Daftar mahasiswa aktif.

---

CLUE 08

Daftar mahasiswa DO.

---

OBJECTIVE

Menentukan identitas Semester 14.

---

SOLUTION

Raka Prasetya

---

REWARD

Archive Room Key

---

MAJOR REVEAL

Semester 14 adalah Raka.

---

HORROR EVENT

JS-004

Tangan keluar dari tumpukan dokumen.

---

############################################################
# PZ-005
############################################################

TITLE

Cult Ritual Decryption

---

LOCATION

Archive Room

---

DIFFICULTY

Master Investigation

---

STORY PURPOSE

Mengungkap ritual loop.

---

SCENARIO

Pemain menemukan simbol ritual.

Tidak ada petunjuk langsung mengenai urutannya.

---

CLUE 01

Simbol A muncul di Classroom B.

---

CLUE 02

Simbol B muncul di Library.

---

CLUE 03

Simbol C muncul di Lecturer Office.

---

CLUE 04

Simbol D muncul di Security Room.

---

CLUE 05

Dokumen ritual:

"Awal dimulai dari tempat ilmu lahir."

---

CLUE 06

Catatan kultus:

"Pengetahuan mendahului pengawasan."

---

CLUE 07

Jurnal ritual:

"Penghakiman selalu datang terakhir."

---

CLUE 08

Peta simbol.

---

CLUE 09

Foto ritual lama.

---

CLUE 10

Audio rekaman ritual.

---

PLAYER OBJECTIVE

Menentukan urutan simbol ritual.

---

SOLUTION LOGIC

Classroom

Library

Office

Security

---

SOLUTION

A → B → C → D

---

REWARD

Server Room Access Card

---

LORE REVEAL

Raka bergabung dengan kelompok ritual.

---

HORROR EVENT

Lemari arsip bergerak sendiri.

---

############################################################
# PZ-006
############################################################

TITLE

Loop Core Synchronization

---

LOCATION

Server Room

---

DIFFICULTY

Master Investigation

---

STORY PURPOSE

Menghentikan loop.

---

SCENARIO

Empat server terhubung dengan ritual.

Pemain harus menentukan konfigurasi yang benar.

---

CLUE 01

Server Log Alpha

---

CLUE 02

Server Log Beta

---

CLUE 03

Server Log Gamma

---

CLUE 04

Server Log Delta

---

CLUE 05

Maintenance Report

---

CLUE 06

Backup Report

---

CLUE 07

Power Distribution Diagram

---

CLUE 08

CCTV Snapshot

---

CLUE 09

Ritual Journal

---

CLUE 10

Technician Notes

---

CLUE 11

Emergency Report

---

CLUE 12

Server Map

---

OBJECTIVE

Menentukan:

- Server utama
- Server ritual
- Server cadangan
- Server rusak

Kemudian melakukan sinkronisasi.

---

SOLUTION

Alpha
↓
Beta
↓
Gamma
↓
Delta

---

REWARD

Ritual Document

---

MAJOR REVEAL

Sumber loop ditemukan.

---

HORROR EVENT

JS-006

Semester 14 muncul secara penuh.

---

CHASE SEQUENCE START

---

############################################################
# OPTIONAL PUZZLE
############################################################

PZ-S01

TITLE

Forgotten Graduation

---

TYPE

Optional

---

DIFFICULTY

Master

---

PURPOSE

Secret Ending

---

SCENARIO

Pemain harus menemukan seluruh foto angkatan yang tersembunyi.

---

REQUIREMENT

Photo 01

Photo 02

Photo 03

Photo 04

---

REWARD

Secret Ending Unlock

---

LORE REVEAL

Korban sebelum Arga ternyata lebih banyak daripada yang diperkirakan.

---

# HORROR INTEGRATION RULE

Setiap puzzle wajib memiliki tekanan psikologis.

Contoh:

- Lampu mati
- Bisikan muncul
- Bayangan terlihat
- Semester 14 muncul sekilas
- Lingkungan berubah

Puzzle tidak boleh terasa seperti minigame terpisah dari horror.

---

# FORBIDDEN PUZZLE DESIGN

Dilarang menggunakan:

- Random password
- Random symbol
- Brute force puzzle
- Puzzle matematika kompleks
- Puzzle yang membutuhkan internet
- Puzzle yang membutuhkan pengetahuan dunia nyata
- Puzzle yang tidak berhubungan dengan cerita

---

# PLAYER EXPERIENCE TARGET

Saat menemukan puzzle:

"Hah? Maksudnya apa?"

Saat menemukan beberapa petunjuk:

"Kayaknya ini saling berhubungan."

Saat menemukan solusi:

"Anjir... pantes."

Bukan:

"Mana mungkin ketebak."

---

# COMPLETION TARGET

PZ-001
3–5 menit

PZ-002
5–7 menit

PZ-003
10–15 menit

PZ-004
10–15 menit

PZ-005
15–20 menit

PZ-006
15–20 menit

Optional Puzzle
5–10 menit

Total Puzzle Time:

60–90 menit tanpa walkthrough.

---

# END OF DOCUMENT