using System;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using ServerSync;
using HarmonyLib;
using ItemManager;
using PieceManager;
using LocalizationManager;
using System.Text;

namespace OdinsKingdom
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    //[BepInIncompatibility("org.bepinex.plugins.valheim_plus")]

    public class OdinsKingdom : BaseUnityPlugin
    {
        private const string ModName = "OdinsKingdom";
        private const string ModVersion = "1.2.3";
        private const string ModGUID = "odinplus.plugins.odinskingdom";

        private static readonly ConfigSync configSync = new(ModName) { DisplayName = ModName, CurrentVersion = ModVersion, MinimumRequiredVersion = ModVersion };

        private static ConfigEntry<Toggle> serverConfigLocked = null!;


        private ConfigEntry<T> config<T>(string group, string name, T value, ConfigDescription description, bool synchronizedSetting = true)
        {
            ConfigEntry<T> configEntry = Config.Bind(group, name, value, description);

            SyncedConfigEntry<T> syncedConfigEntry = configSync.AddConfigEntry(configEntry);
            syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

            return configEntry;
        }


        private ConfigEntry<T> config<T>(string group, string name, T value, string description, bool synchronizedSetting = true) => config(group, name, value, new ConfigDescription(description), synchronizedSetting);

        private enum Toggle
        {
            On = 1,
            Off = 0
        }


        public void Awake()
        {
            Localizer.Load();



            Item GB_Parchment_Tool = new("gbcastles", "GB_Parchment_Tool");
            GB_Parchment_Tool.Crafting.Add(ItemManager.CraftingTable.Workbench, 1);
            GB_Parchment_Tool.RequiredItems.Add("FineWood", 10);
            GB_Parchment_Tool.CraftAmount = 1;
            ItemManager.PrefabManager.RegisterPrefab("gbcastles", "GB_Repair_Scroll");


            BuildPiece GB_Large_Gate = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Large_Gate");
            GB_Large_Gate.Tool.Add("GB_Parchment_Tool");
            GB_Large_Gate.RequiredItems.Add("Stone", 10, true);
            GB_Large_Gate.RequiredItems.Add("Iron", 1, true);
            GB_Large_Gate.RequiredItems.Add("IronNails", 10, true);
            GB_Large_Gate.RequiredItems.Add("Wood", 10, true);
            GB_Large_Gate.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Large_Gate.Category.Set("Stone Building");

            BuildPiece GB_Large_Gate_Bridge = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Large_Gate_Bridge");
            GB_Large_Gate_Bridge.Tool.Add("GB_Parchment_Tool");
            GB_Large_Gate_Bridge.RequiredItems.Add("Chain", 2, true);
            GB_Large_Gate_Bridge.RequiredItems.Add("Iron", 1, true);
            GB_Large_Gate_Bridge.RequiredItems.Add("Stone", 10, true);
            GB_Large_Gate_Bridge.RequiredItems.Add("Wood", 6, true);
            GB_Large_Gate_Bridge.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Large_Gate_Bridge.Category.Set("Stone Building");

            BuildPiece GB_Large_Portcullis = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Large_Portcullis");
            GB_Large_Portcullis.Tool.Add("GB_Parchment_Tool");
            GB_Large_Portcullis.RequiredItems.Add("Stone", 20, true);
            GB_Large_Portcullis.RequiredItems.Add("Iron", 5, true);
            GB_Large_Portcullis.RequiredItems.Add("Chain", 2, true);
            GB_Large_Portcullis.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Large_Portcullis.Category.Set("Stone Building");

            BuildPiece GB_StoneWindow = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_StoneWindow");
            GB_StoneWindow.Tool.Add("GB_Parchment_Tool");
            GB_StoneWindow.RequiredItems.Add("Stone", 5, true);
            GB_StoneWindow.RequiredItems.Add("Crystal", 5, true);
            GB_StoneWindow.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_StoneWindow.Category.Set("Stone Building");

            BuildPiece GB_StoneWindow_OdinPlus = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_StoneWindow_OdinPlus");
            GB_StoneWindow_OdinPlus.Tool.Add("GB_Parchment_Tool");
            GB_StoneWindow_OdinPlus.RequiredItems.Add("Stone", 5, true);
            GB_StoneWindow_OdinPlus.RequiredItems.Add("Crystal", 5, true);
            GB_StoneWindow_OdinPlus.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_StoneWindow_OdinPlus.Category.Set("Stone Building");

            BuildPiece GB_Large_Window_OdinPlus = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Large_Window_OdinPlus");
            GB_Large_Window_OdinPlus.Tool.Add("GB_Parchment_Tool");
            GB_Large_Window_OdinPlus.RequiredItems.Add("Stone", 10, true);
            GB_Large_Window_OdinPlus.RequiredItems.Add("Crystal", 10, true);
            GB_Large_Window_OdinPlus.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Large_Window_OdinPlus.Category.Set("Stone Building");

            BuildPiece GB_Large_Window = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Large_Window");
            GB_Large_Window.Tool.Add("GB_Parchment_Tool");
            GB_Large_Window.RequiredItems.Add("Stone", 10, true);
            GB_Large_Window.RequiredItems.Add("Crystal", 10, true);
            GB_Large_Window.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Large_Window.Category.Set("Stone Building");

            BuildPiece GB_Large_Tile_Floor = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Large_Tile_Floor");
            GB_Large_Tile_Floor.Tool.Add("GB_Parchment_Tool");
            GB_Large_Tile_Floor.RequiredItems.Add("Stone", 10, true);
            GB_Large_Tile_Floor.RequiredItems.Add("Flint", 4, true);
            GB_Large_Tile_Floor.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Large_Tile_Floor.Category.Set("Stone Building");

            BuildPiece GB_Stone_Tile_4x4 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Tile_4x4");
            GB_Stone_Tile_4x4.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Tile_4x4.RequiredItems.Add("Stone", 6, true);
            GB_Stone_Tile_4x4.RequiredItems.Add("Flint", 3, true);
            GB_Stone_Tile_4x4.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Tile_4x4.Category.Set("Stone Building");

            BuildPiece GB_Stone_Tile = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Tile");
            GB_Stone_Tile.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Tile.RequiredItems.Add("Stone", 4, true);
            GB_Stone_Tile.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Tile.Category.Set("Stone Building");

            BuildPiece GB_Stone_Tile_1x1 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Tile_1x1");
            GB_Stone_Tile_1x1.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Tile_1x1.RequiredItems.Add("Stone", 2, true);
            GB_Stone_Tile_1x1.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Tile_1x1.Category.Set("Stone Building");

            BuildPiece GB_Stone_Round_Tile = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Round_Tile");
            GB_Stone_Round_Tile.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Round_Tile.RequiredItems.Add("Stone", 4, true);
            GB_Stone_Round_Tile.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Round_Tile.Category.Set("Stone Building");

            BuildPiece GB_Large_Solid_Wood_Floor = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Large_Solid_Wood_Floor");
            GB_Large_Solid_Wood_Floor.Tool.Add("GB_Parchment_Tool");
            GB_Large_Solid_Wood_Floor.RequiredItems.Add("Wood", 16, true);
            GB_Large_Solid_Wood_Floor.RequiredItems.Add("FineWood", 6, true);
            GB_Large_Solid_Wood_Floor.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Large_Solid_Wood_Floor.Category.Set("Wood Building");

            BuildPiece GB_Solid_Wood_Tile_4x4 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Solid_Wood_Tile_4x4");
            GB_Solid_Wood_Tile_4x4.Tool.Add("GB_Parchment_Tool");
            GB_Solid_Wood_Tile_4x4.RequiredItems.Add("Wood", 8, true);
            GB_Solid_Wood_Tile_4x4.RequiredItems.Add("FineWood", 4, true);
            GB_Solid_Wood_Tile_4x4.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Solid_Wood_Tile_4x4.Category.Set("Wood Building");

            BuildPiece GB_Solid_Wood_Round_Tile = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Solid_Wood_Round_Tile");
            GB_Solid_Wood_Round_Tile.Tool.Add("GB_Parchment_Tool");
            GB_Solid_Wood_Round_Tile.RequiredItems.Add("Wood", 4, true);
            GB_Solid_Wood_Round_Tile.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Solid_Wood_Round_Tile.Category.Set("Wood Building");

            BuildPiece GB_Solid_Wood_Tile = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Solid_Wood_Tile");
            GB_Solid_Wood_Tile.Tool.Add("GB_Parchment_Tool");
            GB_Solid_Wood_Tile.RequiredItems.Add("Wood", 4, true);
            GB_Solid_Wood_Tile.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Solid_Wood_Tile.Category.Set("Wood Building");

            BuildPiece GB_Solid_Wood_Tile_1x1 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Solid_Wood_Tile_1x1");
            GB_Solid_Wood_Tile_1x1.Tool.Add("GB_Parchment_Tool");
            GB_Solid_Wood_Tile_1x1.RequiredItems.Add("Wood", 2, true);
            GB_Solid_Wood_Tile_1x1.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Solid_Wood_Tile_1x1.Category.Set("Wood Building");

            BuildPiece GB_Wood_Tile = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Wood_Tile");
            GB_Wood_Tile.Tool.Add("GB_Parchment_Tool");
            GB_Wood_Tile.RequiredItems.Add("Wood", 4, true);
            GB_Wood_Tile.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Wood_Tile.Category.Set("Wood Building");

            BuildPiece GB_Wood_1x1_Floor = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Wood_1x1_Floor");
            GB_Wood_1x1_Floor.Tool.Add("GB_Parchment_Tool");
            GB_Wood_1x1_Floor.RequiredItems.Add("Wood", 2, true);
            GB_Wood_1x1_Floor.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Wood_1x1_Floor.Category.Set("Wood Building");

            BuildPiece GB_Stone_Arch = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Arch");
            GB_Stone_Arch.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Arch.RequiredItems.Add("Stone", 5, true);
            GB_Stone_Arch.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Arch.Category.Set("Stone Building");

            BuildPiece GB_Stone_Doorframe = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Doorframe");
            GB_Stone_Doorframe.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Doorframe.RequiredItems.Add("Stone", 10, true);
            GB_Stone_Doorframe.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Doorframe.Category.Set("Stone Building");

            BuildPiece GB_StoneWall = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_StoneWall");
            GB_StoneWall.Tool.Add("GB_Parchment_Tool");
            GB_StoneWall.RequiredItems.Add("Stone", 4, true);
            GB_StoneWall.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_StoneWall.Category.Set("Stone Building");

            BuildPiece GB_Stone_Aperture = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Aperture");
            GB_Stone_Aperture.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Aperture.RequiredItems.Add("Stone", 6, true);
            GB_Stone_Aperture.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Aperture.Category.Set("Stone Building");

            BuildPiece GB_Stone_Halfwall_Aperture = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Halfwall_Aperture");
            GB_Stone_Halfwall_Aperture.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Halfwall_Aperture.RequiredItems.Add("Stone", 4, true);
            GB_Stone_Halfwall_Aperture.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Halfwall_Aperture.Category.Set("Stone Building");

            BuildPiece GB_Stone_Halfwall = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Halfwall");
            GB_Stone_Halfwall.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Halfwall.RequiredItems.Add("Stone", 2, true);
            GB_Stone_Halfwall.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Halfwall.Category.Set("Stone Building");

            BuildPiece GB_Stone_Halfwall_Arch = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Halfwall_Arch");
            GB_Stone_Halfwall_Arch.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Halfwall_Arch.RequiredItems.Add("Stone", 2, true);
            GB_Stone_Halfwall_Arch.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Halfwall_Arch.Category.Set("Stone Building");

            BuildPiece GB_Wooden_Ladder = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Wooden_Ladder");
            GB_Wooden_Ladder.Tool.Add("GB_Parchment_Tool");
            GB_Wooden_Ladder.RequiredItems.Add("RoundLog", 10, true);
            GB_Wooden_Ladder.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Wooden_Ladder.Category.Set("Wood Building");

            BuildPiece GB_Small_Wooden_Ladder = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Small_Wooden_Ladder");
            GB_Small_Wooden_Ladder.Tool.Add("GB_Parchment_Tool");
            GB_Small_Wooden_Ladder.RequiredItems.Add("RoundLog", 10, true);
            GB_Small_Wooden_Ladder.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Small_Wooden_Ladder.Category.Set("Wood Building");

            BuildPiece GB_Black_Curtains = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Black_Curtains");
            GB_Black_Curtains.Tool.Add("GB_Parchment_Tool");
            GB_Black_Curtains.RequiredItems.Add("Bronze", 1, true);
            GB_Black_Curtains.RequiredItems.Add("LeatherScraps", 6, true);
            GB_Black_Curtains.RequiredItems.Add("Coal", 4, true);
            GB_Black_Curtains.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Black_Curtains.Category.Set("Deco");

            BuildPiece GB_Single_Candle_1 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Single_Candle_1");
            GB_Single_Candle_1.Tool.Add("GB_Parchment_Tool");
            GB_Single_Candle_1.RequiredItems.Add("Honey", 2, true);
            GB_Single_Candle_1.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Single_Candle_1.Category.Set("Lights and Storage");

            BuildPiece GB_Single_Candle_2 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Single_Candle_2");
            GB_Single_Candle_2.Tool.Add("GB_Parchment_Tool");
            GB_Single_Candle_2.RequiredItems.Add("Honey", 2, true);
            GB_Single_Candle_2.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Single_Candle_2.Category.Set("Lights and Storage");

            BuildPiece GB_Candle_Stick = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Candle_Stick");
            GB_Candle_Stick.Tool.Add("GB_Parchment_Tool");
            GB_Candle_Stick.RequiredItems.Add("Honey", 5, true);
            GB_Candle_Stick.RequiredItems.Add("Resin", 5, true);
            GB_Candle_Stick.RequiredItems.Add("Tin", 1, true);
            GB_Candle_Stick.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Candle_Stick.Category.Set("Lights and Storage");

            BuildPiece GB_Standing_Candles = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Standing_Candles");
            GB_Standing_Candles.Tool.Add("GB_Parchment_Tool");
            GB_Standing_Candles.RequiredItems.Add("Honey", 5, true);
            GB_Standing_Candles.RequiredItems.Add("Resin", 5, true);
            GB_Standing_Candles.RequiredItems.Add("Tin", 2, true);
            GB_Standing_Candles.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Standing_Candles.Category.Set("Lights and Storage");

            BuildPiece GB_Leather_Chair = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Leather_Chair");
            GB_Leather_Chair.Tool.Add("GB_Parchment_Tool");
            GB_Leather_Chair.RequiredItems.Add("LeatherScraps", 5, true);
            GB_Leather_Chair.RequiredItems.Add("FineWood", 5, true);
            GB_Leather_Chair.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Leather_Chair.Category.Set("Furniture");

            BuildPiece GB_Old_Wood_Throne = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Old_Wood_Throne");
            GB_Old_Wood_Throne.Tool.Add("GB_Parchment_Tool");
            GB_Old_Wood_Throne.RequiredItems.Add("FineWood", 5, true);
            GB_Old_Wood_Throne.RequiredItems.Add("DeerHide", 5, true);
            GB_Old_Wood_Throne.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Old_Wood_Throne.Category.Set("Furniture");

            BuildPiece GB_Wood_Chair_1 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Wood_Chair_1");
            GB_Wood_Chair_1.Tool.Add("GB_Parchment_Tool");
            GB_Wood_Chair_1.RequiredItems.Add("FineWood", 5, true);
            GB_Wood_Chair_1.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Wood_Chair_1.Category.Set("Furniture");

            BuildPiece GB_Wood_Chair_2 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Wood_Chair_2");
            GB_Wood_Chair_2.Tool.Add("GB_Parchment_Tool");
            GB_Wood_Chair_2.RequiredItems.Add("FineWood", 5, true);
            GB_Wood_Chair_2.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Wood_Chair_2.Category.Set("Furniture");

            BuildPiece GB_Wood_Stool = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Wood_Stool");
            GB_Wood_Stool.Tool.Add("GB_Parchment_Tool");
            GB_Wood_Stool.RequiredItems.Add("FineWood", 4, true);
            GB_Wood_Stool.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Wood_Stool.Category.Set("Furniture");

            BuildPiece GB_Wall_Torch = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Wall_Torch");
            GB_Wall_Torch.Tool.Add("GB_Parchment_Tool");
            GB_Wall_Torch.RequiredItems.Add("Wood", 1, true);
            GB_Wall_Torch.RequiredItems.Add("Resin", 2, true);
            GB_Wall_Torch.RequiredItems.Add("Tin", 5, true);
            GB_Wall_Torch.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Wall_Torch.Category.Set("Lights and Storage");

            BuildPiece GB_Wood_Table = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Wood_Table");
            GB_Wood_Table.Tool.Add("GB_Parchment_Tool");
            GB_Wood_Table.RequiredItems.Add("FineWood", 5, true);
            GB_Wood_Table.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Wood_Table.Category.Set("Furniture");

            BuildPiece GB_Old_Wood_Table = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Old_Wood_Table");
            GB_Old_Wood_Table.Tool.Add("GB_Parchment_Tool");
            GB_Old_Wood_Table.RequiredItems.Add("Wood", 5, true);
            GB_Old_Wood_Table.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Old_Wood_Table.Category.Set("Furniture");

            BuildPiece GB_Casket_Lid = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Casket_Lid");
            GB_Casket_Lid.Tool.Add("GB_Parchment_Tool");
            GB_Casket_Lid.RequiredItems.Add("Wood", 2, true);
            GB_Casket_Lid.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Casket_Lid.Category.Set("Furniture");

            BuildPiece GB_Casket = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Casket");
            GB_Casket.Tool.Add("GB_Parchment_Tool");
            GB_Casket.RequiredItems.Add("Wood", 10, true);
            GB_Casket.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Casket.Category.Set("Furniture");

            BuildPiece GB_Barrel_O_Skulls = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Barrel_O_Skulls");
            GB_Barrel_O_Skulls.Tool.Add("GB_Parchment_Tool");
            GB_Barrel_O_Skulls.RequiredItems.Add("RoundLog", 5, true);
            GB_Barrel_O_Skulls.RequiredItems.Add("BoneFragments", 5, true);
            GB_Barrel_O_Skulls.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Barrel_O_Skulls.Category.Set("Deco");

            BuildPiece GB_Pile_O_Skulls = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Pile_O_Skulls");
            GB_Pile_O_Skulls.Tool.Add("GB_Parchment_Tool");
            GB_Pile_O_Skulls.RequiredItems.Add("BoneFragments", 5, true);
            GB_Pile_O_Skulls.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Pile_O_Skulls.Category.Set("Deco");

            BuildPiece GB_Black_Banner = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Black_Banner");
            GB_Black_Banner.Tool.Add("GB_Parchment_Tool");
            GB_Black_Banner.RequiredItems.Add("Iron", 1, true);
            GB_Black_Banner.RequiredItems.Add("Chain", 2, true);
            GB_Black_Banner.RequiredItems.Add("DeerHide", 2, true);
            GB_Black_Banner.RequiredItems.Add("Coal", 4, true);
            GB_Black_Banner.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Black_Banner.Category.Set("Deco");

            BuildPiece GB_Black_Half_Curtains = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Black_Half_Curtains");
            GB_Black_Half_Curtains.Tool.Add("GB_Parchment_Tool");
            GB_Black_Half_Curtains.RequiredItems.Add("Bronze", 1, true);
            GB_Black_Half_Curtains.RequiredItems.Add("LeatherScraps", 3, true);
            GB_Black_Half_Curtains.RequiredItems.Add("Coal", 2, true);
            GB_Black_Half_Curtains.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Black_Half_Curtains.Category.Set("Deco");

            BuildPiece GB_Book_Shelf = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Book_Shelf");
            GB_Book_Shelf.Tool.Add("GB_Parchment_Tool");
            GB_Book_Shelf.RequiredItems.Add("Wood", 5, true);
            GB_Book_Shelf.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Book_Shelf.Category.Set("Furniture");

            BuildPiece GB_Hanging_Cage = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Hanging_Cage");
            GB_Hanging_Cage.Tool.Add("GB_Parchment_Tool");
            GB_Hanging_Cage.RequiredItems.Add("Iron", 2, true);
            GB_Hanging_Cage.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Hanging_Cage.Category.Set("Furniture");

            BuildPiece GB_Hanging_Skeleton = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Hanging_Skeleton");
            GB_Hanging_Skeleton.Tool.Add("GB_Parchment_Tool");
            GB_Hanging_Skeleton.RequiredItems.Add("TrophySkeleton", 1, true);
            GB_Hanging_Skeleton.RequiredItems.Add("BoneFragments", 5, true);
            GB_Hanging_Skeleton.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Hanging_Skeleton.Category.Set("Deco");

            BuildPiece GB_Purple_Curtains = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Purple_Curtains");
            GB_Purple_Curtains.Tool.Add("GB_Parchment_Tool");
            GB_Purple_Curtains.RequiredItems.Add("Bronze", 1, true);
            GB_Purple_Curtains.RequiredItems.Add("LeatherScraps", 6, true);
            GB_Purple_Curtains.RequiredItems.Add("Raspberry", 6, true);
            GB_Purple_Curtains.RequiredItems.Add("Blueberries", 6, true);
            GB_Purple_Curtains.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Purple_Curtains.Category.Set("Furniture");

            BuildPiece GB_Purple_Half_Curtains = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Purple_Half_Curtains");
            GB_Purple_Half_Curtains.Tool.Add("GB_Parchment_Tool");
            GB_Purple_Half_Curtains.RequiredItems.Add("Bronze", 1, true);
            GB_Purple_Half_Curtains.RequiredItems.Add("LeatherScraps", 3, true);
            GB_Purple_Half_Curtains.RequiredItems.Add("Raspberry", 3, true);
            GB_Purple_Half_Curtains.RequiredItems.Add("Blueberries", 3, true);
            GB_Purple_Half_Curtains.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Purple_Half_Curtains.Category.Set("Furniture");

            BuildPiece GB_Single_Shelf = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Single_Shelf");
            GB_Single_Shelf.Tool.Add("GB_Parchment_Tool");
            GB_Single_Shelf.RequiredItems.Add("Wood", 2, true);
            GB_Single_Shelf.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Single_Shelf.Category.Set("Furniture");

            BuildPiece GB_Sitting_Skeleton = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Sitting_Skeleton");
            GB_Sitting_Skeleton.Tool.Add("GB_Parchment_Tool");
            GB_Sitting_Skeleton.RequiredItems.Add("TrophySkeleton", 1, true);
            GB_Sitting_Skeleton.RequiredItems.Add("BoneFragments", 5, true);
            GB_Sitting_Skeleton.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Sitting_Skeleton.Category.Set("Deco");

            BuildPiece GB_Skeleton_Pole = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Skeleton_Pole");
            GB_Skeleton_Pole.Tool.Add("GB_Parchment_Tool");
            GB_Skeleton_Pole.RequiredItems.Add("TrophySkeleton", 1, true);
            GB_Skeleton_Pole.RequiredItems.Add("BoneFragments", 5, true);
            GB_Skeleton_Pole.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Skeleton_Pole.Category.Set("Deco");

            BuildPiece GB_Stone_Pillar_1x1 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Pillar_1x1");
            GB_Stone_Pillar_1x1.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Pillar_1x1.RequiredItems.Add("Stone", 2, true);
            GB_Stone_Pillar_1x1.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Pillar_1x1.Category.Set("Stone Building");

            BuildPiece GB_Stone_Pillar_Base = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Pillar_Base");
            GB_Stone_Pillar_Base.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Pillar_Base.RequiredItems.Add("Stone", 5, true);
            GB_Stone_Pillar_Base.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Pillar_Base.Category.Set("Stone Building");

            BuildPiece GB_Stone_Pillar_Bottom = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Pillar_Bottom");
            GB_Stone_Pillar_Bottom.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Pillar_Bottom.RequiredItems.Add("Stone", 4, true);
            GB_Stone_Pillar_Bottom.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Pillar_Bottom.Category.Set("Stone Building");

            BuildPiece GB_Stone_Pillar_Middle = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Pillar_Middle");
            GB_Stone_Pillar_Middle.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Pillar_Middle.RequiredItems.Add("Stone", 3, true);
            GB_Stone_Pillar_Middle.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Pillar_Middle.Category.Set("Stone Building");

            BuildPiece GB_Stone_Pillar_Small = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Pillar_Small");
            GB_Stone_Pillar_Small.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Pillar_Small.RequiredItems.Add("Stone", 6, true);
            GB_Stone_Pillar_Small.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Pillar_Small.Category.Set("Stone Building");

            BuildPiece GB_Stone_Pillar_Top = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Pillar_Top");
            GB_Stone_Pillar_Top.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Pillar_Top.RequiredItems.Add("Stone", 4, true);
            GB_Stone_Pillar_Top.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Pillar_Top.Category.Set("Stone Building");

            BuildPiece GB_Stone_Pillar_Base_Round = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Pillar_Base_Round");
            GB_Stone_Pillar_Base_Round.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Pillar_Base_Round.RequiredItems.Add("Stone", 6, true);
            GB_Stone_Pillar_Base_Round.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Pillar_Base_Round.Category.Set("Stone Building");

            BuildPiece GB_Stone_Square_Pillar_Middle = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Square_Pillar_Middle");
            GB_Stone_Square_Pillar_Middle.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Square_Pillar_Middle.RequiredItems.Add("Stone", 10, true);
            GB_Stone_Square_Pillar_Middle.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Square_Pillar_Middle.Category.Set("Stone Building");

            BuildPiece GB_Stone_Table = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Table");
            GB_Stone_Table.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Table.RequiredItems.Add("Stone", 10, true);
            GB_Stone_Table.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Table.Category.Set("Furniture");

            BuildPiece GB_Wooden_Awning = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Wooden_Awning");
            GB_Wooden_Awning.Tool.Add("GB_Parchment_Tool");
            GB_Wooden_Awning.RequiredItems.Add("FineWood", 10, true);
            GB_Wooden_Awning.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Wooden_Awning.Category.Set("Wood Building");
            MaterialReplacer.RegisterGameObjectForShaderSwap(GB_Wooden_Awning.Prefab, MaterialReplacer.ShaderType.UseUnityShader);

            BuildPiece GB_Cloth_Bag = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Cloth_Bag");
            GB_Cloth_Bag.Tool.Add("GB_Parchment_Tool");
            GB_Cloth_Bag.RequiredItems.Add("LeatherScraps", 10, true);
            GB_Cloth_Bag.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Cloth_Bag.Category.Set("Lights and Storage");

            BuildPiece GB_Old_Book = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Old_Book");
            GB_Old_Book.Tool.Add("GB_Parchment_Tool");
            GB_Old_Book.RequiredItems.Add("LeatherScraps", 5, true);
            GB_Old_Book.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Old_Book.Category.Set("Deco");

            BuildPiece GB_Old_Jug = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Old_Jug");
            GB_Old_Jug.Tool.Add("GB_Parchment_Tool");
            GB_Old_Jug.RequiredItems.Add("Wood", 2, true);
            GB_Old_Jug.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Old_Jug.Category.Set("Deco");

            BuildPiece GB_Small_Bottle = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Small_Bottle");
            GB_Small_Bottle.Tool.Add("GB_Parchment_Tool");
            GB_Small_Bottle.RequiredItems.Add("Crystal", 1, true);
            GB_Small_Bottle.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Small_Bottle.Category.Set("Deco");

            BuildPiece GB_Old_Kettle = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Old_Kettle");
            GB_Old_Kettle.Tool.Add("GB_Parchment_Tool");
            GB_Old_Kettle.RequiredItems.Add("Iron", 1, true);
            GB_Old_Kettle.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Old_Kettle.Category.Set("Deco");

            BuildPiece GB_Wooden_Barrel = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Wooden_Barrel");
            GB_Wooden_Barrel.Tool.Add("GB_Parchment_Tool");
            GB_Wooden_Barrel.RequiredItems.Add("Wood", 4, true);
            GB_Wooden_Barrel.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Wooden_Barrel.Category.Set("Lights and Storage");

            BuildPiece GB_Stone_Fireplace = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Fireplace");
            GB_Stone_Fireplace.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Fireplace.RequiredItems.Add("Stone", 20, true);
            GB_Stone_Fireplace.RequiredItems.Add("Wood", 4, true);
            GB_Stone_Fireplace.RequiredItems.Add("Coal", 2, true);
            GB_Stone_Fireplace.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Fireplace.Category.Set("Lights and Storage");

            BuildPiece GB_Wooden_Bucket = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Wooden_Bucket");
            GB_Wooden_Bucket.Tool.Add("GB_Parchment_Tool");
            GB_Wooden_Bucket.RequiredItems.Add("Wood", 4, true);
            GB_Wooden_Bucket.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Wooden_Bucket.Category.Set("Deco");

            BuildPiece GB_Old_Open_Book = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Old_Open_Book");
            GB_Old_Open_Book.Tool.Add("GB_Parchment_Tool");
            GB_Old_Open_Book.RequiredItems.Add("LeatherScraps", 5, true);
            GB_Old_Open_Book.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Old_Open_Book.Category.Set("Deco");

            BuildPiece GB_Stone_Beam = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Beam");
            GB_Stone_Beam.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Beam.RequiredItems.Add("Stone", 3, true);
            GB_Stone_Beam.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Beam.Category.Set("Stone Building");

            BuildPiece GB_Stone_Pole = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Pole");
            GB_Stone_Pole.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Pole.RequiredItems.Add("Stone", 3, true);
            GB_Stone_Pole.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Pole.Category.Set("Stone Building");

            BuildPiece GB_Stone_Pole_Small = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Pole_Small");
            GB_Stone_Pole_Small.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Pole_Small.RequiredItems.Add("Stone", 3, true);
            GB_Stone_Pole_Small.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Pole_Small.Category.Set("Stone Building");

            BuildPiece GB_Stone_Beam_Small = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Beam_Small");
            GB_Stone_Beam_Small.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Beam_Small.RequiredItems.Add("Stone", 3, true);
            GB_Stone_Beam_Small.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Beam_Small.Category.Set("Stone Building");

            BuildPiece GB_Stone_Beam_26 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Beam_26");
            GB_Stone_Beam_26.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Beam_26.RequiredItems.Add("Stone", 3, true);
            GB_Stone_Beam_26.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Beam_26.Category.Set("Stone Building");

            BuildPiece GB_Stone_Beam_45 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Beam_45");
            GB_Stone_Beam_45.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Beam_45.RequiredItems.Add("Stone", 3, true);
            GB_Stone_Beam_45.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Beam_45.Category.Set("Stone Building");

            BuildPiece GB_Stone_Wall_26 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Wall_26");
            GB_Stone_Wall_26.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Wall_26.RequiredItems.Add("Stone", 3, true);
            GB_Stone_Wall_26.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Wall_26.Category.Set("Stone Building");

            BuildPiece GB_Stone_Wall_45 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Wall_45");
            GB_Stone_Wall_45.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Wall_45.RequiredItems.Add("Stone", 3, true);
            GB_Stone_Wall_45.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Wall_45.Category.Set("Stone Building");

            BuildPiece GB_Stone_Wall_Invert_26 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Wall_Invert_26");
            GB_Stone_Wall_Invert_26.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Wall_Invert_26.RequiredItems.Add("Stone", 3, true);
            GB_Stone_Wall_Invert_26.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Wall_Invert_26.Category.Set("Stone Building");

            BuildPiece GB_Stone_Wall_Invert_45 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Wall_Invert_45");
            GB_Stone_Wall_Invert_45.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Wall_Invert_45.RequiredItems.Add("Stone", 3, true);
            GB_Stone_Wall_Invert_45.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Wall_Invert_45.Category.Set("Stone Building");

            BuildPiece GB_Large_Stone_Aperture = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Large_Stone_Aperture");
            GB_Large_Stone_Aperture.Tool.Add("GB_Parchment_Tool");
            GB_Large_Stone_Aperture.RequiredItems.Add("Stone", 6, true);
            GB_Large_Stone_Aperture.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Large_Stone_Aperture.Category.Set("Stone Building");

            BuildPiece GB_Large_StoneWall = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Large_StoneWall");
            GB_Large_StoneWall.Tool.Add("GB_Parchment_Tool");
            GB_Large_StoneWall.RequiredItems.Add("Stone", 10, true);
            GB_Large_StoneWall.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Large_StoneWall.Category.Set("Stone Building");

            BuildPiece GB_Stone_Halfwall_Battlement = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Halfwall_Battlement");
            GB_Stone_Halfwall_Battlement.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Halfwall_Battlement.RequiredItems.Add("Stone", 3, true);
            GB_Stone_Halfwall_Battlement.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Halfwall_Battlement.Category.Set("Stone Building");

            BuildPiece GB_StoneWall_Door = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_StoneWall_Door");
            GB_StoneWall_Door.Tool.Add("GB_Parchment_Tool");
            GB_StoneWall_Door.RequiredItems.Add("Stone", 6, true);
            GB_StoneWall_Door.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_StoneWall_Door.Category.Set("Wood Building");

            BuildPiece GB_Large_StoneWall_Door = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Large_StoneWall_Door");
            GB_Large_StoneWall_Door.Tool.Add("GB_Parchment_Tool");
            GB_Large_StoneWall_Door.RequiredItems.Add("Stone", 10, true);
            GB_Large_StoneWall_Door.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Large_StoneWall_Door.Category.Set("Stone Building");

            BuildPiece GB_Wall_Shelf = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Wall_Shelf");
            GB_Wall_Shelf.Tool.Add("GB_Parchment_Tool");
            GB_Wall_Shelf.RequiredItems.Add("Wood", 6, true);
            GB_Wall_Shelf.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Wall_Shelf.Category.Set("Furniture");

            BuildPiece GB_Rectangle_Rug = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Rectangle_Rug");
            GB_Rectangle_Rug.Tool.Add("GB_Parchment_Tool");
            GB_Rectangle_Rug.RequiredItems.Add("DeerHide", 6, true);
            GB_Rectangle_Rug.RequiredItems.Add("Coal", 2, true);
            GB_Rectangle_Rug.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Rectangle_Rug.Category.Set("Furniture");

            BuildPiece GB_Round_Rug = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Round_Rug");
            GB_Round_Rug.Tool.Add("GB_Parchment_Tool");
            GB_Round_Rug.RequiredItems.Add("DeerHide", 2, true);
            GB_Round_Rug.RequiredItems.Add("Coal", 2, true);
            GB_Round_Rug.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Round_Rug.Category.Set("Furniture");

            BuildPiece GB_Oval_Rug = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Oval_Rug");
            GB_Oval_Rug.Tool.Add("GB_Parchment_Tool");
            GB_Oval_Rug.RequiredItems.Add("DeerHide", 4, true);
            GB_Oval_Rug.RequiredItems.Add("Coal", 2, true);
            GB_Oval_Rug.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Oval_Rug.Category.Set("Furniture");

            BuildPiece GB_Rug_Section = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Rug_Section");
            GB_Rug_Section.Tool.Add("GB_Parchment_Tool");
            GB_Rug_Section.RequiredItems.Add("DeerHide", 4, true);
            GB_Rug_Section.RequiredItems.Add("Coal", 2, true);
            GB_Rug_Section.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Rug_Section.Category.Set("Furniture");

            BuildPiece GB_Rug_End = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Rug_End");
            GB_Rug_End.Tool.Add("GB_Parchment_Tool");
            GB_Rug_End.RequiredItems.Add("DeerHide", 1, true);
            GB_Rug_End.RequiredItems.Add("Coal", 1, true);
            GB_Rug_End.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Rug_End.Category.Set("Furniture");

            BuildPiece GB_Stone_Stairs = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Stairs");
            GB_Stone_Stairs.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Stairs.RequiredItems.Add("Stone", 8, true);
            GB_Stone_Stairs.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Stairs.Category.Set("Stone Building");

            BuildPiece GB_Wood_Stairs = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Wood_Stairs");
            GB_Wood_Stairs.Tool.Add("GB_Parchment_Tool");
            GB_Wood_Stairs.RequiredItems.Add("Wood", 8, true);
            GB_Wood_Stairs.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Wood_Stairs.Category.Set("Wood Building");

            BuildPiece GB_Rock_Pole_Small = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Rock_Pole_Small");
            GB_Rock_Pole_Small.Tool.Add("GB_Parchment_Tool");
            GB_Rock_Pole_Small.RequiredItems.Add("Stone", 2, true);
            GB_Rock_Pole_Small.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Rock_Pole_Small.Category.Set("Stone Building");

            BuildPiece GB_Rock_Pole = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Rock_Pole");
            GB_Rock_Pole.Tool.Add("GB_Parchment_Tool");
            GB_Rock_Pole.RequiredItems.Add("Stone", 4, true);
            GB_Rock_Pole.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Rock_Pole.Category.Set("Stone Building");

            BuildPiece GB_Rock_Beam = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Rock_Beam");
            GB_Rock_Beam.Tool.Add("GB_Parchment_Tool");
            GB_Rock_Beam.RequiredItems.Add("Stone", 4, true);
            GB_Rock_Beam.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Rock_Beam.Category.Set("Stone Building");

            BuildPiece GB_Rock_Beam_Small = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Rock_Beam_Small");
            GB_Rock_Beam_Small.Tool.Add("GB_Parchment_Tool");
            GB_Rock_Beam_Small.RequiredItems.Add("Stone", 2, true);
            GB_Rock_Beam_Small.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Rock_Beam_Small.Category.Set("Stone Building");

            BuildPiece GB_Large_Stone_Orn = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Large_Stone_Orn");
            GB_Large_Stone_Orn.Tool.Add("GB_Parchment_Tool");
            GB_Large_Stone_Orn.RequiredItems.Add("Stone", 6, true);
            GB_Large_Stone_Orn.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Large_Stone_Orn.Category.Set("Stone Building");

            BuildPiece GB_Small_Wall_Orn = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Small_Wall_Orn");
            GB_Small_Wall_Orn.Tool.Add("GB_Parchment_Tool");
            GB_Small_Wall_Orn.RequiredItems.Add("Stone", 2, true);
            GB_Small_Wall_Orn.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Small_Wall_Orn.Category.Set("Stone Building");

            BuildPiece GB_Wooden_Door = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Wooden_Door");
            GB_Wooden_Door.Tool.Add("GB_Parchment_Tool");
            GB_Wooden_Door.RequiredItems.Add("Wood", 12, true);
            GB_Wooden_Door.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Wooden_Door.Category.Set("Wood Building");

            BuildPiece GB_Glass_Door = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Glass_Door");
            GB_Glass_Door.Tool.Add("GB_Parchment_Tool");
            GB_Glass_Door.RequiredItems.Add("Wood", 6, true);
            GB_Glass_Door.RequiredItems.Add("Crystal", 2, true);
            GB_Glass_Door.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Glass_Door.Category.Set("Wood Building");

            BuildPiece GB_Glass_Door_OdinPlus = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Glass_Door_OdinPlus");
            GB_Glass_Door_OdinPlus.Tool.Add("GB_Parchment_Tool");
            GB_Glass_Door_OdinPlus.RequiredItems.Add("Wood", 6, true);
            GB_Glass_Door_OdinPlus.RequiredItems.Add("Crystal", 2, true);
            GB_Glass_Door_OdinPlus.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Glass_Door_OdinPlus.Category.Set("Wood Building");

            BuildPiece GB_Iron_Floor_Grate = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Iron_Floor_Grate");
            GB_Iron_Floor_Grate.Tool.Add("GB_Parchment_Tool");
            GB_Iron_Floor_Grate.RequiredItems.Add("Iron", 12, true);
            GB_Iron_Floor_Grate.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Iron_Floor_Grate.Category.Set("Deco");

            BuildPiece GB_Castle_Rope_Fence = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Castle_Rope_Fence");
            GB_Castle_Rope_Fence.Tool.Add("GB_Parchment_Tool");
            GB_Castle_Rope_Fence.RequiredItems.Add("Wood", 4, true);
            GB_Castle_Rope_Fence.RequiredItems.Add("LinenThread", 2, true);
            GB_Castle_Rope_Fence.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Castle_Rope_Fence.Category.Set("Wood Building");

            BuildPiece GB_Iron_Fence = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Iron_Fence");
            GB_Iron_Fence.Tool.Add("GB_Parchment_Tool");
            GB_Iron_Fence.RequiredItems.Add("Iron", 12, true);
            GB_Iron_Fence.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Iron_Fence.Category.Set("Deco");

            BuildPiece GB_Iron_Fence_End = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Iron_Fence_End");
            GB_Iron_Fence_End.Tool.Add("GB_Parchment_Tool");
            GB_Iron_Fence_End.RequiredItems.Add("Iron", 8, true);
            GB_Iron_Fence_End.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Iron_Fence_End.Category.Set("Deco");

            BuildPiece GB_Iron_Fence_Door = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Iron_Fence_Door");
            GB_Iron_Fence_Door.Tool.Add("GB_Parchment_Tool");
            GB_Iron_Fence_Door.RequiredItems.Add("Iron", 12, true);
            GB_Iron_Fence_Door.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Iron_Fence_Door.Category.Set("Deco");

            BuildPiece GB_Large_Chest = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Large_Chest");
            GB_Large_Chest.Tool.Add("GB_Parchment_Tool");
            GB_Large_Chest.RequiredItems.Add("Wood", 12, true);
            GB_Large_Chest.RequiredItems.Add("Obsidian", 4, true);
            GB_Large_Chest.RequiredItems.Add("Bronze", 2, true);
            GB_Large_Chest.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Large_Chest.Category.Set("Lights and Storage");

            BuildPiece GB_Small_Chest = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Small_Chest");
            GB_Small_Chest.Tool.Add("GB_Parchment_Tool");
            GB_Small_Chest.RequiredItems.Add("Wood", 8, true);
            GB_Small_Chest.RequiredItems.Add("Bronze", 1, true);
            GB_Small_Chest.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Small_Chest.Category.Set("Lights and Storage");

            BuildPiece GB_Red_Jute_Bed = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Red_Jute_Bed");
            GB_Red_Jute_Bed.Tool.Add("GB_Parchment_Tool");
            GB_Red_Jute_Bed.RequiredItems.Add("FineWood", 10, true);
            GB_Red_Jute_Bed.RequiredItems.Add("JuteRed", 2, true);
            GB_Red_Jute_Bed.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Red_Jute_Bed.Category.Set(BuildPieceCategory.Furniture);

            BuildPiece GB_Black_Cloth_Bed = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Black_Cloth_Bed");
            GB_Black_Cloth_Bed.Tool.Add("GB_Parchment_Tool");
            GB_Black_Cloth_Bed.RequiredItems.Add("FineWood", 10, true);
            GB_Black_Cloth_Bed.RequiredItems.Add("Tar", 2, true);
            GB_Black_Cloth_Bed.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Black_Cloth_Bed.Category.Set(BuildPieceCategory.Furniture);

            BuildPiece GB_Stone_RoundWall = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_RoundWall");
            GB_Stone_RoundWall.Tool.Add("GB_Parchment_Tool");
            GB_Stone_RoundWall.RequiredItems.Add("Stone", 12, true);
            GB_Stone_RoundWall.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_RoundWall.Category.Set("Stone Building");

            BuildPiece GB_Stone_RoundWall_Door = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_RoundWall_Door");
            GB_Stone_RoundWall_Door.Tool.Add("GB_Parchment_Tool");
            GB_Stone_RoundWall_Door.RequiredItems.Add("Stone", 12, true);
            GB_Stone_RoundWall_Door.RequiredItems.Add("Wood", 6, true);
            GB_Stone_RoundWall_Door.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_RoundWall_Door.Category.Set("Stone Building");

            BuildPiece GB_Stone_RoundWall_DoorFrame = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_RoundWall_DoorFrame");
            GB_Stone_RoundWall_DoorFrame.Tool.Add("GB_Parchment_Tool");
            GB_Stone_RoundWall_DoorFrame.RequiredItems.Add("Stone", 10, true);
            GB_Stone_RoundWall_DoorFrame.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_RoundWall_DoorFrame.Category.Set("Stone Building");

            BuildPiece GB_Stone_RoundWall_Third = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_RoundWall_Third");
            GB_Stone_RoundWall_Third.Tool.Add("GB_Parchment_Tool");
            GB_Stone_RoundWall_Third.RequiredItems.Add("Stone", 6, true);
            GB_Stone_RoundWall_Third.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_RoundWall_Third.Category.Set("Stone Building");

            BuildPiece GB_Stone_RoundWall_Window = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_RoundWall_Window");
            GB_Stone_RoundWall_Window.Tool.Add("GB_Parchment_Tool");
            GB_Stone_RoundWall_Window.RequiredItems.Add("Stone", 12, true);
            GB_Stone_RoundWall_Window.RequiredItems.Add("Crystal", 2, true);
            GB_Stone_RoundWall_Window.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_RoundWall_Window.Category.Set("Stone Building");

            BuildPiece GB_Stone_RoundWall_Window_OP = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_RoundWall_Window_OP");
            GB_Stone_RoundWall_Window_OP.Tool.Add("GB_Parchment_Tool");
            GB_Stone_RoundWall_Window_OP.RequiredItems.Add("Stone", 12, true);
            GB_Stone_RoundWall_Window_OP.RequiredItems.Add("Crystal", 2, true);
            GB_Stone_RoundWall_Window_OP.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_RoundWall_Window_OP.Category.Set("Stone Building");

            BuildPiece GB_Stone_Roundwall_Battlement = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Roundwall_Battlement");
            GB_Stone_Roundwall_Battlement.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Roundwall_Battlement.RequiredItems.Add("Stone", 8, true);
            GB_Stone_Roundwall_Battlement.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Roundwall_Battlement.Category.Set("Stone Building");

            BuildPiece GB_Stone_Wall_1x1 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Wall_1x1");
            GB_Stone_Wall_1x1.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Wall_1x1.RequiredItems.Add("Stone", 1, true);
            GB_Stone_Wall_1x1.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Wall_1x1.Category.Set("Stone Building");

            BuildPiece GB_Hanging_Candles = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Hanging_Candles");
            GB_Hanging_Candles.Tool.Add("GB_Parchment_Tool");
            GB_Hanging_Candles.RequiredItems.Add("Tin", 3, true);
            GB_Hanging_Candles.RequiredItems.Add("Resin", 2, true);
            GB_Hanging_Candles.RequiredItems.Add("Honey", 2, true);
            GB_Hanging_Candles.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Hanging_Candles.Category.Set("Lights and Storage");

            BuildPiece GB_Wooden_Cup = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Wooden_Cup");
            GB_Wooden_Cup.Tool.Add("GB_Parchment_Tool");
            GB_Wooden_Cup.RequiredItems.Add("FineWood", 1, true);
            GB_Wooden_Cup.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Wooden_Cup.Category.Set("Deco");

            BuildPiece GB_Wooden_Plate = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Wooden_Plate");
            GB_Wooden_Plate.Tool.Add("GB_Parchment_Tool");
            GB_Wooden_Plate.RequiredItems.Add("FineWood", 1, true);
            GB_Wooden_Plate.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Wooden_Plate.Category.Set("Deco");

            BuildPiece GB_Odin_Banner = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Odin_Banner");
            GB_Odin_Banner.Tool.Add("GB_Parchment_Tool");
            GB_Odin_Banner.RequiredItems.Add("Iron", 1, true);
            GB_Odin_Banner.RequiredItems.Add("LinenThread", 2, true);
            GB_Odin_Banner.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Odin_Banner.Category.Set("Deco");

            BuildPiece GB_Standing_Brazier = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Standing_Brazier");
            GB_Standing_Brazier.Tool.Add("GB_Parchment_Tool");
            GB_Standing_Brazier.RequiredItems.Add("Tin", 6, true);
            GB_Standing_Brazier.RequiredItems.Add("Wood", 4, true);
            GB_Standing_Brazier.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Standing_Brazier.Category.Set("Lights and Storage");

            BuildPiece GB_Standing_Torch = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Standing_Torch");
            GB_Standing_Torch.Tool.Add("GB_Parchment_Tool");
            GB_Standing_Torch.RequiredItems.Add("Tin", 4, true);
            GB_Standing_Torch.RequiredItems.Add("Wood", 2, true);
            GB_Standing_Torch.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Standing_Torch.Category.Set("Lights and Storage");

            BuildPiece GB_Tower_Roof = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Tower_Roof");
            GB_Tower_Roof.Tool.Add("GB_Parchment_Tool");
            GB_Tower_Roof.RequiredItems.Add("Stone", 5, true);
            GB_Tower_Roof.RequiredItems.Add("FineWood", 4, true);
            GB_Tower_Roof.RequiredItems.Add("Wood", 15, true);
            GB_Tower_Roof.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Tower_Roof.Category.Set("Wood Building");
            MaterialReplacer.RegisterGameObjectForShaderSwap(GB_Tower_Roof.Prefab, MaterialReplacer.ShaderType.UseUnityShader);

            BuildPiece GB_Castle_Half_Roof_26 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Castle_Half_Roof_26");
            GB_Castle_Half_Roof_26.Tool.Add("GB_Parchment_Tool");
            GB_Castle_Half_Roof_26.RequiredItems.Add("FineWood", 1, true);
            GB_Castle_Half_Roof_26.RequiredItems.Add("Wood", 2, true);
            GB_Castle_Half_Roof_26.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Castle_Half_Roof_26.Category.Set("Wood Building");
            MaterialReplacer.RegisterGameObjectForShaderSwap(GB_Castle_Half_Roof_26.Prefab, MaterialReplacer.ShaderType.UseUnityShader);

            BuildPiece GB_Castle_Half_Roof_45 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Castle_Half_Roof_45");
            GB_Castle_Half_Roof_45.Tool.Add("GB_Parchment_Tool");
            GB_Castle_Half_Roof_45.RequiredItems.Add("FineWood", 1, true);
            GB_Castle_Half_Roof_45.RequiredItems.Add("Wood", 2, true);
            GB_Castle_Half_Roof_45.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Castle_Half_Roof_45.Category.Set("Wood Building");
            MaterialReplacer.RegisterGameObjectForShaderSwap(GB_Castle_Half_Roof_45.Prefab, MaterialReplacer.ShaderType.UseUnityShader);

            BuildPiece GB_Castle_Corner_Roof_45 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Castle_Corner_Roof_45");
            GB_Castle_Corner_Roof_45.Tool.Add("GB_Parchment_Tool");
            GB_Castle_Corner_Roof_45.RequiredItems.Add("FineWood", 1, true);
            GB_Castle_Corner_Roof_45.RequiredItems.Add("Wood", 4, true);
            GB_Castle_Corner_Roof_45.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Castle_Corner_Roof_45.Category.Set("Wood Building");
            MaterialReplacer.RegisterGameObjectForShaderSwap(GB_Castle_Corner_Roof_45.Prefab, MaterialReplacer.ShaderType.UseUnityShader);

            BuildPiece GB_Castle_Inverted_Corner_Roof = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Castle_Inverted_Corner_Roof");
            GB_Castle_Inverted_Corner_Roof.Tool.Add("GB_Parchment_Tool");
            GB_Castle_Inverted_Corner_Roof.RequiredItems.Add("FineWood", 1, true);
            GB_Castle_Inverted_Corner_Roof.RequiredItems.Add("Wood", 4, true);
            GB_Castle_Inverted_Corner_Roof.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Castle_Inverted_Corner_Roof.Category.Set("Wood Building");
            MaterialReplacer.RegisterGameObjectForShaderSwap(GB_Castle_Inverted_Corner_Roof.Prefab, MaterialReplacer.ShaderType.UseUnityShader);

            BuildPiece GB_Castle_Roof_45 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Castle_Roof_45");
            GB_Castle_Roof_45.Tool.Add("GB_Parchment_Tool");
            GB_Castle_Roof_45.RequiredItems.Add("FineWood", 1, true);
            GB_Castle_Roof_45.RequiredItems.Add("Wood", 4, true);
            GB_Castle_Roof_45.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Castle_Roof_45.Category.Set("Wood Building");
            MaterialReplacer.RegisterGameObjectForShaderSwap(GB_Castle_Roof_45.Prefab, MaterialReplacer.ShaderType.UseUnityShader);


            BuildPiece GB_Castle_Roof_Top_45 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Castle_Roof_Top_45");
            GB_Castle_Roof_Top_45.Tool.Add("GB_Parchment_Tool");
            GB_Castle_Roof_Top_45.RequiredItems.Add("FineWood", 1, true);
            GB_Castle_Roof_Top_45.RequiredItems.Add("Wood", 4, true);
            GB_Castle_Roof_Top_45.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Castle_Roof_Top_45.Category.Set("Wood Building");
            MaterialReplacer.RegisterGameObjectForShaderSwap(GB_Castle_Roof_Top_45.Prefab, MaterialReplacer.ShaderType.UseUnityShader);

            BuildPiece GB_Castle_Corner_Roof_26 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Castle_Corner_Roof_26");
            GB_Castle_Corner_Roof_26.Tool.Add("GB_Parchment_Tool");
            GB_Castle_Corner_Roof_26.RequiredItems.Add("FineWood", 1, true);
            GB_Castle_Corner_Roof_26.RequiredItems.Add("Wood", 4, true);
            GB_Castle_Corner_Roof_26.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Castle_Corner_Roof_26.Category.Set("Wood Building");
            MaterialReplacer.RegisterGameObjectForShaderSwap(GB_Castle_Corner_Roof_26.Prefab, MaterialReplacer.ShaderType.UseUnityShader);

            BuildPiece GB_Castle_Inverted_Corner_Roof_26 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Castle_Inverted_Corner_Roof_26");
            GB_Castle_Inverted_Corner_Roof_26.Tool.Add("GB_Parchment_Tool");
            GB_Castle_Inverted_Corner_Roof_26.RequiredItems.Add("FineWood", 1, true);
            GB_Castle_Inverted_Corner_Roof_26.RequiredItems.Add("Wood", 4, true);
            GB_Castle_Inverted_Corner_Roof_26.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Castle_Inverted_Corner_Roof_26.Category.Set("Wood Building");
            MaterialReplacer.RegisterGameObjectForShaderSwap(GB_Castle_Inverted_Corner_Roof_26.Prefab, MaterialReplacer.ShaderType.UseUnityShader);

            BuildPiece GB_Castle_Roof_26 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Castle_Roof_26");
            GB_Castle_Roof_26.Tool.Add("GB_Parchment_Tool");
            GB_Castle_Roof_26.RequiredItems.Add("FineWood", 1, true);
            GB_Castle_Roof_26.RequiredItems.Add("Wood", 4, true);
            GB_Castle_Roof_26.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Castle_Roof_26.Category.Set("Wood Building");
            MaterialReplacer.RegisterGameObjectForShaderSwap(GB_Castle_Roof_26.Prefab, MaterialReplacer.ShaderType.UseUnityShader);

            BuildPiece GB_Castle_Roof_Top_26 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Castle_Roof_Top_26");
            GB_Castle_Roof_Top_26.Tool.Add("GB_Parchment_Tool");
            GB_Castle_Roof_Top_26.RequiredItems.Add("FineWood", 1, true);
            GB_Castle_Roof_Top_26.RequiredItems.Add("Wood", 4, true);
            GB_Castle_Roof_Top_26.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Castle_Roof_Top_26.Category.Set("Wood Building");
            MaterialReplacer.RegisterGameObjectForShaderSwap(GB_Castle_Roof_Top_26.Prefab, MaterialReplacer.ShaderType.UseUnityShader);

            BuildPiece GB_Wood_Roof_Point = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Wood_Roof_Point");
            GB_Wood_Roof_Point.Tool.Add("GB_Parchment_Tool");
            GB_Wood_Roof_Point.RequiredItems.Add("FineWood", 4, true);
            GB_Wood_Roof_Point.RequiredItems.Add("Wood", 8, true);
            GB_Wood_Roof_Point.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Wood_Roof_Point.Category.Set("Wood Building");
            MaterialReplacer.RegisterGameObjectForShaderSwap(GB_Wood_Roof_Point.Prefab, MaterialReplacer.ShaderType.UseUnityShader);

            BuildPiece GB_Stone_Railing = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Railing");
            GB_Stone_Railing.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Railing.RequiredItems.Add("Stone", 4, true);
            GB_Stone_Railing.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Railing.Category.Set("Wood Building");

            BuildPiece GB_Wood_Railing = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Wood_Railing");
            GB_Wood_Railing.Tool.Add("GB_Parchment_Tool");
            GB_Wood_Railing.RequiredItems.Add("Wood", 4, true);
            GB_Wood_Railing.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Wood_Railing.Category.Set("Wood Building");

            BuildPiece GB_Castle_Stairs_Left = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Castle_Stairs_Left");
            GB_Castle_Stairs_Left.Tool.Add("GB_Parchment_Tool");
            GB_Castle_Stairs_Left.RequiredItems.Add("Wood", 8, true);
            GB_Castle_Stairs_Left.RequiredItems.Add("Stone", 4, true);
            GB_Castle_Stairs_Left.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Castle_Stairs_Left.Category.Set("Wood Building");

            BuildPiece GB_Castle_Stairs_Right = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Castle_Stairs_Right");
            GB_Castle_Stairs_Right.Tool.Add("GB_Parchment_Tool");
            GB_Castle_Stairs_Right.RequiredItems.Add("Wood", 8, true);
            GB_Castle_Stairs_Right.RequiredItems.Add("Stone", 4, true);
            GB_Castle_Stairs_Right.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Castle_Stairs_Right.Category.Set("Wood Building");

            BuildPiece GB_Large_Cupboard = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Large_Cupboard");
            GB_Large_Cupboard.Tool.Add("GB_Parchment_Tool");
            GB_Large_Cupboard.RequiredItems.Add("FineWood", 14, true);
            GB_Large_Cupboard.RequiredItems.Add("BronzeNails", 4, true);
            GB_Large_Cupboard.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Large_Cupboard.Category.Set("Lights and Storage");

            BuildPiece GB_Stone_Railing_26 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Railing_26");
            GB_Stone_Railing_26.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Railing_26.RequiredItems.Add("Stone", 4, true);
            GB_Stone_Railing_26.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Railing_26.Category.Set("Stone Building");

            BuildPiece GB_Stone_Wall_Battlement_26 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Wall_Battlement_26");
            GB_Stone_Wall_Battlement_26.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Wall_Battlement_26.RequiredItems.Add("Stone", 4, true);
            GB_Stone_Wall_Battlement_26.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Wall_Battlement_26.Category.Set("Stone Building");

            BuildPiece GB_Wooden_Railing_26 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Wooden_Railing_26");
            GB_Wooden_Railing_26.Tool.Add("GB_Parchment_Tool");
            GB_Wooden_Railing_26.RequiredItems.Add("Wood", 4, true);
            GB_Wooden_Railing_26.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Wooden_Railing_26.Category.Set("Wood Building");

            BuildPiece GB_Stone_Hatch = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Hatch");
            GB_Stone_Hatch.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Hatch.RequiredItems.Add("Stone", 4, true);
            GB_Stone_Hatch.RequiredItems.Add("Wood", 4, true);
            GB_Stone_Hatch.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Hatch.Category.Set("Stone Building");

            BuildPiece GB_Stone_Beam_Deco_1 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Beam_Deco_1");
            GB_Stone_Beam_Deco_1.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Beam_Deco_1.RequiredItems.Add("Stone", 4, true);
            GB_Stone_Beam_Deco_1.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Beam_Deco_1.Category.Set("Deco");

            BuildPiece GB_Stone_Beam_Deco_2 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Beam_Deco_2");
            GB_Stone_Beam_Deco_2.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Beam_Deco_2.RequiredItems.Add("Stone", 4, true);
            GB_Stone_Beam_Deco_2.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Beam_Deco_2.Category.Set("Deco");

            BuildPiece GB_Broken_Barrel = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Broken_Barrel");
            GB_Broken_Barrel.Tool.Add("GB_Parchment_Tool");
            GB_Broken_Barrel.RequiredItems.Add("Wood", 4, true);
            GB_Broken_Barrel.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Broken_Barrel.Category.Set("Furniture");

            BuildPiece GB_Broken_Wood_Pile = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Broken_Wood_Pile");
            GB_Broken_Wood_Pile.Tool.Add("GB_Parchment_Tool");
            GB_Broken_Wood_Pile.RequiredItems.Add("Wood", 2, true);
            GB_Broken_Wood_Pile.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Broken_Wood_Pile.Category.Set("Deco");

            BuildPiece GB_Small_Rock_Pile = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Small_Rock_Pile");
            GB_Small_Rock_Pile.Tool.Add("GB_Parchment_Tool");
            GB_Small_Rock_Pile.RequiredItems.Add("Stone", 4, true);
            GB_Small_Rock_Pile.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Small_Rock_Pile.Category.Set("Deco");

            BuildPiece GB_Large_Rock_Pile = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Large_Rock_Pile");
            GB_Large_Rock_Pile.Tool.Add("GB_Parchment_Tool");
            GB_Large_Rock_Pile.RequiredItems.Add("Stone", 6, true);
            GB_Large_Rock_Pile.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Large_Rock_Pile.Category.Set("Deco");

            BuildPiece GB_Pile_O_Bones = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Pile_O_Bones");
            GB_Pile_O_Bones.Tool.Add("GB_Parchment_Tool");
            GB_Pile_O_Bones.RequiredItems.Add("BoneFragments", 4, true);
            GB_Pile_O_Bones.RequiredItems.Add("TrophySkeleton", 2, true);
            GB_Pile_O_Bones.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Pile_O_Bones.Category.Set("Deco");

            BuildPiece GB_Stone_Pillar_Broken_1 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Pillar_Broken_1");
            GB_Stone_Pillar_Broken_1.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Pillar_Broken_1.RequiredItems.Add("Stone", 4, true);
            GB_Stone_Pillar_Broken_1.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Pillar_Broken_1.Category.Set("Stone Building");

            BuildPiece GB_Stone_Pillar_Broken_2 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Pillar_Broken_2");
            GB_Stone_Pillar_Broken_2.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Pillar_Broken_2.RequiredItems.Add("Stone", 4, true);
            GB_Stone_Pillar_Broken_2.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Pillar_Broken_2.Category.Set("Stone Building");

            BuildPiece GB_Stone_Wall_Broken = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Wall_Broken");
            GB_Stone_Wall_Broken.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Wall_Broken.RequiredItems.Add("Stone", 2, true);
            GB_Stone_Wall_Broken.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Wall_Broken.Category.Set("Stone Building");

            BuildPiece GB_Stone_RoundWall_Corbel = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_RoundWall_Corbel");
            GB_Stone_RoundWall_Corbel.Tool.Add("GB_Parchment_Tool");
            GB_Stone_RoundWall_Corbel.RequiredItems.Add("Stone", 4, true);
            GB_Stone_RoundWall_Corbel.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_RoundWall_Corbel.Category.Set("Stone Building");

            BuildPiece GB_Wall_Corbel_1 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Wall_Corbel_1");
            GB_Wall_Corbel_1.Tool.Add("GB_Parchment_Tool");
            GB_Wall_Corbel_1.RequiredItems.Add("Stone", 1, true);
            GB_Wall_Corbel_1.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Wall_Corbel_1.Category.Set("Stone Building");

            BuildPiece GB_Wall_Corbel_2 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Wall_Corbel_2");
            GB_Wall_Corbel_2.Tool.Add("GB_Parchment_Tool");
            GB_Wall_Corbel_2.RequiredItems.Add("Stone", 1, true);
            GB_Wall_Corbel_2.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Wall_Corbel_2.Category.Set("Stone Building");

            BuildPiece GB_Tapestry_1 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Tapestry_1");
            GB_Tapestry_1.Tool.Add("GB_Parchment_Tool");
            GB_Tapestry_1.RequiredItems.Add("LinenThread", 10, true);
            GB_Tapestry_1.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Tapestry_1.Category.Set("Furniture");

            BuildPiece GB_Tapestry_2 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Tapestry_2");
            GB_Tapestry_2.Tool.Add("GB_Parchment_Tool");
            GB_Tapestry_2.RequiredItems.Add("LinenThread", 10, true);
            GB_Tapestry_2.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Tapestry_2.Category.Set("Furniture");

            BuildPiece GB_Tapestry_3 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Tapestry_3");
            GB_Tapestry_3.Tool.Add("GB_Parchment_Tool");
            GB_Tapestry_3.RequiredItems.Add("LinenThread", 10, true);
            GB_Tapestry_3.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Tapestry_3.Category.Set("Furniture");

            BuildPiece GB_Tapestry_4 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Tapestry_4");
            GB_Tapestry_4.Tool.Add("GB_Parchment_Tool");
            GB_Tapestry_4.RequiredItems.Add("LinenThread", 10, true);
            GB_Tapestry_4.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Tapestry_4.Category.Set("Furniture");

            BuildPiece GB_Stone_Chimney = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Chimney");
            GB_Stone_Chimney.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Chimney.RequiredItems.Add("Stone", 10, true);
            GB_Stone_Chimney.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Chimney.Category.Set("Stone Building");

            BuildPiece GB_Marble_Fireplace = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Marble_Fireplace");
            GB_Marble_Fireplace.Tool.Add("GB_Parchment_Tool");
            GB_Marble_Fireplace.RequiredItems.Add("BlackMarble", 5, true);
            GB_Marble_Fireplace.RequiredItems.Add("Stone", 10, true);
            GB_Marble_Fireplace.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Marble_Fireplace.Category.Set("Stone Building");

            BuildPiece GB_Castle_Wood_Beam_26 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Castle_Wood_Beam_26");
            GB_Castle_Wood_Beam_26.Tool.Add("GB_Parchment_Tool");
            GB_Castle_Wood_Beam_26.RequiredItems.Add("Wood", 2, true);
            GB_Castle_Wood_Beam_26.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Castle_Wood_Beam_26.Category.Set("Wood Building");

            BuildPiece GB_Castle_Wood_Beam_45 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Castle_Wood_Beam_45");
            GB_Castle_Wood_Beam_45.Tool.Add("GB_Parchment_Tool");
            GB_Castle_Wood_Beam_45.RequiredItems.Add("Wood", 2, true);
            GB_Castle_Wood_Beam_45.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Castle_Wood_Beam_45.Category.Set("Wood Building");

            BuildPiece GB_Castle_Wood_Beam = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Castle_Wood_Beam");
            GB_Castle_Wood_Beam.Tool.Add("GB_Parchment_Tool");
            GB_Castle_Wood_Beam.RequiredItems.Add("Wood", 2, true);
            GB_Castle_Wood_Beam.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Castle_Wood_Beam.Category.Set("Wood Building");

            BuildPiece GB_Castle_Wood_Pole = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Castle_Wood_Pole");
            GB_Castle_Wood_Pole.Tool.Add("GB_Parchment_Tool");
            GB_Castle_Wood_Pole.RequiredItems.Add("Wood", 2, true);
            GB_Castle_Wood_Pole.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Castle_Wood_Pole.Category.Set(BuildPieceCategory.Furniture);

            BuildPiece GB_Castle_Wood_Pole_Small = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Castle_Wood_Pole_Small");
            GB_Castle_Wood_Pole_Small.Tool.Add("GB_Parchment_Tool");
            GB_Castle_Wood_Pole_Small.RequiredItems.Add("Wood", 1, true);
            GB_Castle_Wood_Pole_Small.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Castle_Wood_Pole_Small.Category.Set("Wood Building");

            BuildPiece GB_Castle_Wood_Beam_Small = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Castle_Wood_Beam_Small");
            GB_Castle_Wood_Beam_Small.Tool.Add("GB_Parchment_Tool");
            GB_Castle_Wood_Beam_Small.RequiredItems.Add("Wood", 1, true);
            GB_Castle_Wood_Beam_Small.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Castle_Wood_Beam_Small.Category.Set("Wood Building");

            BuildPiece GB_Wood_Beam_Deco = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Wood_Beam_Deco");
            GB_Wood_Beam_Deco.Tool.Add("GB_Parchment_Tool");
            GB_Wood_Beam_Deco.RequiredItems.Add("Wood", 1, true);
            GB_Wood_Beam_Deco.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Wood_Beam_Deco.Category.Set("Wood Building");

            BuildPiece GB_Wood_Pole_Deco_Down = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Wood_Pole_Deco_Down");
            GB_Wood_Pole_Deco_Down.Tool.Add("GB_Parchment_Tool");
            GB_Wood_Pole_Deco_Down.RequiredItems.Add("Wood", 1, true);
            GB_Wood_Pole_Deco_Down.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Wood_Pole_Deco_Down.Category.Set("Wood Building");

            BuildPiece GB_Wood_Pole_Deco_Up = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Wood_Pole_Deco_Up");
            GB_Wood_Pole_Deco_Up.Tool.Add("GB_Parchment_Tool");
            GB_Wood_Pole_Deco_Up.RequiredItems.Add("Wood", 1, true);
            GB_Wood_Pole_Deco_Up.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Wood_Pole_Deco_Up.Category.Set("Wood Building");

            BuildPiece GB_Tower_Floor_Deco = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Tower_Floor_Deco");
            GB_Tower_Floor_Deco.Tool.Add("GB_Parchment_Tool");
            GB_Tower_Floor_Deco.RequiredItems.Add("Stone", 10, true);
            GB_Tower_Floor_Deco.RequiredItems.Add("Wood", 10, true);
            GB_Tower_Floor_Deco.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Tower_Floor_Deco.Category.Set("Deco");

            BuildPiece GB_Stone_Floor_Deco_1 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Floor_Deco_1");
            GB_Stone_Floor_Deco_1.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Floor_Deco_1.RequiredItems.Add("Stone", 4, true);
            GB_Stone_Floor_Deco_1.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Floor_Deco_1.Category.Set("Deco");

            BuildPiece GB_Stone_Floor_Deco_2 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Floor_Deco_2");
            GB_Stone_Floor_Deco_2.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Floor_Deco_2.RequiredItems.Add("Stone", 4, true);
            GB_Stone_Floor_Deco_2.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Floor_Deco_2.Category.Set("Deco");

            BuildPiece GB_Stone_Floor_Deco_3 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Floor_Deco_3");
            GB_Stone_Floor_Deco_3.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Floor_Deco_3.RequiredItems.Add("Stone", 4, true);
            GB_Stone_Floor_Deco_3.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Floor_Deco_3.Category.Set("Deco");

            BuildPiece GB_Stone_Floor_Deco_4 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Floor_Deco_4");
            GB_Stone_Floor_Deco_4.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Floor_Deco_4.RequiredItems.Add("Stone", 2, true);
            GB_Stone_Floor_Deco_4.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Floor_Deco_4.Category.Set("Deco");

            BuildPiece GB_Stone_Floor_Deco_5 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Floor_Deco_5");
            GB_Stone_Floor_Deco_5.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Floor_Deco_5.RequiredItems.Add("Stone", 1, true);
            GB_Stone_Floor_Deco_5.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Floor_Deco_5.Category.Set("Deco");

            BuildPiece GB_CobWebs = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_CobWebs");
            GB_CobWebs.Tool.Add("GB_Parchment_Tool");
            GB_CobWebs.RequiredItems.Add("LinenThread", 2, true);
            GB_CobWebs.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_CobWebs.Category.Set("Deco");

            BuildPiece GB_Castle_Rope_Door = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Castle_Rope_Door");
            GB_Castle_Rope_Door.Tool.Add("GB_Parchment_Tool");
            GB_Castle_Rope_Door.RequiredItems.Add("Wood", 2, true);
            GB_Castle_Rope_Door.RequiredItems.Add("LinenThread", 1, true);
            GB_Castle_Rope_Door.RequiredItems.Add("BronzeNails", 1, true);
            GB_Castle_Rope_Door.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Castle_Rope_Door.Category.Set("Wood Building");

            BuildPiece GB_Ship_Gate = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Ship_Gate");
            GB_Ship_Gate.Tool.Add("GB_Parchment_Tool");
            GB_Ship_Gate.RequiredItems.Add("Stone", 4, true);
            GB_Ship_Gate.RequiredItems.Add("Iron", 5, true);
            GB_Ship_Gate.RequiredItems.Add("IronNails", 2, true);
            GB_Ship_Gate.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Ship_Gate.Category.Set("Deco");

            BuildPiece GB_Castle_Glass_Roof_26 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Castle_Glass_Roof_26");
            GB_Castle_Glass_Roof_26.Tool.Add("GB_Parchment_Tool");
            GB_Castle_Glass_Roof_26.RequiredItems.Add("Crystal", 2, true);
            GB_Castle_Glass_Roof_26.RequiredItems.Add("FineWood", 2, true);
            GB_Castle_Glass_Roof_26.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Castle_Glass_Roof_26.Category.Set("Wood Building");
            MaterialReplacer.RegisterGameObjectForShaderSwap(GB_Castle_Glass_Roof_26.Prefab, MaterialReplacer.ShaderType.UseUnityShader);

            BuildPiece GB_Castle_Glass_Roof_45 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Castle_Glass_Roof_45");
            GB_Castle_Glass_Roof_45.Tool.Add("GB_Parchment_Tool");
            GB_Castle_Glass_Roof_45.RequiredItems.Add("Crystal", 2, true);
            GB_Castle_Glass_Roof_45.RequiredItems.Add("FineWood", 2, true);
            GB_Castle_Glass_Roof_45.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Castle_Glass_Roof_45.Category.Set("Wood Building");
            MaterialReplacer.RegisterGameObjectForShaderSwap(GB_Castle_Glass_Roof_45.Prefab, MaterialReplacer.ShaderType.UseUnityShader);

            BuildPiece GB_Stone_Cross = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Cross");
            GB_Stone_Cross.Tool.Add("GB_Parchment_Tool");
            GB_Stone_Cross.RequiredItems.Add("Stone", 6, true);
            GB_Stone_Cross.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Cross.Category.Set("Deco");

            BuildPiece GB_Hidden_Floor_Hatch = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Hidden_Floor_Hatch");
            GB_Hidden_Floor_Hatch.Tool.Add("GB_Parchment_Tool");
            GB_Hidden_Floor_Hatch.RequiredItems.Add("Wood", 4, true);
            GB_Hidden_Floor_Hatch.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Hidden_Floor_Hatch.Category.Set("Wood Building");

            BuildPiece GB_Hidden_Stone_Hatch = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Hidden_Stone_Hatch");
            GB_Hidden_Stone_Hatch.Tool.Add("GB_Parchment_Tool");
            GB_Hidden_Stone_Hatch.RequiredItems.Add("Stone", 4, true);
            GB_Hidden_Stone_Hatch.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Hidden_Stone_Hatch.Category.Set("Stone Building");

            BuildPiece GB_Ivy_Bush = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Ivy_Bush");
            GB_Ivy_Bush.Tool.Add("GB_Parchment_Tool");
            GB_Ivy_Bush.RequiredItems.Add("Wood", 1, true);
            GB_Ivy_Bush.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Ivy_Bush.Category.Set("Deco");
            MaterialReplacer.RegisterGameObjectForShaderSwap(GB_Ivy_Bush.Prefab, MaterialReplacer.ShaderType.VegetationShader);

            BuildPiece GB_Castle_Hearth = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Castle_Hearth");
            GB_Castle_Hearth.Tool.Add("GB_Parchment_Tool");
            GB_Castle_Hearth.RequiredItems.Add("Stone", 15, true);
            GB_Castle_Hearth.Crafting.Set(PieceManager.CraftingTable.StoneCutter);
            GB_Castle_Hearth.Category.Set("Lights and Storage");

            BuildPiece GB_Fire_Pit = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Fire_Pit");
            GB_Fire_Pit.Tool.Add("GB_Parchment_Tool");
            GB_Fire_Pit.RequiredItems.Add("Stone", 5, true);
            GB_Fire_Pit.RequiredItems.Add("Wood", 2, true);
            GB_Fire_Pit.Category.Set("Lights and Storage");

            BuildPiece GB_Small_Cupboard = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Small_Cupboard");
            GB_Small_Cupboard.Tool.Add("GB_Parchment_Tool");
            GB_Small_Cupboard.RequiredItems.Add("FineWood", 6, true);
            GB_Small_Cupboard.RequiredItems.Add("BronzeNails", 2, true);
            GB_Small_Cupboard.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Small_Cupboard.Category.Set("Lights and Storage");

            BuildPiece GB_Large_Crate = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Large_Crate");
            GB_Large_Crate.Tool.Add("GB_Parchment_Tool");
            GB_Large_Crate.RequiredItems.Add("Wood", 10, true);
            GB_Large_Crate.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Large_Crate.Category.Set("Lights and Storage");


            ItemManager.PrefabManager.RegisterPrefab("gbcastles", "sfx_openbag");


            Assembly assembly = Assembly.GetExecutingAssembly();
            Harmony harmony = new(ModGUID);
            harmony.PatchAll(assembly);



        }
    }
}



