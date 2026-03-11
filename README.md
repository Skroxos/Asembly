#  Drone Assembly Simulator

[![Download](https://img.shields.io/badge/Download-itch.io-FA5C5C?style=for-the-badge&logo=itch.io)](https://chroustek.itch.io/drone-assembly-simulator)
[![Made with Unity](https://img.shields.io/badge/Made_with-Unity_2022+-000000.svg?style=for-the-badge&logo=unity)](https://unity.com/)
[![Powered by Azure](https://img.shields.io/badge/Powered_by-Azure_SQL-0089D6?style=for-the-badge&logo=microsoft-azure)](https://azure.microsoft.com/)

**Drone Assembly Simulator** je 3D simulátor vytvořený v Unity, kde si hráči mohou sestavit vlastního drona. Projekt obsahuje plně funkční cloudový backend s globálním online žebříčkem.

##  Hlavní funkce

* **Sestavování dronů:** Interaktivní skládání komponentů s přesným pozicováním.
* **Globální Online Leaderboard:** Hráči po dokončení trati odesílají své časy (s přesností na setiny sekundy) na server a soupeří s ostatními hráči po celém světě.

##  Architektura a Technologie

Tento projekt není jen klientská hra, ale obsahuje kompletní vlastní backendovou infrastrukturu:

* **Herní Engine:** Unity (C#)
* **Databáze:** Microsoft Azure SQL Database (hostováno v cloudu).
* **Webové API:** Vlastní Azure App Service fungující jako most mezi Unity a databází.
* **Zabezpečení:** Skóre odesílané z Unity je zabezpečeno pomocí SHA256 hashe s tajným klíčem.

