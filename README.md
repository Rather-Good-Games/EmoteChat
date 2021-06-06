# EmoteChat

**Author:** RLC

**Compatibility: (tested on) MMORPG Kit Version 1.65f**

**Description:** Provides slash commands or hotkey activation to perform emote
animations.

**Other Dependencies:**

You need to provide your own animations.

**Core MMORPG Kit modifications:**

Requires edits to UIChatHandler class.

**Instructions for use:**

1.  [Core Edit] In the “UIChatHandler” change:

    1.  From: public void SendChatMessage()

    2.  TO: public virtual void SendChatMessage()

2.  On your CanvaGameplay prefab

    1.  Hint: you can locate this on your GameInstance component in you init
        scene under “UI Scene Gameplay”

3.  Recommended: Copy the prefab to another folder outside the kit before
    editing.

    ![Graphical user interface, website Description automatically
    generated](media/40e9c517d77f5269fe94a481171d75a5.png)

4.  On the UIChat_Standalone component add the “UIChatHandler_RGEmote”

5.  Copy the component links form your current UIChatHandler to
    UIChatHandler_RGEmote and then delete the UIChatHandler.

![A screenshot of a computer Description automatically generated with medium
confidence](media/243ad7201167a09e5954c51df9ec84f5.png)

1.  Save the prefab.

2.  Create a new Emote Data Scriptable object for your Emote Animation Data:

3.  Right click in a folder and select: Create -\> RatherGoodGames -\> EmoteData
    and set up your animations actions as desired.

    1.  Alternatively you can copy the example provided and modify as needed.

4.  Add this to the EmoteData field of

5.  ![A screenshot of a computer Description automatically generated with medium
    confidence](media/321646e54302fb9eaaf961498e438c73.png)

    ![](media/9d82f6583bd7377d6343e04e31157bc9.png)

![](media/001153dd92eb5c0505d413de1a300129.png)

**Done.**

Hit play and press the button you selected to show the cursor.

[![](http://img.youtube.com/vi/Wbf0DS2OH38/0.jpg)](http://www.youtube.com/watch?v=Wbf0DS2OH38)
