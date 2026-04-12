# Enemy AI State Machine

This document explains how the enemy AI works in the current implementation.

## Architecture

The enemy behavior is split into three layers:

- enemyHealth: Owns HP, receives hit triggers, emits damage events, finalizes death.
- AI: Owns FSM transitions and interruption rules.
- State classes (Idle, Pursue, Attack, Hurt): Own state-specific behavior.

This separation is important: health does not choose states, and states do not modify HP.

## Core State Flow

The enemy uses a finite state machine with these states:

- IDLE
- PURSUE
- ATTACK
- HURT

Typical flow:

- Idle -> Pursue or Attack (random chances + distance checks)
- Pursue -> Attack or Idle
- Attack -> Pursue or Idle after attack timer
- Any non-Hurt state can be redirected to Hurt when buffered damage is consumed (Attack is delayed, not interrupted)
- Hurt -> Idle (non-lethal) or death finalization (lethal)

## Base State Lifecycle

All states inherit from State and use this lifecycle:

- ENTER
- UPDATE
- EXIT

AI runs this each frame:

```csharp
currentState = currentState.Process();
```

A state transitions by assigning nextState and setting stage to EXIT.

## Per-State Summary

### Idle

- Stops movement (`agent.isStopped = true`).
- Triggers `Idle` animation.
- Randomly decides to pursue or attack based on player distance.
- Uses randomized cooldown (`minCooldown` to `maxCooldown`) before switching.

### Pursue

- Enables walking (`isWalking = true`).
- Recalculates destination via nearest checkpoint approximately every 0.5s.
- When close to player, probabilistically chooses Attack or Idle.
- Can also randomly drop back to Idle.

### Attack

- Triggers `Attack` animation once.
- Waits `attackDuration`.
- Chooses next state based on distance (far -> Pursue, near -> Idle).
- Uses randomized cooldown before transition.

### Hurt

- Stops movement and walking animation.
- Disables damage reception while Hurt is active.
- Triggers `IsHurt` animation once.
- Waits hurt timer (`hurtDuration` from payload).
- On completion:
  - non-lethal hit -> transitions to Idle
  - lethal hit -> asks AI to finalize death through health
- Re-enables damage reception on Exit.

## Damage Pipeline

### 1) Hit detection

enemyHealth listens for trigger entries:

```csharp
void OnTriggerEnter(Collider other)
{
    if (other.TryGetComponent<damageDealer>(out var dealer))
    {
        receiveDamage(dealer.damage);
    }
}
```

### 2) Duplicate-hit protection

enemyHealth contains a short grace window to avoid accidental duplicate trigger damage from the same damageDealer instance:

- `duplicateHitGraceSeconds` (default 0.08)
- `lastDamageDealer`
- `lastDamageTime`

If the same dealer hits again inside that tiny window, the second hit is ignored.

### 3) Damage gate during Hurt

enemyHealth also uses `canReceiveDamage`:

- If `canReceiveDamage` is false, incoming damage is ignored.
- Hurt.Enter sets it to false.
- Hurt.Exit sets it back to true.

This prevents fast combo hits from stacking during the hurt animation window.

### 4) Damage payload emission

When valid damage is applied, enemyHealth builds and emits an EnemyDamagePayload:

```csharp
EnemyDamagePayload payload = new EnemyDamagePayload(
    amount: dmgAmount,
    hurtDuration: defaultHurtDuration,
    isLethal: isLethal
);

OnDamageReceived?.Invoke(payload);
```

Payload fields:

- Amount
- HurtDuration
- IsLethal

### 5) AI buffering and consume policy

AI subscribes to OnDamageReceived and stores pending damage:

- `pendingDamage`
- `hasPendingDamage`
- `hasPendingLethalDamage`

AI then decides when to consume:

- If current state is Idle or Pursue: consume immediately into Hurt.
- If current state is Attack: do not interrupt, keep damage buffered.
- If current state is Hurt: do not consume.

### 6) Lethal finalization

For lethal hits:

- enemyHealth marks pending death finalization.
- Hurt runs first.
- Hurt notifies AI (`HandleHurtFinished(true)`).
- AI calls `health.FinalizeDeath()`.
- enemyHealth fires `OnEnemyDefeated` and deactivates the GameObject.

## Pooling and Reactivation

Enemies are pooled and re-enabled frequently during hordes. On re-enable:

- enemyHealth resets HP and internal hit-protection fields.
- AI re-subscribes events and clears pending damage.
- AI also resets NavMesh state and forces a fresh starting state (Idle).

This avoids stale state instances from previous life cycles.

## Key Inspector Fields

enemyHealth:

- MaxHealth
- defaultHurtDuration
- duplicateHitGraceSeconds

AI:

- minCooldown
- maxCooldown
- attackDuration
- player (optional; auto-resolved by tag/name fallback)

## Troubleshooting

If enemies do not move after respawn:

- Confirm AI OnEnable is resetting currentState and NavMesh path.
- Confirm player reference can be resolved.
- Confirm spawn position is valid on NavMesh.

If enemies take too much damage:

- Verify only one enemyHealth handles the hitbox for that enemy.
- Check duplicateHitGraceSeconds is not set too low.
- Confirm canReceiveDamage is false during Hurt.

If enemies never take combo damage after Hurt ends:

- Confirm Hurt.Exit runs and re-enables `SetCanReceiveDamage(true)`.

## File Map

- Assets/Scripts/Enemy/enemyHealth.cs
- Assets/Scripts/Enemy/EnemyDamagePayload.cs
- Assets/Scripts/Enemy/States/AI.cs
- Assets/Scripts/Enemy/States/State.cs
- Assets/Scripts/Enemy/States/Idle.cs
- Assets/Scripts/Enemy/States/Pursue.cs
- Assets/Scripts/Enemy/States/Attack.cs
- Assets/Scripts/Enemy/States/Hurt.cs
