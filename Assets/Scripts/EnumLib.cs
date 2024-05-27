using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class EnumLib
{
    public enum Element{Physical,Fire,Ice,Lightning};
    public enum Status{Weaken,Burn,Frozen,Paralyze,Corrosion,Corrupted,Dead};
    public enum Condition{Target,FlareGuard,};
    public enum SkillCategory{NonDamage,Damage,Counter,Charge,Responsive, UnParryable,UnDodgeable}
}