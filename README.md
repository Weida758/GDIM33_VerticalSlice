# GDIM33 Vertical Slice
## Milestone 1 Devlog
1. I utilized the visual scripting graphs to handle the display and updating of the player's health UI to the screen. In the extension of the Update node in the HealthUIGraph, I have object variables that references the PlayerHealth script, which allows me to get the current health and max health of the player, divide them, and then set health bar fill value with that resulting ratio. It also sequentially sets the health text inside the health bar in the format of "currentHealth/maxHealth".
2. I added a finite state machine break down of my game's game states, which are separated into PausedState, PlayingState, and LevelingUpState. These states handles what should happen when the game enters that state. My current state machine graph implementation doesn't cover leveling up state yet, but I have used it already for pausing and playing. How it works is, custom events are defined inside the transitioning of the states, which calls the Pause() or Resume() method respectively from a custom C# script I created, when that event is called, and then transitions from the pause state to the playing state or vice versa. Inside the actual states, during the entering of the state, I attached a node which sets the TimeScale of the game correspondingly based on the state (pausing have a timescale of 0 while playing have time scale of 1). During the update method, the nodes check for the input of the escape key on the keyboard, and if that is detected, the custom event defined inside the transitions will be called, and causes to pause/unpause & transition into the opposite state. This interacts with the other systems of our game by completing stopping/resuming all activities of the player, enemy, and spawning logics. 
<img width="1680" height="1165" alt="GDIM 33 Game Breakdown (1)" src="https://github.com/user-attachments/assets/f7f04cc5-e68a-4f24-be8e-ac2cb3dc08ad" />
## Milestone 2 Devlog
Milestone 2 Devlog goes here.
## Milestone 3 Devlog
Milestone 3 Devlog goes here.
## Milestone 4 Devlog
Milestone 4 Devlog goes here.
## Final Devlog
Final Devlog goes here.
## Open-source assets
- Cite any external assets used here!
