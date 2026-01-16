using System.Collections.Generic;
using UnityEngine;

public class ProcGenSprite : MonoBehaviour
{
    public GameObject Body;
    public GameObject Head;
    public GameObject Legs;
    
    private SpriteRenderer bodyRenderer;
    private SpriteRenderer headRenderer;
    private SpriteRenderer legsRenderer;

    public int DEFchangeStep = 1;
    public int STRchangeStep = 1;
    public int HPchangeStep = 1;
    public int SPDchangeStep = 1;

    public SheepData data;
    public SheepSpriteGroup sheepSpriteGroup;
    
    private int totalDEFLevels;
    private int totalSTRLevels;
    private int totalHPLevels;
    private int totalSPDLevels;

    private void Start()
    {
        data = gameObject.GetComponent<Stats>().Data;
        
        bodyRenderer = Body.GetComponent<SpriteRenderer>();
        headRenderer = Head.GetComponent<SpriteRenderer>();
        legsRenderer = Legs.GetComponent<SpriteRenderer>();

        totalDEFLevels = sheepSpriteGroup.STR1HeadSprites.Count;
        totalSTRLevels = 4;
        totalHPLevels = sheepSpriteGroup.BodySprites.Count;
        totalSPDLevels = sheepSpriteGroup.LegsSprites.Count;

        // Choose sprite based on level
        int DEFLevel = data.DEF / DEFchangeStep;
        int STRLevel = data.STR / STRchangeStep;
        int HPLevel = data.maxHP / HPchangeStep;
        int SPDLevel = data.SPD / SPDchangeStep;
        // Body
        if (HPLevel < totalHPLevels) bodyRenderer.sprite = sheepSpriteGroup.BodySprites[HPLevel];
        // Legs
        if (SPDLevel < totalSPDLevels) legsRenderer.sprite = sheepSpriteGroup.LegsSprites[SPDLevel];
        // Head
        List<Sprite> headParts;
        if (STRLevel == 0) headParts = sheepSpriteGroup.STR1HeadSprites;
        else if (STRLevel == 1) headParts = sheepSpriteGroup.STR2HeadSprites;
        else if (STRLevel == 2) headParts = sheepSpriteGroup.STR3HeadSprites;
        else if (STRLevel == 3) headParts = sheepSpriteGroup.STR4HeadSprites;
        else headParts = sheepSpriteGroup.STR4HeadSprites;
        
        if (DEFLevel < totalDEFLevels) headRenderer.sprite = headParts[DEFLevel];
        
        // Set data so piece can use it in battle
        data.HeadSprite = headRenderer.sprite;
        data.BodySprite = bodyRenderer.sprite;
        data.LegsSprite = legsRenderer.sprite;
    }
}
