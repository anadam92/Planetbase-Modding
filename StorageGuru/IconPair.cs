using UnityEngine;

namespace StorageGuru {

    public struct IconPair {

        public Texture2D enabledIcon;
        public Texture2D disabledIcon;

        public IconPair(Texture2D enabled, Texture2D disabled) {
            enabledIcon = enabled;
            disabledIcon = disabled;
        }

    }

}
