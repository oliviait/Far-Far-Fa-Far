using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName ="Game/SheepSpriteGroup")]
public class SheepSpriteGroup : ScriptableObject
{
    // DEF and STR
    public List<Sprite> STR1HeadSprites;
    public List<Sprite> STR2HeadSprites;
    public List<Sprite> STR3HeadSprites;
    public List<Sprite> STR4HeadSprites;
    // HP
    public List<Sprite> BodySprites;
    // SPD
    public List<Sprite> LegsSprites;
}
