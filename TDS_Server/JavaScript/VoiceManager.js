mp.events.add("VoiceToAdd_Client", (player, target) => {
    if (target) 
        player.enableVoiceTo(target);
    
})

mp.events.add("VoiceToRemove_Client", (player, target) => {
    if (target)
        player.disableVoiceTo(target);
})