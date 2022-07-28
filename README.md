# Isla

---
🤖 A bot for my Discord server

## 💾 Usage
⚠️ Isla is only intended to run inside of my Discord server.

1. Download the sample config file [here](https://github.com/niallVR/Isla/blob/main/Source/Isla/appsettings.json).
2. Pull down the Docker container [here](https://hub.docker.com/r/niallvr/isla`).
3. Fill out the config and mount it to `/opt/Run/appsettings.json` inside the container.

## ⏱️ Versioning
Isla follows the versioning format: `Major.Fix`.
- Major - Features were added/removed from the bot.
- Fix - A patch was made to the existing feature set.

## 🔧 Developing
The Isla solution is laid out in two projects.
- Isla - The main bot code
- Isla.Bootstrap - Code used to integrate Discord.Net into the hosting model.

Most of the time, code will be written in the `Isla` project.  
The bot is seperated into modules, of which there are two types:
- Client Module (Root Directory) - Code which helps power the bot
- Bot Module (Inside `/Modules`) - Bot functionality code (the "business logic").

For new features, create a folder and follow the predefined structure of files from other modules.

