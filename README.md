# README
## Stationary
### Project 2
#### CS 491 - Intro to Game Development
#### Team Members: Alex, Andrew, Gale, John, Josue

## Final Tweaks 

- [x] door bugs
- [x] fire extinguisher bug (hold and release)
- [x] add unlimited door charges
- [x] remove initial fires in each room
- [x] increase oxygen meter (more balanced)
- [x] fix beeping sound (at beginning) &mdash; looping
- [ ] softer music
- [ ] different buttons for check and code submission
- [x] highlight interactable
- [x] remove lock with fire extinguisher and terminal

### Extra Ideas

- blue fire
- golden apple
- achievements...

## TODO

- [x] door check on BOTH sides (or condition)
- [x] have array and check boundaries (keep list of UNLOCKED rooms) -- CENTER ROOMS
- [x] true random fire spawning (queue and shuffle)
- [x] fire collision (ignore player)
- [x] update count of fires present (ALL unlocked rooms)\
- [x] update UI buttons (when they are available to use)

## Important Notes

### Highlight

- need to set the `Read/Write` option in the import settings shown in the `Inspector` to `true` (or ticked) for the 
  highlighting to work (gives an error otherwise)

### Prefabs

- not aware of specific scene hierarchies or instances of scripts (so find components during runtime...relying on scene hierarchy in scripts &mdash; not robust to changes to hierarchy...)

### Inspector

- to put headers, the first immediate variable below the header MUST be serializable (e.g., `public` or with the `SerializeField` field)

### Interactable (interface)

- quick fix &mdash; create new script for different key bindings...not modular and scalable
- `TryGetComponent()` will work for subclasses too (e.g., `IInteractable` and `IInteractableDoor` that inherits from `IInteractable`)

### Managers

- `DoorManager` is not really a manager (each door has one and does not manages multiple doors...) &mdash; updated to `DoorEvent`

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
Unity Particle Pack (Unity Asset Store)
https://assetstore.unity.com/packages/vfx/particles/particle-pack-127325

### Calculator

https://github.com/cwgtech/UICalc

### Interactable Code (`SphereOverlap` with abstracted `IInterface`)

https://www.youtube.com/watch?v=LdoImzaY6M4

### Sound Credits

*Copied over from the `README.txt` under the `Assets/Sound` folder made by Alex

***ALL SOUNDS ARE USED IN ACCORDANCE WITH THE LICENSE THEY WERE PUBLISHED UNDER, ALL CREDIT FOR SOUNDS REMAINS WITH 
THE ORIGINAL AUTHORS***

#### Sound cues

Crackling of fire (when the player is close to it) - Fire Sound Efftect - Sound Effect by MaxHammarbäck - https://pixabay.com/sound-effects/id-21991/

Fire being put out with water - extinguishing fire - Sound Effect by soundslikewillem - https://freesound.org/people/soundslikewillem/sounds/399548/

Fire extinguisher - ONOMATOPOEIC sounds by human » 01674 fire extenguisher.wav - Sound Effect by Robinhood76 - https://freesound.org/people/Robinhood76/sounds/96176/#

Puzzle solved (doesn’t have to be a negative sound but some subtle difference between the two)
correctly - Correct-2 - Sound Effect by bwg2020 - https://pixabay.com/sound-effects/id-46134/
incorrectly - System error notice - Sound Effect by UNIVERSFIELD - https://pixabay.com/sound-effects/id-132470/

low oxygen - system trouble - Sound Effect by seewalker - https://pixabay.com/sound-effects/id-32321/

Escape pod launch - escape_pod - Sound Effect by Alexander Atencio using sounds by bolkmar - https://freesound.org/people/bolkmar/sounds/434136/, MGA95 - https://freesound.org/people/MGA95/sounds/555745/, vartioh - https://freesound.org/people/vartioh/sounds/338837/

Space Ambience - spaceship ambience with effects - Sound Effect by Placidplace - https://pixabay.com/sound-effects/id-21420/

Main Menu - spacesound - Sound Effect by SamuelFrancisJohnson - https://pixabay.com/sound-effects/id-7547/

### Outline

When interacting with objects, there will now be a highlight to make it easier what the player is interacting with

Quick Outline (Unity Asset Store)
https://assetstore.unity.com/packages/tools/particles-effects/quick-outline-115488
