# Isla

🤖 A bot for my Discord server

## 💾 Usage
⚠️ Isla is only intended to run inside of my Discord server.

1. Download the sample config file [here](https://github.com/niallVR/Isla/blob/main/Source/Isla/appsettings.yaml).
2. Pull down the Docker container [here](https://hub.docker.com/r/niallvr/isla`).
3. Fill out the config and mount it to `/opt/Run/appsettings.yaml` inside the container.  

If a config value is confusing, the config models have XML documentation!

## ⏱️ Versioning
Isla follows the versioning format: `Major.Fix`.
- Major - Features were added/removed from the bot.
- Fix - A patch was made to the existing feature set.

## 🔧 Developing
Firstly, thank you for wanting to help out!  

##### Layout
The Isla solution is split into the following projects:

| Project        | Purpose           |
|----------------|-------------------|
| Isla           | Application Code  |
| Isla.Bootstrap | .Net Hosting code |

Most of the time, code will be written inside of the main project.
Please look at some of the existing code to get an idea of the style, I'm a big fan of folders :)

##### Modules
Inside of the main project, code is split into "modules". 
A module is a overarching feature or functionality of the bot.

| Module        | Purpose                                                                       |
|---------------|-------------------------------------------------------------------------------|
| Activity      | Updates the bots "playing" activity periodically.                             |
| Notifications | Assigns/revokes "notification" roles when joining/leaving a configured event. |
| Roles         | Role selection buttons.                                                       |
