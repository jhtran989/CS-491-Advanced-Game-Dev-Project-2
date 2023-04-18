# README
## Stationary
### Project 2
#### CS 491 - Intro to Game Development
#### Team Members: Alex, Andrew, Gale, John, Josue

## Important Notes

### Finding Stuff that are Inactive

- restricted to certain methods, like `GetComponentInChildren()`, `FindObjectsOfType()`
- maybe avoid the `Find()` methods and its variants...

### Delegates

- delegates are scattered all over the place, need to consolidate them (or come up with better design)
- `static` delegate instances will have **all** instances call the functions stored in the delegate (e.g., `DoorManager` will check the condition to unlock the door on all doors, not just the current door)
- should avoid using `static` delegates, especially when there is a possibility for a lot of instances (e.g., fires) &mdash; fine if there's one or just a few controlled instances (like managers)
- also should avoid invoking delegates outside of class that declared it...

### Multiple Scenes

- can't directly add `GameObject` from another `Scene` in the `Inspector`...use `GameObject.Find()` instead for hard string search (need to use **unique** names)

### Fire System

- For the smoke, it needs to be **above** the floor of the station &mdash; the behavior of the smoke does not behave as expected otherwise (everything in `Fire` scene is a child of some game object with a modified `Y` position)
- For the water hose (fire extinguisher), the initial shape needs to be changed from `sphere` to `cone` and the `Speed Modifier` under `Velocity over Lifetime` needs to be changed to some higher number ("weak" spray otherwise) &mdash; Edit: or edit `Start Speed` under the main parameters (`WaterHose`)
- For multiple scenes, need some way to link the steam to the fire...

## Resources

### Fire Assets

(Old)
https://www.youtube.com/watch?v=5Mw6NpSEb2o

(New)
Unity Particle Pack

### Calculator

https://github.com/cwgtech/UICalc

### Interactable Code (`SphereOverlap` with abstracted `IInterface`)

https://www.youtube.com/watch?v=LdoImzaY6M4


