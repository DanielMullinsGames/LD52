using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioTrackOnStart : ManagedBehaviour
{
    public string id;

    private void Start()
    {
        AudioController.Instance.SetLoopAndPlay(id);
    }
}
