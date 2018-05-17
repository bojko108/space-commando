# Space commando

The game is designed for standalone build. There are options for saving and loading game progress. Each character's position, health level and movement mode along with finished tasks by the player can be saved and loaded later.

## Contents

* [Game](#game)
  * [Main Game Menu](#main-game-menu)
  * [Gameplay](#gameplay)
* [Game Setup](#game-setup)
  * [HUD](#hud)
  * [Environment setup](#environment-setup)
  * [Player](#player)
  * [Enemies](#enemies)
    * [Base commander](#base-commander)
    * [Commander](#commander)
    * [Soldier](#soldier)
    * [Worker](#worker)
  * [Main Control Room](#main-control-room)
  * [Storage Room](#storage-room)
  * [Spaceship](#spaceship)
  * [Medpacks](#medpacks)
* [Additional Scripts](#additional-scripts)
  * [Menu](#menu)
    * [MainMenu](#mainmenu)
    * [OutroScene](#outroscene)
    * [PauseMenu](#pausemenu)
  * [Managers](#panagers)
    * [GameManager](#gamemanager)
    * [EnemiesManager](#enemiesmanager)
    * [TasksManager](#tasksmanager)
  * [SaveLoad](#saveload)
    * [GameSaveLoad](#gamesaveload)
    * [GameProgress](#gameprogress)
    * [Character](#character)
    * [Player](#player)
    * [Enemy](#enemy)
    * [Vector](#vector)
  * [Helpers](#helpers)
    * [EventManager](#eventmanager)
    * [Extensions](#extensions)
    * [MapProperties](#mapproperties)
    * [Import](#import)
    * [MeshScripts](#meshscripts)
    * [Resources](#resources)
  * [BulletScript](#bulletscript)
  * [ObjectPooler](#objectpooler)
  * [ShipEngineScript](#shipenginescript)
  * [WorkersSafePoint](#workerssafepoint)

# Game

### Main Game Menu

Options for:

* Start a new game
* Continue from last save
* Delete save
* View controls
* Quit game

### Gameplay

After a fierce space battle the player manages to escape but his spaceship is badly damaged. His only chance is to look for spare parts for the ship in an old abandoned base, located on a nearby planet. The problem is the base has been taken over by hostile aliens.

The player lands his spaceship near the old base and is now searching for supplies and spare parts to repair it. Player's ship has a broken Dark Matter Module, which can be found in the Storage Room. The game starts at this point.

When the game starts, the player is running out of oxygen and has to find the Main Control Room to turn Base Life Support Systems on. The access to the Main Computer is blocked by a password. The player has to decipher the password in order to login, turn Life Support Systems on and find the Storage Room location. When the player is near the Main Computer, the game waits for him to enter the password. After the player has the password, his oxygen level is set to full and no longer decreases. The minimap is also updated: `Buildings` layer and Storage Room are added to the map. Enemies detect distance will be increased.

After the player locates and takes the Dark Matter Module he can return and repair the Spaceship. At this point enemies detect distance will be again increased and `Base Commanders` will be spawned.

When the player has the Dark Matter Module and is near the spaceship he can start repairing it. In order to do that he must stay near the ship for at least 10 seconds or the process will be interrupted. After the ship is repaired, the player can get on board and leave the planet for good.

# Game setup

## HUD

* Minimap - shows important locations and directions to the player
* Oxygen bar - shows current oxygen level
* Health bar - shows current health level
* Tasks: shown when `TAB` key is pressed - list of tasks which the player has to complete
* Pause Menu: shown when `Escape` key is pressed - pauses the game and displays a menu with several options:
  * `Resume` - resumes the game
  * `Save and Exit` - saves the game at current state and returns to the main menu
  * `Main Menu` - closes the game and returnes to the main menu - current progress will be lost
  * `Quit` - quits the game - current progress will be lost

## Environment setup

* Real buildings imported with a script - source spatial data is stored in `Assets/Maps/Level3.txt` file
* Import map data using the script for importing from `GeoJSON` asset:
  * The unique ID is stored in `osm_id` property
  * The extrusion parameter is stored in `height` property
  * Put all buildings in layer `Buildings`
  * Combine buildings in one mesh
  * Add Mesh Collider using the script for generating mesh colliders
  * Add Material using a script
  * All buildings and base border are set to `static`
  * Create ground object and build the NavMesh

## Player

Imported from Unity Assets - FPS Player Character.

* Gun and Body
* Second camera for the minimap (orthographic projection) - renders data only from layers `Minimap` and `Buildings`.
* Sphere for the minimap (layer must be set to `minimap`) - it's redered only in the minimap
* Player components:
  * Audio Source
  * Rigidbody
  * Character Controller
  * First Person Controller Script
  * `PlayerShootingScript` (attached to the gun) - responsible for shooting logic. Player's gun fires laser bullets stored in `ObjectPooler` script.
    * Damage per shot: 20 - damage to the enemy when shot
    * Fire rate: 0.2
  * `PlayerHealthScript` - responsible for managing player's health and oxygen levels. Player's oxygen level will drop down (from `80` to `0`) until the player activates the Life Support Systems. `0` level of oxygen will decrease player's blood level. While the volume of oxygen is running low, the player's camera smooth parameter will also decrease (from `5` to `1`):
    * Health: 200 - starting health level
    * Oxygen: 80 - starting oxygen level
    * Walking speed: 5
    * Running speed: 16
  * `RadarScript`: responsible for drawing minimap on the screen
  * `ZoomInScript`: zooms in when the player right click with the mouse

## Enemies

There are four types of enemies moving around the base using NavMesh. If the player is close enough to be detected or is shooting at them, they will attack.

### Base commander

Base commanders are spawned near the spaceship after the player has taken the Dark Matter Module.
Components:
  * Animator - responsible for managing animation states. There are several states:
    * `walk` - default
    * `attack` - triggered when the player is in range for attack
    * `death` - triggered when the enemy is dead
  * Audio Source
  * NavMesh Agent - for navigating around the base
  * Rigidbody
  * Capsule collider - represents the physical presense of the enemy
  * Sphere collider - trigger - responsible for detecting the player and switching to attack mode
  * `EnemyMovementScript` - responsible for moving the enemy - setting destinations on the NavMesh
    * Detect distance: 60 - minimum distance to detect the player
    * Wander radius: 100 - maximum travel distance for new destinations
    * Wander time: 0 - time to wait before a new destination is set
    * Is Chasing: false - is the enemy currently chasing the player or not
    * Is Scared: false - is the enemy currently running away from the player
    * Stop: false - stop moving around
    * Walking speed: 3
    * Running speed: 5
  * `EnemyHealthScript` - manages enemy health level
    * Starting health: 1000
    * Death sound - played when the enemy is dead
  * `EnemyAttackScript` - responsible for attacking the player
    * Time between attacks: 3
    * Attack damage: 40

### Commander

Components:
  * Animator - responsible for managing animation states. There are several states:
    * `walk` - default
    * `attack` - triggered when the player is in range for attack
    * `death` - triggered when the enemy is dead
  * Audio Source
  * NavMesh Agent - for navigating around the base
  * Rigidbody
  * Capsule collider - represents the physical presense of the enemy
  * Sphere collider - trigger - responsible for detecting the player and switching to attack mode
  * `EnemyMovementScript` - responsible for moving the enemy - setting destinations on the NavMesh
    * Detect distance: 60 - minimum distance to detect the player
    * Wander radius: 300 - maximum travel distance for new destinations
    * Wander time: 0 - time to wait before a new destination is set
    * Is Chasing: false - is the enemy currently chasing the player or not
    * Is Scared: false - is the enemy currently running away from the player
    * Stop: false - stop moving around
    * Walking speed: 5
    * Running speed: 13
  * `EnemyHealthScript` - manages enemy health level
    * Starting health: 300
    * Death sound - played when the enemy is dead
  * `EnemyAttackScript` - responsible for attacking the player
    * Time between attacks: 3
    * Attack damage: 20

### Soldier

Components:
  * Animator - responsible for managing animation states. There are several states:
    * `walk` - default
    * `walk-attack` - triggered when the player is in range
    * `gethit` - when the player shoot at the enemy
    * `death` - triggered when the enemy is dead
  * Audio Source
  * NavMesh Agent - for navigating around the base
  * Rigidbody
  * Capsule collider - represents the physical presense of the enemy
  * Sphere collider - trigger - responsible for detecting the player and switching to attack mode
  * `EnemyMovementScript` - responsible for moving the enemy - setting destinations on the NavMesh
    * Detect distance: 50 - minimum distance to detect the player
    * Wander radius: 70 - maximum travel distance for new destinations
    * Wander time: 0 - time to wait before a new destination is set
    * Is Chasing: false - is the enemy currently chasing the player or not
    * Is Scared: false - is the enemy currently running away from the player
    * Stop: false - stop moving around
    * Walking speed: 5
    * Running speed: 15
  * `EnemyHealthScript` - manages enemy health level
    * Starting health: 100
    * Death sound - played when the enemy is dead
  * `EnemyAttackScript` - responsible for attacking the player
    * Time between attacks: 1
    * Attack damage: 10

### Worker

Workers are spawn only from two points: one in the Main Control Room and another in the Storage Room - they will not attack but run away from the player:

Components:
  * Animator - responsible for managing animation states. There are several states:
    * `walk` - default
    * `run` - triggered when the player is in range
    * `gethit` - when the player shoot at the enemy
    * `death` - triggered when the enemy is dead
  * Audio Source
  * NavMesh Agent - for navigating around the base
  * Rigidbody
  * Capsule collider - represents the physical presense of the enemy
  * Sphere collider - trigger - responsible for detecting the player and switching to attack mode
  * `EnemyMovementScript` - responsible for moving the enemy - setting destionations on the NavMesh
    * Detect distance: 30 - minimum distance to detect the player
    * Wander radius: 10 - maximum travel distance for new destinations
    * Wander time: 0 - time to wait before a new destination is set
    * Is chasing: false - is the enemy currently chasing the player or not
    * Is scared: false - is the enemy currently running away from the player
    * Stop: false - stop moving around
    * Walking speed: 2
    * Running speed: 20
  * `EnemyHealthScript` - manages enemy health level
    * Starting health: 100
    * Death sound - played when the enemy is dead

## Main Control Room

The player needs to find the passsword in order to login and switch Base Life Support Systems back on.

Components:
  * Audio Source
  * Sphere collider - trigger
  * `TasksManagerScript` - responsible for emitting events

## Storage Room

The Dark Matter Module is located in that room.

Components:
  * Audio Source
  * Sphere collider - trigger
  * `TasksManagerScript` - responsible for emitting events

## Spaceship

When the player is near the ship and has the Dark Matter Module, he can start repairing it. In order to do that he must stay near the ship for at least 10 seconds or the process will be interrupted.

Components:
  * Audio Source
  * Sphere collider - trigger
  * `TasksManagerScript` - responsible for emitting events
  * `BrokenEngineScript` - responsible for animating ship's engine

## Medpacks

There are several medpacks around the base. When the player walks through them, his health is reset to its starting level. The medpacks are also shown on the radar.

Components:
  * AudioSource
  * Sphere collider - trigger
  * `MedpackScript` - responsible for restoring player's health

# Additional Scripts

## Menu

Scripts used for loading scenes or pausing game.

### MainMenu

Used for starting the game, loading or deleting a saved game or checking game controls.

### OutroScene

Played when the game finish.

### PauseMenu

Pause menu can be activated only if the player is not near an important area such as the Ship, Main Control Room or the Storage Room.

## Managers

### GameManager

This is the main script for managing all game states and player's progress in the game.

### EnemiesManager

Used for spawning and creating enemies. All enemies are spawn at the start of the game.

### TasksManager

Responsible for emitting various game events.

## SaveLoad

Responsible for saving and loading game progress and characters.

### GameSaveLoad

This script is used for saving and loading game progress.

* `LoadSavedGame` - static property set from the main menu
* `IsPlayerDead` - static property used when loading outro scene
* `FileName` - save file name

### GameProgress

Game progress parameters:

* `IsPlayerDead`
* `IsMainControlRoomFound`
* `IsPasswordFound`
* `IsStorageRoomFound`
* `IsDarkMatterModuleFound`
* `IsSpaceshipRepaired`
* `Player` - saved player
* `Enemies` - saved enemies

### Character

Stores GameObject's `Tag`, `Position` and `Rotation`.

### Player

Extends `Character` and saves player's Health and Oxygen levels.

### Enemy

Extends `Character` and saves enemy's Health level and movement mode.

### Vector

Stores `Vector3`'s `x`, `y` and `z` components.

## Helpers

### EventManager

Responsible for attaching and emitting events:

* `PlayerEnteredImportantArea` - when the player enters important area such as: Computer Room, Storage Room or the Spaceship
* `PlayerExitedImportantArea` - when the player exits important area
* `MainComputerFound` - when the player finds the Main Control Room
* `StorageRoomFound` - when the player finds the Storage Room
* `DarkMatterModuleFound` - when the player finds the Dark Matter Module. The spaceship can be repaired now
* `SpaceshipFound` - when the player finds the spaceship - if the player has the Dark Matter Module the game will end
* `PlayerDead` - when the player dies
* `GameFinish`
* `ResumeGame`
* `PauseGame`
* `SaveGame`
* `GoToMenu`
* `QuitGame`

### Extensions

Some additional functions:

* `FindChildrenByName` - Find all children of the Transform/GameObject by name (includes self)
* `FindChildrenByTag` - Find all children of the Transform/GameObject by tag (includes self)

### MapProperties

For storing map origin and scale. When spatial data is imported all coordinates are reduced due to floating point precision problem.

### Import

For importing spatial data - scripts for importing data from `GeoJSON` or `GML` formats.

### MeshScripts

For building and extruding meshes.

### Resources

Static class with references to all tags, layers, events and messages used in the game.

## BulletScript

Responsible for managind collisions between bullets and other game objects.

## ObjectPooler

Responsible for managing bullets.

## ShipEngineScript

Animates ship engine status and sound.

## WorkersSafePoint

Attached to the comamnder's spawn points - all scared workers will run to a random spawn point and when near will be destroyed.