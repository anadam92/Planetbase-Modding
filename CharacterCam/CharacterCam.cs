using Planetbase;
using Redirection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CharacterCam
{
    public class CharacterCam : IMod
    {
        public void Init()
        {
            Redirector.PerformRedirections();
            Debug.Log("[MOD] CharacterCam activated");
        }

        public void Update() {}
    }

    public abstract class CustomCharacter : Character
    {
        [RedirectFrom(typeof(Character))]
        public override bool isCloseCameraAvailable()
        {
            return true;
        }
    }
}
