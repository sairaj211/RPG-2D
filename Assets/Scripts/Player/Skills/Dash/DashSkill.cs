﻿using Misc;
using UnityEngine;

namespace Player.Skills
{
    public class DashSkill : Skill
    {
        public override void UseSkill()
        {
            base.UseSkill();
            
            Debug.Log("Created clone behind");
        }
    }
}

