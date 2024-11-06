using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BtnShop : BtnScreen
{
    [SerializeField] private TMP_Text _textScreen;

    public override void OnMouseDown()
    {
        base.OnMouseDown();
        UpdateScreenText();
    }

  
    private void UpdateScreenText()
    {
        _textScreen.text = $"<color=green>1000 PS";
    }
}

