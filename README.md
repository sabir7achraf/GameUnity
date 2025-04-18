# Université Abdelmalek Essaâdi  
## Faculté des Sciences & Techniques de Tanger  
### Département Génie Informatique  

---

# 📝 REPORT: Ruby Adventure Game
![image](https://github.com/user-attachments/assets/ddbb92f9-0014-4115-8e57-567d2f8f5ed0)

- **Department:** Computer Engineering  
- **Academic Year:** 2023/2024  
- **Module:** Advanced Algorithms & Graph Theory  
- **Realised by:**  
  - Ashraf Sabir  
  - Ayoub Ait Said  
- **Supervised by:**  
  - Lotfi EL AACHAK  
  - Abderrahim GHADI  

---

## 📑 Table of Contents
1. [Introduction](#introduction)  
2. [Tools](#tools)  
3. [Random Generation of Game Map](#random-generation-of-game-map)  
   - 3.1 [Initial Attempt](#1-make-an-initial-attempt-to-generate)  
   - 3.2 [Practical Implementation](#2-a-practical-way-to-generate)  
   - 3.3 [Generating Enemies and Coins](#3-generate-enemies-and-coins-randomly)  
   - 3.4 [Player and Boss Placement](#4-generate-player-and-boss-with-map)  
4. [Pathfinding Algorithms](#pathfinding-algorithms)  
   - 4.1 [Grid of Nodes](#1-grid-of-nodes)  
   - 4.2 [A* Algorithm](#2-a-algorithm)  
   - 4.3 [Dijkstra Algorithm](#3-dijkstra-algorithm)  
5. [Conclusion](#conclusion)  
6. [Bibliography](#bibliography)  

---

## 📘 Introduction
Procedural world generation offers a powerful solution by utilizing algorithms to automatically create game worlds. In this project, inspired by *Ruby's Adventure*, we designed a system capable of generating game levels (maps) using Graph Theory and various algorithms for pathfinding and entity placement.

---

## 🛠️ Tools

### 1. Unity (Game Engine)
Unity is a cross-platform game engine used to develop 2D and 3D games. It supports mobile, desktop, and AR/VR platforms.

![image](https://github.com/user-attachments/assets/33607844-f4c5-4810-842c-04416c77a0b0)

### 2. Visual Studio IDE
Visual Studio is an IDE developed by Microsoft for building apps, web services, and more, using .NET technologies.
![image](https://github.com/user-attachments/assets/7d36bd63-4295-4059-bd64-e923ab006d78)

### 3. C#
C# is an object-oriented programming language developed by Microsoft. It is widely used with Unity for game development.
![image](https://github.com/user-attachments/assets/758d70d7-e7bb-4f57-9852-98998449175d)

### 4. GitHub
GitHub is a platform for code hosting and collaboration. It allows version control, sharing, and team development.

---

## 🎲 Random Generation of Game Map

### 1. Make an Initial Attempt to Generate
We attempted to model rooms as nodes and connect them with edges but faced difficulties due to limited experience with Unity and C#.

### 2. A Practical Way to Generate
We followed tutorials from *Sunny Valley Studio* using:
- **Random Walk**: Generates organic, unpredictable paths.
- **Binary Space Partitioning (BSP)**: Divides space recursively to form structured dungeons.


![image](https://github.com/user-attachments/assets/cfb74433-46de-4bbe-868a-ac4a75f2dfc8)

### 3. Generate Enemies and Coins Randomly
Enemies and coins were placed using random coordinates within room bounds:
```csharp
float randomX = Random.Range(roomBounds.min.x + 4, roomBounds.max.x - 4);
float randomY = Random.Range(roomBounds.min.y + 4, roomBounds.max.y - 4);
```

![image](https://github.com/user-attachments/assets/5f8bdd9a-9a6c-4a48-8c12-d47d64e1265e)

### 4. Generate Player and Boss with Map

- **Player** is placed in the first room.
- **Boss** is placed in the room farthest from the player by calculating Euclidean distances.

---
![image](https://github.com/user-attachments/assets/217d1167-7755-4706-b68f-7cfeb9ef079c)

## 🧠 Pathfinding Algorithms

### 1. Grid of Nodes
- Implemented via `GridA` and `Node` classes.
- Grid is made of **walkable** and **unwalkable** nodes.
- Uses `Physics2D.OverlapCircle()` to detect obstacles.

![image](https://github.com/user-attachments/assets/8b95bf3c-9475-4f03-a3bd-adfd2955e3e5)

### 2. A* Algorithm
- Efficient pathfinding (not always optimal):
  - Maintains **open/closed** lists.
  - Considers neighboring nodes and updates scores accordingly.
  - Stops upon reaching the goal.
![image](https://github.com/user-attachments/assets/8304b3bd-c204-48ae-9ebb-9fd019bb19e3)

#### A. Implementation
- `Pathfinding` class:
  - Uses `GridA` to find shortest path from Player to Boss.
  - Initialized in the `Awake()` method.
![image](https://github.com/user-attachments/assets/bff296cd-a038-4fa6-9832-c2f76b292be7)

#### B. Follow A*
- `FollowPathAstar` class:
  - Moves the player along the A* path.
  - Uses raycasting to detect and avoid enemies.

### 3. Dijkstra Algorithm
- Finds shortest path to **all nodes** from a single source.

  ![image](https://github.com/user-attachments/assets/e11dcad1-2b02-486d-8d78-26edf70fcaf4)


#### A. Implementation
- `Dijkstra` class:
  - Uses sets to track **visited/unvisited** nodes.
  - Calculates **cost** and **parent pointers**.
  - Reconstructs path from Boss to Player.
![image](https://github.com/user-attachments/assets/8c0e6454-6d57-4828-91df-3efe2ccedea1)

#### B. Follow Dijkstra
- `FollowDijkstra` class:
  - Moves player along the shortest path from Dijkstra's algorithm.

---

## ✅ Conclusion

We successfully implemented **procedural map generation** and **advanced pathfinding** with Unity and C#. The integration of **A*** and **Dijkstra** algorithms enhances gameplay dynamics. Future improvements could involve **machine learning-based navigation** or **richer simulation environments**.

---

## 📚 Bibliography

**Unity:**
- https://docs.unity.com/
- https://learn.unity.com/course/2d-beginner-adventure-game

**Map Generation:**
- https://www.youtube.com/playlist?list=PLcRSafycjWFenI87z7uZHFv6cUG2Tzu9v

**Grid System:**
- https://www.youtube.com/playlist?list=PLFt_AvWsXl0cq5Umv3pMC9SPnKjfp9eGW

**Others:**
- https://chatgpt.com/

