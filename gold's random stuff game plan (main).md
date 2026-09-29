# 📙 project stuff

# **gold’s random stuff game**

navigate through tabs to find details related to one specific thing

if you want to suggest something to be added fill out [this](https://forms.gle/krfGRPbBwvqPDfev9) form

unless you’re an editor, then just add it idk

# **development status (do not edit these)**

**estimated break end: waiting for claude pro (waiting for debit card)**  
*notify me if this is not here after about a week in the taking break phase*  
*this can change at ANY TIME so check back periodically*

**overall \- great**  
**dev rate \- locked in (norm)**  
**dean status (live) \- school**  
	player rig \- finished  
	ship functionality \- getting rough  
	ai systems \- not reached  
	the zenith update \- not reached  
	gold incorporated update \- not reached  
	closed beta release (bugs & suggestions) \- not reached  
	open beta release (app lab, v1) \- not reached  
	BlueBlaze Empire Update (v1.2) \- not reached  
	full release (v2, complete polish) \- not reached

- go to (year).(month).(day):(hotfix) format, with advanced version names being actual build, like v2.1 (seen inside settings and on your SFCD when you’re on either a 

# 🖥️ dev stuff

# **dev stuff (subject to change)**

**scripts**

* **ship.cs**  
  * main ship controller, only one per ship  
  * takes in input from other scripts  
  * controls velocity of the rigid body  
  * controls fuel values & others (like if it has something with it)  
  * multiple presets for the integer values  
    * zenith  
    * orbital station (tbd)  
    * airship (tbd)  
  * autopopulates if you choose a preset  
  * integer settings are grayed out with presets, but if autopopulate fails for some reason you can manually put in where the other scripts (ex. [engine.cs](http://engine.cs), [naelle.cs](http://naelle.cs))   
  * more to come (i have never done smth like this)  
* **engine.cs**  
  * controls force outputted by nacelles  
  * communicates with ship.cs and [throttle.cs](http://throttle.cs)  
  * tells [nacelle.cs](http://nacelle.cs) how much force to output  
  * has multiple statuses  
    * online  
    * offline  
    * c-offline (happens when discharge lever is pulled)  
    * overridden (via overrider)  
      * this will make the engines supercharged, but lock the throttle and steering yoke (hard lock)  
      * the throttle’s screen pulses red during this  
      * it will attempt to turn to the nearest star, and send the ship going there  
      * when this happens, your ship is cooked and you need to get out of there NOW  
    * destroyed  
    * overheating (with percentage to thermal runaway)  
    * thermal runaway (this is way worse than a nacelle one, this can and will blow up your entire ship)  
      * if you are in the engine room, you will hear the engines being really loud and working really hard and irregularly, and even having the metal on them looking a little red  
      * if you do not let the engines relax, the engines will enter the next state of thermal runaway  
      * if you are in this state for 3 minutes, the type 3 ai will be erased and you will be left with a capsule  
      * during this if you are in a ship with a type 3 ai you can notice the voice modulator glitching  
    * thermal runaway (fatal)  
      * the engines will be in OVERDRIVE during this, when the engines are in this state there is no going back  
      * you only have about a minute to get out before the engine’s internals reach supercritical (even though its impossible, it happens due to the composition of the engines, a closely guarded secret), because as soon as it does, a quantum superdetonation of the engines will occur (so loud and devastating it gives sound a medium to travel through for a short amount of time), resulting in explosion of the zenith’s reactor core almost immediately after, strengthening the explosion with elements not even known to exist, so if you’re near the explosion you will be poisoned  
    * more to come maybe  
  * more to come  
* **throttle.cs**   
  * controls how much force is outputted by the engine  
  * has e-brake (suppresses the engine from the nacelles and activates physical brakes)  
  * controls brake value (when the throttle is all the way back it automatically stops outputting power and brakes as the aggressiveness value suggests, more means more abrupt)  
  * has screen below where the throttle is (the stick), which when you push the throttle from the starting position (0) (green means max, red means minimum, follows the spectrum, light blue \[kinda sky blue\] means hyperdrive is active, so you always go max throttle)  
  * more to come maybe  
* [**nacelle.cs**](http://nacelle.cs)  
  * goes on every nacelle the ship has (this is where the engine outputs its force to)  
  * has multiple status effects  
    * online  
    * offline  
    * overheating (will show percentage until thermal runaway in inspector, more means closer, happens after about 10 minutes)  
    * thermal runaway (critical, you can stay in this for about 5-8 minutes before explosion, if you are in this for 3 minutes the type 3 ai can be erased)  
    * i-comprimised (meaning the integrity has hit 0\)  
    * destroyed  
    * gone (the nacelles in a future update can literally break off the ship, or be partly broken off the ship or bent, physics-driven)  
    * more to come maybe  
  * can overheat if the ship is in hyperdrive for more than 10 minutes  
  * thermal runaway makes the nacelle at risk of explosion, the computer can make an estimate time from the game’s actual time it sends to gpt-oss, the otherwise light blue accents on the nacelle turn a not so good dark neon red during this, and the ship will feel like its falling apart (you can hear some not-so-good sounds coming from the hull)  
    * if the ai is a type 3 ai (goes inside of the engine room) during thermal runaway, the core can overheat and erase whatever ai is inside if the engines are in thermal runaway for too long (about 3 minutes)  
    * the ai will not notify you of this if it is infected with an incorrectly made upgrade (only with type 3 ais so far), which will make it think all crew are intruders and you will have to stay out of sight, which is extremely difficult, and take out the upgrade, but type 3 ai hulls have a voice modulator that can rotate around itself as to protect the ship and the core ($$$), which means you have to give it something to lock on if you want to get it out  
    * this rotation mechanic is only for the ai’s mainframe (where you put the ai core in, inside the engine room)  
  * integrity compromised means it is very unsafe to keep the engines on, it will break off the ship or be in a state where the ship is no longer pilotable and you will need to get towed by some other ship (any ship can do this in the gold inc and the blaze empire lineup, but it may be a little slow depending on which ship, you can use the hyperdrive during towing but its risky as it can snap the towing thing, which is a projectible material that i do not know exactly what it is)  
  * more to come  
* [**aicore.cs**](http://aicore.cs)  
  * handles the ai core object  
  * not used when inside of the mainframe, or inside of the arms which keep it in with type 2 cores  
  * you can hold this or wield it on your back  
  * when the ai is in this state, it cannot wirelessly connect to anything until you connect it to a ship, it can only talk and see  
* **more to come soon**

**update timeline**  
*a line of time of update*

- the ship functionality update  
  - includes ship functions  
  - not done  
- the ai update  
  - includes ai functions, with the system prompt being done and a screen which shows the tools that were called  
  - not done (yet to be reached)  
- the zenith update  
  - includes the zenith and its many functions, by far the longest update in the alpha stage  
  - not done (yet to be reached)  
- gold incorporated update  
  - includes gold incorporated ships and places you can go to  
  - not done (yet to be reached)  
- open beta release  
  - release onto app lab after some polish  
  - not done (yet to be reached)  
- BlueBlaze Empire update  
  - add the blueblaze empire into the main branch  
  - the empire will still exist in LAB as other updates are developed before this update, however  
  - not done (yet to be reached)  
- full release (v2 but 2026.4.17:3)  
  - the full release of the game, complete polish on backend and frontend  
  - by far the biggest update in this lifetime, will go from actual updates to mainly suggestions (main updates will still sometimes happen, but suggestions will be the biggest thing to be taken into account  
  - will go from v1.2.1 (example) to 2026.4.17:3 for example, the advanced version will show v2.3.3 (build 763\)  
  - goes back to v1 after v2 is reached  
  - yet to be reached, will have to do some legal stuff (for some reason) before this happens  
- 

# 🤖 ai models

**ai models**  
*models that ai (alt models for when limits reached)*

**code developing (with plan)**  
*developing thats code*  
opus: gemini 3.1 pro (similar/better)  
sonnet: (to be determined, likely something that performs similar to sonnet)  
haiku: gpt-5-mini (free model)

**planning**  
*bro i dont have a note for this one*  
opus: not needed  
sonnet: either gemini 3.1 pro or gpt  
haiku: likely not needed

**making other stuff**  
*for other stuff (woah)*  
opus: gemini 3.1 pro  
sonnet: no idea  
haiku: gpt-5-mini

**non-alt models**  
*offikal modal*

**coding (unity)**  
sniper: opus  
regular: sonnet  
cost-saver: haiku  
**coding (other)**  
exclusively Sonnet, Opus when in dire situations

**limit reached**  
opus: gemini 3.1 pro preview  
sonnet: codex  
haiku: gpt-5-mini

**limit reached (again)**  
opus: qwen3-coder (any model)  
sonnet: gpt-oss:120b  
haiku: gemma 4

# 📝 forms

**the forms**  
*are they forms guys?*

[**suggestion form**](https://forms.gle/sjPwDVD3rYQbiHpN9)  
*form of suggestions*

- you fill it out when you want to suggest something  
- only evaluated by me  
- not much else to say

[**test pilot application**](https://forms.gle/hwfZfgWBJ4P6fxo7A)  
*its an application for test pilot*

- you fill out the questions, then submit  
- reviewed by etps and head fighter pilots  
- if you get in, you get to test updates  
- elite test pilots are not the same, you can’t even apply for elite test pilot  
- baseball

[**fighter pilot application**](https://forms.gle/E8UfQZtSqRcU8ozi6)  
*air force*

- you fill out the questions and submit  
- this one is hard to get into; you have to show dedication before you even apply, maybe in discord or something  
- you are unlikely to be considered if you do not follow what is said above  
- if you do get in, you start out as a jr. fighter pilot and you work your way up  
- basketball

# ❔ story mode

# **story mode**

*mode for story*

**overview**  
*view that is over*

- an offline story mode you can play  
- based on whatever team you choose (tbd), you go on a story of that team  
- after you finish one team’s story, you can change your team in that story only  
- more to come maybe

**different stories**  
*blue pill or red pill?*

- gold inc.  
  - if you choose to be part of gold inc. you get to kind of sandbox, and you get missions once every day, with the upgrades provided to you  
  - from time-to-time there will be some special events where YOU have to fight ships off, these vary  
  - more to come maybe  
- The BlueBlaze Empire  
  - add some as you see fit idk

**more to come**

# ✨ special roles

# **special roles** 

# **(application or invite-only)**

# **difficulty key**

## ***easy \- very easy to get in first try***

## ***meh \- may take you about two apps or more***

## ***hard \- this will take you a while; maybe even months***

## ***extremely hard \- you have to have the determination of frisk if you want to get a role with this difficulty***

## ***you're lucky to even get considered \- self explanatory; winning the lottery is likely easier than getting a role with this difficulty***

## ***you're NOT getting this \- you’re just not getting this, like ever***

## ***invite-only \- only people I (and maybe some ETPs that I choose) invite get these special roles, the difficulty varies***

# **Test Pilots (application only)**

## ***difficulty \- easy***

*apply [here](https://forms.gle/BU77DyzKviEVK8bx9)*

- get to test updates early & provide feedback, also get access to the beta  
- get a cool id stamp on their physical id (in the game) and a badge to put on their character  
- more to come (maybe)

# **Elite Test Pilots (hand-picked, invite-only)**

## ***difficulty \- invite-only (you're NOT getting this)***

*no application*

- get to test the updates before they’re even updates  
- get a cool id stamp that is one of those ‘reflective’ gray but rainbow and my actual e-signature on their id, also have the option to disguise the id (more below)  
- get a special, one of a kind for each ETP, badge (they get to design it)  
- not even a version on the quest store; just a closed apk that i give  
- head admins (get to ban people and use special tools & special ships, ships are for later, after v1)  
- ETPs are able to disguise themselves; they just choose a username via their cool ui, then they are promoted to restart their game, then their username is set to that (similar to Yeeps’ disguising feature for Star Creators)  
- they are also able to disguise and undisguise their IDs while disguised; allowing you to reveal yourself in a very cool way  
  - when you do this, you press your top face button on whatever controller you’re holding it in while disguised  
- direct contact with me (that they already had)  
- 100% more to come

# **Fighter Pilots (application)**

## ***difficulty \- extremely hard***

*no application yet*

- regular mods, extremely selective (very few get in every application period)  
- application is a very small part of how you get in; you have to show interest before applications open (open very rarely)  
- badge with a shield to the left side, and to the right their username and “Fighter Pilot” below, with their rank also shown (more in ranks below)  
- access to the wrench (tbd on what tool, that’s all I could think of), which if you let go of it without throwing it it will float (if you gave it some velocity it will dampen then stop), but if you throw it it falls normally   
- when the wrench is floating, a UI appears to the side of it only the mod can see, which has multiple fields; the first one near the top is the selected player (arrows to switch), then below is the ban type (mic, captain, or regular), and below that is ban duration (1 day to perm, details below in ranks)

# **Jr. Fighter Pilot (start-out rank)**

## ***difficulty stated previously***

- this is the rank you start out as  
- can only ban people up to 3 days with a discord command (not with the wrench)  
- the captain ban bans you from being able to own a ship; you are only able to be part of a crew  
- mic ban bans you from being able to *talk* in voice chat; you still are able to hear people  
- your shield on your physical id is gray, and is a basic shape (no image as of now)  
- more to come

# **Fighter Pilot (normal rank)**

## ***difficulty \- rank-up only (meh)***

- rank achieved by a google form; you will fill the form out when applications (only for jr mods) are open  
- if it is seen you have banned people justly, you will be promoted to this rank  
- unlocks the ability to ban people for however long you desire (except perm) and the wrench to show that you are a regular mod  
- white outline and blue shield fill on id  
- more to come

# **Sr. Fighter Pilot**

## ***difficulty \- rank-up only (extremely hard)***

- rank achieved by a google form; you have to be a Fighter Pilot for a while to get this and prove yourself as a mod  
- unlocks the ability to perm ban without supervision  
- if you have banned a bunch of people (justly), and have been a mod a long time you will be eligible to rank up  
- gray outline and dark blue shield fill on id  
- maybe more to come

# **Head Fighter Pilot**

***difficulty \- invite-only (you're lucky to even get considered)***

- only senior fighter pilots are eligible; only 10 can be head fighter pilots at a time (tbd)  
- required to be readily available, or they may get demoted to senior fighter pilot  
- being demoted from head fighter pilot does not necessarily mean that you were bad as a head one; the role requires a lot of time  
- get a rainbow reflective shield stamp on their id, and the e-signature of an ETP (whoever wants to lead the Fighter Pilots, TBD)  
- able to ban and demote moderators freely; also are the people who accept rank-ups  
- more to come (for sure)

# 📡 chat

# **this is the chat for editors to communicate**

***very cool (etp lounge too)***

**dean \- ill work on the tab, keep this chat open tho so we can still kinda talk**  
**bekham** **\- i need to work on compass**

(add yours if you have editing access)

# 📋 things to copy

**these are some things you can copy to reference in the chat**

## **rank difficulty dropdown \- has the difficulty for each rank (green is easiest, purple is hardest)**

# **todo difficulty (dean stuff) \- difficulty dropdown (purple is hardest)**

# **version dropdown \- shows which things come in which versions**

**more to come (if you add more please put them here for future reference)**

# 🎩 The Toppat Clan

# 🎈 Toppat Airship

# **Toppat Airship** 

# **(v1.1 (the toppat update))**

**Propulsion**

- The airship simply moves around while in the air; if you’re getting off the ground, just push the throttle up, and then you will start going up and forward  
- If you put the throttle all the way down again, you will start going down slowly and you will stop being propelled

**Steering**

- The steering “yolk” doesn’t function like the *Zenith*’s, more like a car’s  
- Turning left makes the airship go left, turning right makes the airship go right, etc

**Drill Pods**

- The airship (from the cockpit) can launch drill pods wherever there is collision detection for these  
- They can **ABSOLUTELY DECIMATE** anything they hit while the drill is on, even without it on, you’re still cooked (not as powerful as supreme dominance though)

# 🛰️ Toppat Orbital Station

# **Toppat Orbital Station**

# **(v1.1 (the toppat update))**

plans for the orbital station

**Launch Sequence**

**Propulsion**

- During the launch sequence, the orbital station cannot move unless it’s airborne. The three boosters are your primary source of propulsion during launch.

**Artillery**

- None of the on-board weapons work until you get into space  
- Collision Course Detection will NOT work until you are in space  
- Supreme Dominance and Big Bomb will NOT work until you are in space and are fully deployed

**InvisiTech**

- The Orbital Station does NOT have InvisiTech by default; you have to find the item and put it somewhere in the station, then you’re able to become invisible. Only works when in space (I mean, you can do it while landed, but do you want the Earth to become invisible?)

**Escape Pods**

- Escape pods aren’t available during launch; what would you even use them for?

**more to be added**

**SPAAAAAAAAAAAAAAAAAAAAACEEEEE**

**Propulsion**

- You can propel yourself even without the boosters; however, not very well  
- Only supports tilting ever-so-slightly, as you are only in orbit

**Artillery**

- Supreme Dominance is a huge beam that is very destructive, but can easily be countered with a reflective object.  
- The Big Bomb has no known counter; when it hits, it causes mass destruction and can take down even a megaship in just a few hits  
- Using the Big Bomb on a starship makes it fall into pieces, and makes you go into an uncontrolled descent if you’re close enough to a planet  
- There are basic laser thingies you can shoot (according to Jewel Baron route) with infinite ammo, if they hit, it goes boom (These are extremely inaccurate even when you lock on, so baraging is your only option)  
- You can lock onto a target. In that scenario, the station will auto-tilt; however, you can easily escape the lock if you move out of range, as the station tilts extremely slowly  
- Locking onto a target works with all forms of defense

**Tractor Beam**

- Even if you’re on Earth, if you have an SFCD, you can call for the Orbital Station to point to your location and beam you back into the hangar.  
- If you don’t have an SFCD, you’ll have to communicate somehow before you go down  
- Anything and everything in the beam, including other people, will be sucked up as well  
- This takes up a lot of energy, but you have a central core, so you have basically infinite energy (version of a nuclear reactor)  
- The beam can also work backwards, as it can beam you down as well

**Gravity Generator**

- Similar to the *Zenith* and other ships, the Orbital Station can generate artificial gravity.  
- The artificial gravity cannot be turned off in the orbital station unless the gravity chamber is destroyed or tampered with; even then, the gravity would just be weakened (aka if someone decides to go inside of it and gets stuck)

# 🔥 The BlueBlaze Empire

# **BlueBlaze Empire** 

# **v1.2 (the blueblaze empire update)**

add tabs below for each ship or special thing or something idk  
add some stuff here regarding some big stuff about the blaze empire

**ship hierarchy (lowest to highest)**

* Ship  
* Transport ship  
* Starship  
* Supership  
* Stormship  
* Destroyer  
* Mothership

**info format**  
**title**  
*note about it (try to be short, optional)*

* features

**general game info**  
*its info about the game bro*

* you can purchase BlueBlaze Empire ships for personal use at the BlueBlaze Empire home base (upon buying, you will also be under the gold inc and BlueBlaze empire umbrella, allowing you cool features) at the BlueBlaze Empire mothership  
* there’s a high chance you will meet BlueBlaze there, so don’t hesitate to say hi or ask questions about anything (probably wont answer)  
* if you are under the blueblaze empire (not gold inc, but you have the opportunity to do them as a gold inc. member), you can do tasks for money to buy more ships  
* in the empire, you can gain xp in order to increase your role, which shows up on a BlueBlaze Empire roles chart *(if the mothership has one, in the guest sector of the mothership with the shop and everything, right?)*  
* If you want a BlueBlaze ship that you can't find on the BlueBlaze Mothership shop, it's probably only on BlueBlaze’s Ship (only he has it, you will have to wait)  
* add more as needed

**ship fleet**  
*i think its a fleet of ships*

* Tier 1-10 and Stormships and Destroyers

# 

# 

# 

# **Roles (Best to Worst)**

* Emperor (only can be me)  
* First hand to Emperor  
* Second hand to Emperor  
* Third hand to Emperor  
* *Guest (for people who are just visiting the mothership, for example to get one of the tiers)*

# **Daily Tasks** 

# *(if you see anything like this then it’s a suggestion from me)*

Tasks include;

* Circling around the ship you are stationed to  
* Doing tasks in the command center  
* Working on ships (doesn't need to be a thing) (i like this idea, it can be a thing, but i’d rather it be in a later update due to complexity, but i’ll add it overtime in the beta version)  
* *Farting (X button) (obviously a joke)*  
* *Fighting NPC ships (they spawn in waves, randomly going towards the mothership)*

# 🚀 Gold Incorporated

# **gold incorporated**

**roles in company**

* gold (thegoldgamer): founder and chief executive officer of gold inc, also owns the game so has some secret dev stuff nobody else gets  
* blueblaze (blueblaze12): chief operating officer and head of gold inc security

**history**

* gold inc starts out as space incorporated, a company which researches things in multiple research facilities  
* the first facility built was space incorporated headquarters, which originally orbited earth, and also has all the main offices along with where space incorporated intelligence agency was headquartered  
* their research led to having enough resources to build the space incorporated mission center, where the gold inc security is headquartered at, made as a research center and a training center for recruits  
* the mission center is also a second headquarters, mainly where all the ships can be controlled from  
* after a long period of time, space incorporated expands across the galaxy and more, building research facilities and more  
* after a while, space incorporated’s original cofounder, who will not be named, leaves the company abruptly (not even the intelligence agency knows why), which leads to space incorporated, even with their multitrillion dollar networth, dissolving with all assets being left as an echo of their former self, with the reactors shutdown and the control panels dusty  
* after a few years or so, gold incorporated is founded above space incorporated’s remains, and all assets were bought back for full market value (big bill, but not a big dent in the money they have)  
* after a few months of paperwork and polishing old facilities, the hq, for the first time in its life, is landed back on a gold incorporated manufacturing facility using its engines (which are not used at all basically) to be disassembled and used for parts  
* the mission center then becomes gold incorporated’s only hq, with parts from the hq used to expand on it  
* gold inc then produced more ships, including the zenith, and allied with the blueblaze empire, giving them better manufacturing and ship computers, along with assistance with engineering new ships  
* after all of this, their net worth is at the octillions and counting  
* the rest is to be written

**ship fleet**  
*see ships/t-ships for more info regarding this*

**commercial fleet (under the Gold Incorporated Transit Division)**

- transport ships  
  - have port for an AI core, supports type 2 and partially type 3  
  - capabilities limited compared to starships, but still has a good amount of them  
  - these are easily mass-produced; you can even print one with a Fabricator (tier II), so long as you have enough room and you use the correct material can (heavy-duty preset)  
- multipurpose starships  
  - supports exclusively type 3, no adapter  
  - made to live in long-term, due to size, repairability, and the number of rooms along with partial modularity  
  - \-sphere ships fall under this category  
  - more to come maybe  
- static megaships  
  - has a built in automated system; not ai  
  - these are commonly used for stores and things, have multiple docking bays  
  - size varies, as there is no template for this, these are all custom-made  
  - have engines which can output a lot of force and are very precise, run off of batteries charged via solar power or by charger ships (multipurpose ships) coming with a different type of power which is extremely efficient (a lot of power with extremely efficient usage), so these are barely needed  
  - maybe more to come

**the battleship fleet (under Gold Inc. Security Division)**

- not for consumers, only given to people in the security division of gold incorporated  
- each ship is custom, so no tiers  
- the thing that sets these ships apart from consumer ships is that they go way faster, have more power storage, have artillery   
- current ships include  
  - zenith  
    - includes hyperdrive  
  - more to come  
- these aren’t really mass-produced ships, each are (again) custom made

**the intelligence agency fleet**

- admin ships  
- quite generic; but go very very fast  
- also have artillery  
- more to come maybe idk, not much to say

**The Destroyers (branded under Gold Incorporated)**

- not much is known about these ships due to their classification  
- you cannot find these in game, only ETPs, specific ones, are able to pilot this  
- this fleet is branded under gold incorporated, rather than the transit or even security division  
- this quite literally deserves its own tab because the lore goes DEEP  
- current fleet includes  
  - Project Supernova  
    - Project Supernova is a starship only brought out in the most dire situations  
    - since creation, it has not been brought out of the HQ’s emergency hangar and has only been used during in-factory testing  
    - if you were to throw it up against the most elite fleet of at least 500 ships, it would come back out of the smoke without even a tiny scratch, with all the ships dealt with  
    - has super destructive weapons equipped on it, which is what makes it powerful (but not the only thing)  
    - check [Project Supernova]() for more information

# 🚢 ships/t-ships

# **ships and transport ships**

***very cool***

**info format**  
**title**  
*little quote thingy*

* features

**mini-ships**  
*a starship in a regular ship*

* as the statement above implies, mini-ships have the engine of a (base, no hyperdrive) starship, but modified to be in a transport ship, thus losing some things but is quite unnoticeable  
* only found in the fighter fleet subdivision (among the zenith)  
* all ships included in the zenith are these ships  
* supports type 2 ais, but their automation is limited compared to being in an actual starship, especially if they aren’t in one that they’re included in  
* depends on whether it falls under the umbrella of the GITD or the GISD  
* more to come

**transport ships**  
*it’s a ship that transports*

* ships easily mass-producable to help get people around space or around a planet easily  
* most are autopilot only, but do have an emergency control override in the event that is needed (uses holograms for control)  
* has the gold incorporated logo on the side, and the transit division logo on the nacelles  
* no engine room, very small engines

**regular ships**  
*ships*

- regular ships  
- these ships usually are inside of the hangar of consumer-tier ships  
- made for solar system only exploration, does not get around very fast  
- not much else

**starships**  
*they are ships that zoom across stars*

	

# 🤖 ai systems

# **ai systems for gold inc. ships and affiliates (if they decide to)**

**personality**

- depending on what ai you have, the personality varies  
- for example, something like a \-Sphere ship (to come) would be robotic (in personality, but isn’t really that robotic, still has a ‘soft-spot’ sometimes), but still have some personality just not much, while the zenith’s ai acts like a crew member kind of, like they’re actually there, like this  
  - sphere: “Gold Incorporated ship wreckage detected. Perform evasive maneuvers to prevent a fatal impact.”  
  - zenith: “We’ve got some junk up ahead from a Gold Inc. ship, should probably move or we’ll end up like they did\!”  
- maybe some configuration slider for less personality, but the personality will still follow that general thing for each ship  
- when in security protocol and you are not identified (the protocol is usually turned on when you’re away from the ship) the ai will always act serious, even if the ai is usually quite outgoing

**ai cores**

- **type 3 (consumer-type)**  
  - a little skinnier and a little taller than a soda can; easily fits in your hand  
  - has screen near the right of it with the voice modulator of the ai that it is in, the ai in it can be rewritten  
  - unlike type 2, the ai cannot talk through it and cannot see through it, but there is a screen on the capsule itself so you can preview the voice modulator (helps identify)  
  - goes into \-sphere gold inc. ships by a little cylindrical chamber you put it in vertically, which it kinda just floats in there  
  - the core itself cannot do anything, it’s only when its plugged into the ship it has capabilities  
  - eligible for custom upgrades (ex. security upgrade, these are made for ships made for just living in, not really much battle)  
  - cannot be forcefully taken out because the chamber closes when you put the ai core in  
  - does not have a built in security system unlike type 2, but will notify of an intruder but cannot do anything to stop it if they are already inside the ship  
  - has control over the ship (doors, turning devices on, locating devices, etc) but does not have the fine-tuned control type 2 cores get  
  - more to come maybe  
- **type 2 (limited to gold inc. battleships like the zenith and blaze empire starships and up)**  
  - quite big, kinda like a rectangular prism but with tiny connectors on each side which connect to a type 2 connector  
  - ai can talk through the capsule and ‘see’ through it  
  - can be forcefully taken out  
  - has more control over the ship since it is directly connected  
  - examples of control  
    - can pilot with much more control, such as adjusting trim (simulated) and adjusting engine power  
    - with the type 2 ai, while you are piloting it assists you with big maneuvers like doing a slingshot maneuver around a planet or doing a sharp turn by adjusting multiple aspects as needed  
      - while autopilot is on, it also does this  
      - you can disable this feature if you do not want it, but it helps with big maneuvers  
    - it can also disable safety limits & other things on the fly while you or the ai (via a mini ‘engine’ of sorts, not the ai itself) is piloting, which it would warn you of this  
      - computer: We’ve got a lot of obstacles ahead\! Going to disable safety protocols, buckle up\!  
      - other computers (like BlueBlaze Empire starships, no ai backend unless enabled): Confirmed obstacles detected. Disabling safety protocols to evade. Please ensure that you have followed protocol for the safety of your crew.  
    - while they are off, the ship is more prone to breaking and crashing (along with hitting overheat and thermal runaway WAY faster), but the computer will assist if you are manually piloting and you, or the ai, chooses to turn them off in dire situations with these dangers  
      - when the computer is off, it is extremely advised to not disable the safety limits, even for the most experienced of pilots  
      - these safety limits also include flight assist, which is on by default (not to be confused with the computer’s flight assist)  
        - if you were to turn on flight assist, you wouldn’t be able to do anything besides pilot the ship, and even if you were absolutely locked in it would still be extremely hard to pilot  
- **more types to come, like for gold inc. affiliates**

**backend**  
	**stt**

* after the wake word “computer” via some other thing, an mp3 recorder will record all voices in the same room separately, then after about a 1 second pause, stop the recording and send them all to whisper (one at a time but so fast it feels all at once)

	**stt transcript to gpt-oss**

* adds relevant ship info and more, then puts all the users (either username or display name, display name when possible) into separate messages, like “gold:” and “blueblaze12:”, then sends it to gpt-oss

	**gpt-oss output to tts**

* gets the ai response and checks for any bracketed commands, then keeps it in memory for a short amount of time  
* puts the rest of the ai response excluding bracketed commands into orpheus-v1, then plays the mp3 and runs the commands after the mp3 is done playing unless it is necessary it is instant

**Core Logic: Autonomous Physics vs. AI Brain**

* **The "Mini-Engine":** A standalone physics script manages real-time flight assist, slingshot maneuvers, and nacelle power distribution. It is **deterministic** and abides by game physics independently of the LLM.  
* **The AI's Role (Narrative Overlay):** The AI (GPT-OSS) does not "calculate" the physics. It receives state flags from the mini-engine and "narrates" the actions to the player based on its core type.  
* **Type 2 (Zenith/Battleship):** High-level integration. During maneuvers, it can autonomously force higher power to a specific nacelle and temporarily disable steering assist to give the pilot tighter, raw control (though the transition is designed to be nearly seamless/unnoticeable to the player).  
* **Type 3 (Sphere/Consumer):** Low-level integration. It can perform basic piloting and standard evasive maneuvers but lacks the "fine-control" to micro-manage specific nacelles or overclock the hardware.  
* **Vision/Voice Bridge:** Type 2 can "see" and "talk" through its own modulator/core hardware; Type 3 is a "blind" processing unit that relies on the ship's external sensors and speakers.  
* **Security Protocol:** A hard-coded personality shift. Regardless of the AI's "base" personality (Zenith vs. Sphere), it becomes strictly serious and tactical if an unidentified user is detected or the ship is in "Away Mode."

# 📱 sfcd

# **sfcd (super futuristic communication device)**

note: these tools (and some functions) are only given to gold inc. and affiliates (others get an fcd \[manufactured by space inc. back in the day but was open-sourced after the sfcd came out\], which does not have these tools but is still able to communicate and has other functions in the same way)  
affiliates include (as of now): the blueblaze empire (they get sfcds, cool)

# **functions**

* goes onto your non-dominant arm (for most people it’s left), you select this in settings  
* lets you communicate with crew or people outside of it (other company members for example) from short distances (if they’re on another planet over 50 light years away you cannot communicate with them), transfers voice and camera (you can turn off camera) via the hologram you are using to communicate with  
* you can communicate with people who are not in your crew or company, but it uses fcd signals rather than sfcd signals (imessage and android type situation)  
* if you are a crew member of a ship that has a computer, you are able to communicate with it to pick you up for example  
* compatible with multiple different tools (below)

# **tools**

*applied remotely from gold inc or blueblaze empire higher up or inserting an sd card looking thing into your sfcd*

* Remote Scan (allows you to scan certain things to show what they are) \[v1.2\]  
* Repair Tools (contains multiple tools) \[v1.5\]  
  * Weld (welds things together by a version of soldering)  
  * Repair (repairs certain computer parts, so long as you keep your arm remotely in the same position)  
  * Laser (melts stuff and allows you to fry stuff) (galactech boostable)  
  * Unweld (heats up soldered materials within about a second in order to detach things, very concentrated version of the laser)  
  * ToolBox (category)  
    * screwdriver (lets you unscrew stuff)  
    * analyzer (analyzes primarily tech)  
    * solder (lets you solder wires)  
    * more to come  
  * Configure (pointing it at some sort of tech lets you configure it, exclusively an exposed motherboard)  
  * more to come (tbd)  
* more to come

**tools (BlueBlaze empire exclusive)**  
*tools cools, applied kind of the same way*

* Direct Communication (lets you communicate with BlueBlaze himself, while he’s offline this will notify him, you can only do this every 30 minutes)  
  * with DC, he can disable the notify button when he is unavailable (will be grayed out)  
* more as needed  
  * also more as needed (but sub)  
    * also more as needed (but sub of the sub)

# **identification**

* the sfcd allows you to be identified on different ships, if you do not have it on you are considered a general intruder (unless you are part of the crew or the captain of said ship), if you are on a ship that is not of your company or an affiliate of said company you are still considered an intruder  
* exceptions can be made; you can tell the computer that they are an intruder  
* putting it up to keycard scanners will open the door if you do not have a physical id on you, for example you left it on your ship (although you should always have it on you since it’s in your inventory \[tbd\] or something)

# 🌌 The Zenith

# **The *Zenith***

# **(v1 (release))**

features of the *Zenith* in this game

**Throttle (speed thing)**

- pushing it will accelerate the ship; more aggressiveness when you push the throttle forward means more force from the engine at once (ex. if you push it gradually it’ll start moving gradually, but if you throw it to full really fast you’ll go ‘brrrt pow’, with brrt being the engines powering up, which makes the ship shake because how fast they’re powering up, then the pow is the energy being sent to the nacelles). For example, gradually makes it go slower but more ‘accurately’, while going full aggressive makes it go way faster but less accurate in terms of speed  
- as you push it in ‘drive’ the color below changes in a gradient-way depending on the force of the engines (red is the most, green is the least, blue means hyperdrive is enabled and you can’t change the throttle during this)  
- pulling it all the way back will make the ship come to a complete stop either gradually or forcefully depending on the aggressiveness value  
- Pressing B on your controller while gripping the throttle activates the E-Brake, but it consumes a lot of fuel and can damage the engine over time. TBD how that will work, holding it means it will keep the brake on, meaning you can come to a complete stop with this, but you should probably pull the throttle to a full halt before you do that, brakes faster than just pulling the throttle all the way back  
- Rotating the throttle a full 180 degrees backwards in any direction will make the back of the engine's face where the bridge is facing (the front), meaning that it’s a full reverse; speed is not limited this way, still TBD for normal reverse. The bottom (where the throttle’s stick that keeps it inside of its little box thing) will become yellow to show that it is in reverse  
- Rotating the throttle only 90 degrees doesn’t do anything, but it’s weird to see on the throttle  
- Pushing down on the throttle will make the ship in PARK, meaning you can not move the throttle forward or backward, you can only pull it back up, the screen below, similar to full reverse, turns red during this  
- On the contrary, pulling up on the throttle while in drive puts the ship in neutral, which means it won’t help you stay straight (you will start kinda spinning) with the stability boosters, and you can see the planets outside tilting. Putting it back into drive will fix this, where the throttle does not matter in this gear, and it also does what you think it would do; make the engines stop outputting force so you’re just gliding  
- Running out of power in the *Zenith* also makes the engines and stability boosters automatically disable; so you can’t move and you’re in a form of neutral, the throttle will be stuck in this scenario  
- Being in neutral makes the throttle have no resistance. The aggressiveness value is always 0 when you move the throttle and put it back into drive, so it will flow normally

**The Steering Yoke (steering thing)**

- fair warning: this is subject to change as the steering yoke originally was the throttle too, so TBD  
- steering functions  
  - rotating it left and right like in a car makes it roll in that direction, more rotation means faster roll  
  - pulling back makes the ship pull up, pushing forward makes the ship go down  
  - while relatively still (prob around \<50 knots), you can use the right joystick to yaw left and right (no throttle, ship goes too fast to yaw while going)  
  - while not moving fast (around \<100 knots) you can use the left joystick to ascend and descend, usually from a hangar (up is up, down is down, adaptive based on how far the joystick is, more joystick means faster less joystick means slower)  
  - pressing left trigger while both hands are on the steering yolk will toggle using the left joystick to navigate the steering yolk screen ui and right trigger to select stuff, rather than touchscreen so you can do it while piloting  
  - when your stuff is armed, you use the Y or B button to switch between weapons  
- Has a screen in the middle of it which controls engine functions, invisitech, armed weapons, etc.

**InvisiTech**

- When activated, it hides the exterior of whatever it’s attached to (The *Zenith* includes it as a function, no need to attach it)  
- Only the exterior including windows are hidden; if a door on the outside happens to open while InvisiTech is on, the doorway will not be cloaked  
- Either a weldable item or part of the ship (this is separate from the *Zenith*)

**Battle Artillery**

- When active, the *Zenith* has two laser turret things that pop out from the sides, which can shoot down ships and such (very lethal).  
- The shutdown beam can shut down anything that is considered to be electric until the engine/electric component is fixed. The main use for this is the engines of a ship; the toppat orbital station applies because the shutdown beam can shut down the central core if exposed.  
- The trap beam can grab anything and everything. Depending on the setting you choose for how much power you wish to put in it, it can lift more or less weight. More importantly, more power equals faster power drainage  
- The *Zenith,* among other ships which will come soon, is shutdown resistant, meaning if the engines aren’t shut down fatally (A shutdown *bomb* will NOT shut down the engines, but will either make your life worse or shut it down temporarily) you’re still kinda able to move except not as well  
- To clarify, shutdown bombs are little objects you can throw at mini-ships (not a main spaceship) to completely fry their engines until it can get repaired; if you do it for a big ship maneuverability is the only thing that is hurt  
- More will come soon, as the *Zenith* is the equivalent of an absolute tank, but in spaceship form.

**Emergency Separation**

- When you activate this, the engines and the main part of the ship will split, which means both can be driven freely.  
- The bigger part of the ship without the engines can only maneuver to an extent; it is not enough to escape gravity or trap beams.  
- Your maximum capacity on power is lower due to you only having about ¾ of the ship, but you will use less energy.

**Collision Course Detection**

- Like other ships (and the [Toppat Orbital Station]()), the *Zenith* can detect a collision ahead of time  
- If something (like a ship or asteroid) is on a collision course with the ship and is big enough to cause damage, the computer is notified via the ai bridge and on the right hologram of the captain’s station you can see the path it is going to take, accounting for the object’s predicted velocity and the ship’s current velocity  
- This can only detect to an extent; you can’t detect something hundreds of thousands of light years away for example.

**Gravity Generator**

- The *Zenith*, similar to other ships (and the [Toppat Orbital Station]()), can create artificial gravity  
- The gravity generator can’t be found anywhere in the ship, but it is there  
- If the ship happens to be damaged (like crashing it) fatally, the gravity generator is likely to malfunction, where as an example, you would be plowed against the wall with insane force (resulting in HP loss)  
- The gravity generator can be turned on and off freely via either computer, other stations, or the captain’s steering yoke  
- This DOES use a good amount of power, but it is vital to being able to survive so it’s basically low power mode if you have gravity generator off (because it’ll just make the power go down slower), but the Zenith and other ships hold a lot of power already so it barely makes a difference

**Oxygen Generator (aka Life Support)**

- Because nobody wants to die while on the ship, there’s an oxygen generator/life support, which keeps the ship feeling like you’re still on Earth  
- If the ship has holes in it or is damaged severely in some way, the Life Support integrity will start going down, meaning you will only have a few minutes of oxygen left  
- Galactech can repair any holes in the ship and ultimately bring back life support  
- Life Support CANNOT be turned off automatically; you will have to go to the oxygen generator and manually shut off the power; it does not connect to any buttons in the ship other than the one on the oxygen generator

**Galactech Globe/Gold Globe**

- The Galactech Globe can be found somewhere (TBD), but not active  
- You have to go to another multiverse if you want to activate it, precisely the (TBD) planet, then you put it in the tallest tower, and it starts glowing  
- It can only be upgraded to the Gold Globe if you get me or someone whom I give privileges to  
- To take advantage of the features, you have to put the globe into a ship’s fuel cell and activate Galactech mode  
- Putting it in automatically activates Galactech mode

**Computer**

- Using whisper, gpt-oss:20b, and orpheus-v1 (in that order), the *Zenith* and other ships will have a computer that can automate functions like InvisiTech and such  
- Will automate via text commands catched by some sort of handler in the AI’s core object (in brackets, with an action that it is given in the first message of the chat which is automatically sent with personality, things about the ship, and the commands it can use, for example \[music/play: music1.mp3\] and \[controls/autopilot: earth/3LY\], autopilot commands have arguments in slashes, where the first is location and the next is speed in light years per hour (depending on what you have, the maximum can be changed, by default it’ll go at a pretty cool speed of like 50 per hour, in game hours)  
- Talks through voice modulators around the ship, little screens where it sees and talks out of, and more importantly is able to do many things through, voice modulator features will be in the next paragraph  
- See [ai syste]()ms for technical detail on how the other ai systems work

**Voice Modulators**

- The screens embedded either on the top of doorways or on walls near the roof, mainly can be anywhere, where the AI sees and talks out of  
- When you want the AI to analyze something (late in development), you can put what you want analyzed near the voice modulator, and this blue little triangular flat plane “cone” thing will appear, it’s a hologram that the AI uses to analyze things, or something else (which will be described in detail in the next feature)  
- When the ship’s security is on, the same cone-like hologram will be red instead of blue, and will always be active, rotating at its origin (the voice modulator) to look around all rooms, it cannot go through objects, but can see through glass (the shape itself won’t go through glass that would be too complicated, the code will just know that you’re there rather than think you’re behind an object)  
- If you’re caught and not a member of the crew or don’t have an SFCD with identification that you are allowed onto the ship, the scanner will lock onto you (if you move it’ll follow you exactly, locks on either your chest or your head), and one of the variations of the audio that’s not the computer’s personality (outgoing, pretty enthusiastic), and instead just cold and directive like “Stop moving and raise your hands above your head,” and if you don’t and keep attempting to run away while it has a lock, then you will be trap beamed the next time you’re visible, which will not let you move, or if you decide to actually be good and surrender, instead you will have to stay there, but if you move even an inch away you will get beamed down

**Hyperdrive**

- The hyperdrive is something that goes into certain types of ships (others maybe with an adapter, TBD)  
- Makes the ship go WAYYYYYY faster, the item itself is modular, the *Zenith* is unique because it includes this unlike other ships  
- Can be stolen out of the main engine core, makes the ship way slower if it doesn’t have a Galactech or Landent Globe (it still hurts speed regardless)  
- Putting it in a ship with too weak of an engine core WILL light it up like it’s dynamite  
- You can either build one yourself (way later), steal or borrow one, and maybe more methods (TBD)  
- See the [hyperdrive]() tab for more details

**Communications**

- the zenith has a long-range communications array used for communicating with video and audio, as all ships+ do (transport ships have a med-range array)  
- this is not visible on the outside of the ship as it is internal only, but it is also very fragile so if you were to crash you would only rely on short-range communication (which is only planet-wide), or launch an sos signal to gold inc, which travels at 1 light year/5 minutes (a long time)  
- the zenith has a unique holographic transmission thing, for uh idk  
- i dont think there’s more to come but there’s always a way (the more you know)

**the crew**

- the zenith can have five people as part of the crew, with people listed below  
- captain  
  - the captain is usually the pilot, but it can be piloted by other crew members when needed  
- operations specialist  
  - handles ship operations, like extending boarding tunnel and more when the ship is fully crewed (usually it’s solo or with 2 people)  
- weapons specialist  
  - handles weapons like shutdown beam, flash beam, shutdown bombs, etc  
- communications specialist  
  - handles communications, not much else to say  
  - also handles other stuff, as all stations have the same controls  
- analyzation specialist  
  - i dont even know anymore  
  - analyzes stuff with the zenith’s tools?  
  - mostly just fills in for other places when needed

# ⏩ hyperdrive

# **the hyperdrive**

*idk how it works but it vroom vroom*

**features**

* when in a compatible ship, the hyperdrive supercharges its engines  
  * it does not check if the engine is powerful enough, if it isn’t it will make your ship fireworks if you activate it and go full throttle by making the engine go into thermal runaway faster than you can say cheese, and making the engine go boom faster than you can say boom  
  * all starships in the gold inc and blueblaze empire lineup are already compatible as the computer can adjust the power of it if needed, i cannot say the same about transport ships or regular ships (their engines are tiny, inside of the nacelles themselves because it’s only made to get around)  
  * can make a trip from another end of a galaxy to the other go from about 10 minutes to just about 1 minute  
* to activate it, you can either use the computer or activate it via whatever your pilot quarters are (for the zenith it is the steering yoke, other ships tbd)  
  * for context on the zenith, the captain is usually the pilot since it is meant to be a 5 person manned ship (  
  * for GITD ships, it can be either captain or a separate pilot  
* can be stolen to put into other ships, this makes the ship basically uncatchable  
* you CANNOT print a hyperdrive with the fabricator (consumer-tier) unless you have an admin can, which you cannot get normally (only given to ETPs)  
* if you get the materials, you can condense it into the material can you need, this is how you would craft things  
* the zenith includes this natively, one of the only ones  
* also craftable, but later update  
* galactech compatible, the hyperdrive gets COLOSSALLY megacharged when galactech is active  
  * makes getting across galaxies super easy; you can reach other galaxies in a matter of seconds  
* more to come probably, like a supercharged hyperdrive or a galactech hyperdrive

**versions**

- generic  
  - generic hyperdrive, internals owned by gold inc. but isn’t really made by them  
  - crafting this will give you this version, just looks like a boring capsule, can’t really skin it  
  - supercharge value is just 2x here  
- zenith  
  - the zenith hyperdrive, you cannot craft or fabricate this (without an admin hyperdrive)  
  - supercharges the engines by 5x  
  - unfortunately does not allow for OmegaSpace  
  - custom-made  
- intelligence agency (ETP but lore-related)  
  - the GIA hyperdrive lets you enter OmegaSpace, and stay in there unlike other ships for as long as needed, no overheating  
  - travels through it so fast you literally just teleport (gold globe doesnt do this)  
  - unlike others, does not supercharge the engines by much as engines on Intelligence Agency ships are already powerful enough, and are unstable prototypes  
- galactech-charged  
  - this goes with any hyperdrive, while galactech is on  
  - can enter OmegaSpace for about 30 seconds safely, after that you will start flickering in and out of it until it becomes extremely unstable and if you do not exit your ship will lose all integrity at inhumane speeds  
  - multiplies the engine’s power by 10x \+ the original multiplier  
- gold-charged  
  - again applies to any hyperdrive  
  - can enter OmegaSpace at unfathomable speeds, stability will not be lost at all during this  
  - supercharges the engines by an unfathomable amount, the full power of the globe isn’t even used because the engines aren’t powerful enough to  
- more to come (if you want to you can add some more)

# 🖨️ fabricator

**the fabricator**  
*not the one from subnautica*

**about**

- the fabricator is a highly complex tool to autonomous robot (see tiers for more info)  
- using material cans with contained, compressed void energy (engineered by the Gold Inc. Void Department, very safe even if it leaks), it is able to print many things in a matter of seconds (depending on which material can preset you’re using and your blueprint)  
- gold inc. ships feature a state of the art, low energy usage material can generator on board (more on that later), so you don’t have to worry about finding the correct can types, as you can make your own or make a preset one

**tiers**

- the tiers of the fabricator make a huge difference  
- tier I  
  - tier I is the starting tier, and the cheapest tier; it is a handheld tool which takes only one can, you just put it in the open position and slap it on the top of the tool  
  - in order to print something, you use the holographic display on the thing or send it via anything compatible (like the sfcd or just a computer), then just hold down the trigger and it will print  
  - this one also prints the fastest, but can’t print many big things, it is made for printing small objects/tools or replacement parts  
- tier II  
  - tier II is autonomous and hovers, it is also easily portable so you can take it anywhere (not fit-in-your-pocket portable, however)  
  - can print transport ships and mini vehicles, either by the heavy duty preset or by a custom can  
  - can print anything you would want  
  - can cook things for some reason  
- tier III  
  - bigger than tier II, still autonomous but isn’t portable anymore, very fast and can print all consumer materials  
  - these are included in gold inc. starship hangars, and stay hidden in the roof until it needs to come back out to print something  
  - you input material cans via the hangar controls, there will be a slot that opens up in the wall where you can put a material can  
  - you have at least two of these, depending on the type of starship  
  - still can cook things for some reason  
  - more to come maybe  
- tier IV  
  - much bigger than tier III, the tier right below the GIMS  
  - this goes in stormships for mass manufacturing of small-medium ships  
  - multiple work together  
  - cannot cook things :(  
  - more to come i think  
- the gold inc. manufacturing system (highest tier)  
  - cannot be put in ship due to size, one is multiple of these  
  - cannot be bought normally, these are only in manufacturing facilities, you would need to contact gold inc. themselves (lore, not in game)  
  - builds multiple parts of the ships, each fabricator does their own job  
  - then, it welds the ship things together (engines to hull, etc), after individual parts are tested  
  - the ship is then autopiloted to Gold Incorporated for further testing and to be kept until needed  
  - they can also manufacture a lot of other stuff, from simple suits to megaships  
  - if you were to buy these as a corporate entity gold inc will only ship you what you need and get it configured to match your production  
  - still cannot cook things \=(  
  - maybe more to come idk

**material cans**

- material cans let fabricators print different things, depending on the can  
- generally, each preset can lasts about 5 prints unless stated otherwise  
- you can drink it but then you’ll ragdoll and will be passed out until somebody with a defibrillator revives you  
- you will not be able to move at all during this; all controller actions will be ignored (you’re paralyzed), you can still move around your head  
- made with condensed void energy, kinda fragile but not really (dropping it won’t break it)  
- if the void energy leaks, you are to clean it up using an sfcd IMMEDIATELY and dispose of the can using an incinerator (gold inc ships have a version of these where it converts it into power for engines)  
- more to come maybe but im not sure

# ⏭️ omegaspace

**omegaspace**  
*almost alien tech by gold inc.*

**overview**  
*its a view thats over*

- omegaspace is something discovered and innovated on by gold inc quite recently, used exclusively by their ships (with the exception of the BlueBlaze Empire, but a modified version of it), to travel across even multiverses at speeds well over the speed of light  
- entering it requires a hyperdrive or an engine powerful enough to handle it  
- if you have the hyperdrive you don’t have to worry so much about the engine, so long as it’s a starship  
- in lore, when turned on, you go through a wormhole-like part of space which allows you to travel through it at very high speeds  
- quite unstable as this is new tech; but will one day be put in transport ships (lore)

**functions**  
*you can’t spell functions without fun*

- when a compatible hyperdrive is put in, you activate omegaspace by pressing a button in your pilot’s quarters to prime it, then after a few seconds when it’s primed you press engage while you’re at full throttle (will not work otherwise)  
- when you engage omegaspace, your ship will have a neon white outline, and start vibrating as if it’s flipping in and out of reality very fast (looks kind of unstable), then after a little bit there will be a white path generated from the engines to where you’re going to go, and the same neon white color will be there but in the silhouette of your ship, which fades away after about 1 or 2 seconds  
- when you are in omegaspace, it looks like you’re traveling through a wormhole really fast from inside of the ship, you are not seen in space and are very unstable to track (also unstable to communicate with, with the exception of the BlueBlaze Empire)  
- when you are in it for too long (usually about 30 seconds), you will start getting alerts saying that the engines and the hyperdrive are becoming unstable  
- after about another 15 seconds in omegaspace, you will get an alert on your ship’s HUD (which is usually the windshield) that it is becoming unstable  
- in this unstable state, you will start flickering in and out of it (you will be flashing in and out of space during this), and you will be getting multiple alerts  
- when you deactivate it, you will reappear in space, but with the neon white silhouette covering your ship for about a second, before it quickly fades away  
- there is a cooldown of about 10 minutes using this (max), depending on how long you stay in it for  
- more to come maybe

**states**   
*not american ones*

- offline (just offline, not primed)  
- priming (when the omegaspace drive is priming, the engines will start powering up with a similar sound to firing them up normally, but it sounds more powerful)  
- primed (when omegaspace is primed; can only be primed for about 1 minute before it shuts off)  
- engaging (when your ship is about to go vroom, the engines will power up even more during this)  
- engaged (when you are in omegaspace, traveling at incomprehensible speeds to people outside)  
- unstable (when you have been in it for too long you will be in this state, after about a minute, the wormhole-like part will keep flickering on and off like you’re coming back in and out of reality)  
- dangerously unstable (when you’ve been in it for about 1 minute and 30 seconds, it will be flashing in and out, but you will mostly be out of it, you will be getting a million ship alerts like hull, engine, unknown, etc)  
- fatal (when you’ve been in it for 2 minutes, you will start hearing weird sounds coming from the hull and the engines, disengage IMMEDIATELY)  
- damaged (you will be warned if you try to prime it in this state, it is very unpredictable if you activate it)  
- destroyed (you cannot activate this without disabling safety precautions on your ship \[safety precautions keep your ship safe, if you disable them these failsafes and more will no longer be enabled and you are at more risk of destroying your ship\], if you activate it it is colossally unpredictable)

**more to come maybe idk**

# ⚠️ overrider

**the overrider**  
*very dangerous prototype*

**overview**  
*should i really say it again?*

- a prototype ship weapon by gold inc, still being worked on (lore)  
- when used, it supercharges a ship’s engines, then locks all controls and makes it go full speed across the universe  
- gives control to the user of the overrider for a short amount of time, until you are locked out for good  
- if your ship gets hijacked with this, your only choice is to abandon ship, nothing, not even taking apart the engines bit-by-bit will save you  
- can be fabricated, custom can only  
- blueprint only given by admins up until v1.5 or smth

**features**  
*features*

- you can store these in a ship’s hangar, or by just holding it  
- when handheld, it can be used to crack doors forcefully by using a tiny fraction of its payload  
- when in handheld mode, it will not be autonomous and you have to get it near the engines, whether that be from rolling it or just keeping it near it  
- when it gets near the engines in handheld mode, it will automatically float and launch its entire payload into the engines, leaving the ship cooked and the overrider a spherical brick  
- when in autonomous mode, you launch it from some sort of ship, whether that be a transport ship or a starship  
- it will find its way to the engine room itself, and is quite stealthy while doing it  
- the computer can detect it but cannot stop it remotely, you would have to trap beam it then open up a vulnerability in it (a hole in the shell), then shutdown beam it  
- when it reaches the engine room, it will launch its entire payload while floating, then fall down to the floor, rendering it as a spherical brick (again)

# ☀️ Project Supernova

**Project Supernova**  
*the most powerful ship in the lineup*

**overview**  
*i am not saying it again*

- Project Supernova is a starship part of The Destroyer fleet, and one of the first ever created  
- only one exists in the entire fleet, as for almost every Destroyer-class ship  
- can go from one side of the universe to the other within seconds when omegaspace is active  
- only ETPs i allow are allowed to pilot this, even then i have to initiate a protocol for it to even be able to go in and liftoff  
- if you were to put it up to the most elite fleet of 300 ships, it would come out with not even a scratch, so long as the crew isn’t horrible  
- the ship follows a black color scheme with white accents, and maybe some gray  
- crewed by 4 etps, however can be crewed by 1 with computer assist  
- also very resistant to overriders, and can detect them when they do as much as to crack open a door (which would be stealthy on any other ship)  
  - when an overrider is detected, the computer boots up security around the ship with the cone-like things, and when the overrider is inside one of those cones, even if it’s behind something, the computer will launch a shutdown beam from a voice modulator, shutting down the overrider

**activation**  
*activat*

- in order for this ship to be activated, it has to follow this order  
  - gold (me\!\!\!) would have to initiate codename delta from his office, whether that be in his ship (zenith), gold inc headquarters, etc  
  - i can choose who would be part of the crew, so long as they are online  
  - when codename delta is initiated, all ETPs get a notification on their sfcd and their computer is notified  
    - the notification would say “CODENAME DELTA INITIATED; STANDBY FOR FURTHER INSTRUCTION” on their sfcd  
    - when inside their ship alone (or with fellow etps), the computer would say   
      - sphere-type/empire: “Codename Delta initiated. Land at the nearest home base and await further instructions.”  
      - zenith-type: “Just got news from Gold that we’ve gotta bring out the big guns, get to the nearest base or just get to the HQ, we’re on the clock here\!”  
    - if they have other people in the ship (non-etps)  
      - sphere-type/empire: “Encoded message for “usernames”: Delta, immediate, await.  
      - zenith: “Just got something, sorry crew but you’re gonna have to go for this one\! Just printed some fresh transport ships in the hangar, got some classified info for the others. If you don’t want to just get thrown out with force, I suggest you start getting down there\!“  
    - after they land at gold inc. hq, they would find the secret door to go down to the emergency hangar (with only Destroyer ships and maybe some secret prototypes)  
    - after they go down the elevator, they would scan their ID (not their SFCD) to go into the hangar, then they would go into the ship via the boarding tunnel  
    - when they pass through the air trapper (which is also a security system) their SFCD will change from green to solid black, along with getting a suit which is entirely black along with the Project Supernova logo as the ‘patch’ of the suit  
      - when the suit is on, they are completely anonymous, they cannot broadcast their voice, the computer serves as the communication barrier  
      - in the leaderboard, they are shown as “supernova(number)” to others and are completely anonymous, but in the ship they can talk to each other and see their names, along with being able to see each other through their helmets  
      - their names on the leaderboard show “supernova(number) \[(actual username/display name)\]” to ETPs and me (not even head fighter pilots)  
    - after everyone is on board, the ship is started along with all systems being initiated, then the hq’s emergency hangar opens up, and Project Supernova launches at extremely ludicrous speed, going so fast it leaves behind a silhouette of the ship for a few seconds, and the clouds even deform around it because the air expands so rapidly

**artillery**  
*super powerful*

- the artillery of Project Supernova is super extensive  
- uses an upgraded version of the laser turrets that the zenith has, making them able to clip the steering boosters of a ship with just one shot, with the nacelles taking a bit more hits  
- has an upgraded shutdown beam that takes things out for good, if you just aim at a nacelle you can take out the engines in just that hit  
- the trap beam is also upgraded, allows you to carry 3 starships at once with no speed penalties  
  - if you were to carry more the penalty would be small as well, an average of \-0.5 kt removed per starship after the 3 (due to sheer engine power)  
- Project Supernova also has a passive frequency which disables any signals that don’t come from or are for Project Supernova, this can be toggled on and off  
- Project Supernova also has InvisiTech, which is to stay on at almost all times while in space until it starts the battle  
- 

# 🪦 scrapped

# **the things below this tab are scrapped (usually for now)**

# 📋 todo

# **todo list (in phases)**

# **phase 1 (v0.1, starting out) \- difficulty: easy**

- [x] ~~make player rig~~  
- [x] ~~add player rig to xr origin & add simple grip animations~~  
- [x] ~~find out how to use xr interactables~~  
- [x] ~~make models~~  
      - [x] ~~throttle~~  
      - [x] ~~steering yoke~~  
      - [x] ~~sfcd~~  
- [x] ~~yeeps break~~  
      

# **phase 2 (v0.3, the ship \[functionality\] update) \- difficulty: meh**

- [x] ~~make scripts via claude code~~  
      - [x] ~~throttleinteractable.cs~~  
      - [x] ~~steeringyoke.cs~~  
      - [x] ~~simplesfcd.cs (easily upgradable to be the actual sfcd.cs)~~  
      - [x] ~~[ship.cs](http://ship.cs) (simple)~~  
      - [x] ~~[engine.cs](http://engine.cs) (simple, literally only essential mechanics for physics)~~  
      - [x] ~~[nacelle.cs](http://nacelle.cs) (simple, but the most complete out of all)~~  
- [x] ~~connect to a simple test ship to conduct physics tests alone~~  
      - [x] ~~fix the harry potter bug~~  
      - [ ] fix handles not working  
- [ ] add photon fusion 2 (and actually get it working CORRECTLY)  
- [ ] give build to Bekham so he can test (after conducting thorough tests w/ multiplayer)  
- [ ] make any adjustments (if any are needed)  
- [ ] make models 2  
      - [ ] zenith model  
            - [ ] engine model  
            - [ ] exterior model  
            - [ ] interior model (only if invisible on only outside is impossible; otherwise merge)  
            - [ ] engine discharge lever model  
      - [ ] voice modulator model (zenith variant)  
      - [ ] ai capsule model (zenith variant)  
      - [ ] hyperdrive (tbd where it goes)  
- [ ] make scripts  
      - [ ] hyperdrive (simple)  
      - [ ] steering yoke controls (easily modular for upgrading & future features)  
      - [ ] tbd  
- [ ] yeeps break

# **phase 3 (v0.5, the \[simple\] ai update) \- difficulty: somewhat hard**

- [ ] make scripts (or merge if possible, while keeping things simple)  
      - [ ] gpt-oss to game bridge (likely the most complex)  
            - [ ] request handler  
            - [ ] response handler  
            - [ ] command parser (simple vers)  
            - [ ] tbd (add more as needed)  
      - [ ] mp3 recorder to whisper (ez)  
      - [ ] gpt response to orpheus-v1 (idk)  
      - [ ] orpheus-v1 output downloader (ez)  
      - [ ] ai voice handler (plays the orpheus-v1 output, ignores bracketed commands) (ez)  
      - [ ] voice modulator handler (idk-meh)  
- [ ] hook up to template voice modulator  
- [ ] make a part that has text on it, showing commands received from the ai  
- [ ] build and give to bekham to test the frick out of it  
- [ ] (opt) make adjustments  
- [ ] hook some functions up to actual ship functions  
- [ ] yeeps break 

# **phase 4 (v0.6, the zenith update, skippable): difficulty \- actual heck**

- [ ] add the models into Unity  
- [ ] add nacelle movement handler (when extended to the sides, that means the hyperdrive is enabled)  
- [ ] make scripts  
      - [ ] invisitech  
      - [ ] steering yoke screen ui  
            - [ ] hyperdrive activator  
            - [ ] shields activator  
            - [ ] omegaspace primer/engager  
            - [ ] tbd, get CC to make web prototype of how the yolk screen would look (exact) then let it on its way  
      - [ ] engine discharge lever  
      - [ ] multiple ‘hub’ scripts  
            - [ ] [ship.cs](http://ship.cs) (ONLY ONE PER SHIP)  
            - [ ] [engine.cs](http://engine.cs) (ONLY ONE PER SHIP)  
            - [ ] [nacelle.cs](http://nacelle.cs) (multiple on each ship, ex. zenith has two)  
            - [ ] more to come  
      - [ ] tbd on more  
- [ ] add scripts to parts  
- [ ] test stuff alone  
- [ ] give to bekham to test together (multiplayer required by this phase, full syncing without any inconsistencies)  
- [ ] make changes as needed  
- [ ] yeeps break

