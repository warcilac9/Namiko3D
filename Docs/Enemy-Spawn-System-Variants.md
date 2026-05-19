# Enemy Spawn System with Variants

## Current Architecture Overview

Your enemy spawn system supports multiple enemy types (melee and ranged) in a single wave with independent per-type active caps.

### Event Flow

```
1. Player enters horde trigger
   ↓
2. hordeEnemyTrigger raises EventSOEnemySpawn.Occurred()
   ↓
3. EnemySpawnListener receives event and packs data into EnemySpawnData
   ↓
4. UnityEvent calls SpawnEnemies.spawnEnemies(EnemySpawnData)
   ↓
5. SpawnEnemies calls EnemyPoolManager.StartWave()
   ↓
6. EnemyPoolManager spawns both melee and ranged up to their individual caps
   ↓
7. When enemy dies, enemyHealth triggers OnEnemyDefeated
   ↓
8. EnemyPoolManager spawns next pending enemy of that type
   ↓
9. Wave ends when all pending and active enemies are defeated
```

## Current System Structure

### 1. **hordeEnemyTrigger.cs**
Configurable per-horde in Inspector:
- `enemiesToSpawn` – total melee enemies
- `rangedEnemiesToSpawn` – total ranged enemies
- `maxBasicEnemies` – max melee on-screen at once
- `maxRangedEnemies` – max ranged on-screen at once
- `origin` – spawn point

### 2. **EventSOEnemySpawn.cs** (ScriptableObject)
Broadcasts to all listeners with:
- enemiesToSpawn, rangedEnemiesToSpawn, maxBasicEnemies, maxRangedEnemies, origin

### 3. **EnemySpawnListener.cs**
Listens to EventSOEnemySpawn and packs 5 parameters into `EnemySpawnData` object, then invokes UnityEvent.

### 4. **EnemySpawnData.cs** (Data Class)
```csharp
public class EnemySpawnData
{
    public int EnemiesToSpawn;
    public int RangedEnemiesToSpawn;
    public int MaxBasicEnemies;
    public int MaxRangedEnemies;
    public Transform Origin;
}
```

### 5. **SpawnEnemies.cs**
Receives the packed `EnemySpawnData`, passes to `EnemyPoolManager.StartWave()`.

### 6. **EnemyPoolManager.cs**
Maintains two separate pools:
- `bulletList` + `Prefab` for melee enemies
- `rangedEnemyList` + `rangedPrefab` for ranged enemies

Tracks separately:
- `pendingEnemies` and `maxBasicActiveEnemies`
- `pendingRangedEnemies` and `maxRangedActiveEnemies`

Spawning logic respects both caps independently when calling `TrySpawnPending()`.

---

## Adding a New Enemy Variant

If you want to add a third enemy type (e.g., "Heavy" or "Caster"), follow this pattern:

### Step 1: Add Data Fields to EnemySpawnData

**File: Assets/Scripts/Events/EnemySpawnListener.cs**

```csharp
public class EnemySpawnData
{
    public int EnemiesToSpawn;
    public int RangedEnemiesToSpawn;
    public int HeavyEnemiesToSpawn;          // NEW
    public int MaxBasicEnemies;
    public int MaxRangedEnemies;
    public int MaxHeavyEnemies;              // NEW
    public Transform Origin;
}
```

### Step 2: Update the Event Signature

**File: Assets/Scripts/Events/EventSOEnemySpawn.cs**

```csharp
public void Occurred(
    int enemiesToSpawn, 
    int rangedEnemiesToSpawn, 
    int heavyEnemiesToSpawn,                 // NEW
    int maxBasicEnemies, 
    int maxRangedEnemies, 
    int maxHeavyEnemies,                     // NEW
    Transform origin)
{
    for(int i = 0; i < eventListeners.Count; i++)
    {
        eventListeners[i].InvokeEvent(
            enemiesToSpawn, 
            rangedEnemiesToSpawn, 
            heavyEnemiesToSpawn,              // NEW
            maxBasicEnemies, 
            maxRangedEnemies, 
            maxHeavyEnemies,                  // NEW
            origin);
    }
}
```

### Step 3: Update Listener Method

**File: Assets/Scripts/Events/EnemySpawnListener.cs**

```csharp
public void InvokeEvent(
    int enemiesToSpawn, 
    int rangedEnemiesToSpawn, 
    int heavyEnemiesToSpawn,                 // NEW
    int maxBasicEnemies, 
    int maxRangedEnemies, 
    int maxHeavyEnemies,                     // NEW
    Transform origin)
{
    var data = new EnemySpawnData
    {
        EnemiesToSpawn = enemiesToSpawn,
        RangedEnemiesToSpawn = rangedEnemiesToSpawn,
        HeavyEnemiesToSpawn = heavyEnemiesToSpawn,  // NEW
        MaxBasicEnemies = maxBasicEnemies,
        MaxRangedEnemies = maxRangedEnemies,
        MaxHeavyEnemies = maxHeavyEnemies,          // NEW
        Origin = origin
    };

    response?.Invoke(data);
}
```

### Step 4: Update Horde Trigger

**File: Assets/Scripts/Hordes/hordeEnemyTrigger.cs**

```csharp
public class hordeEnemyTrigger : MonoBehaviour
{
    public Collider hordeTrigger;
    public EventSOEnemySpawn triggerEffect;
    public int enemiesToSpawn;
    public int rangedEnemiesToSpawn;
    public int heavyEnemiesToSpawn;         // NEW
    public int maxBasicEnemies;
    public int maxRangedEnemies;
    public int maxHeavyEnemies;             // NEW
    public GameObject triggerObj;
    public Transform origin;

    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerHealth>(out var playerHealth) && triggerEffect != null)
        {
            triggerEffect.Occurred(
                enemiesToSpawn, 
                rangedEnemiesToSpawn, 
                heavyEnemiesToSpawn,        // NEW
                maxBasicEnemies, 
                maxRangedEnemies, 
                maxHeavyEnemies,            // NEW
                origin);
        }
    }
}
```

### Step 5: Update Pool Manager

**File: Assets/Scripts/EnemyPoolManager.cs**

Add new fields:
```csharp
[SerializeField] private GameObject heavyPrefab;         // NEW
[SerializeField] private List<GameObject> heavyEnemyList; // NEW

private int pendingHeavyEnemies;        // NEW
private int maxHeavyActiveEnemies;      // NEW
```

Update `Start()`:
```csharp
if (heavyEnemyList == null)
{
    heavyEnemyList = new List<GameObject>();
}
```

Update `EnsurePoolInitialized()`:
```csharp
AddObjectsToPool(poolSize, heavyPrefab, heavyEnemyList); // NEW
```

Update `StartWave()` signature:
```csharp
public void StartWave(
    int enemiesToSpawn,
    int rangedEnemiesToSpawn,
    int heavyEnemiesToSpawn,                    // NEW
    int maxBasicEnemies,
    int maxRangedEnemies,
    int maxHeavyEnemies,                        // NEW
    Transform spawnPoint = null)
{
    EnsurePoolInitialized();
    
    allEnemiesDefeatedLogged = false;
    pendingEnemies = Mathf.Max(0, enemiesToSpawn);
    pendingRangedEnemies = Mathf.Max(0, rangedEnemiesToSpawn);
    pendingHeavyEnemies = Mathf.Max(0, heavyEnemiesToSpawn);      // NEW
    maxBasicActiveEnemies = Mathf.Max(0, maxBasicEnemies);
    maxRangedActiveEnemies = Mathf.Max(0, maxRangedEnemies);
    maxHeavyActiveEnemies = Mathf.Max(0, maxHeavyEnemies);        // NEW
    currentWaveSpawnPoint = spawnPoint != null ? spawnPoint : origin;

    TrySpawnPending(currentWaveSpawnPoint);
    CheckWaveCompletion();
}
```

Add pool request method:
```csharp
public GameObject RequestHeavyObject(Transform position)
{
    return RequestObjectFromPool(position, heavyEnemyList, heavyPrefab);
}
```

Update `CheckWaveCompletion()`:
```csharp
private void CheckWaveCompletion()
{
    int activeBasicEnemies = CountActiveEnemiesInPool(bulletList);
    int activeRangedEnemies = CountActiveEnemiesInPool(rangedEnemyList);
    int activeHeavyEnemies = CountActiveEnemiesInPool(heavyEnemyList);  // NEW
    int totalActiveEnemies = activeBasicEnemies + activeRangedEnemies + activeHeavyEnemies;

    if (pendingEnemies <= 0 && pendingRangedEnemies <= 0 && pendingHeavyEnemies <= 0   // NEW
        && totalActiveEnemies <= 0 && !allEnemiesDefeatedLogged)
    {
        allEnemiesDefeatedLogged = true;
        AllEnemiesDefeated();
    }
}
```

Update `TrySpawnPending()`:
```csharp
private void TrySpawnPending(Transform spawnPoint)
{
    while (pendingEnemies > 0 || pendingRangedEnemies > 0 || pendingHeavyEnemies > 0)  // NEW
    {
        int activeBasicEnemies = CountActiveEnemiesInPool(bulletList);
        int activeRangedEnemies = CountActiveEnemiesInPool(rangedEnemyList);
        int activeHeavyEnemies = CountActiveEnemiesInPool(heavyEnemyList);             // NEW

        bool canSpawnBasic = pendingEnemies > 0 && activeBasicEnemies < maxBasicActiveEnemies;
        bool canSpawnRanged = pendingRangedEnemies > 0 && activeRangedEnemies < maxRangedActiveEnemies;
        bool canSpawnHeavy = pendingHeavyEnemies > 0 && activeHeavyEnemies < maxHeavyActiveEnemies;  // NEW

        if (!canSpawnBasic && !canSpawnRanged && !canSpawnHeavy)  // NEW
        {
            break;
        }

        if (canSpawnBasic)
        {
            if (RequesObject(spawnPoint) != null)
            {
                pendingEnemies--;
            }
        }

        if (canSpawnRanged)
        {
            if (RequestRangedObject(spawnPoint) != null)
            {
                pendingRangedEnemies--;
            }
        }

        if (canSpawnHeavy)  // NEW
        {
            if (RequestHeavyObject(spawnPoint) != null)
            {
                pendingHeavyEnemies--;
            }
        }
    }
}
```

### Step 6: Update SpawnEnemies

**File: Assets/Scripts/Hordes/SpawnEnemies.cs**

```csharp
public void spawnEnemies(EnemySpawnData data)
{
    if (data == null)
    {
        return;
    }

    if (enemyPoolManager == null)
    {
        enemyPoolManager = FindFirstObjectByType<EnemyPoolManager>();
    }

    if (enemyPoolManager == null)
    {
        return;
    }

    enemyPoolManager.StartWave(
        data.EnemiesToSpawn, 
        data.RangedEnemiesToSpawn, 
        data.HeavyEnemiesToSpawn,             // NEW
        data.MaxBasicEnemies, 
        data.MaxRangedEnemies, 
        data.MaxHeavyEnemies,                 // NEW
        data.Origin);
}
```

---

## Summary: Pattern for N Enemy Types

To scale beyond 3 types, follow this pattern:

1. **EnemySpawnData** – add counts and caps for each type
2. **EventSOEnemySpawn.Occurred()** – expand parameters
3. **EnemySpawnListener.InvokeEvent()** – pack into data object
4. **hordeEnemyTrigger** – add inspector fields
5. **EnemyPoolManager** – add:
   - Prefab + List for the new type
   - Pending count and max active count
   - Request method
   - Include in spawn/completion logic
6. **SpawnEnemies** – update StartWave call

The system is **data-driven** and **inspector-configurable**, so designers can set per-horde counts without touching code.

---

## Debugging Checklist

If enemies still don't spawn after adding a variant:

- [ ] EnemyPoolManager has the new prefab assigned (not null)
- [ ] hordeEnemyTrigger has at least one count > 0 and corresponding max > 0
- [ ] EventSOEnemySpawn, EnemySpawnListener, hordeEnemyTrigger signatures are **all consistent**
- [ ] UnityEvent binding in EnemySpawnListener is reassigned to SpawnEnemies.spawnEnemies
- [ ] Each enemy prefab has an `enemyHealth` component (required for death signal)
