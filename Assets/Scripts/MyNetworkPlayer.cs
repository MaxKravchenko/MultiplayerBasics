using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

public class MyNetworkPlayer : NetworkBehaviour
{
    [SerializeField] private TMP_Text displayNameText = null;
    [SerializeField] private Renderer displayColorRenderer = null;
    
    [SyncVar(hook = nameof(HandleDisplayNameTextUpdated))]
    [SerializeField]
    private string displayName = "Missing Name";
    
    [SyncVar(hook = nameof(HandleDisplayColorUpdated))]
    [SerializeField]
    private Color displayColor = Color.black;

    #region Server    
    
    [Server]
    public void SetDisplayName(string newDisplayName)
    {
        displayName = newDisplayName;
    }

    [Server]
    public void SetDisplayColor(Color newColor)
    {
        displayColor = newColor;
    }

    [Command]
    private void CmdSetDisplayName(string newDisplayName)
    {
        if (newDisplayName.Length < 2 || newDisplayName.Length > 20) { return; }
        
        RpcLogNewName(newDisplayName);
        
        SetDisplayName(newDisplayName);
    }

    #endregion

    #region Client
    
    private void HandleDisplayNameTextUpdated(string oldText, string newText)
    {
        displayNameText.text = newText;
    }

    private void HandleDisplayColorUpdated(Color oldColor, Color newColor)
    {
        displayColorRenderer.material.SetColor("_BaseColor", newColor);
    }

    [ContextMenu("Set My Name")]
    private void SetMyName()
    {
        CmdSetDisplayName("My New Name bigger than 20 characters");
    }

    [ClientRpc]
    private void RpcLogNewName(string newDisplayName)
    {
        Debug.Log($"New name {newDisplayName}");
    } 

    #endregion

    
}
