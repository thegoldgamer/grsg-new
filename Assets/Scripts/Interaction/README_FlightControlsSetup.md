# 🚀 VR Flight Controls Setup Guide (Blockout Template)

This guide explains how to set up your `YokeInteractable` and `ThrottleInteractable` scripts in Unity. Because you're currently taking a break from 3D modeling (which is totally fair!), this guide includes a **Blockout Hierarchy** using basic Unity 3D objects (Cubes, Cylinders, etc.) so you can build a working test ship right now.

---

## 🧠 The Golden Rule: Separate Physics from Visuals
When working with VR interactables—especially complex ones with pivots like a Yoke—it is highly recommended to separate the object you **physically grab** from the **visual model**. 

If you put the physics joint directly on a nested visual model, Unity's physics engine can stretch, flatten, and deform your meshes (especially if any parent object has a scale other than `1, 1, 1`). 

**The Solution:** We create an *Invisible Physics Object* that handles the collision, grabbing, and joints. The script then reads the rotation/position of that invisible object and cleanly applies it to your visual models.

---

## 🕹️ 1. YOKE SETUP (Pitch, Roll, Yaw)

### The Blockout Hierarchy (Using Unity Primitives)
Create these using `GameObject > 3D Object`. Ensure all scales are ideally set to `1, 1, 1` where possible, adjusting size using the Mesh dimensions or childing them to a uniformly scaled parent.

```text
🚢 Ship Hull (Empty GameObject or big Cube)
 │
 ├── 📦 YokeVisuals (Empty)
 │    ├── 🟫 YokeBase (Cube - Placed on the floor)
 │    │    └── 🛢️ YolkForwardBackwardPivot (Cylinder rotated sideways - Handles Pitch)
 │    │         └── 🛢️ ScreenLeftRightRotatePivot (Cylinder pointing forward - Handles Roll)
 │    │              └── 🟫 YolkScreen (Cube - The main display board)
 │    │                   ├── 💊 LeftHandleGrip (Capsule)
 │    │                   └── 💊 RightHandleGrip (Capsule)
 │
 └── 👻 YokePhysicsInteractable (Cube - Turn OFF MeshRenderer so it's invisible!)
```

### Setting up the Invisible Physics Object (`YokePhysicsInteractable`)
1. Create a Cube, place it exactly where the `YolkScreen` is.
2. Turn **OFF** its `MeshRenderer`.
3. Add a **Rigidbody**. (Set Mass to `5`, Drag to `2`, Angular Drag to `5`, Use Gravity to `false`).
4. Add a **Configurable Joint**.
   - Connect the joint to the `Ship Hull`'s Rigidbody (if your ship moves).
   - **X, Y, Z Motion**: `Locked` (It shouldn't move around).
   - **Angular X Motion**: `Limited` (This is Pitch. Set High/Low limit to `45`).
   - **Angular Y Motion**: `Locked` (No Yaw physically twisting).
   - **Angular Z Motion**: `Limited` (This is Roll. Set limit to `90`).
5. Add the **YokeInteractable** script. (This automatically adds `XRGrabInteractable`).

### Script Configuration (`YokeInteractable`)
- **Yolk Screen:** Drag your visual `YolkScreen` here.
- **Screen Left Right Rotate Pivot:** Drag `ScreenLeftRightRotatePivot` here.
- **Yolk Forward Backward Pivot:** Drag `YolkForwardBackwardPivot` here.
- **Controller:** Drag your `ShipController` here.
- **Yaw / Vertical / Toggle UI Actions:** Bind these to your XR controller thumbsticks/triggers.
- **Update Decor Pivots:** Keep this checked `True`.

---

## 🎚️ 2. THROTTLE SETUP (Drive, Reverse, E-Brake)

### The Blockout Hierarchy (Using Unity Primitives)
```text
🚢 Ship Hull
 │
 ├── 📦 ThrottleVisuals (Empty)
 │    ├── 🟫 ThrottleFrame (Cube - The housing base)
 │    │    ├── 📍 RailStartPoint (Empty - Placed at the back of the frame)
 │    │    ├── 📍 RailEndPoint (Empty - Placed at the front of the frame)
 │    │    └── 🛢️ ThrottleConnectorDown (Cylinder/Tube - The base hole)
 │    │
 │    └── 🟫 VisualThrottleHandle (Cube - The part that slides)
 │         ├── 🛢️ ThrottleConnectorUp (Cylinder sliding into the down connector)
 │         ├── 💊 ThrottleGrip (Capsule)
 │         └── 🟥 EBrakeButton (Small Red Cube)
 │
 └── 👻 ThrottlePhysicsInteractable (Cube - Turn OFF MeshRenderer!)
```

### Setting up the Invisible Physics Object (`ThrottlePhysicsInteractable`)
1. Create a Cube, size it to cover the slide path of the throttle, place it over the handle.
2. Turn **OFF** its `MeshRenderer`.
3. Add a **Rigidbody**. (Set Mass to `2`, Use Gravity to `false`).
4. Add a **Configurable Joint**.
   - Connect it to the `Ship Hull` Rigidbody.
   - **X and Y Motion**: `Locked`.
   - **Z Motion**: `Limited` (This allows it to slide forward and back. Set the limit to match your rail length).
   - **Angular X, Y, Z Motion**: `Locked` (The throttle shouldn't twist).
5. Add the **ThrottleInteractable** script.

### Script Configuration (`ThrottleInteractable`)
- **Throttle Handle:** Drag the invisible `ThrottlePhysicsInteractable` here (or the `VisualThrottleHandle` if you child it to the physics object).
- **Throttle Frame:** Drag `ThrottleFrame`.
- **Rail Start/End Point:** Drag your two empty GameObjects. *(Make sure they are children of the Frame so they don't move!).*
- **Controller:** Drag your `ShipController`.
- **E Brake Action:** Bind to your controller's face button (B/Y).

---

## 🛠️ Pro Tips for Blockouts
*   **Colors Help!** Create simple Unity Materials (Right Click in Project > Create > Material), give them solid colors (Red, Blue, Grey, Yellow), and drag them onto your cubes. It makes testing 100x easier.
*   **Check your Colliders:** The visual meshes don't need colliders if you are using the invisible physics objects to grab them. You can remove `BoxCollider` from the visual parts to save performance and prevent accidental physics bumps.
*   **The Joint Axis:** Configurable joints can be tricky. If your throttle slides sideways instead of forward, rotate the invisible physics object by 90 degrees until the Joint's Z-axis lines up with your rail.