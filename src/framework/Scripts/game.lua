--[[
    Use this to test each and every function
    Filter and validate each and every parameter of lua functions
    Use this category to show case all game engine features and capabilities
    Test every thing!. Example: Pause game, restart game, initial variables, edge cases for every function
    - Test everything together:
    - camera, movement, draw
    Edge cases: 
    - Close to limits of index for 1d and 2d arrays
    - Beyong the lLimits of index for 1d and 2d arrays
    - Big negative and positive numbers 
]]

local game={
    timer = 0,
    flat_sfx_list = {
        "jump",
        "jump2",
        "roll",
        "spring1",
        "spring2",
        "booster",
        "booster2",
        "activate_power",
        "checkpoint",
        "start_game",
        "level_complete",

        "menu_click",
        "menu_intro",
        "menu_select",
        "menu_select2",
        "menu_start",
        "ui_error",
        "ui_error2",
        "ui_error_long",
        "no_entry",

        "collect_point",
        "collect_point_2",
        "collect_big_point",
        "collect_big_point_2",
        "life_gain",
        "lose_coins",
        "point_count[loop]",

        "hit_enemy_1",
        "hit_enemy_2",
        "hit_enemy_01",
        "hit_enemy_02",
        "hit_enamy_3",
        "enemy_hit01",
        "fire_hit_01",
        "fire_hit_02",
        "magic_hit",
        "magic_hit_multiple",
        "vaporise",
        "all_enemies_killed",

        "player_hit_small",
        "player_died",
        "life_lose",

        "laser_fire_multiple",
        "lazerfire1",
        "lazerfire2",
        "lazerfire3",
        "lazerfire4",
        "lazerfire5",

        "ghost_01",
        "ghost_02",
        "orc_hit01",
        "orc_hit02",
        "orc_hit03",
        "human_hit01",
        "human_hit02",
        "human_hit03",
        "human_die01",
        "human_die02",

        "walking_on_grass_fast",
        "walking_on_grass_mid",
        "walking_on_grass_single",
        "walking_on_grass_slow",
        "walk_alt01",
        "walk_alt02",
        "walk_alt03",

        "animal_vowel_1",
        "animal_vowel_2",
        "animal_vowel_3",
        "animal_vowel_4",
        "character_vowel_1",
        "character_vowel_2",
        "character_vowel_3",
        "character_vowel_4",

        "text_short",
        "text_fast",
        "text_long",

        "alarm1",
        "alarm2",
        "high_score",

        "touch01",
        "touch02",
        "touch03",
        "touch04"
    },
    sfxidx = 1
}

function game:init()
end

function game:update()
    if _btnp(_keys.Q) then     
        self.sfxidx = self.sfxidx - 1
    end
    if _btnp(_keys.E) then
        self.sfxidx = self.sfxidx + 1
    end

    self.sfxidx = clamp(1, self.sfxidx, #self.flat_sfx_list)

    if _btnp(_keys.Space) then
        _psfx(self.flat_sfx_list[self.sfxidx])
    end
end

function game:draw()
    _print(self.flat_sfx_list[self.sfxidx], 20, 20, 12, true)
end

return game