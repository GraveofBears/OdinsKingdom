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

namespace OdinsKingdom
{
    [BepInPlugin(ModGUID, ModName, ModVersion)]
    [BepInIncompatibility("org.bepinex.plugins.valheim_plus")]

    public class OdinsKingdom : BaseUnityPlugin
    {
        private const string ModName = "OdinsKingdom";
        private const string ModVersion = "1.0.4";
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
            GB_Parchment_Tool.Crafting.Add(ItemManager.CraftingTable.MageTable, 1);
            //GB_Parchment_Tool.Name.English("Castle Blueprint Parchment");
            //GB_Parchment_Tool.Description.English("A strange paper");
            GB_Parchment_Tool.RequiredItems.Add("FineWood", 10);
            GB_Parchment_Tool.CraftAmount = 1;

            BuildPiece GB_Large_Gate = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Large_Gate", true, "GB_Parchment_Tool");
            //GB_Large_Gate.Name.English("Large Stone Door");
            //GB_Large_Gate.Description.English("A Large Stone Door");
            GB_Large_Gate.RequiredItems.Add("Stone", 10, true);
            GB_Large_Gate.RequiredItems.Add("Iron", 1, true);
            GB_Large_Gate.RequiredItems.Add("IronNails", 10, true);
            GB_Large_Gate.RequiredItems.Add("Wood", 10, true);
            GB_Large_Gate.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Large_Gate.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Large_Portcullis = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Large_Portcullis", true, "GB_Parchment_Tool");
            //GB_Large_Portcullis.Name.English("Large Stone Portcullis");
            //GB_Large_Portcullis.Description.English("A Large Stone Portcullis");
            GB_Large_Portcullis.RequiredItems.Add("Stone", 20, true);
            GB_Large_Portcullis.RequiredItems.Add("Iron", 5, true);
            GB_Large_Portcullis.RequiredItems.Add("Chain", 2, true);
            GB_Large_Portcullis.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Large_Portcullis.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_StoneWindow = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_StoneWindow", true, "GB_Parchment_Tool");
            //GB_StoneWindow.Name.English("Stone Window Wall");
            //GB_StoneWindow.Description.English("A Stone Wall with a Window");
            GB_StoneWindow.RequiredItems.Add("Stone", 5, true);
            GB_StoneWindow.RequiredItems.Add("Crystal", 5, true);
            GB_StoneWindow.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_StoneWindow.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Large_Window = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Large_Window", true, "GB_Parchment_Tool");
            //GB_Large_Window.Name.English("Large Window Wall");
            //GB_Large_Window.Description.English("A Large Stone Wall with a Window");
            GB_Large_Window.RequiredItems.Add("Stone", 10, true);
            GB_Large_Window.RequiredItems.Add("Crystal", 10, true);
            GB_Large_Window.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Large_Window.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Large_Tile_Floor = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Large_Tile_Floor", true, "GB_Parchment_Tool");
            //GB_Large_Tile_Floor.Name.English("Large Stone Tile Floor");
            //GB_Large_Tile_Floor.Description.English("A large stone tile flooring.");
            GB_Large_Tile_Floor.RequiredItems.Add("Stone", 10, true);
            GB_Large_Tile_Floor.RequiredItems.Add("Flint", 4, true);
            GB_Large_Tile_Floor.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Large_Tile_Floor.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Stone_Tile = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Tile", true, "GB_Parchment_Tool");
            //GB_Stone_Tile.Name.English("Stone Tile Flooring");
            //GB_Stone_Tile.Description.English("A stone tile flooring.");
            GB_Stone_Tile.RequiredItems.Add("Stone", 5, true);
            GB_Stone_Tile.RequiredItems.Add("Flint", 2, true);
            GB_Stone_Tile.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Tile.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Wood_Tile = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Wood_Tile", true, "GB_Parchment_Tool");
            //GB_Wood_Tile.Name.English("Wood Tile Flooring");
            //GB_Wood_Tile.Description.English("A wood tile flooring.");
            GB_Wood_Tile.RequiredItems.Add("FineWood", 2, true);
            GB_Wood_Tile.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Wood_Tile.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Wood_1x1_Floor = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Wood_1x1_Floor", true, "GB_Parchment_Tool");
            //GB_Wood_1x1_Floor.Name.English("1x1 Old Wood Floor");
            //GB_Wood_1x1_Floor.Description.English("A small wood floor.");
            GB_Wood_1x1_Floor.RequiredItems.Add("Wood", 1, true);
            GB_Wood_1x1_Floor.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Wood_1x1_Floor.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Stone_Arch = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Arch", true, "GB_Parchment_Tool");
            //GB_Stone_Arch.Name.English("Stone Arch Half");
            //GB_Stone_Arch.Description.English("A stone archway half.");
            GB_Stone_Arch.RequiredItems.Add("Stone", 5, true);
            GB_Stone_Arch.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Arch.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Stone_Doorframe = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Doorframe", true, "GB_Parchment_Tool");
            //GB_Stone_Doorframe.Name.English("Stone Doorframe");
            //GB_Stone_Doorframe.Description.English("A stone doorframe.");
            GB_Stone_Doorframe.RequiredItems.Add("Stone", 10, true);
            GB_Stone_Doorframe.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Doorframe.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_StoneWall = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_StoneWall", true, "GB_Parchment_Tool");
            //GB_StoneWall.Name.English("Rough Stone Wall");
            //GB_StoneWall.Description.English("A stone wall.");
            GB_StoneWall.RequiredItems.Add("Stone", 6, true);
            GB_StoneWall.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_StoneWall.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Stone_Aperture = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Aperture", true, "GB_Parchment_Tool");
            //GB_Stone_Aperture.Name.English("Stone Wall Aperture");
            //GB_Stone_Aperture.Description.English("A stone wall aperture.");
            GB_Stone_Aperture.RequiredItems.Add("Stone", 6, true);
            GB_Stone_Aperture.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Aperture.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Stone_Halfwall_Aperture = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Halfwall_Aperture", true, "GB_Parchment_Tool");
            //GB_Stone_Halfwall_Aperture.Name.English("Stone Halfwall Aperture");
            //GB_Stone_Halfwall_Aperture.Description.English("A stone halfwall battlement.");
            GB_Stone_Halfwall_Aperture.RequiredItems.Add("Stone", 4, true);
            GB_Stone_Halfwall_Aperture.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Halfwall_Aperture.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Stone_Halfwall = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Halfwall", true, "GB_Parchment_Tool");
            //GB_Stone_Halfwall.Name.English("Rough Stone Halfwall");
            //GB_Stone_Halfwall.Description.English("A stone halfwall.");
            GB_Stone_Halfwall.RequiredItems.Add("Stone", 4, true);
            GB_Stone_Halfwall.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Halfwall.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Stone_Beam = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Beam", true, "GB_Parchment_Tool");
            //GB_Stone_Beam.Name.English("Fancy Stone Beam");
            //GB_Stone_Beam.Description.English("A stone beam.");
            GB_Stone_Beam.RequiredItems.Add("Stone", 3, true);
            GB_Stone_Beam.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Beam.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Stone_Pole = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Pole", true, "GB_Parchment_Tool");
            //GB_Stone_Pole.Name.English("Fancy Stone Pole");
            //GB_Stone_Pole.Description.English("A stone pole.");
            GB_Stone_Pole.RequiredItems.Add("Stone", 3, true);
            GB_Stone_Pole.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Pole.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Stone_Pole_Small = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Pole_Small", true, "GB_Parchment_Tool");
            //GB_Stone_Pole_Small.Name.English("Fancy Small Stone Pole");
            //GB_Stone_Pole_Small.Description.English("A small stone pole.");
            GB_Stone_Pole_Small.RequiredItems.Add("Stone", 3, true);
            GB_Stone_Pole_Small.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Pole_Small.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Stone_Beam_Small = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Beam_Small", true, "GB_Parchment_Tool");
            //GB_Stone_Beam_Small.Name.English("Fancy Small Stone Beam");
            //GB_Stone_Beam_Small.Description.English("A small stone beam.");
            GB_Stone_Beam_Small.RequiredItems.Add("Stone", 3, true);
            GB_Stone_Beam_Small.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Beam_Small.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Wooden_Ladder = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Wooden_Ladder", true, "GB_Parchment_Tool");
            //GB_Wooden_Ladder.Name.English("Large WoodenLadder");
            //GB_Wooden_Ladder.Description.English("A wooden ladder");
            GB_Wooden_Ladder.RequiredItems.Add("RoundLog", 10, true);
            GB_Wooden_Ladder.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Wooden_Ladder.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Black_Curtains = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Black_Curtains", true, "GB_Parchment_Tool");
            //GB_Black_Curtains.Name.English("Black Curtains");
            //GB_Black_Curtains.Description.English("Black curtains.");
            GB_Black_Curtains.RequiredItems.Add("Bronze", 1, true);
            GB_Black_Curtains.RequiredItems.Add("LeatherScraps", 6, true);
            GB_Black_Curtains.RequiredItems.Add("Coal", 4, true);
            GB_Black_Curtains.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Black_Curtains.Category.Add(BuildPieceCategory.Furniture);

            BuildPiece GB_Candle_Stick = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Candle_Stick", true, "GB_Parchment_Tool");
            //GB_Candle_Stick.Name.English("Candlestick");
            //GB_Candle_Stick.Description.English("A candlestick.");
            GB_Candle_Stick.RequiredItems.Add("Honey", 5, true);
            GB_Candle_Stick.RequiredItems.Add("Resin", 5, true);
            GB_Candle_Stick.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Candle_Stick.Category.Add(BuildPieceCategory.Furniture);

            BuildPiece GB_Standing_Candles = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Standing_Candles", true, "GB_Parchment_Tool");
            //GB_Standing_Candles.Name.English("Standing Candles");
            //GB_Standing_Candles.Description.English("Standing Candles");
            GB_Standing_Candles.RequiredItems.Add("Honey", 5, true);
            GB_Standing_Candles.RequiredItems.Add("Resin", 5, true);
            GB_Standing_Candles.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Standing_Candles.Category.Add(BuildPieceCategory.Furniture);

            BuildPiece GB_Leather_Chair = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Leather_Chair", true, "GB_Parchment_Tool");
            //GB_Leather_Chair.Name.English("Black Leather Chair");
            //GB_Leather_Chair.Description.English("A black leather chair");
            GB_Leather_Chair.RequiredItems.Add("LeatherScraps", 5, true);
            GB_Leather_Chair.RequiredItems.Add("RoundLog", 5, true);
            GB_Leather_Chair.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Leather_Chair.Category.Add(BuildPieceCategory.Furniture);

            BuildPiece GB_Wall_Torch = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Wall_Torch", true, "GB_Parchment_Tool");
            //GB_Wall_Torch.Name.English("Wooden Wall Torch");
            //GB_Wall_Torch.Description.English("A wooden wall torch");
            GB_Wall_Torch.RequiredItems.Add("RoundLog", 1, true);
            GB_Wall_Torch.RequiredItems.Add("Resin", 2, true);
            GB_Wall_Torch.RequiredItems.Add("Stone", 5, true);
            GB_Wall_Torch.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Wall_Torch.Category.Add(BuildPieceCategory.Furniture);

            BuildPiece GB_Wood_Table = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Wood_Table", true, "GB_Parchment_Tool");
            //GB_Wood_Table.Name.English("Wooden Table");
            //GB_Wood_Table.Description.English("A wooden table");
            GB_Wood_Table.RequiredItems.Add("Wood", 10, true);
            GB_Wood_Table.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Wood_Table.Category.Add(BuildPieceCategory.Furniture);

            BuildPiece GB_Casket_Lid = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Casket_Lid", true, "GB_Parchment_Tool");
            //GB_Casket_Lid.Name.English("Wooden Casket Lid");
            //GB_Casket_Lid.Description.English("A wooden casket lid");
            GB_Casket_Lid.RequiredItems.Add("Wood", 2, true);
            GB_Casket_Lid.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Casket_Lid.Category.Add(BuildPieceCategory.Furniture);

            BuildPiece GB_Casket = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Casket", true, "GB_Parchment_Tool");
            //GB_Casket.Name.English("Wooden Casket");
            //GB_Casket.Description.English("A wooden casket");
            GB_Casket.RequiredItems.Add("Wood", 10, true);
            GB_Casket.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Casket.Category.Add(BuildPieceCategory.Furniture);

            BuildPiece GB_Barrel_O_Skulls = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Barrel_O_Skulls", true, "GB_Parchment_Tool");
            //GB_Barrel_O_Skulls.Name.English("Barrel O Skulls");
            //GB_Barrel_O_Skulls.Description.English("A wooden barrel of skulls");
            GB_Barrel_O_Skulls.RequiredItems.Add("RoundLog", 5, true);
            GB_Barrel_O_Skulls.RequiredItems.Add("BoneFragments", 5, true);
            GB_Barrel_O_Skulls.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Barrel_O_Skulls.Category.Add(BuildPieceCategory.Furniture);

            BuildPiece GB_Pile_O_Skulls = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Pile_O_Skulls", true, "GB_Parchment_Tool");
            //GB_Pile_O_Skulls.Name.English("Pile O Skulls");
            //GB_Pile_O_Skulls.Description.English("A pile of skulls");
            GB_Pile_O_Skulls.RequiredItems.Add("BoneFragments", 5, true);
            GB_Pile_O_Skulls.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Pile_O_Skulls.Category.Add(BuildPieceCategory.Furniture);

            BuildPiece GB_Black_Banner = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Black_Banner", true, "GB_Parchment_Tool");
            //GB_Black_Banner.Name.English("Odins Black Banner");
            //GB_Black_Banner.Description.English("OdinPlus Banner");
            GB_Black_Banner.RequiredItems.Add("Iron", 1, true);
            GB_Black_Banner.RequiredItems.Add("Chain", 2, true);
            GB_Black_Banner.RequiredItems.Add("DeerHide", 2, true);
            GB_Black_Banner.RequiredItems.Add("Coal", 4, true);
            GB_Black_Banner.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Black_Banner.Category.Add(BuildPieceCategory.Furniture);

            BuildPiece GB_Black_Half_Curtains = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Black_Half_Curtains", true, "GB_Parchment_Tool");
            //GB_Black_Half_Curtains.Name.English("Black Half Curtains");
            //GB_Black_Half_Curtains.Description.English("Black Curtains but half the size");
            GB_Black_Half_Curtains.RequiredItems.Add("Bronze", 1, true);
            GB_Black_Half_Curtains.RequiredItems.Add("LeatherScraps", 3, true);
            GB_Black_Half_Curtains.RequiredItems.Add("Coal", 2, true);
            GB_Black_Half_Curtains.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Black_Half_Curtains.Category.Add(BuildPieceCategory.Furniture);

            BuildPiece GB_Book_Shelf = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Book_Shelf", true, "GB_Parchment_Tool");
            //GB_Book_Shelf.Name.English("Old Wooden Bookshelf");
            //GB_Book_Shelf.Description.English("A place to put things.");
            GB_Book_Shelf.RequiredItems.Add("Wood", 5, true);
            GB_Book_Shelf.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Book_Shelf.Category.Add(BuildPieceCategory.Furniture);

            BuildPiece GB_Hanging_Cage = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Hanging_Cage", true, "GB_Parchment_Tool");
            //GB_Hanging_Cage.Name.English("Small Hanging Cage");
            //GB_Hanging_Cage.Description.English("A small hanging cage");
            GB_Hanging_Cage.RequiredItems.Add("Iron", 2, true);
            GB_Hanging_Cage.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Hanging_Cage.Category.Add(BuildPieceCategory.Furniture);

            BuildPiece GB_Hanging_Skeleton = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Hanging_Skeleton", true, "GB_Parchment_Tool");
            //GB_Hanging_Skeleton.Name.English("Hanging Skeleton");
            //GB_Hanging_Skeleton.Description.English("A hanging skeleton, probably Lagoshi.");
            GB_Hanging_Skeleton.RequiredItems.Add("TrophySkeleton", 1, true);
            GB_Hanging_Skeleton.RequiredItems.Add("BoneFragments", 5, true);
            GB_Hanging_Skeleton.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Hanging_Skeleton.Category.Add(BuildPieceCategory.Furniture);

            BuildPiece GB_Purple_Curtains = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Purple_Curtains", true, "GB_Parchment_Tool");
            //GB_Purple_Curtains.Name.English("Purple Curtains");
            //GB_Purple_Curtains.Description.English("A long set of purple curtains");
            GB_Purple_Curtains.RequiredItems.Add("Bronze", 1, true);
            GB_Purple_Curtains.RequiredItems.Add("LeatherScraps", 6, true);
            GB_Purple_Curtains.RequiredItems.Add("Raspberry", 6, true);
            GB_Purple_Curtains.RequiredItems.Add("Blueberries", 6, true);
            GB_Purple_Curtains.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Purple_Curtains.Category.Add(BuildPieceCategory.Furniture);

            BuildPiece GB_Purple_Half_Curtains = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Purple_Half_Curtains", true, "GB_Parchment_Tool");
            //GB_Purple_Half_Curtains.Name.English("Purple Half Curtains");
            //GB_Purple_Half_Curtains.Description.English("A set of purple half curtains");
            GB_Purple_Half_Curtains.RequiredItems.Add("Bronze", 1, true);
            GB_Purple_Half_Curtains.RequiredItems.Add("LeatherScraps", 3, true);
            GB_Purple_Half_Curtains.RequiredItems.Add("Raspberry", 3, true);
            GB_Purple_Half_Curtains.RequiredItems.Add("Blueberries", 3, true);
            GB_Purple_Half_Curtains.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Purple_Half_Curtains.Category.Add(BuildPieceCategory.Furniture);

            BuildPiece GB_Single_Shelf = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Single_Shelf", true, "GB_Parchment_Tool");
            //GB_Single_Shelf.Name.English("Old Wooden Shelf");
            //GB_Single_Shelf.Description.English("An old wooden shelf");
            GB_Single_Shelf.RequiredItems.Add("Wood", 2, true);
            GB_Single_Shelf.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Single_Shelf.Category.Add(BuildPieceCategory.Furniture);

            BuildPiece GB_Sitting_Skeleton = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Sitting_Skeleton", true, "GB_Parchment_Tool");
            //GB_Sitting_Skeleton.Name.English("Sitting Skeleton");
            //GB_Sitting_Skeleton.Description.English("A sitting skeleton, probably GraveBear");
            GB_Sitting_Skeleton.RequiredItems.Add("TrophySkeleton", 1, true);
            GB_Sitting_Skeleton.RequiredItems.Add("BoneFragments", 5, true);
            GB_Sitting_Skeleton.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Sitting_Skeleton.Category.Add(BuildPieceCategory.Furniture);

            BuildPiece GB_Stone_Pillar_Base = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Pillar_Base", true, "GB_Parchment_Tool");
            //GB_Stone_Pillar_Base.Name.English("Stone Pillar Base");
            //GB_Stone_Pillar_Base.Description.English("A stone pillar part");
            GB_Stone_Pillar_Base.RequiredItems.Add("Stone", 5, true);
            GB_Stone_Pillar_Base.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Pillar_Base.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Stone_Pillar_Bottom = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Pillar_Bottom", true, "GB_Parchment_Tool");
            //GB_Stone_Pillar_Bottom.Name.English("Stone Pillar Bottom");
            //GB_Stone_Pillar_Bottom.Description.English("A stone pillar bottom");
            GB_Stone_Pillar_Bottom.RequiredItems.Add("Stone", 4, true);
            GB_Stone_Pillar_Bottom.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Pillar_Bottom.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Stone_Pillar_Middle = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Pillar_Middle", true, "GB_Parchment_Tool");
            //GB_Stone_Pillar_Middle.Name.English("Stone Pillar Middle");
            //GB_Stone_Pillar_Middle.Description.English("A stone pillar piece");
            GB_Stone_Pillar_Middle.RequiredItems.Add("Stone", 3, true);
            GB_Stone_Pillar_Middle.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Pillar_Middle.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Stone_Pillar_Small = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Pillar_Small", true, "GB_Parchment_Tool");
            //GB_Stone_Pillar_Small.Name.English("Small Stone Pillar");
            //GB_Stone_Pillar_Small.Description.English("A small stone pillar");
            GB_Stone_Pillar_Small.RequiredItems.Add("Stone", 6, true);
            GB_Stone_Pillar_Small.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Pillar_Small.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Stone_Pillar_Top = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Pillar_Top", true, "GB_Parchment_Tool");
            //GB_Stone_Pillar_Top.Name.English("Stone Pillar Top");
            //GB_Stone_Pillar_Top.Description.English("A stone pillar top");
            GB_Stone_Pillar_Top.RequiredItems.Add("Stone", 4, true);
            GB_Stone_Pillar_Top.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Pillar_Top.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Stone_Table = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Table", true, "GB_Parchment_Tool");
            //GB_Stone_Table.Name.English("Rough Stone Table");
            //GB_Stone_Table.Description.English("A stone table");
            GB_Stone_Table.RequiredItems.Add("Stone", 10, true);
            GB_Stone_Table.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Table.Category.Add(BuildPieceCategory.Furniture);

            BuildPiece GB_Wooden_Awning = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Wooden_Awning", true, "GB_Parchment_Tool");
            //GB_Wooden_Awning.Name.English("Wooden Awning");
            //GB_Wooden_Awning.Description.English("A wooden awning");
            GB_Wooden_Awning.RequiredItems.Add("FineWood", 10, true);
            GB_Wooden_Awning.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Wooden_Awning.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Cloth_Bag = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Cloth_Bag", true, "GB_Parchment_Tool");
            //GB_Cloth_Bag.Name.English("Cloth Storage Bag");
            //GB_Cloth_Bag.Description.English("A cloth storage bag");
            GB_Cloth_Bag.RequiredItems.Add("LeatherScraps", 10, true);
            GB_Cloth_Bag.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Cloth_Bag.Category.Add(BuildPieceCategory.Misc);

            BuildPiece GB_Old_Book = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Old_Book", true, "GB_Parchment_Tool");
            //GB_Old_Book.Name.English("Old Dusty Book");
            //GB_Old_Book.Description.English("An old dusty book");
            GB_Old_Book.RequiredItems.Add("LeatherScraps", 5, true);
            GB_Old_Book.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Old_Book.Category.Add(BuildPieceCategory.Furniture);

            BuildPiece GB_Old_Jug = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Old_Jug", true, "GB_Parchment_Tool");
            //GB_Old_Jug.Name.English("Old Clay Jug");
            //GB_Old_Jug.Description.English("A clay jug");
            GB_Old_Jug.RequiredItems.Add("Wood", 2, true);
            GB_Old_Jug.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Old_Jug.Category.Add(BuildPieceCategory.Furniture);

            BuildPiece GB_Small_Bottle = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Small_Bottle", true, "GB_Parchment_Tool");
            //GB_Small_Bottle.Name.English("Small Glass Bottle");
            //GB_Small_Bottle.Description.English("A small glass bottle");
            GB_Small_Bottle.RequiredItems.Add("Crystal", 1, true);
            GB_Small_Bottle.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Small_Bottle.Category.Add(BuildPieceCategory.Furniture);

            BuildPiece GB_Wooden_Barrel = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Wooden_Barrel", true, "GB_Parchment_Tool");
            //GB_Wooden_Barrel.Name.English("Old Wooden Barrel");
            //GB_Wooden_Barrel.Description.English("A old storage barrel");
            GB_Wooden_Barrel.RequiredItems.Add("Wood", 4, true);
            GB_Wooden_Barrel.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Wooden_Barrel.Category.Add(BuildPieceCategory.Misc);

            BuildPiece GB_Stone_Fireplace = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Fireplace", true, "GB_Parchment_Tool");
            //GB_Stone_Fireplace.Name.English("Rough Stone Fireplace");
            //GB_Stone_Fireplace.Description.English("A stone fireplace");
            GB_Stone_Fireplace.RequiredItems.Add("Stone", 20, true);
            GB_Stone_Fireplace.RequiredItems.Add("Wood", 4, true);
            GB_Stone_Fireplace.RequiredItems.Add("Coal", 2, true);
            GB_Stone_Fireplace.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Fireplace.Category.Add(BuildPieceCategory.Misc);

            BuildPiece GB_Wooden_Bucket = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Wooden_Bucket", true, "GB_Parchment_Tool");
            //GB_Wooden_Bucket.Name.English("Old Wooden Bucket");
            //GB_Wooden_Bucket.Description.English("An old wooden bucket");
            GB_Wooden_Bucket.RequiredItems.Add("Wood", 4, true);
            GB_Wooden_Bucket.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Wooden_Bucket.Category.Add(BuildPieceCategory.Furniture);

            BuildPiece GB_Old_Open_Book = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Old_Open_Book", true, "GB_Parchment_Tool");
            //GB_Old_Open_Book.Name.English("Old Open Book");
            //GB_Old_Open_Book.Description.English("An old opened book");
            GB_Old_Open_Book.RequiredItems.Add("LeatherScraps", 5, true);
            GB_Old_Open_Book.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Old_Open_Book.Category.Add(BuildPieceCategory.Furniture);

            BuildPiece GB_Stone_Beam_26 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Beam_26", true, "GB_Parchment_Tool");
            //GB_Stone_Beam_26.Name.English("Rough Stone Beam 26");
            //GB_Stone_Beam_26.Description.English("A stone beam 26");
            GB_Stone_Beam_26.RequiredItems.Add("Stone", 3, true);
            GB_Stone_Beam_26.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Beam_26.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Stone_Beam_45 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Beam_45", true, "GB_Parchment_Tool");
            //GB_Stone_Beam_45.Name.English("Rough Stone Beam 45");
            //GB_Stone_Beam_45.Description.English("A stone beam 45");
            GB_Stone_Beam_45.RequiredItems.Add("Stone", 3, true);
            GB_Stone_Beam_45.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Beam_45.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Stone_Wall_26 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Wall_26", true, "GB_Parchment_Tool");
            //GB_Stone_Wall_26.Name.English("Stone Wall 26");
            //GB_Stone_Wall_26.Description.English("A stone wall 26");
            GB_Stone_Wall_26.RequiredItems.Add("Stone", 3, true);
            GB_Stone_Wall_26.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Wall_26.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Stone_Wall_45 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Wall_45", true, "GB_Parchment_Tool");
            //GB_Stone_Wall_45.Name.English("Stone Wall 45");
            //GB_Stone_Wall_45.Description.English("A stone wall 45");
            GB_Stone_Wall_45.RequiredItems.Add("Stone", 3, true);
            GB_Stone_Wall_45.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Wall_45.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Stone_Wall_Invert_26 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Wall_Invert_26", true, "GB_Parchment_Tool");
            //GB_Stone_Wall_Invert_26.Name.English("Stone Wall Inverted 26");
            //GB_Stone_Wall_Invert_26.Description.English("A stone wall inverted 26");
            GB_Stone_Wall_Invert_26.RequiredItems.Add("Stone", 3, true);
            GB_Stone_Wall_Invert_26.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Wall_Invert_26.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Stone_Wall_Invert_45 = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Wall_Invert_45", true, "GB_Parchment_Tool");
            //GB_Stone_Wall_Invert_45.Name.English("Stone Wall Inverted 45");
            //GB_Stone_Wall_Invert_45.Description.English("A stone wall inverted 45");
            GB_Stone_Wall_Invert_45.RequiredItems.Add("Stone", 3, true);
            GB_Stone_Wall_Invert_45.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Wall_Invert_45.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Large_Stone_Aperture = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Large_Stone_Aperture", true, "GB_Parchment_Tool");
            //GB_Large_Stone_Aperture.Name.English("Large Stone Aperture Wall");
            //GB_Large_Stone_Aperture.Description.English("A large stone aperture wall.");
            GB_Large_Stone_Aperture.RequiredItems.Add("Stone", 6, true);
            GB_Large_Stone_Aperture.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Large_Stone_Aperture.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Large_StoneWall = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Large_StoneWall", true, "GB_Parchment_Tool");
            //GB_Large_StoneWall.Name.English("Large StoneWall");
            //GB_Large_StoneWall.Description.English("A large stone wall");
            GB_Large_StoneWall.RequiredItems.Add("Stone", 4, true);
            GB_Large_StoneWall.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Large_StoneWall.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Stone_Halfwall_Battlement = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Halfwall_Battlement", true, "GB_Parchment_Tool");
            //GB_Stone_Halfwall_Battlement.Name.English("Stone Halfwall Battlement");
            //GB_Stone_Halfwall_Battlement.Description.English("A stone half wall battlement");
            GB_Stone_Halfwall_Battlement.RequiredItems.Add("Stone", 3, true);
            GB_Stone_Halfwall_Battlement.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Halfwall_Battlement.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_StoneWall_Door = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_StoneWall_Door", true, "GB_Parchment_Tool");
            //GB_StoneWall_Door.Name.English("Stone Wall Door");
            //GB_StoneWall_Door.Description.English("A hidden stone wall door.");
            GB_StoneWall_Door.RequiredItems.Add("Stone", 10, true);
            GB_StoneWall_Door.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_StoneWall_Door.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Wall_Shelf = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Wall_Shelf", true, "GB_Parchment_Tool");
            //GB_Wall_Shelf.Name.English("Wooden Wall Shelf");
            //GB_Wall_Shelf.Description.English("An old wooden shelf.");
            GB_Wall_Shelf.RequiredItems.Add("Wood", 10, true);
            GB_Wall_Shelf.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Wall_Shelf.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Stone_Square_Pillar_Middle = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Square_Pillar_Middle", true, "GB_Parchment_Tool");
            //GB_Stone_Square_Pillar_Middle.Name.English("Stone Square Pillar");
            //GB_Stone_Square_Pillar_Middle.Description.English("A square pillar middle.");
            GB_Stone_Square_Pillar_Middle.RequiredItems.Add("Stone", 10, true);
            GB_Stone_Square_Pillar_Middle.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Square_Pillar_Middle.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Stone_Pillar_Base_Round = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Pillar_Base_Round", true, "GB_Parchment_Tool");
            //GB_Stone_Pillar_Base_Round.Name.English("Round Stone Pillar Base");
            //GB_Stone_Pillar_Base_Round.Description.English("A round pillar base.");
            GB_Stone_Pillar_Base_Round.RequiredItems.Add("Stone", 6, true);
            GB_Stone_Pillar_Base_Round.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Pillar_Base_Round.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Rectangle_Rug = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Rectangle_Rug", true, "GB_Parchment_Tool");
            //GB_Rectangle_Rug.Name.English("Black Rectangle Rug");
            //GB_Rectangle_Rug.Description.English("A large rectangle rug.");
            GB_Rectangle_Rug.RequiredItems.Add("DeerHide", 6, true);
            GB_Rectangle_Rug.RequiredItems.Add("Coal", 2, true);
            GB_Rectangle_Rug.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Rectangle_Rug.Category.Add(BuildPieceCategory.Furniture);

            BuildPiece GB_Round_Rug = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Round_Rug", true, "GB_Parchment_Tool");
            //GB_Round_Rug.Name.English("Black Round Rug");
            //GB_Round_Rug.Description.English("A large black round rug.");
            GB_Round_Rug.RequiredItems.Add("DeerHide", 2, true);
            GB_Round_Rug.RequiredItems.Add("Coal", 2, true);
            GB_Round_Rug.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Round_Rug.Category.Add(BuildPieceCategory.Furniture);

            BuildPiece GB_Oval_Rug = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Oval_Rug", true, "GB_Parchment_Tool");
            //GB_Oval_Rug.Name.English("Black Oval Rug");
            //GB_Oval_Rug.Description.English("A large black oval rug.");
            GB_Oval_Rug.RequiredItems.Add("DeerHide", 4, true);
            GB_Oval_Rug.RequiredItems.Add("Stone", 2, true);
            GB_Oval_Rug.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Oval_Rug.Category.Add(BuildPieceCategory.Furniture);

            BuildPiece GB_Rug_Section = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Rug_Section", true, "GB_Parchment_Tool");
            //GB_Rug_Section.Name.English("Black Rug Section");
            //GB_Rug_Section.Description.English("A large section of rug.");
            GB_Rug_Section.RequiredItems.Add("DeerHide", 4, true);
            GB_Rug_Section.RequiredItems.Add("Coal", 2, true);
            GB_Rug_Section.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Rug_Section.Category.Add(BuildPieceCategory.Furniture);

            BuildPiece GB_Rug_End = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Rug_End", true, "GB_Parchment_Tool");
            //GB_Rug_End.Name.English("Black Rug End");
            //GB_Rug_End.Description.English("An end piece for run sections.");
            GB_Rug_End.RequiredItems.Add("DeerHide", 1, true);
            GB_Rug_End.RequiredItems.Add("Coal", 1, true);
            GB_Rug_End.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Rug_End.Category.Add(BuildPieceCategory.Furniture);

            BuildPiece GB_Stone_Stairs = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_Stairs", true, "GB_Parchment_Tool");
            //GB_Stone_Stairs.Name.English("Stone Staircase");
            //GB_Stone_Stairs.Description.English("A stone stars.");
            GB_Stone_Stairs.RequiredItems.Add("Stone", 8, true);
            GB_Stone_Stairs.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_Stairs.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Rock_Pole_Small = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Rock_Pole_Small", true, "GB_Parchment_Tool");
            //GB_Rock_Pole_Small.Name.English("Small Rock Pole");
            //GB_Rock_Pole_Small.Description.English("A small stack of rocks.");
            GB_Rock_Pole_Small.RequiredItems.Add("Stone", 2, true);
            GB_Rock_Pole_Small.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Rock_Pole_Small.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Rock_Pole = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Rock_Pole", true, "GB_Parchment_Tool");
            //GB_Rock_Pole.Name.English("Rock Pole");
            //GB_Rock_Pole.Description.English("A stack of rocks.");
            GB_Rock_Pole.RequiredItems.Add("Stone", 4, true);
            GB_Rock_Pole.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Rock_Pole.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Rock_Beam = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Rock_Beam", true, "GB_Parchment_Tool");
            //GB_Rock_Beam.Name.English("Rock Beam");
            //GB_Rock_Beam.Description.English("A beam of rocks.");
            GB_Rock_Beam.RequiredItems.Add("Stone", 4, true);
            GB_Rock_Beam.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Rock_Beam.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Rock_Beam_Small = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Rock_Beam_Small", true, "GB_Parchment_Tool");
            //GB_Rock_Beam_Small.Name.English("Small Rock Beam");
            //GB_Rock_Beam_Small.Description.English("A small beam of rocks.");
            GB_Rock_Beam_Small.RequiredItems.Add("Stone", 2, true);
            GB_Rock_Beam_Small.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Rock_Beam_Small.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Large_Stone_Orn = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Large_Stone_Orn", true, "GB_Parchment_Tool");
            //GB_Large_Stone_Orn.Name.English("Stone Wall Orn");
            //GB_Large_Stone_Orn.Description.English("A large stone Orn for the outside of large walls.");
            GB_Large_Stone_Orn.RequiredItems.Add("Stone", 10, true);
            GB_Large_Stone_Orn.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Large_Stone_Orn.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Wooden_Door = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Wooden_Door", true, "GB_Parchment_Tool");
            //GB_Wooden_Door.Name.English("Old Wooden Door");
            //GB_Wooden_Door.Description.English("A old wooden door.");
            GB_Wooden_Door.RequiredItems.Add("Wood", 12, true);
            GB_Wooden_Door.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Wooden_Door.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Iron_Fence = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Iron_Fence", true, "GB_Parchment_Tool");
            //GB_Iron_Fence.Name.English("Old Iron Fence");
            //GB_Iron_Fence.Description.English("A old iron fence.");
            GB_Iron_Fence.RequiredItems.Add("Iron", 12, true);
            GB_Iron_Fence.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Iron_Fence.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Iron_Fence_Door = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Iron_Fence_Door", true, "GB_Parchment_Tool");
            //GB_Iron_Fence_Door.Name.English("Old Iron Fence Door");
            //GB_Iron_Fence_Door.Description.English("A old iron fence door.");
            GB_Iron_Fence_Door.RequiredItems.Add("Iron", 12, true);
            GB_Iron_Fence_Door.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Iron_Fence_Door.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Large_Chest = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Large_Chest", true, "GB_Parchment_Tool");
            //GB_Large_Chest.Name.English("OdinPlus Large Chest");
            //GB_Large_Chest.Description.English("An odinplus large chest.");
            GB_Large_Chest.RequiredItems.Add("Wood", 12, true);
            GB_Large_Chest.RequiredItems.Add("Obsidian", 4, true);
            GB_Large_Chest.RequiredItems.Add("Bronze", 2, true);
            GB_Large_Chest.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Large_Chest.Category.Add(BuildPieceCategory.Misc);

            BuildPiece GB_Small_Chest = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Small_Chest", true, "GB_Parchment_Tool");
            //GB_Small_Chest.Name.English("OdinPlus Small Chest");
            //GB_Small_Chest.Description.English("An odinplus small chest.");
            GB_Small_Chest.RequiredItems.Add("Wood", 8, true);
            GB_Small_Chest.RequiredItems.Add("Bronze", 1, true);
            GB_Small_Chest.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Small_Chest.Category.Add(BuildPieceCategory.Misc);

            BuildPiece GB_Red_Jute_Bed = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Red_Jute_Bed", true, "GB_Parchment_Tool");
            //GB_Red_Jute_Bed.Name.English("Red Jute Bed");
            //GB_Red_Jute_Bed.Description.English("A bed with red jute covers for your castle.");
            GB_Red_Jute_Bed.RequiredItems.Add("FineWood", 10, true);
            GB_Red_Jute_Bed.RequiredItems.Add("JuteRed", 2, true);
            GB_Red_Jute_Bed.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Red_Jute_Bed.Category.Add(BuildPieceCategory.Furniture);

            BuildPiece GB_Black_Cloth_Bed = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Black_Cloth_Bed", true, "GB_Parchment_Tool");
            //GB_Black_Cloth_Bed.Name.English("Black Cloth Bed");
            //GB_Black_Cloth_Bed.Description.English("A bed with black covers for your castle.");
            GB_Black_Cloth_Bed.RequiredItems.Add("FineWood", 10, true);
            GB_Black_Cloth_Bed.RequiredItems.Add("Tar", 2, true);
            GB_Black_Cloth_Bed.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Black_Cloth_Bed.Category.Add(BuildPieceCategory.Furniture);
            /*
            BuildPiece GB_Stone_RoundWall = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_RoundWall", true, "GB_Parchment_Tool");
            //GB_Stone_RoundWall.Name.English("Stone Roundwall");
            //GB_Stone_RoundWall.Description.English("A stone roundwall.");
            GB_Stone_RoundWall.RequiredItems.Add("Stone", 12, true);
            GB_Stone_RoundWall.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_RoundWall.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Stone_RoundWall_Door = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_RoundWall_Door", true, "GB_Parchment_Tool");
            //GB_Stone_RoundWall_Door.Name.English("Stone Roundwall Door");
            //GB_Stone_RoundWall_Door.Description.English("A stone roundwall door.");
            GB_Stone_RoundWall_Door.RequiredItems.Add("Stone", 12, true);
            GB_Stone_RoundWall_Door.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_RoundWall_Door.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Stone_RoundWall_Half = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_RoundWall_Half", true, "GB_Parchment_Tool");
            //GB_Stone_RoundWall_Half.Name.English("Stone Half Roundwall");
            //GB_Stone_RoundWall_Half.Description.English("A halfwall that is rounded.");
            GB_Stone_RoundWall_Half.RequiredItems.Add("Stone", 6, true);
            GB_Stone_RoundWall_Half.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_RoundWall_Half.Category.Add(BuildPieceCategory.Building);

            BuildPiece GB_Stone_RoundWall_Window = new(PiecePrefabManager.RegisterAssetBundle("gbcastles"), "GB_Stone_RoundWall_Window", true, "GB_Parchment_Tool");
            //GB_Stone_RoundWall_Window.Name.English("Stone Roundwall Window");
            //GB_Stone_RoundWall_Window.Description.English("A stone roundwall window.");
            GB_Stone_RoundWall_Window.RequiredItems.Add("Stone", 12, true);
            GB_Stone_RoundWall_Window.Crafting.Set(PieceManager.CraftingTable.Workbench);
            GB_Stone_RoundWall_Window.Category.Add(BuildPieceCategory.Building);
            */

           

            Assembly assembly = Assembly.GetExecutingAssembly();
            Harmony harmony = new(ModGUID);
            harmony.PatchAll(assembly);

        }
    }
}



