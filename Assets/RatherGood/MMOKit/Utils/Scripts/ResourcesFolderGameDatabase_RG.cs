using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MultiplayerARPG
{
    /// <summary>
    /// This game database will load and setup game data from Resources folder
    /// </summary>
    [CreateAssetMenu(fileName = "Resources Folder Game Database_RG", menuName = "Create GameDatabase/Resources Folder Game Database_RG", order = -5998)]
    public class ResourcesFolderGameDatabase_RG : BaseGameDatabase
    {
        [Tooltip("Sub folder within Resources folder to load from.")]
        public string subFolder = "RGGResources";
        public override void LoadData(GameInstance gameInstance)
        {
            // Use Resources Load Async ?
            Attribute[] attributes = Resources.LoadAll<Attribute>(subFolder);
            BaseItem[] items = Resources.LoadAll<BaseItem>(subFolder);
            BaseSkill[] skills = Resources.LoadAll<BaseSkill>(subFolder);
            BaseNpcDialog[] npcDialogs = Resources.LoadAll<BaseNpcDialog>(subFolder);
            Quest[] quests = Resources.LoadAll<Quest>(subFolder);
            GuildSkill[] guildSkills = Resources.LoadAll<GuildSkill>(subFolder);
            PlayerCharacter[] playerCharacters = Resources.LoadAll<PlayerCharacter>(subFolder);
            MonsterCharacter[] monsterCharacters = Resources.LoadAll<MonsterCharacter>(subFolder);
            BaseMapInfo[] mapInfos = Resources.LoadAll<BaseMapInfo>(subFolder);
            Faction[] factions = Resources.LoadAll<Faction>(subFolder);
            BaseCharacterEntity[] characterEntities = Resources.LoadAll<BaseCharacterEntity>(subFolder);
            VehicleEntity[] vehicleEntities = Resources.LoadAll<VehicleEntity>(subFolder);

            GameInstance.AddAttributes(attributes);
            GameInstance.AddItems(items);
            GameInstance.AddSkills(skills);
            GameInstance.AddNpcDialogs(npcDialogs);
            GameInstance.AddQuests(quests);
            GameInstance.AddGuildSkills(guildSkills);
            GameInstance.AddCharacters(playerCharacters);
            GameInstance.AddCharacters(monsterCharacters);
            GameInstance.AddMapInfos(mapInfos);
            GameInstance.AddFactions(factions);
            GameInstance.AddCharacterEntities(characterEntities);
            GameInstance.AddVehicleEntities(vehicleEntities);
            // Tell game instance that data loaded
            gameInstance.LoadedGameData();
        }
    }
}
