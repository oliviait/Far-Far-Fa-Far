using System;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSwapper : MonoBehaviour
{
    public GameObject Body;
    public GameObject Head;
    public GameObject Legs;

    private SpriteRenderer bodyRenderer;
    private SpriteRenderer headRenderer;
    private SpriteRenderer legsRenderer;

    public int DEFChangeStep = 1;
    public int STRChangeStep = 1;
    public int HPChangeStep = 1;
    public int SPDChangeStep = 1;

    private SheepData data;
    public SheepSpriteGroup sheepSpriteGroup;

    private int totalDEFLevels;
    private int totalHPLevels;
    private int totalSPDLevels;

    private void Awake()
    {
        bodyRenderer = Body.GetComponent<SpriteRenderer>();
        headRenderer = Head.GetComponent<SpriteRenderer>();
        legsRenderer = Legs.GetComponent<SpriteRenderer>();
    }

    public void ChooseSprites()
    {
        data = gameObject.GetComponent<Stats>().Data;
        totalDEFLevels = sheepSpriteGroup.STR1HeadSprites.Count;
        totalHPLevels = sheepSpriteGroup.BodySprites.Count;
        totalSPDLevels = sheepSpriteGroup.LegsSprites.Count;
        
        int DEFLevel = data.DEF / DEFChangeStep;
        int STRLevel = data.STR / STRChangeStep;
        int HPLevel = data.maxHP / HPChangeStep;
        int SPDLevel = data.SPD / SPDChangeStep;
        
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
        data.headSprite = headRenderer.sprite;
        data.bodySprite = bodyRenderer.sprite;
        data.legsSprite = legsRenderer.sprite;
    }
}