using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmyOptimizer.Helpers
{
    class GameImageLibrary
    {

        //library of images for the game, to be used in the UI to  display the units on the summary and army detail 

        public static Dictionary<string, string> HeroImages = new()
        {
            { "Barbarian King", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774375917/Avatar_Hero_Barbarian_King_s87uct.png" },
            { "Archer Queen", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774375916/Avatar_Hero_Archer_Queen_peli8n.png" },
            { "Grand Warden", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774375916/Avatar_Hero_Grand_Warden_quwu8o.png" },
            { "Minion Prince", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774375916/Avatar_Hero_Minion_Prince_b9nkfj.png" },
            { "Royal Champion", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774375917/Avatar_Hero_Royal_Champion_iu0z2e.png" }
        };

        public static Dictionary<string, string> TroopImages = new()
        {
            // Elixir
            { "Barbarian", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376169/Avatar_Barbarian_lwtrbq.png" },
            { "Archer", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376167/Avatar_Archer_p8fvxs.png" },
            { "Giant", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376170/Avatar_Giant_ddigmo.png" },
            { "Goblin", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376168/Avatar_Goblin_iiwpog.png" },
            { "Wall Breaker", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376174/Avatar_Wall_Breaker_z7i4jg.png" },
            { "Balloon", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376166/Avatar_Balloon_gxewyx.png" },
            { "Wizard", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376175/Avatar_Wizard_pnwv2h.png" },
            { "Healer", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376166/Avatar_Healer_ahurvo.png" },
            { "Dragon", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376166/Avatar_Dragon_rgvc1t.png" },
            { "P.E.K.K.A", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376173/Avatar_P.E.K.K.A_uiwpd6.png" },
            { "Baby Dragon", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376171/Avatar_Baby_Dragon_lano31.png" },
            { "Miner", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376172/Avatar_Miner_u50jh2.png" },
            { "Electro Dragon", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376168/Avatar_Electro_Dragon_pk9esh.png" },
            { "Yeti", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376175/Avatar_Yeti_qmbs18.png" },
            { "Dragon Rider", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376170/Avatar_Dragon_Rider_hva940.png" },
            { "Electro Titan", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376170/Avatar_Electro_Titan_lgs4ee.png" },
            { "Root Rider", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376173/Avatar_Root_Rider_wynbsd.png" },
            { "Thrower", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376174/Avatar_Thrower_ughnfy.png" },
            { "Meteor Golem", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376172/Avatar_Meteor_Golem_ecr7w8.png" },

            // Dark
            { "Minion", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376407/Avatar_Minion_miyqch.png" },
            { "Hog Rider", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376414/Avatar_Hog_Rider_gnx8mx.png" },
            { "Valkyrie", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376415/Avatar_Valkyrie_xfxukv.png" },
            { "Golem", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376413/Avatar_Golem_vef0wn.png" },
            { "Witch", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376411/Avatar_Witch_mtqi6z.png" },
            { "Lava Hound", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376410/Avatar_Lava_Hound_bok88r.png" },
            { "Bowler", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376407/Avatar_Bowler_inluv7.png" },
            { "Ice Golem", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376413/Avatar_Ice_Golem_gdaagq.png" },
            { "Headhunter", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376411/Avatar_Headhunter_mlfchb.png" },
            { "Apprentice Warden", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376406/Avatar_Apprentice_Warden_skdqyq.png" },
            { "Druid", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376416/Avatar_Druid_kxagut.png" },
            { "Furnace", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376409/Avatar_Furnace_jvxkus.png" },

            // Super
            { "Super Barbarian", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376772/Avatar_Super_Barbarian_fphb14.png" },
            { "Super Archer", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376771/Avatar_Super_Archer_zt7amw.png" },
            { "Super Giant", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376777/Avatar_Super_Giant_wetksa.png" },
            { "Sneaky Goblin", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376768/Avatar_Sneaky_Goblin_g4kro0.png" },
            { "Super Wall Breaker", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376776/Avatar_Super_Wall_Breaker_swt7rd.png" },
            { "Rocket Balloon", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376773/Avatar_Rocket_Balloon_try2b2.png" },
            { "Super Wizard", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376782/Avatar_Super_Wizard_i32ge7.png" },
            { "Super Dragon", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376775/Avatar_Super_Dragon_asqh6m.png" },
            { "Inferno Dragon", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376769/Avatar_Inferno_Dragon_frhlmg.png" },
            { "Super Minion", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376779/Avatar_Super_Minion_x1ipym.png" },
            { "Super Valkyrie", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376767/Avatar_Super_Valkyrie_ngqc64.png" },
            { "Super Witch", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376780/Avatar_Super_Witch_mpdgfd.png" },
            { "Ice Hound", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376767/Avatar_Ice_Hound_zs2pne.png" },
            { "Super Bowler", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376774/Avatar_Super_Bowler_veuaf3.png" },
            { "Super Hog Rider", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774376778/Avatar_Super_Hog_Rider_lxpiic.png" }
        };

        //spells
        public static Dictionary<string, string> SpellImages = new()
        {
            { "Lightning Spell", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774378218/Lightning_Spell_info_wd7hkz.png" },
            { "Healing Spell", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774378209/Healing_Spell_info_kobwfq.png" },
            { "Rage Spell", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774378209/Rage_Spell_info_cf9p9f.png" },
            { "Jump Spell", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774378217/Jump_Spell_info_lscait.png" },
            { "Freeze Spell", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774378213/Freeze_Spell_info_dmplps.png" },
            { "Clone Spell", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774378215/Clone_Spell_info_n8xesr.png" },
            { "Invisibility Spell", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774378216/Invisibility_Spell_info_fhqu0g.png" },
            { "Recall Spell", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774378219/Recall_Spell_info_wzchax.png" },
            { "Revive Spell", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774378213/Revive_Spell_info_rnvsny.png" },
            { "Totem Spell", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774378214/Totem_Spell_info_akrugc.png" },

            { "Poison Spell", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774379040/Poison_Spell_info_hkhqfs.png" },
            { "Earthquake Spell", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774379038/Earthquake_Spell_info_b7anqy.png" },
            { "Haste Spell", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774379031/Haste_Spell_info_ceyyeo.png" },
            { "Skeleton Spell", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774379033/Skeleton_Spell_info_nbn76m.png" },
            { "Bat Spell", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774379033/Skeleton_Spell_info_nbn76m.png" },
            { "Overgrowth Spell", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774379036/Overgrowth_Spell_info_pe6duh.png" },
            { "Ice Block Spell", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774379034/Ice_Block_Spell_info_dakpyu.png" }
        };
        //siegemachines
        public static Dictionary<string, string> SiegeImages = new()
        {
            { "Wall Wrecker", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774378760/Avatar_Wall_Wrecker_icdrng.png" },
            { "Battle Blimp", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774378758/Avatar_Battle_Blimp_g5wpna.png" },
            { "Stone Slammer", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774378757/Avatar_Stone_Slammer_up7cpv.png" },
            { "Siege Barracks", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774378761/Avatar_Siege_Barracks_xekb2e.png" },
            { "Log Launcher", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774378763/Avatar_Log_Launcher_lmgcyw.png" },
            { "Flame Flinger", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774378756/Avatar_Flame_Flinger_uok6nn.png" },
            { "Battle Drill", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774378755/Avatar_Battle_Drill_c3aa3g.png" }
        };

        public static Dictionary<string, string> Pets = new()
        {
            { "Unicorn", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774544115/Avatar_Unicorn_hreoji.png" },
            { "Spirit Fox", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774544114/Avatar_Spirit_Fox_lr4rdq.png" },
            { "Sneezy", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774544113/Avatar_Sneezy_pt72kg.png" },
            { "Poison Lizard", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774544113/Avatar_Poison_Lizard_o8u0mh.png" },
            { "Mighty Yak", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774544113/Avatar_Mighty_Yak_scwvwp.png" },
            { "Phoenix", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774544113/Avatar_Phoenix_bd2zze.png" },
            { "Greedy Raven", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774544110/Avatar_Greedy_Raven_hdghyv.png" },
            { "L.A.S.S.I", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774544110/Avatar_L.A.S.S.I_of3tye.png" },
            { "Angry Jelly", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774544109/Avatar_Angry_Jelly_zir92m.png" },
            { "Electro Owl", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774544109/Avatar_Electro_Owl_dvdzur.png" },
            { "Diggy", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774544109/Avatar_Diggy_o1uvgs.png" },
            { "Frosty", "https://res.cloudinary.com/dibrwiwx5/image/upload/v1774544109/Avatar_Frosty_gmslhm.png" }
        };
    }
}
