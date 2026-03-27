# Enemy Wave Spawn System

## Purpose
This system controls enemy wave spawning with two rules:

- enemiesToSpawn = total enemies that should appear in the wave.
- maxEnemies = maximum enemies alive at the same time.

When one active enemy is defeated, the system spawns the next pending enemy until the wave is fully consumed.

## Main Scripts and Responsibilities

### 1) Horde Trigger
File: Assets/Scripts/Hordes/hordeEnemyTrigger.cs

Responsibilities:
- Detect when the player enters the horde trigger collider.
- Raise the spawn event with three values:
  - enemiesToSpawn
  - maxEnemies
  - triggerObj

Important fields:
- enemiesToSpawn: total enemies in this horde.
- maxEnemies: concurrent alive enemy cap.
- triggerObj: object that gets disabled after activation.
- triggerEffect: ScriptableObject event used to notify listeners.

### 2) Spawn Event (ScriptableObject)
File: Assets/Scripts/Events/EventSOEnemySpawn.cs

Responsibilities:
- Keep a list of listeners.
- Broadcast spawn data to all registered listeners.

Method used:
- Occurred(int enemiesToSpawn, int maxEnemies, GameObject triggerObj)

### 3) Spawn Event Listener
File: Assets/Scripts/Events/EnemySpawnListener.cs

Responsibilities:
- Register and unregister to the ScriptableObject event.
- Forward event payload to a UnityEvent response.

Method used:
- invokeEvent(int enemiesToSpawn, int maxEnemies, GameObject triggerObj)

### 4) Spawn Coordinator
File: Assets/Scripts/Hordes/SpawnEnemies.cs

Responsibilities:
- Disable the trigger object after activation.
- Call EnemyPoolManager.StartWave with enemiesToSpawn and maxEnemies.
- Use an optional custom spawn point.

### 5) Enemy Pool and Wave Logic
File: Assets/Scripts/EnemyPoolManager.cs

Responsibilities:
- Maintain a pool of enemy instances.
- Start a wave and enforce max concurrent active enemies.
- Track pending enemies and active enemies.
- Spawn next enemy when a current one is defeated.

Main runtime values:
- pendingEnemies: enemies left to spawn for current wave.
- activeEnemies: currently active enemies in scene.
- maxActiveEnemies: active cap for current wave.

Main methods:
- StartWave(int enemiesToSpawn, int maxEnemies, Transform spawnPoint = null)
- RequesObject(Transform position)
- TrySpawnPending(Transform spawnPoint)
- HandleEnemyDefeated(enemyHealth health)

### 6) Enemy Health Defeat Signal
File: Assets/Scripts/Enemy/enemyHealth.cs

Responsibilities:
- Handle received damage and death.
- Emit OnEnemyDefeated event before deactivating the enemy object.

Event:
- OnEnemyDefeated(enemyHealth)

## Runtime Flow

1. Player enters horde trigger.
2. hordeEnemyTrigger raises event with enemiesToSpawn and maxEnemies.
3. SpawnEnemies receives event and calls EnemyPoolManager.StartWave.
4. EnemyPoolManager spawns up to maxEnemies immediately.
5. Defeated enemy triggers OnEnemyDefeated from enemyHealth.
6. EnemyPoolManager receives that signal, decreases active count, and spawns the next pending enemy.
7. Process repeats until pendingEnemies reaches zero.

## Inspector Setup Checklist

### hordeEnemyTrigger
- Assign triggerEffect to the Enemy Spawn ScriptableObject.
- Set enemiesToSpawn to total wave size.
- Set maxEnemies to alive-at-once limit.
- Assign triggerObj.

### EnemySpawnListener
- Assign gameEvent to same Enemy Spawn ScriptableObject.
- In response UnityEvent, hook to SpawnEnemies.spawnEnemies.

### SpawnEnemies
- Assign enemyPoolManager reference (or let it auto-find).
- Optional: assign spawnPoint.

### EnemyPoolManager
- Assign Prefab enemy object.
- Assign origin transform.
- Ensure bulletList exists in inspector (or keep initialized by code path).

## Behavior Notes

- If maxEnemies is less than 1, it is clamped to 1.
- If enemiesToSpawn is less than 0, it is clamped to 0.
- If there are no inactive pooled enemies, the pool expands by one and spawns it.
- The same pool can support multiple horde triggers, one wave request at a time.

## Known Improvement Ideas

- Rename generic variable names in EnemyPoolManager:
  - Prefab -> enemyPrefab
  - bulletList -> enemyPool
  - RequesObject -> RequestObject
- Add optional per-wave completion event when pendingEnemies is 0 and activeEnemies is 0.
- Add spawn cooldown between enemy replacements for pacing.
- Add editor validation to prevent missing references.

## Quick Test Plan

1. Configure a horde trigger with enemiesToSpawn = 10 and maxEnemies = 3.
2. Enter trigger zone.
3. Verify only 3 enemies are active at first.
4. Defeat 1 enemy.
5. Verify exactly 1 new enemy appears.
6. Repeat until 10 total enemies have spawned and been defeated.
7. Verify no extra enemies spawn after wave completion.
