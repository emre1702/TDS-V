# TDS-V - Team Deathmatch Server - community project 

Welcome to the team-deathmatch community project in GT-MP.  
This gamemode was created by me and opened for the community on 08.10.2017.  
Everyone can help make the gamemode better and be a part of something big.  
You can always play the gamemode on our GT-MP server:  
tds-v.com

You can always visit our discord-server:  
[discord.gg/ntVnGFt](discord.gg/ntVnGFt)

Please read the readme before trying to use the gamemode!


# Can I help?

Sure, I want the gamemode to become a big community-project where everyone can help.  
You all can create pull requests and help the gamemode to get bigger.  


# How can I use the gamemode?

## What do I need?

You need to use following files:
- TDS.dll 
  - You can find it in server -> DLL
  - Put it in [resourceName] -> server
- the maps
  - You can find them in server -> maps
  - Put the maps folder in [resourceName] -> server 
- meta.xml
  - Put it in [resourceName]
- script.js
  - You can find it in client
  - Put it in [resourceName] -> client 
- language, window, sounds and pic folder
  - You can find them in client
  - Put them in [resourceName] -> client
- MySql.Data.dll 
  - You can find it in server -> DLL
  - Put it where your GrandTheftMultiplayer.Server.exe, acl.xml etc. are
  
Also you need to change manager -> database -> DatabaseManager.cs.  
Here you can change the settings to connect to your database.  
  

## What is `script.js`? 

script.js contains all the clientside scripts (except language.js).  
To get it you have to use minify.js with NodeJS  


## What is minify.js and how do I use it?

minify.js is a JavaScript file using NodeJS and the packages `fm`, `babel-core` and `uglify-js`.  
It opens all clientside-script-files and put them together in one string, then transform it to ES2015, make it ugly and put the string in script.js.  
The idea for uglify came from Vio-V (Forces).  

### What are the packages for?

`fs` reads and writes the files.  
`babel-core` transforms the code in ES2015. That's important because `uglify-js` can't use ES6, so "let" etc. wouldn't work.  
`uglify-js` makes the code uglify, removes some unneccessary things etc. At the end the code becomes really hard to read.  

### How do I use minify.js?

First you need to install NodeJS:  
[https://nodejs.org/en/download/](https://nodejs.org/en/download/)  

Then you need to install the packages `fs`, `babel-core` and `uglify-js`.  
[https://docs.npmjs.com/getting-started/installing-npm-packages-locally](https://docs.npmjs.com/getting-started/installing-npm-packages-locally)

After that you can open the CMD prompt, cd to minify.js and use this command:  
`node minify.js` 
That will update your script.js which you can use for your server after that.  


# Changelogs:

### 03.10.2017:
- added cam-shake on bomb-explode  

### 01.10.2017:
- cursor-toggle on DEL now (instead of ALT)  
- flash blip on bomb-plant  

### 21.09.2017:
- fixed many things on lobby-leave and death    
- added cursor-info on welcome-message   
- fixed animation-cancel on bomb-plant/-defuse  
- fixed no bomb if no terrorist in countdown-start  
- fixed wrong map-infos  
- released bomb-mode  
**Thanks to Pluz. for testing with me**  

### 20.09.2017:
- added bomb-mode Beta  
- added killingspree (only HP and armor bonus right now)  
- better sync of opp-aiming  
- map-info now in middle of the screen  

You can find older changelogs in discord.




