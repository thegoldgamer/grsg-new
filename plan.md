# Gold's Random Stuff Game — Dev Plan

> **Session note:** Plan was fully updated from the HTML doc at `C:\Users\gtost\Downloads\gold's random stuff game plan (main)\goldsrandomstuffgameplan_main_.html` — that is the source of truth for game design details. plan.md is the dev/implementation plan. Phase 1 is manual work. Phase 2 scripts are next — Claude Code writes all of them. No scripts have been written yet.

---

## Phase 1 (v0.1) — Player Rig & Setup
> *Done manually, not Claude Code's job*

- [ ] Make player rig
- [ ] Add player rig to XR Origin + simple grip animations
- [ ] Get XR interactables working (XRI 2.x setup)
- [x] Make models: Throttle, Steering Yoke, SFCD
- [ ] *yeeps break*

---

## Phase 2 (v0.3) — Core Scripts & Physics Test

> Claude Code writes all scripts in this phase.
> Goal: get a working test ship with throttle + yoke controlling a Rigidbody in full 6DOF.

### 2-A: Ship Brain & Modular Architecture (foundation everything else plugs into)

> Fully modular — each script is a self-contained component on its own GameObject. Minimum scripting per module. Adding a new ship = select a preset or configure manually.

- [ ] **Ship.cs** — placed on the **root ship GameObject**
  - **Preset dropdown** in the Inspector (e.g. Zenith, Toppat Airship, Orbital Station, Generic)
  - Selecting a preset **auto-configures** all fields and locks them (greyed out, no manual editing)
  - Selecting **Generic** unlocks all fields for full manual configuration
  - Defines ship-wide stats: max power, fuel cap, supported built-in features (InvisiTech, hyperdrive, etc.)
  - Single source of truth — all other modules reference Ship.cs

- [ ] **Engine.cs** — placed directly on the **engine GameObject**
  - Handles thrust logic and engine health
  - References its **Nacelle objects** (serialized list in Inspector — default 2 for the Zenith, configurable)
  - Takes input from Ship.cs and distributes power output to each Nacelle
  - Tracks engine health; degrades from E-Brake abuse or damage
  - Shuts down when ship power runs out
  - **Phase 2 implements basics only.** Full status list for later phases:
    - `online` / `offline` / `destroyed`
    - `c-offline` — triggered by engine discharge lever
    - `overridden` — via Overrider device; supercharges engines, hard-locks throttle + yoke, ship steers toward nearest star; no fix, abandon ship
    - `overheating` — shows % to thermal runaway
    - `thermal runaway` — engines loud, irregular, metal glowing red; voice modulator glitches on Type 3 AI; if sustained 3 min, Type 3 AI core is erased
    - `thermal runaway (fatal)` — engines in OVERDRIVE, no going back; ~1 min to quantum superdetonation of reactor core; explosion poisons nearby players

- [ ] **Nacelle.cs** — placed on each **individual booster/thruster GameObject**
  - Registered in Engine.cs (drag-and-drop in Inspector)
  - Receives power level from Engine.cs and responds (visuals, particle effects, force contribution)
  - Each nacelle is independent — one can fail without taking the others down
  - Shuts off when Engine.cs is offline
  - Extends outward when hyperdrive is active (visual indicator)
  - **Phase 2 implements basics only.** Full status list for later phases:
    - `online` / `offline` / `destroyed`
    - `overheating` — shows % to thermal runaway in Inspector; triggers after ~10 min in hyperdrive; accents turn dark neon red
    - `thermal runaway` — 5–8 min to explosion; if sustained 3 min, Type 3 AI core can be erased; ship sounds and feels like it's falling apart
    - `i-compromised` — integrity hit 0; unsafe to keep engines on; ship may need to be towed
    - `gone` — future: nacelles can physically break off or bend (physics-driven)

- [ ] **ShipController.cs** — Rigidbody physics layer, driven by Engine.cs
  - Exposes: `SetThrust`, `SetPitch`, `SetYaw`, `SetRoll`, `Brake`, `SetNeutral`, `SetPark`, `EBrake`
  - Tracks drive mode (DRIVE / REVERSE / NEUTRAL / PARK)
  - Pure physics — no game logic, just moves the Rigidbody

> *More modules TBD as development continues (e.g. FuelTank.cs, StabilityBooster.cs, HullIntegrity.cs, etc.)*

### 2-B: Throttle Script

- [ ] **ThrottleInteractable.cs** — placed on the **throttle handle model** (the part with the E-Brake button)
  - Serialized field: **Screen GameObject** — drag in whichever child object is the throttle's screen
  - Grabbable XR interactable, slides along a fixed rail axis
  - Position along rail maps to thrust output
  - **Aggressiveness** = speed of movement (delta) — fast push = aggressive burst of force; gradual push = gradual/accurate acceleration
  - **Resistance system** — throttle has physical resistance by design so it feels like a real object; resistance scales inversely with force applied: push/pull harder = feels lighter and faster, push/pull slower = feels heavier and more resistant; resistance is absent in neutral (feels like a toy — no longer connected to the engine)
  - Pull all the way back = engine stops outputting force + **gradually brakes** to a halt (`Brake()`)
  - Pull up = **NEUTRAL** — gliding, no engine force, no braking, stability boosters disabled, aggressiveness resets to 0 on return to drive
  - Rotate 180° = **REVERSE** — indicator below throttle turns yellow; speed not limited
  - **Push down** = **PARK** — throttle fully locked, cannot be moved at all; screen turns red
  - **B button (Quest 2/3)** while gripping = **E-Brake** (`EBrake()`) — emergency use only; hold to sustain; much faster stop than pulling back all the way; burns a lot of fuel and damages the engine over time
  - Gradient color indicator on screen: green (min force) → red (max force); **sky blue** = hyperdrive active, throttle locked at max
  - If power is out: throttle gets stuck (locked, no input processed)
  - If ship is **overridden** (see Overrider in Future Mechanics): throttle locks at full and cannot be moved; indicator light pulses red

### 2-C: Steering Yoke Script

- [ ] **YokeInteractable.cs**
  - W-shaped, grabbable with one or both hands (XRI 2.x)
  - Ship "drifts" toward where the yoke is held — hold it in a direction and it keeps going that way
  - **Push forward** → pitch nose down
  - **Pull back** → pitch nose up
  - **Rotate left/right (car-style)** → roll; more rotation = faster roll
  - **Right joystick** (while <50 knots) → yaw left/right; ship goes too fast to yaw while moving
  - **Left joystick** (while <100 knots) → ascend/descend (e.g. from a hangar); up = up, down = down; adaptive to joystick distance
  - **Left trigger** (both hands on yoke) → toggles left joystick to navigate yoke screen UI; right trigger to select — allows UI interaction while piloting
  - **Y or B button** when weapons armed → cycle between weapons
  - Dampened — soft resistance, no snap-back on release; stays roughly where left
  - Rotation only, no effect on thrust
  - Screen in the middle of the yoke — controls engine functions, InvisiTech, armed weapons, etc. (details TBD)

### 2-D: Simple SFCD Script

- [ ] **SFCD.cs**
  - Wearable item, goes on the non-dominant arm
  - Stores: owner username, captain flag
  - Used by voice modulator security system to identify authorized crew
  - Required for tractor beam call-up from Toppat Orbital Station (later)
  - Compatible with keycard scanners — opens doors without a physical ID
  - Foundation for future tool expansions (Remote Scan v1.2, Repair Tools v1.5)

### 2-E: Testing

- [ ] Hook ThrottleInteractable + YokeInteractable up to a simple test ship with ShipController
- [ ] Physics tests: acceleration, stopping, reverse, neutral glide, E-brake, park
- [ ] (opt) Add basic multiplayer — Photon Fusion or Realtime for syncing (~5 private players)
- [ ] Internal pass → give build to Bekham → adjust if needed

### 2-F: Models (make these manually, not Claude Code)

- [ ] Zenith model (engine, exterior, interior)
- [ ] Voice modulator model (Zenith variant)
- [ ] AI capsule model (Zenith variant)
- [ ] Hyperdrive model (TBD where it goes)

### 2-G: Additional Scripts (after models are in)

- [ ] **InvisiTech.cs**
  - Hides the exterior (including windows) of the attached GameObject on activation
  - Open doors while active = doorway not cloaked, only the door itself
  - Can be toggled via script, interactable, or yoke screen
  - Either a weldable item or built-in ship feature (Zenith has it built in)

- [ ] **VoiceModulator.cs**
  - AI "eyes and ears" — screens embedded on doorway tops or walls near the roof
  - Blue cone = analyze mode (put item near it to analyze)
  - Red cone = security scan mode (always active, rotates at origin, can't pass through walls, can see through glass)
  - If unauthorized person (no SFCD) detected: AI locks on → cold directive warning ("Stop moving and raise your hands above your head.") → trap beams if they run; if they surrender, moving even an inch = beamed
  - Goes offline if AI capsule is removed

- [ ] **AICapsule.cs**
  - Grabbable XR interactable
  - AI port on each ship = trigger zone + invisible snap (Zenith: main engine)
  - Insert into port → AI online, voice modulators activate
  - Remove gracefully (AI-ready) → clean shutdown
  - Remove unexpectedly → modulators cut abruptly + robotic warning:
    *"Warning, AI capsule removed unexpectedly. Please analyze the port in the engine room for any damage."*
    *(robotic fallback voice = placeholder for now, wire up later in Phase 3)*

- [ ] *yeeps break*

---

## Phase 3 (v0.5) — Zenith AI System

### 3-A: Core AI Pipeline (STT → LLM → TTS)

- [ ] **Whisper (STT)**
  - Wake word: `"computer"`
  - Records MP3 from wake word onward; stops on pause detection
  - Real-time transcription if possible
  - Multiple players supported — each player's audio stream is separated before LLM injection

- [ ] **LLM** (`gpt-oss:20b` via Groq API)
  - Structured prompt injection; script-generated system state injected automatically
  - Strict 1–2 sentence response limit (3 max only if absolutely necessary)
  - In-world tone only — no markdown, no code blocks, no out-of-character text
  - Some personality — professional ship AI, not purely robotic

- [ ] **Orpheus v1 (TTS)**
  - Receives LLM response (brackets stripped by command parser)
  - Voice output downloaded and played through voice modulators in the room

### 3-B: Command Parser

- [ ] **AICommandParser.cs** — parses bracket commands from LLM response using format `[system/action:value]`
- [ ] Bind to in-game systems:
  - Engines: `[engine/throttle:<0-100>]`, `[engine/shutdown]`, `[e-brake/engage]`, `[e-brake/disengage]`
  - Hyperdrive: `[hyperdrive/engage]`, `[hyperdrive/disengage]`, `[hyperdrive/prime]`, `[hyperdrive/abort]`
  - Shields + Defense: `[shields/raise]`, `[shields/lower]`, `[weapons/arm]`, `[weapons/disarm]`, `[countermeasures/deploy]`, `[tractorbeam/lock:]`, `[tractorbeam/release]`
  - Internal Systems: `[lights/on]`, `[lights/off]`, `[lights/sector:]`, `[temperature/set:]`, `[gravity/set:]`, `[oxygen/set:]`, `[doors/lock:]`, `[doors/unlock:]`
  - Sensors + Comms: `[sensors/scan:]`, `[sensors/analysis]`, `[comms/send:]`, `[comms/broadcast:]`, `[alerts:info]`, `[alerts:warning]`, `[alerts:critical]`
  - Memory: `[memory/log:]`, `[memory/summarize]`, `[memory/recall:]`
- [ ] Block AI from issuing unsupported commands
- [ ] Show received commands via on-screen text readout

### 3-C: Memory Persistence

- [ ] On all players leaving: AI generates memory summary → saved to per-ship `.txt` file
- [ ] Memory injected into next session context; summaries must be concise
- [ ] Isolated per ship — no cross-ship contamination
- [ ] Configurable memory file size in settings
- [ ] Chat cleared between sessions

### 3-D: Emergency Event System

- [ ] Auto-trigger events: engine failure, hull breach, shield collapse, reactor instability, hyperdrive malfunction, power surge, autopilot override, comms blackout, total system failure
- [ ] AI prioritizes captain unless another crew member is actively speaking
- [ ] AI issues immediate status + triggers relevant commands where possible

---

## Phase 4 (v0.6) — Zenith Model Integration & Polish

- [ ] Add all Zenith models into Unity
- [ ] Add nacelle movement handler — nacelles extend outward when hyperdrive is enabled
- [ ] Polish AI personality — immersive, some character, not robotic
- [ ] Latency optimization, async request queue for voice pipeline
- [ ] Multi-ship architecture — each ship gets its own AI instance, captain, and memory file
- [ ] Captain system — AI always says "Captain" (never username) for the captain; crew by nickname or username
- [ ] Single-instance enforcement for Toppat Airship and Toppat Orbital Station

---

## Technical Decisions

| Topic | Decision |
|---|---|
| XRI version | XRI **2.x** (legacy input) |
| Throttle interaction | Grab + **slide on fixed rail** |
| Aggressiveness | **Speed of movement (delta)** — fast push = burst, gradual = accurate |
| Throttle resistance | Inversely proportional to force applied — harder/faster = lighter feel, slower = heavier feel; absent in neutral |
| Throttle rotation | Player **physically twists** in VR |
| Throttle park | **Push down** on throttle |
| Throttle neutral | **Pull up** on throttle |
| E-Brake input | **Quest 2/3 B button** (right controller), hold to sustain — emergency only |
| Ship physics | Unity **Rigidbody** + AddForce |
| Ship movement | **Full 6DOF** |
| AI capsule port | Trigger zone proximity → invisible snap |
| Yoke type | W-shape, one or two hands, **dampened** |
| Yoke pitch | Push forward = nose down, pull back = nose up |
| Yoke roll | **Car-style left/right rotation** — more rotation = faster roll |
| Yoke yaw | **Right joystick** at <50 knots only |
| Yoke vertical | **Left joystick** at <100 knots (hangar use); adaptive to stick distance |
| Yoke UI nav | **Left trigger** (both hands on yoke) → left joystick navigates screen, right trigger selects |
| Yoke screen | Controls engine functions, InvisiTech, armed weapons, etc. (TBD) |
| Multiplayer | Photon Fusion or Realtime (~5 private players, opt for Phase 2) |
| AI stack | Whisper (STT) → gpt-oss:20b via Groq (LLM) → Orpheus v1 (TTS) |
| Groq API keys | Isolated per subsystem (STT / LLM / TTS), rate limits managed independently |

---

## Notes & TBDs

- Tilt axis on yoke (roll) = TBD once it's in-hand
- Throttle + yoke may be merged into one object — TBD
- Communications system on Zenith = TBD, skip for now
- Galactech / Landent Globe placement = TBD, skip for now
- Going down in Toppat Airship = TBD, suggestions appreciated
- Campaign mode = **scrapped**
- Hyperdrive adapter for non-compatible ships = TBD
- SFCD tool expansions: Remote Scan (v1.2), Repair Tools — Weld / Repair / Laser (v1.5)

---

## 🚢 Ships Overview

### The Zenith *(v1 — release)*
- Full 6DOF Rigidbody ship; primary flagship
- Built-in: InvisiTech, hyperdrive, gravity generator, life support, collision course detection, emergency separation, battle artillery, voice modulators, AI computer
- **Emergency Separation** — engines split from main hull; both driveable independently; lower max power but less energy draw
- **Collision Course Detection** — AI bridge notified; right hologram on captain's station shows predicted collision path
- **Gravity Generator** — toggleable via computer/stations/yoke; malfunctions on fatal damage (wall-slam + HP loss)
- **Life Support / Oxygen Generator** — cannot be turned off remotely; must go to the unit and manually cut power
- **Battle Artillery:**
  - Shutdown beam — fries anything electric until repaired; Toppat Orbital Station's central core is vulnerable if exposed
  - Trap beam — grabs anything; more power = more weight capacity = faster power drain
  - Basic lasers (TBD)
  - Zenith is shutdown-resistant — shutdown bombs only hurt maneuverability or cause a temporary shutdown, not a full fry
- **Galactech / Landent Globe** — must travel to another multiverse to activate; plug into fuel cell to enable Galactech mode; can only be upgraded to Landent Globe by a privileged user

### Toppat Orbital Station *(v1.1 — the toppat update)*
- Single instance only
- **Launch sequence** — boosters are primary propulsion; no weapons/artillery, no Collision Course Detection, no Supreme Dominance/Big Bomb until fully in space; InvisiTech not built-in (must find item; only works in space); escape pods unavailable during launch
- **In space** — limited tilt-only propulsion (orbit mode); weapons online
- **Artillery:** Supreme Dominance (huge destructive beam, countered by reflective objects), Big Bomb (no known counter 💀), basic lock-on lasers (infinite ammo, inaccurate); lock-on auto-tilts station (very slow)
- **Tractor Beam** — SFCD required to call it from the surface; beams up AND down; pulls everything in the beam including other players; high energy but central core = infinite energy
- **Gravity Generator** — cannot be turned off normally; must destroy or tamper with the gravity chamber to weaken it

### Toppat Airship *(v1.1 — the toppat update)*
- Single instance only
- Push throttle up to take off; stays airborne if throttle drops back down; going down = TBD
- Steering like a car (turn left/right)
- **Drill Pods** — launched from cockpit to any collision-detected surface; devastating on impact with drill on; still dangerous without it

---

## 🔮 Future Mechanics (logged, not doing yet)

### Manual Engine Discharge Lever
- A physical lever on the ship that **hard cuts** the engine — not a soft shutdown, a full kill
- Immediately stops all power output
- To restart: must wait through a full engine restart sequence (not just toggle off/on)
- Can be **overridden** (see Overrider below) — lever becomes useless when overridden
- Script TBD (probably `EngineDischarge.cs` or a mode in `Engine.cs`)

### The Overrider
- A small ball-shaped device — can be **carried by a player** or launched to act **autonomously**
- Has a payload of **laser balls** it fires one at a time
  - Laser balls silently open locked doors with **no alert triggered**
  - It uses these to creep through the ship toward the engine room
- Once it reaches the engine room, it **dumps its entire payload into the engines**
  - This **supercharges** the engines — full speed, uncontrollable
  - **Locks all controls** — throttle locks at full with pulsing red indicator light, yoke frozen, discharge lever useless
- **No counter** — you cannot fix it, disassembling the engine piece by piece does nothing
- Engine enters an **irreversible state** — the ship is permanently cooked, cannot be repaired
- Only option: **bail out and abandon ship** 💀
- Must travel to the nearest **ship provider** to get a new one (hitchhike to get there — TBD)
- Scripts TBD: `Overrider.cs`, `LaserBall.cs` (autonomous pathfinding probably via NavMesh or custom)
