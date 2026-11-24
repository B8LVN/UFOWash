# UFO WASH — Asset Register

Every non-original asset added to the project must be recorded here before the milestone is accepted.

| Asset ID | Name / description | Type | Source URL | Creator / vendor | License / terms | Cost | Files / GUID | Intended use | Status |
|---|---|---|---|---|---|---|---|---|---|
| ASSET-001 | Kenney Space Kit 2.0 — spacecraft, alien, station props and isometric sprite assets | 3D FBX/DAE models and 2D PNGs | https://kenney.nl/assets/space-kit | Kenney | CC0 1.0; included `License.txt` explicitly permits personal, educational and commercial use; attribution optional | Free | `Assets/UFO_WASH/Art/Imported/KenneySpaceKit/` | Imported spacecraft, alien customer, and service generator/pump/relay/chemical-drum dressing | Existing CC0 models are used for customer and player-facing service props; Unity import and runtime appearance still require review |
| ASSET-002 | Kenney Modular Space Kit 1.0 — 40 modular rooms, corridors, gates and station pieces | 3D FBX models + color atlas | https://kenney.nl/assets/modular-space-kit | Kenney | CC0 1.0; included `License.txt` permits commercial use and modification; attribution optional | Free | `Assets/UFO_WASH/Art/Imported/KenneyModularSpaceKit/` | Vesper outpost and the saved, two-tier Bay 01 recovery annex | Included CC0 room variants are now used by the purchasable bay upgrade in builder source; Unity import, saved-scene appearance and in-Play visual review remain unverified |
| ASSET-003 | Original local coating shader and generated pressure loop | Shader source / runtime audio synthesis | Original project work; no downloaded recording | UFO WASH project | Original source, no third-party asset license added | Free | `Scripts/Cleaning/UfoWashLocalCoating.shader`; `Scripts/Equipment/UfoWashToolAudio.cs`, under `Assets/UFO_WASH/`; matching metadata added | Experimental local grime/foam and water/foam/rinse feedback | Source implemented; appearance, loudness and runtime cleanup unverified in Unity |
| CANDIDATE-004 | Modular Sci-Fi MegaKit — 277 modular sci-fi models | FBX/OBJ/Blend/glTF models | https://quaternius.com/packs/modularscifimegakit.html | Quaternius | CC0, verified on the pack page; commercial use and modification allowed | Free | Not downloaded | Candidate replacement/detail kit for a purpose-built Vesper garage | Researched; not imported |
| CANDIDATE-005 | Ultimate Space Kit — 92 models | FBX/OBJ/Blend/glTF models | https://quaternius.com/packs/ultimatespacekit.html | Quaternius | CC0, verified on the pack page | Free | Not downloaded | Candidate ship silhouettes, outpost props and alien-world set dressing | Researched; not imported |
| CANDIDATE-006 | Stylized Nature MegaKit — 116 models | FBX/OBJ/Blend/glTF models | https://quaternius.com/packs/stylizednaturemegakit.html | Quaternius | CC0, verified on the pack page | Free | Not downloaded | Candidate alien-planet plants and rocks, subject to matching Vesper's palette | Researched; not imported |
| CANDIDATE-007 | Poly Haven 3D models, HDRIs and PBR textures | 3D models, HDRIs and texture sets | https://polyhaven.com/models | Poly Haven | CC0, verified on official license page | Free | Not downloaded | Candidate scanned basalt, environment lighting and high-detail surface materials | Researched; not imported |
| CANDIDATE-008 | ambientCG materials | PBR texture sets | https://ambientcg.com/ and https://ambientcg.com/license | ambientCG | CC0 1.0, verified on official license page; commercial use allowed | Free | Not downloaded | Candidate painted metal, concrete, rock and contamination textures | Researched; not imported |

Candidates are not project dependencies. Select and stage only the pieces that fit
the approved Vesper art direction; inspect source licenses again on the exact
download and keep the supplied license files with imported content.

## Asset approval checklist

- Source URL is recorded and accessible.
- License explicitly permits intended commercial use.
- Attribution requirements are recorded.
- Redistribution/modification restrictions are understood.
- Asset is visually consistent with the approved art direction.
- Asset is optimized and configured for Unity/URP.
- Asset has a stable local path and correct `.meta` file.
- Any cost was approved by the owner before purchase.

Do not use assets with unclear rights. Do not assume “free download” means unrestricted commercial use.
