
# Thunder Strike 3D

A 3D arcade-style space shooter developed in Unity.

---

## 1. Game Overview

Thunder Strike 3D is a stage-based 3D arcade shooter inspired by classic vertical scrolling games such as *Raiden* and *Sky Force*.

The player controls a combat fighter aircraft deployed within an asteroid field defense zone. The objective is to eliminate all hostile units while managing health, resources, and positioning.

The game uses constrained 3D movement on the X–Y plane while maintaining full 3D depth for enemies, projectiles, lighting, and visual effects.



## 2. Game Features

### 2.1 Core Systems

* 3D Player Movement (X–Y plane, fixed Z-axis)
* Automatic Shooting System
* Limited Enemy Spawning System
* Elite Enemy Units with Item Drops
* Score and Win Condition Logic
* Player Health System
* Speed Boost Skill with Cooldown
* Item Drop & Inventory System
* UI State Management (Start / Game Over / Victory)



### 2.2 Combat Mechanics

* Enemies spawn in controlled waves
* Victory is triggered after all enemies are eliminated
* Elite enemies drop collectible items
* Player must balance movement, evasion, and skill usage

---

## 3. Controls

### 3.1 Movement

W – Move Forward
S – Move Backward
A – Move Left
D – Move Right

Movement is restricted to a bounded combat area on the X–Y plane.



### 3.2 Combat and Skills

F – Activate Speed Boost Skill
E – Use Health Item (Heart)
Q – Use Skill Resource Item (Lightning)

Speed Boost temporarily increases movement speed and enters cooldown after activation.

---

## 4. Game Flow

1. Start Screen appears when the game launches.
2. Player presses Start to begin gameplay.
3. Enemy waves spawn within the combat zone.
4. Player eliminates enemies while collecting items.
5. Victory screen appears once all enemies are defeated.
6. Game Over screen appears if player health reaches zero.

Gameplay is paused automatically during UI states using time scale control.



## 5. Technology

* Unity 6 (6000.3 LTS)
* C#
* Unity Input System
* Universal Render Pipeline (URP)
* Rigidbody-based 3D Physics

---

## 6. Planned Features

* Multi-phase Boss Encounter (Level 2)
* Weapon Upgrade System
* Enhanced Enemy AI Patterns
* Improved UI Animations
* Additional Combat Stages



## 7. Developer

Liu Chen
Game Development Coursework 001

GitHub Repository:
[https://github.com/Liu-Chen787/ThunderStrike3D](https://github.com/Liu-Chen787/ThunderStrike3D)


