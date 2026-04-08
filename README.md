*Read this in other languages: [English](README.md), [Čeština](README.cs.md).*

[![Download](https://img.shields.io/badge/Download-itch.io-FA5C5C?style=for-the-badge&logo=itch.io)](https://chroustek.itch.io/drone-assembly-simulator)
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



## Features
-  **Interactive Drone Assembly** — snap drone parts together with precise 3D positioning
-  **Global Online Leaderboard** — submit and compare completion times with players worldwide
-  **Tamper-proof Score Submission** — scores are signed with a SHA-256 hash and a secret key before being sent from Unity
-  **Rate Limiting** — API endpoints are protected against abuse with per-IP request throttling
-  **Full Cloud Deployment** — entire infrastructure provisioned via Terraform on Microsoft Azure

## Architecture & Tech Stack
**Game Engine:** Unity (C#)
* **Database:** Microsoft Azure SQL Database (Cloud-hosted).
* **Web API:** Custom Azure App Service acting as a bridge between Unity and the database.


## Credits
* **3D Drone Model:** Created by *Angel* on GrabCAD.
* **Background Music:** Created by *Kuzu* (Pixabay).
* **Sound Effects:** Created by *DRAGON-STUDIO* and *Klemen Flerin* (Pixabay).
