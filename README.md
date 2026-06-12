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
1. The shadergraph that I made is used on the experience points in the game and the graders can find the shader graph named shine under the Art/Shader folder, and the material using that shader graph under Art/Material which is named Shine as well. These materials are used for the EXPT2 and EXPT3 prefabs under Prefab folder. It is a 2D Sprite Unlit shader that samples the sprite texture with a Sample Texture 2D node using the sprite UVs, multiplies the sampled RGBA by a gold tint, and sends the texture alpha to the Alpha output so the sprite keeps its transparent shape. I used a Time node with Sine to make the brightness pulse, and I used the UV X/Y values with the Fraction, Absolute, One Minus, and SmoothStep nodes to create the diagonal shine that goes across the sprite. These nodes combined together changes the sprite's fragment color during rendering and makes the experience point on the ground shine periodically. 
<img width="2793" height="1388" alt="image" src="https://github.com/user-attachments/assets/7eef7439-838d-4338-8eb4-14be17019e10" />

2. The previous feedback from playtesting was that the waves spawns a little too quickly and that they weren't sure if the balancing is fine because there isn't multiple weapons. I addressed these feedbacks by adding an additional weapon that the player can obtain by leveling up. I feel like by adding another weapon to obtain and upgrade, the player wouldn't think the enemies are spawning too quickly as they are much more powerful now.

3. Since the last milestone I have added enemy damaged sound effects to make the game feel good to play. I also added a new weapon, which when obtained, the player will periodically shoot out orbs that bounce around the boundaries of the camera and damages any enemy it touches. In addition to this, to let the player upgrade even after they have the max leveled weapon, I added stats upgrades so that when the weapons are leveled to the max and the player levels up, they can choose to upgrade their base stats. I also adjusted enemy healths to reflect the addition of a new weapon, by increasing the health of the goblin enemy that spawns a few waves after the skeleton enemies spawn. 

## Final Devlog


1. Knightmare Survival is a designed to be a Vampire Survivors game where the goal of the player is to survive as long as possible through defeating endless increasingly stronger enemies and leveling up to gain stronger weapons & stats. Player will move around a large arena, auto attack enemies, collect experience points from defeated enemies, and choose randomized weapon/stat upgrades. The implemented features in the verticle slice includes infinite wave with enemy scalings, multiple enemies that starts spawning at different times of the game including a boss, experience point drops, pick ups like food that heals you and magnet that sucks in all the experience point, and 2 weapons. These things match with what was discussed in the original vertical slice plan to create a vampire survivors-like game. These features also define what the full game would look like, as the overall game mechanics are all playable in this verticle slice as described earlier. In a full game, the only thing that would differ is more enemy variety as time goes on, more weapon options, other upgrade options, and a more thought out balancing.
2. The rendering effect that I decided to include that interacts with gameplay is a post processing effect that shows red vignette controlled by LowHealthPostProcessing.cs script when the player health becomes low. The script listens to PlayerHealth.onHealthChanged from PlayerHealth.cs, calculates the player's health percentage, and when health drops below the threshold, it increases the vignette.intensity on a URP Volume. 
3. 
    1. I think breaking down the project into modular aspects definitely helps, because one of the biggest challenges to building something as big as a game is the mass amount of things we have to engineer, and without a process to break it into smaller components, it quickly becomes overwhelming. So with that being said, I definitely do plan on using both bubble diagrams an dtask step breakdowns as the bubble diagram visualizes how systems and components connect, while task breakdowns create lays out small and completeable steps to follow to make the game actually buildable. 
    2. Breaking down a large project into small steps gives us approachable places to start. If I were to just think about for example how a weapon needs to shoot out randomly around the player, bounce around the boundary of the camera, and the player needs to obtain it from upgrade, and it needs to be upgradable after obtaining, that becomes very overwhelming to implement. But breaking it into smaller steps that work independently makes this task approachable. 
    3. The plan relates to this verticle slice project because breaking it down into smaller steps is what helps me implement some of the features. So I think that's what I want to make sure to do for any projects I take on in the future. However, not everything was smooth this quarter, because I had much more academic responsibilities for other classes this quarter, and it's hard to find time blocks to work on this verticle slice in a non cramming fashion where I work a little bit everyday. A part of this is also ofcourse due to my own poor time management, so I think it's a combination of these two things. I want to avoid this in the future by not taking 5 classes simultaneously again, especially with junior year college being all upper division classes. 
## Open-source assets
- [Health Bar UI](https://assetstore.unity.com/packages/2d/gui/icons/gui-parts-159068)
- [Attack VFX](https://untiedgames.itch.io/super-pixel-effects-gigapack)
- [Environment Tilemap](https://backterria.itch.io/the-roguelike)
- [Character Sprite](https://zerie.itch.io/tiny-rpg-character-asset-pack)
- [Card Upgrade](https://cafedraw.itch.io/fantasy-card-assets)
- [PickUp Food Sprite](https://henrysoftware.itch.io/pixel-food)
