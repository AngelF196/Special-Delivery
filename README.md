[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-22041afd0340ce965d47ae6ef1cefeee28c7c493a6346c4f15d667ab976d596c.svg)](https://classroom.github.com/a/r7TEDUTZ)
# Final Project
## Check-in
### Angel's Devlog
At this point, I have written the code for the player s movement, and although it is not finished I think that it has progressed smoothly and is very close to completion. I split the movement into two scripts, one dealing with normal and basic movement, and one dealing with the player s dashes and dives. I ve made multiple variables that are easily changeable for tweaking like the player s  _horizontalSpeed ,  _maxFallSpeed ,  _dashPower , and many others. This allows for simple player experimentation in the editor. There s still a lot of work to be done in the engine and the code. I still need to make a tilemap for easy level building, an animation tree for the player for when we make the sprites, and implement audio into the game. The player is the only fleshed-out game object and even then it lacks any animation or sprites. As much work as the player still needs, there are also many other prefabs that I need to make and mechanics to implement. I plan to implement conveyor belts, springs, proper jumps, particle effects, and some other things as said in the proposal. Speaking of the proposal, I think it has helped in breaking down the project into steps, very similar to the breakdowns from the earlier homework assignments. Unfortunately, due to the open-ended nature of this assignment, the proposal wasn t as detailed as previous breakdowns. I had to work out the logistics on how to implement the mechanics that I mentioned in the proposal and the movement code has taken up most of my time due to how much more complicated I expected it to be. Overall, I expect most tasks after the movement to be fairly simple to implement. We ve been using a Google doc with the tasks outlined and have been marking what tasks have been done, what still needs to be started, and the priority of all the tasks. I think the game was a bit too ambitious at the beginning and we ve been struggling a bit to reach our expectations. However, it is definitely possible and we re working on it. I think in the future we ll be a bit more mindful of the scope of our projects and give more detailed steps to take to implement what we set out to do. Overall, I m optimistic that we ll be able to make this game shape our vision.

### Student Name 2
### Akai Strong
These are the C# scripts and GameObjects that I've contributed to the project so far;
- ParentNPC: This is the parent NPC class for the two types of NPCs present. This will be used for our giving and receiving package NPCs. At the moment, the parent class is being used for the giving and receiving alike for stand-in. I'm currently working on the mission acceptance code to then differentiate the two.
 - Methods in this class are:
   - Enum NPCstate {}
   - IdleState()
   - WavingState()
   - InteractState()
   - UpdateState() 
   - An NPC GameObject is used to be attached by the ParentNPC cting as a base NPC in the game. Once we continue further the NPC types will be split into previously mentioned.

- NPCInteraction: This is used to open the dialogue box assigned to an NPC once the player interacts with it. 
 - Methods in this class are:
   - Interact()
   - OpenDialogue()
   - CloseDialogue()
 - Important Variables:
   - public GameObject dialoguePanel;
   - private bool hasInteracted = false;
   - private bool dialogueActive = false;
- This is attached to the NPC GameObject to allow it to use a textbox and the dialogue can appear once it's attached.

- PlayerInteraction: This allows the player to press E which checks whether or not the player is close enough to an NPC to then interact with it.
 - Methods used:
   - OnTriggerEnter2D() 
   - OnTriggerExit2D()
   - Update() 
 - Important variables:
   - public KeyCode interactKey = KeyCode.E;
   - private NPCInteraction currentNPC;
 - This is attached to the Player Object as it gives ability to press interaction button to interact with NPCs.
- Game Objects created:
  - NPC Loofy
  - NPC Ace
  - Dialogue Panels for both

I found the way that we broke down the project for this assignment to be way more effective since this is going to be on a much larger scale. The previous exercises worked effectively due to the smaller scale of them but when things begin to become convoluted, it's more effective to break things down into very minute details. As of right now the proposal has been more than helpful when it came to giving me the right idea of what we're doing for this project. The only issue is that the proposal didn't give all the information on how to exactly build out NPC interaction which was to be expected especially since it was just an outline of what the game was going to be. Perhaps, as we get further into the game the proposal document will keep us reminded of the goals that we want to include and it'll prevent us from going beyond our initial goals. We didn't use a Trello but we had a google doc in which we highlighted the goals in different sections; To-Do, In Progress, and Completed. When it comes to future projects, I honestly thing having a dedicated tool such as Trello would be more effective when planning out what needs to be due by when. I've realized that having a bunch of classes and outside events like work heavily impact how I need to approach group assignments. Having Trello would not only keep us on track, it would actively remind me to complete things through the many notifications that I've gotten from them over the past few months on different projects.



 
## Final submission
### Group Devlog
Our code utilizes three out of the five concepts available for us to use.
 - Finite State Machine
  - 
 - Inheritance with Polymorphism
  - For our project we realized that there would need to be two different types of NPCs that functionally are extremely similar but would need to have key differences. For example, one of our NPCs would need to be able to start the timer and our other one would need to be able to end the timer. It would be much easier to have two different NPCs, one being the StarterNPC and the other being the EndNPC with the functionalities with the timer built into them. Everything about them would be the same though with our FSM and the need to change sprites is important for each so inheriting them into each subclass was important. So everything was inherited when handling states but the StarterNPC carries the StartRace() while the EndNPC carries the StopTimer() functionality.
 - Unity's ScriptableObjects
  - There is dialogue that appears once the player presses "e" which interacts with NPC. Although, a smaller project it would be foolish to carry all of our text simply with a typical TMP_Text. So we created a DialogueScriptableObject that would allow for us to easily create dialogue Scripts that can be altered within the inspector for easy use of changing and creating dialogue for each NPC that would need to go in our world.

### Student Name 1
Put your second Devlog here.

### Akai Strong
Since the check-in this is what I've contributed to the project.
- TimerManager: This is where the code to manage the timer and how it works. It's the same timer that is displayed in the top left of the gameplay.
 - Variables in this class are:
   - float currentTime;
   - TMP_Text timerText;
   - bool isTimerActive;
 - Methods in this class are:
   - StartTimer()
   - StopTimer()
   - UpdateTimerDisplay()

- StarterNPC: This is where the NPC that gives the package to the player will have their functionality. They start the timer and inherit everything else from the ParentNPC class.
 - Variables in this class are:
   - TMP_Text timerText
 - Methods in this class are:
   - StartRace()
   - HandleState()

- EndNPC: This is where the NPC that receives the package from the player will have their functionality. They end the timer and inherit everything else from the ParentNPC class.
 - Methods in this class are:
   - OnTriggerEnter2D(): This is where the timer stops

- DialogueSO: This is where the dialogue scriptable object is located in order to create easy dialogue to give to the NPCs
 - Variables in this class are:
   - string dialogueText

- I also worked on implementing the sprites into the animator
 - PlayerDash
 - PlayerFall
 - PlayerIdle
 - PlayerJump
 - PlayerRun
 - PlayerStopping



Teammate: Angel Flores

Score: 2

Reason: Angel was a fantastic teammate for this project, he put his fair share into the assignment and was very communicative. At no point did our communication go away and he was very easy to work with. I think he deserves full points!

## Open-source assets
- City any external assets used here.
 - Images used from One Piece owned by Toei Animation & Echiiro Oda
