*Read this in other languages: [English](README.md), [Čeština](README.cs.md).*

[![Download](https://img.shields.io/badge/Download_PC-itch.io-FA5C5C?style=for-the-badge&logo=itch.io)](https://chroustek.itch.io/drone-assembly-simulator)
[![Download](https://img.shields.io/badge/Download_VR-itch.io-FA5C5C?style=for-the-badge&logo=itch.io)](https://chroustek.itch.io/droneassembly-vr)
[![Made with Unity](https://img.shields.io/badge/Made_with-Unity_2022+-000000.svg?style=for-the-badge&logo=unity)](https://unity.com/)
[![Powered by Azure](https://img.shields.io/badge/Powered_by-Azure_SQL-0089D6?style=for-the-badge&logo=microsoft-azure)](https://azure.microsoft.com/)
[![Backend: PHP](https://img.shields.io/badge/Backend-PHP_8.2-777BB4?style=for-the-badge&logo=php)](https://www.php.net/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)](LICENSE)

# Drone Assembly Simulator

**Drone Assembly Simulator** is a 3D interactive simulation built in Unity (Playable on PC and VR), where the player assembles a drone by snapping individual parts together in the correct order. Upon completing the assembly, the player's time is submitted to a **global online leaderboard** — backed by a full cloud infrastructure deployed on Microsoft Azure.

This project was created as a portfolio piece, showcasing a complete **3-tier cloud architecture**: a Unity game client, a custom PHP REST API, and a cloud-hosted MySQL database — all provisioned with Terraform.

[![YouTube](https://img.shields.io/badge/YouTube-Video_Showcase-red?style=for-the-badge&logo=youtube)](https://youtu.be/AfFFgCSjMrA)
<p align="center">
  <img src="Media/SnapPartGifoptimize.gif" alt="Drone Assembly Gameplay" width="800"/>
</p>

<p align="center">
  <img src="Media/Drone_Ghost_preview.png" alt="Ghost Preview" width="390"/>
  &nbsp;
  <img src="Media/Drone_closeup.png" alt="Drone Close-up" width="390"/>
</p>



## Architecture & Tech Stack

```
[ Unity Client (C#) ]
        │
        │  HTTPS + SHA-256 signed payload
        ▼
[ PHP REST API — Azure App Service ]
        │
        │  PDO / MySQL
        ▼
[ MySQL Flexible Server — Azure ]
```

| Layer | Technology |
|---|---|
| Game Client | Unity 2022+, C#, ShaderLab, HLSL |
| REST API | PHP 8.2, PDO |
| Database | Azure MySQL Flexible Server 8.0 |
| Infrastructure | Terraform, Azure App Service (Linux) |
| Security | SHA-256 request signing, `hash_equals()` |

### API Endpoints

| Endpoint | Method | Description |
|---|---|---|
| `save_score.php` | `POST` | Submits a new score with a signed hash |
| `get_top10.php` | `GET` | Returns the top 10 leaderboard entries |

---

## Other Technical Highlights
* **Automated CI Pipeline:** Configured GitHub Actions for headless Unity builds, secure secret management, and automated unit testing on every push/PR to ensure production stability.
* **Data-Driven Architecture:** Extensive use of Scriptable Objects (Procedure SO) to allow rapid iteration of assembly logic without touching the codebase.
* **Custom Editor Tools:** To help with level design and logic testing.
* **API Rate Limiting:** Implemented custom IP-based request throttling (Cooldown for POST, Fixed Window for GET) to protect the endpoints against abuse and DDoS.
* **Asynchronous Networking:** Utilizing modern C# async/await Tasks for all API calls to ensure smooth UI thread performance without callback hell.

## Credits
* **3D Drone Model:** Created by *Angel* on GrabCAD.
* **Background Music:** Created by *Kuzu* (Pixabay).
* **Sound Effects:** Created by *DRAGON-STUDIO* and *Klemen Flerin* (Pixabay).
