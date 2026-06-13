# Semester 14 Entity Hooks

## Overview

The Horror Foundation System is designed to support future entity implementation without requiring monster logic now.

## Available Hooks

### 1. HorrorManager Escalation Events

```csharp
// Subscribe to horror level changes for entity behavior
HorrorManager.Instance.OnEscalationForEntity += (float level) =>
{
    // Entity behavior based on horror level
    // e.g., increase aggression, spawn frequency, etc.
};

// Subscribe to stage changes for entity state transitions
HorrorManager.Instance.OnStageEscalation += (HorrorStage stage) =>
{
    // Entity state changes based on horror stage
    // e.g., Calm = passive, Paranoia = aggressive, Collapse = chase
};
```

### 2. Horror Level Thresholds

| Stage | Level | Entity Behavior (Future) |
|-------|-------|--------------------------|
| Calm | 0-20 | Entity dormant / distant sightings |
| Unease | 20-40 | Entity observes from distance |
| Disturbance | 40-60 | Entity makes presence known |
| Paranoia | 60-80 | Entity actively stalks player |
| Collapse | 80-100 | Entity becomes aggressive / chase |

### 3. Integration Points

```csharp
// In entity script (future implementation):
public class Semester14Entity : MonoBehaviour
{
    private void Start()
    {
        HorrorManager.Instance.RegisterEntityEscalationCallback(OnEscalation);
        HorrorManager.Instance.RegisterStageEscalationCallback(OnStageChange);
    }

    private void OnEscalation(float level)
    {
        // Adjust entity behavior based on level
    }

    private void OnStageChange(HorrorStage stage)
    {
        // Transition entity state
    }
}
```

### 4. Trigger Integration

The `HorrorEventTrigger` can be extended to include entity-specific triggers:

```csharp
// Future: Add entity events to the event pool
public class EntitySpawnEvent : HorrorEvent
{
    // Entity spawn logic
}
```

## DO NOT IMPLEMENT

- Monster AI
- Chase mechanics
- Jump scare sequences
- Entity animations
- Combat systems

## Notes

- Horror system operates independently of entity system
- Entity hooks are one-way (Horror → Entity)
- Entity can read horror state but cannot modify it directly
- All entity behavior should respond to horror level/stage, not the other way around
