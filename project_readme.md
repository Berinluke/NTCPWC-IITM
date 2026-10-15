Real-Time Robot Telemetry & Monitoring System

A highly optimized Unity application designed to visualize real-time JSON telemetry data from multiple autonomous robots. The system features custom object pooling, throttled UI rendering, dynamic camera modes, and collision zone monitoring.

 Setup Steps

1. Prerequisites: Ensure you are using Unity 6000.x (or compatible version).
2. Layer Configuration: The project relies on the "Water" layer for collision detection and raycasting. Ensure your Robot prefabs and Proximity/Zone triggers are assigned to this layer.
3. Scene Setup:
   - Assign the `CameraController` to the Main Camera.
   - Attach `SelectionManager`, `UIManager`, `LevelManager`, and `DataCreation` to a persistent Manager GameObject in the scene.
   - Link required UI Text components and Panels to the `UIManager` via the Inspector.
4. Run Simulation: Press Play in edtior. The `DataCreation` script will automatically simulate incoming network telemetry.

  Controls

* W, A, S, D / Arrow Keys:** Move the camera forward, backward, left, and right (in Free Mode).
* Mouse Movement: Look around (in Free Mode).
* Left Mouse Click: Raycast select a robot in the scene. This will:
  * Open the Robot Detail UI panel.
  * Snap the camera into **Follow Mode**, locking onto the selected robot.
* 'C' Key: Exit Follow Mode, hide the Robot Detail UI, and return the camera to **Free Mode**.

  Architecture Summary

The project is built with a decoupled, data-driven architecture:

* `LevelManager.cs` (Singleton): The core router. Receives JSON strings, parses them without allocating new memory (`FromJsonOverwrite`), and dispatches data to the correct robot via an O(1) Dictionary lookup.
* `RobotCreation.cs`: Manages the Object Pool, instantiating a set number of robots at startup and recycling them to prevent runtime instantiation overhead.
* `RobotController.cs`: Handles individual robot logic, translating target coordinates into smooth vector movement (`MoveTowards`) and accurate procedural rotation (`LookRotation`). Tracks data-loss states via time deltas.
* `UIManager.cs`: Centralized UI handler. Manages Proximity Warnings, Restricted Zone Alerts, and Robot Details using throttled loops to save CPU cycles.
* `CameraController.cs` & `SelectionManager.cs`: Work together to handle user inputs, raycasting against the "Water" layer, and smoothly interpolating (`Lerp`) camera positions between free-roam and tracking states.
* `DataCreation.cs`:** Simulates a live data stream using Coroutines to generate and dispatch mock JSON telemetry payloads at random intervals.

  Performance Considerations & Optimizations

This project was engineered to heavily minimize Garbage Collection (GC) spikes and CPU overhead:

* String Pre-Allocation: The `DataCreation` script pre-calculates and arrays all possible Robot IDs and Names on `Start()`. This prevents the continuous creation of temporary strings during runtime JSON generation.
* Zero-GC Dictionary Keys: The `UIManager` utilizes custom `structs` (`RobotPair` and `ZoneKey`) as Dictionary keys instead of concatenated strings, completely eliminating heap allocation during warning lookups.
* StringBuilder UI: `UIManager` avoids standard string concatenation arrays in loops by utilizing a single, reusable `System.Text.StringBuilder` for rendering active warnings.
* Execution Throttling:
  * Heavy UI text updates are throttled via `Time.time` checks to run only 5 times per second (0.2s), rather than running every single frame.
  * `ProximeterSensor` similarly throttles distance calculations to 0.2s intervals.
* Optimized Math: `RobotController` utilizes `sqrMagnitude` instead of `Vector3.Distance` to check if a target is reached, bypassing expensive CPU square-root operations.

  Known Limitations

* Transform-Based Movement: `RobotController` currently uses `transform.position` in the `Update()` loop. If physical colliders (Rigidbodies) are heavily utilized in the future, this logic should be migrated to `FixedUpdate()` using `Rigidbody.MovePosition` to prevent physics interpolation jitter.
* Local Data Source: The telemetry network is currently mocked locally via `DataCreation.cs`. For production, this should be swapped for a network client (e.g., MQTT, WebSocket, UDP).
* Hardcoded Layer Masks: The Selection Manager and Sensors rely strictly on the `Water` layer index for targeting.