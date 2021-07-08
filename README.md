# EmoteChat


**Demo Video**

[![Emote Chat](media/3dae21cdc03a875b06937f7529b4aedf.jpeg)](https://youtu.be/E2ZnmYjW9Pk)

**Author:** RatherGood1

**Version**: 0.3

**Updated:** 8 July 21

**Compatibility: (tested on) Suriyun** **MMORPG Kit Version 1.66d**

**Description:** Provides slash commands or hotkey activation to perform emote
animations.

**Other Dependencies:**

You need to provide your own animations. Demo uses simple kit animations.

**Core MMORPG Kit modifications:**

None. Requires edits to UIGamePlay components.

**Description:**

By slash command “/dance” or key press will send emote command to all players in
scene. Animation plays for sender. Messages are generated depending on the
listener.

**Instructions for use:**

1.  On your CanvasGameplay prefab replace the UIChat_Standalone with the
    UIChatMessageRGEmoteMod” prefab

    1.  The ew Prefab is provided In the
        Assets/RatherGood/MMOKit/EmoteChat/Prefabs folder

    2.  Hint: you can locate this on your GameInstance component in your init
        scene under “UI Scene Gameplay”

        ![](media/fcfee0744d595cd2c025e53dac7237b0.png)

2.  Recommended: Copy the prefab to another folder outside the kit before
    editing.

3.  The EmoteData database component is located on the UI and therefore will be
    shared with any players with the same UI prefab.

4.  Save the prefab.

5.  Edit the Demo animation data or create a new Emote Data Scriptable object
    for your Emote Animation Data:

6.  Right click in a folder and select: Create -\> RatherGoodGames -\> EmoteData
    and set up your animations actions as desired.

**EmoteData Fields:**

**slashCmdText**: This is the text the user types in the chat window to activate
the emote.

**KeyName**: Assign a key name to enable using this animation to be activated by
key press. The name must EXACTLY match the name used in your GameInstance
InputSettingsManager component in your Init scene. (See example below).

![Graphical user interface Description automatically
generated](media/b2c8fe66a89a3ad081df1f9ef1107205.png)

**ActionAnimations**: Insert the appropriate animations. (Not all animation
types work with ActionAnimations)

**(NEW) Multiple animations will play one after the other.**

**PlayClipAllLayers**: (Usually should be true) All layers will play animation
on the full body. If false will only play on upper body.

**AnimSpeedRate**: (0 will be ignored) Animation speed can be adjusted. 1(or 0)
is normal speed. 0.1 will be 1/10th speed.

**TriggerDurationRate**: N/A No effect for Emotes currently.

**DurationType**: ByClipLength will let animation play in full.

**(New feature) Extra Duration:** After animation plays this time will be added
before ending.

**AudioClips**: Will play audio at start of animation if included.

**EmoteMessageStringForMe**:

The sender of the emote will see this message. The typed message will not be
shown.

Ex: User “Player1” types “/playDead”

His chat reads: “[You are playing dead]”.

**EmoteMessageStringForOthers**:

From the above example other players will see: [Player1 is dead…or is he?]

**EXAMPLES:**

**/wave**

Simple example of a single short animation. CancelOnMovementState is not checked
so will always play in full even in player moves. However, another action (i.e.
attack, spell etc. will override this.)

![Graphical user interface, application Description automatically
generated](media/b065d356dcacb36e050e180700281705.png)

**/squat**

Single long animation about \~25 seconds. Can be canceled by moving or jumping.

![](media/e5fa2f68656968f7f760d110d1b11f3d.png)

**/playDead**

Single short animation. However “extraDuration is set to 1000 seconds”. So after
the death animation plays the player will remain in the playdead state until the
“ExtraDuration” runs out or they move or jump.

![](media/4cc4ff6627fbb12060924985431b65bb.png)

**/sit**

**NOTE: Animations shown here are not included as they are not part of the
standard kit.**

In this example, the sit animation plays and then transitions to a sitIdle
animation. The extra duration is set to 100 so sitIdle will loop for 100 seconds
or until movementState changes.

![](media/7ede00f06c8e644eb9899118f91d5ec0.png)

**Done.**

