*Tento text je k dispozici také v dalších jazycích: [English](README.md), [Čeština](README.cs.md).*

[![Download](https://img.shields.io/badge/Download_PC-itch.io-FA5C5C?style=for-the-badge&logo=itch.io)](https://chroustek.itch.io/drone-assembly-simulator)
[![Download](https://img.shields.io/badge/Download_VR-itch.io-FA5C5C?style=for-the-badge&logo=itch.io)](https://chroustek.itch.io/droneassembly-vr)
[![Made with Unity](https://img.shields.io/badge/Made_with-Unity_2022+-000000.svg?style=for-the-badge&logo=unity)](https://unity.com/)
[![Powered by Azure](https://img.shields.io/badge/Powered_by-Azure_SQL-0089D6?style=for-the-badge&logo=microsoft-azure)](https://azure.microsoft.com/)
[![Backend: PHP](https://img.shields.io/badge/Backend-PHP_8.2-777BB4?style=for-the-badge&logo=php)](https://www.php.net/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)](LICENSE)

# Drone Assembly Simulator

**Drone Assembly Simulator** je 3D interaktivní simulace vytvořená v Unity (Hratelná na PC i VR), kde hráč sestavuje dron skládáním jednotlivých dílů ve správném pořadí. Po dokončení sestavení je hráčův čas odeslán do **globálního online žebříčku** — podpořeného plnou cloudovou infrastrukturou nasazenou na Microsoft Azure.

Projekt byl vytvořen jako portfolio piece, demonstrující kompletní **třívrstvou cloudovou architekturu**: herní klient v Unity, vlastní PHP REST API a cloudově hostovaná MySQL databáze — vše zřízeno pomocí Terraformu.

[![YouTube](https://img.shields.io/badge/YouTube-Video_Showcase-red?style=for-the-badge&logo=youtube)](https://youtu.be/AfFFgCSjMrA)
<p align="center">
  <img src="Media/SnapPartGifoptimize.gif" alt="Drone Assembly Gameplay" width="800"/>
</p>

<p align="center">
  <img src="Media/Drone_Ghost_preview.png" alt="Ghost Preview" width="390"/>
  &nbsp;
  <img src="Media/Drone_closeup.png" alt="Drone Close-up" width="390"/>
</p>

---

## Architektura a Technologie

```
[ Unity Client (C#) ]
        │
        │  HTTPS + SHA-256 podepsaný payload
        ▼
[ PHP REST API — Azure App Service ]
        │
        │  PDO / MySQL
        ▼
[ MySQL Flexible Server — Azure ]
```

| Vrstva | Technologie |
|---|---|
| Herní klient | Unity 2022+, C#, ShaderLab, HLSL |
| REST API | PHP 8.2, PDO |
| Databáze | Azure MySQL Flexible Server 8.0 |
| Infrastruktura | Terraform, Azure App Service (Linux) |
| Bezpečnost | SHA-256 podepisování požadavků, `hash_equals()` |

### API Endpointy

| Endpoint | Metoda | Popis |
|---|---|---|
| `save_score.php` | `POST` | Odešle nové skóre s podepsaným hashem |
| `get_top10.php` | `GET` | Vrátí top 10 záznamů žebříčku |

---

## Další technické detaily

* **Automatizovaná CI Pipeline:** Nakonfigurované GitHub Actions pro headless Unity buildy, bezpečnou správu secrets a automatické unit testování při každém push/PR pro zajištění stability produkční verze.
* **Datově řízená architektura:** Rozsáhlé využití Scriptable Objects (Procedure SO) pro rychlou iteraci logiky sestavování bez zásahu do kódu.
* **Vlastní nástroje editoru:** Pro pomoc s návrhem levelů a testováním logiky.
* **Rate Limiting API:** Implementované vlastní IP-based omezování požadavků (Cooldown pro POST, Fixed Window pro GET) k ochraně endpointů před zneužitím a DDoS.
* **Asynchronní síťová komunikace:** Využití moderních C# async/await Tasks pro všechna API volání, aby bylo zajištěno plynulé výkon UI vlákna bez callback hell.

---

## Credits

* **3D model dronu:** Vytvořil *Angel* na platformě [GrabCAD](https://grabcad.com/).
* **Hudba na pozadí:** Vytvořil *Kuzu* ([Pixabay](https://pixabay.com/)).
* **Zvukové efekty:** Vytvořili *DRAGON-STUDIO* a *Klemen Flerin* ([Pixabay](https://pixabay.com/)).

---

## Licence

Tento projekt je licencován pod [MIT licencí](LICENSE).
