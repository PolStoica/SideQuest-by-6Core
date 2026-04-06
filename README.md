# 🗺️ SideQuest

**Digital solution for coordinating proximity-based activities**

Although digital communication platforms are omnipresent, organizing a simple physical activity (like a tennis match or a board game night) remains logistically difficult. **SideQuest** is a solution created by the 6Core team that eliminates the barriers between intention and action, transforming the digital environment into an engine for social activation. The platform proposes an interaction model based on proximity and common interests.

## 🚀 Main Features
* **Proximity Exploration:** An interactive map that uses real-time geographical data to display nearby events.
* **Instant Connection:** Reduces time wasted in long chat group negotiations by transparently displaying availability.
* **Coordination Simplicity:** Centralizes essential details (location, time, number of participants) in a standardized format.
* **Matchmaking System:** Social profiles with clearly defined interests and experience levels to ensure efficient matchmaking between users.

## 🛠️ Tech Stack
The project uses an N-tier architecture for optimal modularity and scalability:

* **Presentation Layer (UI):** Blazor Server with SignalR for instant updates on the interactive map.
* **Business Logic Layer (BLL):** .NET 10 LTS (C# 14) for validation rules and the matchmaking algorithm.
* **Data Access Layer (DAL):** Entity Framework Core (Repository Pattern) connected to an SQLite database (configured in WAL mode for concurrency).

## 👥 Team 6Core
* **Stoica Paul-Isaac** - Team Lead / Fullstack Developer
* **Bonațiu Alexandru** - QA Tester
* **Pop Sebastian** - Database Administrator
* **Goga Alexandru** - Frontend Developer
* **Gavrău Bogdan** - Backend Developer
* **Bobon Valentin** - Backend Developer
