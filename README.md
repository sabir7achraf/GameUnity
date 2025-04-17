# Université Abdelmalek Essaâdi  
## Faculté des Sciences & Techniques de Tanger  
### Département Génie Informatique  

---

# 📝 REPORT: Ruby Adventure Game

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

### 2. Visual Studio IDE
Visual Studio is an IDE developed by Microsoft for building apps, web services, and more, using .NET technologies.

### 3. C#
C# is an object-oriented programming language developed by Microsoft. It is widely used with Unity for game development.

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



### 3. Generate Enemies and Coins Randomly
Enemies and coins were placed using random coordinates within room bounds:
```csharp
float randomX = Random.Range(roomBounds.min.x + 4, roomBounds.max.x - 4);
float randomY = Random.Range(roomBounds.min.y + 4, roomBounds.max.y - 4);

```
### 4. Generate Player and Boss with Map

- **Player** is placed in the first room.
- **Boss** is placed in the room farthest from the player by calculating Euclidean distances.

---

## 🧠 Pathfinding Algorithms

### 1. Grid of Nodes
- Implemented via `GridA` and `Node` classes.
- Grid is made of **walkable** and **unwalkable** nodes.
- Uses `Physics2D.OverlapCircle()` to detect obstacles.

### 2. A* Algorithm
- Efficient pathfinding (not always optimal):
  - Maintains **open/closed** lists.
  - Considers neighboring nodes and updates scores accordingly.
  - Stops upon reaching the goal.

#### A. Implementation
- `Pathfinding` class:
  - Uses `GridA` to find shortest path from Player to Boss.
  - Initialized in the `Awake()` method.

#### B. Follow A*
- `FollowPathAstar` class:
  - Moves the player along the A* path.
  - Uses raycasting to detect and avoid enemies.

### 3. Dijkstra Algorithm
- Finds shortest path to **all nodes** from a single source.

#### A. Implementation
- `Dijkstra` class:
  - Uses sets to track **visited/unvisited** nodes.
  - Calculates **cost** and **parent pointers**.
  - Reconstructs path from Boss to Player.

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

