using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PowerSaver {

    public class SavingMode {
        public int trigger;
        public List<Type> typesToShutDown;

        public SavingMode(int trigger) {
            this.trigger = trigger;
            typesToShutDown = new List<Type>();
        }
    }

}
