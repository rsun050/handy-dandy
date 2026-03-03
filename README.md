# handy-dandy
## Check-In
### Team Prompt
Our game allows the player to pick up items (novel!). A player can pick up an item when the following conditions are both true:
  - The player is close enough to the item 
  - The player is looking at said item

The game detects if a player is "looking" at the item by Raycasting from the camera in the direction of the camera's orientation for a certain distance. This Raycast only detects Colliders that are attached to a GameObject that is on the Item layer, so we don't have to worry about the raycast hitting something like an NPC or other obstacle. 

This "is the player looking at an item" code also serves for displaying item information in the UI, to let the player know what they're looking at/about to pick up. Once we know what the player is looking at, we can fetch the item's data and display any relevant information.
<hr>

### Individual Prompts
#### Ruth Sun 
I contributed the player and camera controllers, some UI for items, code for managing player inventory and picking up items.

The proposal has been somewhat helpful - keeps the idea straight, although some implementation details have been tricky to figure out, particularly for the inventory. Although implementation details have changed, overall architecture plans haven't changed.

In future games I think making sure people know specifically what they're doing (more than just 'I'll make [XYZ]'), especially for people newer to what they're doing, would be helpful. For more involved processes like coding, thinking about and refining the design, along with considering all common cases that could take place in gameplay would be important. Communications and regular check-ins also...

#### Armando Topete
#### Michael Lopez

<hr>

## Final Submission
### Group Devlog
### Ruth Sun 
### Armando Topete
### Michael Lopez

## Open-Source Assets
