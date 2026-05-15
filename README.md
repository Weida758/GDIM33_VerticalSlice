# GDIM33 Vertical Slice
## Milestone 1 Devlog
1. I utilized the visual scripting graphs to handle the display and updating of the player's health UI to the screen. In the extension of the Update node in the HealthUIGraph, I have object variables that references the PlayerHealth script, which allows me to get the current health and max health of the player, divide them, and then set health bar fill value with that resulting ratio. It also sequentially sets the health text inside the health bar in the format of "currentHealth/maxHealth".
2. I added a finite state machine break down of my game's game states, which are separated into PausedState, PlayingState, and LevelingUpState. These states handles what should happen when the game enters that state. My current state machine graph implementation doesn't cover leveling up state yet, but I have used it already for pausing and playing. How it works is, custom events are defined inside the transitioning of the states, which calls the Pause() or Resume() method respectively from a custom C# script I created, when that event is called, and then transitions from the pause state to the playing state or vice versa. Inside the actual states, during the entering of the state, I attached a node which sets the TimeScale of the game correspondingly based on the state (pausing have a timescale of 0 while playing have time scale of 1). During the update method, the nodes check for the input of the escape key on the keyboard, and if that is detected, the custom event defined inside the transitions will be called, and causes to pause/unpause & transition into the opposite state. This interacts with the other systems of our game by completing stopping/resuming all activities of the player, enemy, and spawning logics. 
<img width="1680" height="1165" alt="GDIM 33 Game Breakdown (1)" src="https://github.com/user-attachments/assets/f7f04cc5-e68a-4f24-be8e-ac2cb3dc08ad" />


## Milestone 2 Devlog

1. The complicating gameplay feature that I want to build for this milestone is the level up and upgrade system of my game. Whenever enemies are defeated in the game, experience points will drop and the player can collect them. Once enough have been collected, the player will level up, the game will pause, and 3 cards will show up for the player to choose one from as a reward. The reward can either be an upgrade of an already obtained weapon, a new weapon, or a stats upgrade.

Big Step 1: Data Driven approach to making weapons
- Use scriptable objects to hold the stats of individual weapons
- Rewrite player attack so that it uses the Scriptable object assets instead of hardcoded attacks.
- Give player an inventory for holding weapons, and a capacity for that.

Big Step 2: 
- When enemies die, they drop XP Gems which should be a prefab that drops at its position of death
- The XP are picked up when the trigger collider enters player collider.
- Add levels for the player, and use math to create a curve for the amount of experience points needed to upgrade player for each level.

Big Step 3:
- When the level up event is triggered the game should be paused
- The game should then pick three random weapons or upgrades that the player can choose from and show them as cards on a panel.
- Clicking the cards applies the upgrade and starts the game again.


2. The breakdown definitely did help me implement the level up feature of my game. They break the big feature into small steps that can be achievable, so I didn't become overwhelmed. What I would improve the breakdown next time could be adding more details to the small steps of each big step, since it just makes keeping track of everything much easier. For example, I can also include implementing the UI for player level up progress for big step 2.

3. I implemented the pause and resume state and menu using visual scripting. I created a PauseUI script under the UI folder in the script folder, which the visual scripting graphs used by calling the Pause() or Resume() method. This gave me a visual scripting approach to handling game pausing state. 
<img width="1776" height="1154" alt="image" src="https://github.com/user-attachments/assets/2422a288-ff41-45dc-ae92-3eaac82ce930" />

4. The Unity system that I want to be graded is the WeaponDataSO.cs script under Scripts/Weapons/ folder. All the weapon's base stat as well as per level specific stats are handled by the WeaponDataSO, which I can use to create weapon assets that can be used for future weapons as well. 

## Milestone 3 Devlog
Milestone 3 Devlog goes here.
## Milestone 4 Devlog
Milestone 4 Devlog goes here.
## Final Devlog
Final Devlog goes here.
## Open-source assets
- [Health Bar UI](https://assetstore.unity.com/packages/2d/gui/icons/gui-parts-159068)
- [Attack VFX](https://untiedgames.itch.io/super-pixel-effects-gigapack)
- [Environment Tilemap](https://backterria.itch.io/the-roguelike)
- [Character Sprite](https://zerie.itch.io/tiny-rpg-character-asset-pack)
- [Card Upgrade](https://cafedraw.itch.io/fantasy-card-assets)
