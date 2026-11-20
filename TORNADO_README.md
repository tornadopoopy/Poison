# Tornado Menu (Poison fork)

This branch adapts the open-source Poison Gorilla Tag menu to the requested Tornado Menu layout.

## PCVR operation

- **Controller:** Quest 3 / 3S through PCVR.
- **Open/close:** press **Y** once on the left Touch controller; press Y again to close.
- **Placement:** the existing Poison 3D board follows your **left hand / wrist**.
- **Selection:** use the **right-hand pointer** to touch menu buttons.
- **Appearance:** the original Poison 3D menu structure, page arrows and FPS indicator, but with a blue primary theme and **Tornado Menu** title.
- **Rooms:** the menu is gated on Utilla's modded-gamemode events. It stays closed by default and resets enabled menu toggles when a modded gamemode is exited.

## GitHub-hosted build — private dependencies

The `.github/workflows/build.yml` workflow runs on GitHub-hosted Windows. It downloads a ZIP of the user's local reference assemblies from a **private** repository.

One-time owner setup:

1. Create a **private** repository named `tornadopoopy/tornado-menu-build-deps` with a `main` branch.
2. Upload `TornadoBuildReferences.zip` into that private repository's root (do **not** upload it to this public repository).
3. Create a GitHub fine-grained personal access token with **Contents: Read-only**, restricted to just `tornadopoopy/tornado-menu-build-deps`.
4. In `tornadopoopy/Poison` → Settings → Secrets and variables → Actions, create secret `TORNADO_DEPS_TOKEN` with the token as its value.
5. Rerun `Build Tornado Menu` from the Actions tab or manually dispatch `build.yml`.

The ZIP must contain `References/Managed/*.dll`, `References/core/*.dll`, and `References/Utilla/Utilla.dll`. The ZIP is for private build dependency storage; do not distribute it publicly. GitHub-hosted builds do not have local game installations, so missing references are expected until this setup is complete.

## Build

Install BepInEx 5 (Mono) and a compatible Utilla mod in your Gorilla Tag PC installation. The project expects `BepInEx/plugins/Utilla/Utilla.dll`. Update `Directory.Build.props` if your game is not in the default Steam directory.

Then compile:

```powershell
dotnet build PoisonMenu.csproj -c Release
```

Look under `bin/Release/netstandard2.1/` for `TornadoMenu.dll`. Put **only the resulting plugin DLL** in `Gorilla Tag/BepInEx/plugins`.

Do **not** install the old TornadoMenu v1.x DLL and this Poison-based menu at the same time. They may conflict.

## Scope and testing

This branch is a source-level implementation, **not** a verified in-game build. It has not been tested with the current Quest 3/3S PCVR game build. Utilla events are used for room gating; this is not an anti-cheat bypass or a guarantee that all Poison features are safe, functional or permitted in every modded room. Some Poison code includes independent patches and network services, which merit a separate audit before public release.

## Source and license

Based on **Poison Menu** by Poison Software and its contributors:
https://github.com/heycanihavethis/Poison

Source and modifications remain licensed under **GPL-3.0-or-later**. Keep the original repository license and copyright notices. Poison's brand names and artwork are not included in the code license; use your own branding for published forks.
