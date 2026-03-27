# Player Transformation System

## Overview
The Transformation component controls a timed transformation cycle with three phases:

1. Initial lockout at game start (cannot transform yet).
2. Active transformation for a fixed duration.
3. Cooldown before the next transformation is allowed.

Script reference:
- Assets/Scripts/PlayerLogic/Transformation.cs

## Main Purpose
This system prevents instant and repeated transformations. It guarantees:

- No transformation available immediately on scene start.
- Transformation lasts only for a configured amount of time.
- A cooldown always happens after transformation ends.
- Input spam cannot retrigger while transforming or cooling down.

## Inspector Fields
Public fields in the script:

- Input Handler (`inputHandler`)
  - Event source that triggers transformation attempts.
- Namiko Sprite (`namikoSprite`)
  - Object used to resolve the sprite Animator if not manually assigned.
- Is Transforming (`isTransforming`)
  - Runtime flag showing whether transformation is currently active.
- Is Transformed (`isTransformed`)
  - True only while the player is in the transformed state. Becomes false as soon as detransform begins. Readable by other scripts to check active form.
- Transform Duration (`transformDuration`)
  - How long transformed state remains active.
- Transform Cooldown (`transformCooldown`)
  - Wait time after transformation ends before transformation can be used again.
- Initial Transform Delay (`initialTransformDelay`)
  - Startup lockout time before first transformation can happen.
- Detransform Animation Delay (`detransformAnimationDelay`)
  - Time to wait after the detransform trigger before restoring the original controller.
- Magical Girl Namiko (`magicalGirlNamiko`)
  - Runtime Animator Controller used while transformed.

Serialized fields:

- `animator`
  - Plays the transformation trigger animation (`Transformation`).
- `animatorNamiko`
  - Animator whose Runtime Animator Controller is swapped during transform.

## Internal Runtime State
Private fields used to control state:

- `canTransform`
  - True only when input is allowed to start a new transformation.
- `defaultNamikoController`
  - Cached original controller restored when transformation ends.

## Lifecycle Flow

### 1. Awake
- Resolves `animatorNamiko` from `namikoSprite` if needed.
- Caches `defaultNamikoController` from current controller.

### 2. Start
- Sets `canTransform` to false.
- Starts `InitialTransformLockout()` coroutine.

### 3. InitialTransformLockout
- Waits `initialTransformDelay` seconds (if greater than zero).
- Sets `canTransform = true`.

### 4. Input Event (NamikoTransform)
When the input event fires:

- If `canTransform` is false, request is ignored.
- If `isTransforming` is true, request is ignored.
- Otherwise starts `TransformationFlow()`.

### 5. TransformationFlow
Step-by-step:

1. Lock input: `canTransform = false`.
2. Mark active: `isTransforming = true`.
3. Trigger transform animation: `animator.SetTrigger("Transformation")`.
4. Wait 1 second (animation lead-in).
5. Swap to transformed controller (`magicalGirlNamiko`).
6. Wait `transformDuration`.
7. Trigger detransform animation: `animator.SetTrigger("Transform")`.
8. Wait `detransformAnimationDelay`.
9. Restore original controller (`defaultNamikoController`).
10. Mark inactive: `isTransforming = false`.
11. Wait `transformCooldown`.
12. Unlock input: `canTransform = true`.

## Timeline Example
With these values:

- `initialTransformDelay = 3`
- `transformDuration = 6`
- `transformCooldown = 8`

Behavior:

1. Time 0-3s: player cannot transform.
2. At 3s: first transform can be triggered.
3. After trigger:
   - 1s lead-in animation,
   - then 6s transformed state.
4. Detransform trigger runs (`Transform`), then waits `detransformAnimationDelay`.
5. Original controller is restored, then 8s cooldown starts.
6. After cooldown: transform can be triggered again.

## Event Subscription Safety
- OnEnable subscribes to `inputHandler.transforming` only if `inputHandler` is assigned.
- OnDisable unsubscribes with the same null check.

This avoids null reference errors when the reference is not configured yet.

## Animator Contract
Required setup:

- The main animator (`animator`) must have trigger parameter named `Transformation` for transform start.
- The main animator (`animator`) must also have trigger parameter named `Transform` for detransform.
- `magicalGirlNamiko` must point to the transformed Runtime Animator Controller.
- The original controller must be valid at startup so it can be restored correctly.

## Common Tuning Guidelines
- Want quick combat rhythm:
  - Lower `transformDuration` and `transformCooldown`.
- Want powerful but rare transformation:
  - Increase both `transformDuration` and `transformCooldown`.
- Want delayed access after spawn:
  - Increase `initialTransformDelay`.

## Troubleshooting
- Transformation never starts:
  - Check `inputHandler` assignment.
  - Check event invocation for `transforming`.
  - Check `initialTransformDelay` is not very high.
- Character stays transformed forever:
  - Verify `defaultNamikoController` exists at startup.
- No visual change on transform:
  - Verify `magicalGirlNamiko` is assigned.
  - Verify `animatorNamiko` resolves correctly from `namikoSprite`.
- Trigger animation not playing:
  - Ensure animator parameters are exactly `Transformation` and `Transform`.
